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
[DungeonScript("MYTHIC_STARTOWER_MINI_AUTO_HARD")]
public class FallenLegwynFamilyDungeon : DungeonScript
{
	// Stage ID constants for branching
	private const string STAGE_STAGE3 = "mythic_startower_mini_auto_hard_stage3";
	private const string STAGE_BOSS1 = "mythic_startower_mini_auto_hard_boss1";
	private const string STAGE_BOSS2 = "mythic_startower_mini_auto_hard_boss2";
	private const string STAGE_READY = "mythic_startower_mini_auto_hard_ready";
	private const string STAGE_TIMER = "mythic_startower_mini_auto_hard_timer";
	private const string STAGE_TIME = "mythic_startower_mini_auto_hard_time";
	private const string STAGE_SUCCESS = "mythic_startower_mini_auto_hard_success";
	private const string STAGE_END = "mythic_startower_mini_auto_hard_end";
	private const string STAGE_PATTERN = "mythic_startower_mini_auto_hard_pattern";
	private const string STAGE_EVENT_2101_SUPPLY = "mythic_startower_mini_auto_hard_event_2101_supply";

	protected override void Load()
	{
		this.SetId("MYTHIC_STARTOWER_MINI_AUTO_HARD");
		this.SetName("Fallen Legwyn Family Dungeon");
		this.SetMapName("Mythic_startower");
		this.SetStartPosition(new Position(0f, 0f, 0f));
	}

