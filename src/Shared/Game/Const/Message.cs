namespace Melia.Shared.Game.Const
{
	/// <summary>
	/// Used in BC_MESSAGE.
	/// </summary>
	public enum MsgType : byte
	{
		Text = 0,
		UsernameOrPasswordIncorrect1 = 1, // Username?
		UsernameOrPasswordIncorrect2 = 2, // Password?
		DoubleLogin = 3, // Disconnects gracefully
		WrongVerificationCode = 4,
		ServerVersionDoesNotMatch = 5,
		NameAlreadyExists = 6,
		CannotCreateCharacter = 7,
		CharacterNameTooShort = 8, // "The character name must be at least %s characters.
								   // 9
		CannotDeleteCharacter1 = 10,
		CreateCharFail = 11,
		CharacterNameTooLong = 12, // "The character name must be less than %s characters.
		CannotDeleteCharacter2 = 13,
		TeamDoesNotExist = 14,
		CharacterDoesNotExist = 15,
		CannotStartGame = 16,
		NewJobAdded = 17,
		// 18
		TeamMemberCapExceeded = 19,
		InsufficientCharacterPoints = 20,
		AccountBlocked1 = 21,
		AccountBlocked2 = 22,
		InventoryFull = 23,
		StartingMapNotAvailable = 24,
		UnableToDeletePartyLeader = 25,
		NotCBTUser = 26,
		RatedEsrbTeen = 27,
		InvalidIpf = 28, // Client IPF files are invalid.

		// ...
	}

	/// <summary>
	/// Constant strings that may be sent with the packet `ZC_ADDON_MSG`.
	/// </summary>
	public static class AddonMessage
	{
		/// <summary>
		/// Used for quitting the game, transitioning from the zone to the barrack, and possibly other reasons.
		/// </summary>
		public const string EXPIREDITEM_ALERT_OPEN = "EXPIREDITEM_ALERT_OPEN";
		public const string GAMEEXIT_TIMER_END = "GAMEEXIT_TIMER_END";

		public const string RESET_STAT_UP = "RESET_STAT_UP";
		public const string RESET_SKL_UP = "RESET_SKL_UP";
		public const string INV_ITEM_CHANGE_COUNT = "INV_ITEM_CHANGE_COUNT";
		public const string INV_ITEM_POST_REMOVE = "INV_ITEM_POST_REMOVE";
		public const string FAIL_SHOP_BUY = "FAIL_SHOP_BUY";

		public const string RESET_ABILITY_UP = "RESET_ABILITY_UP";
		public const string SUCCESS_BUY_ABILITY_POINT = "SUCCESS_BUY_ABILITY_POINT";
		public const string RESET_ABILITY_ACTIVE = "RESET_ABILITY_ACTIVE";

		/// <summary>
		/// Update Account Warehouse Item List?
		/// </summary>
		public const string ACCOUNT_WAREHOUSE_ITEM_LIST = "ACCOUNT_WAREHOUSE_ITEM_LIST";
		/// <summary>
		/// Update Account?
		/// </summary>
		public const string ACCOUNT_UPDATE = "ACCOUNT_UPDATE";

		/// <summary>
		/// Sent when successfully an ability is learned
		/// </summary>
		public const string SUCCESS_LEARN_ABILITY = "SUCCESS_LEARN_ABILITY";

		/// <summary>
		/// Used for ability point update
		/// </summary>
		public const string UPDATE_ABILITY_POINT = "UPDATE_ABILITY_POINT";

		/// <summary>
		/// Used for experience rate changes due to events
		/// StrArg: GoldenFishEvent
		/// </summary>
		/// <remarks>Parameter: 
		/// Event</remarks>
		public const string UPDATE_EXP_UP = "UPDATE_EXP_UP";

		/// <summary>
		/// Opens the event banner.
		/// </summary>
		public const string DO_OPEN_EVENTBANNER_UI = "DO_OPEN_EVENTBANNER_UI";
		public const string SET_CHAT_MACRO_DEFAULT = "SET_CHAT_MACRO_DEFAULT";
		public const string FISHING_SUCCESS_COUNT = "FISHING_SUCCESS_COUNT"; // Parameter: 0

		/// <summary>
		/// The UI for first time players showing how to move using the keyboard.
		/// </summary>
		public const string KEYBOARD_TUTORIAL = "KEYBOARD_TUTORIAL";
		/// <summary>
		/// The UI for first time players showing how to talk to npcs.
		/// </summary>
		public const string DIALOG_SPACE_TUTORIAL_LIME3 = "DIALOG_SPACE_TUTORIAL_LIME3";
		public const string MINIMIZED_TUTORIALNOTE_EFFECT_CHECK = "MINIMIZED_TUTORIALNOTE_EFFECT_CHECK"; // Parameter: guide_1

		/// <summary>
		/// Enable PC Bang Shop (Popo Shop)
		/// </summary>
		public const string ENABLE_PCBANG_SHOP = "ENABLE_PCBANG_SHOP";

		public const string UPDATE_GUILD_MILEAGE = "UPDATE_GUILD_MILEAGE";
		public const string UPDATE_ATTENDANCE_REWARD = "UPDATE_ATTENDANCE_REWARD";

		/// <summary>
		/// The UI for new entries in adventure book
		/// </summary>
		public const string ADVENTURE_BOOK_NEW = "ADVENTURE_BOOK_NEW"; // Parameter: @dicID_^*$ETC_20150317_000001$*^
		public const string SET_COIN_GET_GAUGE = "SET_COIN_GET_GAUGE";

		/// <summary>
		/// Related to changing jobs (classes)
		/// </summary>
		public const string JOB_UPDATE = "JOB_UPDATE";
		public const string START_JOB_CHANGE = "START_JOB_CHANGE";

		/// <summary>
		/// Opens the TP shop UI Help
		/// </summary>
		public const string TP_SHOP_UI_OPEN = "TP_SHOP_UI_OPEN";

		/// <summary>
		/// Sets the personal house name
		/// </summary>
		public const string SET_PERSONAL_HOUSE_NAME = "SET_PERSONAL_HOUSE_NAME";

		/// <summary>
		/// Enter personal house
		/// </summary>
		public const string ENTER_PERSONAL_HOUSE = "ENTER_PERSONAL_HOUSE";

		/// <summary>
		/// Housing Craft? (Furniture Making or Alchemy)
		/// </summary>
		public const string HOUSINGCRAFT_UPDATE_ENDTIME = "HOUSINGCRAFT_UPDATE_ENDTIME";

		/// <summary>
		/// Event reward notification that you've received an item
		/// </summary>
		public const string EVENT_REWARD_NOTIFY_ITEM_GET = "EVENT_REWARD_NOTIFY_ITEM_GET"; // Parameter: !@#$PH_POINT_SHOP_POINT_TRADE_SUCCESS_1#@!;@dicID_^*$ITEM_20200129_025656$*^

		/// <summary>
		/// Party Join
		/// </summary>
		public const string PARTY_JOIN = "PARTY_JOIN";
		/// <summary>
		/// Party Invite Rejected
		/// </summary>
		public const string PARTY_INVITE_CANCEL = "PARTY_INVITE_CANCEL";
		public const string PARTY_UPDATE = "PARTY_UPDATE";
		public const string SUCCESS_UPDATE_PARTY_INFO = "SUCCESS_UPDATE_PARTY_INFO";

		/// <summary>
		/// Force help message to close?
		/// </summary>
		public const string HELP_MSG_CLOSE = "HELP_MSG_CLOSE";

		/// <summary>
		/// The UI notification for map exploration completed
		/// </summary>
		public const string MAP_EXPLORE_NOTIFY_COMPLETE = "MAP_EXPLORE_NOTIFY_COMPLETE"; // Parameter: @dicID_^*$ETC_20150317_001157$*^ (Crystal Mine 3F)

		/// <summary>
		/// Open Beauty Shop UI
		/// </summary>
		public const string BEAUTYSHOP_UI_OPEN = "BEAUTYSHOP_UI_OPEN"; // Parameter: HAIR/SKIN/WIG/DESIGNCUT/ETC
		public const string UPDATE_BEAUTY_COUPON_STAMP = "UPDATE_BEAUTY_COUPON_STAMP"; // Parameter: None

		/// <summary>
		/// Enable Create Guild?
		/// </summary>
		public const string ENABLE_CREATE_GUILD_NAME = "ENABLE_CREATE_GUILD_NAME";
		/// <summary>
		/// Guild Promotion Notice (aka Guilds are available to join?)
		/// </summary>
		public const string GUILD_PROMOTE_NOTICE = "GUILD_PROMOTE_NOTICE";
		/// <summary>
		/// Guild event recruiting joined
		/// </summary>
		public const string GUILD_EVENT_RECRUITING_IN = "GUILD_EVENT_RECRUITING_IN";
		public const string GUILD_EVENT_RECRUITING_ADD = "GUILD_EVENT_RECRUITING_ADD";
		/// <summary>
		/// Guild Hangout Preview?
		/// </summary>
		public const string RECEIVE_OTHER_GUILD_AGIT_INFO = "RECEIVE_OTHER_GUILD_AGIT_INFO";

		/// <summary>
		/// Notice to UI?
		/// </summary>
		public const string NOTICE_TO_UI = "NOTICE_TO_UI"; // parameter: CURSE/headicon_expression_02/113558/0 (uiName, iconName, handle, duration)

		/// <summary>
		/// Shows a scroll with a message
		/// </summary>
		public const string NOTICE_Dm_Exclaimation = "NOTICE_Dm_!"; // parameter: !@#$CantUseInMaxLv#@! (frame, msg, argStr, argNum)  
		/// <summary>
		/// Shows a scroll with a message
		/// </summary>
		public const string NOTICE_Dm_Scroll = "NOTICE_Dm_scroll"; // parameter: !@#$JournalMapFogPreReward$*$ZONE$*$@dicID_^*$ETC_20150317_001151$*^$*$PERCENT$*$19.7#@!
		/// <summary>
		/// Cleared message
		/// <example>Used with a parameter !@#$JournalMapFogRewardAll$*$ZONE$*$@dicID_^*$ETC_20200128_043943$*^#@! or !@#$Episode_Clear_Msg$*$EPNumber$*$Episode 12-2$*$EPName$*$@dicID_^*$ETC_20200710_048476$*^#@!</example>
		/// </summary>
		public const string NOTICE_Dm_Clear = "NOTICE_Dm_Clear";      // Example Parameter: !@#$JournalMapFogRewardAll$*$ZONE$*$@dicID_^*$ETC_20200128_043943$*^#@!

		/// <summary>
		/// Event (GM Login Event) Property Id: 111052
		/// </summary>
		public const string NOTICE_Dm_fanfare = "NOTICE_Dm_fanfare"; // Example Parameter: !@#$EVENT_2209_ATTENDANCE_CHECK_MESSAGE{TIME}$*$TIME$*$117#@!

		/// <summary>
		/// Get Item
		/// </summary>
		public const string NOTICE_Dm_GetItem = "NOTICE_Dm_GetItem"; // Example Parameter: !@#$GET_KEDORA_SUPPORT_BOX_MSG_1#@!

		/// <summary>
		/// ? Return Button for?
		/// </summary>
		public const string ENABLE_RETURN_BUTTON = "ENABLE_RETURN_BUTTON";

		/// <summary>
		/// Ancient Card Start/Open from Gacha
		/// </summary>
		public const string ANCIENT_CARD_GACHA_START = "ANCIENT_CARD_GACHA_START"; // param: 1/1/1/1/1/
		public const string ANCIENT_CARD_GACHA_CARD_OPEN = "ANCIENT_CARD_GACHA_CARD_OPEN"; // param: Ancient_Colimencia

		/// <summary>
		/// Weekly Boss DPS Addon
		/// </summary>
		public const string WEEKLY_BOSS_DPS_START = "WEEKLY_BOSS_DPS_START"; // param: 97185;97191;97192;/PRACTICE
		public const string WEEKLY_BOSS_DPS_END = "WEEKLY_BOSS_DPS_END";
		/// <summary>
		/// Start Boss Battle
		/// Parameter: Boss's Handle
		/// </summary>
		public const string BOSS_BATTLE_START = "BOSS_BATTLE_START"; // param: 232560, monster's handle
		/// <summary>
		/// Boss Cleared
		/// </summary>
		/// <remarks>
		/// Int Parameter: Boss's Monster Id,
		/// Str Parameter: Boss's Handle
		/// </remarks>
		public const string BOSS_CLEAR = "BOSS_CLEAR";

		/// <summary>
		/// Instance Dungeon Related Addons
		/// </summary>
		public const string OPEN_INDUN_REWARD_HUD = "OPEN_INDUN_REWARD_HUD"; // param: Indun_rokas1
		public const string REFRESH_INDUN_REWARD_HUD = "REFRESH_INDUN_REWARD_HUD"; // param: Indun_rokas1
		public const string INDUN_REWARD_RESULT = "INDUN_REWARD_RESULT"; // param: 100#0#misc_BlessedStone;1/misc_talt#6#276659552#256121984#0#0#
		public const string QUEST_UPDATE_ = "QUEST_UPDATE_";
		public const string ON_QUEST_UPDATED = "ON_QUEST_UPDATED";
		public const string ON_DESTROY_NPC_ICON = "ON_DESTROY_NPC_ICON";

		/// <summary>
		/// Show Crystal in top left HUD, used during raids
		/// param: unlimited
		/// </summary>
		public const string SHOW_SOUL_CRISTAL = "SHOW_SOUL_CRISTAL";

		/// <summary>
		/// ? Pre-raid starting npc
		/// params: irredians_level
		/// </summary>
		public const string STYLE_INFO_UPDATE = "STYLE_INFO_UPDATE";

		/// <summary>
		/// Params set by server?
		/// </summary>
		/// <example>
		/// string.Format(AddonMessage.ANCIENT_SOLO_SET_PARAM_BY_SERVER, "1", "None", 0, 20);
		/// </example>
		public const string ANCIENT_SOLO_SET_PARAM_BY_SERVER = "ANCIENT_SOLO_SET_PARAM_BY_SERVER(\"{0}\",\"{1}\",{2},{3})";

		/// <summary>
		/// Params: Str: "None", Num: 1
		/// </summary>
		public const string ANCIENT_SOLO_MONSTER_DEAD = "ANCIENT_SOLO_MONSTER_DEAD";

		/// <summary>
		/// Appeared after new journal
		/// </summary>
		public const string RESET_ACHIEVE_EXCHANGE_EVENT = "RESET_ACHIEVE_EXCHANGE_EVENT";

		/// <summary>
		/// Update Mercenary Badges
		/// </summary>
		/// <remarks>param: 0/166</remarks>
		public const string EARTHTOWERSHOP_BUY_ITEM_RESULT = "EARTHTOWERSHOP_BUY_ITEM_RESULT";

		/// <summary>
		/// Trade Shop Buy Item
		/// </summary>
		/// <remarks>param: 0/166</remarks>
		public const string EARTHTOWERSHOP_BUY_ITEM = "EARTHTOWERSHOP_BUY_ITEM";

		/// <summary>
		/// Challenge Mode Total Kill Count
		/// </summary>
		/// <remarks>SHOW#1#</remarks>
		public const string UI_CHALLENGE_MODE_TOTAL_KILL_COUNT = "UI_CHALLENGE_MODE_TOTAL_KILL_COUNT";

		/// <summary>
		/// Blessed Cube Disabled
		/// </summary>
		public const string BLESSED_CUBE_NOT_ENABLE = "BLESSED_CUBE_NOT_ENABLE";

		/// <summary>
		/// Free Random Option
		/// </summary>
		public const string MSG_SUCCESS_FREE_RANDOM_OPTION = "MSG_SUCCESS_FREE_RANDOM_OPTION";

		/// <summary>
		/// After item is appraised
		/// </summary>
		/// <remarks>Parameter: Equip</remarks>
		public const string UPDATE_ITEM_APPRAISAL = "UPDATE_ITEM_APPRAISAL";

		/// <summary>
		/// Market Register an Item
		/// </summary>
		public const string MARKET_REGISTER = "MARKET_REGISTER";

		/// <summary>
		/// Open Dialog for Equipment
		/// </summary>
		public const string OPEN_DLG_REPAIR = "OPEN_DLG_REPAIR";

		/// <summary>
		/// After equipment is repaired
		/// </summary>
		public const string UPDATE_DLG_REPAIR = "UPDATE_DLG_REPAIR";

		/// <summary>
		/// After equipment is repaired
		/// Param: Equip
		/// </summary>
		public const string UPDATE_ITEM_REPAIR = "UPDATE_ITEM_REPAIR";

		/// <summary>
		/// Open Dialog for Socket Management
		/// </summary>
		public const string DO_OPEN_MANAGE_GEM_UI = "DO_OPEN_MANAGE_GEM_UI";

		/// <summary>
		/// After equipment is appraised from NPC
		/// Param: Equip
		/// </summary>
		public const string SUCCESS_APPRALSAL = "SUCCESS_APPRALSAL";

		/// <summary>
		/// Episode Cleared
		/// </summary>
		public const string EPISODE_REWARD_CLEAR = "EPISODE_REWARD_CLEAR";

		/// <summary>
		/// Open Collection UI
		/// </summary>
		public const string COLLECTION_UI_OPEN = "COLLECTION_UI_OPEN";

		/// <summary>
		/// Show tutorial help for dialog activation via spacebar.
		/// </summary>
		public const string DIALOG_SPACE_TUTORIAL = "DIALOG_SPACE_TUTORIAL";

		/// <summary>
		/// Add Chat Emoticon?
		/// </summary>
		public const string ADD_CHAT_EMOTICON = "ADD_CHAT_EMOTICON";

		/// <summary>
		/// Show Contents Alert UI
		/// Used with Goddess Gear Equip
		/// </summary>
		public const string SHOW_CONTENTS_ALERT_UI = "SHOW_CONTENTS_ALERT_UI";

		/// <summary>
		/// Used in exchanging items for content points
		/// </summary>
		public const string ITEM_POINT_EXTRACTOR_EXECUTE = "ITEM_POINT_EXTRACTOR_EXECUTE";

		/// <summary>
		/// Equip item list update?
		/// Sent after anvil enhancement
		/// </summary>
		public const string EQUIP_ITEM_LIST_UPDATE = "EQUIP_ITEM_LIST_UPDATE";

		/// <summary>
		/// Start crafting an item.
		/// </summary>
		public const string JOURNAL_DETAIL_CRAFT_EXEC_START = "JOURNAL_DETAIL_CRAFT_EXEC_START";
		/// <summary>
		/// Successfully crafted an item.
		/// </summary>
		public const string JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS = "JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS";
		/// <summary>
		/// Failed to craft an item.
		/// </summary>
		public const string JOURNAL_DETAIL_CRAFT_EXEC_FAIL = "JOURNAL_DETAIL_CRAFT_EXEC_FAIL";

		/// <summary>
		/// Successfully enhanced a goddess item.
		/// </summary>
		public const string MSG_SUCCESS_GODDESS_REINFORCE_EXEC = "MSG_SUCCESS_GODDESS_REINFORCE_EXEC";

		/// <summary>
		/// Pilgrim Info
		/// Parameter: NO_DATA (Sent on current week, when no rank data is sent?)
		/// </summary>
		public const string RANK_SYSTEM_DATA = "RANK_SYSTEM_DATA";

		/// <summary>
		/// Open Companion Info
		/// </summary>
		public const string COMPANION_UI_OPEN = "COMPANION_UI_OPEN";

		/// <summary>
		/// Update Class Icon
		/// </summary>
		public const string UPDATE_REPRESENTATION_CLASS_ICON = "UPDATE_REPRESENTATION_CLASS_ICON";

		/// <summary>
		/// Update Relic Equip
		/// Parameter: None
		/// </summary>
		public const string UPDATE_RELIC_EQUIP = "UPDATE_RELIC_EQUIP";

		/// <summary>
		/// Open Bounty Hunt
		/// </summary>
		public const string BOUNTYHUNT_OPEN = "BOUNTYHUNT_OPEN";

		/// <summary>
		/// Quest Reward Select Dialog
		/// </summary>
		public const string SHOW_QUEST_SEL_DLG = "SHOW_QUEST_SEL_DLG";

		/// <summary>
		/// Silver notice after item is sold.
		/// </summary>
		public const string RECEIVABLE_SILVER_NOTICE = "RECEIVABLE_SILVER_NOTICE";

		/// <summary>
		/// Chat Translation?
		/// </summary>
		public const string RECEIVE_SERVER_NATION = "RECEIVE_SERVER_NATION";

		/// <summary>
		/// Quickslot Change Skill?
		/// </summary>
		public const string QUICKSLOT_CHANGE_SKILL_UPDATE = "QUICKSLOT_CHANGE_SKILL_UPDATE";

		/// <summary>
		/// Goddess Roulette Open
		/// </summary>
		public const string GODDESS_ROULETTE_OPEN = "GODDESS_ROULETTE_OPEN";

		/// <summary>
		/// Goddess Roulette Start (String Arg: 16/C/misc_reinforce_percentUp_480_NoTrade/3)
		/// </summary>
		public const string GODDESS_ROULETTE_START = "GODDESS_ROULETTE_START";

		/// <summary>
		/// Goddess Roulette State Update (String Arg: goddess_roulette_board_05;15)
		/// </summary>
		public const string GODDESS_ROULETTE_STATE_UPDATE = "GODDESS_ROULETTE_STATE_UPDATE";

		/// <summary>
		/// Goddess Roulette Item Update
		/// String Arg: SSS:aae69a/1/aae748/1/;SS:aaea0a/1/aae749/1/;S:aaea0f/20/aaea09/1/aaea08/1/aaea0c/1/aaea0b/1/a86350/1/a86341/10/aae6a0/10/;A:aae6a9/20/aae6a7/30/aae6c4/20/aae6c2/30/996af/1/996b0/1/a86341/2/aae6a0/2/;B:a84e07/1/aaea07/3/9c59c/10/9c59d/10/78b12/2/77b47/5/a86341/1/aae6a0/1/;C:aae6ca/1/a8641d/3/9c5bf/10/9c5c0/10/78b12/1/77b47/1/a86311/1/aae69f/1/;
		/// </summary>
		public const string GODDESS_ROULETTE_ITEM_UPDATE = "GODDESS_ROULETTE_ITEM_UPDATE";

		/// <summary>
		/// Necromancer Corpse Parts Update
		/// </summary>
		public const string UPDATE_NECRONOMICON_UI = "UPDATE_NECRONOMICON_UI";

		/// <summary>
		/// An effect played when you unlock Legendary Card Slot?
		/// </summary>
		public const string MSG_PLAY_LEGENDCARD_OPEN_EFFECT = "MSG_PLAY_LEGENDCARD_OPEN_EFFECT";

		/// <summary>
		/// Update raid timer text gauge?
		/// StrArg: RemainTime;Yellow
		/// </summary>
		public const string RAID_TIMER_TEXT_GAUGE_UPDATE = "RAID_TIMER_TEXT_GAUGE_UPDATE";

		/// <summary>
		/// Update Black Market Item
		/// StrArg: 2023-12-22 06:00:00;2023-12-23 00:00:00;High_Enchantchip_low_box1000_black;12000000;0;0;4
		/// </summary>
		public const string UPDATE_BLACK_MARKET_ITEM = "UPDATE_BLACK_MARKET_ITEM";

		/// <summary>
		/// Update hair color change.
		/// </summary>
		public const string HAIR_COLOR_CHANGE = "HAIR_COLOR_CHANGE";

		/// <summary>
		/// Show close cube
		/// </summary>
		public const string CLOSE_GACHA_CUBE = "CLOSE_GACHA_CUBE";

		/// <summary>
		/// Show Cabinet Enchant Success
		/// Applying a Vaivora to an equipment
		/// </summary>
		public const string ITEM_CABINET_SUCCESS_ENCHANT = "ITEM_CABINET_SUCCESS_ENCHANT";

		/// <summary>
		/// Reward received from stamp event (Popo Event 2024)
		/// </summary>
		public const string EVENT_STAMP_TOUR_REWARD_GET = "EVENT_STAMP_TOUR_REWARD_GET";
		/// <summary>
		/// Popo boost reward reset?
		/// Seen after receiving reward for Popo Event 2024
		/// Might be used to refresh UI.
		/// </summary>
		public const string POPOBOOST_REWARD_RSET = "POPOBOOST_REWARD_RSET";

		/// <summary>
		/// Update collections enabled/available.
		/// </summary>
		public const string UPDATE_READ_COLLECTION_COUNT = "UPDATE_READ_COLLECTION_COUNT";

		/// <summary>
		/// Item gaining exp
		/// </summary>
		public const string ITEM_EXP_START = "ITEM_EXP_START";

		/// <summary>
		/// Stop Item gaining exp
		/// </summary>
		public const string ITEM_EXP_STOP = "ITEM_EXP_STOP";
		public const string ITEM_EXP_STOP_CERTIFICATE = "ITEM_EXP_STOP_CERTIFICATE";

		/// <summary>
		/// Create a socket
		/// </summary>
		public const string MSG_MAKE_ITEM_SOCKET = "MSG_MAKE_ITEM_SOCKET";
		/// <summary>
		/// Remove a gem
		/// </summary>
		public const string MSG_REMOVE_GEM = "MSG_REMOVE_GEM";

		/// <summary>
		/// Param Num: -Negative Second Value before start 60
		/// Param Str: START
		/// </summary>
		public const string COLONY_ALARM_MSG = "COLONY_ALARM_MSG";
		public const string IN_COLONYWAR_STATE = "IN_COLONYWAR_STATE";

		/// <summary>
		/// Shows a notification telling that the party leader has entered party auto matching queue.
		/// </summary>
		public const string INDUN_ASK_PARTY_MATCH = "INDUN_ASK_PARTY_MATCH";
		/// <summary>
		/// When the player fails to start party auto matching (it's not the leader).
		/// </summary>		
		public const string FAIL_START_PARTY_MATCHING = "FAIL_START_PARTY_MATCHING";
	}

	/// <summary>
	/// Steam Achievements sent on first unlock on account
	/// </summary>
	public static class SteamAchievement
	{
		/// <summary>
		/// Change Class for the first time.
		/// </summary>
		public const string TOS_STEAM_ACHIEVEMENT_CLASS_CHANGE = "TOS_STEAM_ACHIEVEMENT_CLASS_CHANGE";
		/// <summary>
		/// Equipping a costume for the first time.
		/// </summary>
		public const string TOS_STEAM_ACHIEVEMENT_FASHIONISTA = "TOS_STEAM_ACHIEVEMENT_FASHIONISTA";
		/// <summary>
		/// Using an anvil to reinforce an item for the first time.
		/// </summary>
		public const string TOS_STEAM_ACHIEVEMENT_REINFORCE = "TOS_STEAM_ACHIEVEMENT_REINFORCE";

		/// <summary>
		/// Using Goddess Grace (Silver Gacha) for the first time.
		/// </summary>
		public const string TOS_STEAM_ACHIEVEMENT_GOD_PROTECTION = "TOS_STEAM_ACHIEVEMENT_GOD_PROTECTION";
	}

	public static class SystemMessage
	{
		/// <summary>
		/// Count
		/// </summary>
		public const int MERCENARY_BADGE_COUNT = 4273;
		public const int PERSONAL_HOUSE_LEVEL_10_REQUIRED = 815111;
		public const int WEEKLY_TOTAL_POINT_SUCCESS = 590001; // param1: Point : 10, param2: NowPoint : 10, param3: MaxPoint : 5000, param4: TotalPoint : 10
	}

	/// <summary>
	/// Constant strings sent with `BC_NORMAL_Run`.
	/// </summary>
	public static class BarrackMessage
	{
		public const string THEMA_BUY_SUCCESS = "THEMA_BUY_SUCCESS";
	}

	/// <summary>
	/// Constant strings sent with `ZC_EXEC_CLIENT_SCP`.
	/// Representing Lua Functions to call.
	/// </summary>
	public static class ClientScripts
	{
		/// <summary>
		/// Sent on Pet Adoption?
		/// </summary>
		public const string PET_ADOPT_SUCCESS = "PET_ADOPT_SUC()";

		/// <summary>
		/// Use with string.format (1)
		/// </summary>
		public const string TRY_CECK_BARRACK_SLOT_BY_COMPANION_EXCHANGE = "TRY_CECK_BARRACK_SLOT_BY_COMPANION_EXCHANGE({0})";

		/// <summary>
		/// Use with string.format (msg "START",0 or "COUNT",1)
		/// </summary>
		public const string TEAM_BATTLE_ALARM_MSG = "_TEAM_BATTLE_ALARM_MSG(\"{0}\",{1})"; // msg "START",0 or "COUNT",1

		/// <summary>
		/// Event Shop
		/// Use with string.format and provide shop name eg: string.format(REQ_EVENT_SHOP_OPEN_COMMON, "EVENT_2201_SEOLLAL_SHOP");
		/// </summary>
		/// <example>
		/// string.format(REQ_EVENT_SHOP_OPEN_COMMON, "EVENT_2201_SEOLLAL_SHOP");
		/// string.format(REQ_EVENT_SHOP_OPEN_COMMON, "EVENT_2203_ARBOR_DAY_SHOP");
		/// </example>
		public const string REQ_EVENT_SHOP_OPEN_COMMON = "REQ_EVENT_SHOP_OPEN_COMMON(\'{0}\')";

		/// <summary>
		/// Wings of Vaivora Shop
		/// </summary>
		public const string REQ_DAILY_REWARD_SHOP_1_OPEN = "REQ_DAILY_REWARD_SHOP_1_OPEN()";

		/// <summary>
		/// Open Journal from Server Side
		/// </summary>
		public const string OPEN_DO_JOURNAL = "OPEN_DO_JOURNAL()";


		/// <summary>
		/// Forced Get Item?
		/// First Param is Item Id and Second is Amount
		/// Eg: FORCE_GET_ITEM(900011, 126440)
		/// </summary>
		public const string FORCE_GET_ITEM = "FORCE_GET_ITEM({0}, {1})";

		// Emergency Quest		
		/// <summary>
		/// Start a Emergency Quest for monster hunting quest
		/// Usage: SUDDENQUEST_OPEN("1992:900000:30:Orc_cannon;Orc_flag;Orc_glaive:KillMon:SSN_DPK_QUEST")
		/// </summary>
		public const string SUDDENQUEST_OPEN = "SUDDENQUEST_OPEN(\"{0}:{1}:{2}:{3}:{4}:{5}\")";
		/// <summary>
		/// Update Emergency Quest Timer
		/// Usage: SUDDENQUEST_TIME_UPDATE("215000")
		/// </summary>
		public const string SUDDENQUEST_TIME_UPDATE = "SUDDENQUEST_TIME_UPDATE(\"{0}\")";
		/// <summary>
		/// Update Emergency Quest Monster Kill Count
		/// Usage: SUDDENQUEST_MONCOUNT_UPDATE("30")
		/// </summary>
		public const string SUDDENQUEST_MONCOUNT_UPDATE = "SUDDENQUEST_MONCOUNT_UPDATE(\"{0}\")";
		// CZ_DPK_QUEST_DESTORY_SESSIONOBJ
		// On completed
		/// <summary>
		/// Complete Emergency Quest
		/// Usage: SUDDENQUEST_RESET()
		/// </summary>
		public const string SUDDENQUEST_RESET = "SUDDENQUEST_RESET()";
		//Session Object Remove: 700000 and 0

		// TALK_EFFECT_READY_MGS_BOX()
		public const string TALK_EFFECT_READY_MGS_BOX = "TALK_EFFECT_READY_MGS_BOX()";

		/// <summary>
		/// Equipping Res Sacrae?
		/// </summary>
		/// <remarks>Usage: string.format(UPDATE_SKILL_BY_SKILLMAKECOSTUME,"create_skill:Relic_Release")</remarks>
		public const string UPDATE_SKILL_BY_SKILLMAKECOSTUME = "UPDATE_SKILL_BY_SKILLMAKECOSTUME(\"{0}\")";

		/// <summary>
		/// Sage Portal Changes
		/// </summary>
		public const string SAGE_PORTAL_SAVE_SUCCESS = "SAGE_PORTAL_SAVE_SUCCESS()";

		/// <summary>
		/// Demon God Ragana Gacha Shop?
		/// </summary>
		public const string REQ_SILVER_GACHA_SHOP_OPEN = "REQ_SILVER_GACHA_SHOP_OPEN()";

		/// <summary>
		/// Leticia's Mystery Cube
		/// </summary>
		public const string LETICIA_CUBE_OPEN = "LETICIA_CUBE_OPEN()";

		/// <summary>
		/// Squire Repair Shop Cancel
		/// </summary>
		public const string SQUIRE_REPAIR_CANCEL = "SQUIRE_REPAIR_CANCEL()";

		/// <summary>
		/// Enchant Item with Lock?
		/// </summary>
		public const string REINFORCE_131014_ITEM_LOCK = "REINFORCE_131014_ITEM_LOCK(\'{0}\', \'{1}\')";
		public const string HAIRENCHANT_SUCCESS = "HAIRENCHANT_SUCEECD(\'{0}\', \'{1}\')";

		/// <summary>
		/// Transfer Hair Costume Stats 
		/// </summary>
		public const string REQ_BRIQUETTING_HAIR_ACC_UI_OPEN = "REQ_BRIQUETTING_HAIR_ACC_UI_OPEN()";

		/// <summary>
		/// Beauty Coupon
		/// </summary>
		public const string BEAUTY_COUPON_OPEN = "BEAUTY_COUPON_OPEN()";

		/// <summary>
		/// Open Content Points Shop
		/// </summary>
		public const string CONTENTS_TOTAL_SHOP_OPEN = "CONTENTS_TOTAL_SHOP_OPEN()";

		/// <summary>
		/// Open Offseason Content Points Shop
		/// </summary>
		public const string SEASONOFF_CONTENTS_TOTAL_SHOP_OPEN = "SEASONOFF_CONTENTS_TOTAL_SHOP_OPEN()";

		/// <summary>
		/// Open Class Costume Shop
		/// </summary>
		public const string CLASS_COSTUME_TOTAL_SHOP_OPEN = "CLASS_COSTUME_TOTAL_SHOP_OPEN()";

		/// <summary>
		/// Dismantle Mystic Tome
		/// </summary>
		public const string HIDDENABILITY_DECOMPOSE_UI_OPEN = "HIDDENABILITY_DECOMPOSE_UI_OPEN()";

		/// <summary>
		/// Open Fishing Ranking
		/// </summary>
		public const string FISHING_RANK_OPEN = "FISHING_RANK_OPEN()";

		/// <summary>
		/// Show warning before removal of an item.
		/// </summary>
		/// <remarks>Usage: string.format(RANKRESET_DELETE_RANK_CARD,"TP_jexpCard_UpRank2")</remarks>
		public const string RANKRESET_DELETE_RANK_CARD = "RANKRESET_DELETE_RANK_CARD('{0}')";

		/// <summary>
		/// Create Guild UI
		/// </summary>
		public const string OPEN_GUILD_CREATE_UI = "OPEN_GUILD_CREATE_UI()";

		/// <summary>
		/// Open Blessed Cube?
		/// </summary>
		public const string BLESSED_CUBE_OPEN = "BLESSED_CUBE_OPEN()";

		/// <summary>
		/// Open Skill Transmutation
		/// </summary>
		public const string REQ_COMMON_SKILL_ENCHANT_UI_OPEN = "REQ_COMMON_SKILL_ENCHANT_UI_OPEN()";

		/// <summary>
		/// Open Upgrade Accessory
		/// </summary>
		public const string REQ_COMMON_ACC_UPGRADE_UI_OPEN = "REQ_COMMON_ACC_UPGRADE_UI_OPEN()";

		/// <summary>
		/// Magnum Opus
		/// </summary>
		public const string PUZZLE_COMPLETE = "PUZZLE_COMPLETE()";

		/// <summary>
		/// Raid Timer Update
		/// </summary>
		/// <example>
		/// RAID_TIMER_UPDATE(1800, 1212)
		/// </example>
		public const string RAID_TIMER_UPDATE = "RAID_TIMER_UPDATE({0}, {1})";

		/// <summary>
		/// Update "Follower" List
		/// </summary>
		public const string UPDATE_PC_FOLLOWER_LIST = @"UPDATE_PC_FOLLOWER_LIST("")";

		/// <summary>
		/// Open Mercenary Badge Shop
		/// </summary>
		public const string REQ_PVP_MINE_SHOP_OPEN = "REQ_PVP_MINE_SHOP_OPEN()";

		/// <summary>
		/// Friend Invite Mission Event
		/// </summary>
		public const string REQ_FRIEND_INVITE_MISSION = "REQ_FRIEND_INVITE_MISSION()";

		/// <summary>
		/// Close Event Shop (TOS Coin Shop)
		/// </summary>
		public const string EARTH_TOWER_SHOP_TRADE_LEAVE = "EARTH_TOWER_SHOP_TRADE_LEAVE()";

		/// <summary>
		/// Open Warp Map by NPC
		/// </summary>
		public const string INTE_WARP_OPEN_BY_NPC = "INTE_WARP_OPEN_BY_NPC()";

		/// <summary>
		/// Missle Dead C(Client?)
		/// </summary>
		public const string MSL_DEAD_C = "MSL_DEAD_C";

		/// <summary>
		/// Open Black Market UI Open
		/// Parameters: Start Date, End Date, Item Class Name, Current Price, ?, string ?, maybe bid count?)
		/// </summary>
		/// <example>
		/// BLACK_MARKET_UI_OPEN("2024-05-20 06:00:00", "2024-05-21 00:00:00", "rada_bless_icor_weapon_box3_black", "3000000", 0, "0", 131)
		/// </example>
		public const string BLACK_MARKET_UI_OPEN = "BLACK_MARKET_UI_OPEN(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, \"{5}\", {6})";
	}

	public static class CustomDialog
	{
		/// <summary>
		/// Open Personal Storage (Warehouse)
		/// </summary>
		public const string WAREHOUSE = "warehouse";
		/// <summary>
		/// Open Team Storage (Account Warehouse) Dialog
		/// </summary>
		public const string ACCOUNT_WAREHOUSE = "accountwarehouse";
		/// <summary>
		/// Open Item Identification (Appraisal) Dialog
		/// </summary>
		public const string APPRAISAL = "appraisal";
		/// <summary>
		/// Open Item Refining (Transcend?) Dialog
		/// </summary>
		public const string ITEM_TRANSCEND = "itemtranscend";
		/// <summary>
		/// Open Market Dialog
		/// </summary>
		public const string MARKET = "market";
	}
}
