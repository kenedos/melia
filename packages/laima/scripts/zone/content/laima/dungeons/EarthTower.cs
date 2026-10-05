using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Dungeons;
using Melia.Zone.World.Dungeons.Stages;
using Yggdrasil.Geometry;

[DungeonScript("EARTH_TOWER_SOLMIKI")]
public class EarthTowerSolmikiDungeon : DungeonScript
{
	protected override void Load()
	{
		SetId("EARTH_TOWER_SOLMIKI");
		SetName("Earth Tower Solmiki Area");
		SetMapName("mission_groundtower_2");

		SetStartPosition(new Position(-180f, 240f, -50f));
	}

	private DungeonStage CreateMobFloor(int floor, string stageId, string nextStageId)
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			if (!ZoneServer.Instance.World.TryGetMap(EarthTowerFloorController.MapClassName, out var map))
				throw new InvalidOperationException($"Earth Tower map '{EarthTowerFloorController.MapClassName}' was not found.");

			var runtime = new EarthTowerRuntime(map, instance.Layer);
			
			instance.Vars.SetInt("EarthTower.Floor", floor);

			script.MGameMessage(
				instance,
				"NOTICE_Dm_Clear",
				$"Earth Tower {floor}F - Defeat 40 monsters!",
				5
			);

			foreach (var character in instance.Characters)
			{
				if (character == null)
					continue;

				character.ServerMessage($"Earth Tower {floor}F - Defeat 40 monsters!");
			}

			await Task.Delay(3000, instance.StageCancellationToken);

			runtime.StartFloor(floor);

			var lastDisplayedKills = -1;

			while (!runtime.FloorCompleted)
			{
				await Task.Delay(100, instance.StageCancellationToken);
				runtime.Update(TimeSpan.FromMilliseconds(100));

				var kills = runtime.KilledMonsterCount;

				if (kills != lastDisplayedKills && (kills == 0 || kills == EarthTowerFloorController.MobFloorTotalKills || kills % 5 == 0))
				{
					lastDisplayedKills = kills;

					var progressMessage = $"Earth Tower {floor}F | Monsters {kills}/{EarthTowerFloorController.MobFloorTotalKills}";

					script.MGameMessage(
						instance,
						"NOTICE_Dm_Exclaimation",
						progressMessage,
						2
					);

					foreach (var character in instance.Characters)
					{
						if (character == null)
							continue;

						character.ServerMessage(progressMessage);
					}
				}
			}

			script.MGameMessage(
				instance,
				"NOTICE_Dm_Clear",
				$"Earth Tower {floor}F cleared!",
				5
			);

			foreach (var character in instance.Characters)
			{
				if (character == null)
					continue;

				character.ServerMessage($"Earth Tower {floor}F cleared!");
			}

			runtime.CleanupFloor();

