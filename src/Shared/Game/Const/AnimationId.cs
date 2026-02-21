namespace Melia.Shared.Game.Const
{
	/// <summary>
	/// Names of animations, used for lookup in the animation id db.
	/// </summary>
	public static class AnimationName
	{
		/// <summary>
		/// Player kicks something on the floor. Used for opening chests.
		/// </summary>
		public const string KickBox = "KICKBOX";

		/// <summary>
		/// Chest opens its lid.
		/// Used in Chest Id 147392
		/// </summary>
		public const string Opened = "OPENED";

		/// <summary>
		/// Chest opens its lid.
		/// Used in Chest Ids 147393-147397
		/// </summary>
		public const string Open = "OPEN";

		/// <summary>
		/// Player set to rest animation.
		/// </summary>
		public const string Rest = "REST";

		/// <summary>
		/// Player changes class
		/// </summary>
		public const string ClassChange = "F_pc_class_change";

		/// <summary>
		/// Player level up
		/// </summary>
		public const string LevelUp = "F_pc_joblevel_up";

		/// <summary>
		/// Barrack Unable to Move Lodge
		/// </summary>
		public const string FailedDueToCompanion = "HaveToPutPetAtBarrack";

		/// <summary>
		/// Camp fire
		/// </summary>
		public const string Fire = "F_bg_fire003";

		/// <summary>
		/// White Smoke used in placing furniture
		/// </summary>
		public const string WhiteSmoke = "F_ground068_smoke_white";

		/// <summary>
		/// Used in skills that apply buff via chance?
		/// </summary>
		public const string ShowBuffText = "SHOW_BUFF_TEXT";

		/// <summary>
		/// Used in skills to show critical damage
		/// </summary>
		public const string ShowCriticalDamage = "SHOW_DMG_CRI";

		/// <summary>
		/// Used in snowball skill
		/// </summary>
		public const string DummyTop = "Dummy_top";
		public const string DOWN = "DOWN";
		public const string MissileDead = "MSL_DEAD_C";
		public const string SnowRolling = "F_wizard_snowrolling_ground";

		public const string StoneShower = "E_Cleric_StoneShower";

		/// <summary>
		/// Used in pairing animation for player to sit.
		/// </summary>
		public const string BarrackSit = "BARRACK_SIT";

		/// <summary>
		/// Used to make invisible npc look like sage portal
		/// Looks like F_circle25_{color} is the way to get that effect.
		/// </summary>
		public const string Portal = "F_circle25";
		public const string DelmorePortal = "F_circle25_green";

		/// <summary>
		/// Used with Shield Lob
		/// </summary>
		public const string VioletLight = "I_light004_violet";

		/// <summary>
		/// Empty animation
		/// </summary>
		public const string Empty = "";

		/// <summary>
		/// Sent on channel full, joining another not full channel
		/// </summary>
		public const string JoiningAvailableChannel = "JoiningToAvailableChannel";
	}

	/// <summary>
	/// Constants from fixanim.ies
	/// </summary>
	public enum FixedAnimation
	{
		None = 0,
		STD = 1,
		ASTD = 2,
		HIT = 3,
		HIT2 = 4,
		IDLE = 5,
		READY = 6,
		MOVESACK = 7,
		NOHANDSACK = 8,
		SITDOWN = 9,
		DEAD = 10,
		KNOCKDOWN = 11,
		BORN = 12,
		WLK = 13,
		RUN = 14,
		INJURY1 = 15,
		INJURY2 = 16,
		OPEN1 = 17,
		OPEN2 = 18,
		CLOSE1 = 19,
		CLOSE2 = 20,
		OPEN3 = 21,
		R = 22,
		O = 23,
		Y = 24,
		G = 25,
		B = 26,
		N = 27,
		P = 28,
		BORN_STD = 29,
		ON_R = 30,
		ON_O = 31,
		ON_Y = 32,
		ON_G = 33,
		ON_B = 34,
		ON_N = 35,
		ON_P = 36,
		SIT = 37,
		UNLOCK_STD = 38,
		ON = 39,
		OFF = 40,
		OFF_ = 41,
		LOCK_EVENT = 42,
		LOCK_LOOP = 43,
		SIT3 = 44,
		STD_2 = 45,
		STD_CARE = 46,
		SLEEP = 47,
		IDLE2_LOOP = 48,
		EMPTY = 49,
		SNUFFWLK = 50,
		FALLDOWN = 51,
		UP = 52,
		ON_RED = 53,
		FLICKER = 54,
		FLICKER_RED = 55,
		OPEN = 56,
		SKL_ATAQUE_RUN = 57,
		STD3 = 58,
		FEED_LOOP = 59,
		ATK2_LOOP = 60,
		EVENT_LOOP = 61,
		SKL_BRIQUETTING_STEP1 = 63,
		SKL_BRIQUETTING_ADD1 = 64,
		SKL_BRIQUETTING_ADD2 = 65,
		SKL_BRIQUETTING_STEP_READY = 66,
		SKL_BRIQUETTING_ADD_MATERIAL_READY = 67,
		SKL_BRIQUETTING_ADD_MATERIAL = 68,
		SKL_BRIQUETTING_HARDENING_READY = 69,
		SKL_BRIQUETTING_ADD1_READY = 70,
		SKL_BRIQUETTING_ADD3 = 71,
		SKL_BRIQUETTING_STUCK = 72,
		SKL_BRIQUETTING_HARDENING = 73,
		SKL_BRIQUETTING_STAY = 75,
		SKL_BRIQUETTING_GRIND_LOOP = 76,
		SKL_BRIQUETTING_GRIND_LOOP_READY = 77,
		SKL_MAGNUMOPUS_BORN = 78,
		SKL_MAGNUMOPUS_ADD_MATERIAL = 79,
		SKL_MAGNUMOPUS_ADD_MATERIAL2 = 80,
		SKL_MAGNUMOPUS_ADD1 = 81,
		SKL_MAGNUMOPUS_SHOVEL = 82,
		SKL_MAGNUMOPUS_STAY = 83,
		SKL_MAGNUMOPUS_EXTRACT = 84,
		SKL_MAGNUMOPUS_COVER_CAST = 85,
		SKL_MAGNUMOPUS_COVER_LOOP = 86,
		SKL_MAGNUMOPUS_COVER_SHOT = 87,
		SKL_MAGNUMOPUS_DEAD = 88,
		SKL_ROASTING_BORN = 89,
		SKL_ROASTING_FIRE = 90,
		SKL_ROASTING_ADD1_READY = 91,
		SKL_ROASTING_ADD1 = 92,
		SKL_ROASTING_GRIND1_READY = 93,
		SKL_ROASTING_GRIND1 = 94,
		SKL_ROASTING_SHAKE1 = 95,
		SKL_ROASTING_SHAKE1_READY = 96,
		SKL_ROASTING_CLOSE1_READY = 97,
		SKL_ROASTING_CLOSE1 = 98,
		SKL_ROASTING_OPEN = 99,
		SKL_ROASTING_DEAD = 100,
		SKL_BRIQUETTING_ADD3_READY = 101,
		SKL_BRIQUETTING_ADD3_HOLD = 102,
		SKL_BRIQUETTING_ADD1_HOLD = 103,
		SKL_BRIQUETTING_ADD_MATERIAL_HOLD = 104,
		SKL_TINCTURING_GRIND1_READY = 105,
		SKL_TINCTURING_GRIND1 = 106,
		SKL_TINCTURING_FILTER1 = 107,
		SKL_TINCTURING_FILTER2_READY = 108,
		SKL_TINCTURING_FILTER2 = 109,
		SKL_TINCTURING_SHAKE_READY = 110,
		SKL_TINCTURING_SHAKE = 111,
		SKL_TINCTURING_ADD2 = 112,
		SKL_TINCTURING_TINCTURE = 113,
		SKL_TINCTURING_STD3_READY = 114,
		SKL_TINCTURING_STD3 = 115,
		SKL_TINCTURING_BORN = 116,
		SKL_TINCTURING_DEAD = 117,
		SKL_MAGNUMOPUS_DEAD_READY = 118,
		SKL_MAGNUMOPUS_COVER_CAST_READY = 119,
		SKL_MAGNUMOPUS_STAY_READY = 120,
		SKL_MAGNUMOPUS_STAY_HOLD = 121,
		SKL_ROASTING_CLOSE = 122,
		STDFOOD = 123,
		CIRCLING_READY = 124,
		HOVERING_READY = 125,
		EVENT_BORN = 126,
		ATK = 127,
		leftturn = 132,
		leftturn_loop = 133,
		raid_catapult = 134,
		reday = 135,
		reday_loop = 136,
		shot = 137,
		raid_flying = 138,
		raid_fly_ride = 139,
		raid_fly_std = 140,
		born = 141,
	}
}
