using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;

/// <summary>
/// Dungeon script for 'Castle Dungeon'
/// ID: ID_CASTLE_MINI_02
///
/// Flow:
/// 1. Start (countdown)
/// 2. CombatPhase - Kill 70% of monsters
/// 3. UnlockWarp - Spawns warp portal (persists until dungeon end)
/// 4. BossWait - Waiting stage, player uses warp to trigger boss
/// 5. Boss/Boss2 depending on orb choice
/// 6. End (60s return timer)
///
/// The UnlockWarp stage demonstrates the new pattern:
/// - Spawns persistent warp NPC
/// - Immediately transitions to BossWait
/// - Warp NPC remains active for player to use
/// - Player interaction triggers boss spawn
/// </summary>
[DungeonScript("ID_CASTLE_MINI_02")]
public class CastleDungeon : DungeonScript
{
	// Stage ID constants
	private const string STAGE_START = "id_castle_mini_02_start";
	private const string STAGE_COMBAT = "id_castle_mini_02_combat";
	private const string STAGE_UNLOCK_WARP = "id_castle_mini_02_unlock_warp";
	private const string STAGE_BOSS_WAIT = "id_castle_mini_02_boss_wait";
	private const string STAGE_BOSS = "id_castle_mini_02_boss";
	private const string STAGE_BOSS2 = "id_castle_mini_02_boss2";
	private const string STAGE_END = "id_castle_mini_02_end";

	// Total monsters from XML: Stage1(91) + Stage2(65) + Stage3(55) + 2 orbs = 213
	private const int TOTAL_COMBAT_MONSTERS = 213;

	private PercentageKillObjective _combatObjective;

	protected override void Load()
	{
		SetId("ID_CASTLE_MINI_02");
		SetName("Castle Dungeon");
		SetMapName("id_castle2");
		SetStartPosition(new Position(32, 132, -976));
	}

