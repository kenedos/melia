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
/// Dungeon script for 'Catacombs Underground Dungeon'
/// </summary>
[DungeonScript("ID_CATACOM_MINI")]
public class CatacombsUndergroundDungeon : DungeonScript
{
	// Stage ID constants for branching
	private const string STAGE_READY = "id_catacom_mini_ready";
	private const string STAGE_BOSS = "id_catacom_mini_boss";
	private const string STAGE_END = "id_catacom_mini_end";
	private const string STAGE_STAGE1 = "id_catacom_mini_stage1";
	private const string STAGE_STAGE2 = "id_catacom_mini_stage2";
	private const string STAGE_TRAP = "id_catacom_mini_trap";
	private const string STAGE_트랩조건 = "id_catacom_mini_";
	private const string STAGE_TIME = "id_catacom_mini_time";
	private const string STAGE_PERCENTUI = "id_catacom_mini_percentui";
	private const string STAGE_CHECKALLDEAD = "id_catacom_mini_checkalldead";

	protected override void Load()
	{
		this.SetId("ID_CATACOM_MINI");
		this.SetName("Catacombs Underground Dungeon");
		this.SetMapName("id_catacomb");
		this.SetStartPosition(new Position(-853, 74, 723));
	}

	/// <summary>Creates the 'Ready' stage.</summary>
	private DungeonStage CreateReady()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Starting in 10 seconds", 5);
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Start!", 5);
		}, null, this, "id_catacom_mini_ready", "Ready");
		stage.TransitionTo(STAGE_STAGE1);
		return stage;
	}

	/// <summary>Creates the 'stage1' stage.</summary>
	private DungeonStage CreateStage1()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Cronewt_Bow, new Dictionary<Position, int>
				{
					{ new Position(-1548.49f, 82.45f, 107.73f), 1 },
					{ new Position(-1534.12f, 82.45f, 181.28f), 1 },
					{ new Position(-1481.19f, 56.98f, -147.12f), 1 },
					{ new Position(-1430.30f, 56.98f, -181.37f), 1 },
					{ new Position(-1478.01f, 56.98f, -242.33f), 1 },
					{ new Position(-1524.57f, 56.98f, -187.74f), 1 },
					{ new Position(599.36f, 15.57f, 480.46f), 1 },
					{ new Position(833.55f, 15.57f, 482.64f), 1 },
					{ new Position(686.84f, 15.57f, 481.12f), 1 },
				}
			},
			{ MonsterId.ID_Cronewt_Mage, new Dictionary<Position, int>
				{
					{ new Position(-1373.83f, 82.45f, 261.46f), 1 },
					{ new Position(-1461.91f, 82.45f, 166.30f), 1 },
					{ new Position(-1479.98f, 82.45f, 221.81f), 1 },
					{ new Position(-1375.40f, 56.98f, -257.51f), 1 },
					{ new Position(-769.19f, 107.76f, -137.10f), 1 },
					{ new Position(385.64f, 67.62f, 216.13f), 1 },
					{ new Position(424.88f, 67.62f, 177.94f), 1 },
					{ new Position(686.22f, 15.57f, 401.21f), 1 },
					{ new Position(678.04f, 15.57f, 551.46f), 1 },
					{ new Position(758.72f, 15.57f, 475.23f), 1 },
					{ new Position(-211.57f, 84.46f, -162.38f), 1 },
					{ new Position(-202.49f, 84.46f, 25.45f), 1 },
					{ new Position(-161.15f, 84.46f, 68.78f), 1 },
				}
			},
			{ MonsterId.ID_Prisonfighter, new Dictionary<Position, int>
				{
					{ new Position(-1582.74f, 82.45f, 238.08f), 1 },
					{ new Position(-1519.24f, 82.45f, 295.33f), 1 },
					{ new Position(-1320.59f, 82.45f, 196.55f), 1 },
					{ new Position(-1491.97f, 82.45f, 19.60f), 1 },
					{ new Position(-1451.31f, 56.98f, -327.17f), 1 },
					{ new Position(-1593.99f, 56.98f, -153.49f), 1 },
					{ new Position(-1500.82f, 56.98f, -54.09f), 1 },
					{ new Position(-1213.72f, 65.06f, -137.98f), 1 },
					{ new Position(-1155.83f, 65.06f, -185.84f), 1 },
					{ new Position(-1101.74f, 65.06f, -125.68f), 1 },
					{ new Position(-1149.14f, 65.06f, -79.32f), 1 },
					{ new Position(-813.94f, 107.76f, -177.10f), 1 },
					{ new Position(-811.28f, 107.76f, -94.28f), 1 },
					{ new Position(603.53f, 15.57f, 571.98f), 1 },
					{ new Position(610.81f, 15.57f, 387.63f), 1 },
					{ new Position(794.30f, 15.57f, 393.61f), 1 },
					{ new Position(793.96f, 15.57f, 560.36f), 1 },
					{ new Position(986.58f, 4.90f, 554.58f), 1 },
					{ new Position(1072.13f, 4.90f, 483.43f), 1 },
					{ new Position(1149.50f, 4.90f, 399.00f), 1 },
					{ new Position(1178.24f, 4.90f, 578.50f), 1 },
					{ new Position(51.84f, 102.04f, -99.91f), 1 },
					{ new Position(-9.39f, 102.04f, -152.17f), 1 },
					{ new Position(-117.34f, 84.46f, -264.04f), 1 },
					{ new Position(147.38f, 84.46f, 15.29f), 1 },
					{ new Position(-138.09f, 84.46f, 19.74f), 1 },
					{ new Position(144.49f, 84.46f, -242.29f), 1 },
				}
			},
			{ MonsterId.ID_Lapasape, new Dictionary<Position, int>
				{
					{ new Position(-1371.54f, 56.98f, -117.84f), 1 },
					{ new Position(-1250.75f, 65.06f, -233.54f), 1 },
					{ new Position(-1056.40f, 65.06f, -215.43f), 1 },
					{ new Position(-1060.14f, 65.06f, -48.86f), 1 },
					{ new Position(-1225.26f, 65.06f, -52.90f), 1 },
					{ new Position(-519.40f, 107.76f, -189.48f), 1 },
					{ new Position(-454.35f, 107.76f, -137.75f), 1 },
					{ new Position(-502.01f, 107.76f, -63.49f), 1 },
					{ new Position(376.57f, 67.62f, 176.71f), 1 },
					{ new Position(989.56f, 4.90f, 475.72f), 1 },
					{ new Position(1048.56f, 4.90f, 414.81f), 1 },
					{ new Position(967.60f, 4.90f, 381.31f), 1 },
					{ new Position(973.54f, 4.90f, 604.50f), 1 },
					{ new Position(1067.12f, 4.90f, 555.79f), 1 },
					{ new Position(1128.80f, 4.90f, 491.04f), 1 },
					{ new Position(1142.22f, 4.90f, 605.11f), 1 },
					{ new Position(1129.69f, 4.90f, 342.90f), 1 },
					{ new Position(-217.73f, 84.46f, -91.58f), 1 },
					{ new Position(-21.63f, 84.46f, 129.24f), 1 },
					{ new Position(48.61f, 84.46f, 108.69f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_catacom_mini_stage1", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "stage1");
		stage.OnSuccess(STAGE_STAGE2);
		return stage;
	}

	/// <summary>Creates the 'stage2' stage.</summary>
	private DungeonStage CreateStage2()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Pyran_Green, new Dictionary<Position, int>
				{
					{ new Position(-14.20f, 56.98f, 1230.27f), 1 },
					{ new Position(-44.04f, 65.06f, 1018.83f), 1 },
					{ new Position(25.13f, 65.06f, 961.45f), 1 },
					{ new Position(70.03f, 65.06f, 1030.04f), 1 },
					{ new Position(17.59f, 65.06f, 1075.55f), 1 },
					{ new Position(204.05f, 56.98f, 1276.52f), 1 },
					{ new Position(-22.67f, 56.98f, 1408.96f), 1 },
					{ new Position(-655.19f, 15.57f, -835.36f), 1 },
					{ new Position(-636.56f, 15.57f, -780.39f), 1 },
					{ new Position(-573.18f, 15.57f, -839.93f), 1 },
					{ new Position(-572.91f, 15.57f, -768.16f), 1 },
					{ new Position(-707.00f, 4.90f, -1064.11f), 1 },
					{ new Position(-595.23f, 4.90f, -1213.72f), 1 },
					{ new Position(-639.50f, 4.90f, -1320.24f), 1 },
					{ new Position(-744.48f, 4.90f, -1222.60f), 1 },
					{ new Position(-650.24f, 4.90f, -1148.08f), 1 },
					{ new Position(28.78f, 91.97f, -192.66f), 1 },
					{ new Position(181.22f, 84.46f, -356.66f), 1 },
					{ new Position(241.56f, 84.46f, -289.36f), 1 },
					{ new Position(92.74f, 91.81f, -140.07f), 1 },
					{ new Position(-268.24f, 82.45f, 1288.75f), 1 },
					{ new Position(-231.52f, 82.45f, 1324.36f), 1 },
				}
			},
			{ MonsterId.ID_Velffigy_Green, new Dictionary<Position, int>
				{
					{ new Position(-289.59f, 82.45f, 1337.97f), 1 },
					{ new Position(-390.39f, 82.45f, 1250.38f), 1 },
					{ new Position(-200.93f, 82.45f, 1437.39f), 1 },
					{ new Position(22.12f, 65.06f, 1015.38f), 1 },
					{ new Position(-674.85f, 15.57f, -922.52f), 1 },
					{ new Position(-518.89f, 15.57f, -909.36f), 1 },
					{ new Position(-557.01f, 4.90f, -1070.21f), 1 },
					{ new Position(-495.44f, 4.90f, -1123.75f), 1 },
					{ new Position(-624.42f, 4.90f, -1186.02f), 1 },
					{ new Position(33.77f, 84.46f, -387.63f), 1 },
					{ new Position(266.14f, 84.46f, -125.17f), 1 },
					{ new Position(204.16f, 84.46f, -308.08f), 1 },
				}
			},
			{ MonsterId.ID_Raider_Bow, new Dictionary<Position, int>
				{
					{ new Position(-360.17f, 82.45f, 1351.49f), 1 },
					{ new Position(-313.75f, 82.45f, 1403.79f), 1 },
					{ new Position(-202.41f, 82.67f, 1185.55f), 1 },
					{ new Position(-149.02f, 82.59f, 1248.77f), 1 },
					{ new Position(-112.38f, 65.06f, 970.19f), 1 },
					{ new Position(88.71f, 65.06f, 1149.77f), 1 },
					{ new Position(-50.63f, 56.98f, 1254.49f), 1 },
					{ new Position(0.74f, 56.98f, 1270.08f), 1 },
					{ new Position(237.41f, 56.98f, 1288.50f), 1 },
					{ new Position(-56.01f, 56.98f, 1427.13f), 1 },
					{ new Position(69.76f, 56.98f, 1483.39f), 1 },
					{ new Position(115.85f, 56.98f, 1480.34f), 1 },
					{ new Position(-481.83f, 15.57f, -774.99f), 1 },
					{ new Position(-579.88f, 15.57f, -680.49f), 1 },
					{ new Position(-514.30f, 4.90f, -1269.57f), 1 },
					{ new Position(-690.59f, 4.90f, -1269.38f), 1 },
				}
			},
			{ MonsterId.ID_Hook_Old, new Dictionary<Position, int>
				{
					{ new Position(-379.12f, 82.45f, 1204.65f), 1 },
					{ new Position(-342.34f, 82.45f, 1242.96f), 1 },
					{ new Position(-366.83f, 82.45f, 1486.07f), 1 },
					{ new Position(-443.20f, 82.45f, 1411.82f), 1 },
					{ new Position(-187.19f, 82.45f, 1388.59f), 1 },
					{ new Position(-157.10f, 82.45f, 1419.28f), 1 },
					{ new Position(-103.96f, 65.06f, 920.68f), 1 },
					{ new Position(128.84f, 65.06f, 1123.21f), 1 },
					{ new Position(163.89f, 65.06f, 966.19f), 1 },
					{ new Position(169.87f, 56.98f, 1407.66f), 1 },
					{ new Position(215.96f, 56.98f, 1407.07f), 1 },
					{ new Position(163.00f, 56.98f, 1464.56f), 1 },
					{ new Position(17.12f, 107.76f, 623.56f), 1 },
					{ new Position(-287.93f, 67.62f, -425.11f), 1 },
					{ new Position(-231.95f, 67.62f, -484.13f), 1 },
					{ new Position(-346.60f, 67.62f, -486.44f), 1 },
					{ new Position(-296.32f, 67.62f, -530.27f), 1 },
					{ new Position(-737.78f, 15.57f, -751.00f), 1 },
					{ new Position(-466.65f, 15.57f, -824.85f), 1 },
					{ new Position(-581.88f, 15.57f, -968.27f), 1 },
					{ new Position(17.18f, 84.46f, -321.07f), 1 },
					{ new Position(199.47f, 84.46f, -91.66f), 1 },
				}
			},
			{ MonsterId.ID_Meleech, new Dictionary<Position, int>
				{
					{ new Position(-23.83f, 65.06f, 872.78f), 1 },
					{ new Position(50.60f, 65.06f, 865.85f), 1 },
					{ new Position(135.33f, 65.06f, 940.85f), 1 },
					{ new Position(89.92f, 56.98f, 1320.87f), 1 },
					{ new Position(65.95f, 56.98f, 1371.94f), 1 },
					{ new Position(132.69f, 56.98f, 1373.37f), 1 },
					{ new Position(-26.71f, 107.76f, 365.13f), 1 },
					{ new Position(5.50f, 107.76f, 322.95f), 1 },
					{ new Position(57.54f, 107.76f, 342.41f), 1 },
					{ new Position(11.98f, 107.76f, 385.51f), 1 },
					{ new Position(48.65f, 107.76f, 676.66f), 1 },
					{ new Position(-31.80f, 107.76f, 670.36f), 1 },
					{ new Position(-404.69f, 67.62f, -542.69f), 1 },
					{ new Position(-350.98f, 67.62f, -585.60f), 1 },
					{ new Position(-628.68f, 15.57f, -628.66f), 1 },
					{ new Position(-735.93f, 15.57f, -856.81f), 1 },
					{ new Position(-446.40f, 15.57f, -803.83f), 1 },
					{ new Position(236.61f, 84.46f, -51.92f), 1 },
					{ new Position(-49.26f, 84.46f, -358.00f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_catacom_mini_stage2", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "stage2");
		stage.OnSuccess(STAGE_TRAP);
		return stage;
	}

	/// <summary>Creates the 'trap' stage.</summary>
	private DungeonStage CreateTrap()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "The Iron Door has opened!", 5);
		}, null, this, "id_catacom_mini_trap", "trap");
		stage.TransitionTo(STAGE_BOSS);
		return stage;
	}

	/// <summary>Creates the '트랩조건' stage.</summary>
	private DungeonStage Create트랩조건()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.Cemetery_Irongate, new Dictionary<Position, int>
				{
					{ new Position(295.51f, 108.89f, -400.83f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_catacom_mini_", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "트랩조건");
		stage.OnSuccess(STAGE_TRAP, instance => this.MGameMessage(instance, "NOTICE_Dm_scroll", "Stage complete!", 3));
		return stage;
	}

	/// <summary>Creates the 'boss' stage.</summary>
	private DungeonStage CreateBoss()
	{
		var stage = BossStage.Create(this, "id_catacom_mini_boss", MonsterId.ID_Lastboss_Manticen, new Position(870.40f, 89.17f, -972.43f), successStageId: "TBD", failStageId: StageId.Fail,
			allowEscape: false, supports: null, message: "boss");
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
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(60));
		}, null, this, "id_catacom_mini_end", "You will return to the entrance in 60 seconds");
		//stage.TransitionTo(STAGE_STAGE1);
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
			await Task.Delay(TimeSpan.FromSeconds(300));
			await Task.Delay(TimeSpan.FromSeconds(60));
		}, null, this, "id_catacom_mini_time", "Time");
		stage.TransitionTo(STAGE_PERCENTUI);
		return stage;
	}

	/// <summary>Creates the 'PercentUI' stage.</summary>
	private DungeonStage CreatePercentUI()
	{
		var stage = new InitialSetupStage(instance => { }, this, "id_catacom_mini_percentui");
		stage.TransitionTo(StageId.Complete);
		return stage;
	}

	protected override List<DungeonStage> GetDungeonStages()
	{
		var stages = new List<DungeonStage>
		{
			this.CreateReady(),
			this.CreateStage1(),
			this.CreateStage2(),
			this.CreateTrap(),
			this.Create트랩조건(),
			this.CreateBoss(),
			this.CreateEnd(),
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
