using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melia.Shared.Versioning;
using Yggdrasil.Logging;

namespace Melia.Shared.Network
{
	public enum NormalOpType
	{
		Barracks,
		Zone,
		Social,
		Integrate,
		Guild
	}
	public static class NormalOp
	{
		/// <summary>
		/// Sub-opcodes used with BC_NORMAL.
		/// </summary>
		public static class Barrack
		{
			public const int SetBarrackCharacter = 0x00;
			public const int SetPosition = 0x02;
			public const int SetCompanionPosition = 0x03;
			public const int SetBarrack = 0x05;
			public const int CompanionInfo = 0x09;
			public const int SetCompanion = 0x0A;
			public const int DeleteCompanion = 0x0B;
			public const int TeamUI = 0x0C;
			public const int ZoneTraffic = 0x0D;
			public const int PlayAnimation = 0x0E;
			public const int StartGameFailed = 0x0F;
			public const int Run = 0x10;
			public const int Mailbox = 0x11;
			public const int MailboxState = 0x13;
			public const int MailUpdate = 0x14;
			public const int SetSessionKey = 0x15;
			public const int ClientIntegrityFailure = 0x18;
			public const int BarrackSlotCount = 0x19;
			public const int NGSCallback = 0x1A;
			public const int ThemaSuccess = 0x1B;
			public const int CharacterInfo = 0x1C;
		}

		/// <summary>
		/// Sub-opcodes used with ZC_NORMAL.
		/// Canonical values are based on the LATEST client version.
		/// Descriptive names are prioritized. Opcodes from older clients not present
		/// in the latest are included for completeness if supporting those older clients.
		/// The SubOpcodeMapper will handle adjustments for older client versions.
		/// </summary>
		public static class Zone
		{
			// Currently in Laima(4) + 1 Shift at some point (390044)
			// In Official is + 2 Shift (from Laima) (400000+)

