using System;
using System.Collections.Generic;
using System.Globalization;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Squire's Repair, Equipment Maintenance
	/// and Refreshment Table shops.
	/// </summary>
	public static class SquireSkillHelper
	{
		private const string RepairKitClassName = "misc_repairkit_1";
		private const string WhetstoneClassName = "misc_whetstone";

		// What the client leaves in the item's string buff fields when no
		// maintenance is on it.
		private const string NoneValue = "None";

		private const float DurabilityRatePerSkillLevel = 0.05f;

		// Undercutting the smiths is the point of the shop.
		private const float RepairFeeOfSmithPrice = 0.25f;
		private const float LevelPerMaterial = 30f;
		private const float GradeWearRate = 0.1f;
		private const float ReinforceWearRate = 0.05f;
		private const float TranscendWearRate = 0.2f;
		private const int ServiceChanceDenominator = 9999;
		private const int ServiceChancePerAbilityLevel = 500;

		private const float MaintenanceRatePerSkillLevel = 0.007f;
		private const int WeaponBaseCount = 2500;
		private const int WeaponCountPerSkillLevel = 250;
		private const float WeaponCountPerStat = 0.5f;
		private const int WeaponCountPerAbilityLevel = 20;
		private const int ArmorBaseCount = 500;
		private const int ArmorCountPerSkillLevel = 50;
		private const float ArmorCountPerStat = 0.1f;
		private const int ArmorCountPerAbilityLevel = 5;

		private static readonly TimeSpan MaintenanceDuration = TimeSpan.FromHours(1);

		/// <summary>
		/// One dish a Refreshment Table can serve.
		/// </summary>
		/// <param name="ClassId">The dish's id in the client's food table.</param>
		/// <param name="BuffId">The buff eating it grants.</param>
		/// <param name="SkillLevel">The table level the dish needs.</param>
		/// <param name="AbilityId">The ability that lengthens its buff, if it has one.</param>
		/// <param name="Materials">What one serving costs the Squire.</param>
		private record Dish(int ClassId, BuffId BuffId, int SkillLevel, AbilityId? AbilityId, (string ItemClassName, int Amount)[] Materials);

		/// <summary>
		/// The dishes a Refreshment Table serves, keyed by the id the client
		/// registers and orders them by.
		/// </summary>
		private static readonly Dish[] Dishes =
		[
			new(1, BuffId.squire_food2_buff, 1, AbilityId.Squire6, [("food_033", 2), ("food_034", 4)]),
			new(2, BuffId.squire_food3_buff, 1, AbilityId.Squire7, [("food_020", 2), ("food_035", 3)]),
			new(3, BuffId.squire_food4_buff, 1, AbilityId.Squire8, [("food_035", 5)]),
			new(4, BuffId.squire_food1_buff, 1, AbilityId.Squire5, [("food_020", 6)]),
			new(5, BuffId.squire_food5_buff, 6, null, [("food_040", 3), ("food_041", 3)]),
			new(6, BuffId.squire_food6_buff, 6, null, [("food_038", 6)]),
		];

		private static readonly TimeSpan FoodBuffDuration = TimeSpan.FromMinutes(45);
		private static readonly TimeSpan FoodBuffDurationPerAbilityLevel = TimeSpan.FromMinutes(1);

		/// <summary>
		/// Returns whether the given shop services items rather than
		/// trading them.
		/// </summary>
		/// <param name="shop"></param>
		public static bool IsServiceShop(ShopData shop)
			=> shop.SkillId == SkillId.Squire_Repair || shop.SkillId == SkillId.Squire_EquipmentTouchUp;

		/// <summary>
		/// Returns whether the given shop is a Refreshment Table.
		/// </summary>
		/// <param name="shop"></param>
		public static bool IsFoodTable(ShopData shop)
			=> shop.SkillId == SkillId.Squire_FoodTable;

		/// <summary>
		/// Updates how many servings of each dish the given Squire still
		/// has the materials for.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="shop"></param>
		public static void RefreshFoodTableStock(Character squire, ShopData shop)
		{
			foreach (var (index, product) in shop.Products)
				product.RequiredAmount = GetDishStock(squire, shop, index);
		}

		/// <summary>
		/// Serves the dish the given table lists under the given index,
		/// paying its owner for it.
		/// </summary>
		/// <param name="eater"></param>
		/// <param name="squire"></param>
		/// <param name="shop"></param>
		/// <param name="index"></param>
		public static void ServeFood(Character eater, Character squire, ShopData shop, int index)
		{
			if (!TryGetDish(shop, index, out var dish))
			{
				Log.Warning("SquireSkillHelper.ServeFood: '{0}' asked for dish {1} of '{2}'s table, which lists {3}.", eater.Name, index, squire.Name, shop.Products.Count);
				return;
			}

			if (shop.Level < dish.SkillLevel)
			{
				eater.SystemMessage("NotEnoughMaterial");
				return;
			}

			var price = squire == eater ? 0 : GetProductPrice(shop, index);

			if (price > 0 && eater.Inventory.CountItem(ItemId.Silver) < price)
			{
				eater.SystemMessage("NotEnoughMoney");
				return;
			}

			foreach (var material in dish.Materials)
			{
				if (!TryGetItemId(material.ItemClassName, out var materialId) || squire.Inventory.CountItem(materialId) < material.Amount)
				{
					eater.SystemMessage("NotEnoughMaterial");
					return;
				}
			}

			foreach (var material in dish.Materials)
			{
				if (!TryGetItemId(material.ItemClassName, out var materialId) || !TakeMaterial(squire, materialId, material.Amount))
				{
					Log.Warning("SquireSkillHelper.ServeFood: Couldn't take {0}x '{1}' from '{2}'.", material.Amount, material.ItemClassName, squire.Name);
					return;
				}
			}

			if (price > 0)
			{
				if (eater.RemoveItem(ItemId.Silver, price) != price)
				{
					eater.SystemMessage("NotEnoughMoney");
					return;
				}

				squire.AddItem(ItemId.Silver, price);
			}

			eater.StartBuff(dish.BuffId, shop.Level, 0, GetFoodBuffDuration(squire, dish), squire, SkillId.Squire_FoodTable);

			shop.History.Add(new ShopSaleData
			{
				ClassId = dish.ClassId,
				Price = price,
				Amount = 1,
				BuyerName = eater.Name,
			});

			if (!squire.IsAutoTrading)
			{
				Send.ZC_ADDON_MSG(squire, AddonMessage.INV_ITEM_CHANGE_COUNT, 0, null);
				Send.ZC_NORMAL.AutoSellerHistory(squire.Connection, shop);
			}

			Send.ZC_ADDON_MSG(eater, AddonMessage.INV_ITEM_CHANGE_COUNT, 0, null);
		}

		/// <summary>
		/// Returns how long a dish from the given Squire's table lasts.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="dish"></param>
		private static TimeSpan GetFoodBuffDuration(Character squire, Dish dish)
		{
			var duration = FoodBuffDuration;

			if (dish.AbilityId != null && squire.TryGetActiveAbilityLevel(dish.AbilityId.Value, out var abilityLevel))
				duration += FoodBuffDurationPerAbilityLevel * abilityLevel;

			return duration;
		}

		/// <summary>
		/// Returns how many servings of the table's dish at the given index
		/// its owner can still make.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="shop"></param>
		/// <param name="index"></param>
		private static int GetDishStock(Character squire, ShopData shop, int index)
		{
			if (!TryGetDish(shop, index, out var dish))
				return 0;

			return GetDishStock(squire, dish, shop.Level);
		}

		/// <summary>
		/// Returns how many servings of the given dish a table of the given
		/// level can make out of its owner's materials.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="classId"></param>
		/// <param name="tableLevel"></param>
		public static int GetDishStock(Character squire, int classId, int tableLevel)
		{
			var dish = Array.Find(Dishes, d => d.ClassId == classId);
			if (dish == null)
				return 0;

			return GetDishStock(squire, dish, tableLevel);
		}

		/// <summary>
		/// Returns how many servings of the given dish its owner can make.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="dish"></param>
		/// <param name="tableLevel"></param>
		private static int GetDishStock(Character squire, Dish dish, int tableLevel)
		{
			if (tableLevel < dish.SkillLevel)
				return 0;

			var servings = int.MaxValue;

			foreach (var material in dish.Materials)
			{
				if (!TryGetItemId(material.ItemClassName, out var materialId))
					return 0;

				servings = Math.Min(servings, squire.Inventory.CountItem(materialId) / material.Amount);
			}

			return servings == int.MaxValue ? 0 : servings;
		}

		/// <summary>
		/// Returns the dish the table lists under the given index.
		/// </summary>
		/// <remarks>
		/// The index the client sends is a position in the shop's product
		/// list, and the product carries the dish it was registered for.
		/// </remarks>
		/// <param name="shop"></param>
		/// <param name="index"></param>
		/// <param name="dish"></param>
		private static bool TryGetDish(ShopData shop, int index, out Dish dish)
		{
			dish = null;

			var product = shop.GetProduct(index);
			if (product == null)
				return false;

			dish = Array.Find(Dishes, d => d.ClassId == product.ItemId);

			return dish != null;
		}

		/// <summary>
		/// Returns the silver the shop's product at the given index costs.
		/// </summary>
		/// <param name="shop"></param>
		/// <param name="index"></param>
		private static int GetProductPrice(ShopData shop, int index)
			=> shop.GetProduct(index)?.Price ?? 0;

		/// <summary>
		/// Returns whether the given id is a dish a Refreshment Table
		/// serves.
		/// </summary>
		/// <param name="classId"></param>
		public static bool IsDish(int classId)
			=> Array.Exists(Dishes, d => d.ClassId == classId);

		/// <summary>
		/// Resolves an item class name to its id.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="itemId"></param>
		private static bool TryGetItemId(string className, out int itemId)
		{
			itemId = 0;

			if (!ZoneServer.Instance.Data.ItemDb.TryFind(className, out var itemData))
			{
				Log.Warning("SquireSkillHelper.TryGetItemId: Item '{0}' not found.", className);
				return false;
			}

			itemId = itemData.Id;
			return true;
		}

		/// <summary>
		/// Takes the given amount of an item out of the Squire's inventory,
		/// stack by stack, and returns whether all of it could be taken.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		private static bool TakeMaterial(Character squire, int itemId, int amount)
		{
			while (amount > 0)
			{
				if (!squire.Inventory.TryFindItem(itemId, out var material))
					return false;

				var take = Math.Min(amount, material.Amount);

				if (squire.Inventory.Remove(material, take, InventoryItemRemoveMsg.Used) != InventoryResult.Success)
					return false;

				amount -= take;
			}

			return true;
		}

		/// <summary>
		/// Services one of the customer's items through the Squire's shop,
		/// taking the shop's fee and the Squire's material.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="customer"></param>
		/// <param name="shop"></param>
		/// <param name="itemWorldId"></param>
		public static void ServiceItem(Character squire, Character customer, ShopData shop, long itemWorldId)
		{
			if (!customer.Inventory.TryGetItemOrEquip(itemWorldId, out var item))
				return;

			if (!TryGetServiceMaterial(shop.SkillId, out var materialId))
				return;

			if (!CanService(shop.SkillId, item))
			{
				customer.SystemMessage("WrongDropItem");
				return;
			}

			var materialAmount = GetServiceMaterialAmount(shop.SkillId, item);

			if (squire.Inventory.CountItem(materialId) < materialAmount)
			{
				customer.SystemMessage("NotEnoughMaterial");
				return;
			}

			// A Squire working on their own gear is charged nothing, which
			// is why the shop shows them a count where it shows everyone
			// else a price.
			var price = squire == customer ? 0 : GetServiceFee(shop, item);

			if (price > 0 && customer.Inventory.CountItem(ItemId.Silver) < price)
			{
				customer.SystemMessage("NotEnoughMoney");
				return;
			}

			if (!TakeMaterial(squire, materialId, materialAmount))
			{
				Log.Warning("SquireSkillHelper.ServiceItem: Couldn't take {0}x the material from '{1}'.", materialAmount, squire.Name);
				return;
			}

			if (price > 0)
			{
				if (customer.RemoveItem(ItemId.Silver, price) != price)
				{
					squire.AddItem(materialId, materialAmount);
					customer.SystemMessage("NotEnoughMoney");
					return;
				}

				squire.AddItem(ItemId.Silver, price);
			}

			string detail;

			if (shop.SkillId == SkillId.Squire_Repair)
			{
				RepairItem(squire, customer, item, shop.Level);
				detail = price.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				detail = MaintainItem(squire, customer, item, shop.Level);
			}

			shop.History.Add(new ShopSaleData
			{
				ClassId = item.Id,
				Price = price,
				Amount = 1,
				// The client reads this one string as the whole log line,
				// splitting it into who, which item, and what it gained.
				BuyerName = string.Join("#", customer.Name, item.Id, detail),
			});

			if (!squire.IsAutoTrading)
			{
				Send.ZC_ADDON_MSG(squire, AddonMessage.INV_ITEM_CHANGE_COUNT, 0, null);
				Send.ZC_NORMAL.AutoSellerHistory(squire.Connection, shop);
			}

			Send.ZC_ADDON_MSG(customer, AddonMessage.INV_ITEM_CHANGE_COUNT, 0, null);
		}

		/// <summary>
		/// Restores the item's durability past its maximum.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="customer"></param>
		/// <param name="item"></param>
		/// <param name="skillLevel"></param>
		private static void RepairItem(Character squire, Character customer, Item item, int skillLevel)
		{
			var overMax = item.MaxDurability * DurabilityRatePerSkillLevel * skillLevel;

			if (squire.TryGetActiveAbilityLevel(AbilityId.Squire10, out var abilityLevel))
			{
				if (GameRandom.Get().Next(1, ServiceChanceDenominator + 1) < abilityLevel * ServiceChancePerAbilityLevel)
					overMax *= 2;
			}

			// The durability setter clamps to the maximum, and repairing
			// past it is the whole point of the skill.
			item.Properties.SetFloat(PropertyName.Dur, item.MaxDurability + (int)overMax);

			Send.ZC_OBJECT_PROPERTY(customer.Connection, item, PropertyName.Dur);
			customer.InvalidateProperties();
		}

		/// <summary>
		/// Raises the item's attack or defense for a limited number of
		/// hits.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="customer"></param>
		/// <param name="item"></param>
		/// <param name="skillLevel"></param>
		private static string MaintainItem(Character squire, Character customer, Item item, int skillLevel)
		{
			var isArmor = item.Data.Group == ItemGroup.Armor;

			RemoveMaintenance(item);

			var maintained = MaintainedProperties(item);
			var before = new float[maintained.Length];

			for (var i = 0; i < maintained.Length; ++i)
				item.Properties.TryGetFloat(maintained[i], out before[i]);

			var value = GetMaintenanceValue(item, skillLevel, isArmor);
			var count = GetMaintenanceCount(squire, skillLevel, isArmor);

			// The tooltip reads these as remaining over maximum, so both
			// start at the full count.
			item.Properties.SetFloat(PropertyName.BuffValue, value);
			item.Properties.SetFloat(PropertyName.BuffUseCount, count);
			item.Properties.SetFloat(PropertyName.BuffCount, count);
			item.Properties.SetFloat(PropertyName.BuffSkillType, (float)SkillId.Squire_EquipmentTouchUp);
			item.Properties.SetString(PropertyName.BuffCaster, squire.Name);
			item.Properties.SetString(PropertyName.BuffEndTime, DateTime.Now.Add(MaintenanceDuration).ToPropertyDateTimeString());

			item.Properties.Invalidate(maintained);

			Send.ZC_OBJECT_PROPERTY(customer.Connection, item,
				PropertyName.BuffValue, PropertyName.BuffUseCount, PropertyName.BuffCount, PropertyName.BuffSkillType,
				PropertyName.BuffCaster, PropertyName.BuffEndTime,
				PropertyName.MINATK, PropertyName.MAXATK, PropertyName.MATK,
				PropertyName.DEF, PropertyName.MDEF);

			customer.InvalidateProperties();

			var changes = new List<string>();

			for (var i = 0; i < maintained.Length; ++i)
			{
				if (!item.Properties.TryGetFloat(maintained[i], out var after))
					continue;

				changes.Add(string.Join("@", maintained[i],
					((int)before[i]).ToString(CultureInfo.InvariantCulture),
					((int)after).ToString(CultureInfo.InvariantCulture)));
			}

			return string.Join("@", changes);
		}

		/// <summary>
		/// Spends one of the maintenance's remaining hits on the character's
		/// weapons, or on their armor when they were the one struck.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="isArmor"></param>
		public static void ConsumeMaintenance(ICombatEntity entity, bool isArmor)
		{
			if (entity is not Character character)
				return;

			foreach (var item in character.Inventory.GetEquip().Values)
			{
				if (item is DummyEquipItem)
					continue;

				if (item.Properties.GetFloat(PropertyName.BuffValue) <= 0)
					continue;

				if ((item.Data.Group == ItemGroup.Armor) != isArmor)
					continue;

				if (TryExpireMaintenance(item))
				{
					SendMaintenanceUpdate(character, item);
					continue;
				}

				var remaining = item.Properties.GetFloat(PropertyName.BuffUseCount) - 1;

				if (remaining <= 0)
				{
					RemoveMaintenance(item);
					SendMaintenanceUpdate(character, item);
					continue;
				}

				item.Properties.SetFloat(PropertyName.BuffUseCount, remaining);
				Send.ZC_OBJECT_PROPERTY(character.Connection, item, PropertyName.BuffUseCount);
			}
		}

		/// <summary>
		/// Tells the client the item's maintenance and the stats it was
		/// raising have changed.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		private static void SendMaintenanceUpdate(Character character, Item item)
		{
			Send.ZC_OBJECT_PROPERTY(character.Connection, item,
				PropertyName.BuffValue, PropertyName.BuffUseCount, PropertyName.BuffCount,
				PropertyName.BuffSkillType, PropertyName.BuffCaster, PropertyName.BuffEndTime,
				PropertyName.MINATK, PropertyName.MAXATK, PropertyName.MATK,
				PropertyName.DEF, PropertyName.MDEF);

			character.InvalidateProperties();
		}

		/// <summary>
		/// Removes an expired maintenance bonus from the item, and returns
		/// whether one was removed.
		/// </summary>
		/// <param name="item"></param>
		public static bool TryExpireMaintenance(Item item)
		{
			if (item.Properties.GetFloat(PropertyName.BuffValue) <= 0)
				return false;

			var endTime = item.Properties.GetString(PropertyName.BuffEndTime, NoneValue);
			if (endTime.TryGetPropertyStringToDateTime(out var expiresAt) && DateTime.Now < expiresAt)
				return false;

			RemoveMaintenance(item);

			return true;
		}

		/// <summary>
		/// Takes the item's maintenance bonus back off its stats.
		/// </summary>
		/// <param name="item"></param>
		private static void RemoveMaintenance(Item item)
		{
			if (item.Properties.GetFloat(PropertyName.BuffValue) <= 0)
				return;

			item.Properties.SetFloat(PropertyName.BuffValue, 0);
			item.Properties.SetFloat(PropertyName.BuffUseCount, 0);
			item.Properties.SetFloat(PropertyName.BuffCount, 0);
			item.Properties.SetFloat(PropertyName.BuffSkillType, 0);
			item.Properties.SetString(PropertyName.BuffCaster, NoneValue);
			item.Properties.SetString(PropertyName.BuffEndTime, NoneValue);

			item.Properties.Invalidate(MaintainedProperties(item));
		}

		/// <summary>
		/// Returns the stats the maintenance bonus shows up in, so they can
		/// be recalculated once it changes.
		/// </summary>
		/// <param name="item"></param>
		private static string[] MaintainedProperties(Item item)
		{
			if (item.Data.Group == ItemGroup.Armor)
				return [PropertyName.DEF, PropertyName.MDEF];

			return [PropertyName.MINATK, PropertyName.MAXATK, PropertyName.MATK];
		}

		/// <summary>
		/// Returns the attack or defense the maintenance adds, reading the
		/// item's stats with any previous bonus already taken off.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="skillLevel"></param>
		/// <param name="isArmor"></param>
		private static int GetMaintenanceValue(Item item, int skillLevel, bool isArmor)
		{
			var rate = skillLevel * MaintenanceRatePerSkillLevel;

			if (isArmor)
			{
				var def = item.Properties.GetFloat(PropertyName.DEF);
				var mdef = item.Properties.GetFloat(PropertyName.MDEF);

				if (item.Data.EquipType1 == EquipType.Shield)
				{
					if (def <= 0)
						return (int)(mdef * rate);

					if (mdef <= 0)
						return (int)(def * rate);
				}

				return (int)((def + mdef) / 2f * rate);
			}

			var minAtk = item.Properties.GetFloat(PropertyName.MINATK);
			var maxAtk = item.Properties.GetFloat(PropertyName.MAXATK);
			var mAtk = item.Properties.GetFloat(PropertyName.MATK);

			if (item.Data.EquipType1 == EquipType.Trinket)
				return (int)((maxAtk + mAtk) / 2f * rate);

			if (item.Data.EquipType1 == EquipType.Staff || item.Data.EquipType1 == EquipType.THStaff)
				return (int)(mAtk * rate);

			return (int)((minAtk + maxAtk) / 2f * rate);
		}

		/// <summary>
		/// Returns how many hits the maintenance lasts for.
		/// </summary>
		/// <param name="squire"></param>
		/// <param name="skillLevel"></param>
		/// <param name="isArmor"></param>
		public static int GetMaintenanceCount(Character squire, int skillLevel, bool isArmor)
		{
			var stats = squire.Properties.GetFloat(PropertyName.DEX) + squire.Properties.GetFloat(PropertyName.STR);

			if (isArmor)
			{
				var armorCount = (int)(ArmorBaseCount + skillLevel * ArmorCountPerSkillLevel + stats * ArmorCountPerStat);

				if (squire.TryGetActiveAbilityLevel(AbilityId.Squire4, out var armorAbilityLevel))
					armorCount += armorAbilityLevel * ArmorCountPerAbilityLevel;

				return armorCount;
			}

			var weaponCount = (int)(WeaponBaseCount + skillLevel * WeaponCountPerSkillLevel + stats * WeaponCountPerStat);

			if (squire.TryGetActiveAbilityLevel(AbilityId.Squire3, out var weaponAbilityLevel))
				weaponCount += weaponAbilityLevel * WeaponCountPerAbilityLevel;

			return weaponCount;
		}

		/// <summary>
		/// Returns whether the shop's service applies to the given item.
		/// </summary>
		/// <param name="skillId"></param>
		/// <param name="item"></param>
		private static bool CanService(SkillId skillId, Item item)
		{
			if (item.Data.Type != ItemType.Equip)
				return false;

			if (skillId == SkillId.Squire_Repair)
				return item.MaxDurability > 0;

			if (item.Properties.GetFloat(PropertyName.Dur) <= 0)
				return false;

			// The pieces the shop's own tabs offer: a weapon, an off-hand
			// that attacks, or one of the five armor slots.
			if (item.Data.Group == ItemGroup.Weapon)
				return true;

			if (item.Data.Group == ItemGroup.SubWeapon)
				return item.Properties.GetFloat(PropertyName.MAXATK) > 0;

			if (item.Data.Group != ItemGroup.Armor)
				return false;

			return item.Data.EquipType1 is EquipType.Shield or EquipType.Shirt
				or EquipType.Pants or EquipType.Gloves or EquipType.Boots;
		}

		/// <summary>
		/// Returns the silver servicing the given item costs.
		/// </summary>
		/// <remarks>
		/// Repairing undercuts the smiths deliberately, so its fee is a
		/// fraction of what they charge for the same item. Maintenance has no
		/// such benchmark and charges whatever the shop's owner asked, which
		/// their single dummy product carries.
		/// </remarks>
		/// <param name="shop"></param>
		/// <param name="item"></param>
		private static int GetServiceFee(ShopData shop, Item item)
		{
			if (shop.SkillId == SkillId.Squire_Repair)
			{
				if (!ScriptableFunctions.ItemCalc.TryGet("SCR_Get_Item_RepairPrice", out var repairPriceFunc))
					return 0;

				return (int)(repairPriceFunc(item) * RepairFeeOfSmithPrice);
			}

			foreach (var product in shop.Products.Values)
				return product.Price;

			return 0;
		}

		/// <summary>
		/// Returns how much of its material servicing the given item costs
		/// the shop's owner.
		/// </summary>
		/// <param name="skillId"></param>
		/// <param name="item"></param>
		private static int GetServiceMaterialAmount(SkillId skillId, Item item)
		{
			var grade = item.Properties.GetFloat(PropertyName.ItemGrade, 1);

			if (skillId == SkillId.Squire_Repair)
			{
				var repairLevel = Math.Max(1f, item.UseLevel / LevelPerMaterial);
				var reinforce = item.Properties.GetFloat(PropertyName.Reinforce_2);
				var transcend = item.Properties.GetFloat(PropertyName.Transcend);
				var priceRatio = item.RepairPriceRatio / 100f;

				var wear = 1f + ((grade - 1f) * GradeWearRate + reinforce * ReinforceWearRate + transcend * TranscendWearRate);
				var repairCount = (repairLevel + repairLevel * (grade - 1f) / 2f) * wear * priceRatio;

				return (int)Math.Max(1f, repairCount);
			}

			var maintenanceLevel = Math.Max(1f, Math.Max(item.UseLevel, item.HiddenLevel) / LevelPerMaterial);
			var maintenanceCount = (maintenanceLevel + maintenanceLevel * (grade - 1f) / 2f) / 2f;

			return (int)Math.Max(1f, maintenanceCount);
		}

		/// <summary>
		/// Returns the material one use of the shop's service costs its
		/// owner.
		/// </summary>
		/// <param name="skillId"></param>
		/// <param name="itemId"></param>
		private static bool TryGetServiceMaterial(SkillId skillId, out int itemId)
		{
			itemId = 0;

			var className = skillId == SkillId.Squire_Repair ? RepairKitClassName : WhetstoneClassName;

			if (!ZoneServer.Instance.Data.ItemDb.TryFind(className, out var itemData))
			{
				Log.Warning("SquireSkillHelper.TryGetServiceMaterial: Item '{0}' not found.", className);
				return false;
			}

			itemId = itemData.Id;
			return true;
		}
	}
}