			await Task.Delay(5000, instance.StageCancellationToken);

		}, null, this, stageId, $"Earth Tower {floor}F");

		if (nextStageId != null)
			stage.TransitionTo(nextStageId);
		else
			stage.TransitionTo(StageId.Complete);

		return stage;
	}

	private static string GetFloorStageId(int floor)
	{
		return $"earth_tower_floor_{floor}";
	}

	protected override List<DungeonStage> GetDungeonStages()
	{
		var stages = new List<DungeonStage>();

		for (var floor = 1; floor <= EarthTowerFloorController.MaximumFloor; floor++)
		{
			if (!EarthTowerFloorController.Floors.TryGetValue(floor, out var definition))
				throw new InvalidOperationException($"Earth Tower floor {floor} is not configured.");

			var stageId = GetFloorStageId(floor);
			var nextStageId = floor < EarthTowerFloorController.MaximumFloor ? GetFloorStageId(floor + 1) : null;

			DungeonStage stage;

			switch (definition.Type)
			{
				case EarthTowerFloorType.Mobs:
					stage = CreateMobFloor(floor, stageId, nextStageId);
					break;

				case EarthTowerFloorType.Defense:
					stage = CreateDefenseFloor(floor, stageId, nextStageId);
					break;

				case EarthTowerFloorType.Boss:
					stage = CreateBossFloor(floor, stageId, nextStageId);
					break;

				default:
					throw new InvalidOperationException($"Unsupported Earth Tower floor type '{definition.Type}' on floor {floor}.");
			}

			stages.Add(stage);
		}

		return stages;
	}

	private DungeonStage CreateDefenseFloor(int floor, string stageId, string nextStageId)
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			if (!ZoneServer.Instance.World.TryGetMap(EarthTowerFloorController.MapClassName, out var map))
				throw new InvalidOperationException($"Earth Tower map '{EarthTowerFloorController.MapClassName}' was not found.");

			var runtime = new EarthTowerRuntime(map, instance.Layer);

			instance.Vars.SetInt("EarthTower.Floor", floor);

			script.MGameMessage(
				instance,
				"NOTICE_Dm_Clear",
				$"Earth Tower {floor}F - Protect the structure for 2 minutes!",
				5
			);

			foreach (var character in instance.Characters)
			{
				if (character == null)
					continue;

				character.ServerMessage($"Earth Tower {floor}F - Protect the structure for 2 minutes!");
			}

			await Task.Delay(3000, instance.StageCancellationToken);

			runtime.StartFloor(floor);

			var lastDisplayedSecond = -1;
			var lastDisplayedDefenseWave = 0;

			while (!runtime.FloorCompleted && !runtime.DefenseFailed)
			{
				await Task.Delay(100, instance.StageCancellationToken);

				runtime.Update(TimeSpan.FromMilliseconds(100));

				if (runtime.DefenseWave != lastDisplayedDefenseWave)
				{
					lastDisplayedDefenseWave = runtime.DefenseWave;

					var waveMessage = $"Earth Tower {floor}F | Defense Wave {runtime.DefenseWave}";

					script.MGameMessage(
						instance,
						"NOTICE_Dm_Exclaimation",
						waveMessage,
						2
					);

					foreach (var character in instance.Characters)
					{
						if (character == null)
							continue;

						character.ServerMessage(waveMessage);
					}
				}

				var remaining = runtime.DefenseTimeRemaining;
				var remainingSeconds = Math.Max(0, (int)Math.Ceiling(remaining.TotalSeconds));

				if (remainingSeconds != lastDisplayedSecond && remainingSeconds % 10 == 0)
				{
					lastDisplayedSecond = remainingSeconds;

					var minutes = remainingSeconds / 60;
					var seconds = remainingSeconds % 60;
					var timerMessage = $"Earth Tower {floor}F | Defense | Time Remaining: {minutes}:{seconds:00}";

					foreach (var character in instance.Characters)
					{
						if (character == null)
							continue;

						character.ServerMessage(timerMessage);
					}
				}
			}

			if (runtime.DefenseFailed)
			{
				script.MGameMessage(
					instance,
					"NOTICE_Dm_Clear",
					"Defense failed!",
					5
				);

				await this.FailDefenseFloor(instance, runtime);
				return;
			}

			script.MGameMessage(
				instance,
				"NOTICE_Dm_Clear",
				$"Earth Tower {floor}F cleared!",
				5
			);

			foreach (var character in instance.Characters)
			{
				if (character == null)
					continue;

				character.ServerMessage($"Earth Tower {floor}F cleared!");
			}

			runtime.CleanupFloor();

			await Task.Delay(5000, instance.StageCancellationToken);

		}, null, this, stageId, $"Earth Tower {floor}F");

		if (nextStageId != null)
			stage.TransitionTo(nextStageId);
		else
			stage.TransitionTo(StageId.Complete);

		return stage;
	}

	private DungeonStage CreateBossFloor(int floor, string stageId, string nextStageId)
	{
		var stage = new ActionStage(async (instance, script) =>
		{
			if (!ZoneServer.Instance.World.TryGetMap(EarthTowerFloorController.MapClassName, out var map))
				throw new InvalidOperationException($"Earth Tower map '{EarthTowerFloorController.MapClassName}' was not found.");

			var runtime = new EarthTowerRuntime(map, instance.Layer);

			instance.Vars.SetInt("EarthTower.Floor", floor);

			var definition = EarthTowerFloorController.Floors[floor];
			var bossData = ZoneServer.Instance.Data.MonsterDb.Find(definition.BossMonsterId);
			var bossName = bossData?.Name ?? "Boss";

			script.MGameMessage(
				instance,
				"NOTICE_Dm_Clear",
				$"Earth Tower {floor}F - Defeat {bossName}!",
				5
			);

			await Task.Delay(3000, instance.StageCancellationToken);

			runtime.StartFloor(floor);

			while (!runtime.FloorCompleted)
			{
				await Task.Delay(100, instance.StageCancellationToken);
				runtime.Update(TimeSpan.FromMilliseconds(100));
			}

			script.MGameMessage(
				instance,
				"NOTICE_Dm_Clear",
				$"Earth Tower {floor}F cleared!",
				5
			);

			this.GiveBossCheckpointReward(instance, floor);

			runtime.CleanupFloor();

			await Task.Delay(5000, instance.StageCancellationToken);

		}, null, this, stageId, $"Earth Tower {floor}F");

		if (nextStageId != null)
			stage.TransitionTo(nextStageId);
		else
			stage.TransitionTo(StageId.Complete);

		return stage;
	}

	private const int EarthTowerCheckpointCubeItemId = 642924;

	private void GiveBossCheckpointReward(InstanceDungeon instance, int floor)
	{
		var amount = this.GetCheckpointRewardAmount(floor);

		if (amount <= 0)
			return;

		foreach (var character in instance.Characters)
		{
			if (character == null)
				continue;

			character.AddItem(
				EarthTowerCheckpointCubeItemId,
				amount,
				$"Earth Tower {floor}F checkpoint"
			);

			character.AddonMessage(
				"NOTICE_Dm_Clear",
				$"Earth Tower checkpoint reward: {amount} cube(s)!",
				5
			);
		}
	}

	private int GetCheckpointRewardAmount(int floor)
	{
		switch (floor)
		{
			case 10:
				return 2;

			case 20:
				return 3;

			default:
				return 0;
		}
	}

	private async Task FailDefenseFloor(InstanceDungeon instance, EarthTowerRuntime runtime)
	{
		foreach (var character in instance.Characters.ToList())
		{
			if (character?.Connection == null)
				continue;

			character.ServerMessage("Defense failed! You will leave the dungeon in 5 seconds.");
		}

		await Task.Delay(TimeSpan.FromSeconds(5));

		var characters = instance.Characters.ToList();

		runtime.CleanupFloor();

		foreach (var character in characters)
		{
			if (character?.Connection == null)
				continue;

			if (character.MapId == instance.MapId)
				character.Warp(character.GetCityReturnLocation());
		}

		this.MGameEnd(instance, true);
	}
}