			public const int TimeActionStart = 0x00;             // LATEST. Old: TIME_ACTION = 0x00
			public const int TimeActionEnd = 0x01;               // LATEST. Old: TIME_ACTION_RESULT = 0x01
			public const int TimeActionOnlyTarget = 0x02;        // OLD: TIME_ACTION_ONLY_TARGET = 0x02 (New op in old client?)
			public const int NpcStateAnimation = 0x03;           // OLD: NPC_STATE_ANIM = 0x03 (New op in old client?)
			public const int NpcStateType = 0x04;                // LATEST. Old: NPC_STATE_TYPE = 0x04
			public const int WindArea = 0x05;                    // OLD: WIND_AREA = 0x05
			public const int Skill_MissileThrow = 0x06;          // LATEST. Old: THROW_MSL = 0x06 (Conceptual Match, LATEST name retained)
			public const int SkillFallingProjectile = 0x07;      // LATEST. Old: FALL_MSL = 0x07 (Conceptual Match)
			public const int SkillMonsterToss = 0x08;            // LATEST. Old: THROW_ACTOR = 0x08 (Conceptual Match)
			public const int SkillItemToss = 0x09;               // LATEST. Old: THROW_ITEM = 0x09 (Conceptual Match)
			public const int ThrowUIToActor = 0x0A;              // OLD: THROW_UI_TO_ACTOR = 0x0A
			public const int SetFixRotateBillboard = 0x0B;       // OLD: FIX_BILLBOARD_ROTATE = 0x0B
			public const int AttachItemToMonster = 0x0C;         // OLD: ATTACH_ITEM_MONSTER = 0x0C
			public const int UpdateCharacterLook = 0x0D;         // LATEST. Old: EQUIP_DUMMYITEM = 0x0D (Conceptual Match)
			public const int PlayEquipItem = 0x0E;               // LATEST. Old: PLAY_EQUIP_DUMMYITEM = 0x0E (Conceptual Match)
			public const int StartEffect = 0x0F;                 // LATEST. Old: START_EFFECT = 0x0F (Conceptual Match)
			public const int EndEffect = 0x10;                   // OLD: END_EFFECT = 0x10
			public const int AttachToNode = 0x11;                // OLD: ZC_ATTACH_TO_NODE = 0x11
			public const int AttachEffect = 0x12;                // LATEST. Old: ATTACH_EFFECT = 0x12
			public const int ClearEffects = 0x13;                // LATEST. Old: CLEAR_EFFECT = 0x13
			public const int PlayEffectNode = 0x14;              // LATEST. Old: PLAY_EFFECT_NODE = 0x14
			public const int PlayEffect_15 = 0x15;               // LATEST PlayEffect = 0x16. Old: PLAY_EFFECT = 0x15. These are different.
			public const int PlayEffect = 0x16;                  // LATEST value.
			public const int PlayForceEffect_16 = 0x16;          // LATEST PlayForceEffect = 0x17. Old: PLAY_FORCE_EFFECT = 0x16. These are different.
			public const int PlayForceEffect = 0x17;             // OLD: PLAY_FORCE_POS = 0x17 (If this is conceptually PlayForceEffect for old, mapper handles it)
			public const int PlayForceReflect = 0x18;            // OLD: PLAY_FORCE_REFLECT = 0x18
			public const int AddEffect = 0x1A;                   // OLD: ADD_EFFECT = 0x19
																 // OLD: REMOVE_EFFECT = 0x1A (Latest: DetachEffect ?)
			public const int DetachEffect = 0x1B;                // LATEST. Old: SKILLMAP_INFO = 0x1B (Latest: UpdateSkillEffect)
																 // OLD: FEVER_COMBO = 0x1C
																 // OLD: SKILL_RANDOM_SEED = 0x1D
			public const int ShowComboEffect = 0x1E;             // LATEST. Old: OBJECT_COLOR_BLEND = 0x1E
			public const int UpdateSkillEffect = 0x1F;           // LATEST. Old: OBJECT_TEXTUREPATH = 0x1F
			public const int SetActorColor = 0x20;               // LATEST. Old: OBJECT_OBBSIZE = 0x20
																 // OLD: OBJECT_SHADOW_BLEND = 0x21
																 // OLD: ZC_SELECT_XAC = 0x22
			public const int SetActorShadow = 0x23;              // LATEST. Old: ZC_SELECT_CMD_XAC = 0x23
																 // OLD: SET_CLIENT_DEAD_SCP = 0x24
																 // OLD: ADVENT_DIRECTION = 0x25
			public const int Skill_CallLuaFunc = 0x26;           // LATEST. Old: PLAY_CLIENT_SCP = 0x26
																 // OLD: OPERATOR_SCP = 0x27
																 // OLD: CLIENT_SCRIPT = 0x28
																 // OLD: INST_OBJ = 0x29
																 // OLD: BEAT_KEYBOARD = 0x2A
			public const int SetSkillProperties = 0x2B;          // LATEST. Old: HIT_ANI = 0x2B
																 // OLD: CHANGE_SCL = 0x2C (Latest: SetScale)
																 // OLD: MON_ALERT = 0x2D (Latest: PlaySound)
			public const int SetScale = 0x2E;                    // LATEST. Old: SIMPLE_MSL = 0x2E
			public const int PlaySound = 0x2F;                   // LATEST. Old: SET_FACESTATE = 0x2F
																 // OLD: RELOAD = 0x30
																 // OLD: RUN_GAME_SCP = 0x31
																 // OLD: ARROW_EFFECT = 0x32 (Note: LATEST has PlayArrowEffect = 0x34)
																 // OLD: SYNC_EXEC_SET = 0x33
			public const int PlayArrowEffect = 0x34;             // LATEST. Old: DUMMYPC_INFO = 0x34
																 // OLD: DUMMYPC_LIST = 0x35
																 // OLD: DUMMYPC_HP = 0x36
																 // OLD: DUMMYPC_CONTROL = 0x37
			public const int FadeOut = 0x38;                     // LATEST. Old: POSTPONE_GENTYPE_LEAVE = 0x38
																 // OLD: RIDE_CONTROL = 0x39
																 // LATEST RemoveGroundEffect = 0x3C. Old: REMOVE_GROUND_EFFECT = 0x3A. Different.
			public const int TextEffect = 0x3D;                  // LATEST. Old: ADD_RENDER_OPTION = 0x3D
			public const int BarrackSlotCount = 0x3C;            // LATEST. Old: SUCCESS_BUY_CHAR_SLOT = 0x3C
			public const int RemoveGroundEffect = 0x3C;          // LATEST value (shares with BarrackSlotCount).
																 // OLD: CHANGE_SKILL_ANIM = 0x3E
			public const int SetActorRenderOption = 0x3F;        // LATEST. Sets render options on actors (e.g., "Freeze", "bigheadmode")
																 // OLD: CHANGE_SKILL_ANIM_CLEAR = 0x3F
			public const int SkillChangeAnimation = 0x40;        // LATEST. Old: SKILL_TARGET_ANI = 0x40
			public const int SkillResetAnimation = 0x41;         // LATEST. Old: SKILL_TARGET_ANI_END = 0x41
			public const int SkillTargetAnimation = 0x42;        // LATEST. Old: REQ_MOVEPATH_END = 0x42
			public const int SkillTargetAnimationEnd = 0x43;     // LATEST. Old: SKILL_CHAIN_UPDATE = 0x43
			public const int AttackCancelBow = 0x44;             // LATEST. Old: TOOL_SKILL_CANCEL = 0x44
			public const int Skill_45_Cloak = 0x45;              // LATEST (Skill_45). Old: MON_MINIMAP_START = 0x45
			public const int SkillCancel = 0x46;                 // LATEST. Old: MON_MINIMAP = 0x46
			public const int MonsterMapMarker = 0x47;            // LATEST. Old: PC_MINIMAP = 0x47
			public const int CharacterMapMarker = 0x48;          // LATEST. Old: MON_MINIMAP_END = 0x48
			public const int RemoveMapMarker = 0x49;             // LATEST. Old: MON_INDICATE = 0x49
																 // OLD: MON_INDICATE_END = 0x4A
																 // OLD: MON_OOBE_EFFECT = 0x4B
																 // OLD: ACCOUNT_PROPERTY = 0x4C (Note: LATEST AccountProperties = 0x4D)
			public const int AccountProperties = 0x4D;           // LATEST. Old: CLEAR_MOVE_QUEUE = 0x4D
																 // LATEST Skill_DynamicCastStart = 0x4F. Old: DYNAMIC_CASTING_START = 0x4E.
			public const int Skill_DynamicCastStart = 0x4F;      // LATEST value.
																 // LATEST Skill_DynamicCastEnd = 0x50. Old: DYNAMIC_CASTING_END = 0x4F.
			public const int Skill_DynamicCastEnd = 0x50;        // LATEST value.
																 // OLD: SKILLTOOL_DBG_TARGET = 0x50 (If conceptual match to LATEST Skill_DynamicCastEnd, mapper handles)
																 // OLD: PRELOAD_DIRECTION = 0x51
																 // OLD: DIRECTION_SCP = 0x52 (NPC_PlayTrack)
			public const int NPC_PlayTrack = 0x53;               // LATEST. Old: DIRECTION_INIT_POS = 0x53 (SetNPCTrackPosition)
			public const int SetNPCTrackPosition = 0x54;         // LATEST. Old: MGAME_ZONEENTER = 0x54
			public const int MiniGame = 0x55;                    // LATEST. Old: MGAME_STAGE_INFO = 0x55
			public const int IndunAddonMsg = 0x56;               // LATEST. Old: MGAME_MON_COUNT = 0x56
																 // OLD: MGAME_QUEST = 0x57
			public const int IndunAddonMsgParam = 0x58;          // LATEST. Old: PADSKILL_ENTER = 0x58
			public const int PadUpdate = 0x59;                   // LATEST. Old: PAD_LINKEFFECT = 0x59 (also SkillRunScript in LATEST original)
			public const int SkillRunScript = 0x59;              // LATEST (shares value with PadUpdate).
			public const int PadLinkEffect = 0x5A;               // OLD: PADSKILL_ATTACH_OBJ = 0x5A
																 // OLD: PADSKILL_CREATE_OBSTACLE = 0x5B (Note: LATEST PadCreateObstacle = 0x5D)
			public const int PadSetMonsterAltitude = 0x5C;       // LATEST. Old: PADSKILL_PUSH = 0x5C
			public const int PadCreateObstacle = 0x5D;           // LATEST.
																 // OLD: PADSKILL_DESTORY_AFTER_TIME = 0x5E
			public const int Skill_5F = 0x5F;                    // LATEST. Old: PADSKILL_CHANGE_OWNER = 0x5F
																 // OLD: PADSKILL_BOMBARDMENT = 0x60
			public const int ParticleEffect = 0x61;              // LATEST. Old: PADSKILL_MOVE_DESTTARGET = 0x61
																 // OLD: PADSKILL_MOVE_DESTPOS = 0x62
																 // OLD: PADSKILL_MOVE_DESTPOS_TIME = 0x63
			public const int PadMoveTo = 0x64;                   // LATEST. Old: PADSKILL_ACTIVE_COUNT = 0x64 (also Skill_EffectMovement in LATEST original)
			public const int Skill_EffectMovement = 0x64;        // LATEST (shares value).
																 // OLD: PADSKILL_CHANGE_TYPE = 0x65
			public const int Unknown_64 = 0x66;                  // LATEST. Old: PADSKILL_ATTACK_CMD = 0x66
																 // OLD: PADSKILL_REPOSITION = 0x67
																 // OLD: PADSKILL_CAPTURE_TO_STACK = 0x68
																 // OLD: FORMATION_TYPE = 0x69
																 // OLD: FORMATION_SWAP_POSITION = 0x6A
																 // OLD: PADSCP_ENTER = 0x6B
																 // OLD: DIRECTION_START = 0x6C
			public const int Skill_6D = 0x6D;                    // LATEST. Old: DIRECTION_FORCE_END = 0x6D
			public const int SetupCutscene = 0x6E;               // LATEST. Old: USER_SESSION_INFO = 0x6E
			public const int Unknown_6D = 0x6F;                  // LATEST. Old: TIME_FACTOR = 0x6F
			public const int Unknown_6E = 0x70;                  // LATEST. Old: REVEAL_ALL_MAP = 0x70
																 // OLD: DRAW_AREA = 0x71
																 // OLD: TEAM_ID = 0x72
																 // OLD: UI_FORCE = 0x73
																 // OLD: UI_FORCE_TO_ACTOR = 0x74
																 // OLD: GAME_INIT_MSG = 0x75
																 // OLD: ZONE_OBJ_PROP = 0x76
																 // OLD: SKL_EXP_EFFECT = 0x77 (LATEST has no 0x77)
			public const int LoadCutscene = 0x78;                // LATEST. Old: UPDATE_SKL_SPDRATE = 0x78
																 // OLD: UPDATE_HIT_DELAY = 0x79
			public const int SetHitDelay = 0x7A;                 // LATEST. Old: SKILL_END_SCRIPTS = 0x7A
			public const int SetSkillSpeed = 0x7B;               // LATEST. Old: GAUGE_UPDATE = 0x7B
			public const int SetSkillUseOverHeat = 0x7C;         // LATEST. Old: SKILL_REFRESH_TIME = 0x7C
			public const int SkillCancelCancel = 0x7D;           // LATEST. Old: CHANGE_GROUND_EFT = 0x7D
																 // OLD: ADD_PAD_EFT = 0x7E
																 // OLD: REMOVE_PAD_EFT = 0x7F
																 // OLD: OOBE_CONTROL = 0x80
			public const int Skill_7F = 0x81;                    // LATEST. Old: SET_SUMMON_DUMMYPC = 0x81
			public const int ChangeGroundEffect = 0x81;          // Skill_7F seems to mismatched? (ChangeGroundEffect is correct 26-12-2025).
			public const int PadAddEffect = 0x82;                // OLD: RUN_TOOL_SKL_INDEX = 0x82
			public const int PadRemoveEffect = 0x83;             // OLD: SET_HIRE_DUMMY_PC = 0x83
																 // OLD: CHANGE_NORMAL_ATTACK = 0x84
																 // OLD: USING_TOGGLE_SKILLID = 0x85
																 // OLD: SPIN_THROW = 0x86 (Note: LATEST SpinThrow = 0x89)
			public const int SetMainAttackSkill = 0x87;          // LATEST. Old: SPIN_OBJECT = 0x87 (Note: LATEST SpinObject = 0x8A)
			public const int SkillToggle = 0x88;                 // LATEST. Old: COLL_TO_GROUND = 0x88 (Note: LATEST SkillCollisionToGround = 0x8B)
			public const int SpinThrow = 0x89;                   // LATEST. Old: COLLISION_AND_BACK = 0x89
			public const int SpinObject = 0x8A;                  // LATEST. Old: PENETRATE_POSITION = 0x8A
			public const int SkillCollisionToGround = 0x8B;      // LATEST. Old: THROW_ATTACHED_MONSTER = 0x8B (Note: LATEST ThrowAttachedMonster = 0x8E)
			public const int CollisionAndBack = 0x8C;                    // LATEST. Old: STOP_ANIM = 0x8C (Note: LATEST StopAnimation = 0x8F)
			public const int PenetratePosition = 0x8D;           // OLD: RESERVE_ANIM = 0x8D
			public const int ThrowAttachedMonster = 0x8E;        // LATEST. Old: CONNECTION_EFFECT = 0x8E (Note: LATEST PlayConnectEffect = 0x91)
			public const int StopAnimation = 0x8F;               // LATEST. Old: LINK_OBJECT = 0x8F
			public const int PetPlayAnimation = 0x90;            // LATEST. Old: SYNC_BY_ATTACHED = 0x90
			public const int PlayConnectEffect = 0x91;           // OLD: RESET_ASTD_ANIM = 0x91
																 // OLD: RESET_ARUN_ANIM = 0x92
																 // OLD: PLAY_DIRECTION = 0x93
			public const int ResetStdAnim = 0x94;                // OLD: RESUME_DIRECTION = 0x94
			public const int ResetRunAnim = 0x95;                // OLD: CHANGEAPC_ITEM_BTBODY = 0x95
			public const int CutsceneTrack = 0x96;               // LATEST. Old: DELAY_ENTERWORLD = 0x96 (Note: LATEST DelayEnterWorld = 0x99)
			public const int SetTrackFrame = 0x97;               // LATEST. Old: PICK_REWARD = 0x97
			public const int ChangeApcItemBodyPT = 0x98;         // OLD: NPC_AUCTION_INFO = 0x98
			public const int DelayEnterWorld = 0x99;             // LATEST. Old: ITEM_BALLOON = 0x99 (Note: LATEST ShowItemBalloon = 0x9C)
			public const int PickReward = 0x9A;                  // OLD: ITEM_GET_ANIM = 0x9A (Note: LATEST PlayItemGetAnim = 0x9D)
			public const int NpcAuctionInfo = 0x9B;              // OLD: BOOK_ITEM = 0x9B (Note: LATEST ShowBook = 0x9E)
			public const int ShowItemBalloon = 0x9C;             // LATEST. Old: ATTACH_TO_BG_MODEL = 0x9C
			public const int PlayItemGetAnim = 0x9D;             // LATEST. Old: PCBUFF_TIMEHOLD = 0x9D
			public const int ShowBook = 0x9E;                    // LATEST. Old: JUMP_TO_POS = 0x9E (Note: LATEST JumpToPosition = 0xA2)
			public const int ShowScroll = 0x9F;                  // LATEST. Old: PET_LIST = 0x9F
																 // OLD: PET_PC_PROPERTY = 0xA0
			public const int Unknown_A1 = 0xA1;                  // LATEST. Old: PET_EQUIP = 0xA1
			public const int JumpToPosition = 0xA3;              // LATEST. Old: PET_APPERANCE = 0xA2
																 // OLD: PET_ID = 0xA3
			public const int PetInfo = 0xA4;                     // LATEST. Old: PET_EXP_UP = 0xA4 (Note: LATEST PetExpUp = 0xAA)
																 // OLD: PET_ATTACH_SET = 0xA5
																 // OLD: LINKER_LIST = 0xA6 -> 0xAC? (Note: LATEST PetIsInactive = 0x1A6 for similar concept?)
			public const int Pet_AssociateWorldId = 0xA7;        // LATEST. Old: LINKER_REMOVE = 0xA7
																 // OLD: LINKER_DESTRUCT = 0xA8 -> 0xAD?
			public const int Pet_AssociateHandleWorldId = 0xA9;  // LATEST. Old: KNOCKBACK_EFFECT = 0xA9
			public const int PetExpUp = 0xAA;                    // LATEST. Old: KNOCKBACK_ANIMATION = 0xAA
			public const int AddLinkEffect = 0xAC;                  // OLD: KNOCKBACK_HIT = 0xAB
			public const int DestroyLinkEffect = 0xAD;           // OLD: KB_CALC_BOUNCE_USE = 0xAC
			public const int LinkEffectDestruct = 0xAE;          // OLD: SET_MOVE_TYPE = 0xAD
																 // OLD: CONTROL_MONSTER = 0xAE (Note: LATEST ControlObject = 0xB4)
																 // OLD: CONTROL_VEHICLE = 0xAF (Note: LATEST RidePet = 0xB5)
																 // OLD: COMPANION_OWNER = 0xB0 (Note: LATEST PetOwner = 0xB6)
																 // OLD: RAISE_FROM_GROUND = 0xB1
																 // OLD: ATTACH_TO_CLIENT_MON = 0xB2
																 // OLD: ADD_ATTACH_ANIM = 0xB3
			public const int ControlObject = 0xB4;                     // LATEST. Old: FORCE_ATTACH_ANIM = 0xB4
			public const int RidePet = 0xB5;                     // LATEST. Old: ATTACH_NODE_MONSTER = 0xB5
			public const int PetOwner = 0xB6;                    // LATEST. Old: ITEM_ATTACH_TO_MONSTER_NODE = 0xB6
			public const int OffsetY = 0xB7;                     // LATEST. Old: AUTO_DETACH_ON_OWNER_MOVE = 0xB7
																 // OLD: MOVE_NODE_MONSTER = 0xB8
			public const int PetFlying = 0xB9;                   // LATEST. Old: NODE_MONSTER_ANIM = 0xB9
																 // OLD: ADD_SKILL_SYNCKEY = 0xBA
																 // OLD: EXEC_SKILL_SYNCKEY = 0xBB
																 // OLD: JUMP_BY_ARC = 0xBC
			public const int AutoDetachWhenTargetMove = 0xBD;      // LATEST. Old: HOVER_AROUND = 0xBD
																   // OLD: HOVER_AROUND_ACCURATELY = 0xBE
																   // OLD: HOVER_POSITION = 0xBF
																   // OLD: HOVER_POSITON_PAUSE = 0xC0
																   // OLD: ENTER_DELAYED_ACTOR = 0xC1 (Note: LATEST EnterDelayedActor = 0xC8)
			public const int Skill_MoveJump = 0xC2;              // LATEST. Old: ATTACH_FORCE = 0xC2
																 // OLD: GETBACK_FORCE = 0xC3
																 // OLD: REMOVE_FORCE = 0xC4
																 // OLD: DPARTS_GATHER = 0xC5
																 // OLD: DPARTS_GATHER_ZOMBIE = 0xC6
																 // OLD: DPARTS_GATHER_ATTACH = 0xC7
			public const int EnterDelayedActor = 0xC8;           // LATEST. Old: SKILL_QUEUE = 0xC8
			public const int SkillTargetAttachForce = 0xC9;      // LATEST. Old: DPARTS_CONSUME = 0xC9 (Clarified C7 from latest)
																 // OLD: DPARTS_ATTACK = 0xCA
																 // OLD: DPARTS_ATTACH = 0xCB
			public const int PlayGatherCorpseParts = 0xCC;       // LATEST. Old: DPARTS_ATTACH_APC = 0xCC
																 // OLD: ZOMBIE_ADD = 0xCD
																 // OLD: ZOMBIE_DELETE = 0xCE
																 // OLD: ZOMBIE_HOVER = 0xCF
																 // OLD: ZOMBIE_HOVER_REMOVE = 0xD0
																 // OLD: DPARTS_HOVER = 0xD1
																 // OLD: DPARTS_HOVER_CHANGE_OWNER = 0xD2
																 // OLD: DPARTS_HOVER_REMOVE = 0xD3
			public const int Skill_Unknown_D4 = 0xD4;            // LATEST. Old: DPARTS_HOVER_COLLISION = 0xD4
																 // OLD: DPARTS_SHOOT_GROUND = 0xD5
																 // OLD: COLLECTION_LIST = 0xD6 (Note: LATEST ItemCollectionList = 0xDD)
																 // OLD: ZC_TREASUREMARK_LIST_MAP = 0xD7
			public const int PlayCorpsePartsRing = 0xD8;         // LATEST. Old: ADD_COLLECTION = 0xD8 (Note: LATEST UnlockCollection = 0xDF)
																 // OLD: COLLECTION_ITEM = 0xD9 (Note: LATEST UpdateCollection = 0xE0)
			public const int RemoveCorpseParts = 0xDA;                // LATEST. Old: RANK_INFO_PAGE = 0xDA
			public const int DropCorpseParts = 0xDB;             // LATEST. Old: DIALOG_ROTATE = 0xDB
			public const int PlayThrowCorpseParts = 0xDC;        // LATEST. Old: SKILL_TEXT_EFFECT = 0xDC
			public const int ItemCollectionList = 0xDD;          // LATEST. Old: AUCTION_NOTIFY = 0xDD
																 // OLD: MGAME_USER_VALUE = 0xDE
			public const int UnlockCollection = 0xDF;            // LATEST. Old: MGAME_STATE = 0xDF
			public const int UpdateCollection = 0xE0;            // LATEST. Old: MGAME_STATE_LIST = 0xE0
																 // OLD: MGAME_TOURNAMENT_STATE = 0xE1
			public const int Unknown_E0 = 0xE2;                  // LATEST. Old: MGAME_TOURNAMENT_LIST = 0xE2
			public const int PlayTextEffect = 0xE3;              // LATEST. Old: MGAME_TOURNAMENT_BATTLE_RESULT = 0xE3
			public const int Unknown_E4 = 0xE4;                  // LATEST. Old: MGAME_TOURNAMENT_TOTAL_RESULT = 0xE4
																 // OLD: LAYER_CHAT = 0xE5
																 // OLD: TARGET_UI_FUNC = 0xE6
			public const int Unknown_E5 = 0xE7;                  // LATEST. Old: NOT_PICKABLE = 0xE7
																 // OLD: DELIVER_FORCE = 0xE8
																 // OLD: PERIODIC_TITLE = 0xE9
																 // OLD: BALLOON_TEXT = 0xEA (Note: LATEST Notice = 0xF0)
																 // OLD: SLOW_MOTION = 0xEB (Note: LATEST SlowMotion = 0xF1)
																 // OLD: CHANGEJOB_RANK = 0xEC
																 // OLD: JOB_HISTORY_LIST = 0xED (Note: LATEST JobHistoryList = 0xF2)
																 // OLD: PAKCET_CANCEL_DEAD = 0xEE (typo from txt)
																 // OLD: PARTY_MEMBER_UPDATE = 0xEF (Note: LATEST PartyMemberData = 0xF4)
			public const int Notice = 0xF0;                      // LATEST. Old: PARTY_QUEST_UPDATE = 0xF0
			public const int SlowMotion = 0xF1;                  // OLD: PARTY_LEADER_CHANGE = 0xF1 (Note: LATEST PartyLeaderChange = 0xF6)
			public const int JobHistoryList = 0xF2;              // LATEST. Old: PARTY_NAME_CHANGE = 0xF2 (Note: LATEST PartyNameChange = 0xF7)
																 // OLD: PARTY_DIRECT_INVITE = 0xF3 (Note: LATEST PartyInvite = 0xF8)
			public const int PartyMemberData = 0xF4;             // LATEST. Old: PARTY_PROP_UPDATE = 0xF4 (Note: LATEST PartyPropertyChange = 0xF9)
																 // OLD: PARTY_MEMBER_PROP_UPDATE = 0xF5 (Note: LATEST PartyMemberPropertyChange = 0xFA)
			public const int PartyLeaderChange = 0xF6;           // LATEST. Old: PARTY_SHARED_QUEST = 0xF6
			public const int PartyNameChange = 0xF7;             // LATEST. Old: CABINET_LIST = 0xF7
			public const int PartyInvite = 0xF8;                 // LATEST. Old: CABINET_ITEM_GET = 0xF8
			public const int PartyPropertyChange = 0xF9;         // LATEST. Old: MARKET_BUY_SUCCESS = 0xF9
			public const int PartyMemberPropertyChange = 0xFA;   // LATEST. Old: MARKET_CANCEL_SUCCESS = 0xFA
			public const int Unknown_FB = 0xFB;                  // LATEST. Old: MARKET_ITEM_LIST = 0xFB (Name it MarketItemList_FB if conceptually different from LATEST Unknown_FB)
			public const int MarketRetrievalItems = 0xFC;        // LATEST. Old: CHANGE_TO_MONSTER = 0xFC
			public const int MarketRegisterItem = 0xFD;          // LATEST. Old: ADD_MONSTER_WALL = 0xFD
			public const int MarketBuyItem = 0xFE;               // LATEST. Old: END_MONSTER_WALL = 0xFE
			public const int MarketCancelItem = 0xFF;            // LATEST. Old: OBJ_RECORD_POS = 0xFF

