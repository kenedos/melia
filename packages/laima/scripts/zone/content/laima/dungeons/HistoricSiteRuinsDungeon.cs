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
/// Dungeon script for 'Historic Site Ruins'
/// </summary>
[DungeonScript("ID_REMAINS_MINI")]
public class HistoricSiteRuinsDungeon : DungeonScript
{
	// Stage ID constants for branching
	private const string STAGE_START = "id_remains_mini_start";
	private const string STAGE_END = "id_remains_mini_end";
	private const string STAGE_STAGE1 = "id_remains_mini_stage1";
	private const string STAGE_STAGE2 = "id_remains_mini_stage2";
	private const string STAGE_STAGE3 = "id_remains_mini_stage3";
	private const string STAGE_BOSS = "id_remains_mini_boss";
	private const string STAGE_TIME = "id_remains_mini_time";
	private const string STAGE_PERCENTUI = "id_remains_mini_percentui";
	private const string STAGE_CHECKALLDEAD = "id_remains_mini_checkalldead";

	protected override void Load()
	{
		this.SetId("ID_REMAINS_MINI");
		this.SetName("Historic Site Ruins");
		this.SetMapName("id_remains");
		this.SetStartPosition(new Position(-875.33936f, 148.20898f, -1386.5686f));
	}

	/// <summary>Creates the 'START' stage.</summary>
	private DungeonStage CreateSTART()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			// Spawn stage-specific objects
			var npc0 = script.SpawnNpc(instance, MonsterId.HiddenWall_100_200_500, "", new Position(-870.29f, 196.28f, -1146.27f), Direction.East);
			stageObjects["npc0"] = npc0;
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Starting in 10 seconds", 5);
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Start!!!", 5);
		}, null, this, "id_remains_mini_start", "START");
		stage.TransitionTo(STAGE_STAGE1);
		return stage;
	}

	/// <summary>Creates the 'stage1' stage.</summary>
	private DungeonStage CreateStage1()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Lizardman, new Dictionary<Position, int>
				{
					{ new Position(-791.58f, 204.22f, -887.91f), 1 },
					{ new Position(-680.66f, 204.22f, -672.62f), 1 },
					{ new Position(-619.50f, 204.22f, -846.57f), 1 },
					{ new Position(-503.53f, 204.22f, -739.42f), 1 },
					{ new Position(-426.04f, 204.22f, -608.96f), 1 },
					{ new Position(-780.23f, 204.22f, -830.26f), 1 },
					{ new Position(-723.19f, 204.22f, -884.97f), 1 },
					{ new Position(-658.88f, 204.22f, -623.42f), 1 },
					{ new Position(-612.32f, 204.22f, -679.09f), 1 },
					{ new Position(-606.68f, 204.22f, -801.51f), 1 },
					{ new Position(-578.51f, 204.22f, -832.15f), 1 },
					{ new Position(-490.58f, 204.22f, -702.18f), 1 },
					{ new Position(-458.52f, 204.22f, -740.30f), 1 },
					{ new Position(-418.64f, 204.22f, -560.36f), 1 },
					{ new Position(-382.53f, 204.22f, -606.34f), 1 },
					{ new Position(-195.81f, 204.22f, -518.92f), 1 },
					{ new Position(-83.71f, 204.22f, -396.86f), 1 },
					{ new Position(-28.51f, 204.22f, -504.86f), 1 },
					{ new Position(131.48f, 181.58f, -472.81f), 1 },
					{ new Position(139.84f, 193.27f, -1159.83f), 1 },
					{ new Position(170.96f, 193.27f, -1116.65f), 1 },
				}
			},
			{ MonsterId.ID_Flying_Flog, new Dictionary<Position, int>
				{
					{ new Position(-177.62f, 204.22f, -476.97f), 1 },
					{ new Position(-145.72f, 204.22f, -530.51f), 1 },
					{ new Position(-84.69f, 204.22f, -349.89f), 1 },
					{ new Position(-33.63f, 204.22f, -398.53f), 1 },
					{ new Position(29.05f, 204.22f, -486.15f), 1 },
					{ new Position(24.90f, 204.22f, -558.21f), 1 },
					{ new Position(165.17f, 166.49f, -501.69f), 1 },
					{ new Position(172.66f, 170.92f, -453.16f), 1 },
					{ new Position(381.96f, 159.60f, -573.64f), 1 },
					{ new Position(375.24f, 159.60f, -525.08f), 1 },
					{ new Position(399.20f, 159.60f, -408.26f), 1 },
					{ new Position(353.64f, 159.60f, -370.89f), 1 },
				}
			},
			{ MonsterId.ID_Cockatries, new Dictionary<Position, int>
				{
					{ new Position(336.81f, 159.60f, -539.76f), 1 },
					{ new Position(341.30f, 159.60f, -414.21f), 1 },
					{ new Position(662.45f, 125.85f, -719.89f), 1 },
					{ new Position(486.61f, 125.85f, -758.56f), 1 },
					{ new Position(510.29f, 125.85f, -712.69f), 1 },
					{ new Position(463.27f, 125.85f, -815.90f), 1 },
					{ new Position(615.27f, 125.85f, -922.94f), 1 },
					{ new Position(643.74f, 125.85f, -970.43f), 1 },
					{ new Position(587.87f, 126.88f, -1098.07f), 1 },
					{ new Position(604.18f, 127.33f, -1140.21f), 1 },
					{ new Position(543.15f, 127.25f, -1094.86f), 1 },
					{ new Position(304.58f, 192.24f, -1270.98f), 1 },
					{ new Position(271.50f, 191.20f, -1314.17f), 1 },
				}
			},
			{ MonsterId.ID_Tama, new Dictionary<Position, int>
				{
					{ new Position(599.49f, 158.65f, -521.88f), 1 },
					{ new Position(595.22f, 159.60f, -481.02f), 1 },
					{ new Position(641.81f, 159.60f, -499.64f), 1 },
					{ new Position(598.08f, 159.60f, -373.11f), 1 },
					{ new Position(610.99f, 159.60f, -322.96f), 1 },
					{ new Position(643.40f, 159.60f, -388.19f), 1 },
					{ new Position(655.84f, 159.60f, -344.61f), 1 },
					{ new Position(665.14f, 125.85f, -677.16f), 1 },
					{ new Position(692.24f, 125.85f, -718.24f), 1 },
					{ new Position(635.19f, 125.85f, -707.28f), 1 },
					{ new Position(520.59f, 125.85f, -797.88f), 1 },
					{ new Position(537.79f, 125.85f, -759.56f), 1 },
					{ new Position(595.53f, 125.85f, -960.98f), 1 },
					{ new Position(672.61f, 125.85f, -926.54f), 1 },
					{ new Position(325.99f, 190.21f, -1348.71f), 1 },
					{ new Position(360.40f, 191.05f, -1307.24f), 1 },
					{ new Position(352.50f, 193.27f, -1157.68f), 1 },
					{ new Position(394.06f, 193.27f, -1105.53f), 1 },
					{ new Position(228.24f, 193.27f, -1015.44f), 1 },
					{ new Position(258.09f, 193.27f, -969.52f), 1 },
				}
			},
			{ MonsterId.ID_Lizardman_Mage, new Dictionary<Position, int>
				{
					{ new Position(545.97f, 127.77f, -1134.42f), 1 },
					{ new Position(316.03f, 191.28f, -1305.35f), 1 },
					{ new Position(351.31f, 193.27f, -1116.09f), 1 },
					{ new Position(123.52f, 193.27f, -1119.46f), 1 },
					{ new Position(214.20f, 193.27f, -974.19f), 1 },
					{ new Position(79.23f, 193.27f, -1124.86f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_remains_mini_stage1", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "stage1");
		stage.OnSuccess(STAGE_STAGE2);
		return stage;
	}

	/// <summary>Creates the 'stage2' stage.</summary>
	private DungeonStage CreateStage2()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_InfroBurk, new Dictionary<Position, int>
				{
					{ new Position(217.67f, 206.39f, 24.42f), 1 },
					{ new Position(281.65f, 204.22f, -28.40f), 1 },
					{ new Position(370.88f, 221.36f, 190.17f), 1 },
					{ new Position(431.55f, 218.64f, 142.39f), 1 },
					{ new Position(314.67f, 221.36f, 319.44f), 1 },
					{ new Position(357.85f, 221.36f, 358.42f), 1 },
					{ new Position(262.47f, 221.36f, 359.13f), 1 },
					{ new Position(300.06f, 221.36f, 397.96f), 1 },
				}
			},
			{ MonsterId.ID_Zolem, new Dictionary<Position, int>
				{
					{ new Position(271.03f, 204.22f, 72.25f), 1 },
					{ new Position(327.30f, 204.22f, 24.98f), 1 },
					{ new Position(415.36f, 221.36f, 231.19f), 1 },
					{ new Position(456.49f, 221.36f, 184.57f), 1 },
					{ new Position(618.69f, 221.36f, 257.51f), 1 },
					{ new Position(573.98f, 221.36f, 305.57f), 1 },
					{ new Position(614.45f, 221.36f, 342.10f), 1 },
					{ new Position(661.97f, 221.36f, 292.84f), 1 },
					{ new Position(798.56f, 221.36f, 411.02f), 1 },
					{ new Position(753.77f, 221.36f, 453.03f), 1 },
					{ new Position(843.73f, 221.36f, 440.07f), 1 },
					{ new Position(804.61f, 221.36f, 487.83f), 1 },
					{ new Position(643.83f, 221.36f, 565.80f), 1 },
					{ new Position(690.67f, 221.36f, 629.23f), 1 },
					{ new Position(439.50f, 221.36f, 500.32f), 1 },
					{ new Position(320.79f, 221.36f, 356.37f), 1 },
				}
			},
			{ MonsterId.ID_Gravegolem, new Dictionary<Position, int>
				{
					{ new Position(797.92f, 221.36f, 454.99f), 1 },
					{ new Position(616.24f, 221.36f, 307.16f), 1 },
					{ new Position(623.11f, 221.36f, 632.96f), 1 },
					{ new Position(-245.36f, 231.64f, 996.66f), 1 },
					{ new Position(-201.19f, 232.34f, 1049.83f), 1 },
					{ new Position(-98.36f, 232.71f, 1151.23f), 1 },
					{ new Position(-69.36f, 232.38f, 1193.20f), 1 },
				}
			},
			{ MonsterId.ID_Wolf_Statue_Bow, new Dictionary<Position, int>
				{
					{ new Position(342.33f, 221.36f, 640.37f), 1 },
					{ new Position(427.31f, 221.36f, 752.15f), 1 },
					{ new Position(-470.18f, 232.71f, 1058.05f), 1 },
					{ new Position(-225.03f, 232.71f, 1319.43f), 1 },
					{ new Position(64.58f, 232.71f, 1438.73f), 1 },
					{ new Position(-581.20f, 233.51f, 1179.24f), 1 },
					{ new Position(-526.44f, 234.59f, 1244.56f), 1 },
					{ new Position(-483.88f, 234.72f, 1295.64f), 1 },
					{ new Position(-440.14f, 234.80f, 1349.29f), 1 },
					{ new Position(-395.83f, 234.96f, 1404.65f), 1 },
				}
			},
			{ MonsterId.ID_Templeslave_Sword, new Dictionary<Position, int>
				{
					{ new Position(462.36f, 221.36f, 435.79f), 1 },
					{ new Position(511.33f, 221.36f, 472.08f), 1 },
					{ new Position(321.37f, 242.43f, 718.20f), 1 },
					{ new Position(356.91f, 239.95f, 758.32f), 1 },
					{ new Position(12.00f, 282.94f, 980.55f), 1 },
					{ new Position(-38.40f, 247.23f, 1056.79f), 1 },
					{ new Position(-324.93f, 216.41f, 1184.95f), 1 },
				}
			},
			{ MonsterId.ID_Templeslave_Mage, new Dictionary<Position, int>
				{
					{ new Position(315.03f, 251.86f, 757.32f), 1 },
					{ new Position(-73.45f, 245.89f, 1013.84f), 1 },
					{ new Position(167.33f, 272.91f, 887.61f), 1 },
					{ new Position(124.16f, 272.96f, 857.78f), 1 },
					{ new Position(-112.25f, 232.67f, 1194.61f), 1 },
					{ new Position(-250.81f, 231.96f, 1048.83f), 1 },
					{ new Position(67.66f, 232.42f, 1370.94f), 1 },
					{ new Position(-387.42f, 232.71f, 1012.54f), 1 },
					{ new Position(21.99f, 232.71f, 1408.04f), 1 },
					{ new Position(-392.48f, 216.41f, 1190.62f), 1 },
					{ new Position(-347.67f, 216.41f, 1238.83f), 1 },
				}
			},
			{ MonsterId.ID_Schlesien_Heavycavarly, new Dictionary<Position, int>
				{
					{ new Position(-522.92f, 232.71f, 1067.57f), 1 },
					{ new Position(-493.43f, 232.71f, 1110.12f), 1 },
					{ new Position(-276.93f, 222.37f, 1334.52f), 1 },
					{ new Position(-233.68f, 232.71f, 1361.46f), 1 },
					{ new Position(27.95f, 232.71f, 1470.17f), 1 },
					{ new Position(119.46f, 232.71f, 1407.73f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_remains_mini_stage2", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "stage2");
		stage.OnSuccess(STAGE_STAGE3);
		return stage;
	}

	/// <summary>Creates the 'stage3' stage.</summary>
	private DungeonStage CreateStage3()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.ID_Wolf_Statue, new Dictionary<Position, int>
				{
					{ new Position(-8.35f, 255.23f, 203.37f), 1 },
					{ new Position(35.46f, 248.11f, 203.87f), 1 },
					{ new Position(87.24f, 247.50f, 142.58f), 1 },
					{ new Position(76.12f, 245.00f, 175.97f), 1 },
					{ new Position(-69.68f, 284.98f, 41.01f), 1 },
					{ new Position(-111.81f, 288.48f, 60.26f), 1 },
					{ new Position(-61.53f, 292.50f, -1.22f), 1 },
					{ new Position(-241.55f, 301.47f, -105.45f), 1 },
					{ new Position(-208.05f, 301.47f, -142.59f), 1 },
					{ new Position(-351.89f, 348.70f, -198.46f), 1 },
					{ new Position(-323.13f, 351.50f, -227.00f), 1 },
					{ new Position(-425.73f, 454.95f, 48.37f), 1 },
					{ new Position(-358.08f, 454.94f, 47.60f), 1 },
					{ new Position(-748.78f, 454.31f, -90.30f), 1 },
					{ new Position(-678.81f, 455.40f, -66.60f), 1 },
				}
			},
			{ MonsterId.ID_Schlesien_Claw, new Dictionary<Position, int>
				{
					{ new Position(-202.67f, 301.47f, -101.79f), 1 },
					{ new Position(-297.20f, 355.61f, -267.54f), 1 },
					{ new Position(-480.82f, 449.86f, -318.74f), 1 },
					{ new Position(-462.88f, 434.86f, -282.29f), 1 },
					{ new Position(-746.56f, 455.03f, -41.57f), 1 },
					{ new Position(-395.05f, 454.94f, 86.39f), 1 },
					{ new Position(-567.41f, 454.93f, 243.85f), 1 },
					{ new Position(-752.59f, 454.93f, 139.03f), 1 },
					{ new Position(-490.24f, 454.93f, 248.31f), 1 },
					{ new Position(-831.35f, 454.93f, 80.59f), 1 },
				}
			},
			{ MonsterId.ID_Schlesien_Darkmage, new Dictionary<Position, int>
				{
					{ new Position(-246.73f, 301.47f, -137.12f), 1 },
					{ new Position(-92.17f, 293.95f, 1.29f), 1 },
					{ new Position(-344.96f, 367.30f, -240.57f), 1 },
					{ new Position(-518.08f, 455.79f, -289.59f), 1 },
					{ new Position(-495.14f, 444.93f, -257.88f), 1 },
					{ new Position(-823.61f, 454.93f, 143.61f), 1 },
					{ new Position(-534.38f, 454.93f, 277.18f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "id_remains_mini_stage3", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "stage3");
		stage.OnSuccess(STAGE_BOSS);
		return stage;
	}

	/// <summary>Creates the 'boss' stage.</summary>
	private DungeonStage CreateBoss()
	{
		var stage = BossStage.Create(this, "id_remains_mini_boss", MonsterId.ID_Boss_Necrovanter, new Position(-614.51f, 454.97f, 88.31f), successStageId: "TBD", failStageId: StageId.Fail,
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
			instance.Vars.Set("StageEndTime", DateTime.UtcNow);
			await Task.Delay(TimeSpan.FromSeconds(60));
		}, null, this, "id_remains_mini_end", "end");
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
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "5 minutes are left until the dungeon ends!!", 5);
			await Task.Delay(TimeSpan.FromSeconds(300));
			await Task.Delay(TimeSpan.FromSeconds(60));
		}, null, this, "id_remains_mini_time", "Time");
		stage.TransitionTo(STAGE_PERCENTUI);
		return stage;
	}

	/// <summary>Creates the 'PercentUI' stage.</summary>
	private DungeonStage CreatePercentUI()
	{
		var stage = new InitialSetupStage(instance => { }, this, "id_remains_mini_percentui");
		stage.TransitionTo(StageId.Complete);
		return stage;
	}

	protected override List<DungeonStage> GetDungeonStages()
	{
		var stages = new List<DungeonStage>
		{
			this.CreateSTART(),
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
