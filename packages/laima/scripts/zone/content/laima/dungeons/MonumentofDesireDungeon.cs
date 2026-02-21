using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;

/// <summary>
/// Dungeon script for 'Monument of Desire Dungeon'
/// </summary>
[DungeonScript("Indun_Remains3")]
public class MonumentofDesireDungeon : DungeonScript
{
	// Stage ID constants for branching
	private const string STAGE_START = "indun_remains3_start";
	private const string STAGE_STAGE1 = "indun_remains3_stage1";
	private const string STAGE_STAGE2 = "indun_remains3_stage2";
	private const string STAGE_STAGE3 = "indun_remains3_stage3";
	private const string STAGE_BOSS = "indun_remains3_boss";
	private const string STAGE_LASTBOSS = "indun_remains3_lastboss";
	private const string STAGE_END = "indun_remains3_end";
	private const string STAGE_DOOR = "indun_remains3_door";
	private const string STAGE_TIME = "indun_remains3_time";
	private const string STAGE_PERCENTUI = "indun_remains3_percentui";

	protected override void Load()
	{
		this.SetId("Indun_Remains3");
		this.SetName("Monument of Desire Dungeon");
		this.SetMapName("id_remains3");
		this.SetStartPosition(new Position(-1333f, 989f, -2903f));
	}