			// --- Ops from 0x100 onwards (Mostly from LATEST, integrate OLD if they were higher) ---
			// OLD: OBJ_RECORD_POS_EXEC = 0x100
			// OLD: SORCERER_EFFECT = 0x101
			// OLD: MAGIC_AMULET_LIST = 0x102
			// OLD: EXPROP = 0x103
			// OLD: HOOK_EFFECT = 0x104 (Note: LATEST ShowHookEffect/MakeHookEffect = 0x109)
			// OLD: SHOW_HOOK_EFFECT = 0x105 (ibid)
			// OLD: LIMITATION_SKILL_ADD = 0x106
			// OLD: LIMITATION_SKILL_CLEAR = 0x107
			public const int SummonPlayAnimation = 0x108;        // LATEST. Old: IGNORE_SKILL_COOLTIME_ADD = 0x108
			public const int MakeHookEffect = 0x109;             // LATEST
			public const int ShowHookEffect = 0x10A;             // LATEST
			public const int ApplyBuff = 0x10B;                  // LATEST. Old: SQUIRE_FOODTABE_HISTORY = 0x10B
			public const int EnableUseSkillWhileOutOfBody = ApplyBuff;  // Sadhu OOBE alias
			public const int RemoveBuff = 0x10C;                 // LATEST. Old: AUTOSELLER_LIST = 0x10C
			public const int EndOutOfBodyBuff = RemoveBuff;      // Sadhu OOBE alias
			public const int Skill_10D = 0x10D;                  // LATEST. Old: JUMP_LOOP_BY_SKILL = 0x10D
			public const int Skill_10E = 0x10E;                  // LATEST. Old: CANCEL_DYNAMIC_SKILL = 0x10E
																 // OLD: UPDATE_REMAIN_CASH = 0x10F
																 // OLD: UPDATE_REDUCTION_CASH = 0x110
																 // OLD: UPDATE_PICKUP_NISMS = 0x111
			public const int Unknown_10F = 0x112;                // LATEST. Old: UPDATE_PURCHASE_RESULT = 0x112
			public const int CancelDynamicCast = 0x113;          // LATEST. Old: SKL_SAGE_FIND_FRIEND = 0x113
																 // OLD: UPDATE_REFUND_RESULT = 0x114
																 // OLD: UPDATE_CASH_INVEN = 0x115
																 // OLD: UPDATE_TPITEM_SPECIAL_INFO = 0x116
																 // OLD: UPDATE_TPSHOP_BANNER = 0x117
																 // OLD: INGAMESHOP_ITEM_LIST = 0x118
																 // OLD: INGAMESHOP_ITEM_LIST_NEXON = 0x119
																 // OLD: AUTOSELLER_HISTORY = 0x11A
																 // OLD: AUTOSELLER_TITLE = 0x11B
			public const int Unknown_11A = 0x11C;                // LATEST. Old: ENABLE_PREVIEW_SKILL_RANGE = 0x11C
																 // OLD: ENABLE_PREVIEW_HITRADIUS = 0x11D
			public const int Shop_Unknown11C = 0x11E;            // LATEST. Old: TRANSFORM_TO_MONSTER = 0x11E
			public const int EnableHitRadiusPreview = 0x11F;           // OLD: CONNECT_LINK_TEXTURE = 0x11F
			public const int Transmutation = 0x120;              // LATEST. Old: ARG_OBJECT = 0x120
			public const int Unknown_121 = 0x121;                // LATEST. Old: JUMP_ROPE = 0x121
			public const int Skill_122 = 0x122;                  // LATEST. Old: JUMP_ROPE_EXIT = 0x122
			public const int RunJumpRope = 0x123;                // LATEST. Old: CHAIN_EFFECT = 0x123
																 // OLD: PROMINENCE = 0x124 (Note: LATEST Skill_124 = 0x126)
			public const int ShootChainEffect = 0x125;           // LATEST. Old: PUZZLECRAFT_MAKING = 0x125
			public const int Skill_124 = 0x126;                  // LATEST. Old: GRASS_PAUSE = 0x126
																 // OLD: SAME_ANI_MONSTER = 0x127 (Note: LATEST Skill_127 = 0x129)
																 // OLD: BITE_TARGET = 0x128
			public const int Skill_127 = 0x129;                  // LATEST. Old: BITE_BRING = 0x129
																 // OLD: APPROACH_ACTION = 0x12A
																 // OLD: ITEM_PRIORITY_UPDATE = 0x12B
																 // OLD: CHANNEL_TRAFFICS = 0x12C (Note: LATEST ChannelTraffic = 0x12D)
			public const int ChannelTraffic = 0x12D;             // LATEST. Old: MONINOF_BY_SKILL = 0x12D
																 // OLD: LINKEFFECT_TO_TARGET = 0x12E
			public const int MonsterUsePCSkill = 0x130;          // OLD: MONSTER_USE_PCSKILL = 0x12F
			public const int MonsterDynamicCast = 0x131;         // LATEST. Old: MONSTER_DYNAMIC_CAST = 0x130
																 // OLD: PC_TITLE = 0x131 (SetActorLabel)
			public const int SetActorLabel = 0x132;              // LATEST. Old: MOVING_ATTACK_START = 0x132
																 // OLD: MOVING_ATTACK_MOVE = 0x133
			public const int Unknown_134 = 0x137;                // LATEST (value 0x137). Old: ZC_SQUIRE_UI = 0x134
																 // OLD: ZC_SQUIRE_UI_CLOSE = 0x135
																 // OLD: SOCIAL_START_INFO = 0x136
																 // OLD: PC_COMMENT_CHANGE = 0x137 (Matches value of LATEST Unknown_134)
			public const int SetGreetingMessage = 0x138;         // LATEST. Old: VISIT_BARRACK = 0x138
																 // OLD: UPDATE_APP_TIME = 0x139
																 // OLD: ATTACH_GAUGE = 0x13A
																 // OLD: PARTY_NAME = 0x13B (Value used by ShowParty in LATEST)
			public const int ShowParty = 0x13C;                  // LATEST. Old: MOVEPATH_DEST_CHANGE = 0x13C
																 // OLD: IGNORE_OBB = 0x13D
			public const int Revive = 0x13E;                     // LATEST. Old: PC_SHOP_ANIM = 0x13E
			public const int ShopAnimation = 0x13F;              // LATEST. Old: CONSUME_TARGET = 0x13F
																 // OLD: LIMIT_MIN_TARGET_RANGE = 0x140
																 // OLD: OPEN_CAMP_UI = 0x141
																 // OLD: OPEN_FOODTABLE_UI = 0x142
																 // OLD: PLAY_EAT_FOOD = 0x143
																 // OLD: FLY_WITH_OBJECT = 0x144 (Now 0x147 in LATEST)
																 // OLD: OBJECT_BLINK = 0x145
			public const int SetSessionKey = 0x146;              // LATEST. Old: PATTERN_MISSILE = 0x146
			public const int FlyWithObject = 0x147;              // OLD: I_NEED_PARTY = 0x147
																 // OLD: UPDATE_AUTHCODE = 0x148
																 // OLD: MISSION_CLIENT_SCRIPT = 0x149
			public const int Unknown_14A = 0x14A;                // LATEST. Old: DEAD_OBSERVER_MODE = 0x14A
																 // OLD: PVP_CHAT = 0x14B
																 // OLD: PVP_PLAY_TYPES = 0x14C
			public const int StatusEffect = 0x14D;               // LATEST. Old: LEAVE_EFFECT = 0x14D
																 // OLD: CARDBATTLE = 0x14E
																 // OLD: TITLE_IMAGE = 0x14F
																 // OLD: REVERSE_BORN_ANI = 0x150
			public const int DisconnectError = 0x151;            // LATEST. Old: DESTROY_REASON = 0x151
			public const int ItemDrop = 0x152;                   // LATEST. Old: ITEM_DROP = 0x152
			public const int EnableAction = 0x153;                // LATEST. Old: ENABLE_ACTION = 0x153
			public const int RequestDuel = 0x154;                // LATEST. Old: ASK_FRIENDLY_FIGHT = 0x154
																 // OLD: SHOW_DPS = 0x155
																 // OLD: FRIENDLY_FIGHT_APC = 0x156
			public const int FightState = 0x157;                 // LATEST. Old: SHARED_ANIM = 0x157
																 // OLD: SEAL_AREA = 0x158
																 // OLD: CART_ATTACH = 0x159
																 // OLD: DISABLE_ACTION_FOR_TIME = 0x15A
																 // OLD: CART_OBJECT_RIDE = 0x15B
																 // OLD: ENABLE_TARGET_SELECT = 0x15C
																 // OLD: HOLD_CAMERA_HEIGHT = 0x15D
																 // OLD: SPECIAL_FALL_SPEED_RATE = 0x15E
																 // OLD: RESERVE_LAND_ANIM = 0x15F
																 // OLD: BLOCK_TIME_INFO = 0x160
																 // OLD: UNICODE_DIGIT_EFFECT = 0x161
																 // OLD: UNICODE_DIGIT_CALCAULATE = 0x162 (typo from txt)
																 // OLD: DIGIT_EFFECT = 0x163
																 // OLD: STRING_EFFECT = 0x164
																 // OLD: UPDATE_MONSTER_LEVEL = 0x165
																 // OLD: POLICY_TIME = 0x166
																 // OLD: NGS_CONTROL = 0x167 (LATEST NGSCallback = 0x170)
																 // OLD: GUILD_ITEM_OBJECT = 0x168
																 // OLD: GUILD_ITEM_OBJECT_PROP_CHANGE = 0x169
																 // OLD: CHANGE_NAME_BY_SCRIPT = 0x16A
																 // OLD: SET_GUILD_WAR_TIME = 0x16B
																 // OLD: CANCEL_GUILD_WAR_TIME = 0x16C
																 // OLD: REMOVE_PARTY_WAR_TIME = 0x16D
																 // OLD: CHANGE_GUILD_NEUTRALITY = 0x16E
																 // OLD: ZC_UPDATE_GUILD_BOARD = 0x16F
			public const int NGSCallback = 0x170;                // LATEST. Old: ZC_ACCOUNT_WAREHOUSE_VIS_LOG = 0x170
			public const int StorageSilverTransaction = 0x171;   // LATEST. Old: MARKET_MINMAX_PRICE = 0x171
			public const int MarketMinMaxInfo = 0x172;           // LATEST. Old: SWITCHGENDER_SUCCEED = 0x172
																 // OLD: PLAY_HISTORY_LIST = 0x173
																 // OLD: PLAY_HISTORY_RESPECT = 0x174
																 // OLD: UPDATE_WORLDMAP = 0x175
																 // OLD: INDUN_AUTOMATCH_INFO = 0x176 (LATEST InstanceDungeonMatchMaking = 0x18E)
			public const int DungeonAutoMatching = 0x177;        // OLD: AUTO_MATCH_WITH_PARTY = 0x177
			public const int DungeonAutoMatchWithParty = 0x178; // OLD: AUTOMATCH_PARTY_COUNT = 0x178
			public const int DungeonAutoMatchPartyCount = 0x179;        // OLD: PARTY_MEMBER_MOVE_ZONE = 0x179
			public const int MemberMapStatusUpdate = 0x17A;      // LATEST. Old: STATUE_APC = 0x17A
																 // OLD: HAT_VISIBLE_APC = 0x17B
			public const int HeadgearVisibilityUpdate = 0x17C;   // LATEST. Old: NOTICE_MSG = 0x17C
			public const int WorldClientMessage = 0x17D;         // LATEST. Old: NOTICE_MSG_STRING = 0x17D
			public const int WorldMessage = 0x17E;               // LATEST. Old: PARTY_INVENTORY_ITEM_ADD = 0x17E
																 // OLD: PARTY_INVENTORY_ITEM_REMOVE = 0x17F
																 // OLD: UPDATE_SKILL_PROPERTIES = 0x180 (LATEST SetSkillsProperties = 0x181)
			public const int SetSkillsProperties = 0x181;        // LATEST. Old: MONSTER_SAY = 0x181
																 // OLD: PARTY_CHAT_IN_INTEGRATE = 0x182
																 // OLD: AUTO_MACRO_INFO = 0x183
																 // OLD: SET_MAP_MODE = 0x184
			public const int InstanceStart = 0x185;              // LATEST. Old: SHOP_BUY_LIMIT_INFO = 0x185
																 // OLD: UPDATE_SHOP_BUY_LIMIT_INFO = 0x186
																 // OLD: MCC_PC_INFO = 0x187
																 // OLD: JOB_EXP_LIST = 0x188
			public const int UpdateSkillUI = 0x189;              // LATEST. Old: MON_SKILL_OK = 0x189
																 // OLD: INGAME_ALERT = 0x18A
																 // OLD: UPDATE_TPSHOP_TEST = 0x18B
																 // OLD: SHOW_INDUNENTER = 0x18C
																 // OLD: SETAURACOLOR = 0x18D
			public const int InstanceDungeonMatchMaking = 0x18E; // LATEST. Old: RANK_INFO_PAGE_BY_AID = 0x18E
																 // OLD: AUTO_SKILL_TEST = 0x18F
			public const int FishingRankData = 0x190;            // LATEST. Old: SHOW_FORGERY = 0x190
																 // OLD: SEND_FORGERY = 0x191
																 // OLD: RESULT_MONSTER_DROP_ITEM_TEST = 0x192
																 // OLD: FISHING_STATE = 0x193
																 // OLD: ADVENTURE_BOOK_MY_RANKING = 0x194
																 // OLD: ADVENTURE_BOOK_RANK_INFO_MAIN_PAGE = 0x195
																 // OLD: ADVENTURE_BOOK_RANK_INFO_ITEM_PAGE = 0x196
			public const int AdventureBook = 0x197;              // LATEST. Old: ADVENTURE_BOOK_REWARD_INFO = 0x197
			public const int AdventureBookRank = 0x198;          // LATEST. Old: SERVER_JADDURY_RATE = 0x198
																 // OLD: QUEST_COMPLETE_LIST = 0x199
			public const int Unknown_198 = 0x19A;                // LATEST. Old: UPDATE_GUILD_ASSET_LOG = 0x19A
																 // OLD: ADVENTURE_BOOK_CONTENTS_POINT = 0x19B
			public const int Unknown_19B = 0x19E;                // LATEST (value 0x19E). Old: COLONY_PROGRESS_STATE = 0x19C
																 // OLD: UPDATE_GUILD_ASSET = 0x19D
																 // OLD: COLONY_WAR_POINT_INFO = 0x19E (Matches value of LATEST Unknown_19B)
			public const int Unknown_19D_SetTime = 0x19F;        // LATEST

