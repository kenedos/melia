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

/// <summary>
/// Dungeon script for 'Lanko Lake Dungeon'
/// 
/// Flow:
/// 1. Start (20s countdown)
/// 2. Combat Phase - Kill 70% of 199 monsters (~140) to open portal (can clear up to 100%)
/// 3. Boss (Varleking)
/// 4. End (10s return timer)
/// 
/// Time Limit: 60 minutes (3600s)
/// </summary>
[DungeonScript("ID_3CMLAKE_26_1")]
public class LankoLakeDungeon : DungeonScript
{
	// Stage ID constants
	private const string STAGE_START = "id_3cmlake_26_1_start";
	private const string STAGE_COMBAT = "id_3cmlake_26_1_combat";
	private const string STAGE_WARP = "id_3cmlake_26_1_warp";
	private const string STAGE_BOSS = "id_3cmlake_26_1_boss";
	private const string STAGE_END = "id_3cmlake_26_1_end";

	// Total monsters in the combat phase (Stage1 + Stage2 + Stage3)
	private const int TOTAL_COMBAT_MONSTERS = 199;

	// Reference to the percentage objective for tracking kills
	private PercentageKillObjective _combatObjective;

	protected override void Load()
	{
		this.SetId("ID_3CMLAKE_26_1");
		this.SetName("Lanko Lake Dungeon");
		this.SetMapName("id_3cmlake_26_1");
		this.SetStartPosition(new Position(-1057, -35.06f, -998));
	}

	/// <summary>Creates the 'Start' stage - countdown before combat begins.</summary>
	private DungeonStage CreateStart()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			instance.Vars.Set("DungeonStartTime", DateTime.UtcNow);

			// Spawn blocking wall
			var wall = script.SpawnNpc(instance, MonsterId.HiddenWall_250_250_300, "",
				new Position(-931.67f, -35.16f, -1166.58f), Direction.SouthWest);

			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Starting in 10 seconds!", 5);

			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Start!", 5);

