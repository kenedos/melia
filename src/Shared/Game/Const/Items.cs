using System;
using Melia.Shared.Versioning;

namespace Melia.Shared.Game.Const
{
	/// <summary>
	/// Inventory categories.
	/// </summary>
	/// <remarks>
	/// Index for our InvBaseId database. All values that can be found
	/// there should also be in this enum.
	/// </remarks>
	public enum InventoryCategory
	{
		None,
		Premium_Consume,
		Premium_Special,
		Premium_Costume,
		Premium_Wing,
		Premium_EffectCostume,
		Look_Wig,
		Premium_Helmet,
		Look_Lens,
		Weapon,
		Weapon_Bow,
		Weapon_Mace,
		Weapon_Musket,
		Weapon_Rapier,
		Weapon_Spear,
		Weapon_Staff,
		Weapon_Sword,
		Weapon_THBow,
		Weapon_THMace,
		Weapon_THSpear,
		Weapon_THStaff,
		Weapon_THSword,
		Weapon_Cannon,
		Weapon_Dagger,
		Weapon_Pistol,
		Armor,
		Armor_Boots,
		Armor_Gloves,
		Armor_Pants,
		Armor_Shield,
		Armor_Shirt,
		Accessory_Neck,
		Accessory_Band,
		HairAcc_Acc1,
		HairAcc_Acc2,
		HairAcc_Acc3,
		Outer,
		Look_Costume,
		Look_Helmet,
		Look_Armband,
		Look_SPCostume,
		ChangeEquip_Mace,
		ChangeEquip_Musket,
		ChangeEquip_Pistol,
		ChangeEquip_Rapier,
		ChangeEquip_Shield,
		ChangeEquip_Staff,
		ChangeEquip_Sword,
		ChangeEquip_THBow,
		ChangeEquip_THMace,
		ChangeEquip_THStaff,
		ChangeEquip_THSword,
		Look_Toy,
		OPTMisc_Jewel,
		Consume,
		Consume_ConsumeSp,
		Consume_Potion,
		Consume_Scroll,
		Consume_Cube,
		Consume_Jungtan,
		Consume_PopUp,
		Consume_Orgel,
		Consume_Toy,
		Gem,
		Gem_GemAddExp,
		Gem_GemRed,
		Gem_GemGreen,
		Gem_GemBlue,
		Gem_GemYellow,
		Gem_GemWhite,
		Gem_GemSkill,
		Card_Summon,
		Card_CardAddExp,
		Card_CardGoddess,
		Card_CardLeg,
		Card_CardGreen,
		Card_CardRed,
		Card_CardBlue,
		Card,
		Card_CardPurple,
		Misc_Special,
		Misc_MiscSkill,
		OPTMisc_IcorArmor,
		OPTMisc_IcorWeapon,
		Misc_Mineral,
		Misc_Usual,
		Misc_Etc,
		Misc_Quest,
		Quest,
		Misc_Collect,
		Recipe_Weapon,
		Recipe_Armor,
		Recipe_Accessory,
		Recipe_Premium,
		Recipe,
		PetWeapon,
		PetArmor,
		Unused,
		ChangeEquip_Cannon,
		ChangeEquip_Spear,
		ChangeEquip_THSpear,
		ChangeEquip_Bow,
		ChangeEquip_Dagger,
		Look_Wing,
		Accessory_Seal,
		Premium_Doll,
		HiddenAbility_Misc,
		HiddenAbility_Cube,
		HiddenAbility_Collection,
		HiddenAbility_Swordbook,
		HiddenAbility_Wizardbook,
		HiddenAbility_Clericbook,
		HiddenAbility_Scoutbook,
		HiddenAbility_Archerbook,
		Housing_Furniture,
		Housing_Laboratory,
		Housing_Contract,
		Consume_Book,
		OPTMisc_IcorWeaponSword,
		OPTMisc_IcorWeaponTHSword,
		OPTMisc_IcorWeaponMace,
		OPTMisc_IcorWeaponTHMace,
		OPTMisc_IcorWeaponBow,
		OPTMisc_IcorWeaponTHBow,
		OPTMisc_IcorWeaponPistol,
		OPTMisc_IcorWeaponMusket,
		OPTMisc_IcorWeaponSpear,
		OPTMisc_IcorWeaponTHSpear,
		OPTMisc_IcorWeaponDagger,
		OPTMisc_IcorWeaponCannon,
		OPTMisc_IcorWeaponStaff,
		OPTMisc_IcorWeaponTHStaff,
		OPTMisc_IcorWeaponRapier,
		OPTMisc_IcorArmorShield,
		OPTMisc_IcorArmorShirt,
		OPTMisc_IcorArmorPants,
		OPTMisc_IcorArmorGloves,
		OPTMisc_IcorArmorBoots,
		Look_Skin,
		PHousing_Furniture,
		PHousing_Carpet,
		PHousing_Wall,
		Weapon_Trinket,
		Accessory_Ark,
		OPTMisc_IcorWeaponTrinket,
		Look_Designcut,
		Ancient_Card,
		Ancient_CardBook,
		Ancient_Etc,
		Relic,
		Gem_Relic_Cyan,
		Gem_Relic_Magenta,
		Gem_Relic_Black,
		Weapon_Arcane,
		Gem_High_Color,
		Accessory_Earring,
		Accessory_Core,
		Pharmacy_Material,
		Pharmacy_Additive,
		Pharmacy_PharmacyRecipe,
		Armor_Belt,
		OPTMisc_GoddessIcorWeapon,
		OPTMisc_GoddessIcorArmor,
		Armor_Shoulder,
		SubWeapon,
		Accessory,
		Collection,
		Book,
		Etc,
		Premium,
	}