	/// <summary>Start stage - countdown before combat.</summary>
	private DungeonStage CreateStart()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);

			var wall = script.SpawnNpc(instance, MonsterId.HiddenWall_250_250_300, "",
				new Position(28.85f, 74.88f, -625.36f), Direction.South);

			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Starting in 10 seconds!", 5);

			await Task.Delay(TimeSpan.FromSeconds(10));
			script.MGameMessage(instance, "NOTICE_Dm_scroll", "Start!!!", 5);
			wall.Map?.RemoveMonster(wall);
		}, null, this, STAGE_START, "Start");

		stage.TransitionTo(STAGE_COMBAT);
		return stage;
	}

	/// <summary>Combat phase - kill 70% to proceed.</summary>
	private DungeonStage CreateCombatPhase()
	{
		var minKillPercentage = KillMonstersStage.DefaultMinimumKillPercentage;

		_combatObjective = new PercentageKillObjective(TOTAL_COMBAT_MONSTERS, minKillPercentage)
		{
			Text = $"Defeat enemies ({(int)(minKillPercentage * 100)}% required)"
		};

		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			instance.Vars.Set("PathChosen", false);
			instance.Vars.Set("ChosenPath", "");

			// Spawn all combat monsters...
			SpawnCombatMonsters(instance, script);

			await Task.CompletedTask;
		},
		new List<QuestObjective> { _combatObjective },
		TOTAL_COMBAT_MONSTERS,
		this,
		STAGE_COMBAT,
		"Combat Phase");

		// When combat completes, unlock the warp
		stage.TransitionTo(STAGE_UNLOCK_WARP);
		return stage;
	}

	/// <summary>
	/// Unlock stage - spawns persistent warp portal.
	/// Uses UnlockStage so the warp persists until dungeon ends.
	/// Immediately transitions to BossWait stage.
	/// </summary>
	private DungeonStage CreateUnlockWarp()
	{
		var stage = new UnlockStage(
			async (instance, script, unlockStage) =>
			{
				script.MGameMessage(instance, "NOTICE_Dm_scroll", "The magic circle has opened!", 5);

				// Determine path based on orb choice
				var chosenPath = instance.Vars.GetString("ChosenPath");
				var isPath1 = chosenPath == "Path1";

				instance.Vars.Set("TargetBossStage", isPath1 ? STAGE_BOSS2 : STAGE_BOSS);

				// Spawn PERSISTENT warp portal - survives stage transitions!
				if (isPath1)
				{
					var warp = unlockStage.SpawnPersistentNpcWithProperties(instance, MonsterId.Whorfzone,
						new Position(643.85f, -1.84f, -385.18f), "Dialog#NPC_CASTLE_WARP");
					// This is what makes the invisible npc look like a portal.
					warp.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
					instance.Vars.Set("WarpNpcHandle", warp.Handle);
				}
				else
				{
					var warp = unlockStage.SpawnPersistentNpcWithProperties(instance, MonsterId.Whorfzone,
						new Position(642.81f, -2.54f, 1152.61f), "Dialog#NPC_CASTLE_WARP");
					warp.AddEffect(new AttachEffect(AnimationName.Portal, 1, EffectLocation.Top));
					instance.Vars.Set("WarpNpcHandle", warp.Handle);
				}

				await Task.CompletedTask;
			},
			null, // No objectives - auto-transitions immediately
			this,
			STAGE_UNLOCK_WARP,
			"Warp Unlocked"
		);

		// AutoTransition is true by default, so this immediately goes to BossWait
		stage.TransitionTo(STAGE_BOSS_WAIT);
		return stage;
	}

	/// <summary>
	/// Boss wait stage - sits here until player uses warp.
	/// The warp NPC dialog calls TriggerBossStage() to advance.
	/// </summary>
	private DungeonStage CreateBossWait()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			// Just wait - player will use the warp portal
			// The warp is already spawned and persisting from UnlockWarp stage
			await Task.CompletedTask;
		}, null, this, STAGE_BOSS_WAIT, "Awaiting Warp");

		// No transitions! Player uses warp dialog to trigger boss stage manually.
		return stage;
	}

	/// <summary>
	/// Called by warp NPC dialog to trigger boss stage.
	/// </summary>
	public void TriggerBossStage(InstanceDungeon instance)
	{
		var targetStageId = instance.Vars.GetString("TargetBossStage");
		if (string.IsNullOrEmpty(targetStageId))
			targetStageId = STAGE_BOSS;

		var bossStage = instance.GetStage(targetStageId);
		if (bossStage != null)
		{
			instance.CurrentStage?.Complete();
			instance.CurrentStage = bossStage;
			_ = bossStage.Initialize(instance);
		}
	}

	/// <summary>Boss stage (Armaox).</summary>
	private DungeonStage CreateBoss()
	{
		var stage = BossStage.Create(this, STAGE_BOSS, MonsterId.ID_Boss_Armaox,
			new Position(640.76f, 108.90f, 379.88f),
			successStageId: STAGE_END,
			failStageId: StageId.Fail,
			allowEscape: false,
			supports: null,
			message: "Boss");

		stage.OnSuccess(STAGE_END, instance =>
		{
			this.MGameMessage(instance, "NOTICE_Dm_scroll", "Returning to entrance in 60 seconds.", 5);
		});

		return stage;
	}

	/// <summary>Boss2 stage (Rambandgad_Red).</summary>
	private DungeonStage CreateBoss2()
	{
		var stage = BossStage.Create(this, STAGE_BOSS2, MonsterId.ID_Boss_Rambandgad_Red,
			new Position(639.35f, 108.90f, 378.02f),
			successStageId: STAGE_END,
			failStageId: StageId.Fail,
			allowEscape: false,
			supports: null,
			message: "Boss2");

		stage.OnSuccess(STAGE_END, instance =>
		{
			this.MGameMessage(instance, "NOTICE_Dm_scroll", "Returning to entrance in 60 seconds.", 5);
		});

		return stage;
	}

	/// <summary>End stage - return timer.</summary>
	private DungeonStage CreateEnd()
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			instance.Vars.Set("StageStartTime", DateTime.UtcNow);
			await Task.Delay(TimeSpan.FromSeconds(60));
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
			CreateStart(),
			CreateCombatPhase(),
			CreateUnlockWarp(),    // NEW: UnlockStage for persistent warp
			CreateBossWait(),      // Waits for player to use warp
			CreateBoss(),
			CreateBoss2(),
			CreateEnd(),
		};
	}

	/// <summary>Spawns all combat phase monsters and sets up orb handlers.</summary>
	private void SpawnCombatMonsters(InstanceDungeon instance, DungeonScript script)
	{
		// ==================== STAGE 1 MONSTERS (from XML) ====================

		// Soldiers (26)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-167.69f, -5.99f, -355.92f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-174.75f, -5.99f, -419.46f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-207.05f, -5.99f, -378.84f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-233.47f, -5.61f, -345.79f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-249.75f, -5.99f, -418.93f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-387.67f, -1.22f, -331.97f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-388.94f, -5.99f, -409.44f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-368.64f, -5.99f, -370.25f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-334.42f, -0.94f, -331.05f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-345.86f, -1.62f, -418.11f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-551.86f, -3.73f, -341.26f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-556.51f, -2.56f, -448.35f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-547.91f, -5.99f, -396.43f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-502.21f, -5.99f, -377.02f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-510.89f, -5.85f, -424.19f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-731.28f, -5.00f, -432.57f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-707.79f, -5.99f, -380.17f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-673.36f, -2.03f, -335.68f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-767.06f, -5.99f, -382.64f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-731.87f, -5.99f, -330.84f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-842.24f, 3.79f, -241.36f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-847.35f, 3.63f, -154.09f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-908.27f, -33.84f, -192.81f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-1086.39f, -89.12f, -140.84f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-1086.21f, -89.12f, -188.45f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-1091.60f, -89.12f, -243.75f));

		// Spearmen (31)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-966.75f, -71.99f, -141.41f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-964.74f, -70.68f, -214.88f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1129.45f, -89.12f, -167.65f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1139.45f, -89.12f, -196.44f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1221.10f, -88.65f, -323.75f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1216.58f, -88.65f, -276.94f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1179.71f, -89.12f, -298.55f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1241.80f, -89.12f, -16.15f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1340.22f, -89.12f, -24.75f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1294.64f, -89.12f, -19.56f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1226.92f, -88.65f, -157.96f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1237.00f, -88.65f, -103.97f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1181.66f, -89.05f, -135.58f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1330.31f, -89.12f, -377.41f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1329.67f, -89.12f, -348.19f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1320.90f, -88.65f, -312.52f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1370.78f, -89.12f, -362.26f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1368.88f, -88.65f, -326.36f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1458.03f, -89.12f, -239.73f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1426.16f, -89.12f, -210.14f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1454.03f, -89.12f, -151.68f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1457.64f, -89.12f, -283.37f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1458.14f, -89.12f, -184.45f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1268.18f, -88.65f, -134.40f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1247.50f, -84.55f, -254.96f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1337.86f, -85.08f, -255.09f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1339.97f, -84.55f, -164.85f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1308.83f, -88.65f, -113.72f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1345.07f, -46.49f, 163.84f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1299.96f, -45.00f, 170.73f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-1251.12f, -43.74f, 176.15f));

		// Archers (34)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1331.47f, -36.31f, 219.42f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1261.66f, -36.31f, 224.33f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1298.98f, -36.31f, 216.17f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1314.33f, -80.19f, 22.34f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1274.28f, -80.28f, 21.56f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1453.42f, -37.66f, 288.61f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1419.23f, -37.66f, 289.94f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1374.27f, -37.66f, 292.91f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1196.45f, -37.66f, 286.34f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1162.57f, -37.66f, 286.92f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1127.76f, -37.66f, 284.64f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1318.32f, -37.66f, 349.67f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1277.23f, -37.66f, 347.80f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1235.30f, -37.66f, 346.81f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1255.36f, -37.66f, 399.68f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1293.72f, -37.66f, 397.33f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1142.71f, -37.66f, 378.79f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1119.64f, -37.66f, 360.36f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1094.39f, -37.66f, 382.97f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1461.47f, -37.66f, 390.75f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1422.74f, -37.66f, 373.66f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1397.16f, -37.66f, 401.64f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1447.36f, -37.66f, 490.92f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1373.72f, -37.66f, 497.40f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1414.23f, -37.66f, 527.90f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1340.53f, -37.66f, 493.97f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1285.57f, -37.66f, 443.59f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1292.21f, -37.29f, 530.74f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1252.30f, -37.66f, 491.28f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1409.85f, -37.66f, 470.79f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1129.45f, -37.48f, 523.01f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1102.86f, -37.66f, 490.81f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1138.16f, -37.66f, 496.75f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-1131.02f, -37.66f, 462.30f));

		// ==================== STAGE 2 MONSTERS (from XML) ====================

		// Soldiers (14)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-799.40f, -0.72f, -93.15f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-748.07f, -5.99f, -128.16f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-712.31f, -5.99f, -100.66f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-742.42f, -5.99f, -76.03f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-794.77f, -2.12f, 72.34f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-757.92f, -5.99f, 30.21f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-697.98f, -5.99f, 45.88f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-737.44f, -5.99f, 64.59f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-785.94f, -4.75f, 238.39f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-732.39f, -5.99f, 213.39f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-680.86f, -2.41f, 242.83f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-801.25f, -0.07f, 448.91f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-726.43f, -0.31f, 400.52f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Soldier, new Position(-669.51f, 0.98f, 429.10f));

		// Warriors (24)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-725.41f, -5.99f, 264.14f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-744.27f, -5.99f, 446.93f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-782.53f, -5.84f, 652.80f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-689.12f, -4.27f, 653.11f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-726.50f, -5.99f, 609.92f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-785.90f, -4.85f, 843.48f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-740.76f, -5.99f, 807.96f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-681.77f, -2.66f, 834.20f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-731.15f, -5.99f, 879.25f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-838.24f, 3.27f, 938.84f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-838.16f, 2.49f, 994.94f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-883.57f, -23.54f, 994.62f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-1039.34f, -89.12f, 957.15f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-1042.55f, -89.12f, 1011.93f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-1009.20f, -89.12f, 988.67f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-688.59f, -4.15f, 1087.68f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-763.13f, -5.99f, 1093.65f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-730.97f, -5.99f, 1142.49f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-523.95f, -5.89f, 1110.58f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-519.59f, -5.99f, 1174.17f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-464.22f, -5.99f, 1148.61f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-296.96f, -5.23f, 1107.84f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-299.14f, -5.99f, 1166.83f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Warrior, new Position(-246.24f, -5.99f, 1141.31f));

		// Mages (26)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1072.77f, -89.12f, 993.78f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1142.72f, -89.12f, 857.64f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1125.55f, -89.12f, 909.28f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1171.71f, -89.12f, 888.53f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1146.11f, -89.12f, 1056.56f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1136.31f, -89.12f, 1138.94f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1198.19f, -89.12f, 1115.67f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1340.35f, -89.12f, 1134.97f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1357.25f, -88.48f, 1069.69f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1406.76f, -89.12f, 1124.35f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1415.77f, -88.68f, 870.64f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1416.67f, -89.12f, 793.83f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1458.88f, -89.12f, 843.74f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1343.57f, -84.56f, 1002.89f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1337.22f, -85.17f, 918.71f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1261.26f, -86.65f, 923.85f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1259.49f, -86.65f, 1006.97f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1526.62f, -89.12f, 899.95f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1479.72f, -89.12f, 941.93f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-902.13f, -37.06f, 947.23f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1473.12f, -89.12f, 1022.06f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-1537.77f, -89.12f, 1074.87f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-14.31f, -3.63f, 984.79f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(71.80f, -2.58f, 969.25f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(2.63f, -17.88f, 935.47f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(51.77f, -15.49f, 937.94f));

		// Fighters (1)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-1243.56f, -84.55f, 962.76f));

		// ==================== STAGE 3 MONSTERS (from XML) ====================

		// Mages (20)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-282.27f, -65.52f, 904.38f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-237.60f, -60.21f, 856.33f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-282.35f, -65.52f, 840.34f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-279.94f, -65.52f, 507.73f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-239.46f, -65.52f, 534.25f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-281.20f, -65.29f, 560.62f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-367.14f, -72.94f, 630.10f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-407.92f, -72.94f, 667.13f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-372.41f, -72.94f, 692.81f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-403.83f, -72.94f, 741.67f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-348.74f, -72.94f, 754.79f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-431.31f, -65.52f, 487.18f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-492.74f, -65.52f, 526.76f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-425.32f, -65.52f, 873.12f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-381.55f, -65.52f, 912.03f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-546.83f, -65.52f, 674.80f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-482.06f, -65.05f, 700.26f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-546.44f, -65.52f, 745.86f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-147.39f, -65.52f, 687.94f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Mage, new Position(-143.11f, -65.52f, 725.99f));

		// Spearmen (4)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-39.44f, -63.99f, 75.10f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-37.41f, -64.53f, 101.19f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-42.71f, -63.68f, 686.10f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Spearman, new Position(-45.32f, -63.30f, 715.05f));

		// Archers (23)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-329.62f, -66.05f, 233.34f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-290.42f, -66.05f, 263.65f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-328.28f, -66.05f, 300.67f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-241.29f, -49.74f, 69.48f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-360.02f, -66.05f, -120.88f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-309.10f, -66.05f, -85.75f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-364.75f, -66.05f, -69.36f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-290.13f, -53.60f, 17.82f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-398.55f, -53.60f, 13.59f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-287.81f, -53.60f, 78.58f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-427.35f, -53.60f, 76.09f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-398.89f, -53.60f, 135.89f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-284.29f, -53.60f, 127.36f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-541.51f, -66.05f, -98.63f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-500.68f, -63.41f, -47.81f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-536.82f, -66.05f, 2.16f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-493.12f, -58.50f, 23.33f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-477.55f, -49.75f, 82.95f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-490.23f, -56.52f, 157.20f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-571.37f, -66.05f, 227.92f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-499.60f, -66.05f, 286.10f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-142.19f, -66.03f, 52.70f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Archer, new Position(-138.52f, -66.03f, 111.08f));

		// Fighters (8)
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-438.13f, -72.94f, 609.70f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-432.89f, -72.94f, 789.63f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-275.10f, -72.94f, 778.44f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-283.44f, -72.94f, 618.04f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-413.19f, -50.65f, -39.19f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-419.28f, -49.79f, 190.40f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-340.68f, -53.60f, 141.73f));
		script.SpawnMonster(instance, MonsterId.ID_Vilkas_Fighter, new Position(-332.53f, -53.60f, 17.26f));

		// ==================== PATH SELECTION ORBS ====================

		// ORB 1 - Stage1 orb, killing this leads to Boss2 path (Rambandgad_Red)
		var orb1 = script.SpawnMonster(instance, MonsterId.ID_Mon_Figurine_Device, new Position(-1290.69f, -37.66f, 490.97f));

		// ORB 2 - Stage2 orb, killing this leads to Boss path (Armaox)
		var orb2 = script.SpawnMonster(instance, MonsterId.ID_Mon_Figurine_Device, new Position(-1292.43f, -86.65f, 967.12f));

		// Orb death handlers
		orb1.Died += (mob, killer) =>
		{
			if (!instance.Vars.GetBool("PathChosen"))
			{
				instance.Vars.Set("ChosenPath", "Path1");
				instance.Vars.Set("PathChosen", true);
				this.MGameMessage(instance, "NOTICE_Dm_scroll", "Path to Boss2 selected!", 3);
			}
		};

		orb2.Died += (mob, killer) =>
		{
			if (!instance.Vars.GetBool("PathChosen"))
			{
				instance.Vars.Set("ChosenPath", "Path2");
				instance.Vars.Set("PathChosen", true);
				this.MGameMessage(instance, "NOTICE_Dm_scroll", "Path to Boss selected!", 3);
			}
		};
	}
}