			public const int PetIsInactive = 0x1A6;              // LATEST
			public const int SetSubAttackSkill = 0x1A7;          // LATEST
			public const int Unknown_1A6 = 0x1A8;                // LATEST
			public const int PadSetModel = 0x1AB;                // LATEST
			public const int WigVisibilityUpdate = 0x1AC;        // LATEST
			public const int UsedMedalTotal = 0x1B7;             // LATEST
			public const int Unknown_1B6 = 0x1B8;                // LATEST
			public const int SteamAchievement = 0x1BE;           // LATEST
			public const int ActorRotate = 0x1BF;                // LATEST
			public const int SubWeaponVisibilityUpdate = 0x1C5;  // LATEST


			// --- Opcodes ONLY from the OLDER ZC_NORMAL_PACKETS.txt (Likely removed or heavily refactored in LATEST) ---
			// --- Their values here are their OLD values, serving as their canonical ID for that old version ---
			public const int OLD_TIME_ACTION_ONLY_TARGET = 0x02;
			public const int OLD_NPC_STATE_ANIM = 0x03;
			public const int OLD_WIND_AREA = 0x05;
			public const int OLD_THROW_UI_TO_ACTOR = 0x0A;
			public const int OLD_FIX_BILLBOARD_ROTATE = 0x0B;
			public const int OLD_ATTACH_ITEM_MONSTER = 0x0C;
			public const int OLD_END_EFFECT = 0x10;
			public const int OLD_ZC_ATTACH_TO_NODE = 0x11;
			public const int OLD_PLAY_EFFECT_VALUE_15 = 0x15; // Old PLAY_EFFECT was 0x15
			public const int OLD_PLAY_FORCE_EFFECT_VALUE_16 = 0x16; // Old PLAY_FORCE_EFFECT was 0x16
			public const int OLD_PALY_FORCE_POS = 0x17; // Note: LATEST PlayForceEffect is also 0x17
			public const int OLD_PLAY_FORCE_REFLECT = 0x18;
			public const int OLD_ADD_EFFECT = 0x19;
			public const int OLD_REMOVE_EFFECT_VALUE_1A = 0x1A; // Distinguish from LATEST RemoveEffect 0xDA
			public const int OLD_SKILLMAP_INFO = 0x1B; // Note: LATEST DetachEffect is also 0x1B
			public const int OLD_FEVER_COMBO = 0x1C;
			public const int OLD_SKILL_RANDOM_SEED = 0x1D;
			// LATEST ShowComboEffect = 0x1E. Old OBJECT_COLOR_BLEND = 0x1E
			// LATEST UpdateSkillEffect = 0x1F. Old OBJECT_TEXTUREPATH = 0x1F
			// LATEST SetActorColor = 0x20. Old OBJECT_OBBSIZE = 0x20
			public const int OLD_OBJECT_SHADOW_BLEND = 0x21;
			public const int OLD_ZC_SELECT_XAC = 0x22;
			// LATEST SetActorShadow = 0x23. Old ZC_SELECT_CMD_XAC = 0x23
			public const int OLD_SET_CLIENT_DEAD_SCP = 0x24;
			public const int OLD_ADVENT_DIRECTION = 0x25;
			// LATEST Skill_CallLuaFunc = 0x26. Old PLAY_CLIENT_SCP = 0x26
			public const int OLD_OPERATOR_SCP = 0x27;
			public const int OLD_CLIENT_SCRIPT = 0x28;
			public const int OLD_INST_OBJ = 0x29;
			public const int OLD_BEAT_KEYBOARD = 0x2A;
			// LATEST SetSkillProperties = 0x2B. Old HIT_ANI = 0x2B
			public const int OLD_CHANGE_SCL = 0x2C;
			public const int OLD_MON_ALERT = 0x2D;
			// LATEST SetScale = 0x2E. Old SIMPLE_MSL = 0x2E
			// LATEST PlaySound = 0x2F. Old SET_FACESTATE = 0x2F
			public const int OLD_RELOAD = 0x30;
			public const int OLD_RUN_GAME_SCP = 0x31;
			public const int OLD_ARROW_EFFECT = 0x32;
			public const int OLD_SYNC_EXEC_SET = 0x33;
			// LATEST PlayArrowEffect = 0x34. Old DUMMYPC_INFO = 0x34
			public const int OLD_DUMMYPC_LIST = 0x35;
			public const int OLD_DUMMYPC_HP = 0x36;
			public const int OLD_DUMMYPC_CONTROL = 0x37;
			// LATEST FadeOut = 0x38. Old POSTPONE_GENTYPE_LEAVE = 0x38
			public const int OLD_RIDE_CONTROL = 0x39;
			public const int OLD_REMOVE_GROUND_EFFECT_VALUE_3A = 0x3A;
			// LATEST TextEffect = 0x3D. Old ADD_RENDER_OPTION = 0x3D
			// LATEST BarrackSlotCount = 0x3C. Old SUCCESS_BUY_CHAR_SLOT = 0x3C
			public const int OLD_CHANGE_SKILL_ANIM = 0x3E;
			public const int OLD_CHANGE_SKILL_ANIM_CLEAR = 0x3F;
			// LATEST SkillChangeAnimation = 0x40. Old SKILL_TARGET_ANI = 0x40
			// LATEST SkillResetAnimation = 0x41. Old SKILL_TARGET_ANI_END = 0x41
			// LATEST SkillTargetAnimation = 0x42. Old REQ_MOVEPATH_END = 0x42
			// LATEST SkillTargetAnimationEnd = 0x43. Old SKILL_CHAIN_UPDATE = 0x43
			// LATEST AttackCancelBow = 0x44. Old TOOL_SKILL_CANCEL = 0x44
			// LATEST Skill_45_Cloak = 0x45. Old MON_MINIMAP_START = 0x45
			// LATEST SkillCancel = 0x46. Old MON_MINIMAP = 0x46
			// LATEST MonsterMapMarker = 0x47. Old PC_MINIMAP = 0x47
			// LATEST CharacterMapMarker = 0x48. Old MON_MINIMAP_END = 0x48
			// LATEST RemoveMapMarker = 0x49. Old MON_INDICATE = 0x49
			public const int OLD_MON_INDICATE_END = 0x4A;
			public const int OLD_MON_OOBE_EFFECT = 0x4B;
			public const int OLD_ACCOUNT_PROPERTY_VALUE_4C = 0x4C;
			// LATEST AccountProperties = 0x4D. Old CLEAR_MOVE_QUEUE = 0x4D
			public const int OLD_DYNAMIC_CASTING_START_VALUE_4E = 0x4E;
			public const int OLD_DYNAMIC_CASTING_END_VALUE_4F = 0x4F;
			// LATEST Skill_DynamicCastEnd = 0x50. Old SKILLTOOL_DBG_TARGET = 0x50
			public const int OLD_PRELOAD_DIRECTION = 0x51;
			public const int OLD_DIRECTION_SCP = 0x52;
			// LATEST NPC_PlayTrack = 0x53. Old DIRECTION_INIT_POS = 0x53
			// LATEST SetNPCTrackPosition = 0x54. Old MGAME_ZONEENTER = 0x54
			// LATEST MiniGame = 0x55. Old MGAME_STAGE_INFO = 0x55
			// LATEST IndunAddonMsg = 0x56. Old MGAME_MON_COUNT = 0x56
			public const int OLD_MGAME_QUEST = 0x57;
			// LATEST IndunAddonMsgParam = 0x58. Old PADSKILL_ENTER = 0x58
			// LATEST PadUpdate/SkillRunScript = 0x59. Old PAD_LINKEFFECT = 0x59
			public const int OLD_PADSKILL_ATTACH_OBJ = 0x5A;
			public const int OLD_PADSKILL_CREATE_OBSTACLE_VALUE_5B = 0x5B;
			// LATEST PadSetMonsterAltitude = 0x5C. Old PADSKILL_PUSH = 0x5C
			// LATEST PadCreateObstacle = 0x5D
			public const int OLD_PADSKILL_DESTORY_AFTER_TIME = 0x5E;
			// LATEST Skill_5F = 0x5F. Old PADSKILL_CHANGE_OWNER = 0x5F
			public const int OLD_PADSKILL_BOMBARDMENT = 0x60;
			// LATEST ParticleEffect = 0x61. Old PADSKILL_MOVE_DESTTARGET = 0x61
			public const int OLD_PADSKILL_MOVE_DESTPOS = 0x62;
			public const int OLD_PADSKILL_MOVE_DESTPOS_TIME = 0x63;
			// LATEST PadMoveTo/Skill_EffectMovement = 0x64. Old PADSKILL_ACTIVE_COUNT = 0x64
			public const int OLD_PADSKILL_CHANGE_TYPE = 0x65;
			// LATEST Unknown_64 = 0x66. Old PADSKILL_ATTACK_CMD = 0x66
			public const int OLD_PADSKILL_REPOSITION = 0x67;
			public const int OLD_PADSKILL_CAPTURE_TO_STACK = 0x68;
			public const int OLD_FORMATION_TYPE = 0x69;
			public const int OLD_FORMATION_SWAP_POSITION = 0x6A;
			public const int OLD_PADSCP_ENTER = 0x6B;
			public const int OLD_DIRECTION_START = 0x6C;
			// LATEST Skill_6D = 0x6D. Old DIRECTION_FORCE_END = 0x6D
			// LATEST SetupCutscene = 0x6E. Old USER_SESSION_INFO = 0x6E
			public const int OLD_TIME_FACTOR = 0x6F; // Note: LATEST Unknown_6D is 0x6F
			public const int OLD_REVEAL_ALL_MAP = 0x70; // Note: LATEST Unknown_6E is 0x70
			public const int OLD_DRAW_AREA = 0x71;
			public const int OLD_TEAM_ID = 0x72;
			public const int OLD_UI_FORCE = 0x73;
			public const int OLD_UI_FORCE_TO_ACTOR = 0x74;
			public const int OLD_GAME_INIT_MSG = 0x75;
			public const int OLD_ZONE_OBJ_PROP = 0x76;
			public const int OLD_SKL_EXP_EFFECT = 0x77;
			// LATEST LoadCutscene = 0x78. Old UPDATE_SKL_SPDRATE = 0x78
			public const int OLD_UPDATE_HIT_DELAY = 0x79;
			// LATEST SetHitDelay = 0x7A. Old SKILL_END_SCRIPTS = 0x7A
			// LATEST SetSkillSpeed = 0x7B. Old GAUGE_UPDATE = 0x7B
			// LATEST SetSkillUseOverHeat = 0x7C. Old SKILL_REFRESH_TIME = 0x7C
			// LATEST SkillCancelCancel = 0x7D. Old CHANGE_GROUND_EFT = 0x7D
			public const int OLD_ADD_PAD_EFT = 0x7E;
			public const int OLD_REMOVE_PAD_EFT = 0x7F;
			public const int OLD_OOBE_CONTROL = 0x80;
			// LATEST Skill_7F = 0x81. Old SET_SUMMON_DUMMYPC = 0x81
			public const int OLD_RUN_TOOL_SKL_INDEX = 0x82;
			public const int OLD_SET_HIRE_DUMMY_PC = 0x83;
			public const int OLD_CHANGE_NORMAL_ATTACK = 0x84;
			public const int OLD_USING_TOGGLE_SKILLID = 0x85;
			public const int OLD_SPIN_THROW_VALUE_86 = 0x86;
			// LATEST SetMainAttackSkill = 0x87. Old SPIN_OBJECT = 0x87
			// LATEST SkillToggle = 0x88. Old COLL_TO_GROUND = 0x88
			// LATEST SpinThrow = 0x89. Old COLLISION_AND_BACK = 0x89
			// LATEST SpinObject = 0x8A. Old PENETRATE_POSITION = 0x8A
			// LATEST SkillCollisionToGround = 0x8B. Old THROW_ATTACHED_MONSTER = 0x8B
			// LATEST Skill_8C = 0x8C. Old STOP_ANIM = 0x8C
			public const int OLD_RESERVE_ANIM = 0x8D;
			// LATEST ThrowAttachedMonster = 0x8E. Old CONNECTION_EFFECT = 0x8E
			// LATEST StopAnimation = 0x8F. Old LINK_OBJECT = 0x8F
			// LATEST PetPlayAnimation = 0x90. Old SYNC_BY_ATTACHED = 0x90
			public const int OLD_RESET_ASTD_ANIM = 0x91;
			public const int OLD_RESET_ARUN_ANIM = 0x92;
			public const int OLD_PLAY_DIRECTION = 0x93;
			public const int OLD_RESUME_DIRECTION = 0x94;
			public const int OLD_CHANGEAPC_ITEM_BTBODY = 0x95;
			// LATEST CutsceneTrack = 0x96. Old DELAY_ENTERWORLD = 0x96
			// LATEST SetTrackFrame = 0x97. Old PICK_REWARD = 0x97
			public const int OLD_NPC_AUCTION_INFO = 0x98;
			// LATEST DelayEnterWorld = 0x99. Old ITEM_BALLOON = 0x99
			public const int OLD_ITEM_GET_ANIM = 0x9A;
			public const int OLD_BOOK_ITEM = 0x9B;
			// LATEST ShowItemBalloon = 0x9C. Old ATTACH_TO_BG_MODEL = 0x9C
			// LATEST PlayItemGetAnim = 0x9D. Old PCBUFF_TIMEHOLD = 0x9D
			// LATEST ShowBook = 0x9E. Old JUMP_TO_POS = 0x9E
			// LATEST ShowScroll = 0x9F. Old PET_LIST = 0x9F
			public const int OLD_PET_PC_PROPERTY = 0xA0;
			// LATEST Unknown_A1 = 0xA1. Old PET_EQUIP = 0xA1
			// LATEST JumpToPosition = 0xA2. Old PET_APPERANCE = 0xA2
			public const int OLD_PET_ID = 0xA3;
			// LATEST PetInfo = 0xA4. Old PET_EXP_UP = 0xA4
			public const int OLD_PET_ATTACH_SET = 0xA5;
			public const int OLD_LINKER_LIST = 0xA6;
			// LATEST Pet_AssociateWorldId = 0xA7. Old LINKER_REMOVE = 0xA7
			public const int OLD_LINKER_DESTRUCT = 0xA8;
			// LATEST Pet_AssociateHandleWorldId = 0xA9. Old KNOCKBACK_EFFECT = 0xA9
			// LATEST PetExpUp = 0xAA. Old KNOCKBACK_ANIMATION = 0xAA
			public const int OLD_KNOCKBACK_HIT = 0xAB;
			public const int OLD_KB_CALC_BOUNCE_USE = 0xAC;
			public const int OLD_SET_MOVE_TYPE = 0xAD;
			public const int OLD_CONTROL_MONSTER = 0xAE;
			public const int OLD_CONTROL_VEHICLE = 0xAF;
			public const int OLD_COMPANION_OWNER = 0xB0;
			public const int OLD_RAISE_FROM_GROUND = 0xB1;
			public const int OLD_ATTACH_TO_CLIENT_MON = 0xB2;
			public const int OLD_ADD_ATTACH_ANIM = 0xB3;
			// LATEST PetBuff = 0xB4. Old FORCE_ATTACH_ANIM = 0xB4
			// LATEST RidePet = 0xB5. Old ATTACH_NODE_MONSTER = 0xB5
			// LATEST PetOwner = 0xB6. Old ITEM_ATTACH_TO_MONSTER_NODE = 0xB6
			// LATEST OffsetY = 0xB7. Old AUTO_DETACH_ON_OWNER_MOVE = 0xB7
			public const int OLD_MOVE_NODE_MONSTER = 0xB8;
			// LATEST PetFlying = 0xB9. Old NODE_MONSTER_ANIM = 0xB9
			public const int OLD_ADD_SKILL_SYNCKEY = 0xBA;
			public const int OLD_EXEC_SKILL_SYNCKEY = 0xBB;
			public const int OLD_JUMP_BY_ARC = 0xBC;
			// LATEST SetPetDefaultAnimation = 0xBD. Old HOVER_AROUND = 0xBD
			public const int OLD_HOVER_AROUND_ACCURATELY = 0xBE;
			public const int OLD_HOVER_POSITION = 0xBF;
			public const int OLD_HOVER_POSITON_PAUSE = 0xC0;
			public const int OLD_ENTER_DELAYED_ACTOR_VALUE_C1 = 0xC1;
			// LATEST Skill_MoveJump = 0xC2. Old ATTACH_FORCE = 0xC2
			public const int OLD_GETBACK_FORCE = 0xC3;
			public const int OLD_REMOVE_FORCE = 0xC4;
			public const int OLD_DPARTS_GATHER = 0xC5;
			public const int OLD_DPARTS_GATHER_ZOMBIE = 0xC6;
			public const int OLD_DPARTS_GATHER_ATTACH = 0xC7;
			// LATEST EnterDelayedActor = 0xC8. Old SKILL_QUEUE = 0xC8
			// LATEST Skill_Unknown_C7 = 0xC9. Old DPARTS_CONSUME = 0xC9
			public const int OLD_DPARTS_ATTACK = 0xCA;
			public const int OLD_DPARTS_ATTACH = 0xCB;
			// LATEST PlayGatherCorpseParts = 0xCC. Old DPARTS_ATTACH_APC = 0xCC
			public const int OLD_ZOMBIE_ADD = 0xCD;
			public const int OLD_ZOMBIE_DELETE = 0xCE;
			public const int OLD_ZOMBIE_HOVER = 0xCF;
			public const int OLD_ZOMBIE_HOVER_REMOVE = 0xD0;
			public const int OLD_DPARTS_HOVER = 0xD1;
			public const int OLD_DPARTS_HOVER_CHANGE_OWNER = 0xD2;
			public const int OLD_DPARTS_HOVER_REMOVE = 0xD3;
			// LATEST Skill_Unknown_D4 = 0xD4. Old DPARTS_HOVER_COLLISION = 0xD4
			public const int OLD_DPARTS_SHOOT_GROUND = 0xD5;
			public const int OLD_COLLECTION_LIST = 0xD6;
			public const int OLD_ZC_TREASUREMARK_LIST_MAP = 0xD7;
			// LATEST PlayCorpsePartsRing = 0xD8. Old ADD_COLLECTION = 0xD8
			public const int OLD_COLLECTION_ITEM = 0xD9;
			// LATEST RemoveEffect = 0xDA. Old RANK_INFO_PAGE = 0xDA
			// LATEST DropCorpseParts = 0xDB. Old DIALOG_ROTATE = 0xDB
			// LATEST PlayThrowCorpseParts = 0xDC. Old SKILL_TEXT_EFFECT = 0xDC
			// LATEST ItemCollectionList = 0xDD. Old AUCTION_NOTIFY = 0xDD
			public const int OLD_MGAME_USER_VALUE = 0xDE;
			// LATEST UnlockCollection = 0xDF. Old MGAME_STATE = 0xDF
			// LATEST UpdateCollection = 0xE0. Old MGAME_STATE_LIST = 0xE0
			public const int OLD_MGAME_TOURNAMENT_STATE = 0xE1;
			// LATEST Unknown_E0 = 0xE2. Old MGAME_TOURNAMENT_LIST = 0xE2
			// LATEST PlayTextEffect = 0xE3. Old MGAME_TOURNAMENT_BATTLE_RESULT = 0xE3
			// LATEST Unknown_E4 = 0xE4. Old MGAME_TOURNAMENT_TOTAL_RESULT = 0xE4
			public const int OLD_LAYER_CHAT = 0xE5;
			public const int OLD_TARGET_UI_FUNC = 0xE6;
			// LATEST Unknown_E5 = 0xE7. Old NOT_PICKABLE = 0xE7
			public const int OLD_DELIVER_FORCE = 0xE8;
			public const int OLD_PERIODIC_TITLE = 0xE9;
			public const int OLD_BALLOON_TEXT = 0xEA;
			public const int OLD_SLOW_MOTION = 0xEB;
			public const int OLD_CHANGEJOB_RANK = 0xEC;
			public const int OLD_JOB_HISTORY_LIST = 0xED;
			public const int OLD_PAKCET_CANCEL_DEAD = 0xEE;
			public const int OLD_PARTY_MEMBER_UPDATE = 0xEF;
			// LATEST Notice = 0xF0. Old PARTY_QUEST_UPDATE = 0xF0
			public const int OLD_PARTY_LEADER_CHANGE_VALUE_F1 = 0xF1;
			// LATEST GlobalJobCount = 0xF2. Old PARTY_NAME_CHANGE = 0xF2
			public const int OLD_PARTY_DIRECT_INVITE_VALUE_F3 = 0xF3;
			// LATEST PartyMemberData = 0xF4. Old PARTY_PROP_UPDATE = 0xF4
			public const int OLD_PARTY_MEMBER_PROP_UPDATE_VALUE_F5 = 0xF5;
			// LATEST PartyLeaderChange = 0xF6. Old PARTY_SHARED_QUEST = 0xF6
			// LATEST PartyNameChange = 0xF7. Old CABINET_LIST = 0xF7
			// LATEST PartyInvite = 0xF8. Old CABINET_ITEM_GET = 0xF8
			// LATEST PartyPropertyChange = 0xF9. Old MARKET_BUY_SUCCESS = 0xF9
			// LATEST PartyMemberPropertyChange = 0xFA. Old MARKET_CANCEL_SUCCESS = 0xFA
			public const int OLD_MARKET_ITEM_LIST_VALUE_FB = 0xFB; // Note: LATEST Unknown_FB = 0xFB
																   // LATEST MarketRetrievalItems = 0xFC. Old CHANGE_TO_MONSTER = 0xFC
																   // LATEST MarketRegisterItem = 0xFD. Old ADD_MONSTER_WALL = 0xFD
																   // LATEST MarketBuyItem = 0xFE. Old END_MONSTER_WALL = 0xFE
																   // LATEST MarketCancelItem = 0xFF. Old OBJ_RECORD_POS = 0xFF
			public const int OLD_OBJ_RECORD_POS_EXEC = 0x100;
			public const int OLD_SORCERER_EFFECT = 0x101;
			public const int OLD_MAGIC_AMULET_LIST = 0x102;
			public const int OLD_EXPROP = 0x103;
			public const int OLD_HOOK_EFFECT = 0x104;
			public const int OLD_SHOW_HOOK_EFFECT_VALUE_105 = 0x105;
			public const int OLD_LIMITATION_SKILL_ADD = 0x106;
			public const int OLD_LIMITATION_SKILL_CLEAR = 0x107;
			// LATEST SummonPlayAnimation = 0x108. Old IGNORE_SKILL_COOLTIME_ADD = 0x108
			// LATEST ShowHookEffect = 0x109. Old IGNORE_SKILL_COOLTIME_CLEAR = 0x109
			public const int OLD_SQUIRE_CAMP_HISTORY = 0x10A;
			// LATEST ApplyBuff = 0x10B. Old SQUIRE_FOODTABE_HISTORY = 0x10B
			// LATEST RemoveBuff = 0x10C. Old AUTOSELLER_LIST = 0x10C
			// LATEST Skill_10D = 0x10D. Old JUMP_LOOP_BY_SKILL = 0x10D
			// LATEST Skill_10E = 0x10E. Old CANCEL_DYNAMIC_SKILL = 0x10E
			public const int OLD_UPDATE_REMAIN_CASH = 0x10F;
			public const int OLD_UPDATE_REDUCTION_CASH = 0x110;
			public const int OLD_UPDATE_PICKUP_NISMS = 0x111;
			// LATEST Unknown_10F = 0x112. Old UPDATE_PURCHASE_RESULT = 0x112
			// LATEST CancelDynamicCast = 0x113. Old SKL_SAGE_FIND_FRIEND = 0x113
			public const int OLD_UPDATE_REFUND_RESULT = 0x114;
			public const int OLD_UPDATE_CASH_INVEN = 0x115;
			public const int OLD_UPDATE_TPITEM_SPECIAL_INFO = 0x116;
			public const int OLD_UPDATE_TPSHOP_BANNER = 0x117;
			public const int OLD_INGAMESHOP_ITEM_LIST = 0x118;
			public const int OLD_INGAMESHOP_ITEM_LIST_NEXON = 0x119;
			public const int OLD_AUTOSELLER_HISTORY = 0x11A;
			public const int OLD_AUTOSELLER_TITLE = 0x11B;
			// LATEST Unknown_11A = 0x11C. Old ENABLE_PREVIEW_SKILL_RANGE = 0x11C
			public const int OLD_ENABLE_PREVIEW_HITRADIUS = 0x11D;
			// LATEST Shop_Unknown11C = 0x11E. Old TRANSFORM_TO_MONSTER = 0x11E
			public const int OLD_CONNECT_LINK_TEXTURE = 0x11F;
			// LATEST Transmutation = 0x120. Old ARG_OBJECT = 0x120
			// LATEST Unknown_121 = 0x121. Old JUMP_ROPE = 0x121
			// LATEST Skill_122 = 0x122. Old JUMP_ROPE_EXIT = 0x122
			// LATEST RunJumpRope = 0x123. Old CHAIN_EFFECT = 0x123
			public const int OLD_PROMINENCE = 0x124;
			// LATEST ShootChainEffect = 0x125. Old PUZZLECRAFT_MAKING = 0x125
			// LATEST Skill_124 = 0x126. Old GRASS_PAUSE = 0x126
			public const int OLD_SAME_ANI_MONSTER = 0x127;
			public const int OLD_BITE_TARGET = 0x128;
			// LATEST Skill_127 = 0x129. Old BITE_BRING = 0x129
			public const int OLD_APPROACH_ACTION = 0x12A;
			public const int OLD_ITEM_PRIORITY_UPDATE = 0x12B;
			public const int OLD_CHANNEL_TRAFFICS_VALUE_12C = 0x12C;
			// LATEST ChannelTraffic = 0x12D. Old MONINOF_BY_SKILL = 0x12D
			public const int OLD_LINKEFFECT_TO_TARGET = 0x12E;
			public const int OLD_MONSTER_USE_PCSKILL = 0x12F;
			// LATEST Unknown_130 = 0x130. Old MONSTER_DYNAMIC_CAST = 0x130
			public const int OLD_PC_TITLE = 0x131;
			// LATEST SetActorLabel = 0x132. Old MOVING_ATTACK_START = 0x132
			public const int OLD_MOVING_ATTACK_MOVE = 0x133;
			public const int OLD_ZC_SQUIRE_UI = 0x134;
			public const int OLD_ZC_SQUIRE_UI_CLOSE = 0x135;
			public const int OLD_SOCIAL_START_INFO = 0x136;
			// LATEST Unknown_134 is 0x137. Old PC_COMMENT_CHANGE = 0x137.
			// LATEST SetGreetingMessage = 0x138. Old VISIT_BARRACK = 0x138
			public const int OLD_UPDATE_APP_TIME = 0x139;
			public const int OLD_ATTACH_GAUGE = 0x13A;
			public const int OLD_PARTY_NAME_VALUE_13B = 0x13B; // Note: LATEST ShowParty can be 0x13C
															   // LATEST ShowParty = 0x13C. Old MOVEPATH_DEST_CHANGE = 0x13C
			public const int OLD_IGNORE_OBB = 0x13D;
			// LATEST Revive = 0x13E. Old PC_SHOP_ANIM = 0x13E
			// LATEST ShopAnimation = 0x13F. Old CONSUME_TARGET = 0x13F
			public const int OLD_LIMIT_MIN_TARGET_RANGE = 0x140;
			public const int OLD_OPEN_CAMP_UI = 0x141;
			public const int OLD_OPEN_FOODTABLE_UI = 0x142;
			public const int OLD_PLAY_EAT_FOOD = 0x143;
			public const int OLD_FLY_WITH_OBJECT = 0x144;
			public const int OLD_OBJECT_BLINK = 0x145;
			// LATEST SetSessionKey = 0x146. Old PATTERN_MISSILE = 0x146
			public const int OLD_I_NEED_PARTY = 0x147;
			public const int OLD_UPDATE_AUTHCODE = 0x148;
			public const int OLD_MISSION_CLIENT_SCRIPT = 0x149;
			// LATEST Unknown_14A = 0x14A. Old DEAD_OBSERVER_MODE = 0x14A
			public const int OLD_PVP_CHAT = 0x14B;
			public const int OLD_PVP_PLAY_TYPES = 0x14C;
			// LATEST StatusEffect = 0x14D. Old LEAVE_EFFECT = 0x14D
			public const int OLD_CARDBATTLE = 0x14E;
			public const int OLD_TITLE_IMAGE = 0x14F;
			public const int OLD_REVERSE_BORN_ANI = 0x150;
			// LATEST DisconnectError = 0x151. Old DESTROY_REASON = 0x151
			// LATEST ItemDrop = 0x152. Old ITEM_DROP = 0x152
			// LATEST Unknown_153 = 0x153. Old ENABLE_ACTION = 0x153
			// LATEST RequestDuel = 0x154. Old ASK_FRIENDLY_FIGHT = 0x154
			public const int OLD_SHOW_DPS = 0x155;
			public const int OLD_FRIENDLY_FIGHT_APC = 0x156;
			// LATEST FightState = 0x157. Old SHARED_ANIM = 0x157
			public const int OLD_SEAL_AREA = 0x158;
			public const int OLD_CART_ATTACH = 0x159;
			public const int OLD_DISABLE_ACTION_FOR_TIME = 0x15A;
			public const int OLD_CART_OBJECT_RIDE = 0x15B;
			public const int OLD_ENABLE_TARGET_SELECT = 0x15C;
			public const int OLD_HOLD_CAMERA_HEIGHT = 0x15D;
			public const int OLD_SPECIAL_FALL_SPEED_RATE = 0x15E;
			public const int OLD_RESERVE_LAND_ANIM = 0x15F;
			public const int OLD_BLOCK_TIME_INFO = 0x160;
			public const int OLD_UNICODE_DIGIT_EFFECT = 0x161;
			public const int OLD_UNICODE_DIGIT_CALCAULATE = 0x162;
			public const int OLD_DIGIT_EFFECT = 0x163;
			public const int OLD_STRING_EFFECT = 0x164;
			public const int OLD_UPDATE_MONSTER_LEVEL = 0x165;
			public const int OLD_POLICY_TIME = 0x166;
			public const int OLD_NGS_CONTROL = 0x167;
			public const int OLD_GUILD_ITEM_OBJECT = 0x168;
			public const int OLD_GUILD_ITEM_OBJECT_PROP_CHANGE = 0x169;
			public const int OLD_CHANGE_NAME_BY_SCRIPT = 0x16A;
			public const int OLD_SET_GUILD_WAR_TIME = 0x16B;
			public const int OLD_CANCEL_GUILD_WAR_TIME = 0x16C;
			public const int OLD_REMOVE_PARTY_WAR_TIME = 0x16D;
			public const int OLD_CHANGE_GUILD_NEUTRALITY_VALUE_16E = 0x16E;
			public const int OLD_ZC_UPDATE_GUILD_BOARD = 0x16F;
			// LATEST NGSCallback = 0x170. Old ZC_ACCOUNT_WAREHOUSE_VIS_LOG = 0x170
			// LATEST StorageSilverTransaction = 0x171. Old MARKET_MINMAX_PRICE = 0x171
			// LATEST MarketMinMaxInfo = 0x172. Old SWITCHGENDER_SUCCEED = 0x172
			public const int OLD_PLAY_HISTORY_LIST = 0x173;
			public const int OLD_PLAY_HISTORY_RESPECT = 0x174;
			public const int OLD_UPDATE_WORLDMAP = 0x175;
			public const int OLD_INDUN_AUTOMATCH_INFO = 0x176;
			public const int OLD_AUTO_MATCH_WITH_PARTY = 0x177;
			public const int OLD_AUTOMATCH_PARTY_COUNT = 0x178;
			public const int OLD_PARTY_MEMBER_MOVE_ZONE_VALUE_179 = 0x179;
			// LATEST MemberMapStatusUpdate = 0x17A. Old STATUE_APC = 0x17A
			public const int OLD_HAT_VISIBLE_APC = 0x17B;
			// LATEST HeadgearVisibilityUpdate = 0x17C. Old NOTICE_MSG = 0x17C
			// LATEST WorldClientMessage = 0x17D. Old NOTICE_MSG_STRING = 0x17D
			// LATEST WorldMessage = 0x17E. Old PARTY_INVENTORY_ITEM_ADD = 0x17E
			public const int OLD_PARTY_INVENTORY_ITEM_REMOVE = 0x17F;
			public const int OLD_UPDATE_SKILL_PROPERTIES_VALUE_180 = 0x180;
			// LATEST SetSkillsProperties = 0x181. Old MONSTER_SAY = 0x181
			public const int OLD_PARTY_CHAT_IN_INTEGRATE = 0x182;
			public const int OLD_AUTO_MACRO_INFO = 0x183;
			public const int OLD_SET_MAP_MODE = 0x184;
			// LATEST InstanceStart = 0x185. Old SHOP_BUY_LIMIT_INFO = 0x185
			public const int OLD_UPDATE_SHOP_BUY_LIMIT_INFO = 0x186;
			public const int OLD_MCC_PC_INFO = 0x187;
			public const int OLD_JOB_EXP_LIST = 0x188;
			// LATEST UpdateSkillUI = 0x189. Old MON_SKILL_OK = 0x189
			public const int OLD_INGAME_ALERT = 0x18A;
			public const int OLD_UPDATE_TPSHOP_TEST = 0x18B;
			public const int OLD_SHOW_INDUNENTER = 0x18C;
			public const int OLD_SETAURACOLOR = 0x18D;
			// LATEST InstanceDungeonMatchMaking = 0x18E. Old RANK_INFO_PAGE_BY_AID = 0x18E
			public const int OLD_AUTO_SKILL_TEST = 0x18F;
			// LATEST FishingRankData = 0x190. Old SHOW_FORGERY = 0x190
			public const int OLD_SEND_FORGERY = 0x191;
			public const int OLD_RESULT_MONSTER_DROP_ITEM_TEST = 0x192;
			public const int OLD_FISHING_STATE = 0x193;
			public const int OLD_ADVENTURE_BOOK_MY_RANKING = 0x194;
			public const int OLD_ADVENTURE_BOOK_RANK_INFO_MAIN_PAGE = 0x195;
			public const int OLD_ADVENTURE_BOOK_RANK_INFO_ITEM_PAGE = 0x196;
			// LATEST AdventureBook = 0x197. Old ADVENTURE_BOOK_REWARD_INFO = 0x197
			// LATEST AdventureBookRank = 0x198. Old SERVER_JADDURY_RATE = 0x198
			public const int OLD_QUEST_COMPLETE_LIST = 0x199;
			// LATEST Unknown_198 = 0x19A. Old UPDATE_GUILD_ASSET_LOG = 0x19A
			public const int OLD_ADVENTURE_BOOK_CONTENTS_POINT = 0x19B;
			public const int OLD_COLONY_PROGRESS_STATE = 0x19C;
			public const int OLD_UPDATE_GUILD_ASSET = 0x19D;
			// LATEST Unknown_19B is 0x19E. Old COLONY_WAR_POINT_INFO = 0x19E
		}