	public enum CardUseType
	{
		Always,
		Attack,
		Attacked,
		BeforeAttack,
		Dead,
		Kill,
		UseItem,
	}

	public enum CardGroup
	{
		None,
		ATK,
		DEF,
		GODDESS,
		LEG,
		REINFORCE_CARD,
		REINFORCE_GODDESS_CARD,
		STAT,
		UTIL,
	}

	/// <summary>
	/// An item's type.
	/// </summary>
	/// <remarks>
	/// Field "type" in our data, "ItemType" in the client.
	/// </remarks>
	public enum ItemType
	{
		Consume,
		Equip,
		Quest,
		Etc,
		Unused,
		Recipe,
		PetWeapon,
		PetArmor
	}

	/// <summary>
	/// An item's group.
	/// </summary>
	/// <remarks>
	/// Field "group" in our data, "GroupName" in the client.
	/// </remarks>
	public enum ItemGroup
	{
		Drug,
		Material,
		Premium,
		Cube,
		Event,
		Collection,
		Card,
		Quest,
		LegendMaterial,
		MagicAmulet,
		SpecialMaterial,
		FishingRod,
		PasteBait,
		Book,
		Unused,
		Misc,
		Recipe,
		Icor,
		ExpOrb,
		SubExpOrb,
		Helmet,
		Armband,
		SubWeapon,
		Armor,
		Weapon,
		Arcane,
		Seal,
		Ark,
		Relic,
		Entrance_Ticket,
		Consume,
		Gem,
		Gem_High_Color,
		Gem_Relic,
		HiddenAbility,
		PetWeapon,
		PetArmor,
		Earring,
		BELT,
		SHOULDER,
		CORE,
		Item_Consume,
	}

	/// <summary>
	/// An item's equip type.
	/// </summary>
	/// <remarks>
	/// Field "equipType1/2" in our data, "ClassType(2)" in the client.
	/// </remarks>
	public enum EquipType
	{
		None,
		Sword,
		THSword,
		Dagger,
		Staff,
		THStaff,
		Bow,
		THBow,
		Mace,
		THMace,
		Spear,
		THSpear,
		Rapier,
		Musket,
		Gun,
		Pistol,
		Cannon,
		Shield,
		Charm,
		Outer,
		Pants,
		Gloves,
		Boots,
		Shirt,
		Armband,
		Helmet,
		Hat,
		Premium,
		Neck,
		Ring,
		Hair,
		Lens,
		Wing,
		Artefact,
		SpecialCostume,
		EffectCostume,
		Seal,
		Trinket,
		Relic,
		Skin,
		Fishing,
		Arcane,
		Ark,
		Doll,
		Earring,
		BELT,
		SHOULDER,
		CORE,
		// Legacy
		Wand,
	}

