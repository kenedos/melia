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
/// Dungeon script for 'Fallen Legwyn Family Dungeon'
/// </summary>
[DungeonScript("ID_STARTOWER_MINI")]
public class FallenLegwynFamilyDungeon2018 : DungeonScript
{
	// Stage ID constants for branching
	private const string STAGE_DEFGROUP = "id_startower_mini_defgroup";
	private const string STAGE_STAGE2 = "id_startower_mini_stage2";
	private const string STAGE_STAGE3 = "id_startower_mini_stage3";
	private const string STAGE_END = "id_startower_mini_end";
	private const string STAGE_BOSS1 = "id_startower_mini_boss1";
	private const string STAGE_LASTBOSS = "id_startower_mini_lastboss";
	private const string STAGE_READY = "id_startower_mini_ready";

	protected override void Load()
	{
		this.SetId("ID_STARTOWER_MINI");
		this.SetName("Fallen Legwyn Family Dungeon");
		this.SetMapName("ID_startower");
		this.SetStartPosition(new Position(x: 155f, y: -106f, z: -2387f));
	}

	/// <summary>Creates the 'ready' stage.</summary>
	private DungeonStage CreateReady()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(10));
			// Event: 10 at 10s
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Starting in 10 seconds", 5);
			await Task.Delay(TimeSpan.FromSeconds(10));
			// Event: start at 20s
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Start!", 5);

		}, null, this, "id_startower_mini_ready", "ready");

		stage.TransitionTo(STAGE_DEFGROUP);
		return stage;
	}

	/// <summary>Creates the 'DefGroup' stage.</summary>
	private DungeonStage CreateDefGroup()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Pawnd_Purple, new Dictionary<Position, int>
				{
					{ new Position(1247.18f, -106.16f, -1453.02f), 1 },
					{ new Position(1304.80f, -106.16f, -1392.84f), 1 },
					{ new Position(1262.35f, -106.16f, -1022.81f), 1 },
					{ new Position(1334.33f, -106.16f, -1080.97f), 1 },
					{ new Position(1544.07f, -106.16f, -1273.65f), 1 },
					{ new Position(1713.02f, -106.16f, -261.47f), 1 },
					{ new Position(1332.09f, -106.16f, -436.86f), 1 },
					{ new Position(1295.11f, -106.16f, -190.14f), 1 },
					{ new Position(1517.54f, -106.16f, -207.32f), 1 },
					{ new Position(1835.70f, -106.16f, 305.17f), 1 },
					{ new Position(1832.52f, -106.16f, 476.09f), 1 },
					{ new Position(1833.77f, -106.16f, 398.19f), 1 },
					{ new Position(1619.80f, -106.16f, 306.35f), 1 },
					{ new Position(1617.77f, -106.16f, 399.25f), 1 },
					{ new Position(1616.97f, -106.16f, 470.07f), 1 },
					{ new Position(2037.64f, -106.16f, 231.39f), 1 },
					{ new Position(2118.30f, -106.16f, 232.59f), 1 },
					{ new Position(2125.80f, -106.16f, 303.78f), 1 },
					{ new Position(2125.57f, -106.16f, 394.71f), 1 },
					{ new Position(2135.85f, -106.16f, 477.71f), 1 },
					{ new Position(2022.61f, -106.16f, 474.40f), 1 },
					{ new Position(1432.80f, -106.16f, 230.65f), 1 },
					{ new Position(1332.04f, -106.16f, 229.75f), 1 },
					{ new Position(1330.99f, -106.16f, 309.73f), 1 },
					{ new Position(1330.93f, -106.16f, 406.00f), 1 },
					{ new Position(1329.58f, -106.16f, 478.11f), 1 },
					{ new Position(1430.00f, -106.16f, 469.62f), 1 },
					{ new Position(1867.48f, -106.95f, -482.55f), 1 },
					{ new Position(1663.51f, -106.16f, -427.32f), 1 },
					{ new Position(1916.85f, -108.25f, -452.19f), 1 },
					{ new Position(1702.60f, -106.16f, -1057.94f), 1 },
					{ new Position(1636.77f, -106.16f, -1113.23f), 1 },
					{ new Position(1400.92f, -106.16f, -1203.24f), 1 },
					{ new Position(1619.05f, -106.16f, 231.50f), 1 },
					{ new Position(1836.70f, -106.16f, 232.12f), 1 },
					{ new Position(1607.98f, -106.16f, -1375.62f), 1 },
					{ new Position(1666.72f, -106.16f, -1438.69f), 1 },
				}
			},
			{ MonsterId.ID_Pawndel_Blue, new Dictionary<Position, int>
				{
					{ new Position(1168.98f, -106.16f, -1174.97f), 1 },
					{ new Position(1167.43f, -106.16f, -1305.59f), 1 },
					{ new Position(1418.00f, -106.16f, -922.00f), 1 },
					{ new Position(1528.70f, -106.16f, -918.11f), 1 },
					{ new Position(1464.72f, -106.16f, -1303.35f), 1 },
					{ new Position(1862.39f, -106.16f, -274.13f), 1 },
					{ new Position(1472.98f, -106.16f, -434.00f), 1 },
					{ new Position(1447.01f, -106.16f, -145.18f), 1 },
					{ new Position(1367.17f, -106.16f, -132.30f), 1 },
					{ new Position(1674.56f, -106.16f, -62.85f), 1 },
					{ new Position(1782.27f, -106.16f, -58.63f), 1 },
					{ new Position(1648.84f, -106.16f, -259.72f), 1 },
					{ new Position(1925.59f, -106.16f, -272.35f), 1 },
					{ new Position(1686.32f, -106.16f, 513.32f), 1 },
					{ new Position(1776.56f, -106.16f, 517.07f), 1 },
					{ new Position(530.89f, -106.16f, -1238.78f), 1 },
					{ new Position(423.26f, -106.16f, -1230.20f), 1 },
					{ new Position(1705.57f, -106.16f, -480.77f), 1 },
					{ new Position(1459.43f, -106.16f, -1140.72f), 1 },
					{ new Position(1541.73f, -106.16f, -1174.96f), 1 },
					{ new Position(1706.87f, -106.16f, -1258.93f), 1 },
					{ new Position(1697.47f, -106.16f, -1190.90f), 1 },
					{ new Position(1231.18f, -106.16f, -1244.85f), 1 },
					{ new Position(848.85f, -106.16f, -1320.96f), 1 },
					{ new Position(473.89f, -106.16f, -1304.53f), 1 },
					{ new Position(474.77f, -106.16f, -1188.23f), 1 },
					{ new Position(1415.49f, -106.16f, -1534.42f), 1 },
					{ new Position(1522.56f, -106.16f, -1531.88f), 1 },
					{ new Position(853.95f, -106.16f, -1212.60f), 1 },
					{ new Position(859.45f, -106.16f, -1168.46f), 1 },
					{ new Position(849.17f, -106.16f, -1264.21f), 1 },
				}
			},
			{ MonsterId.ID_Glizardon_Instance, new Dictionary<Position, int>
				{
					{ new Position(1472.02f, -106.16f, -920.26f), 1 },
					{ new Position(1150.97f, -106.16f, -1229.27f), 1 },
					{ new Position(1415.00f, -106.16f, -193.05f), 1 },
					{ new Position(1484.52f, -106.16f, 349.00f), 1 },
					{ new Position(1992.40f, -106.16f, 351.47f), 1 },
					{ new Position(482.82f, -106.16f, -1245.74f), 1 },
					{ new Position(1419.56f, -106.16f, -401.89f), 1 },
					{ new Position(1468.51f, -106.16f, -1229.60f), 1 },
					{ new Position(1760.82f, -106.16f, -1226.63f), 1 },
					{ new Position(1791.75f, -106.16f, -369.81f), 1 },
					{ new Position(1472.85f, -106.16f, -1539.72f), 1 },
				}
			},
			{ MonsterId.ID_Malstatue, new Dictionary<Position, int>
				{
					{ new Position(1422.96f, -106.16f, -868.62f), 1 },
					{ new Position(1419.62f, -106.16f, -752.43f), 1 },
					{ new Position(1423.45f, -106.16f, -621.65f), 1 },
					{ new Position(1535.14f, -106.16f, -619.29f), 1 },
					{ new Position(1526.69f, -106.16f, -747.52f), 1 },
					{ new Position(1520.46f, -106.16f, -870.70f), 1 },
					{ new Position(1684.59f, -106.16f, 72.49f), 1 },
					{ new Position(1786.15f, -106.16f, 81.97f), 1 },
					{ new Position(1683.86f, -106.16f, -20.32f), 1 },
					{ new Position(1781.75f, -106.16f, -17.97f), 1 },
					{ new Position(1684.22f, -106.16f, 658.18f), 1 },
					{ new Position(1691.38f, -106.16f, 703.01f), 1 },
					{ new Position(1690.54f, -106.16f, 750.18f), 1 },
					{ new Position(1692.72f, -106.16f, 798.07f), 1 },
					{ new Position(1772.72f, -106.16f, 652.03f), 1 },
					{ new Position(1775.82f, -106.15f, 699.87f), 1 },
					{ new Position(1773.02f, -106.14f, 753.39f), 1 },
					{ new Position(1777.41f, -106.13f, 813.68f), 1 },
					{ new Position(1679.49f, -106.16f, 604.14f), 1 },
					{ new Position(1775.71f, -106.16f, 600.22f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_startower_mini_defgroup", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "DefGroup");
		stage.OnSuccess(STAGE_BOSS1);
		return stage;
	}

	/// <summary>Creates the 'Stage2' stage.</summary>
	private DungeonStage CreateStage2()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_NightMaiden, new Dictionary<Position, int>
				{
					{ new Position(104.34f, -106.16f, -947.66f), 1 },
					{ new Position(194.94f, -106.16f, -950.32f), 1 },
					{ new Position(-6.72f, -20.82f, 448.15f), 1 },
					{ new Position(146.51f, -20.82f, 407.32f), 1 },
					{ new Position(185.06f, -20.82f, 651.26f), 1 },
					{ new Position(113.23f, -20.82f, 655.23f), 1 },
					{ new Position(373.44f, -20.82f, 648.72f), 1 },
					{ new Position(447.49f, -20.82f, 648.70f), 1 },
					{ new Position(177.37f, -20.82f, 911.79f), 1 },
					{ new Position(102.63f, -106.45f, -349.16f), 1 },
					{ new Position(190.44f, -106.46f, -346.33f), 1 },
					{ new Position(-26.33f, -106.25f, -475.73f), 1 },
					{ new Position(86.01f, -106.16f, -611.38f), 1 },
					{ new Position(326.21f, -106.22f, -124.96f), 1 },
					{ new Position(323.90f, -106.28f, -193.02f), 1 },
					{ new Position(201.46f, -106.17f, -618.75f), 1 },
					{ new Position(319.66f, -106.25f, -484.81f), 1 },
					{ new Position(-30.53f, -104.38f, -188.68f), 1 },
					{ new Position(-37.23f, -103.91f, -118.92f), 1 },
				}
			},
			{ MonsterId.ID_NightMaiden_Bow, new Dictionary<Position, int>
				{
					{ new Position(112.85f, -106.16f, -868.94f), 1 },
					{ new Position(-66.92f, -20.82f, 446.00f), 1 },
					{ new Position(277.79f, -20.82f, 446.19f), 1 },
					{ new Position(375.29f, -20.82f, 900.14f), 1 },
					{ new Position(-138.35f, -20.82f, 918.66f), 1 },
					{ new Position(7.14f, -105.03f, -265.01f), 1 },
					{ new Position(-72.19f, -104.39f, -260.95f), 1 },
					{ new Position(276.04f, -106.36f, -269.60f), 1 },
					{ new Position(340.95f, -106.29f, -271.54f), 1 },
					{ new Position(324.42f, -106.29f, -410.49f), 1 },
					{ new Position(-37.34f, -106.29f, -409.02f), 1 },
				}
			},
			{ MonsterId.ID_NightMaiden_Mage, new Dictionary<Position, int>
				{
					{ new Position(81.89f, -106.16f, -824.40f), 1 },
					{ new Position(98.03f, -104.33f, -91.75f), 1 },
					{ new Position(354.29f, -20.82f, 451.40f), 1 },
					{ new Position(-66.42f, -20.82f, 651.48f), 1 },
					{ new Position(-125.41f, -20.82f, 652.39f), 1 },
					{ new Position(-67.66f, -20.82f, 903.48f), 1 },
					{ new Position(105.73f, -20.82f, 913.77f), 1 },
					{ new Position(443.02f, -20.82f, 900.16f), 1 },
					{ new Position(218.11f, -106.16f, -842.28f), 1 },
					{ new Position(138.65f, -106.33f, -453.02f), 1 },
					{ new Position(143.92f, -105.54f, -139.13f), 1 },
					{ new Position(171.73f, -105.94f, -101.03f), 1 },
					{ new Position(127.76f, -106.26f, -296.08f), 1 },
					{ new Position(154.16f, -106.49f, -295.66f), 1 },
				}
			},
			{ MonsterId.ID_Pyran, new Dictionary<Position, int>
				{
					{ new Position(116.77f, -106.23f, -506.71f), 1 },
					{ new Position(176.71f, -106.29f, -511.92f), 1 },
					{ new Position(-25.64f, -105.40f, -304.60f), 1 },
					{ new Position(303.29f, -106.34f, -309.64f), 1 },
					{ new Position(-39.18f, -20.82f, 414.73f), 1 },
					{ new Position(317.11f, -20.82f, 420.90f), 1 },
					{ new Position(-117.18f, -20.82f, 623.91f), 1 },
					{ new Position(-106.43f, -20.82f, 887.30f), 1 },
					{ new Position(140.84f, -20.82f, 886.68f), 1 },
					{ new Position(144.48f, -20.82f, 623.23f), 1 },
					{ new Position(417.67f, -20.82f, 618.55f), 1 },
					{ new Position(409.54f, -20.82f, 873.33f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_startower_mini_stage2", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "Stage2");
		stage.OnSuccess(STAGE_STAGE3); // Inferred transition
		return stage;
	}

	/// <summary>Creates the 'Stage3' stage.</summary>
	private DungeonStage CreateStage3()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Spector_Gh_Purple, new Dictionary<Position, int>
				{
					{ new Position(-165.12f, -106.16f, -1192.70f), 1 },
					{ new Position(-169.21f, -106.16f, -1288.97f), 1 },
					{ new Position(-1109.80f, -106.16f, -1032.84f), 1 },
					{ new Position(-705.08f, -106.16f, -1049.37f), 1 },
					{ new Position(-712.91f, -106.16f, -1110.87f), 1 },
					{ new Position(-1141.37f, -106.16f, -1477.66f), 1 },
					{ new Position(-1120.73f, -106.16f, -1083.74f), 1 },
					{ new Position(-1148.00f, -106.16f, -1423.05f), 1 },
					{ new Position(-677.53f, -106.16f, -528.04f), 1 },
					{ new Position(-1224.62f, -106.16f, -533.45f), 1 },
					{ new Position(-1238.45f, -106.16f, -48.55f), 1 },
					{ new Position(-1167.37f, -106.16f, -44.47f), 1 },
					{ new Position(-725.87f, -106.16f, -1408.20f), 1 },
					{ new Position(-714.85f, -106.16f, -1471.46f), 1 },
					{ new Position(-1168.92f, -106.16f, -532.83f), 1 },
					{ new Position(-621.60f, -106.16f, -531.50f), 1 },
					{ new Position(-675.84f, -106.16f, -76.14f), 1 },
					{ new Position(-624.00f, -106.16f, -73.10f), 1 },
					{ new Position(-983.03f, -13.60f, 679.47f), 1 },
					{ new Position(-917.64f, -13.60f, 680.21f), 1 },
					{ new Position(-1210.32f, -13.60f, 608.05f), 1 },
					{ new Position(-1195.72f, -13.60f, 674.73f), 1 },
					{ new Position(-1408.13f, -13.60f, 604.06f), 1 },
					{ new Position(-1395.84f, -13.60f, 676.13f), 1 },
				}
			},
			{ MonsterId.ID_Hallowventor, new Dictionary<Position, int>
				{
					{ new Position(-207.06f, -106.16f, -1244.35f), 1 },
					{ new Position(-1185.42f, -106.16f, -1450.14f), 1 },
					{ new Position(-645.37f, -106.16f, -483.67f), 1 },
					{ new Position(-1195.40f, -106.16f, -491.53f), 1 },
					{ new Position(-1214.87f, -106.16f, 0.38f), 1 },
					{ new Position(-657.20f, -106.16f, -31.01f), 1 },
					{ new Position(-1154.94f, -106.16f, -1046.14f), 1 },
					{ new Position(-752.41f, -106.16f, -1065.12f), 1 },
					{ new Position(-766.20f, -106.16f, -1437.62f), 1 },
					{ new Position(-988.16f, -106.16f, -423.92f), 1 },
					{ new Position(-896.15f, -106.16f, -428.61f), 1 },
					{ new Position(-984.72f, -106.16f, -46.90f), 1 },
					{ new Position(-895.44f, -106.16f, -46.56f), 1 },
					{ new Position(-943.92f, -106.16f, -827.97f), 1 },
				}
			},
			{ MonsterId.ID_Hallowventor_Mage, new Dictionary<Position, int>
				{
					{ new Position(-264.76f, -106.16f, -1186.22f), 1 },
					{ new Position(-269.09f, -106.16f, -1285.12f), 1 },
					{ new Position(-1005.62f, -106.16f, -860.03f), 1 },
					{ new Position(-888.76f, -106.16f, -874.51f), 1 },
					{ new Position(-1068.52f, -106.16f, -236.35f), 1 },
					{ new Position(-790.49f, -106.16f, -241.86f), 1 },
					{ new Position(-943.99f, -107.52f, -1317.74f), 1 },
					{ new Position(-924.03f, -107.52f, -1142.00f), 1 },
					{ new Position(-1027.31f, -107.52f, -1216.79f), 1 },
					{ new Position(-842.51f, -107.52f, -1238.60f), 1 },
					{ new Position(-940.93f, -13.60f, 636.22f), 1 },
					{ new Position(-1176.49f, -13.60f, 648.81f), 1 },
					{ new Position(-1373.00f, -13.60f, 640.62f), 1 },
					{ new Position(-1165.09f, -106.16f, -223.16f), 1 },
					{ new Position(-710.22f, -106.16f, -229.68f), 1 },
					{ new Position(-923.69f, -107.52f, -1257.03f), 1 },
					{ new Position(-916.90f, -107.52f, -1216.11f), 1 },
					{ new Position(-939.27f, -106.16f, -1552.86f), 1 },
					{ new Position(-1276.17f, -106.16f, -1275.34f), 1 },
					{ new Position(-938.48f, -106.16f, -1494.24f), 1 },
					{ new Position(-1273.94f, -106.16f, -1192.38f), 1 },
					{ new Position(-1324.10f, -106.16f, -1224.87f), 1 },
				}
			},
			{ MonsterId.ID_Minivern_Elite, new Dictionary<Position, int>
				{
					{ new Position(-1108.00f, -106.16f, -269.35f), 1 },
					{ new Position(-746.68f, -106.16f, -270.77f), 1 },
					{ new Position(-932.65f, -106.16f, -454.89f), 1 },
					{ new Position(-959.09f, -106.16f, -267.25f), 1 },
					{ new Position(-937.55f, -106.16f, -70.51f), 1 },
					{ new Position(-903.79f, -106.16f, -275.48f), 1 },
				}
			},
			{ MonsterId.ID_Malstatue, new Dictionary<Position, int>
				{
					{ new Position(-991.99f, -97.12f, 96.71f), 1 },
					{ new Position(-993.92f, -78.02f, 187.43f), 1 },
					{ new Position(-995.95f, -66.78f, 297.90f), 1 },
					{ new Position(-896.57f, -75.94f, 197.74f), 1 },
					{ new Position(-894.00f, -96.39f, 105.67f), 1 },
					{ new Position(-885.93f, -66.78f, 307.05f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_startower_mini_stage3", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "Stage3");
		stage.OnSuccess(STAGE_LASTBOSS);
		return stage;
	}

	/// <summary>Creates the 'boss1' stage.</summary>
	private DungeonStage CreateBoss1()
	{
		var stage = BossStage.Create(this, "id_startower_mini_boss1", MonsterId.ID_Boss_Mineloader, new Position(1743.01f, -95.36f, 1203.65f), successStageId: "TBD", failStageId: StageId.Fail,
			allowEscape: false, supports: null, message: "Mineloader has appeared!");
		stage.OnSuccess(STAGE_STAGE2);
		return stage;
	}

	/// <summary>Creates the 'Lastboss' stage.</summary>
	private DungeonStage CreateLastboss()
	{
		var stage = BossStage.Create(this, "id_startower_mini_lastboss", MonsterId.ID_Boss_Harpeia_Orange, new Position(-1862.33f, -13.60f, 637.97f), successStageId: "TBD", failStageId: StageId.Fail,
			allowEscape: false, supports: null, message: "Harpeia has appeared!");
		stage.OnSuccess(STAGE_END, instance =>
		{
			this.MGameMessage(instance, "NOTICE_Dm_scroll", "Stage complete!", 3);
		});
		return stage;
	}

	/// <summary>Creates the 'end' stage.</summary>
	private DungeonStage CreateEnd()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageEndTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(60));


		}, null, this, "id_startower_mini_end", "You will return to the entrance in 60 seconds");

		return stage;
	}

	protected override List<DungeonStage> GetDungeonStages()
	{
		var stages = new List<DungeonStage>
		{
			this.CreateReady(),
			this.CreateDefGroup(),
			this.CreateStage2(),
			this.CreateStage3(),
			this.CreateBoss1(),
			this.CreateLastboss(),
			this.CreateEnd(),
		};

		return stages;
	}

	private void OnStageFailed(InstanceDungeon instance, string reason)
	{
		this.MGameMessage(instance, "NOTICE_Dm_scroll", reason, 5);
		this.DungeonEnded(instance, true);
	}

}