		/// <summary>
		/// Sub-opcodes used with SC_NORMAL.
		/// </summary>
		public static class Social
		{
			public const int LoginSuccess = 0x00;
			public const int Unknown_01 = 0x01;
			public const int Start = 0x02;
			public const int AddMessage = 0x03;
			public const int MessageList = 0x04;
			public const int CreateRoom = 0x05;
			public const int Shout = 0x06;
			public const int SystemMessage = 0x07;
			public const int FriendInfo = 0x08;
			public const int FriendResponse = 0x09;
			public const int PartyMemberUpdate = 0x0C;
			public const int PartyInfo = 0x0D;
			public const int FriendRequested = 0x10;
			public const int FriendBlocked = 0x11;
			public const int Unknown_19 = 0x19;

			// Relation Server
			public const int RelationCount = 0x7D00;
			public const int RelatedSessions = 0x7D01;
			public const int RelationCountUpdate = 0x7D02;
			public const int RelationHistory = 0x7D03;
			public const int LikeNotify = 0x7D04;
			public const int LikeSuccess = 0x7D05;
			public const int UnlikeSuccess = 0x7D07;
			public const int LikeConfirm = 0x7D08;
			public const int LikedList = 0x7D09;
			public const int LikedMeList = 0x7D0A;
			public const int LikeCount = 0x7D0B;
			public const int LikeFailed = 0x7D0D;
		}