	/// <summary>Creates the 'Stage3' stage.</summary>
	private DungeonStage CreateStage3()
	{
		var monsters = new Dictionary<int, Dictionary<Position, int>> {
			{ MonsterId.Mythic_Pawnd_Purple, new Dictionary<Position, int>
				{
					{ new Position(-146.14f, -106.16f, -1154.39f), 1 },
					{ new Position(-161.41f, -106.16f, -1331.94f), 1 },
					{ new Position(-252.15f, -106.16f, -1166.69f), 1 },
					{ new Position(-245.49f, -106.16f, -1290.55f), 1 },
					{ new Position(-118.42f, -106.16f, -1246.47f), 1 },
					{ new Position(-747.39f, -106.16f, -428.93f), 1 },
					{ new Position(-664.51f, -106.16f, -422.97f), 1 },
					{ new Position(-1910.26f, -111.29f, -1312.19f), 1 },
					{ new Position(-1811.69f, -111.29f, -1315.44f), 1 },
					{ new Position(-1860.79f, -26.84f, -25.31f), 1 },
					{ new Position(-1888.24f, -57.17f, -373.25f), 1 },
				}
			},
			{ MonsterId.Mythic_Pawndel_Blue, new Dictionary<Position, int>
				{
					{ new Position(-196.22f, -106.16f, -1240.15f), 1 },
					{ new Position(-775.45f, -106.16f, -508.25f), 1 },
					{ new Position(-629.22f, -106.16f, -504.17f), 1 },
					{ new Position(-708.08f, -106.16f, -573.10f), 1 },
					{ new Position(-1990.78f, -111.29f, -1182.11f), 1 },
					{ new Position(-1763.22f, -111.29f, -1176.48f), 1 },
					{ new Position(-1952.30f, -111.29f, -1246.81f), 1 },
					{ new Position(-1886.78f, -26.84f, -27.36f), 1 },
					{ new Position(-1860.24f, -57.17f, -373.39f), 1 },
					{ new Position(-1779.32f, -111.29f, -1237.62f), 1 },
				}
			},
			{ MonsterId.Mythic_Spector_Gh_Purple, new Dictionary<Position, int>
				{
					{ new Position(-903.39f, -106.16f, -958.16f), 1 },
					{ new Position(-639.22f, -106.16f, -1120.69f), 1 },
					{ new Position(-889.13f, -107.52f, -1135.91f), 1 },
					{ new Position(-844.08f, -107.23f, -1059.81f), 1 },
					{ new Position(-638.38f, -106.16f, -1321.22f), 1 },
					{ new Position(-942.70f, -107.50f, -1417.57f), 1 },
				}
			},
			{ MonsterId.Mythic_Hallowventor, new Dictionary<Position, int>
				{
					{ new Position(-991.02f, -106.16f, -1498.17f), 1 },
					{ new Position(-1022.79f, -107.52f, -1351.01f), 1 },
					{ new Position(-692.48f, -106.16f, -1168.65f), 1 },
					{ new Position(-689.37f, -106.16f, -1250.79f), 1 },
					{ new Position(-1027.28f, -106.16f, -993.92f), 1 },
					{ new Position(-1003.09f, -107.52f, -1124.85f), 1 },
				}
			},
			{ MonsterId.Mythic_Pyran, new Dictionary<Position, int>
				{
					{ new Position(-1125.97f, -106.16f, -482.63f), 1 },
					{ new Position(-1000.50f, -106.16f, -42.83f), 1 },
					{ new Position(-870.50f, -106.16f, -43.16f), 1 },
					{ new Position(-1171.56f, -13.60f, 644.38f), 1 },
				}
			},
			{ MonsterId.Mythic_Glizardon_Instance, new Dictionary<Position, int>
				{
					{ new Position(-874.08f, -106.16f, -139.63f), 1 },
					{ new Position(-1008.16f, -106.16f, -150.88f), 1 },
					{ new Position(-935.83f, -106.16f, -166.33f), 1 },
					{ new Position(-689.72f, -106.16f, -484.68f), 1 },
				}
			},
			{ MonsterId.Mythic_Hallowventor_Mage, new Dictionary<Position, int>
				{
					{ new Position(-890.96f, -106.16f, -1504.85f), 1 },
					{ new Position(-899.54f, -107.52f, -1337.84f), 1 },
					{ new Position(-842.09f, -106.16f, -1431.88f), 1 },
					{ new Position(-944.61f, -107.01f, -1039.32f), 1 },
				}
			},
			{ MonsterId.Mythic_Minivern_Elite, new Dictionary<Position, int>
				{
					{ new Position(-931.41f, -106.16f, -65.05f), 1 },
					{ new Position(-1157.36f, -106.16f, -537.25f), 1 },
					{ new Position(-1068.63f, -106.16f, -539.38f), 1 },
					{ new Position(-1183.43f, -106.16f, -424.22f), 1 },
					{ new Position(-1077.00f, -106.16f, -417.53f), 1 },
				}
			},
			{ MonsterId.Mythic_Malstatue, new Dictionary<Position, int>
				{
					{ new Position(-991.99f, -97.12f, 96.71f), 1 },
					{ new Position(-993.92f, -78.02f, 187.43f), 1 },
					{ new Position(-995.95f, -66.78f, 297.90f), 1 },
					{ new Position(-896.57f, -75.94f, 197.74f), 1 },
					{ new Position(-894.00f, -96.39f, 105.67f), 1 },
					{ new Position(-885.93f, -66.78f, 307.05f), 1 },
					{ new Position(-1917.79f, -57.17f, -415.61f), 1 },
					{ new Position(-1916.31f, -57.17f, -331.99f), 1 },
					{ new Position(-1848.34f, -57.17f, -399.95f), 1 },
					{ new Position(-1836.00f, -57.17f, -331.85f), 1 },
					{ new Position(-1913.93f, -26.84f, 17.30f), 1 },
					{ new Position(-1910.63f, -26.84f, -65.42f), 1 },
					{ new Position(-1839.44f, -26.84f, -64.34f), 1 },
					{ new Position(-1834.06f, -26.84f, 16.52f), 1 },
				}
			},
			{ MonsterId.Mythic_NightMaMythicen_Mage, new Dictionary<Position, int>
				{
					{ new Position(-1868.69f, -107.85f, -1042.73f), 1 },
					{ new Position(-1794.07f, -106.10f, -997.34f), 1 },
					{ new Position(-1942.53f, -106.10f, -1018.91f), 1 },
					{ new Position(-943.65f, -13.60f, 637.85f), 1 },
				}
			},
			{ MonsterId.Mythic_NightMaMythicen, new Dictionary<Position, int>
				{
					{ new Position(-1874.91f, -106.11f, -970.91f), 1 },
					{ new Position(-872.89f, -13.60f, 630.16f), 1 },
					{ new Position(-1223.00f, -13.60f, 686.50f), 1 },
					{ new Position(-1012.42f, -13.60f, 633.25f), 1 },
					{ new Position(-1217.01f, -13.60f, 591.38f), 1 },
				}
			},
			{ MonsterId.Mythic_NightMaMythicen_Bow, new Dictionary<Position, int>
				{
					{ new Position(-1932.96f, -106.14f, -939.65f), 1 },
					{ new Position(-1841.89f, -106.14f, -926.78f), 1 },
					{ new Position(-988.90f, -13.60f, 547.92f), 1 },
					{ new Position(-907.64f, -13.60f, 549.28f), 1 },
				}
			},
		};
		var stage = KillMonstersStage.Create(this, "mythic_startower_mini_auto_hard_stage3", monsters, successStageId: "TBD", failStageId: StageId.Fail,
			message: "Stage3");
		stage.OnSuccess(STAGE_BOSS1);
		return stage;
	}

