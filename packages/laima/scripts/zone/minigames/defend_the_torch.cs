using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Spawning;
using Yggdrasil.Geometry;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

/// <summary>
/// Defend the Torch minigame.
/// A torch monster spawns and players must defend it from waves of monsters for few minutes.
/// </summary>
public class DefendTheTorchScript : IMinigameScript
{
	public IMinigameInstance CreateInstance(Map map, Position position, Direction direction)
	{
		return new DefendTheTorchInstance(map, position, direction);
	}
}

public class DefendTheTorchInstance : MinigameBase
{
	// Configuration (constants)
	private const int TorchMonsterId = MonsterId.Npc_Zachariel_Lantern_2;

	// Reward Tiers Configuration (edit these values to change rewards)
	// Format: (MinLevel, MaxLevel, ItemId, Amount)
	private static readonly (int MinLevel, int MaxLevel, int ItemId, int Amount)[] RewardTiers = new[]
	{
		(1, 10, 640080, 4),
		(11, 20, 640081, 3),
		(21, 30, 640082, 2),
		(31, 40, 640082, 4),
		(41, 50, 640084, 3),
		(51, 60, 640085, 3),
		(61, 70, 640086, 2),
		(71, 80, 640113, 2),
		(81, 999, 640114, 3),
	};

	// State
	private Mob _torch;
	private bool _isStarted;
	private TimeSpan _timeSinceLastWave;
	private int _currentWave;
	private readonly List<Mob> _spawnedMonsters;
	private readonly HashSet<int> _participatingPlayerIds;
	private readonly List<int> _availableMonsterIds;
	private int _monstersPerWave;
	private int _averageMonsterLevel;

	public DefendTheTorchInstance(Map map, Position position, Direction direction)
		: base(map, position, direction)
	{
		_spawnedMonsters = new List<Mob>();
		_participatingPlayerIds = new HashSet<int>();
		_availableMonsterIds = new List<int>();
		_isStarted = false;
		_timeSinceLastWave = TimeSpan.Zero;
		_currentWave = 0;
		_monstersPerWave = 3; // Default fallback
	}