		/// <summary>
		/// Sub-opcodes used with SC_FROM_INTEGRATE.
		/// </summary>
		/// <remarks>
		/// The exact purpose of the integrate packets is currently only partially
		/// known, but they appear to be mostly related to inter-server processes,
		/// such as instanced dungeons, match-making, and PVP.
		/// </remarks>
		public static class Integrate
		{
			// These ops weren't actively maintained and might be outdated.

			public const int Unknown_01 = 0x01;
			public const int Unknown_05 = 0x05;
			public const int SpectateMatchInfo = 0x0E;
			public const int AutoMatchComplete = 0x12;
			public const int Unknown_14 = 0x14;
			public const int AutoMatchQueueUpdate = 0x16;
			public const int AutoMatchQueueTotal = 0x17;
			public const int Unknown_19 = 0x19;
			public const int PvpRanking = 0x1C;
			public const int Unknown_1D = 0x1D;
		}

		public static class GuildOp
		{
			public const int Message = 0x10;
			public const int MessageParameter = 0x11;
			public const int GuildMessage = 0x13;
		}
	}

	internal class SubOpcodeVersionMapping
	{
		public int CanonicalValue { get; }
		public string Name { get; } // For debugging
		private List<VersionRule> _rules = new();

		public SubOpcodeVersionMapping(int canonicalValue, string name)
		{
			this.CanonicalValue = canonicalValue;
			this.Name = name;
		}