	/// <summary>Creates the 'Boss1' stage.</summary>
	private DungeonStage CreateBoss1()
	{
		var stage = BossStage.Create(this, "mythic_startower_mini_auto_hard_boss1", MonsterId.Mythic_Boss_Mineloader, new Position(-1860.19f, -13.60f, 655.12f), successStageId: "TBD", failStageId: StageId.Fail,
			allowEscape: false, supports: null, message: "Boss1");
		return stage;
	}

	/// <summary>Creates the 'Boss2' stage.</summary>
	private DungeonStage CreateBoss2()
	{
		var stage = BossStage.Create(this, "mythic_startower_mini_auto_hard_boss2", MonsterId.Mythic_Boss_Harpeia_Orange, new Position(-1861.65f, -111.29f, -1210.56f), successStageId: "TBD", failStageId: StageId.Fail,
			allowEscape: false, supports: null, message: "Boss2");
		return stage;
	}

	/// <summary>Creates the 'READY' stage.</summary>
	private DungeonStage CreateREADY()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Starting in 10 seconds", 5);
			await Task.Delay(TimeSpan.FromSeconds(10));
			instance.Vars.SetInt("Start", 1);
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Start!", 5);
		}, null, this, "mythic_startower_mini_auto_hard_ready", "READY");
		stage.TransitionTo(STAGE_TIMER);
		return stage;
	}

	/// <summary>Creates the 'TIMER' stage.</summary>
	private DungeonStage CreateTIMER()
	{
		var stage = new InitialSetupStage(instance => { }, this, "mythic_startower_mini_auto_hard_timer");
		stage.TransitionTo(StageId.Complete);
		return stage;
	}

	/// <summary>Creates the 'TIME' stage.</summary>
	private DungeonStage CreateTIME()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(3660));
		}, null, this, "mythic_startower_mini_auto_hard_time", "TIME");
		stage.TransitionTo(STAGE_SUCCESS);
		return stage;
	}

	/// <summary>Creates the 'SUCCESS' stage.</summary>
	private DungeonStage CreateSUCCESS()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(3));
		}, null, this, "mythic_startower_mini_auto_hard_success", "SUCCESS");
		stage.TransitionTo(STAGE_EVENT_2101_SUPPLY);
		return stage;
	}

	/// <summary>Creates the 'END' stage.</summary>
	private DungeonStage CreateEND()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			var stageObjects = new Dictionary<string, IMonster>();
			await Task.Delay(TimeSpan.FromSeconds(60));
		}, null, this, "mythic_startower_mini_auto_hard_end", "Returning to the entrance soon.");
		stage.TransitionTo(STAGE_PATTERN);
		return stage;
	}

	/// <summary>Creates the 'Pattern' stage.</summary>
	private DungeonStage CreatePattern()
	{
		var stage = new InitialSetupStage(instance => { }, this, "mythic_startower_mini_auto_hard_pattern");
		stage.TransitionTo(StageId.Complete);
		return stage;
	}

	protected override List<DungeonStage> GetDungeonStages()
	{
		var stages = new List<DungeonStage>
		{
			this.CreateStage3(),
			this.CreateBoss1(),
			this.CreateBoss2(),
			this.CreateREADY(),
			this.CreateTIMER(),
			this.CreateTIME(),
			this.CreateSUCCESS(),
			this.CreateEND(),
			this.CreatePattern(),
		};

		return stages;
	}

	private void OnStageFailed(InstanceDungeon instance, string reason)
	{
		this.MGameMessage(instance, "NOTICE_Dm_scroll", reason, 5);
		this.DungeonEnded(instance, true);
	}

}