			// Remove blocking wall
			wall.Map?.RemoveMonster(wall);
		}, null, this, STAGE_START, "Start");

		stage.TransitionTo(STAGE_COMBAT);
		return stage;
	}

	/// <summary>
	/// Creates the combined Combat Phase stage.
	/// Spawns all monsters from original stages 1, 2, and 3.
	/// Requires configured kill percentage to open the portal.
	/// </summary>
	private DungeonStage CreateCombatPhase()
	{
		// Use config value for minimum kill percentage
		var minKillPercentage = KillMonstersStage.DefaultMinimumKillPercentage;

		// Create the percentage-based objective
		_combatObjective = new PercentageKillObjective(TOTAL_COMBAT_MONSTERS, minKillPercentage)
		{
			Text = $"Defeat enemies ({(int)(minKillPercentage * 100)}% required)"
		};

		var stage = new ActionStage(
			async (instance, script) =>
			{
				instance.Vars.Set("StageStartTime", DateTime.UtcNow);

				// ========================================
				// Original STAGE1 monsters (98 total)
				// 47 Pondus, 14 Lakegolem, 37 Anchor_Golem
				// ========================================
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-899.46f, -35.16f, -1120.32f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-703.82f, -35.16f, -1116.89f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-896.41f, -35.16f, -1170.31f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-843.64f, -35.16f, -1121.47f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-745.91f, -35.16f, -1156.90f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-653.36f, -35.16f, -1140.59f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-987.15f, -35.16f, -1304.68f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-989.22f, -35.16f, -1350.97f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-942.64f, -35.16f, -1304.04f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-949.84f, -35.16f, -1347.44f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-843.43f, -35.16f, -1173.48f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-687.22f, -35.16f, -1195.29f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(-770.86f, -35.16f, -1290.86f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(-886.07f, -35.16f, -1255.05f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(-681.39f, -35.16f, -1424.64f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(-579.33f, -35.16f, -1260.85f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(-954.62f, -35.16f, -1421.74f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-685.19f, -35.16f, -1307.31f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-686.12f, -35.16f, -1357.88f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-641.35f, -35.16f, -1352.17f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-636.27f, -35.16f, -1303.39f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-809.72f, -35.16f, -1402.00f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-807.34f, -35.16f, -1448.93f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-771.82f, -35.16f, -1439.82f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-761.30f, -35.16f, -1393.53f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-912.43f, -35.16f, -1326.62f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-867.84f, -35.16f, -1333.50f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-606.40f, -35.16f, -1321.68f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-563.64f, -35.16f, -1330.43f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-985.02f, -35.16f, -1210.53f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-979.87f, -35.16f, -1255.08f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-395.34f, -59.90f, -1230.15f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-333.43f, -61.20f, -1192.85f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-339.04f, -61.21f, -1274.10f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-125.73f, -122.03f, -1383.10f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-135.90f, -122.03f, -1087.29f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(16.79f, -122.03f, -1212.35f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(16.71f, -122.03f, -1305.96f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(93.77f, -122.03f, -1305.66f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(91.91f, -122.03f, -1220.77f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(67.56f, -122.03f, -1031.02f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(65.70f, -122.03f, -1409.24f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(263.51f, -122.03f, -1098.16f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(264.44f, -122.03f, -1370.97f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(260.86f, -122.03f, -1266.12f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(165.38f, -122.03f, -1099.94f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(148.53f, -122.03f, -1391.90f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(174.78f, -122.03f, -1266.16f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(59.04f, -122.03f, -1263.91f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(-36.03f, -122.03f, -1094.69f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(-38.53f, -122.03f, -1376.57f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(181.48f, -122.03f, -1196.33f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(267.63f, -122.03f, -1184.20f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-43.89f, -122.03f, -1297.35f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-35.44f, -122.03f, -1180.49f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-143.86f, -122.03f, -1306.93f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-133.92f, -122.03f, -1148.41f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(72.57f, -122.03f, -1116.14f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-65.38f, -122.03f, -1435.19f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(332.36f, -122.03f, -1349.74f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(328.91f, -122.03f, -1138.63f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(325.21f, -122.03f, -1239.19f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(98.76f, -123.09f, -1566.43f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(194.60f, -123.47f, -1573.82f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(69.88f, -124.06f, -1689.26f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(207.08f, -124.78f, -1697.76f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(141.96f, -125.10f, -1974.24f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(141.27f, -125.10f, -2143.56f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(-4.55f, -125.10f, -1977.34f));
				script.SpawnMonster(instance, MonsterId.ID_Lakegolem, new Position(299.65f, -125.10f, -1955.90f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(32.45f, -125.10f, -1854.83f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(224.13f, -125.10f, -1854.87f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(-34.87f, -125.10f, -1980.85f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(120.55f, -125.10f, -1992.18f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(160.15f, -125.10f, -1987.94f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(221.56f, -125.10f, -1809.52f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(64.39f, -125.10f, -1820.96f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(62.74f, -125.10f, -1854.35f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(326.25f, -125.10f, -1965.39f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(7.18f, -125.10f, -2152.70f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(284.72f, -125.10f, -2156.16f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(43.22f, -125.10f, -2157.67f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(248.00f, -125.10f, -2153.72f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(33.35f, -125.10f, -1820.86f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(249.35f, -125.10f, -1806.05f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(-37.26f, -125.10f, -1939.08f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(325.05f, -125.10f, -1924.29f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(251.13f, -125.10f, -2109.49f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(289.58f, -125.10f, -2114.08f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(37.89f, -125.10f, -2113.73f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(4.74f, -125.10f, -2114.86f));
				script.SpawnMonster(instance, MonsterId.ID_Anchor_Golem, new Position(253.20f, -125.10f, -1851.71f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(181.02f, -125.10f, -1950.02f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(99.63f, -125.10f, -1953.38f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(166.61f, -125.10f, -2171.59f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(123.03f, -125.10f, -2167.17f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(183.41f, -125.10f, -2127.67f));
				script.SpawnMonster(instance, MonsterId.ID_Pondus, new Position(100.96f, -125.10f, -2132.14f));

				// ========================================
				// Original STAGE2 monsters (74 total)
				// 13 VarleHenchman, 28 VarleAnchor, 33 VarleHench
				// ========================================
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1722.77f, -122.62f, -1220.34f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1843.43f, -122.62f, -1045.15f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1837.20f, -122.62f, -1342.28f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1571.79f, -122.62f, -1049.15f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1573.62f, -122.62f, -1324.01f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1726.52f, -122.62f, -1163.63f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1805.10f, -122.62f, -1312.05f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1697.35f, -122.62f, -1370.64f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1686.91f, -122.62f, -1215.67f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1941.58f, -122.62f, -1142.24f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1938.79f, -122.62f, -1210.29f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1821.86f, -122.62f, -1072.28f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1468.48f, -99.38f, -1233.02f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1474.53f, -99.38f, -1097.08f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1903.49f, -122.62f, -1183.94f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1580.61f, -122.62f, -1016.25f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1536.59f, -122.62f, -1314.54f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1589.30f, -122.62f, -1284.50f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1579.24f, -122.62f, -1087.76f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1803.75f, -122.62f, -1372.55f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1987.26f, -122.62f, -1309.84f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1876.08f, -122.62f, -1151.89f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1874.79f, -122.62f, -1212.69f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1986.31f, -122.62f, -1279.94f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1980.95f, -122.62f, -1080.19f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1981.19f, -122.62f, -1049.66f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1876.90f, -122.62f, -1080.95f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1857.40f, -122.62f, -1310.42f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1816.44f, -122.62f, -1008.01f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1585.66f, -122.62f, -1348.22f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1531.74f, -122.62f, -1051.23f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1364.41f, -99.38f, -1230.30f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1361.04f, -99.38f, -1104.62f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1410.07f, -99.38f, -1190.03f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1413.05f, -99.38f, -1144.30f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(894.87f, -96.08f, -1043.82f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(985.69f, -96.08f, -1181.28f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1081.55f, -96.08f, -1054.74f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1087.57f, -96.08f, -1336.82f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(922.63f, -96.08f, -1341.51f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(1152.67f, -96.08f, -1198.27f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHenchman, new Position(818.24f, -96.08f, -1192.33f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(791.62f, -96.08f, -1231.30f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(796.84f, -96.08f, -1143.61f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(628.32f, -87.60f, -1254.05f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(633.39f, -87.60f, -1119.35f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(869.02f, -96.08f, -1232.62f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(875.06f, -96.08f, -1156.60f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(851.83f, -96.08f, -1016.62f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(898.01f, -96.08f, -1375.18f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1045.63f, -96.08f, -1063.48f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1090.19f, -96.08f, -1083.60f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1124.38f, -96.08f, -1222.97f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1117.94f, -96.08f, -1296.32f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1057.94f, -96.08f, -1303.10f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1059.66f, -96.08f, -1357.48f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1064.65f, -96.08f, -1007.09f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1190.10f, -96.08f, -1219.59f));
				script.SpawnMonster(instance, MonsterId.ID_VarleAnchor, new Position(1193.76f, -96.08f, -1157.97f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1117.06f, -96.08f, -1369.87f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1277.31f, -96.86f, -1259.90f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1121.65f, -96.08f, -1159.77f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1273.39f, -97.03f, -1114.82f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1123.80f, -96.08f, -1044.55f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(920.18f, -96.08f, -1009.41f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(858.21f, -96.08f, -1085.21f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(928.42f, -96.08f, -1083.03f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(892.17f, -96.08f, -1314.53f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(955.63f, -96.08f, -1310.83f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(952.04f, -96.08f, -1382.01f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(967.22f, -96.08f, -1143.05f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(969.65f, -96.08f, -1215.27f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1024.45f, -96.08f, -1217.43f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHench, new Position(1016.36f, -96.08f, -1151.21f));

				// ========================================
				// Original STAGE3 monsters (27 total)
				// 9 VarleGunner, 5 VarleSkipper, 13 VarleHelmsman
				// ========================================
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(824.18f, -112.51f, -443.85f));
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(1056.78f, -112.41f, -448.57f));
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(907.02f, -112.52f, -391.94f));
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(1005.35f, -112.51f, -386.38f));
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(946.17f, -112.22f, -496.98f));
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(1096.92f, -112.57f, -329.45f));
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(779.07f, -112.57f, -330.03f));
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(825.61f, -112.57f, -208.19f));
				script.SpawnMonster(instance, MonsterId.ID_VarleGunner, new Position(1061.63f, -112.57f, -202.76f));
				script.SpawnMonster(instance, MonsterId.ID_VarleSkipper, new Position(962.59f, -112.57f, -281.82f));
				script.SpawnMonster(instance, MonsterId.ID_VarleSkipper, new Position(929.49f, -112.13f, -537.70f));
				script.SpawnMonster(instance, MonsterId.ID_VarleSkipper, new Position(970.72f, -112.18f, -540.62f));
				script.SpawnMonster(instance, MonsterId.ID_VarleSkipper, new Position(829.64f, -112.49f, -479.65f));
				script.SpawnMonster(instance, MonsterId.ID_VarleSkipper, new Position(1053.22f, -112.34f, -494.12f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(854.87f, -112.50f, -439.96f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(1027.89f, -112.44f, -444.56f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(1082.33f, -112.47f, -448.11f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(795.89f, -112.53f, -447.90f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(900.20f, -111.96f, -612.42f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(996.17f, -111.96f, -610.38f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(810.81f, -112.55f, -358.79f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(1077.59f, -112.54f, -354.07f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(1122.56f, -112.56f, -355.88f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(768.37f, -112.56f, -351.69f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(960.47f, -112.55f, -340.20f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(821.99f, -112.57f, -263.61f));
				script.SpawnMonster(instance, MonsterId.ID_VarleHelmsman, new Position(1059.77f, -112.57f, -260.50f));

				await Task.CompletedTask;
			},
			new List<QuestObjective> { _combatObjective },
			TOTAL_COMBAT_MONSTERS,
			this,
			STAGE_COMBAT,
			"Combat Phase"
		);

		stage.OnSuccess(STAGE_WARP);
		return stage;
	}

	private DungeonStage CreateWarp()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			// Show portal open message
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "The path to the boss has opened!", 5);

			// Spawn warp portal to boss area
			var warp = script.SpawnNpcWithProperties(instance, MonsterId.Hiddennpc,
				new Position(934.41f, -112.57f, -153.57f), "Dialog#NPC_3CMLAKE_WARP");
			warp.AttachEffect("F_circle25", 2.3f, EffectLocation.Middle);
		}, null, this, STAGE_WARP, "Portal Open");

		stage.TransitionTo(STAGE_BOSS);
		return stage;
	}

	/// <summary>Creates the Boss stage (Varleking).</summary>
	private DungeonStage CreateBoss()
	{
		var stage = BossStage.Create(this, STAGE_BOSS, MonsterId.ID_Boss_Varleking,
			new Position(955.78f, -79.77f, 1480.10f),
			successStageId: STAGE_END,
			failStageId: StageId.Fail,
			allowEscape: false,
			supports: null,
			message: "Boss");

		stage.OnSuccess(STAGE_END, instance =>
		{
			this.MGameMessage(instance, "NOTICE_Dm_scroll", "Returning to entrance in 10 seconds.", 5);
		});

		return stage;
	}

	/// <summary>Creates the End stage - 10 second timer then return to entrance.</summary>
	private DungeonStage CreateEnd()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			instance.Vars.Set("DungeonEnded", true);

			await Task.Delay(TimeSpan.FromSeconds(10));

			// Return players to entrance
			this.MGameReturn();
			this.DungeonEnded(instance, false);
		}, null, this, STAGE_END, "End");

		stage.TransitionTo(StageId.Complete);
		return stage;
	}

	protected override List<DungeonStage> GetDungeonStages()
	{
		return new List<DungeonStage>
		{
			this.CreateStart(),
			this.CreateCombatPhase(),
			this.CreateWarp(),
			this.CreateBoss(),
			this.CreateEnd(),
		};
	}

	private void OnStageFailed(InstanceDungeon instance, string reason)
	{
		this.MGameMessage(instance, "NOTICE_Dm_scroll", reason, 5);
		this.DungeonEnded(instance, true);
	}
}
