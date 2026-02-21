using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Versioning;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class ItemData : PropertizedData
	{
		public int Id { get; set; }

		public string ClassName { get; set; }
		public string Name { get; set; }

		public ItemType Type { get; set; }
		public ItemGroup Group { get; set; }
		public InventoryCategory Category { get; set; }

		public float Weight { get; set; }
		public int MaxStack { get; set; }
		public int Price { get; set; }
		public int SellPrice { get; set; }

		public EquipType EquipType1 { get; set; }
		public EquipType EquipType2 { get; set; }
		public EquipExpGroup EquipExpGroup { get; set; }

		public ItemGrade Grade { get; set; }
		public int MinLevel { get; set; }
		public int CardLevel { get; set; }
		public int GemLevel { get; set; }
		public int Potential { get; set; }
		public int MaxPotential { get; set; }
		public int Durability { get; set; }
		public int MaxDurability { get; set; }
		public float MinAtk { get; set; }
		public float MaxAtk { get; set; }
		public float PAtk { get; set; }
		public float MAtk { get; set; }
		public float AddMinAtk { get; set; }
		public float AddMaxAtk { get; set; }
		public float AddMAtk { get; set; }
		public float Def { get; set; }
		public float MDef { get; set; }
		public float AddDef { get; set; }
		public float AddMDef { get; set; }
		public float SmallSizeBonus { get; set; }
		public float MediumSizeBonus { get; set; }
		public float LargeSizeBonus { get; set; }
		public ArmorMaterialType Material { get; set; }
		public SkillId LeftHandSkill { get; set; }
		public SkillAttackType AttackType { get; set; }

		public float Slash { get; set; }
		public float Aries { get; set; }
		public float Strike { get; set; }
		public float SlashDefense { get; set; }
		public float AriesDefense { get; set; }
		public float StrikeDefense { get; set; }
		public float SlashRange { get; set; }
		public float AriesRange { get; set; }
		public float StrikeRange { get; set; }

		public float FireResistence { get; set; }
		public float IceResistence { get; set; }
		public float LightningResistence { get; set; }
		public float EarthResistence { get; set; }
		public float PoisonResistence { get; set; }
		public float HolyResistence { get; set; }
		public float DarkResistence { get; set; }
		public float SoulResistence { get; set; }

		public CooldownId CooldownId { get; set; }
		public TimeSpan CooldownTime { get; set; } = TimeSpan.Zero;

		public float CriticalHitRate { get; set; }
		public float CriticalAttack { get; set; }
		public float CriticalDodgeRate { get; set; }
		public float AddHitRate { get; set; }
		public float AddDodgeRate { get; set; }
		public float Str { get; set; }
		public float Dex { get; set; }
		public float Con { get; set; }
		public float Int { get; set; }
		public float Mna { get; set; }
		public float SR { get; set; }
		public float SDR { get; set; }
		public float CriticalMagicAttack { get; set; }
		public float MGP { get; set; }
		public float AddSkillMaxR { get; set; }
		public float SkillRange { get; set; }
		public float SkillAngle { get; set; }
		public float Luck { get; set; }
		public float BlockRate { get; set; }
		public float Block { get; set; }
		public float BlockBreak { get; set; }
		public float Revive { get; set; }
		public float HitCount { get; set; }
		public float BackHit { get; set; }
		public float SkillPower { get; set; }
		public float ASPD { get; set; }
		public float MSPD { get; set; }
		public float KnockdownPower { get; set; }
		public float MHp { get; set; }
		public float MSp { get; set; }
		public float Msta { get; set; }
		public float RHp { get; set; }
		public float RSp { get; set; }
		public float RSptime { get; set; }
		public float RSta { get; set; }
		public float ClothBonus { get; set; }
		public float LeatherBonus { get; set; }
		public float ChainBonus { get; set; }
		public float IronBonus { get; set; }
		public float AddGhost { get; set; }
		public float AddForester { get; set; }
		public float AddWidling { get; set; }
		public float AddVelias { get; set; }
		public float AddParamune { get; set; }
		public float AddKlaida { get; set; }
		public float AddFire { get; set; }
		public float AddIce { get; set; }
		public float AddPoison { get; set; }
		public float AddLightning { get; set; }
		public float AddEarth { get; set; }
		public float AddSoul { get; set; }
		public float AddHoly { get; set; }
		public float AddDark { get; set; }
		public float AddBossAtk { get; set; }
		public float BaseSocket { get; set; }
		public float MaxSocketCount { get; set; }
		public float BaseSocketMa { get; set; }
		public float MaxSocketMa { get; set; }
		public float MinOption { get; set; }
		public float MaxOption { get; set; }
		public float MinRDmg { get; set; }
		public float MaxRDmg { get; set; }
		public float FDMinR { get; set; }
		public float FDMaxR { get; set; }
		public float LifeTime { get; set; }
		public float ItemLifeTimeOver { get; set; }
		public float NeedAppraisal { get; set; }
		public float NeedRandomOption { get; set; }
		public float LootingChance { get; set; }
		public float IsAlwaysHatVisible { get; set; }
		public float SkillWidthRange { get; set; }
		public float DynamicLifeTime { get; set; }
		public float TeamBelonging { get; set; }
		public float AddDamageAtk { get; set; }
		public float ResAddDamage { get; set; }
		public float JobGrade { get; set; }
		public float MagicIceAtk { get; set; }
		public float MagicEarthAtk { get; set; }
		public float MagicSoulAtk { get; set; }
		public float MagicDarkAtk { get; set; }
		public float MagicMeleeAtk { get; set; }
		public float MagicFireAtk { get; set; }
		public float MagicLightningAtk { get; set; }
		public int Cooldown { get; set; }
		public string CooldownGroup { get; set; }

		public ItemScriptData Script { get; set; }

		public bool HasScript => this.Script != null;
		public bool HasCooldown => this.CooldownTime > TimeSpan.Zero;

		public string EquipSkill { get; set; }
		public bool Journal { get; set; }
		public bool CubeDuplicate { get; set; }
		public bool CubeReopen { get; set; }
		public int ReopenDiscount { get; set; }
		public ReinforceType ReinforceType { get; set; }
		public int MaterialPrice { get; set; }
		public CardGroup CardGroup { get; set; }
		public string EquipSlot { get; set; }
		public bool IsOneHanded => OneHandedWeapons.Contains(this.EquipType1);
		public bool IsTwoHanded => TwoHandedWeapons.Contains(this.EquipType1);

		public string MainProperties { get; internal set; }

		private static readonly EquipType[] OneHandedWeapons =
		{
			EquipType.Sword,
			EquipType.Staff,
			EquipType.Bow,
			EquipType.Mace,
			EquipType.Spear
		};

		private static readonly EquipType[] TwoHandedWeapons =
		{
			EquipType.THSword,
			EquipType.THStaff,
			EquipType.THBow,
			EquipType.THMace,
			EquipType.THSpear
		};


		/// <summary>
		/// Gets an array of the main stats/properties for this item.
		/// </summary>
		/// <returns>An array of PropertyName strings representing the main stats.</returns>
		public List<string> GetMainStatProperties()
		{
			var mainStats = new List<string>();

			// Add attack-related properties if the item is a weapon
			if (this.Group == ItemGroup.Weapon || this.Group == ItemGroup.SubWeapon)
			{
				mainStats.AddRange(new[] { PropertyName.MINATK, PropertyName.MAXATK });
				if (this.PAtk != 0) mainStats.Add(PropertyName.PATK);
				if (this.MAtk != 0) mainStats.Add(PropertyName.MATK);
			}

			// Add defense properties if the item is armor
			if (this.Group == ItemGroup.Armor || this.Group == ItemGroup.Helmet)
			{
				mainStats.AddRange([PropertyName.DEF, PropertyName.MDEF]);
			}

			return mainStats;
		}

		public bool IsWeapon()
		{
			return (this.EquipType1 >= EquipType.Sword && this.EquipType1 <= EquipType.Cannon) && this.Group != ItemGroup.Recipe;
		}

		public bool IsArmor()
		{
			return (this.EquipType1 == EquipType.Shirt
				|| this.EquipType1 == EquipType.Gloves
				|| this.EquipType1 == EquipType.Pants
				|| this.EquipType1 == EquipType.Boots)
				&& this.Group != ItemGroup.Recipe;
		}

		public bool IsEquippable()
		{
			return this.EquipType1 != EquipType.None;
		}

		public bool CanJobEquip(JobClass job)
		{
			return true;
		}
	}

	[Serializable]
	public class ItemScriptData
	{
		public string Function { get; set; }
		public string StrArg { get; set; }
		public string StrArg2 { get; set; }
		public float NumArg1 { get; set; }
		public float NumArg2 { get; set; }
	}

	/// <summary>
	/// Item database, indexed by item id.
	/// </summary>
	public class ItemDb : DatabaseJsonIndexed<int, ItemData>
	{
		/// <summary>
		/// Returns the item with the given name or null if there was no
		/// matching item.
		/// </summary>
		/// <param name="name">Name of the item (case-insensitive)</param>
		/// <returns></returns>
		public ItemData Find(string name)
		{
			name = name.ToLower();
			return this.Entries.FirstOrDefault(a => a.Value.Name.ToLower() == name).Value;
		}

		/// <summary>
		/// Returns the item with the given class name or null if there was no
		/// matching item.
		/// </summary>
		/// <param name="name">Name of the item (case-insensitive)</param>
		/// <returns></returns>
		public ItemData FindByClass(string name)
		{
			name = name.ToLower();
			return this.Entries.FirstOrDefault(a => a.Value.ClassName.ToLower() == name).Value;
		}

		/// <summary>
		/// Returns a list of all items whose name contains the given string
		/// If there is an exact match, return only those ones
		/// </summary>
		/// <param name="searchString">String to search for (case-insensitive)</param>
		/// <returns></returns>
		public List<ItemData> FindAllPreferExact(string searchString)
		{
			searchString = searchString.ToLower();

			var exactMatches = this.Entries.Values.Where(a => a.Name.ToLower() == searchString);
			if (exactMatches.Any())
				return exactMatches.ToList();

			return this.FindAll(searchString);
		}

		/// <summary>
		/// Returns a list of all items whichs' names contain the given
		/// string.
		/// </summary>
		/// <param name="searchString">String to look for in the item names (case-insensitive)</param>
		/// <returns></returns>
		public List<ItemData> FindAll(string searchString)
		{
			searchString = searchString.ToLower();
			return this.Entries.Where(a => a.Value.Name.ToLower().Contains(searchString)).Select(a => a.Value).ToList();
		}

		/// <summary>
		/// Try to find item by class name if null then search by item name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		/// <returns>If item is found or not</returns>
		public bool TryFind(string name, out ItemData item)
		{
			item = this.FindByClass(name);
			if (item != null)
				return true;
			item = this.Find(name);
			if (item != null)
				return true;
			return false;
		}

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("itemId", "className", "name", "type", "group", "weight", "maxStack", "price", "sellPrice");

			var data = new ItemData();

			data.Id = entry.ReadInt("itemId");

			data.ClassName = entry.ReadString("className");
			data.Name = entry.ReadString("name");
			data.Type = entry.ReadEnum<ItemType>("type");
			data.Group = entry.ReadEnum<ItemGroup>("group");

			data.Weight = entry.ReadFloat("weight", 0);
			data.MaxStack = entry.ReadInt("maxStack", 1);
			data.Price = entry.ReadInt("price", 0);
			data.SellPrice = entry.ReadInt("sellPrice", 0);
			if (data.SellPrice == 0 && data.Price > 0)
			{
				data.SellPrice = data.Price / 2;
			}
			data.EquipSlot = entry.ReadString("equipSlot");
			data.EquipType1 = entry.ReadEnum("equipType1", EquipType.None);
			data.EquipType2 = entry.ReadEnum("equipType2", EquipType.None);
			data.Category = GetCategory(data);
			data.EquipExpGroup = entry.ReadEnum("equipExpGroup", EquipExpGroup.None);
			data.ReinforceType = entry.ReadEnum("reinforceType", ReinforceType.None);
			data.CardGroup = entry.ReadEnum("cardGroup", CardGroup.None);
			data.Grade = (ItemGrade)entry.ReadInt("grade", 0);
			data.MinLevel = entry.ReadInt("minLevel", 1);
			data.CardLevel = entry.ReadInt("cardLevel", 0);
			data.GemLevel = entry.ReadInt("gemLevel", 0);
			data.Durability = entry.ReadInt("durability", 0);
			data.MaxDurability = entry.ReadInt("maxDurability", 0);
			data.Potential = entry.ReadInt("potential", 0);
			data.MaxPotential = entry.ReadInt("maxPotential", 0);
			data.Journal = entry.ReadBool("journal");

			data.MinAtk = entry.ReadFloat("minAtk", 0);
			data.MaxAtk = entry.ReadFloat("maxAtk", 0);
			data.PAtk = entry.ReadFloat("pAtk", 0);
			data.MAtk = entry.ReadFloat("mAtk", 0);
			data.AddMinAtk = entry.ReadFloat("addMinAtk", 0);
			data.AddMaxAtk = entry.ReadFloat("addMaxAtk", 0);
			data.AddMAtk = entry.ReadFloat("addMAtk", 0);
			data.Def = entry.ReadFloat("def", 0);
			data.MDef = entry.ReadFloat("mDef", 0);
			data.AddDef = entry.ReadFloat("defBonus", 0);
			data.AddMDef = entry.ReadFloat("mDefBonus", 0);
			data.SmallSizeBonus = entry.ReadFloat("smallSizeBonus", 0);
			data.MediumSizeBonus = entry.ReadFloat("mediumSizeBonus", 0);
			data.LargeSizeBonus = entry.ReadFloat("largeSizeBonus", 0);
			data.Material = entry.ReadEnum<ArmorMaterialType>("material", ArmorMaterialType.None);
			data.MaterialPrice = entry.ReadInt("materialPrice", 0);
			data.AttackType = entry.ReadEnum<SkillAttackType>("attackType", SkillAttackType.None);

			data.Aries = entry.ReadFloat("aries", 0);
			data.Slash = entry.ReadFloat("slash", 0);
			data.Strike = entry.ReadFloat("strike", 0);
			data.AriesDefense = entry.ReadFloat("ariesDef", 0);
			data.SlashDefense = entry.ReadFloat("slashDef", 0);
			data.StrikeDefense = entry.ReadFloat("strikeDef", 0);
			data.AriesRange = entry.ReadFloat("ariesRange", 0);
			data.SlashRange = entry.ReadFloat("slashRange", 0);
			data.StrikeRange = entry.ReadFloat("strikeRange", 0);

			data.FireResistence = entry.ReadFloat("fireRes", 0);
			data.IceResistence = entry.ReadFloat("iceRes", 0);
			data.PoisonResistence = entry.ReadFloat("poisonRes", 0);
			data.LightningResistence = entry.ReadFloat("lightningRes", 0);
			data.EarthResistence = entry.ReadFloat("earthRes", 0);
			data.SoulResistence = entry.ReadFloat("soulRes", 0);
			data.HolyResistence = entry.ReadFloat("holyRes", 0);
			data.DarkResistence = entry.ReadFloat("darkRes", 0);

			if (entry.ContainsKey("cooldown"))
				data.CooldownTime = entry.ReadTimeSpan("cooldown", TimeSpan.Zero);
			if (data.CooldownTime != TimeSpan.Zero && entry.ContainsKey("cooldownGroup"))
				data.CooldownId = entry.ReadEnum<CooldownId>("cooldownGroup", 0);

			data.CriticalAttack = entry.ReadFloat("critAttack", 0);
			data.CriticalMagicAttack = entry.ReadFloat("critMagicAttack", 0);
			data.CriticalHitRate = entry.ReadFloat("critHitRate", 0);
			data.CriticalDodgeRate = entry.ReadFloat("critDodgeRate", 0);
			data.AddHitRate = entry.ReadFloat("hitRateBonus", 0);
			data.AddDodgeRate = entry.ReadFloat("dodgeRateBonus", 0);
			data.Str = entry.ReadFloat("str", 0);
			data.Dex = entry.ReadFloat("dex", 0);
			data.Con = entry.ReadFloat("con", 0);
			data.Int = entry.ReadFloat("int", 0);
			data.Mna = entry.ReadFloat("mna", 0);
			data.SR = entry.ReadFloat("sr", 0);
			data.SDR = entry.ReadFloat("sdr", 0);
			data.MGP = entry.ReadFloat("mgp", 0);
			data.AddSkillMaxR = entry.ReadFloat("addSkillMaxR", 0);
			data.SkillRange = entry.ReadFloat("skillRange", 0);
			data.SkillAngle = entry.ReadFloat("skillAngle", 0);
			data.Luck = entry.ReadFloat("luck", 0);
			data.BlockRate = entry.ReadFloat("blockRate", 0);
			data.Block = entry.ReadFloat("block", 0);
			data.BlockBreak = entry.ReadFloat("blockBreak", 0);
			data.Revive = entry.ReadFloat("revive", 0);
			data.HitCount = entry.ReadFloat("hitCount", 0);
			data.BackHit = entry.ReadFloat("backHit", 0);
			data.SkillPower = entry.ReadFloat("skillPower", 0);
			data.ASPD = entry.ReadFloat("aspd", 0);
			data.MSPD = entry.ReadFloat("mspd", 0);
			data.KnockdownPower = entry.ReadFloat("knockdownPower", 0);
			data.MHp = entry.ReadFloat("maxHp", 0);
			data.MSp = entry.ReadFloat("maxSp", 0);
			data.Msta = entry.ReadFloat("maxStamina", 0);
			data.RHp = entry.ReadFloat("recoveryHp", 0);
			data.RSp = entry.ReadFloat("recoverySp", 0);
			data.RSptime = entry.ReadFloat("recoverySpTime", 0);
			data.RSta = entry.ReadFloat("recoverySta", 0);
			data.ClothBonus = entry.ReadFloat("clothDamageBonus", 0);
			data.LeatherBonus = entry.ReadFloat("leatherDamageBonus", 0);
			data.ChainBonus = entry.ReadFloat("chainDamageBonus", 0);
			data.IronBonus = entry.ReadFloat("ironDamageBonus", 0);
			data.AddGhost = entry.ReadFloat("ghostDamageBonus", 0);
			data.AddForester = entry.ReadFloat("foresterDamageBonus", 0);
			data.AddWidling = entry.ReadFloat("widlingDamageBonus", 0);
			data.AddVelias = entry.ReadFloat("veliasDamageBonus", 0);
			data.AddParamune = entry.ReadFloat("paramuneDamageBonus", 0);
			data.AddKlaida = entry.ReadFloat("klaidaDamageBonus", 0);
			data.AddFire = entry.ReadFloat("fireDamageBonus", 0);
			data.AddIce = entry.ReadFloat("iceDamageBonus", 0);
			data.AddPoison = entry.ReadFloat("poisonDamageBonus", 0);
			data.AddLightning = entry.ReadFloat("lightningDamageBonus", 0);
			data.AddEarth = entry.ReadFloat("earthDamageBonus", 0);
			data.AddSoul = entry.ReadFloat("soulDamageBonus", 0);
			data.AddHoly = entry.ReadFloat("holyDamageBonus", 0);
			data.AddDark = entry.ReadFloat("darkDamageBonus", 0);
			data.AddBossAtk = entry.ReadFloat("bossBonusDamage", 0);
			data.BaseSocket = entry.ReadFloat("maxSocketCount", 0);
			data.MaxSocketCount = entry.ReadFloat("maxSocketCount", 0);
			data.BaseSocketMa = entry.ReadFloat("baseSocketMagicAmulet", 0);
			data.MaxSocketMa = entry.ReadFloat("maxSocketMagicAmulet", 0);
			data.MinOption = entry.ReadFloat("minOption", 0);
			data.MaxOption = entry.ReadFloat("maxOption", 0);
			data.MinRDmg = entry.ReadFloat("minRDmg", 0);
			data.MaxRDmg = entry.ReadFloat("maxRDmg", 0);
			data.FDMinR = entry.ReadFloat("fdMinR", 0);
			data.FDMaxR = entry.ReadFloat("fdMaxR", 0);
			data.LifeTime = entry.ReadFloat("lifetime", 0);
			data.ItemLifeTimeOver = entry.ReadFloat("itemLifeTimeOver", 0);
			data.NeedAppraisal = entry.ReadFloat("needAppraisal", 0);
			data.NeedRandomOption = entry.ReadFloat("needRandomOption", 0);
			data.LootingChance = entry.ReadFloat("lootingchance", 0);
			data.IsAlwaysHatVisible = entry.ReadFloat("isAlwaysHatVisible", 0);
			data.SkillWidthRange = entry.ReadFloat("skillWidthRange", 0);
			data.DynamicLifeTime = entry.ReadFloat("dynamicLifeTime", 0);
			data.TeamBelonging = entry.ReadFloat("teamBelonging", 0);
			data.AddDamageAtk = entry.ReadFloat("addDamageAtk", 0);
			data.MagicEarthAtk = entry.ReadFloat("magicEarthAtk", 0);
			data.ResAddDamage = entry.ReadFloat("resAddDamage", 0);
			data.JobGrade = entry.ReadFloat("jobGrade", 0);
			data.MagicIceAtk = entry.ReadFloat("magicIceAtk", 0);
			data.MagicSoulAtk = entry.ReadFloat("magicSoulAtk", 0);
			data.MagicDarkAtk = entry.ReadFloat("magicDarkAtk", 0);
			data.MagicMeleeAtk = entry.ReadFloat("magicMeleeAtk", 0);
			data.MagicFireAtk = entry.ReadFloat("magicFireAtk", 0);
			data.MagicLightningAtk = entry.ReadFloat("magicLightningAtk", 0);
			if (entry.ContainsKey("mainProperties"))
				data.MainProperties = entry.ReadString("mainProperties");

			data.CubeDuplicate = entry.ReadBool("cubeDuplicate");
			data.CubeReopen = entry.ReadBool("cubeReopen");
			data.ReopenDiscount = entry.ReadInt("reopenDiscount");

			if (entry.ContainsKey("equipSkill"))
				data.EquipSkill = entry.ReadString("equipSkill");

			if (entry.TryGetObject("script", out var scriptEntry))
			{
				// We can't really assert that no fields are missing,
				// because thanks to dialog transactions, some items
				// have no script function but do define arguments.
				//scriptEntry.AssertNotMissing("function", "strArg", "numArg1", "numArg2");

				var scriptData = new ItemScriptData();

				scriptData.Function = scriptEntry.ReadString("function");
				scriptData.StrArg = scriptEntry.ReadString("strArg");
				scriptData.NumArg1 = scriptEntry.ReadFloat("numArg1");
				scriptData.NumArg2 = scriptEntry.ReadFloat("numArg2");

				data.Script = scriptData;
			}

			this.AddOrReplace(data.Id, data);
		}

		/// <summary>
		/// Returns the category for the given item.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private static InventoryCategory GetCategory(ItemData data)
		{
			// This data is not included in the client and as far as I
			// can tell there is nothing in the client to simplify this.
			// We just have to manually categorize the items.
			if (Versions.Protocol > 500)
			{
				if (data.Group == ItemGroup.Premium)
				{
					return InventoryCategory.Premium_Consume;
				}

				if (data.Type == ItemType.Equip)
				{
					if (data.Group == ItemGroup.Weapon)
						return InventoryCategory.Weapon_Bow;

					if (data.Group == ItemGroup.SubWeapon)
						return InventoryCategory.Weapon_Dagger;

					if (data.EquipType1 == EquipType.Boots)
						return InventoryCategory.Armor_Boots;

					if (data.EquipType1 == EquipType.Gloves)
						return InventoryCategory.Armor_Gloves;

					if (data.EquipType1 == EquipType.Pants)
						return InventoryCategory.Armor_Pants;

					if (data.EquipType1 == EquipType.Shield)
						return InventoryCategory.Armor_Shield;

					if (data.EquipType1 == EquipType.Shirt)
						return InventoryCategory.Armor_Shirt;

					if (data.EquipType1 == EquipType.Outer)
						return InventoryCategory.Outer;

					if (data.Group == ItemGroup.Helmet)
						return InventoryCategory.Premium_Helmet;

					if (data.Group == ItemGroup.Armband)
						return InventoryCategory.Accessory_Band;

					return InventoryCategory.Armor_Boots;
				}

				if (data.Type == ItemType.Quest)
				{
					// Quest items with Material group go to Misc_Quest category
					if (data.Group == ItemGroup.Material)
						return InventoryCategory.Misc_Quest;

					// All other quest items go to Quest category
					return InventoryCategory.Quest;
				}

				if (data.Group == ItemGroup.Book)
					return InventoryCategory.Consume_Book;

				if (data.Group == ItemGroup.Card)
					return InventoryCategory.Card;

				if (data.Group == ItemGroup.Gem)
					return InventoryCategory.Gem;

				if (data.Group == ItemGroup.Collection)
					return InventoryCategory.Misc_Collect;

				if (data.Type == ItemType.Consume)
				{
					if (data.Group == ItemGroup.Cube)
						return InventoryCategory.Consume_Cube;

					return InventoryCategory.Consume_Potion;
				}

				if (data.Type == ItemType.Recipe)
					return InventoryCategory.Recipe;

				if (data.Group == ItemGroup.Material)
					return InventoryCategory.Misc_Etc;

				// Use Non for unused, so items like money get hidden.
				if (data.Group == ItemGroup.Unused)
					return InventoryCategory.None;

				// Return unused by default, which is labeled as "N/A".
				//return InventoryCategory.Unused;

				// Actually, for unknown reasons not all items appear when put
				// into Unused. Let's use a random category for now.
				return InventoryCategory.Misc_Usual;
			}
			else
			{
				switch (data.Group)
				{
					case ItemGroup.Weapon:
						return InventoryCategory.Weapon;
					case ItemGroup.Armor:
						return InventoryCategory.Armor;
					case ItemGroup.SubWeapon:
						return InventoryCategory.SubWeapon;
					case ItemGroup.Premium:
						return InventoryCategory.Premium;
					case ItemGroup.Quest:
						return InventoryCategory.Quest;
					case ItemGroup.Book:
						return InventoryCategory.Book;
					case ItemGroup.Drug:
						return InventoryCategory.Consume;
					default:
						return InventoryCategory.Unused;
				}
			}
		}

		public bool Exists(int itemId)
		{
			return this.Entries.ContainsKey(itemId);
		}
	}
}
