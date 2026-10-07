using System;
using System.Globalization;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Helper functions for the Sage's Portal and Portal Shop.
	/// </summary>
	public static class SageSkillHelper
	{
		/// <summary>
		/// The number of portal slots every Sage has.
		/// </summary>
		public const int BasePortalCount = 3;

		/// <summary>
		/// The highest number of portal slots, one etc property each.
		/// </summary>
		public const int MaxPortalCount = 6;

		/// <summary>
		/// The material a Portal Shop spends on every warp it sells.
		/// </summary>
		public const string PortalStoneClassName = "misc_portalstone";

		/// <summary>
		/// How long a portal opened by the Portal skill stays up.
		/// </summary>
		public static readonly TimeSpan PortalLifeTime = TimeSpan.FromSeconds(15);

		/// <summary>
		/// Returns the etc property that holds the given portal slot.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string GetPortalPropertyName(int index) => "Sage_Portal_" + index;

		/// <summary>
		/// Returns how many portal slots the character has, the base three
		/// plus one per level of Sage1, Sage16 and Sage17.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static int GetMaxPortalCount(Character character)
		{
			var count = BasePortalCount;

			foreach (var abilityId in new[] { AbilityId.Sage1, AbilityId.Sage16, AbilityId.Sage17 })
			{
				if (character.TryGetActiveAbilityLevel(abilityId, out var level))
					count += level;
			}

			return Math.Min(MaxPortalCount, count);
		}

		/// <summary>
		/// Returns the cooldown a portal slot goes on once its portal was
		/// opened, 30 minutes minus one per skill level past the first.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static TimeSpan GetPortalCooldown(Character character)
		{
			var level = character.TryGetSkill(SkillId.Sage_Portal, out var skill) ? skill.Level : 1;
			return TimeSpan.FromSeconds(1800 - (level - 1) * 60);
		}

		/// <summary>
		/// Returns the raw value of the given portal slot, "None" when it's
		/// empty.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private static string GetPortalValue(Character character, int index)
		{
			var value = character.Etc.Properties.GetString(GetPortalPropertyName(index), "None");
			return string.IsNullOrEmpty(value) ? "None" : value;
		}

		/// <summary>
		/// Returns the location saved in the given portal slot and the time
		/// its cooldown ends, if it is on one.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="index"></param>
		/// <param name="location">The location as "map#x#y#z".</param>
		/// <param name="cooldownEnd"></param>
		/// <returns></returns>
		public static bool TryGetPortal(Character character, int index, out string location, out DateTime? cooldownEnd)
		{
			location = null;
			cooldownEnd = null;

			if (index < 1 || index > GetMaxPortalCount(character))
				return false;

			var value = GetPortalValue(character, index);
			if (value == "None")
				return false;

			var parts = value.Split('@');
			location = parts[0];

			if (parts.Length > 1 && parts[1].TryGetPropertyStringToDateTime(out var end))
				cooldownEnd = end;

			return true;
		}

		/// <summary>
		/// Saves the character's current location in the first free portal
		/// slot, and returns whether there was one.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static bool SavePortal(Character character)
		{
			var max = GetMaxPortalCount(character);

			for (var i = 1; i <= max; i++)
			{
				if (GetPortalValue(character, i) != "None")
					continue;

				character.SetEtcProperty(GetPortalPropertyName(i), character.GetLocationToString());
				return true;
			}

			return false;
		}

		/// <summary>
		/// Empties the given portal slot, unless its portal is on cooldown.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool DeletePortal(Character character, int index)
		{
			if (!TryGetPortal(character, index, out _, out var cooldownEnd))
				return false;

			if (cooldownEnd > DateTime.Now)
				return false;

			character.SetEtcProperty(GetPortalPropertyName(index), "None");
			return true;
		}

		/// <summary>
		/// Lifts the cooldown off every portal slot whose cooldown ran out.
		/// </summary>
		/// <param name="character"></param>
		/// <returns>True if any slot changed.</returns>
		public static bool RefreshPortalCooldowns(Character character)
		{
			var changed = false;

			for (var i = 1; i <= MaxPortalCount; i++)
			{
				if (!TryGetPortal(character, i, out var location, out var cooldownEnd) || cooldownEnd == null || cooldownEnd > DateTime.Now)
					continue;

				character.SetEtcProperty(GetPortalPropertyName(i), location);
				changed = true;
			}

			return changed;
		}

		/// <summary>
		/// Parses a saved "map#x#y#z" location.
		/// </summary>
		/// <param name="location"></param>
		/// <param name="mapId"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static bool TryParseLocation(string location, out int mapId, out Position position)
		{
			mapId = 0;
			position = Position.Zero;

			var parts = location?.Split('#');
			if (parts == null || parts.Length != 4)
				return false;

			if (!ZoneServer.Instance.Data.MapDb.TryFind(parts[0], out var mapData))
				return false;

			if (!float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var x)
				|| !float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var y)
				|| !float.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var z))
				return false;

			position = new Position(x, y, z);
			if (!ZoneServer.Instance.World.TryGetMap(mapData.Id, out var map) || !map.Ground.IsValidPosition(position))
				return false;

			mapId = mapData.Id;
			return true;
		}

		/// <summary>
		/// Opens the portal saved in the given slot next to the character,
		/// for them and their party, and puts the slot on cooldown.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool OpenPortal(Character character, int index)
		{
			if (!TryGetPortal(character, index, out var location, out var cooldownEnd))
				return false;

			if (cooldownEnd > DateTime.Now)
			{
				character.SystemMessage("CannotUseInCoolTime");
				return false;
			}

			if (!TryParseLocation(location, out var mapId, out var position))
			{
				Log.Debug("SageSkillHelper.OpenPortal: Invalid portal location '{0}' of '{1}'.", location, character.Name);
				return false;
			}

			var cooldown = GetPortalCooldown(character);
			character.SetEtcProperty(GetPortalPropertyName(index), location + "@" + DateTime.Now.Add(cooldown).ToPropertyDateTimeString());

			var portal = new WarpMonster(MonsterId.MissionGate, new Location(character.MapId, character.Position), new Location(mapId, position), new Direction(1, 0));
			portal.AssociatedHandle = character.Handle;
			portal.DialogName = "SAGE_WARP";
			portal.Properties[PropertyName.Scale] = 1;

			if (character.Connection.Party != null)
			{
				portal.Visibility = ActorVisibility.Party;
				portal.VisibilityId = character.Connection.Party.ObjectId;
			}
			else
			{
				portal.Visibility = ActorVisibility.Individual;
				portal.VisibilityId = character.ObjectId;
			}

			portal.DisappearTime = DateTime.Now.Add(PortalLifeTime);
			character.Map.AddMonster(portal);
			portal.AttachEffect(AnimationName.Portal, 1, EffectLocation.Top);

			CallSafe(LiftCooldownLater(character, cooldown));

			return true;
		}

		/// <summary>
		/// Lifts the character's expired portal cooldowns once the given
		/// time passed, if they are still online.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="delay"></param>
		/// <returns></returns>
		private static async Task LiftCooldownLater(Character character, TimeSpan delay)
		{
			await Task.Delay(delay + TimeSpan.FromSeconds(1));

			if (character.Connection == null || !character.IsOnline)
				return;

			if (RefreshPortalCooldowns(character))
				Send.ZC_EXEC_CLIENT_SCP(character.Connection, ClientScripts.SAGE_PORTAL_SAVE_SUCCESS);
		}

		/// <summary>
		/// Creates the Portal Shop product for the given portal slot, if the
		/// slot holds a portal.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="index"></param>
		/// <param name="price"></param>
		/// <param name="level"></param>
		/// <param name="product"></param>
		/// <returns></returns>
		public static bool TryCreatePortalProduct(Character character, int index, int price, int level, out ProductData product)
		{
			product = null;

			if (price < 1 || !TryGetPortal(character, index, out var location, out _) || !TryParseLocation(location, out _, out _))
				return false;

			product = new ProductData
			{
				ItemId = index,
				Price = price,
				Amount = level,
				RequiredAmount = GetPortalShopStock(character),
				ArgStr = location,
			};

			return true;
		}

		/// <summary>
		/// Returns how many warps the given Sage's portal stones still pay
		/// for, one stone per warp.
		/// </summary>
		/// <param name="sage"></param>
		/// <returns></returns>
		public static int GetPortalShopStock(Character sage)
		{
			if (!ZoneServer.Instance.Data.ItemDb.TryFind(PortalStoneClassName, out var itemData))
				return 0;

			return sage.Inventory.CountItem(itemData.Id);
		}

		/// <summary>
		/// Updates how many warps the given Portal Shop can still sell.
		/// </summary>
		/// <param name="sage"></param>
		/// <param name="shop"></param>
		public static void RefreshPortalShopStock(Character sage, ShopData shop)
		{
			var stock = GetPortalShopStock(sage);
			foreach (var product in shop.Products.Values)
				product.RequiredAmount = stock;
		}

		/// <summary>
		/// Warps the buyer to the portal the given Portal Shop lists under
		/// the given index, paying the shop's owner for it.
		/// </summary>
		/// <param name="buyer"></param>
		/// <param name="sage"></param>
		/// <param name="shop"></param>
		/// <param name="index"></param>
		public static void SellPortal(Character buyer, Character sage, ShopData shop, int index)
		{
			var product = shop.GetProduct(index);
			if (product == null)
			{
				Log.Warning("SellPortal: '{0}' asked for portal {1} of '{2}'s shop, which lists {3}.", buyer.Name, index, sage.Name, shop.Products.Count);
				return;
			}

			if (buyer == sage)
			{
				buyer.ServerMessage(Localization.Get("You can't buy from your own Portal Shop."));
				return;
			}

			if (!TryParseLocation(product.ArgStr, out var mapId, out var position))
			{
				Log.Warning("SellPortal: '{0}'s shop lists the invalid portal '{1}'.", sage.Name, product.ArgStr);
				return;
			}

			if (!ZoneServer.Instance.Data.ItemDb.TryFind(PortalStoneClassName, out var stoneData) || !sage.Inventory.TryFindItem(stoneData.Id, out var stone))
			{
				buyer.SystemMessage("NotEnoughStock");
				return;
			}

			if (buyer.Inventory.CountItem(ItemId.Silver) < product.Price)
			{
				buyer.SystemMessage("NotEnoughSilver");
				return;
			}

			if (sage.Inventory.Remove(stone, 1, InventoryItemRemoveMsg.Used) != InventoryResult.Success)
				return;

			if (buyer.RemoveItem(ItemId.Silver, product.Price) != product.Price)
			{
				sage.AddItem(stoneData.Id, 1);
				buyer.SystemMessage("NotEnoughSilver");
				return;
			}

			sage.AddItem(ItemId.Silver, product.Price);

			shop.History.Add(new ShopSaleData
			{
				ClassId = product.ItemId,
				Price = product.Price,
				Amount = 1,
				BuyerName = buyer.Name,
			});

			if (!sage.IsAutoTrading)
			{
				Send.ZC_ADDON_MSG(sage, AddonMessage.INV_ITEM_CHANGE_COUNT, 0, null);
				Send.ZC_NORMAL.AutoSellerHistory(sage.Connection, shop);
			}

			Send.ZC_ADDON_MSG(buyer, AddonMessage.INV_ITEM_CHANGE_COUNT, 0, null);

			buyer.Warp(mapId, position);
		}
	}
}
