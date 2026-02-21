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
/// Dungeon script for 'Underground Chapel Dungeon'
/// </summary>
[DungeonScript("ID_CHAPLE_MINI")]
public class UndergroundChapelDungeon : DungeonScript
{
	// Stage ID constants for branching
	private const string STAGE_READY = "id_chaple_mini_ready";
	private const string STAGE_STAGE1 = "id_chaple_mini_stage1";
	private const string STAGE_STAGE2 = "id_chaple_mini_stage2";
	private const string STAGE_STAGE3 = "id_chaple_mini_stage3";
	private const string STAGE_BOSS = "id_chaple_mini_boss";
	private const string STAGE_END = "id_chaple_mini_end";
	private const string STAGE_TIME = "id_chaple_mini_time";
	private const string STAGE_PERCENTUI = "id_chaple_mini_percentui";
	private const string STAGE_CHECKALLDEAD = "id_chaple_mini_checkalldead";

	protected override void Load()
	{
		this.SetId("ID_CHAPLE_MINI");
		this.SetName("Underground Chapel Dungeon");
		this.SetMapName("id_chaple");
		this.SetStartPosition(new Position(-153f, 0.44f, -934f));
	}

	/// <summary>Creates the 'Ready' stage.</summary>
	private DungeonStage CreateReady()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			// Spawn stage-specific objects
			var npc0 = script.SpawnNpc(instance, MonsterId.HiddenWall_100_200_500, "", new Position(-150.29f, 0.34f, -734.26f), Direction.East);
			stageObjects["npc0"] = npc0;
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Starting in 10 seconds", 5);
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Start!!", 5);
		}, null, this, "id_chaple_mini_ready", "Ready");
		stage.TransitionTo(STAGE_STAGE1);
		return stage;
	}

	/// <summary>Creates the 'stage1' stage.</summary>
	private DungeonStage CreateStage1()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Npanto_Sword, new Dictionary<Position, int>
				{
					{ new Position(-429.53f, 52.95f, -276.61f), 1 },
					{ new Position(-372.67f, 52.95f, -283.39f), 1 },
					{ new Position(-924.03f, 54.38f, -618.28f), 1 },
					{ new Position(-913.70f, 54.38f, -557.13f), 1 },
					{ new Position(-788.01f, 54.38f, -464.91f), 1 },
					{ new Position(-787.78f, 54.38f, -387.16f), 1 },
					{ new Position(-441.93f, 52.95f, -103.19f), 1 },
					{ new Position(-385.75f, 52.95f, -102.53f), 1 },
					{ new Position(-407.47f, 52.95f, -68.93f), 1 },
					{ new Position(-433.74f, 52.95f, 306.23f), 1 },
					{ new Position(-799.65f, 56.34f, 291.45f), 1 },
					{ new Position(-796.69f, 56.17f, 356.09f), 1 },
					{ new Position(-837.61f, 55.43f, 324.42f), 1 },
				}
			},
			{ MonsterId.ID_Npanto_Staff, new Dictionary<Position, int>
				{
					{ new Position(-407.45f, 52.95f, -246.35f), 1 },
					{ new Position(-947.93f, 54.38f, -281.79f), 1 },
					{ new Position(-1071.95f, 54.38f, -455.35f), 1 },
					{ new Position(-1065.82f, 54.38f, -390.32f), 1 },
					{ new Position(-817.14f, 54.38f, -425.61f), 1 },
					{ new Position(-463.59f, 52.95f, 345.00f), 1 },
					{ new Position(-393.60f, 52.95f, 342.73f), 1 },
					{ new Position(-676.44f, 54.12f, 291.58f), 1 },
					{ new Position(-679.35f, 54.14f, 358.76f), 1 },
					{ new Position(-639.11f, 53.90f, 328.22f), 1 },
					{ new Position(-903.59f, 56.54f, 130.93f), 1 },
					{ new Position(-895.88f, 55.88f, 187.23f), 1 },
					{ new Position(-938.66f, 56.08f, 158.87f), 1 },
					{ new Position(-985.99f, 56.48f, 131.07f), 1 },
					{ new Position(-978.10f, 55.53f, 193.98f), 1 },
					{ new Position(-926.46f, 55.38f, 431.08f), 1 },
					{ new Position(-908.79f, 56.58f, 512.69f), 1 },
					{ new Position(-950.97f, 56.15f, 482.81f), 1 },
					{ new Position(-1004.03f, 55.78f, 451.49f), 1 },
					{ new Position(-997.37f, 56.72f, 525.74f), 1 },
				}
			},
			{ MonsterId.ID_Banshee, new Dictionary<Position, int>
				{
					{ new Position(-437.28f, 52.95f, -210.61f), 1 },
					{ new Position(-379.47f, 52.95f, -216.72f), 1 },
					{ new Position(-657.99f, 54.03f, -458.62f), 1 },
					{ new Position(-657.22f, 53.97f, -402.54f), 1 },
					{ new Position(-975.09f, 54.38f, -242.86f), 1 },
					{ new Position(-983.51f, 54.38f, -315.37f), 1 },
					{ new Position(-992.30f, 54.38f, -554.21f), 1 },
					{ new Position(-995.29f, 54.38f, -621.95f), 1 },
					{ new Position(-1155.40f, 54.38f, -456.54f), 1 },
					{ new Position(-1160.32f, 54.38f, -382.16f), 1 },
					{ new Position(-1106.38f, 54.38f, -422.46f), 1 },
					{ new Position(-444.17f, 52.95f, -34.41f), 1 },
					{ new Position(-384.35f, 52.95f, -37.39f), 1 },
					{ new Position(-442.93f, 52.95f, 89.35f), 1 },
					{ new Position(-379.36f, 52.95f, 93.02f), 1 },
					{ new Position(-408.56f, 52.95f, 127.01f), 1 },
					{ new Position(-870.05f, 55.10f, 364.53f), 1 },
					{ new Position(-869.63f, 55.65f, 297.66f), 1 },
					{ new Position(-1064.74f, 55.35f, 288.51f), 1 },
					{ new Position(-1052.77f, 55.20f, 360.57f), 1 },
					{ new Position(-1090.52f, 55.68f, 331.73f), 1 },
					{ new Position(-1142.38f, 56.44f, 300.59f), 1 },
					{ new Position(-1143.95f, 56.54f, 364.04f), 1 },
					{ new Position(-221.32f, 67.18f, 986.23f), 1 },
					{ new Position(-73.64f, 67.15f, 975.44f), 1 },
					{ new Position(-138.59f, 67.18f, 982.60f), 1 },
				}
			},
			{ MonsterId.ID_Puragi_Green, new Dictionary<Position, int>
				{
					{ new Position(-588.61f, 53.61f, -408.48f), 1 },
					{ new Position(-590.52f, 53.64f, -454.36f), 1 },
					{ new Position(-536.02f, 53.33f, -408.16f), 1 },
					{ new Position(-534.07f, 53.29f, -452.39f), 1 },
					{ new Position(-915.63f, 54.38f, -245.26f), 1 },
					{ new Position(-908.36f, 54.38f, -312.54f), 1 },
					{ new Position(-955.28f, 54.38f, -585.25f), 1 },
					{ new Position(-860.45f, 54.38f, -466.91f), 1 },
					{ new Position(-857.80f, 54.38f, -391.96f), 1 },
					{ new Position(-443.26f, 52.95f, 156.07f), 1 },
					{ new Position(-377.84f, 52.95f, 154.77f), 1 },
					{ new Position(-468.75f, 52.95f, 276.35f), 1 },
					{ new Position(-383.43f, 52.95f, 270.37f), 1 },
					{ new Position(-590.75f, 53.64f, 290.21f), 1 },
					{ new Position(-590.29f, 53.62f, 355.61f), 1 },
					{ new Position(-910.83f, 54.38f, -297.64f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_chaple_mini_stage1", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "stage1");
		stage.OnSuccess(STAGE_STAGE2);
		return stage;
	}

	/// <summary>Creates the 'stage2' stage.</summary>
	private DungeonStage CreateStage2()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Pawnd, new Dictionary<Position, int>
				{
					{ new Position(222.23f, 53.25f, -455.05f), 1 },
					{ new Position(519.42f, 54.38f, -379.62f), 1 },
					{ new Position(521.90f, 54.38f, -459.87f), 1 },
					{ new Position(222.38f, 53.22f, -390.33f), 1 },
					{ new Position(803.39f, 54.38f, -558.22f), 1 },
					{ new Position(735.60f, 67.04f, -410.06f), 1 },
					{ new Position(658.43f, 67.04f, -370.27f), 1 },
					{ new Position(654.20f, 67.04f, -484.67f), 1 },
					{ new Position(730.00f, 67.04f, -462.06f), 1 },
					{ new Position(669.86f, 67.04f, -423.55f), 1 },
					{ new Position(245.84f, 53.41f, 275.18f), 1 },
					{ new Position(239.99f, 53.31f, 338.58f), 1 },
					{ new Position(332.71f, 53.79f, 272.75f), 1 },
					{ new Position(335.76f, 53.74f, 350.87f), 1 },
					{ new Position(531.11f, 56.32f, 299.06f), 1 },
					{ new Position(535.54f, 56.33f, 358.93f), 1 },
					{ new Position(641.25f, 55.61f, 433.33f), 1 },
					{ new Position(646.29f, 57.28f, 506.73f), 1 },
					{ new Position(716.19f, 55.39f, 432.70f), 1 },
					{ new Position(717.33f, 57.26f, 507.65f), 1 },
					{ new Position(830.27f, 55.75f, 291.67f), 1 },
					{ new Position(826.54f, 55.79f, 373.41f), 1 },
					{ new Position(857.10f, 56.13f, 333.56f), 1 },
					{ new Position(891.69f, 57.03f, 292.12f), 1 },
					{ new Position(888.60f, 56.85f, 366.07f), 1 },
					{ new Position(734.89f, 67.87f, 280.42f), 1 },
					{ new Position(740.19f, 67.87f, 354.00f), 1 },
					{ new Position(-160.42f, 67.18f, 1188.80f), 1 },
					{ new Position(-79.26f, 67.17f, 1084.55f), 1 },
					{ new Position(-83.33f, 67.18f, 1189.04f), 1 },
					{ new Position(-220.07f, 67.18f, 1192.09f), 1 },
					{ new Position(-217.30f, 67.18f, 1084.39f), 1 },
					{ new Position(-147.85f, 67.18f, 1088.19f), 1 },
					{ new Position(55.30f, 52.95f, -208.39f), 1 },
					{ new Position(129.86f, 52.95f, -219.18f), 1 },
					{ new Position(84.37f, 52.95f, -252.76f), 1 },
					{ new Position(38.71f, 52.95f, 208.82f), 1 },
					{ new Position(116.93f, 52.95f, 194.75f), 1 },
					{ new Position(51.39f, 52.95f, 283.85f), 1 },
					{ new Position(113.03f, 52.95f, 283.64f), 1 },
					{ new Position(49.35f, 52.95f, 353.54f), 1 },
					{ new Position(129.35f, 52.95f, 347.21f), 1 },
				}
			},
			{ MonsterId.ID_Deadbornscab_Mage, new Dictionary<Position, int>
				{
					{ new Position(308.01f, 53.79f, -456.96f), 1 },
					{ new Position(305.93f, 53.86f, -398.24f), 1 },
					{ new Position(684.01f, 54.38f, -235.41f), 1 },
					{ new Position(690.69f, 54.38f, -307.85f), 1 },
					{ new Position(433.59f, 54.38f, -455.10f), 1 },
					{ new Position(435.15f, 54.38f, -397.72f), 1 },
					{ new Position(800.26f, 54.38f, -460.81f), 1 },
					{ new Position(804.10f, 54.38f, -375.88f), 1 },
					{ new Position(862.38f, 54.38f, -465.69f), 1 },
					{ new Position(869.94f, 54.38f, -389.34f), 1 },
					{ new Position(776.60f, 54.38f, -278.83f), 1 },
					{ new Position(606.41f, 67.04f, -426.79f), 1 },
					{ new Position(705.65f, 57.15f, 126.33f), 1 },
					{ new Position(715.02f, 55.70f, 186.76f), 1 },
					{ new Position(663.59f, 67.87f, 284.13f), 1 },
					{ new Position(660.50f, 67.87f, 355.56f), 1 },
					{ new Position(699.00f, 67.87f, 317.95f), 1 },
					{ new Position(291.63f, 53.58f, 306.78f), 1 },
					{ new Position(41.18f, 52.95f, -6.84f), 1 },
					{ new Position(110.52f, 52.95f, -21.08f), 1 },
					{ new Position(40.10f, 52.95f, 80.61f), 1 },
					{ new Position(119.64f, 52.95f, 91.18f), 1 },
					{ new Position(88.53f, 52.95f, 322.30f), 1 },
				}
			},
			{ MonsterId.ID_Galok, new Dictionary<Position, int>
				{
					{ new Position(272.02f, 53.55f, -422.92f), 1 },
					{ new Position(654.86f, 54.38f, -280.28f), 1 },
					{ new Position(681.10f, 54.38f, -573.56f), 1 },
					{ new Position(484.87f, 54.38f, -419.34f), 1 },
					{ new Position(836.09f, 54.38f, -419.02f), 1 },
					{ new Position(568.65f, 56.27f, 423.14f), 1 },
					{ new Position(574.73f, 56.04f, 227.46f), 1 },
					{ new Position(802.76f, 56.17f, 192.58f), 1 },
					{ new Position(804.17f, 56.10f, 437.85f), 1 },
					{ new Position(82.90f, 52.95f, -70.66f), 1 },
					{ new Position(83.84f, 52.95f, 137.09f), 1 },
				}
			},
			{ MonsterId.ID_Pawndel, new Dictionary<Position, int>
				{
					{ new Position(614.24f, 54.38f, -315.24f), 1 },
					{ new Position(612.96f, 54.38f, -249.39f), 1 },
					{ new Position(635.64f, 54.38f, -601.43f), 1 },
					{ new Position(637.83f, 54.38f, -540.17f), 1 },
					{ new Position(711.61f, 54.38f, -612.33f), 1 },
					{ new Position(718.81f, 54.38f, -547.08f), 1 },
					{ new Position(528.00f, 54.38f, -526.18f), 1 },
					{ new Position(544.21f, 54.38f, -307.46f), 1 },
					{ new Position(467.98f, 57.28f, 292.12f), 1 },
					{ new Position(459.66f, 57.28f, 370.28f), 1 },
					{ new Position(496.94f, 57.28f, 324.91f), 1 },
					{ new Position(641.46f, 57.28f, 127.73f), 1 },
					{ new Position(644.59f, 55.75f, 194.42f), 1 },
					{ new Position(43.64f, 52.95f, -307.49f), 1 },
					{ new Position(128.92f, 52.95f, -306.21f), 1 },
					{ new Position(39.65f, 52.95f, -119.79f), 1 },
					{ new Position(127.94f, 52.95f, -120.21f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_chaple_mini_stage2", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "stage2");
		stage.OnSuccess(STAGE_STAGE3);
		return stage;
	}

	/// <summary>Creates the 'stage3' stage.</summary>
	private DungeonStage CreateStage3()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Spector_Gh, new Dictionary<Position, int>
				{
					{ new Position(-187.23f, 0.34f, 353.89f), 1 },
					{ new Position(-195.11f, 0.34f, 172.85f), 1 },
					{ new Position(-104.62f, 0.34f, 347.82f), 1 },
					{ new Position(-187.55f, 0.34f, -38.79f), 1 },
					{ new Position(-110.05f, 0.34f, 169.61f), 1 },
					{ new Position(-105.58f, 0.34f, -36.82f), 1 },
					{ new Position(-191.08f, 0.34f, 285.18f), 1 },
					{ new Position(-99.72f, 0.34f, 283.08f), 1 },
					{ new Position(-127.85f, 0.34f, 539.97f), 1 },
					{ new Position(-202.86f, 0.34f, 501.44f), 1 },
					{ new Position(-82.29f, 0.34f, 498.23f), 1 },
					{ new Position(-143.41f, 0.34f, 468.31f), 1 },
				}
			},
			{ MonsterId.ID_Zombiegirl2_Chpel, new Dictionary<Position, int>
				{
					{ new Position(-186.34f, 0.34f, -406.44f), 1 },
					{ new Position(-107.89f, 0.34f, -400.13f), 1 },
					{ new Position(-152.46f, 0.34f, -258.82f), 1 },
					{ new Position(-196.34f, 0.34f, -285.29f), 1 },
					{ new Position(-112.51f, 0.34f, -292.07f), 1 },
					{ new Position(-148.90f, 0.34f, -69.82f), 1 },
					{ new Position(-183.60f, 0.34f, 95.77f), 1 },
					{ new Position(-152.61f, 0.34f, 124.96f), 1 },
					{ new Position(-108.20f, 0.34f, 92.26f), 1 },
				}
			},
			{ MonsterId.ID_New_Desmodus, new Dictionary<Position, int>
				{
					{ new Position(-182.23f, 0.34f, -229.37f), 1 },
					{ new Position(-100.66f, 0.34f, -231.08f), 1 },
					{ new Position(-104.50f, 0.34f, -102.07f), 1 },
					{ new Position(-194.83f, 0.34f, -101.75f), 1 },
					{ new Position(-148.71f, 0.34f, -431.36f), 1 },
					{ new Position(-195.17f, 0.34f, -462.32f), 1 },
					{ new Position(-114.10f, 0.34f, -468.42f), 1 },
					{ new Position(-146.44f, 0.34f, 312.15f), 1 },
				}
			},
			{ MonsterId.ID_Glizardon_Chapel, new Dictionary<Position, int>
				{
					{ new Position(-199.75f, 0.34f, 556.14f), 1 },
					{ new Position(-75.29f, 0.34f, 548.77f), 1 },
					{ new Position(-91.06f, 0.34f, 443.07f), 1 },
					{ new Position(-190.43f, 0.34f, 454.45f), 1 },
					{ new Position(-303.47f, 67.02f, 1029.52f), 1 },
					{ new Position(-8.71f, 67.01f, 1026.15f), 1 },
					{ new Position(-9.56f, 67.02f, 1137.12f), 1 },
					{ new Position(-293.34f, 67.04f, 1149.83f), 1 },
					{ new Position(-288.81f, 67.05f, 1271.40f), 1 },
					{ new Position(92.91f, 66.79f, 1308.92f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_chaple_mini_stage3", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "stage3");
		stage.OnSuccess(STAGE_BOSS);
		return stage;
	}

	/// <summary>Creates the 'boss' stage.</summary>
	private DungeonStage CreateBoss()
	{
		var stage = BossStage.Create(this, "id_chaple_mini_boss", MonsterId.ID_Boss_ShadowGaoler, new Position(-147.82f, 67.18f, 1315.02f), successStageId: "TBD", failStageId: StageId.Fail,
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
		}, null, this, "id_chaple_mini_end", "You will return to the entrance in 60 seconds");
		stage.TransitionTo(STAGE_TIME);
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
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "5 minutes are left until the time limit!!", 5);
			await Task.Delay(TimeSpan.FromSeconds(300));
			await Task.Delay(TimeSpan.FromSeconds(60));
		}, null, this, "id_chaple_mini_time", "Time");
		stage.TransitionTo(STAGE_PERCENTUI);
		return stage;
	}

	/// <summary>Creates the 'PercentUI' stage.</summary>
	private DungeonStage CreatePercentUI()
	{
		var stage = new InitialSetupStage(instance => { }, this, "id_chaple_mini_percentui");
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
			this.CreateStage3(),
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
