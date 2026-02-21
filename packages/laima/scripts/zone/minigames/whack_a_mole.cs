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
/// Whack-a-Mole minigame.
/// Players must hunt moles that randomly appear and move between burrow spots.
/// </summary>
public class WhackAMoleScript : IMinigameScript
{
	public IMinigameInstance CreateInstance(Map map, Position position, Direction direction)
	{
		return new WhackAMoleInstance(map, position, direction);
	}
}

/// <summary>
/// Represents a cluster of burrow holes.
/// </summary>
public class BurrowCluster
{
	public List<Position> Burrows { get; set; }
	public List<Position> ActiveBurrows { get; set; }
	public Position Center { get; set; }
	public TimeSpan TimeSinceLastShift { get; set; }

	public BurrowCluster()
	{
		Burrows = new List<Position>();
		ActiveBurrows = new List<Position>();
		TimeSinceLastShift = TimeSpan.Zero;
	}
}

public class WhackAMoleInstance : MinigameBase
{
	// Configuration (constants)
	private const int MoleMonsterId = 57459; // Mole monster ID
	private const int BurrowNpcId = 155003; // Burrow spot NPC ID

	// Reward Tiers Configuration (same as other minigames)
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
	private readonly List<BurrowCluster> _clusters;
	private readonly Dictionary<Position, Npc> _burrowNpcs;
	private readonly Dictionary<Mob, BurrowCluster> _moleClusterMap;
	private readonly List<Mob> _activeMoles;
	private readonly List<Mob> _spawnedMonsters;
	private readonly HashSet<int> _participatingPlayerIds;
	private readonly List<int> _availableMonsterIds;
	private int _averageMonsterLevel;
	private int _molesKilled;
	private TimeSpan _timeSinceLastMoleSpawn;
	private TimeSpan _timeSinceLastMonsterWave;
	private bool _isStarted;
	private bool _isCompleted;
	private int _targetClusters;
	private bool _enableClusterShifting;

	public WhackAMoleInstance(Map map, Position position, Direction direction)
		: base(map, position, direction)
	{
		_clusters = new List<BurrowCluster>();
		_burrowNpcs = new Dictionary<Position, Npc>();
		_moleClusterMap = new Dictionary<Mob, BurrowCluster>();
		_activeMoles = new List<Mob>();
		_spawnedMonsters = new List<Mob>();
		_participatingPlayerIds = new HashSet<int>();
		_availableMonsterIds = new List<int>();
		_molesKilled = 0;
		_timeSinceLastMoleSpawn = TimeSpan.Zero;
		_timeSinceLastMonsterWave = TimeSpan.Zero;
		_isStarted = false;
		_isCompleted = false;
		_targetClusters = 0;
		_enableClusterShifting = false;
	}