		public SubOpcodeVersionMapping AddRule(int minClientVersion, int maxClientVersion, int? absoluteValue = null, int? offsetFromCanonical = null)
		{
			_rules.Add(new VersionRule(minClientVersion, maxClientVersion, absoluteValue, offsetFromCanonical));
			// Sort rules to apply more specific or higher version rules first.
			// Higher MinVersion first, then higher MaxVersion for tie-breaking.
			_rules = _rules.OrderByDescending(r => r.MinClientVersion)
						   .ThenByDescending(r => r.MaxClientVersion)
						   .ToList();
			return this;
		}

		public int GetVersionedValue(int clientVersion)
		{
			foreach (var rule in _rules)
			{
				if (clientVersion >= rule.MinClientVersion && clientVersion <= rule.MaxClientVersion)
				{
					if (rule.AbsoluteValue.HasValue)
						return rule.AbsoluteValue.Value;
					if (rule.OffsetFromCanonical.HasValue)
						return this.CanonicalValue + rule.OffsetFromCanonical.Value;
				}
			}
			return this.CanonicalValue; // Default to canonical if no rule matches for the version
		}

		private sealed class VersionRule
		{
			public int MinClientVersion { get; }
			public int MaxClientVersion { get; }
			public int? AbsoluteValue { get; }
			public int? OffsetFromCanonical { get; }