	/// <summary>Creates the 'START' stage.</summary>
	private DungeonStage CreateSTART()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			// Spawn stage-specific objects
			var npc0 = script.SpawnNpc(instance, MonsterId.HiddenWall_250_250_300, "", new Position(-1570.03f, 989.25f, -2650.79f), Direction.East);
			stageObjects["npc0"] = npc0;
			var npc1 = script.SpawnNpc(instance, MonsterId.HiddenWall_250_250_300, "", new Position(-1495.30f, 989.25f, -2617.76f), Direction.East);
			stageObjects["npc1"] = npc1;
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Starting in 10 seconds!", 5);
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Start!", 5);
			instance.Vars.SetInt("Door_check", 1);
		}, null, this, "indun_remains3_start", "START");
		stage.TransitionTo(STAGE_STAGE1);
		return stage;
	}

	/// <summary>Creates the 'STAGE1' stage.</summary>
	private DungeonStage CreateSTAGE1()
	{
		var stage = new ActionStage(
			async (instance, script) =>
			{
				instance.Vars.Set("StageStartTime", DateTime.UtcNow);
				// Spawn stage-specific NPCs and objects
				//script.SpawnNpcWithProperties(instance, MonsterId.Board3, new Position(884.60f, 1056.92f, -2235.10f), "Range#100#Dialog#ID_REMAINS3");

				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-1300.26f, 989.25f, -2174.02f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-1253.39f, 989.25f, -2209.65f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-1262.74f, 989.25f, -2143.61f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-1226.55f, 989.25f, -2179.49f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-1038.41f, 989.25f, -2276.59f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-1098.83f, 989.25f, -2071.40f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-1016.65f, 989.25f, -2237.88f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-1039.70f, 989.25f, -2198.88f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-1040.92f, 989.25f, -2043.15f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-914.79f, 989.25f, -2369.22f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-885.17f, 989.25f, -2421.28f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-919.55f, 989.25f, -2410.09f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-845.54f, 989.25f, -2244.63f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-727.59f, 989.25f, -2269.35f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-775.37f, 989.25f, -2209.05f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-791.25f, 989.25f, -2253.04f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-782.00f, 989.25f, -2091.36f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-791.78f, 989.25f, -1980.69f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-737.88f, 989.25f, -2041.01f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-786.36f, 989.25f, -2043.29f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-701.25f, 990.37f, -2472.85f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-643.38f, 990.41f, -2430.25f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-496.43f, 989.55f, -2283.48f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-585.17f, 989.25f, -2242.20f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-683.03f, 989.68f, -2423.29f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-434.93f, 989.25f, -2150.13f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-382.44f, 989.25f, -2210.69f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-388.12f, 989.25f, -2109.96f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(-340.86f, 989.25f, -2169.69f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-388.54f, 989.25f, -2164.99f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(-58.51f, 1022.73f, -2291.06f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-144.67f, 989.25f, -2264.49f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(-140.46f, 992.91f, -2217.77f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(-76.52f, 1018.96f, -2222.14f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(148.29f, 1077.40f, -2253.99f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(172.84f, 1077.40f, -2302.70f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(172.93f, 1077.40f, -2212.33f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(559.24f, 1113.98f, -2276.16f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(487.01f, 1112.60f, -2206.27f));
				script.SpawnMonster(instance, MonsterId.ID_PagSawyer, new Position(559.74f, 1113.98f, -2233.95f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(507.81f, 1113.98f, -2241.59f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(342.84f, 1090.03f, -2268.77f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(348.64f, 1091.25f, -2218.95f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(-534.72f, 989.25f, -2211.89f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(-922.50f, 989.25f, -2453.00f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(-565.96f, 989.25f, -2316.88f));
				script.SpawnMonster(instance, MonsterId.ID_Pagshearer, new Position(474.54f, 1032.58f, -1869.59f));
				script.SpawnMonster(instance, MonsterId.ID_Pagshearer, new Position(521.38f, 1029.85f, -1864.31f));
				script.SpawnMonster(instance, MonsterId.ID_Pagshearer, new Position(498.51f, 1028.08f, -1459.32f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(498.70f, 1027.99f, -1827.93f));
				script.SpawnMonster(instance, MonsterId.ID_PagDoper, new Position(513.34f, 1021.04f, -1709.09f));
				script.SpawnMonster(instance, MonsterId.ID_Pagshearer, new Position(470.63f, 1017.81f, -1676.78f));
				script.SpawnMonster(instance, MonsterId.ID_Pagshearer, new Position(534.87f, 1014.68f, -1501.26f));
				script.SpawnMonster(instance, MonsterId.ID_Pagshearer, new Position(548.83f, 1017.70f, -1678.95f));
				script.SpawnMonster(instance, MonsterId.ID_Pagshearer, new Position(467.89f, 1017.94f, -1493.56f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(468.33f, 1061.05f, -1990.69f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(526.35f, 1062.18f, -1994.08f));
				script.SpawnMonster(instance, MonsterId.ID_Charog, new Position(499.72f, 1071.33f, -2025.21f));
				await Task.CompletedTask;
			}, new List<QuestObjective> {
				new KillObjective(16, MonsterId.ID_PagSawyer),
				new KillObjective(25, MonsterId.ID_Charog),
				new KillObjective(10, MonsterId.ID_PagDoper),
				new KillObjective(7, MonsterId.ID_Pagshearer),
			}, this, "indun_remains3_stage1", "STAGE1");
		stage.OnSuccess(STAGE_STAGE2);
		return stage;
	}

	/// <summary>Creates the 'STAGE2' stage.</summary>
	private DungeonStage CreateSTAGE2()
	{
		var stage = new ActionStage(
			async (instance, script) =>
			{
				instance.Vars.Set("StageStartTime", DateTime.UtcNow);
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(480.16f, 1035.67f, -1277.34f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(749.41f, 1035.66f, -1330.87f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(872.48f, 1035.66f, -1151.40f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(832.67f, 1035.66f, -1189.34f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(815.34f, 1035.66f, -1115.37f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(400.85f, 1035.67f, -1066.97f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(776.70f, 1035.66f, -910.98f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(851.19f, 1035.66f, -908.52f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(812.59f, 1035.66f, -857.86f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(455.04f, 1035.67f, -809.81f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(514.38f, 1035.67f, -864.90f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(565.60f, 1035.66f, -818.28f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(516.99f, 1074.88f, -687.02f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(531.37f, 1089.87f, -649.12f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(472.67f, 1096.46f, -648.49f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(493.61f, 1123.16f, -421.10f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(550.94f, 1123.16f, -387.16f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(506.09f, 1123.16f, -443.64f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(492.62f, 1123.98f, -108.92f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(566.73f, 1123.98f, -115.96f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(536.44f, 1123.98f, -80.14f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(457.71f, 1123.07f, 102.93f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(555.68f, 1122.93f, 105.50f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(513.38f, 1121.98f, 142.03f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(511.21f, 1123.43f, 100.22f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(488.06f, 1165.17f, 329.15f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(607.79f, 1173.39f, 329.41f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(544.58f, 1173.63f, 344.68f));
				script.SpawnMonster(instance, MonsterId.ID_Flak, new Position(427.09f, 1035.67f, -1301.65f));
				script.SpawnMonster(instance, MonsterId.ID_Flak, new Position(542.69f, 1035.66f, -1320.88f));
				script.SpawnMonster(instance, MonsterId.ID_Flak, new Position(399.37f, 1035.67f, -1136.95f));
				script.SpawnMonster(instance, MonsterId.ID_Flak, new Position(691.71f, 1035.66f, -1324.71f));
				script.SpawnMonster(instance, MonsterId.ID_Flak, new Position(719.42f, 1035.66f, -1295.73f));
				script.SpawnMonster(instance, MonsterId.ID_Flak, new Position(474.66f, 1027.86f, -1115.71f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(512.93f, 1035.67f, -814.03f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(622.92f, 1021.37f, -1045.97f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(699.20f, 1028.83f, -1050.72f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Archer, new Position(655.80f, 1035.66f, -991.16f));
				await Task.CompletedTask;
			}, new List<QuestObjective> {
				new KillObjective(12, MonsterId.ID_Flamme_Priest),
				new KillObjective(18, MonsterId.ID_Flamme_Archer),
				new KillObjective(2, MonsterId.ID_Flamme_Mage),
				new KillObjective(6, MonsterId.ID_Flak),
			}, this, "indun_remains3_stage2", "STAGE2");
		stage.OnSuccess(STAGE_STAGE3);
		return stage;
	}

	/// <summary>Creates the 'STAGE3' stage.</summary>
	private DungeonStage CreateSTAGE3()
	{
		var stage = new ActionStage(
			async (instance, script) =>
			{
				instance.Vars.Set("StageStartTime", DateTime.UtcNow);
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(448.64f, 1177.81f, 405.32f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(522.05f, 1177.89f, 425.49f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(676.31f, 1178.06f, 535.03f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(614.91f, 1178.06f, 559.59f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(610.23f, 1178.06f, 393.29f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(661.10f, 1179.91f, 340.66f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(433.05f, 1178.06f, 574.99f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(487.82f, 1178.06f, 568.70f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(467.84f, 1178.06f, 629.42f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(799.44f, 1178.06f, 593.12f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(619.44f, 1178.06f, 713.81f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(741.53f, 1178.10f, 453.54f));
				script.SpawnMonster(instance, MonsterId.ID_Paggnat, new Position(784.47f, 1178.06f, 426.12f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(787.35f, 1178.06f, 470.99f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(805.75f, 1178.06f, 634.25f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(603.34f, 1178.06f, 762.20f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(408.76f, 1178.06f, 843.16f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(847.87f, 1178.06f, 576.07f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(669.19f, 1178.06f, 706.81f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(328.26f, 1178.06f, 860.29f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(456.31f, 1178.06f, 853.36f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(412.37f, 1178.06f, 786.47f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(642.37f, 1178.06f, 509.51f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(489.71f, 1177.76f, 384.87f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(623.34f, 1178.81f, 341.89f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(1032.61f, 1101.43f, 512.12f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(1038.79f, 1104.87f, 566.35f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(988.55f, 1124.48f, 533.65f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(91.69f, 1342.81f, 1511.92f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(33.16f, 1356.53f, 1802.29f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(291.26f, 1370.25f, 1583.34f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(504.82f, 1377.32f, 1598.83f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(333.86f, 1416.87f, 1821.85f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(346.32f, 1237.12f, 1062.71f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(393.39f, 1232.99f, 1064.22f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(314.11f, 1298.27f, 1321.67f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(261.75f, 1306.47f, 1365.89f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(237.06f, 1301.00f, 1298.67f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(97.18f, 1346.48f, 1567.12f));
				script.SpawnMonster(instance, MonsterId.ID_Thornball, new Position(142.75f, 1342.89f, 1548.65f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(42.83f, 1354.93f, 1846.55f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(87.44f, 1358.86f, 1787.63f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(297.95f, 1370.45f, 1546.47f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(328.94f, 1370.40f, 1577.71f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(443.52f, 1384.64f, 1612.96f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(493.58f, 1386.77f, 1657.17f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(285.42f, 1420.01f, 1842.10f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Priest, new Position(345.07f, 1419.30f, 1860.37f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(333.25f, 1413.59f, 1775.44f));
				script.SpawnMonster(instance, MonsterId.ID_Flamme_Mage, new Position(389.33f, 1413.18f, 1805.28f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(255.99f, 1178.06f, 672.67f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(261.43f, 1178.06f, 764.52f));
				script.SpawnMonster(instance, MonsterId.ID_Spector_Gh_Black, new Position(305.87f, 1178.06f, 713.26f));
				script.SpawnBoss(instance, MonsterId.ID_Boss_Grinender, new Position(1483.46f, 1094.28f, 558.66f));
				await Task.CompletedTask;
			}, new List<QuestObjective> {
				new KillObjective(12, MonsterId.ID_Paggnat),
				new KillObjective(16, MonsterId.ID_Spector_Gh_Black),
				new KillObjective(13, MonsterId.ID_Thornball),
				new KillObjective(9, MonsterId.ID_Flamme_Mage),
				new KillObjective(3, MonsterId.ID_Flamme_Priest),
			}, this, "indun_remains3_stage3", "STAGE3");
		stage.OnSuccess(STAGE_LASTBOSS);
		return stage;
	}

	/// <summary>Creates the 'LASTBOSS' stage.</summary>
	private DungeonStage CreateLASTBOSS()
	{
		var stage = BossStage.Create(this, "indun_remains3_lastboss", MonsterId.ID_Boss_Flammidus, new Position(567.22f, 1428.59f, 3151.70f), successStageId: "TBD", failStageId: StageId.Fail,
			allowEscape: false, supports: null, message: "LASTBOSS");
		stage.OnSuccess(STAGE_END, instance =>
		{
			this.MGameMessage(instance, "NOTICE_Dm_scroll", "Stage complete!", 3);
		});
		return stage;
	}

	/// <summary>Creates the 'BOSS' stage.</summary>
	private DungeonStage CreateBOSS()
	{
		var stage = BossStage.Create(this, "indun_remains3_boss", MonsterId.ID_Boss_Grinender, new Position(1483.46f, 1094.28f, 558.66f), successStageId: "TBD", failStageId: StageId.Fail,
			allowEscape: false, supports: null, message: "BOSS");
		return stage;
	}

	/// <summary>Creates the 'END' stage.</summary>
	private DungeonStage CreateEND()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(10));
		}, null, this, "indun_remains3_end", "Returning to the entrance in 10 seconds.");
		//stage.TransitionTo(STAGE_DOOR);
		return stage;
	}

	/// <summary>Creates the 'DOOR' stage.</summary>
	private DungeonStage CreateDOOR()
	{
		var stage = new ActionStage(
			async (instance, script) =>
			{
				instance.Vars.Set("StageStartTime", DateTime.UtcNow);
				// Spawn stage-specific NPCs and objects
				var npc1 = script.SpawnMonsterWithProperties(instance, MonsterId.Thorn_Gateway_6_2, new Position(378.00f, -0.99f, -116.00f), "OBB#YES#OBBSize#100;100;200#Name#관문");
				var npc2 = script.SpawnNpc(instance, MonsterId.HiddenWall_100_200_500, "", new Position(359.00f, -0.99f, -214.00f), Direction.South);
				await Task.CompletedTask;
			}, new List<QuestObjective>
			{
			}, this, "indun_remains3_door", "DOOR");
		stage.OnSuccess(STAGE_TIME);
		return stage;
	}

	/// <summary>Creates the 'Time' stage.</summary>
	private DungeonStage CreateTime()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(3300));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "5 minutes are left until the dungeon ends!!", 5);
			await Task.Delay(TimeSpan.FromSeconds(300));
			await Task.Delay(TimeSpan.FromSeconds(10));
		}, null, this, "indun_remains3_time", "Time");
		stage.TransitionTo(STAGE_PERCENTUI);
		return stage;
	}

	/// <summary>Creates the 'PercentUI' stage.</summary>
	private DungeonStage CreatePercentUI()
	{
		var stage = new InitialSetupStage(instance => { }, this, "indun_remains3_percentui");
		stage.TransitionTo(StageId.Complete);
		return stage;
	}

	protected override List<DungeonStage> GetDungeonStages()
	{
		var stages = new List<DungeonStage>
		{
			this.CreateSTART(),
			this.CreateSTAGE1(),
			this.CreateSTAGE2(),
			this.CreateSTAGE3(),
			this.CreateLASTBOSS(),
			//CreateBOSS(),
			//CreateEND(),
			//CreateDOOR(),
			//CreateTime(),
			//CreatePercentUI(),
		};

		return stages;
	}

	private void OnStageFailed(InstanceDungeon instance, string reason)
	{
		this.MGameMessage(instance, "NOTICE_Dm_scroll", reason, 5);
		this.DungeonEnded(instance, true);
	}

}