	/// <summary>
	/// An item's equip exp group.
	/// </summary>
	/// <remarks>
	/// Field "equipExpGroup" in our data, "EquipXpGroup" in the client.
	/// </remarks>
	public enum EquipExpGroup
	{
		None,
		Blessed,
		Card,
		Card_dummy01,
		Card_dummy01_Event,
		Card_Fish_300,
		Card_Gacha_dummy01,
		Card_xp_100,
		Equip,
		Gem,
		GemExp_5000,
		GemExp_randomQuest1,
		GemExp_randomQuest2,
		GemExp_randomQuest3,
		GemExp_randomQuest4,
		GemExp_randomQuest5,
		GemExp_Talt01,
		GemExpStone01,
		GemExpStone02,
		GemExpStone03,
		GemExpStone04,
		GemExpStone05,
		GemExpStone07,
		GemExpStone09,
		GemExpStone10,
		GemExpStone11,
		GemExpStone12,
		Gem_Skill,
		Goddess_Card,
		Goddess_ReinForce_Card,
		hethran_material,
		KQ_token_hethran_1,
		KQ_token_hethran_2,
		KQ_token_hethran_3,
		Legend_Card,
		Legend_Reinforce_Card_MaxLv4,
		Legend_Reinforce_Card_MaxLv6,
		Legend_Reinforce_Card_MaxLv8,
		Legend_Reinforce_Card_MaxLv10,
		Lv10_Card_TP,
		Misc,
		Old_Card_TP,
	}

	public enum ReinforceType
	{
		None,
		Card,
		Gem,
		Goddess,
		GoddessCard,
		GoddessReinForceCard,
		Hethran,
		LegendCard,
		Moru,
		ReinForceCard,
	}


	/// <remarks>
	/// We're using this enum to save which slot an item is equipped on,
	/// should the values change, the values in the database would be
	/// incorrect. Should this enum change again, we should think about
	/// saving the slots as string or creating an intermediary enum.
	/// </remarks>
	public enum EquipSlot : byte
	{
		None = 0xFF,
		HairAccessory = 0, // HAT
		SubsidiaryAccessory, // HAT_L
		Hair, // HAIR
		Top, // SHIRT
		Gloves, // GLOVES
		Shoes, // BOOTS
		Helmet, // HELMET
		Armband, // ARMBAND
		RightHand, // RH
		LeftHand, // LH
		Outer1, // OUTER
		Ring1, // OUTERADD1
		Ring2, // OUTERADD2
		Outer2, // BODY
		Pants, // PANTS
		Ring3, // PANTSADD1
		Ring4, // PANTSADD2
		Bracelet1, // RING1
		Bracelet2, // RING2
		Necklace, // NECK
		Hat, // HAT_T
		Lens, // LENS
		Wing, // WING
		SpecialCostume, // SPECIAL_COSTUME
		EffectCostume, // EFFECT_COSTUME
		Seal, // SEAL
		Doll, // DOLL
		Ark, // ARK
		Trinket, // TRINKET
		Relic, // RELIC
		RightHandSub, // RH_SUB
		LeftHandSub, // LH_SUB
		Earring, // EARRING
		Belt, // BELT
		Shoulder, // SHOULDER
		Core, // CORE
		Last,
	}

	public enum InventoryItemRemoveMsg : byte
	{
		Given = 0, // 0,6,10
		Sold = 1,
		PaidWith = 2,
		Destroyed = 4,
		Equipped = 5,
		Used = 7, // 7,8
	}

	public enum InventoryType : byte
	{
		Inventory = 0,
		PersonalStorage = 1,
		Sold = 3,
		TeamStorage = 6,
		Guild = 7,
	}

	public enum InventoryAddType : byte
	{
		New = 0,
		Buy = 1,
		Sell = 2,
		PickUp = 3, // ?
		NotNew = 4,
	}

