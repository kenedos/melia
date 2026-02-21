using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Scripting.Shortcuts;

namespace Melia.Zone.Scripting.Shared
{
	public static class Triggers
	{
		public static async Task COMMON_QUEST_HANDLER(TriggerActorArgs args)
		{
			if (args.Type == TriggerType.Enter)
			{
				//await dialog.Hooks(dialog.Npc.EnterName, "BeforeStart");
				//await dialog.Hooks(dialog.Npc.EnterName, "BeforeEnd");
			}
			else if (args.Type == TriggerType.Leave)
			{
				//await dialog.Hooks(dialog.Npc.LeaveName, "BeforeStart");
				//await dialog.Hooks(dialog.Npc.LeaveName, "BeforeEnd");
			}
		}

		[TriggerFunction]
		public static async Task ABBEY_35_3_ABBEY_35_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_3_DARK_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_3_DOMINIKAS_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_3_SIAULIAI_35_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_3_SQ_4_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_3_VILLAGE_A(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_3_VILLAGE_B(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_3_VILLAGE_C(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_ABBEY_35_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_DARK_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_DARK_WALL_OBB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_DOMINIKAS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_SQ_1_FAKE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_SQ_1_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_SQ_8_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_TO_CORAL_44_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_TO_ROKAS_36_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY_35_4_UNHOLY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_ABBEY22_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ6_PEEPLE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ6_PEEPLE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ7_UNKNOWN_GUIDE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ7_UNKNOWN_GUIDE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ7_UNKNOWN_GUIDE3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ7_UNKNOWN_GUIDE4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ7_UNKNOWN_GUIDE5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ7_UNKNOWN_OBJ_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_SUBQ7_UNKNOWN_OBJ_OUT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_4_WHITETREES22_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_5_ABBEY22_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_5_SUBQ11_NPC1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY22_5_WHITETREES56_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY225_FLURRY1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY225_FLURRY3_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY225_SUBQ1_HIDDEN_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY225_SUBQ5_NPC1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_MQ_01_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_MQ_02_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_MQ_05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_MQ_06_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_MQ_09_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_MQ_10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_MQ_10_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_MQ_10_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_TO_THORN393(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_WALL_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY394_WALL_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY41_6_PILGRIM41_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY416_SQ_01_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY416_SQ_01_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY416_SQ_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY416_SQ_08_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY416_SQ_08_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY416_SQ_09_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY416_SQ_09_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ABBEY416_SQ_10_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ADDHELP_NPCSHOP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task AGARIO_MINE_DMG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task Assistor_TUTO_04_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BAUBAS_GO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BEAUTY_BOUTIQUE_MOVE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BEAUTY_HAIRSHOP_MOVE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BEAUTY_IN_MOVE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BEAUTY_OUT_MOVE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BORUTOSKAPAS_GO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN42_1_TO_BRACKEN42_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN42_1_TO_EP13_F_SIAULIAI_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN42_1_TO_LIMESTONECAVE52_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN42_1_TO_PILGRIM41_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN42_2_TO_BRACKEN42_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN421_SQ_03_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN421_SQ_04_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN421_SQ_08_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN422_SQ_08(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN422_SQ_09(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN422_SQ_10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN43_1_3CMLAKE83(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN43_1_BRACKEN43_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN431_SUBQ_ALTAR_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN431_SUBQ_START_NPC_MSG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN431_SUBQ6_NPC1_ORB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN431_SUBQ6_NPC2_ORB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN431_SUBQ6_NPC3_ORB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN431_SUBQ6_NPC4_ORB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN432_BRACKEN431(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN432_BRACKEN433(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN432_CORAL441(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN432_FLOWER_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN432_SUB1_FENCE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN432_SUBQ_NPC2_CHECK_POINT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN432_SUBQ_NPC3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN432_SUBQ7_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN433_BRACKEN432(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN433_BRACKEN434(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN433_LIMESTONECAVE551(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN433_SUBQ5_TRACK_NPC1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN433_SUBQ5_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_BRACKEN433(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUB6_NPC2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ_FAKE_NPC2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ_FAKE_NPC3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ10_TRACK_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ3_FAKE_NPC1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ4_HIDDEN1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ4_HIDDEN2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ4_HIDDEN3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ4_HIDDEN4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ4_HIDDEN5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task BRACKEN434_SUBQ4_HIDDEN6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_3CMLAKE_26_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_BARRIER_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_BARRIER_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_CASTLE_20_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_CASTLE_93(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_OBJ_2_1_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_OBJ_2_2_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_OBJ_2_3_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_OBJ_4_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_1_SQ_4_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_2_CASTLE_20_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_2_CASTLE_20_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_2_CASTLE_94(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_2_OBJ_6_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_2_SQ_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_2_SQ_6_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_3_CASTLE_20_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_3_CASTLE_20_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_3_DCAPITAL_103(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_3_MAPLE_25_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_3_OBJ_7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_3_TABLELAND74(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_CASTLE_20_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_CASTLE_97(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_DCAPITAL_103(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_DCAPITAL_20_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_OBJ_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_SQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_SQ_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_SQ_5_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE_20_4_SQ_7_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE94_MAIN05_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE94_NPC_MAIN01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE95_MAIN03_END(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE96_MQ_2_TRG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CASTLE96_MQ_5_TRG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_01_EVILAURA_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_01_SPIRIT_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_01_TO_GELE_57_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_02_CATACOMB_38_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_02_OBJ_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_02_OBJ_06(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_02_REMAINS37_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_02_WARP_A(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_02_WARP_B(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_04_CATACOMB_38_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_04_CATACOMB_38_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_04_REMAINS37_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_100_WARP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_110(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_120(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_50(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_70_1_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_70_2_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_70_3_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_70_4_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_SQ_80_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_25_4_TO_MAPLE_25_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_38_1_CATACOMB_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_38_1_CATACOMB_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_38_1_CATACOMB_38_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_38_1_SQ_07_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_38_2_CATACOMB_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_38_2_CATACOMB_38_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_38_2_SQ_06_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_38_2_THORN_39_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATACOMB_80_2_HEAL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL_SQ_OBJECT01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL_SQ_OBJECT02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL_SQ_OBJECT03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL_SQ_OBJECT04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL_SQ_OBJECT05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL53_CATHEDRAL54(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL53_PILGRIM52(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL54_CATHEDRAL53(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL54_CATHEDRAL56(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL54_HIDDEN_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL54_PILGRIM55(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL54_PILGRIM55_RE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CATHEDRAL56_CATHEDRAL54(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL575_BASIC_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL575_CHAPEL576(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL575_GELE574(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL576_BASIC_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL576_BASIC_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL576_BASIC_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL576_CHAPEL575(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL576_CHAPEL577_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL576_GELE574(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL577_BASIC_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL577_BASIC_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPEL577_CHAPEL576(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPLE_57_6_HQ_01_ENTER_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPLE_576_HQ01_SESSION_CREATE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPLE_576_HQ01_SESSION_DESTROY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPLE575_MQ_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPLE577_GESTI(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAPLE577_SECRET_ROOM_WARP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR120_MSTEP5_NPC1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR220_MSETP2_1_2_GEN_INIT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR220_MSETP2_2_1_GEN_INIT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR220_MSETP2_2_2_GEN_INIT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR220_MSETP2_5_GEN_INIT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR220_MSETP2_6_GEN_INIT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR220_MSETP2_7_GEN_INIT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR312_MSTEP1_GEN_INIT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR312_MSTEP2_GEN_INIT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHAR312_PRE_MSTEP1_SOLDIER2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL_FINAL_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL_GATE_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL54_PART1_OBJ1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL54_PART1_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL54_SQ01_PART1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL56_MQ04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL56_MQ08_BLUE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL56_MQ08_GREEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL56_MQ08_PURPLE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL56_MQ08_RED(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL56_MQ08_YELLOW(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL56_SEAL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CHATHEDRAL56_SQ05_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CMINE_6_CMINE_8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CMINE_8_CMINE_6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_1_HIDDEN_TRAP1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_1_HIDDEN_TRAP2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_1_SQ_6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_2_SQ_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_2_SQ_1_NOTICE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_2_SQ_13_H1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_2_SQ_13_H2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_2_SQ_13_H3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_2_SQ_13_H4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_32_2_SQ_6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_CRYSTAL_MARINE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_CRYSTAL_TERRA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_HARMONY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_HARMONY_MINI(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_MARINE_MINI(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_SQ_1_START(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_SQ_10_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_SQ_11_CRYSTAL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_SQ_2_START(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_SQ_4_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_SQ_HARMONY_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_SQ_MARINE_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_SQ_TERRA_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_TERRA_MAKING(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_35_2_TERRA_MINI(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_1_TO_BRACKEN_43_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_1_TO_CORAL_44_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_2_SQ_20_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_2_SQ_40_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_2_TO_CORAL_44_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_2_TO_CORAL_44_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_3_SQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_3_SQ_40_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_3_SQ_60_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_3_TO_ABBEY_35_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL_44_3_TO_CORAL_44_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL32_1_CORAL32_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL32_1_ORCHARD34_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL32_2_CORAL32_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task CORAL35_2_SIAULIAI_35_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_CASTLE_19_1_MQ_03_OBJ_CHECK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_CASTLE_19_1_MQ_05_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_CASTLE_19_1_PORTAL_NPC_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_CASTLE_19_1_TO_F_CASTLE_97(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_CASTLE_19_1_TO_F_DCAPITAL_107(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_CMINE_9_DROPSTONET(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_DCAPITAL_108_ROOM1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_DCAPITAL_108_ROOM2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_DCAPITAL_108_ZEMINA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_FANTASYLIB_48_1_COLLECTION_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_FANTASYLIB_48_3_COLLECTION_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_FANTASYLIB_48_4_COLLECTION_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_88_1ST_DEFENCE_DEVICE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_88_TO_D_STARTOWER_89(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_88_TO_F_3CMLAKE_87(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_89_MQ_20_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_89_MQ_50_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_89_MQ_60_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_89_MQ_80_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_89_TO_D_STARTOWER_88(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_89_TO_D_STARTOWER_90(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_90_MQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_90_MQ_30_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_90_MQ_50_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_90_MQ_60_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_90_MQ_HIDDENWALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_90_TO_D_STARTOWER_89(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_90_TO_D_STARTOWER_91(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_91_MQ_50_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_91_MQ_60_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_91_MQ_90_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_91_TO_D_STARTOWER_90(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_91_TO_D_STARTOWER_92(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_92_MQ_30_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_92_MQ_40_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_92_MQ_50_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task D_STARTOWER_92_TO_D_STARTOWER_91(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_103_SHADOW_DEVICE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_105_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_5_SQ_80_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_5_TO_CASTLE_20_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_5_TO_DCAPITAL_104(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_5_TO_DCAPITAL_20_6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_SQ_10_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_SQ_110_RUN_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_SQ_110_RUN_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_SQ_110_RUN_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_SQ_110_RUN_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_SQ_30_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_SQ_60_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_TO_CASTLE_99(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL_20_6_TO_DCAPITAL_20_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL103_SQ_06(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL103_TO_CASTLE20_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL103_TO_CASTLE20_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL103_TO_DCAPITAL104(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL103_TO_DCAPITAL105(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL105_SQ_1_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL105_SQ_6_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL106_SQ_2_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL106_SQ_4_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL106_SQ_6_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL106_SQ_7_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL107_SQ_1_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL107_SQ_3_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL107_TRAP1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL53_1_MQ_03_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DCAPITAL53_1_MQ_05_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task DTAMOVING(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_BLACKSTONE_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ03_GUIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ03_GUIDE_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_01_GUIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ05_GUIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ06_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ08_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ08_GUIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ09_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ11_GUIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ11_GUIDE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_D_DCAPITAL_108_MQ16_TRACK_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_F_CASTLE_101_MQ01_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_F_CASTLE_101_MQ02_BOWER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_F_CASTLE_101_MQ02_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_F_CASTLE_101_MQ03_1_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_F_CASTLE_101_MQ04_STONE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_F_CASTLE_101_MQ04_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_2_F_CASTLE_101_MQ05_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_FINALE_DIRECTION_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_FINALE_MQ_03_OBJECT01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_FINALE_MQ_03_OBJECT02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_FINALE_MQ_03_OBJECT03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_FINALE_MQ_03_OBJECT04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_PRELUDE_04_START_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP12_PRELUDE_09_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON1_MQ_NPC_9(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON2_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON2_MQ_NPC_7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON2_MQ_SAFEZONE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_DUMMY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_NPC_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_NPC_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_NPC_L_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_NPC_L_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_NPC_R_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_NPC_R_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_L1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_L2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_L3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_L4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_L5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_L6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_L7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_L8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_R1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_R2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_R3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_R4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_R5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_R6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_R7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_2_DPRISON3_MQ_TRAP_R8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_1_MQ_04_TRACK_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_1_SQ_02_TRG_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_1_SQ_04_TRG_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_1_SQ_05_TRG_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_2_MQ_04_SMOKE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_2_MQ_05_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_2_MQ_06_SMOKE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_2_MQ_07_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_2_MQ_08_TRACK_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_3_HQ_01_TRG_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_3_MQ_04_TRACK_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_3_MQ_07_TRACK_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_4_BOMB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_4_MQ03_TRIG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_4_MQ03_TRIG2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_4_MQ04_WAY04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_4_MQ07_TRIG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_4_ZEMINA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_5_ENDPOINT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_5_MQ_01_SCRL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_5_MQ_01_TRIG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_5_WAVE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP13_F_SIAULIAI_5_WAVE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_F_CASTLE_1_NPC2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_F_CASTLE_2_06_EV_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_F_CASTLE_2_MQ_DUMMY_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE1_MQ_6_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE1_MQ_7_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_1_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_1_NPC2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_3_AFTER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_3_AFTER2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_3_AFTER3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_3_AFTER4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_4_UNHIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_5_UNHIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_6_UNHIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE2_MQ_7_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE3_MQ_1_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE3_MQ_5_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE3_MQ_5_SOLDIER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE3_MQ_8_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE4_MQ_1_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE4_MQ_2_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE4_MQ_3_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE4_MQ_5_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE4_MQ_7_FERRET(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE4_MQ_8_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE5_MQ_2_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE5_MQ_3_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE5_MQ_4_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE5_MQ_5_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE5_MQ_6_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE5_MQ_7_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_1_FCASTLE5_MQ_8_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASLTE2_ZEMINA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE_CAVEWARP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE_CAVEWARP2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE1_MQ_7_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE2_CAVEWARP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE2_CAVEWARP2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE2_MQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE2_MQ_6_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE3_CAVEWARP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE3_CAVEWARP2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_2_DCASTLE4_MQ_5_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_3LINE_TUTO_MQ_1_GATE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_3LINE_TUTO_MQ_1_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_3LINE_TUTO_MQ_4_GATE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_3LINE_TUTO_MQ_4_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_3LINE_TUTO_MQ_5_GATE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_3LINE_TUTO_MQ_9_1_GATE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_3LINE_TUTO_MQ_9_1_HELP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_3LINE_TUTO_MQ_9_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP14_FCASTLE4_FIELDNPC_TRANING(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_F_ABBEY_1_6_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_F_ABBEY_1_8_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_F_ABBEY2_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_F_ABBEY2_4_BOWER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_F_ABBEY2_4_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_F_ABBEY3_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_F_ABBEY3_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_F_ABBEY3_5_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_1_FABBEY2_ZEMINA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_1_MQ_1_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_1_MQ_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_1_MQ_5_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_1_MQ_6_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_1_MQ_8_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_1_NICO811(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_2_MQ_1_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_2_MQ_2_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_2_MQ_3_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_2_MQ_4_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_2_MQ_5_HIDDEN_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP15_2_D_NICOPOLIS_2_MQ_5_HIDDEN_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP16_1_CORAL_32_1_MQ4_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP16_1_F_NICO_81_1_MQ_1_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP16_1_PILGRIM_36_2_MQ12_GRAVE_GOLEM(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EP16_1_ROKAS_36_1_MQ8_HIDDEN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXORCIST_MASTER_STEP33_NPC1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXORCIST_PLACE_HIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE11(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE13(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE14(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE15(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE16(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE17(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE18(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE19(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE20(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE21(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE22(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE23(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE24(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE25(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE26(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE27(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE28(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE29(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE30(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE31(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE32(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE33(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE34(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE35(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE36(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE37(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE38(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE39(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE40(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE41(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE42(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE43(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE44(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE45(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE46(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE47(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE48(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE49(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE50(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task EXPLORER_MISLE9(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_26_1_COLLECTION_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_26_1_TO_F_3CMLAKE_26_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_26_1_TO_F_3CMLAKE_85(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_26_1_TO_F_CASTLE_20_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_26_2_COLLECTION_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_26_2_TO_F_3CMLAKE_26_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_26_2_ZEMINA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_27_1_SQ_7_TRACK_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_27_1_TO_F_3CMLAKE_27_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_27_1_TO_F_3CMLAKE_87(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_27_2_TO_F_3CMLAKE_27_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_27_2_TO_F_3CMLAKE_27_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_27_2_TO_F_CASTLE_95(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_27_3_SQ_7_HNPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_27_3_TO_F_3CMLAKE_27_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_85_MQ_02_3_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_85_MQ_03_OBJ_PRE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_85_MQ_05_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_85_MQ_07_AFTER_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_85_MQ_08_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_85_MQ_09_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_85_TO_F_3CMLAKE_26_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_85_TO_F_3CMLAKE_86(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_02_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_06_TRRIGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_07_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_08_CHECK2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_10_TRRIGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_11_TRRIGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_12_TRRIGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_16_CHECK1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_16_CHECK2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_MQ_16_CHECK3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_SQ_02_AFTER_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_TO_F_3CMLAKE_85(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_TO_F_3CMLAKE_87(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_WOODENWALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_86_WOODENWALL_DESTROY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_MQ_08_OBJ_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_MQ_08_OBJ_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_MQ_08_OBJ_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_MQ_09_TRRIGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_MQ_10_TRRIGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_MQ_11_OBJ_CHECK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_MQ_12_TRRIGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_MQ_13_TRRIGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_TO_D_STARTOWER_88(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_TO_F_3CMLAKE_27_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_TO_F_3CMLAKE_86(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE_87_TO_F_CASTLE_93(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_3CMLAKE262_SQ01_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_93_TO_F_3CMLAKE_87(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_93_TO_F_CASTLE_20_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_93_TO_F_CASTLE_94(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_94_TO_F_CASTLE_20_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_94_TO_F_CASTLE_93(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_94_TO_F_CASTLE_95(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_95_TO_F_3CMLAKE_27_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_95_TO_F_CASTLE_94(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_95_TO_F_CASTLE_96(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_96_TO_F_CASTLE_95(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_96_TO_F_CASTLE_98(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_97_MQ_02_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_97_MQ_03_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_97_MQ_04_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_97_TO_D_CASTLE_19_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_97_TO_F_CASTLE_99(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_98_TO_F_CASTLE_96(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_02_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE_AFTER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE_AFTER2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE_AFTER3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE_AFTER4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE_AFTER5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE_AFTER6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_STONE6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_03_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_MQ_05_OBJ_CHECK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_TO_F_CASTLE_102(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_CASTLE_99_TO_F_CASTLE_97(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_FLASH_64_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_KATYN_14_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_241_MQ_01_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_241_MQ_01_OBJ_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_241_TO_F_MAPLE_242(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_241_TO_F_MAPLE_243(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_241_TO_WHITETREES23_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_242_MQ_04_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_242_MQ_07_OBJ1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_242_MQ_07_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_242_MQ_08_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_242_MQ_08_OBJ_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_242_MQ_MEDEINA_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_242_TO_F_MAPLE_241(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_02_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_05_FLOWER1_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_05_FLOWER2_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_05_FLOWER3_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_05_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_07_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_07_OBJ_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_08_BOSS_MONSTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_09_TREE1_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_09_TREE1_BEFORE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_09_TREE2_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_09_TREE2_BEFORE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_09_TREE3_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_09_TREE3_BEFORE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_09_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_MQ_11_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_TO_F_MAPLE_241(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_TO_MAPLE23_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_MAPLE_243_TO_MAPLE25_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_ROKAS_31_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_SIAULIAI_OUT_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_SIAULIAI_WEST_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task F_TABLELAND_28_2_RAID_05_NPC_00(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FALLENDRAGOON_GO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB482_MQ_6_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB483_MQ_7_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB483_MQ_8_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB484_MQ_2_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB485_MQ_1_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB485_MQ_2_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB485_MQ_4_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB485_MQ_6_BOOKS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FANTASYLIB485_MQ_6_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_1_JOB_5_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_1_JOB_5_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_1_TO_FARM_47_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_1_TO_FARM_47_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_1_TO_HUEVILL_58_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_2_TO_FARM_47_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_2_TO_FARM_47_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_2_TO_FARM_49_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_3_TO_FARM_47_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_47_3_TO_FARM_47_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_49_1_JOB_5_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM_49_1_JOB_5_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_2_CROP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_2_LEAVES(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_ALTAR01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_BOSS_CIRCLE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_BOSS_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_CHECK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_CORRUPT_EVENT_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_CORRUPT_EVENT_L(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_DALIUS_CORRUPT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_DOWN_SACK_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_DRUM01_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_DRUM02_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_FENCE_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_HEAD_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_MAGIC_FAKE_OUT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_MAGIC04_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_MAGIC11(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_MAGIC31_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_NORMAL_CROP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_ODD_FEEL_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_STATUE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_WHOUSE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM47_WING_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM491_TO_FARM472(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM491_TO_FARM492(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM492_MQ_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM492_MQ_04_FARMER_W(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM492_MQ_06_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM492_SQ_08_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM492_TO_FARM491(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM492_TO_FARM493(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM492_TO_SIAULIAI464(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM493_MQ_05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM493_SQ_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM493_SQ_06_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FARM493_TO_FARM492(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FD_STARTOWER762_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FED_EQUIP_HQ_REINFORCE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FEDIMIAN_REQUEST1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FEDIMIAN_TERIAVELIS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FEDMIAN_PILGRIM46(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FEDMIAN_TO_REMAINS40(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER41_TO_FIRETOWER42(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER41_TO_REMAINS40(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER42_TO_FIRETOWER41(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER42_TO_FIRETOWER43(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER43_TO_FIRETOWER42(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER43_TO_FIRETOWER44(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER44_TO_FIRETOWER43(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER44_TO_FIRETOWER45(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER44_TO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER45_TO_FIRETOWER44(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER611_TO_FIRETOWER612(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER611_TO_PILGRIMROAD52(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER612_TO_FIRETOWER611(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER691_MQ_2_NPC_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FIRETOWER691_TO_REMAINS37(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_29_1_FIRST_DETECTOR(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_29_1_TO_FLASH_58(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_29_1_TO_FLASH_63(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_58_TO_FLASH_29_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_58_TO_FLASH_64(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_63_ELT_PART1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_64_TO_FLASH_58(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_SOLDIER01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_SOLDIER02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_SOLDIER03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_SOLDIER04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_SOUL_COLLECTOR_S1_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_SOUL_COLLECTOR_S2_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH_SOUL_COLLECTOR_S3_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH29_1_HIDDENQ1_ORB1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH29_1_HIDDENQ1_ORB2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH29_1_HIDDENQ1_ORB3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH29_1_HIDDENQ1_ORB4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH29_1_SQ_100_OBJ1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH59_FLASH60(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH59_FLASH61(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH59_FLASH63(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH59_PILGRIM52(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH59_SQ_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH60_FLASH59(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH60_FLASH61(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH60_TABLELAND28_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH61_FLASH59(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH61_FLASH60(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH61_TABLELAND11_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH61_TO_CASTLE_MISSION_RN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH63_FLASH29_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH63_FLASH59(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH63_FLASH64(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH63_SQ13_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH64_FLASH63(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH64_TABLELAND70(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FLASH64_UNDERFORTRESS65(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER_69_2_G1_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER42_MQ_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER42_SQ_SUB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER43_MQ_04_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER43_MQ_06_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER44_MQ_05_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER44_SQ_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER45_GRITA_PHOENIX(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER45_MQ_01_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER45_MQ_02_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER45_MQ_03_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER45_MQ_04_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER45_MQ_05_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER45_SQ_05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task FTOWER45_SQ_T(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE_57_4_TO_CATACOMB_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE57_2_KATYN45_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE57_3_PILGRIM41_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE571_GELE572(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE571_MQ_04_TGT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE571_MQ_07_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE571_SIALLAIOUT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE572_BOSS_LOCK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE572_GELE571(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE572_GELE573(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE572_MQ_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE572_MQ_05_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE573_BASIC_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE573_BASIC_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE573_TO_GELE572(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE573_TO_GELE574(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE573_TO_HUE581(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE574_ARUNE_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE574_KATYN_7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE574_TO_CHAPEL575_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE574_TO_CHAPEL576(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE574_TO_GELE573(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GELE574_TO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GHOST_APPEAR(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GILTINE_GO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GIVEHOLYTHING_Q_1_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GLACIER_RAID_TUTO_RP_ENTER_NPC_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task GT2_INIT_MGAME_STARTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HACKAPELL_MASTER_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HIDDEN_WATER_REMAINS_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_11(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT_PATRIFICATION_NPC_9(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP06(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP07(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP08(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP09(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP11(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_FARM493_CAVEWARP12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_THORN23_ALAN_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_THORN23_WALLACE_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HT2_THORN23_WISE_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILL_58_3_TO_FARM_47_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_1_MQ02_SMOKE_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_1_MQ11_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_1_SIAUL50_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_2_OBELISK_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_2_SIAUL50_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_3_MQ04_NPC02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_3_MQ04_TO_HUE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_4_MQ01_MAGIC01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_4_MQ01_STONE_STATUE01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_4_MQ01_STONE_STATUE02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_4_MQ01_VINE01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_4_MQ01_WALL01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_58_4_MQ09_BLACK_MAN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_SOUL01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_SOUL02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_SOUL03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE_SOUL04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_1_TO_GELE573(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_1_TO_HUEVILLAGE58_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_2_TO_HUEVILLAGE58_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_2_TO_HUEVILLAGE58_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_3_TO_HUEVILLAGE58_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_3_TO_HUEVILLAGE58_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_4_KATYN13(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_4_SIAULIAI46_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_4_TO_HUEVILLAGE58_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task HUEVILLAGE58_4_TO_THORN19(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_2_HOPLITE4_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_2_THAUMATURGE_6_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_3_DRUID_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_ASSASSIN_Q1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_BOCOR4_4_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_BULLETMARKER1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_CANNONEER_8_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_CENTURION5_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_CHROSOC5_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_CLERIC_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_CLERIC2_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_CRYOMANCER4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_DIEVDIRBYS2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_FALCONER_8_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_FEDIMIAN_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_FENCER_7_1_WOOD_CARVING(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_FLETCHER_6_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_FLETCHER5_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_HIGHLANDER4_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_HUNTER2_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_HUNTER2_3_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_JOB_SORCERER4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_KLAIPE_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_KRIVI4_2_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_LINKER2_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_LINKER4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_MATADOR1_Q_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_MURMILLO_8_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_MUSKETEER_8_1_WOOD_CARVING(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_NECROMANCER4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_ONMYOJI_Q1_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_ORACLE_6_1_OBJ1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_ORACLE_6_1_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_ORACLE_6_1_OBJ3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_OUTLAW_Q1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PALADIN3_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PARDONER4_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PELTASTA4_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PIED_PIPER_Q1_OBJ(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PRIEST2_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PSYCHOKINESIST3_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PSYCHOKINESIST4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PYROMANCER3_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_PYROMANCER4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_QUARREL3_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_QUARRELSHOOTER1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_ROGUE4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_ROGUE5_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_SAPPER2_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_SPYLIAL5_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_SWORDMAN1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_THAUELE5_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_THAUMATURGE4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_WARLOCK4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_WIPYCRY5_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task JOB_WIZARD4_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_13_2_GELE574(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_13_2_HQ_01_TRIGGER01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_13_2_HQ_01_TRIGGER02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_18_RE_SQ_OBJ_3_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_18_RE_SQ_OBJ_3_KEEP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_18_RE_SQ_OBJ_5_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_45_1_OWL2_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_45_2_SQ_13(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_45_3_OWL4_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_45_3_SCULPT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_45_3_SCULPT2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_45_3_SCULPT3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_45_3_SQ_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_7_2_HQ01_NPC01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN_7_2_JOB_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_1_KEY_SUB1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_1_TO_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_1_TO_OWLJUNIOR2_S1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_1_TO_OWLJUNIOR3_S1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_2_HUEVILLAGE58_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_2_KATYN12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_2_THORN22(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_3_KATYN12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_3_ROKAS25(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_ADDQUEST1_TRIGGER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_ADDQUEST2_TRIGGER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_ADDQUEST3_TRIGGER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_ADDQUEST4_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_HUEVILLAGE58_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_KATYN14(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN13_KATYN7_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_BOSS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_GOBACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_LAIMUNAS_SOL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_PREBOSS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_SUB_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_SUB_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_SUB_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_SUB_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_SUB_05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_SUB_06(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_SUB_07(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_SUB_08(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN14_THORN22(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN17_REMAINS37(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN17_ROKAS29(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN17_ROKAS31(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN17_SIAUL15(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN45_1_GELE57_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN45_1_KATYN45_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN45_1_PILGRIM41_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN45_1_PILGRIM41_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN45_2_KATYN45_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN45_2_KATYN45_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN45_3_KATYN45_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN7_2_ADD_BOSS_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN7_2_BLOCK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN7_2_KATYN13(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN7_2_KATYN7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN7_GELE574(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN7_KATYN7_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN7_KEYNPC_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN7_PREBOSS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN71_DEADSOL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN71_SOLDIER01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN71_SOLDIER02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KATYN72_SUB_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KLAIPE_HQ_01_NPC_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KLAPEDA_FISHING_CAT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task KLAPEDA_TO_SIAUL50_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESOTNE_52_5_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_1_MAGIC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_1_MAGIC_ON(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_1_MQ_10_PROG_TRG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_1_MQ_3_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_1_MQ_8_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_2_DARKWALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_2_DARKWALL_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_2_MQ_1_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_2_MQ_3_FAKE_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_2_MQ_3_FAKE_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_2_MQ_3_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_3_MQ_3_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_3_MQ_9_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_4_BLACKMAN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_4_MQ_1_PROG_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_4_MQ_1_PROG_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_4_MQ_1_PROG_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_4_MQ_6_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_4_MQ_7_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_5_GESTI_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_5_MQ_2_FAKE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONE_52_5_MQ_2_PROG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_1_LIMESTONECAVE_52_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_2_LIMESTONECAVE_52_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_2_LIMESTONECAVE_52_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_3_LIMESTONECAVE_52_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_3_LIMESTONECAVE_52_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_3_MAGIC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_3_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_4_LIMESTONECAVE_52_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_4_LIMESTONECAVE_52_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_5_ALENA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_5_LIMESTONECAVE_52_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_5_SERIJA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_5_SIUTE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE_52_5_TRIA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMESTONECAVE52_1_BRACKEN42_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMSTONE_52_1_CART(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LIMSTONE_52_1_CRYSTAL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_BOASTER_SQ_50(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_10_MON1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_10_MON2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_10_MON3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_10_MON4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_10_MON5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_10_MON6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_30_MON1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_30_MON2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_30_MON3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_30_MON4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_30_MON5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_50(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_EYEOFBAIGA_SQ_60(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LOWLV_MASTER_ENCY_SQ_40(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LSCAVE551_ALTAR_NPC_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LSCAVE551_SQ_3_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LSCAVE551_SQ_6_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task LSCAVE551_TO_BRACKEN433(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_1_OUT_OF_SECRET(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_1_SQ_10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_1_SQ_40_DOG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_1_TO_CASTLE_20_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_1_TO_MAPLE_25_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_2_SQ_60(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_2_SQ_90(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_2_TO_KATYN_18(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_2_TO_MAPLE_25_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_2_TO_MAPLE_25_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_2_TO_TALBELAND_28_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_SQ_130(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_SQ_50(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_SQ_80_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_SQ_80_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_SQ_90_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_TO_CATACOMB_25_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_TO_MAPLE_23_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_TO_MAPLE_24_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE_25_3_TO_MAPLE_25_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE23_2_TO_MAPLE24_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE23_2_TO_MAPLE25_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE23_2_TO_WHITETREES23_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE23_2_TO_WHITETREES23_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE232_SQ_10_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE232_SQ_10_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE232_SQ_10_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE232_SQ_11(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MAPLE232_SQ_12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MASTER_CRYOMANCER2_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MASTER_FIREMAGE_BOX_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MASTER_HIGHLANDER2_3_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MASTER_PELTASTA2_3_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MIKO_VINE_ORCHARD34_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_1_CRYSTAL_18_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_1_CRYSTAL_9_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_1_ELEVATOR(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_2_CRYSTAL_10_TRIGGER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_2_CRYSTAL_10_TRIGGER2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_2_CRYSTAL_20_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_2_CRYSTAL_3_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_3_BOSS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_3_DEFENCE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_3_RESQUE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task MINE_3_RESQUE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO811_NICO812(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO811_REMAINS37_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO812_NICO811(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO812_NICO813(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO812_SUB10_HIDE_NPC1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO812_SUB10_HIDE_NPC2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO812_SUB10_HIDE_NPC3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO812_SUB5_NPC1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO812_SUBQ12_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO812_SUBQ8_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO813_NICO812(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO813_SUBQ042_HIDE_OBJ1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO813_SUBQ042_HIDE_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO813_SUBQ042_HIDE_OBJ3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO813_SUBQ042_HIDE_OBJ4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO813_SUBQ05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NICO813_SUBQ06(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task NUNNERY_PILGRIM47(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_7_1_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_7_2_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_7_3_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_7_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD_34_1_SQ_2_OBJ_8_DUMMY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD_34_1_TO_PILGRIM_36_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD_34_3_SQ_OBJ_10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD_34_3_SQ_OBJ_6_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD34_1_ORCHARD34_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD34_1_PILGRIM51(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD34_3_CORAL32_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD34_3_ORCHARD34_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD34_3_PILGRIM55(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORCHARD34_3_SIAULIAI_35_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORSHA_HIDDENQ2_IN_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ORSHA_HQ1_CONDITION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PAJAUTA_EP11_SIGN_201(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PARTY_Q_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PARTY_Q_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PARTY_Q06_THORN02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PARTY_Q7_DEVICE02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PAYAUTA_EP11_4_TRG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PAYAUTA_EP11_COMMON_TRG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PAYAUTA_EP11_NPC_TRG1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PAYAUTA_EP11_NPC_TRG2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PAYAUTA_EP11_NPC_TRG3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PCATACOMB1_PILGRIM47(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PCATACOMB1_ROKAS29(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PCATHEDRAL1_REMAINS37(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PCATHEDRAL1_THORN20(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_FIRE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_RUIN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_SHRINE_FK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_SQ_010_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_SQ_020_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_SQ_050_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_SQ_150_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_TO_ORCHARD_34_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_TO_PILGRIM_48(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_TO_ROKAS_36_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_36_2_TO_SIAULIAI_35_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_47_TO_PILGRIM_49(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_48_SQ_050_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_48_SQ_060_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_48_SQ_090_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_48_TO_PILGRIM_36_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_48_TO_PILGRIM_49(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_49_TO_PILGRIM_47(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_49_TO_PILGRIM_48(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_49_TO_PILGRIM_50(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_50_TO_PILGRIM_49(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM_SUCCUBUS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_1_GELE57_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_1_KATYN45_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_1_PILGRIM41_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_2_PILGRIM41_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_2_PILGRIM41_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_2_PILGRIM41_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_2_SQ07_S(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_2_TO_BRACKEN42_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_3_ABBEY41_6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_3_PILGRIM41_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_3_PILGRIM41_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_4_KATYN45_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_4_PILGRIM41_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM41_5_PILGRIM41_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM411_SQ_07_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM411_SQ_08_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM412_SQ_04_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM412_SQ_07_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM412_SQ_08_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM412_SQ_08_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM412_SQ_09_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM413_SQ_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM413_SQ_05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM413_SQ_09(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM414_SQ_12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM415_SQ_02_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM415_SQ_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM415_SQ_06_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM415_SQ_09_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM415_SQ_10_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM46_FEDMIAN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM46_FOOD01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM46_GRASS_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM46_NPC05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM46_PILGRIM47(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_CRY_CORE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_CRYST02_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_CRYST08(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_CURSE01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_CURSE02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_NUNNERY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_PCATACOMB1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_PILGRIM46(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM47_PILGRIM50(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_BIBLE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_BUNT_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_BUNT_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_BUNT_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_BUNT_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_BUNT_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_GHOST3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_PILGRIM47(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM50_PILGRIM51(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM51_INSIGNIA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM51_ORCHARD34_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM51_PILGRIM50(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM51_PILGRIM52(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM52_CATHEDRAL53(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM52_FLASH59(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM52_NPC_TOMB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM52_PILGRIM51(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM52_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM52_TRUTHTREE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM52_TRUTHTREE_REAL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM55_CATHEDRAL54(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM55_CATHEDRAL54_RE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIM55_ORCHARD34_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIMROAD_51_TO_STOWERTOWER_60_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIMROAD_CAULDRON(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIMROAD52_TO_FIRETOWER611(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIMROAD55_ALTAR01_ANIM(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIMROAD55_ALTAR02_ANIM(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIMROAD55_ALTAR03_ANIM(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIMROAD55_ALTAR04_ANIM(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PILGRIMROAD55_SQ11_TOMB(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_78_MQ_7_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_78_MQ_9_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_78_MQ_9_TRIGGER_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_78_OBJ_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_79_MQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_79_MQ_10_TRIGGER_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_79_OBJ_3_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_79_OBJ_5_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_79_OBJ_6_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_79_OBJ_8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_80_MQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_80_MQ_10_TRIGGER_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_80_MQ_7_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_80_OBJ_6_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_81_MQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_81_MQ_10_TRIGGER_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_81_MQ_4_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_81_OBJ_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_81_OBJ_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_81_OBJ_INFOR(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_82_MQ_11_BOOK2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_82_OBJ_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_82_OBJ_3_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON_82_OBJ_7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON78_PRISON79(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON78_TABLELAND74(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON79_PRISON78(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON79_PRISON80(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON80_PRISON79(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON80_PRISON81(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON81_PRISON80(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON81_PRISON82(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRISON82_PRISON81(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRIST_DEAD(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task PRIST_DEAD_BODY_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAIN37_BLOCK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS_38_JOB_SPARRING_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_1_CATACOMB_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_1_MT04_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_1_REMAINS37(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_1_REMAINS37_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_2_REMAINS37_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_2_REMAINS37_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_2_VINE_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_3_CATABOMB_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_3_NICO811(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_3_REMAINS37_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_3_WELL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_3_WELL_ROPE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_PCATHEDRAL1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_REMAINS37_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_REMAINS38(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_ROKAS31(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS37_TO_FIRETOWER691(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS38_REMAINS37(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS38_REMAINS39(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS39_REMAINS38(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS39_REMAINS40(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS39_ROKAS30(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMAINS40_SQ_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMANIS37_2_SSN_TRIGGER_E(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REMANIS37_2_SSN_TRIGGER_L(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REPEAT_MON_GEN_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_01_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_01_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_01_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_02_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_02_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_02_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_03_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_03_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQ_SEMPLE_03_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task REQUEST1_FEDIMIAN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL1_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL1_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL1_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL2_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL2_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL2_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL3_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL3_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RETIARII_ENDURANDE_TRAINING_GOAL3_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_24_BEACON_REAL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_24_BEACON_TRIGGERGO(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_24_JOB_SPARRING_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_24_RELIC_TRIGGERGO(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_26_HQ01_NPC01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_26_HQ01_SESSIONDESTORY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_36_1_PILLA01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_36_1_SQ_040_PILLAR_SUMMON(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_36_1_TO_ABBEY_35_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS_36_1_TO_PILGRIM_36_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_KATYN12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_MQ_2TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_QB_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_QB_11_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_QB_4_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_QB_9(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_ROKAS25(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_THORN23(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS24_TO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_REXIPHER2_MBOSS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_ROKAS24(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_ROKAS26(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_SWITCH1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_SWITCH3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_SWITCH4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_TO_26_START(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_ZACHARIEL32(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS25_ZACHARIEL32_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_NPC_SHNAYIM(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_Q14_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_Q4_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_Q5_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_QS5_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_QUEST01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_QUEST03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_QUEST04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_QUEST05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_ROKAS25(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS26_ROKAS27(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_AIRINE_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_CHAIN01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_CHAIN02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_CHAIN03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_CUBE_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_QB_10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_QB_11(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_QB_13(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_QB_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_QB_7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_QB_9(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_ROKAS26(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS27_ROKAS28(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS28_BLOCK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS28_MQ1_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS28_MQ7_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS28_ODEL_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS28_ROKAS27(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS28_ROKAS29(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS28_TO_UNDERF591(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS28_TRAP_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS29_HALF_SUCCESS1_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS29_INITIATING1_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS29_MQ_REXITHERLOST(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS29_PCATACOMB1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS29_ROKAS28(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS29_ROKAS30(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_ATK_DEFENCE_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_MQ6_1_HOGMA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_ODEL_KIDNAP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_PIPOTI01_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_REMAINS39(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_ROKAS29(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_ROKAS31(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_SEALPRODECT_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS30_SUDDEN_ATTACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS31_REMAINS37(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS31_REXITHER3TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS31_ROKAS30(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ROKAS31_ZACHARIEL32(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RVR_BK_701_REST_BORUTA_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RVR_BK_KAZE_NPC_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task RVR_BK_REST_TO_TABLE111(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SHINOBI_MASTER_UNHIDE_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIALLAIOUT_GELE571(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIALUL_WEST_ONION_BIG_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAU16_SQ_06_EV_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_BUBE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_CAMP4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_RECLAIM1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_RECLAIM3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_RECLAIM7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_REQUEST1_S1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_REQUEST2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_REQUEST3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_EAST_REQUEST6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_ADD_SUB_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_BOSS_GOLEM_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_HANAMING_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_LAIMONAS1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_LAIMONAS2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_LAIMONAS3_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_LAIMONAS3_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_MEET_TITAS_AUTO(TriggerActorArgs args)
		{
			COMMON_QUEST_HANDLER(args);
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_NAGLIS2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL_WEST_ROCK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL47_4_TO_FARM_47_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL47_4_TO_FARM_47_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL47_4_TO_SIAUL50_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL50_1_TO_FARM_47_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL50_1_TO_HUEVILLAGE_58_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL50_1_TO_HUEVILLAGE_58_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAUL50_1_TO_KLAPEDA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIA_2_JOB_SPARRING_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_ABBEY_35_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_CORAL35_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_ORCHARD34_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_SQ_1_START(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_SQ_10_BOSS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_SQ_2_START(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_SQ_8_VINE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_SQ_9_END(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_35_1_TO_PILGRIM_36_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_46_1_SQ_05_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_46_2_MQ_03_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_46_2_SQ_04_01_AFTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_46_3_AUSTEJA_ALTAR_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_46_4_MQ02_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_50_1_GRASS_MESSAGE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI_WEST_OUT_OF_SECRET(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_1_SIAULIAI46_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_1_THORN19(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_1_THORN39_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_2_SIAULIAI46_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_2_SIAULIAI46_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_2_SIAULIAI46_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_2_THORN39_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_3_SIAULIAI46_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_3_SIAULIAI46_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_4_FARM492(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_4_HUEVILLAGE58_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_4_SIAULIAI46_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI46_4_SIAULIAI46_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI462_CANDLE1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI462_CANDLE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI462_CANDLE3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI462_HIDDENQ1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE01_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE02_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE03_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE05_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE06(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE06_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE07(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE07_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_FENCE08(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAI50_PLANT_BIGREPRESS_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_ALCHE_A(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_BOSS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_CART(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_MIRTIS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_PREAL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_Q01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_Q03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_Q06(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_Q11(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_Q12(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SIAULIAIOUT_RETUA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SOUT_Q_16_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task SOUT_SUDD(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_60_1_GUARDIAN_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_60_1_REVERSEGRAVITY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_60_1_STARLAMP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_88_ARROWRAIN_TRAP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_89_ARROWRAIN_TRAP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_89_POISON_TRAP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_90_ARROWRAIN_TRAP_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_90_ARROWRAIN_TRAP_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_90_EVIL_ENERGY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_91_ARROWRAIN_TRAP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER_92_MQ_20_HIDDEN_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER762_EVENT2_OBJ_BEFORE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STARTOWER762_EVENT3_OBJ_BEFORE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task STOUP_CAMP(TriggerActorArgs args)
		{
			// Disable message of unlocking warp.
			//if (!ZoneServer.Instance.Conf.World.FastTravelEnabled) return;

			var npc = args.Npc;
			if (args.Initiator is not Character character)
				return;
			var propertyName = npc.DialogName;

			var sessionObject = character.SessionObjects.GetOrCreate(SessionObjectId.Main);
			if (ZoneServer.Instance.Data.MapDb.TryFind(npc.Map.Id, out var mapData)
				&& !sessionObject.Properties.Has(propertyName))
			{
				sessionObject.Properties[propertyName] = 300;
				Send.ZC_OBJECT_PROPERTY(character, sessionObject, propertyName);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, $"{ScpArgMsg("Auto_KaemPeuKan_iDong_:_")}{mapData.Name}{ScpArgMsg("Auto__HwalSeongHwa")}", 5);
				character.PlaySound("quest_event_click");
			}
			character.ShowHelp("TUTO_CAMPWARP");
		}

		[TriggerFunction]
		public static async Task STOWERTOWER_60_1_TO_PILGRIMROAD_51(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABL71_SUBQ6_TABLE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_BOMB_ECT1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_BOMB_ECT2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_BOMB_ECT3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_BOMB_OBJ1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_BOMB_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_BOMB_OBJ3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_FENCE_OBJ1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_FENCE_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_FENCE_OBJ3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_FENCE_OBJ4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE70_SUBQ2_BOX(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE71_POINT1_MARK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE71_POINT2_MARK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE71_POINT3_MARK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE72_ARTIFACT_MSG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE72_PEAPLE5_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE72_SUBQ7_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE74_SUBQ_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE74_SUBQ_TRIGGER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE74_SUBQ_TRIGGER2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLE74_SUBQ_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_CART_LEVER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_DARK_EFF(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_DARK_WALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_ELEMA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_FAUSTAS_DOWN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_FAUSTAS_DOWN2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_FROST_GUARD(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_KRUVINA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_SETPOS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_SQ_01_BALLOON(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_SQ_02_TRIG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_SQ_04_TRIG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_SQ_07_BACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND_11_1_SQ_07_WRONG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND11_1_FLASH61(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND11_1_TABLELAND28_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND11_1_TABLELAND28_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND11_1_TABLELAND71(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND28_1_TABLELAND11_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND28_2_TABLELAND11_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND281_SQ_02_T(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND281_SQ_05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND281_SQ_08_F(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND281_TO_MAPLE252(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND281_TO_TABLELAND282(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND282_SQ_06_F(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND282_TO_FLASH60(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND282_TO_TABLELAND281(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND70_FLASH64(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND70_TABLELAND71(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND71_TABLELAND11_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND71_TABLELAND70(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND71_TABLELAND72(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND72_TABLELAND71(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND72_TABLELAND73(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND72_UNDERFORTRESS65(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND73_SUBQ4_DEVICE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND73_TABLELAND72(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND73_TABLELAND74(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND74_CASTLE_20_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND74_PRISON78(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TABLELAND74_TABLELAND73(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_BLACKMAN_TRIGGER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_GATEWAY_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_GATEWAY_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_GATEWAY_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_MQ02MAGIC_UNHIDE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_MQ03MAGIC_UNHIDE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_MQ04MAGIC_UNHIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_MQ04MAGIC_UNHIDE2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_MQ06MAGIC_UNHIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN_MQ07MAGIC_UNHIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN19_BLACKMAN_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN19_GATEWAY_2_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN19_HUEVILLAGE58_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN19_MQ14_2_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN19_SIAULIAI46_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN19_THORN20(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN19_THORNBUSH(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN19_TO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN20_MQ02_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN20_MQ03_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN20_MQ07_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN20_PCATHEDRAL1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN20_THORN19(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN20_THORN21(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN20_THORN22(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN20_THORN39_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN21_MQ07_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN21_THORN20(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN21_THORN23(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_1_THORN22_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_2_THORN22_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_2_THORN22_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_3_THORN22_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_ADD_SUB_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_ADD_SUB_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_ADD_SUB_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_HIDDEN_CHAPLAIN_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_KATYN14(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_Q_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_Q_14_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_Q_16_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_Q_18_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_Q_6_BONFIRE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_Q_9_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_THORN20(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN22_THORN23(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_1_THORN23_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_2_THORN23_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_2_THORN23_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_3_THORN23_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_BONFIRE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_BOSS_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_Q_4_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_ROKAS24(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_THORN21(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN23_THORN22(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN39_1_SQ06_OBJ_PRE_CHECK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN39_3_SQ07_OBJ1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN39_3_SQ07_OBJ2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN39_3_SQ07_OBJ3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN391_COMPANION_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN391_COMPANION_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN391_MQ_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN391_MQ_03_C(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN391_MQ_05_W(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN391_TO_THORN20(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN391_TO_THORN392(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN392_COMPANION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN392_MQ_02_COMPANION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN392_MQ_03_C(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN392_MQ_05_H(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN392_SQ_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN392_TO_SIAULIAI461(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN392_TO_THORN391(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN392_TO_THORN393(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN393_COMPANION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN393_TO_ABBEY394(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN393_TO_CATACOMB382(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN393_TO_SIAULIAI462(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THORN393_TO_THORN392(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THREECMLAKE_SKETCH_TRIG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THREECMLAKE261_SQ01_TRACK_TRIG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THREECMLAKE261_SQ07_TRACK_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task THREECMLAKE261_SQ14_TRACK_TRIG(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TUTO_APPRAISE_NPC(TriggerActorArgs args)
		{
			if (args.Initiator is Character character)
				character.ShowHelp("TUTO_ITEM_APPRAISE");
		}

		[TriggerFunction]
		public static async Task TUTO_FREE_DUNGEON_CHECK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task TUTO_JOURNAL_NPC(TriggerActorArgs args)
		{
			if (args.Initiator is Character character)
				character.ShowHelp("TUTO_JOURNAL");
		}

		[TriggerFunction]
		public static async Task TUTO_MARKET_NPC(TriggerActorArgs args)
		{
			if (args.Initiator is Character character)
				character.ShowHelp("TUTO_MARKET");
		}

		[TriggerFunction]
		public static async Task TUTO_REPAIR_NPC(TriggerActorArgs args)
		{
			if (args.Initiator is Character character)
				character.ShowHelp("TUTO_NPC_REPAIR");
		}

		[TriggerFunction]
		public static async Task TUTO_SAALUS_NUNNERY_CHECK(TriggerActorArgs args)
		{
			if (args.Initiator is Character character)
				character.ShowHelp("TUTO_SAALUS_NUNNERY");
		}

		[TriggerFunction]
		public static async Task TUTO_STORAGE_NPC(TriggerActorArgs args)
		{
			if (args.Initiator is Character character)
				character.ShowHelp("TUTO_STORAGE");
		}

		[TriggerFunction]
		public static async Task TUTO_TPSHOP_NPC(TriggerActorArgs args)
		{
			if (args.Initiator is Character character)
				character.ShowHelp("TUTO_TP_SHOP");
		}

		[TriggerFunction]
		public static async Task TUTO_UPHILL_DEFENSE_CHECK(TriggerActorArgs args)
		{
			if (args.Initiator is Character character)
				character.ShowHelp("TUTO_UPHILL_DEFENSE");
		}

		[TriggerFunction]
		public static async Task UNDER_65_SOUND_BOMB01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER_65_SOUND_BOMB02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER30_3_EVENT1_OBJ2_UNHIDE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER30_3_EVENT2_SUB1_CONTROL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER30_3_EVENT2_SUB2_CONTROL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER65_SQ01_SESSION_CREATE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER65_SQ01_SESSION_DESTROY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB06(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB07(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB08(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_BONB09(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER66_MQ7_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER67_HIDDENQ1_AREA1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER67_HIDDENQ1_AREA2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER67_HIDDENQ1_AREA3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER67_HIDDENQ1_AREA4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER67_HIDDENQ1_AREA5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER67_MQ060_DEVICE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER67_MQ6_TO_MEMO(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER68_DEVICE01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER68_MQ7_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER69_HIDDENQ1_IN_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER69_MQ060(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER69_SECRET_ROOM_WARP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDER69_SQ3_GHOST_CALL(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF591_TO_ROKAS28(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF591_TO_UNDERF592(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_TO_UNDERF591(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_TO_UNDERF593(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_TYPEB_GATE_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_TYPEB_GATE_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_TYPEB_GATE_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_TYPEB_GATE_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_TYPEB_GATE_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_TYPEB_GATE_6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF592_ZEMINA_STATUE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERF593_TO_UNDERF592(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS30_1_UNDERFORTRESS30_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS30_1_UNDERFORTRESS67(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS30_2_UNDERFORTRESS30_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS30_2_UNDERFORTRESS30_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS30_3_UNDERFORTRESS30_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS65_FLASH64(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS65_TABLELAND72(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS65_UNDERFORTRESS66(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS66_UNDERFORTRESS65(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS66_UNDERFORTRESS67(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS67_UNDERFORTRESS30_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS67_UNDERFORTRESS66(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS67_UNDERFORTRESS68(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS68_UNDERFORTRESS67(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS68_UNDERFORTRESS69(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task UNDERFORTRESS69_UNDERFORTRESS68(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task VACYS_LIVE_ENTER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task VELNIASPRISON_51_1_JOB_5_1_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task VPRISON511_MQ_01_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task VPRISON511_MQ_05_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task VPRISON513_MQ_01_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task VPRISON514_MQ_01_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task VPRISON514_MQ_05_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task VPRISON515_MQ_06_NPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_C_KLAIPE_CATHEDRAL_MEDIUM(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_CASTLE102_TO_CASTLE99(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_CASTLE102_TO_DCAPITAL53_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_D_DCAPITAL_108_TO_DEVINE_SANCTUARY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_D_DCAPITAL_108_TO_F_CASTLE_101(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL104_TO_DCAPITAL53_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL104_TO_F_DCAPITAL_101(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL105_TO_DCAPITAL103(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL105_TO_DCAPITAL104(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL105_TO_DCAPITAL106(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL106_TO_DCAPITAL105(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL106_TO_DCAPITAL107(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL106_TO_DCAPITAL192(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL107_TO_DCAPITAL106(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL107_TO_DCAPITAL181(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL53_1_TO_CASTLE102(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DCAPITAL53_1_TO_DCAPITAL104(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_DEVINE_SANCTUARY_TO_D_DCAPITAL_108(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_2_D_PRISON_1_TO_EP13_2_D_PRISON_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_2_D_PRISON_2_TO_EP13_2_D_PRISON_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_2_D_PRISON_2_TO_EP13_F_SIAULIAI_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_2_D_PRISON_3_TO_EP13_2_D_PRISON_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_2_D_PRISON_3_TO_EP13_2_D_PRISON_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_1_TO_BRACKEN42_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_1_TO_EP13_F_SIAULIAI_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_1_TO_EP14_1_F_CASTLE_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_1_TO_ORSHA(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_2_TO_EP13_F_SIAULIAI_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_2_TO_EP13_F_SIAULIAI_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_2_TO_EP15_1_FABBEY_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_3_TO_EP13_2_D_PRISON_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_3_TO_EP13_F_SIAULIAI_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_3_TO_EP13_F_SIAULIAI_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_4_TO_EP13_F_SIAULIAI_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_4_TO_EP13_F_SIAULIAI_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_5_TO_EP13_F_SIAULIAI_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP13_F_SIAULIAI_5_TO_EP14_1_F_CASTLE_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_1_TO_EP13_F_SIAULIAI_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_1_TO_EP14_1_F_CASTLE_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_2_TO_EP14_1_F_CASTLE_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_2_TO_EP14_1_F_CASTLE_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_3_TO_EP13_F_SIAULIAI_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_3_TO_EP14_1_F_CASTLE_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_3_TO_EP14_1_F_CASTLE_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_4_TO_EP14_1_F_CASTLE_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_4_TO_EP14_1_F_CASTLE_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_5_TO_EP14_1_F_CASTLE_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_1_F_CASTLE_5_TO_EP14_2_D_CASTLE_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_2_D_CASTLE_1_TO_EP14_1_F_CASTLE_5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_2_D_CASTLE_1_TO_EP14_2_D_CASTLE_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP14_2_D_CASTLE_2_TO_EP14_2_D_CASTLE_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP15_1_FABBEY_1_TO_EP13_F_SIAULIAI_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP15_1_FABBEY_1_TO_EP15_1_FABBEY_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP15_1_FABBEY_2_TO_EP15_1_FABBEY_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP15_1_FABBEY_2_TO_EP15_1_FABBEY_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_EP15_1_FABBEY_3_TO_EP15_1_FABBEY_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_F_DCAPITAL_101_TO_D_DCAPITAL_108(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_F_DCAPITAL_101_TO_DCAPITAL104(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WARP_ORSHA_TO_EP13_F_SIAULIAI_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES_21_1_CASTLE_19_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES_21_1_WHITETREES_21_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES_21_2_WHITETREES_21_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES_21_2_WHITETREES_22_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES_21_2_WHITETREES_23_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES_23_3_TO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_1_WHITETREES21_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_1_WHITETREES22_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_2_EV_55_001(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_2_WHITETREES22_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_2_WHITETREES22_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_2_WHITETREES23_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_3_ABBEY22_4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_3_WHITETREES22_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES22_3_WHITETREES56_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES23_1_TO_KATYN18(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES23_1_TO_MAPLE23_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES23_1_TO_WHITETREES21_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES23_1_TO_WHITETREES22_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES23_3_TO_MAPLE23_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES23_3_TO_MAPLE24_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES23_3_TO_WHITETREES56_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SQ_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SQ_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SQ_03_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SQ_03_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SQ_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SQ_07(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SQ_10(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SQ_11(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SUBQ9_ITEM1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SUBQ9_ITEM2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES231_SUBQ9_ITEM3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES233_SQ_05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES233_SQ_08_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES233_SQ_08_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES233_SQ_08_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES233_SQ_09(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES561_ABBEY225(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES561_SUBQ_HIDDEN1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES561_SUBQ_TRIGGER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES561_SUBQ10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES561_WHITETREES223(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WHITETREES561_WHITETREES233(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WOOD_CARVING_SESSION_CREATE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WOOD_CARVING_SESSION_DESTROY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ACT3_ACT4_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ACT4_1_ACT3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ACT4_1_ACT4_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ACT4_2_ACT4_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ACT4_2_ACT4_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ACT4_3_ACT4_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ACT4_3_ACT4_BOSS(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ACT4_BOSS_ACT4_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_CATACOMB01_1_CATACOMB01_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_CATACOMB01_2_CATACOMB01_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_CMINE8_CMINE9(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_CMINE9_CMINE8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_KATYN14_TO_KATYN13(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_KATYN18_TO_CASTLE19_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_KATYN18_TO_MAPLE25_2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_KATYN18_TO_WHITETREES23_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_OUT_OF_SECRET(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_REMAINS40_TO_FEDMIAN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_REMAINS40_TO_FIRETOWER41(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_REMAINS40_TO_REMAINS39(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ZACHA2F_02_TO_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ZACHA2F_03_TO_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WS_ZACHA2F_04_TO_03(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES_21_1_OBJ_2_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES_21_1_OBJ_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES_21_1_OBJ_7_EFFECT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES_21_2_SQ_10_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES_21_2_SQ_8_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ1_PRE_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ2_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_FLOWER1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_FLOWER2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_FLOWER3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_FLOWER4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_FLOWER5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_FLOWER6(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_FLOWER7(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_FLOWER8(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ6_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_2_SUBQ8_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_3_SUBQ3_SUBNPC1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_3_SUBQ3_SUBNPC2(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_3_SUBQ3_SUBNPC3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_3_SUBQ3_SUBNPC4(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_3_SUBQ3_SUBNPC5(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_3_SUBQ4_NPC1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES22_3_SUBQ5_OBJ1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES221_SUBQ_TREE1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES221_SUBQ_TREE2_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES221_SUBQ_TREE3_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES221_SUBQ_TREE4_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES221_SUBQ_TREE5_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES221_SUBQ6_OBJ1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task WTREES221_SUBQ8_OBJ1_IN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA1F_MQ_03_LANTERN(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA1F_MQ_05(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA2F_MQ02_HIDENPC(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA2F_SQ_02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA2F_SQ_05_TRIGGER(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA33_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA34_ENT(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA35_DESTINY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA35_IDENTIFY(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA35_LETHALBLOW(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA35_SACRIFICE(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA35_TRAP(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA35_TRICK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA3F_MQ_04(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA4F_MQ_01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA4F_MQ_03_HIDE01(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA4F_MQ_03_HIDE02(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA4F_MQ_03_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHA4F_MQ_05_TRACK(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL32_ROKAS25(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL32_ROKAS28(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL32_ROKAS31(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL32_ZACHARIEL33(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL33_3_TO_GUILDMISSION(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL33_ZACHARIEL32(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL33_ZACHARIEL34(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL34_3_ZACHARIEL35_3(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL34_ZACHARIEL33(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL35_1_ZACHARIEL34_1(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL35_ZACHARIEL36(TriggerActorArgs args)
		{
			await Task.Yield();
		}

		[TriggerFunction]
		public static async Task ZACHARIEL36_ZACHARIEL35(TriggerActorArgs args)
		{
			await Task.Yield();
		}

	}
}