			public VersionRule(int minClientVersion, int maxClientVersion, int? absoluteValue, int? offsetFromCanonical)
			{
				if (absoluteValue.HasValue && offsetFromCanonical.HasValue)
					throw new ArgumentException("Cannot specify both AbsoluteValue and OffsetFromCanonical.");
				if (minClientVersion > maxClientVersion)
					throw new ArgumentException("MinClientVersion cannot be greater than MaxClientVersion.");

				this.MinClientVersion = minClientVersion;
				this.MaxClientVersion = maxClientVersion;
				this.AbsoluteValue = absoluteValue;
				this.OffsetFromCanonical = offsetFromCanonical;
			}
		}
	}

	internal static class SubOpcodeMapper
	{
		private static readonly Dictionary<NormalOpType, Dictionary<int, SubOpcodeVersionMapping>> Mappings =
			new();

		static SubOpcodeMapper()
		{
			InitializeMappings();
		}

		private static void InitializeMappings()
		{
			// --- General Versioning Rules ---
			// These apply if no more specific rule (like the ~2017 range) matches.
			Action<SubOpcodeVersionMapping> addGeneralPre2017Rules = mapping =>
			{
				var subOpGreaterThan0xA = mapping.CanonicalValue > 0x0A;

				// Rule for clientVersion in (100000, 354443) BUT EXCLUDING the ~2017 range if it falls within.
				// The ~2017 range (148158-185517) will have its own more specific rule.
				// This rule effectively becomes for (100001, 148157) and (185518, 354443)
				if (!(mapping.CanonicalValue >= KnownVersions.ClientVer148158 && mapping.CanonicalValue <= KnownVersions.ClientVer185517))
				{
					// This logic needs to be careful. The rule is on the CLIENT VERSION, not canonical value.
					// AddRule(min, max, ...) means for client versions between min and max.
				}
				// Simpler: Apply broad rules, then ~2017 rules will override due to sorting if min/max overlap.

				mapping.AddRule(100001, KnownVersions.ClientVer148158 - 1, offsetFromCanonical: (subOpGreaterThan0xA ? -1 : 0));
				mapping.AddRule(KnownVersions.ClientVer185517 + 1, 354443, offsetFromCanonical: (subOpGreaterThan0xA ? -1 : 0));


				// Rule for clientVersion in (ClosedBeta2, 100000)
				mapping.AddRule(KnownVersions.ClosedBeta2 + 1, 99999, offsetFromCanonical: (subOpGreaterThan0xA ? -2 : 0));

				// Rules for clientVersion <= KnownVersions.ClosedBeta2
				var cb2BaseEffectiveOffset = subOpGreaterThan0xA ? -3 : 0;
				if (mapping.CanonicalValue >= 0x130)
				{
					mapping.AddRule(0, KnownVersions.ClosedBeta2, offsetFromCanonical: -0x10);
				}
				else
				{
					var finalCb2Offset = cb2BaseEffectiveOffset;
					if (mapping.CanonicalValue >= 0x90) finalCb2Offset -= 8;
					if (mapping.CanonicalValue >= 0xC0) finalCb2Offset -= 1;
					mapping.AddRule(0, KnownVersions.ClosedBeta2, offsetFromCanonical: finalCb2Offset);
				}
			};


			foreach (NormalOpType opType in Enum.GetValues(typeof(NormalOpType)))
			{
				var constantsClass = GetConstantsClassForType(opType);
				if (constantsClass == null) continue;

				var typeMappings = new Dictionary<int, SubOpcodeVersionMapping>();
				foreach (var field in constantsClass.GetFields(BindingFlags.Public | BindingFlags.Static))
				{
					if (field.FieldType != typeof(int)) continue;

					var canonicalValue = (int)field.GetValue(null);
					var name = $"{constantsClass.Name}.{field.Name}";

					// If a mapping for this canonical value already exists (e.g. duplicate const values),
					// reuse it if they are meant to be identical, or handle as error if names differ but values clash
					// For now, assume same canonical value means same sub-op with same rules.
					SubOpcodeVersionMapping mapping;
					if (typeMappings.TryGetValue(canonicalValue, out var existingMapping))
					{
						mapping = existingMapping;
						// Optionally log or check if field.Name differs for same canonicalValue
					}
					else
					{
						mapping = new SubOpcodeVersionMapping(canonicalValue, name);
						typeMappings[canonicalValue] = mapping;
					}

					// Apply general pre-~2017 rules (these are for versions older than the 148158-185517 range)
					addGeneralPre2017Rules(mapping);

					// --- Type-specific overrides for CB1/CB2 (super old) ---
					if (opType == NormalOpType.Barracks && mapping.CanonicalValue > 0x0A)
					{
						mapping.AddRule(KnownVersions.ClosedBeta1, KnownVersions.ClosedBeta2, offsetFromCanonical: -2);
					}

					// For Social, Integrate, GuildOp in CB1/CB2: `this.PutSubOp(subOp, false);` - meant no offset from canonical.
					if (opType == NormalOpType.Social || opType == NormalOpType.Integrate || opType == NormalOpType.Guild)
					{
						mapping.AddRule(KnownVersions.ClosedBeta1, KnownVersions.ClosedBeta2, offsetFromCanonical: 0);
					}

					// --- Rules for the ~2017 client range (148158 to 185517) ---
					// General offset of -2 for this range for ZC_NORMAL type.
					// This rule will be applied first for this version range due to sorting if AddRule correctly sorts.
					// The AddRule sorting should prioritize higher MinClientVersion, so these ~2017 rules will naturally
					// take precedence over broader, older rules if their MinClientVersion is higher.
					if (opType == NormalOpType.Zone && !name.StartsWith("NormalOp.Zone.OLD_")) // Don't apply general offset to OLD_ specific consts
					{
						// General offset for LATEST ops when targeting this ~2017 range
						mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, offsetFromCanonical: -2);
					}

					// --- Specific ABSOLUTE VALUE overrides for ~2017 range (ZC_NORMAL_PACKETS.txt) ---
					// These will override the general -2 offset for this range if defined.
					if (opType == NormalOpType.Zone)
					{
						// If it's an OLD_ prefixed constant, its canonical value *is* its old value.
						// So, for the ~2017 range, it should map to itself.
						if (name.StartsWith("NormalOp.Zone.OLD_"))
						{
							mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: canonicalValue);
						}
						else // These are LATEST ops, mapping to their specific values from ZC_NORMAL_PACKETS.txt
						{
							// Conceptual match: UpdateSkillEffect (0x1F latest) was SKILLMAP_INFO (0x1B old)
							if (canonicalValue == NormalOp.Zone.UpdateSkillEffect)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x1B);

							// Add other absolute overrides here where the -2 offset is not correct
							// or where the conceptual mapping is to a very different old value.
							// Example from previous discussion:
							if (canonicalValue == NormalOp.Zone.PlayEffect) // LATEST 0x16
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x15); // Overrides the -2 rule
							if (canonicalValue == NormalOp.Zone.PlayForceEffect) // LATEST 0x17
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x16); // Overrides the -2 rule
							if (canonicalValue == NormalOp.Zone.RemoveCorpseParts) // LATEST 0xDA
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x1A); // Significant jump, overrides -2
							if (canonicalValue == NormalOp.Zone.Skill_DynamicCastStart) // LATEST 0x4F
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x4E); // Offset is -1, overrides general -2
							if (canonicalValue == NormalOp.Zone.Skill_DynamicCastEnd) // LATEST 0x50
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x4F); // Offset is -1, overrides general -2

							if (canonicalValue == NormalOp.Zone.EnterDelayedActor) // LATEST 0xC8
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0xC1); // Offset is -7
							if (canonicalValue == NormalOp.Zone.DelayEnterWorld) // LATEST 0x99
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x96); // Offset is -3
							if (canonicalValue == NormalOp.Zone.CutsceneTrack) // LATEST 0x96 (PLAY_DIRECTION)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x93); // Offset is -3
							if (canonicalValue == NormalOp.Zone.SetTrackFrame) // LATEST 0x97 (RESUME_DIRECTION)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x94); // Offset is -3
							if (canonicalValue == NormalOp.Zone.ShowComboEffect) // LATEST 0x1E (FEVER_COMBO)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x1C); // Offset is -2 (matches general)
							if (canonicalValue == NormalOp.Zone.SetActorColor) // LATEST 0x20 (OBJECT_COLOR_BLEND)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x1E); // Offset is -2 (matches general)
							if (canonicalValue == NormalOp.Zone.SkillChangeAnimation) // LATEST 0x40 (CHANGE_SKILL_ANIM)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x3E); // Offset is -2 (matches general)
							if (canonicalValue == NormalOp.Zone.SkillResetAnimation) // LATEST 0x41 (CHANGE_SKILL_ANIM_CLEAR)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x3F); // Offset is -2 (matches general)
							if (canonicalValue == NormalOp.Zone.NPC_PlayTrack) // LATEST 0x53 (PRELOAD_DIRECTION)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x51); // Offset is -2 (matches general)
							if (canonicalValue == NormalOp.Zone.SetNPCTrackPosition) // LATEST 0x54 (DIRECTION_INIT_POS)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x53); // Offset is -1
							if (canonicalValue == NormalOp.Zone.PadSetMonsterAltitude) // LATEST 0x5C (PADSKILL_ATTACH_OBJ)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x5A); // Offset is -2 (matches general)
							if (canonicalValue == NormalOp.Zone.PadCreateObstacle) // LATEST 0x5D (PADSKILL_CREATE_OBSTACLE)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x5B); // Offset is -2 (matches general)
							if (canonicalValue == NormalOp.Zone.RunJumpRope) // LATEST 0x123 (JUMP_ROPE start)
								mapping.AddRule(KnownVersions.ClientVer148158, KnownVersions.ClientVer185517, absoluteValue: 0x121); // Offset is -2 (matches general)
						}
					}
				}
				Mappings[opType] = typeMappings;
			}
		}

		private static Type GetConstantsClassForType(NormalOpType opType)
		{
			switch (opType)
			{
				case NormalOpType.Barracks: return typeof(NormalOp.Barrack);
				case NormalOpType.Zone: return typeof(NormalOp.Zone);
				case NormalOpType.Social: return typeof(NormalOp.Social);
				case NormalOpType.Integrate: return typeof(NormalOp.Integrate);
				case NormalOpType.Guild: return typeof(NormalOp.GuildOp);
				default: return null;
			}
		}

		public static int GetVersionedSubOp(NormalOpType type, int canonicalSubOp, int clientVersion)
		{
			if (Mappings.TryGetValue(type, out var typeMappings)
				&& typeMappings.TryGetValue(canonicalSubOp, out var mapping))
			{
				return mapping.GetVersionedValue(clientVersion);
			}
			// Optional: Log warning for missing mapping
			Log.Warning($"No mapping found for {type}, subOp {canonicalSubOp:X4}, clientVersion {clientVersion}. Returning canonical value.");
			return canonicalSubOp; // Fallback to canonical value if no specific mapping is found
		}
	}
}