	public static class InventoryDefaults
	{
		/// <summary>
		/// Ammount of equip slots.
		/// </summary>
		/// <remarks>
		/// [i11025]  20->21, hair
		/// [i170175] 21->22, lens
		/// [i171032] 22->23, wings
		/// [i184075] 23->24, special costume
		/// [i186893] 24->25, effect costume
		/// [i337645] 25->30
		/// [i339415] 30->32
		/// [i354444] 32->33
		/// [i361296] 33->34
		/// [i367470] 34->35, shoulder
		/// [i398704] 35->36, core
		/// </remarks>
		public static int EquipSlotCount
		{
			get
			{
				if (Versions.Client < 11025)
					return 20;
				else if (Versions.Client >= 11025 && Versions.Client < 170175)
					return 21;
				else if (Versions.Client >= 170175 && Versions.Client < 171032)
					return 22;
				else if (Versions.Client >= 171032 && Versions.Client < 184075)
					return 23;
				else if (Versions.Client >= 184075 && Versions.Client < 186893)
					return 24;
				else if (Versions.Client >= 186893 && Versions.Client < 337645)
					return 25;
				else if (Versions.Client >= 337645 && Versions.Client < 339415)
					return 30;
				else if (Versions.Client >= 339415 && Versions.Client < 354245)
					return 32;
				else if (Versions.Client >= 354245 && Versions.Client < 361296)
					return 33;
				else if (Versions.Client >= 361296 && Versions.Client < 367470)
					return 34;
				else if (Versions.Client >= 367470 && Versions.Client < 398686)
					return 35;
				else
					return 36;
			}
		}

		/// <summary>
		/// Ids of the items equipped by default.
		/// (Literally empty items, NoHat, NoWeapon, etc.)
		/// </summary>
		public static int[] EquipItems
		{
			get
			{
				if (Versions.Client < 11025)
					return [2, 4, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 9, 9, 9, 9, 9, 10, 2];
				else if (Versions.Client >= 11025 && Versions.Client < 170175)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 9, 9, 9, 9, 9, 10, 2];
				else if (Versions.Client >= 170175 && Versions.Client < 171032)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4];
				else if (Versions.Client >= 171032 && Versions.Client < 184075)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4];
				else if (Versions.Client >= 184075 && Versions.Client < 186893)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4];
				else if (Versions.Client >= 186893 && Versions.Client < 337645)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4];
				else if (Versions.Client >= 337645 && Versions.Client < 339415)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4];
				else if (Versions.Client >= 354245 && Versions.Client < 361296)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4];
				else if (Versions.Client >= 365941 && Versions.Client < 367470)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4, 4];
				else if (Versions.Client >= 367470 && Versions.Client < 398686)
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4, 4, 4];
				else
					return [2, 2, 12101, 8, 6, 7, 10000, 11000, 9999996, 9999996, 4, 9, 9, 4, 5, 9, 9, 9, 9, 10, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 9999996, 9999996, 4, 4, 4, 4];
			}
		}
	}

	public enum InventoryOrder : byte
	{
		Price = 2,
		Grade = 0,
		Weight = 1,
		Name = 3,

		Id = 0xFF,
	}

	/// <summary>
	/// Item grades, used with equips.
	/// </summary>
	public enum ItemGrade
	{
		None = 0,
		Normal = 1,
		Magic = 2,
		Rare = 3,
		Unique = 4,
		Legend = 5,
		Goddess = 6,
	}

	/// <summary>
	/// Ids of commonly used items.
	/// </summary>
	public static class ItemId
	{
		public const int Mic = 645001;
		public const int Talt = 645268;
		public const int Silver = 900011;
		public const int Gold = 900012;
		public const int Misc_Soil = 645621;
	}

	/// <summary>
	/// Specifies which hats are visible for a character.
	/// </summary>
	[Flags]
	public enum VisibleEquip
	{
		None = 0x00,

		Headgear1 = 0x01,
		Headgear2 = 0x02,
		Headgear3 = 0x04,
		Wig = 0x08,
		SubWeapon = 0x10,

		All = 0xFFFF,
	}
}