	protected override void OnStart()
	{
		// Send initial notice - tell players to get close to start
		var rewardRange = ZoneServer.Instance.Conf.World.WhackAMoleRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"A Mole Den has appeared! Get close to start the Whack-a-Mole challenge!", "Clear", 8);
	}

	public override void Update(TimeSpan elapsed)
	{
		// During waiting phase, check for player activation but don't accumulate time
		if (!_isStarted)
		{
			// Check for nearby players to start the minigame
			var activationRange = ZoneServer.Instance.Conf.World.WhackAMoleActivationRange;
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

	private void StartMinigame()
	{
		_isStarted = true;

		// Activate the minigame so ElapsedTime starts accumulating
		this.IsActive = true;

		// Initialize available monsters from map spawners
		// This is done here (not in OnStart) to ensure monster spawners are loaded
		// Important for script reload - minigame spawner might load before monster spawners
		this.InitializeAvailableMonsters();
		this.InitializeBurrowClusters();

		var rewardRange = ZoneServer.Instance.Conf.World.WhackAMoleRewardRange;
		var durationMinutes = ZoneServer.Instance.Conf.World.WhackAMoleDurationMinutes;
		var requiredKills = ZoneServer.Instance.Conf.World.WhackAMoleRequiredKills;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			string.Format("Whack-a-Mole Challenge! Kill {0} moles in {1} minutes!", requiredKills, durationMinutes),
			"Clear", 8);
	}

	protected override void OnUpdate(TimeSpan elapsed)
	{
		if (_isCompleted)
		{
			this.End();
			return;
		}

		// Check if minigame duration has elapsed
		var minigameDuration = TimeSpan.FromMinutes(ZoneServer.Instance.Conf.World.WhackAMoleDurationMinutes);
		if (this.ElapsedTime >= minigameDuration)
		{
			this.OnMinigameFailed();
			return;
		}

		// Track participating players
		var rewardRange = ZoneServer.Instance.Conf.World.WhackAMoleRewardRange;
		var currentNearbyPlayers = this.GetCharactersInRange(this.SpawnPosition, rewardRange);
		foreach (var player in currentNearbyPlayers)
		{
			_participatingPlayerIds.Add(player.Handle);
		}

		// Update cluster positions - each cluster shifts independently at random intervals
		if (_enableClusterShifting)
		{
			var burrowShiftInterval = TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.WhackAMoleBurrowShiftIntervalSeconds);
			foreach (var cluster in _clusters.ToArray())
			{
				cluster.TimeSinceLastShift += elapsed;
				if (cluster.TimeSinceLastShift >= burrowShiftInterval)
				{
					this.ShiftCluster(cluster);
					// Reset timer with random offset so clusters don't all shift at exact same time
					// Use negative offset to start counting from a bit before zero
					cluster.TimeSinceLastShift = TimeSpan.FromMilliseconds(-this.Rnd.Next(0, 3000));
				}
			}
		}

		// Spawn moles
		_timeSinceLastMoleSpawn += elapsed;
		var moleSpawnInterval = TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.WhackAMoleMoleSpawnIntervalSeconds);
		if (_timeSinceLastMoleSpawn >= moleSpawnInterval)
		{
			this.SpawnMole();
			_timeSinceLastMoleSpawn = TimeSpan.Zero;
		}

		// Update moles
		this.UpdateMoles();

		// Spawn monster waves
		_timeSinceLastMonsterWave += elapsed;
		var monsterWaveInterval = TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.WhackAMoleMonsterWaveIntervalSeconds);
		if (_timeSinceLastMonsterWave >= monsterWaveInterval && currentNearbyPlayers.Count > 0)
		{
			this.SpawnMonsterWave();
			_timeSinceLastMonsterWave = TimeSpan.Zero;
		}

		// Clean up dead monsters and moles
		_spawnedMonsters.RemoveAll(m => m.IsDead);
		_activeMoles.RemoveAll(m => m.IsDead);

		// Check win condition
		var requiredKills = ZoneServer.Instance.Conf.World.WhackAMoleRequiredKills;
		if (_molesKilled >= requiredKills)
		{
			this.OnMinigameSuccess();
		}
	}

	private void InitializeAvailableMonsters()
	{
		// Get all spawners and filter by those that spawn on this map
		// Use spawner.Maps property instead of spawn areas to avoid issues after script reload
		var allSpawners = ZoneServer.Instance.World.GetSpawners().OfType<MonsterSpawner>();
		var spawners = allSpawners.Where(s => s.Maps.Contains(this.Map.Id)).ToList();

		if (spawners.Count > 0)
		{
			var monsterIds = new HashSet<int>();
			var totalLevel = 0;
			var monsterCount = 0;

			foreach (var spawner in spawners)
			{
				// Only consider spawners that spawn frequently (avoid rare spawns)
				var averageSpawnCount = (spawner.MinAmount + spawner.MaxAmount) / 2.0f;
				if (averageSpawnCount < 3.0f)
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
			_averageMonsterLevel = monsterCount > 0 ? totalLevel / monsterCount : 1;
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
			_averageMonsterLevel = 1;
		}
	}

	private void InitializeBurrowClusters()
	{
		// Determine number of clusters based on average monster level
		if (_averageMonsterLevel <= 30)
		{
			_targetClusters = 5;
			_enableClusterShifting = false;
		}
		else if (_averageMonsterLevel <= 40)
		{
			_targetClusters = 6;
			_enableClusterShifting = false;
		}
		else if (_averageMonsterLevel <= 60)
		{
			_targetClusters = 7;
			_enableClusterShifting = true;
		}
		else
		{
			_targetClusters = 8;
			_enableClusterShifting = true;
		}

		var burrowRange = ZoneServer.Instance.Conf.World.WhackAMoleBurrowRange;
		var minClusterDistance = 70f; // Minimum distance between cluster centers

		// Try to create target number of clusters
		for (var i = 0; i < _targetClusters; i++)
		{
			var cluster = this.CreateCluster(burrowRange, minClusterDistance);
			if (cluster != null)
			{
				_clusters.Add(cluster);
			}
		}
	}

	private BurrowCluster CreateCluster(float burrowRange, float minClusterDistance)
	{
		// Find a center position for the cluster
		Position clusterCenter = this.SpawnPosition;
		var foundValidCenter = false;

		for (var attempt = 0; attempt < 100; attempt++)
		{
			var randomAngle = this.Rnd.Next(360) * Math.PI / 180;
			var randomDistance = (float)this.Rnd.Next(50, (int)burrowRange + 1);
			var randomX = this.SpawnPosition.X + (float)(Math.Cos(randomAngle) * randomDistance);
			var randomZ = this.SpawnPosition.Z + (float)(Math.Sin(randomAngle) * randomDistance);
			var randPos = new Position(randomX, this.SpawnPosition.Y, randomZ);

			// Check if position is valid
			if (!this.Map.Ground.IsValidPosition(randPos))
				continue;

			// Check if too close to other cluster centers
			var tooCloseToOtherCluster = false;
			foreach (var existingCluster in _clusters)
			{
				if (randPos.InRange2D(existingCluster.Center, minClusterDistance))
				{
					tooCloseToOtherCluster = true;
					break;
				}
			}

			if (!tooCloseToOtherCluster)
			{
				clusterCenter = randPos;
				foundValidCenter = true;
				break;
			}
		}

		if (!foundValidCenter)
			return null; // Couldn't find a valid cluster center

		// Create the cluster and try to add 2-4 burrows
		var cluster = new BurrowCluster { Center = clusterCenter };
		var targetBurrows = this.Rnd.Next(2, 5); // 2 to 4 burrows
		var minDistanceBetweenBurrows = 70f;
		var maxDistanceBetweenBurrows = 80f;

		for (var i = 0; i < targetBurrows; i++)
		{
			var foundValidPos = false;
			var burrowPos = clusterCenter;

			for (var k = 0; k < 100; k++)
			{
				var randomAngle = this.Rnd.Next(360) * Math.PI / 180;
				var randomDistance = (float)this.Rnd.Next(0, 60); // Within 60 units of cluster center
				var randomX = clusterCenter.X + (float)(Math.Cos(randomAngle) * randomDistance);
				var randomZ = clusterCenter.Z + (float)(Math.Sin(randomAngle) * randomDistance);
				var randPos = new Position(randomX, clusterCenter.Y, randomZ);

				// Check if position is valid
				if (!this.Map.Ground.IsValidPosition(randPos))
					continue;

				// Check distance to existing burrows in this cluster (should be 70-90 units)
				var tooClose = false;
				var tooFar = false;

				foreach (var existingBurrow in cluster.Burrows)
				{
					if (randPos.InRange2D(existingBurrow, minDistanceBetweenBurrows))
					{
						tooClose = true;
						break;
					}
					if (!randPos.InRange2D(existingBurrow, maxDistanceBetweenBurrows))
					{
						tooFar = true;
						break;
					}
				}

				// For the first burrow in cluster, accept any valid position
				if (cluster.Burrows.Count == 0 || (!tooClose && !tooFar))
				{
					burrowPos = randPos;
					foundValidPos = true;
					break;
				}
			}

			if (foundValidPos)
			{
				cluster.Burrows.Add(burrowPos);
			}
		}

		// Only return the cluster if it has at least 2 burrows
		if (cluster.Burrows.Count < 2)
			return null;

		// Initially all burrows are active
		cluster.ActiveBurrows.AddRange(cluster.Burrows);

		// Spawn NPC markers at each burrow
		foreach (var burrowPos in cluster.Burrows)
		{
			this.SpawnBurrowNpc(burrowPos);
		}

		return cluster;
	}

	private void SpawnBurrowNpc(Position position)
	{
		// Create burrow spot NPC marker
		var burrowNpc = AddNpc(BurrowNpcId, "Burrow", this.Map.ClassName, position.X, position.Z, this.Rnd.Next(360));

		// Set as neutral so it can't be attacked
		burrowNpc.Faction = FactionType.Neutral;

		// Add to tracking dictionary
		_burrowNpcs[position] = burrowNpc;
	}

	private void RemoveBurrowNpc(Position position)
	{
		if (_burrowNpcs.TryGetValue(position, out var npc))
		{
			this.Map.RemoveMonster(npc);
			_burrowNpcs.Remove(position);
		}
	}

	private void ShiftCluster(BurrowCluster cluster)
	{
		// Check if there are any alive moles in this cluster
		var hasMolesInCluster = _moleClusterMap.Any(kvp => kvp.Value == cluster && !kvp.Key.IsDead);
		if (hasMolesInCluster)
		{
			// Don't shift this cluster yet, reset timer with small offset
			cluster.TimeSinceLastShift = TimeSpan.FromMilliseconds(-this.Rnd.Next(0, 2000));
			return;
		}

		// Remove all burrow NPCs in this cluster
		foreach (var burrowSpot in cluster.Burrows)
		{
			this.RemoveBurrowNpc(burrowSpot);
		}

		// Clear all burrow positions
		cluster.Burrows.Clear();
		cluster.ActiveBurrows.Clear();

		// Find a new center position for the cluster
		var burrowRange = ZoneServer.Instance.Conf.World.WhackAMoleBurrowRange;
		var minClusterDistance = 70f;
		Position newClusterCenter = this.SpawnPosition;
		var foundValidCenter = false;

		for (var attempt = 0; attempt < 100; attempt++)
		{
			var randomAngle = this.Rnd.Next(360) * Math.PI / 180;
			var randomDistance = (float)this.Rnd.Next(50, (int)burrowRange + 1);
			var randomX = this.SpawnPosition.X + (float)(Math.Cos(randomAngle) * randomDistance);
			var randomZ = this.SpawnPosition.Z + (float)(Math.Sin(randomAngle) * randomDistance);
			var randPos = new Position(randomX, this.SpawnPosition.Y, randomZ);

			// Check if position is valid
			if (!this.Map.Ground.IsValidPosition(randPos))
				continue;

			// Check if too close to other cluster centers
			var tooCloseToOtherCluster = false;
			foreach (var existingCluster in _clusters)
			{
				if (existingCluster == cluster)
					continue; // Skip self

				if (randPos.InRange2D(existingCluster.Center, minClusterDistance))
				{
					tooCloseToOtherCluster = true;
					break;
				}
			}

			if (!tooCloseToOtherCluster)
			{
				newClusterCenter = randPos;
				foundValidCenter = true;
				break;
			}
		}

		if (!foundValidCenter)
			return; // Couldn't find a valid new center, keep cluster hidden

		// Update cluster center
		cluster.Center = newClusterCenter;

		// Create new burrows at the new cluster location
		var targetBurrows = this.Rnd.Next(2, 5); // 2 to 4 burrows
		var minDistanceBetweenBurrows = 50f;
		var maxDistanceBetweenBurrows = 60f;

		for (var i = 0; i < targetBurrows; i++)
		{
			var foundValidPos = false;
			Position burrowPos = newClusterCenter;

			for (var k = 0; k < 100; k++)
			{
				var randomAngle = this.Rnd.Next(360) * Math.PI / 180;
				var randomDistance = (float)this.Rnd.Next(0, 60); // Within 60 units of cluster center
				var randomX = newClusterCenter.X + (float)(Math.Cos(randomAngle) * randomDistance);
				var randomZ = newClusterCenter.Z + (float)(Math.Sin(randomAngle) * randomDistance);
				var randPos = new Position(randomX, newClusterCenter.Y, randomZ);

				// Check if position is valid
				if (!this.Map.Ground.IsValidPosition(randPos))
					continue;

				// Check distance to existing burrows in this cluster (should be 50-60 units)
				var tooClose = false;
				var tooFar = false;

				foreach (var existingBurrow in cluster.Burrows)
				{
					if (randPos.InRange2D(existingBurrow, minDistanceBetweenBurrows))
					{
						tooClose = true;
						break;
					}
					if (!randPos.InRange2D(existingBurrow, maxDistanceBetweenBurrows))
					{
						tooFar = true;
						break;
					}
				}

				// For the first burrow in cluster, accept any valid position
				if (cluster.Burrows.Count == 0 || (!tooClose && !tooFar))
				{
					burrowPos = randPos;
					foundValidPos = true;
					break;
				}
			}

			if (foundValidPos)
			{
				cluster.Burrows.Add(burrowPos);
			}
		}

		// Only keep the cluster if it has at least 2 burrows
		if (cluster.Burrows.Count < 2)
			return;

		// All burrows are active after shift
		cluster.ActiveBurrows.AddRange(cluster.Burrows);

		// Spawn NPC markers at each burrow
		foreach (var burrowPos in cluster.Burrows)
		{
			this.SpawnBurrowNpc(burrowPos);
		}
	}

	private void SpawnMole()
	{
		// Only spawn if there are clusters with active burrows
		var validClusters = _clusters.Where(c => c.ActiveBurrows.Count >= 2).ToList();
		if (validClusters.Count == 0)
			return;

		// Select a random cluster
		var cluster = validClusters[this.Rnd.Next(validClusters.Count)];

		// Select a random burrow spot to spawn the mole
		var startSpot = cluster.ActiveBurrows[this.Rnd.Next(cluster.ActiveBurrows.Count)];

		// Create the mole
		var mole = new Mob(MoleMonsterId, RelationType.Enemy);
		mole.Name = "Mole";
		mole.Position = startSpot;
		mole.SpawnPosition = startSpot;
		mole.Direction = new Direction(this.Rnd.Next(360));

		// Apply property overrides to make moles take 1 damage and have 1 HP
		var moleHp = 1;
		var propertyOverrides = new PropertyOverrides();
		propertyOverrides.Add(PropertyName.HPCount, moleHp);
		propertyOverrides.Add(PropertyName.DR_BM, -9999);
		mole.ApplyOverrides(propertyOverrides);
		mole.Properties.InvalidateAll();
		mole.HealToFull();

		// Add components
		mole.Components.Add(new MovementComponent(mole));
		mole.Components.Add(new LifeTimeComponent(mole, TimeSpan.FromMinutes(2)));

		// Subscribe to mole death event
		mole.Died += this.OnMoleDied;

		// Make mole emerge from ground
		mole.FromGround = true;

		// Add mole to map
		this.Map.AddMonster(mole);
		_activeMoles.Add(mole);

		// Track which cluster this mole belongs to
		_moleClusterMap[mole] = cluster;

		// Store the spawn burrow position so we can exclude it from disappear checks
		mole.Vars.Set("Melia.SpawnBurrow", startSpot);

		// Make the mole move to another burrow spot in the same cluster
		this.MoveMoleToRandomBurrow(mole, cluster, startSpot);
	}

	private void MoveMoleToRandomBurrow(Mob mole, BurrowCluster cluster, Position currentSpot)
	{
		// Select a different burrow spot in the same cluster as destination
		var availableSpots = cluster.ActiveBurrows.Where(s => s != currentSpot).ToList();
		if (availableSpots.Count == 0)
			return;

		var targetSpot = availableSpots[this.Rnd.Next(availableSpots.Count)];

		// Use MovementComponent to move the mole
		if (mole.Components.TryGet<MovementComponent>(out var movement))
		{
			movement.MoveStraight(targetSpot);
		}
	}

	private void UpdateMoles()
	{
		var disappearDistance = 10f; // Distance threshold for mole to disappear

		foreach (var mole in _activeMoles.ToArray())
		{
			if (mole.IsDead)
				continue;

			// Get the cluster this mole belongs to
			if (!_moleClusterMap.TryGetValue(mole, out var cluster))
				continue;

			// Get the spawn burrow to exclude from disappear check
			var hasSpawnBurrow = mole.Vars.TryGet<Position>("Melia.SpawnBurrow", out var spawnBurrow);

			// Check if mole is close to any burrow spot in its cluster (except spawn burrow)
			foreach (var burrowSpot in cluster.ActiveBurrows)
			{
				// Skip if this is the spawn burrow (within 10 units of spawn position)
				if (hasSpawnBurrow && burrowSpot.InRange2D(spawnBurrow, 10f))
					continue;

				// Check if mole reached this burrow spot
				if (mole.Position.InRange2D(burrowSpot, disappearDistance))
				{
					// Mole reached a burrow spot, make it disappear immediately
					if (!mole.IsDead)
					{
						mole.DisappearTime = DateTime.Now;
					}
					_activeMoles.Remove(mole);
					_moleClusterMap.Remove(mole);
					break;
				}
			}
		}
	}

	private void OnMoleDied(Mob mob, ICombatEntity killer)
	{
		_molesKilled++;

		// Remove from cluster map
		_moleClusterMap.Remove(mob);

		// Notify players
		var rewardRange = ZoneServer.Instance.Conf.World.WhackAMoleRewardRange;
		var requiredKills = ZoneServer.Instance.Conf.World.WhackAMoleRequiredKills;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			string.Format("Mole whacked! ({0}/{1})", _molesKilled, requiredKills),
			"Clear", 3);
	}

	private void SpawnMonsterWave()
	{
		// Spawn 2~5 monsters near random burrow spots
		var monstersToSpawn = this.Rnd.Next(2, 6);

		for (var i = 0; i < monstersToSpawn; i++)
		{
			// Select a random cluster and burrow
			if (_clusters.Count == 0)
				continue;

			var cluster = _clusters[this.Rnd.Next(_clusters.Count)];
			if (cluster.ActiveBurrows.Count == 0)
				continue;

			var randomBurrow = cluster.ActiveBurrows[this.Rnd.Next(cluster.ActiveBurrows.Count)];

			// Find a position near the burrow spot
			var spawnPos = randomBurrow;
			var foundValidPos = false;
			var monsterSpawnDistance = (int)ZoneServer.Instance.Conf.World.WhackAMoleMonsterSpawnDistance;

			for (var k = 0; k < 100; k++)
			{
				var randomAngle = this.Rnd.Next(360) * Math.PI / 180;
				var randomDistance = (float)this.Rnd.Next(monsterSpawnDistance, monsterSpawnDistance + 20);
				var randomX = randomBurrow.X + (float)(Math.Cos(randomAngle) * randomDistance);
				var randomZ = randomBurrow.Z + (float)(Math.Sin(randomAngle) * randomDistance);
				var randPos = new Position(randomX, randomBurrow.Y, randomZ);

				if (this.Map.Ground.IsValidPosition(randPos))
				{
					spawnPos = randPos;
					foundValidPos = true;
					break;
				}
			}

			if (!foundValidPos)
			{
				spawnPos = randomBurrow;
			}

			// Select random monster from available monsters
			var monsterId = _availableMonsterIds[this.Rnd.Next(_availableMonsterIds.Count)];
			var monster = new Mob(monsterId, RelationType.Enemy);
			monster.Position = spawnPos;
			monster.SpawnPosition = spawnPos;
			monster.Direction = new Direction(this.Rnd.Next(360));

			// Add required components
			monster.Components.Add(new MovementComponent(monster));
			monster.Components.Add(new LifeTimeComponent(monster, TimeSpan.FromMinutes(5)));

			// Add AI component
			var monsterData = ZoneServer.Instance.Data.MonsterDb.Find(monsterId);
			if (monsterData != null && !string.IsNullOrEmpty(monsterData.AiName) && AiScript.Exists(monsterData.AiName))
				monster.Components.Add(new AiComponent(monster, monsterData.AiName));
			else
				monster.Components.Add(new AiComponent(monster, "BasicMonster"));

			// Make monsters aggressive
			monster.Tendency = TendencyType.Aggressive;

			this.Map.AddMonster(monster);
			_spawnedMonsters.Add(monster);
		}
	}

	private void OnMinigameSuccess()
	{
		_isCompleted = true;

		var rewardRange = ZoneServer.Instance.Conf.World.WhackAMoleRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"Victory! You've successfully whacked all the moles!", "Clear", 10);

		// Give rewards
		this.GiveRewards();

		// Clean up
		this.Cleanup();
		this.End();
	}

	private void OnMinigameFailed()
	{
		_isCompleted = true;

		var rewardRange = ZoneServer.Instance.Conf.World.WhackAMoleRewardRange;
		this.SendNoticeInRange(this.SpawnPosition, rewardRange,
			"Time's up! The moles have escaped...", "scroll", 10);

		// Clean up
		this.Cleanup();
		this.End();
	}

	private (int ItemId, int Amount)? GetRewardTierForLevel(int averageLevel)
	{
		foreach (var tier in RewardTiers)
		{
			if (averageLevel >= tier.MinLevel && averageLevel <= tier.MaxLevel)
			{
				return (tier.ItemId, tier.Amount);
			}
		}
		return null;
	}

	private void GiveRewards()
	{
		var rewardRange = ZoneServer.Instance.Conf.World.WhackAMoleRewardRange;

		// Find the appropriate reward tier
		var rewardTier = this.GetRewardTierForLevel(_averageMonsterLevel);
		if (rewardTier == null)
			return;

		var rewardItemId = rewardTier.Value.ItemId;
		var rewardAmount = rewardTier.Value.Amount;

		// Get all participating players who are still near
		var nearbyPlayers = this.GetCharactersInRange(this.SpawnPosition, rewardRange);

		foreach (var player in nearbyPlayers)
		{
			if (_participatingPlayerIds.Contains(player.Handle))
			{
				try
				{
					player.Inventory.Add(rewardItemId, rewardAmount, InventoryAddType.PickUp);
					player.AddonMessage("NOTICE_Dm_Clear", "You received a reward for completing the challenge!", 8);
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
		// Remove all active moles
		foreach (var mole in _activeMoles)
		{
			if (!mole.IsDead)
			{
				this.Map.RemoveMonster(mole);
			}
		}
		_activeMoles.Clear();
		_moleClusterMap.Clear();

		// Remove all burrow NPCs
		foreach (var npc in _burrowNpcs.Values)
		{
			this.Map.RemoveMonster(npc);
		}
		_burrowNpcs.Clear();

		// Spawned monsters will expire on their own via LifeTimeComponent
		_spawnedMonsters.Clear();
	}

	protected override void OnEnded()
	{
		this.Cleanup();
	}
}