	protected override void OnStart()
	{
		// Initialize available monsters from map spawners
		this.InitializeAvailableMonsters();

		// Spawn the torch as a monster
		_torch = new Mob(TorchMonsterId, RelationType.Neutral);
		_torch.Name = "Torch";
		_torch.Position = this.SpawnPosition;
		_torch.Direction = this.SpawnDirection;
		// Initially spawn as Neutral faction so nothing can attack it
		// It will change to Our_Forces when players enter activation range
		_torch.Faction = FactionType.Neutral;

		// Set torch HP from config
		// Note: HPCount property makes all damage dealt to the torch be exactly 1
		// and also sets the max HP to the HPCount value
		var torchMaxHp = ZoneServer.Instance.Conf.World.DefendTorchHp;
		var propertyOverrides = new PropertyOverrides();
		propertyOverrides.Add(PropertyName.HPCount, torchMaxHp); // Reduce all damage to 1 and set max HP
		_torch.ApplyOverrides(propertyOverrides);
		_torch.Properties.InvalidateAll();
		_torch.HealToFull();

		this.Map.AddMonster(_torch);

		// Subscribe to torch death event
		_torch.Died += this.OnTorchDestroyed;

		// Send a notice to nearby players
		var rewardRange = ZoneServer.Instance.Conf.World.DefendTorchRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"A Divine Torch has appeared! Get close to start defending it!", "Clear", 8);
	}

	public override void Update(TimeSpan elapsed)
	{
		// During waiting phase, check for player activation but don't accumulate time
		if (!_isStarted)
		{
			// Check if torch is still alive
			if (_torch == null || _torch.IsDead)
			{
				this.OnMinigameFailed();
				return;
			}

			// Check for nearby players to start the minigame
			var activationRange = ZoneServer.Instance.Conf.World.DefendTorchActivationRange;
			var nearbyPlayers = this.GetCharactersInRange(this.SpawnPosition, activationRange);
			if (nearbyPlayers.Count > 0)
			{
				this.StartMinigame();
				// Now that we've started, call the base Update to begin normal processing
				base.Update(elapsed);
			}
			return;
		}

		// Once started, use the base Update which will call OnUpdate
		base.Update(elapsed);
	}

	protected override void OnUpdate(TimeSpan elapsed)
	{
		if (_torch == null || _torch.IsDead)
		{
			this.OnMinigameFailed();
			return;
		}

		// Check if minigame duration has elapsed
		var minigameDuration = TimeSpan.FromMinutes(ZoneServer.Instance.Conf.World.DefendTorchDurationMinutes);
		if (this.ElapsedTime >= minigameDuration)
		{
			this.OnMinigameSuccess();
			return;
		}

		// Track participating players
		var rewardRange = ZoneServer.Instance.Conf.World.DefendTorchRewardRange;
		var currentNearbyPlayers = this.GetCharactersInRange(this.SpawnPosition, rewardRange);
		foreach (var player in currentNearbyPlayers)
		{
			_participatingPlayerIds.Add(player.Handle);
		}

		// Spawn waves
		_timeSinceLastWave += elapsed;
		var waveInterval = TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.DefendTorchWaveIntervalSeconds);
		if (_timeSinceLastWave >= waveInterval)
		{
			this.SpawnWave();
			_timeSinceLastWave = TimeSpan.Zero;
		}

		// Clean up dead monsters
		_spawnedMonsters.RemoveAll(m => m.IsDead);
	}

	private void StartMinigame()
	{
		_isStarted = true;

		// Activate the minigame so ElapsedTime starts accumulating
		this.IsActive = true;

		// Change torch faction to Our_Forces so monsters can now attack it
		// (Monster faction has Our_Forces in its hostile list)
		if (_torch != null && !_torch.IsDead)
		{
			_torch.Faction = FactionType.Our_Forces;
		}

		var rewardRange = ZoneServer.Instance.Conf.World.DefendTorchRewardRange;
		var durationMinutes = ZoneServer.Instance.Conf.World.DefendTorchDurationMinutes;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			string.Format("Defend the Torch! Monsters are coming! Hold them off for {0} minutes!", durationMinutes), "Clear", 8);
	}

	/// <summary>
	/// Initializes the list of available monsters from spawners in the map.
	/// Uses the same formula as SuperMonGenMob_Died to determine wave count.
	/// Only considers spawners with average spawn count >= configured threshold to avoid rare spawns.
	/// </summary>
	private void InitializeAvailableMonsters()
	{
		var worldConf = ZoneServer.Instance.Conf.World;
		var minAvgSpawnCount = 3f;

		// Get all spawners and filter by those that spawn on this map
		// Use spawner.Maps property instead of spawn areas to avoid issues after script reload
		var allSpawners = ZoneServer.Instance.World.GetSpawners().OfType<MonsterSpawner>();
		var spawners = allSpawners.Where(s => s.Maps.Contains(this.Map.Id)).ToList();

		if (spawners.Count > 0)
		{
			// Collect unique monster IDs from spawners with average spawn count >= threshold
			// to exclude rare spawns
			var monsterIds = new HashSet<int>();
			var totalLevel = 0;
			var monsterCount = 0;

			foreach (var spawner in spawners)
			{
				// Only consider spawners that spawn frequently
				var averageSpawnCount = (spawner.MinAmount + spawner.MaxAmount) / 2.0f;
				if (averageSpawnCount < minAvgSpawnCount)
					continue;

				// Exclude bosses, root crystals, and other non-normal monsters
				var rank = spawner.MonsterData.Rank;
				if (rank != MonsterRank.Normal && rank != MonsterRank.Elite)
					continue;

				var monsterId = spawner.MonsterData.Id;
				monsterIds.Add(monsterId);
				totalLevel += spawner.MonsterData.Level;
				monsterCount++;
			}

			_availableMonsterIds.AddRange(monsterIds);

			// Calculate average level
			var averageLevel = monsterCount > 0 ? totalLevel / monsterCount : 1;
			_averageMonsterLevel = averageLevel;

			// Wave formula
			var baseCount = worldConf.DefendTorchWaveMonsterCount;
			var levelModifier = Math.Min(1f, averageLevel / 100f);
			_monstersPerWave = (int)Math.Ceiling(baseCount * levelModifier);
			_monstersPerWave = Math.Max(3, _monstersPerWave);
		}

		// Fallback to hardcoded monsters if no spawners found
		if (_availableMonsterIds.Count == 0)
		{
			_availableMonsterIds.AddRange(new[]
			{
				MonsterId.Onion,
				MonsterId.Hanaming,
				MonsterId.InfroRocktor,
				MonsterId.Leaf_Diving,
			});
			_monstersPerWave = 3;
			_averageMonsterLevel = 1;
		}
	}

	private void SpawnWave()
	{
		_currentWave++;

		// Use calculated monsters per wave (based on average level of spawners)
		var monstersToSpawn = _monstersPerWave;

		// Use activation range from config for spawn distance
		var activationRange = ZoneServer.Instance.Conf.World.DefendTorchActivationRange;
		var baseSpawnDistance = activationRange * 0.75f; // Spawn at 75% of activation range
		var randomVariation = (int)(activationRange * 0.25f); // ±25% variation

		for (var i = 0; i < monstersToSpawn; i++)
		{
			// Find a valid spawn position around the torch
			var spawnPos = this.SpawnPosition;
			var foundValidPos = false;

			// Try 100 random positions with random directions from the torch
			for (var k = 1; k <= 100; k++)
			{
				var randomAngle = this.Rnd.Next(360) * Math.PI / 180;
				var randomDistance = baseSpawnDistance + this.Rnd.Next(randomVariation * 2);
				var randomX = this.SpawnPosition.X + (float)(Math.Cos(randomAngle) * randomDistance);
				var randomZ = this.SpawnPosition.Z + (float)(Math.Sin(randomAngle) * randomDistance);
				var randPos = new Position(randomX, this.SpawnPosition.Y, randomZ);

				if (this.Map.Ground.IsValidPosition(randPos))
				{
					spawnPos = randPos;
					foundValidPos = true;
					break;
				}
			}

			// Final fallback: spawn at torch position if no valid position found
			if (!foundValidPos)
			{
				spawnPos = this.SpawnPosition;
			}

			// Select random monster from available monsters
			var monsterId = _availableMonsterIds[this.Rnd.Next(_availableMonsterIds.Count)];
			var monster = new Mob(monsterId, RelationType.Enemy);
			monster.Position = spawnPos;
			monster.SpawnPosition = spawnPos;
			monster.Direction = new Direction(this.Rnd.Next(360));

			// Add required components for monster behavior
			monster.Components.Add(new MovementComponent(monster));
			monster.Components.Add(new LifeTimeComponent(monster, TimeSpan.FromMinutes(5)));

			// Add AI component - use monster's AI if available, otherwise use BasicMonster
			var monsterData = ZoneServer.Instance.Data.MonsterDb.Find(monsterId);
			if (monsterData != null && !string.IsNullOrEmpty(monsterData.AiName) && AiScript.Exists(monsterData.AiName))
				monster.Components.Add(new AiComponent(monster, monsterData.AiName));
			else
				monster.Components.Add(new AiComponent(monster, "BasicMonster"));

			// Make monsters aggressive towards the torch
			monster.Tendency = TendencyType.Aggressive;

			// Add monster to map first so AI starts running
			this.Map.AddMonster(monster);
			_spawnedMonsters.Add(monster);

			// IMPORTANT: Delay hate insertion to avoid it being cleared by the AI's initial ReturnHome routine
			// The ReturnHome routine clears all hate when it starts, so we need to wait for it to transition to Idle
			// before inserting hate for the torch
			Task.Delay(500).ContinueWith(_ =>
			{
				if (!monster.IsDead && !_torch.IsDead)
					monster.InsertHate(_torch, 150);
			});
		}

		// Notify players
		var minigameDuration = TimeSpan.FromMinutes(ZoneServer.Instance.Conf.World.DefendTorchDurationMinutes);
		var rewardRange = ZoneServer.Instance.Conf.World.DefendTorchRewardRange;
		var remainingTime = minigameDuration - this.ElapsedTime;
		var remainingMinutes = (int)remainingTime.TotalMinutes;
		var remainingSeconds = (int)(remainingTime.TotalSeconds % 60);

		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			string.Format("Wave {0}! Time remaining: {1}:{2:D2}", _currentWave, remainingMinutes, remainingSeconds), "scroll", 5);
	}

	private async void OnTorchDestroyed(Mob mob, ICombatEntity killer)
	{
		this.OnMinigameFailed();
	}

	private void OnMinigameSuccess()
	{
		var rewardRange = ZoneServer.Instance.Conf.World.DefendTorchRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"Victory! The Divine Torch has been defended!", "Clear", 10);

		// Give rewards to all participating players
		this.GiveRewards();

		// Clean up
		this.Cleanup();
		this.End();
	}

	private void OnMinigameFailed()
	{
		var rewardRange = ZoneServer.Instance.Conf.World.DefendTorchRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"Defeat! The Divine Torch has been destroyed...", "scroll", 10);

		// Clean up
		this.Cleanup();
		this.End();
	}

	/// <summary>
	/// Gets the appropriate reward tier for the given average monster level.
	/// Returns null if no tier is found.
	/// </summary>
	private (int ItemId, int Amount)? GetRewardTierForLevel(int averageLevel)
	{
		foreach (var tier in RewardTiers)
		{
			if (averageLevel >= tier.MinLevel && averageLevel <= tier.MaxLevel)
			{
				return (tier.ItemId, tier.Amount);
			}
		}

		// No tier found, return null
		return null;
	}

	private void GiveRewards()
	{
		var worldConf = ZoneServer.Instance.Conf.World;
		var rewardRange = worldConf.DefendTorchRewardRange;

		// Find the appropriate reward tier based on average monster level
		var rewardTier = this.GetRewardTierForLevel(_averageMonsterLevel);
		if (rewardTier == null)
			return;

		var rewardItemId = rewardTier.Value.ItemId;
		var rewardAmount = rewardTier.Value.Amount;

		// Get all participating players who are still near the torch
		var nearbyPlayers = this.GetCharactersInRange(this.SpawnPosition, rewardRange);

		foreach (var player in nearbyPlayers)
		{
			// Only reward players who actually participated
			if (_participatingPlayerIds.Contains(player.Handle))
			{
				try
				{
					// Add all items at once using Insert for efficiency (no pickup animation/notifications)
					player.Inventory.Add(rewardItemId, rewardAmount, InventoryAddType.PickUp);
					player.AddonMessage("NOTICE_Dm_Clear", string.Format("You received a reward for defending the torch!"), 8);
				}
				catch (Exception ex)
				{
					player.AddonMessage("NOTICE_Dm_scroll", "Failed to receive reward.", 5);
				}
			}
		}
	}

	private void Cleanup()
	{
		// Remove torch
		if (_torch != null && !_torch.IsDead)
		{
			this.Map.RemoveMonster(_torch);
		}

		_spawnedMonsters.Clear();
	}

	protected override void OnEnded()
	{
		this.Cleanup();
	}
}
