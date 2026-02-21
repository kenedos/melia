using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;

namespace Melia.Zone.World.Spawning
{
	/// <summary>
	/// Spawns and respawns monsters.
	/// </summary>
	public class EventMonsterSpawner : ISpawner
	{
		private const float MinDistanceBetweenBosses = 500f; // Minimum distance between bosses
		private const float MinDistanceFromWarps = 300f;    // Minimum distance from map entrances

		private const float FlexIncreaseLimit = 100;
		private const float FlexDecreaseLimit = -100;
		private const float FlexMeterDefault = 0;
		private const float FlexMeterIncreasePerDeath = 10;
		private const float FlexMeterDecreasePerSecond = 0.5f;
		private readonly static TimeSpan FlexSpawnInterval = TimeSpan.FromSeconds(5);

		private static int Ids = 100000000;

		private readonly MonsterData _monsterData;

		private bool _initialSpawnDone;
		private float _flexMeter = 0;
		private TimeSpan _flexSpawnDelay = TimeSpan.MaxValue;
		private readonly List<TimeSpan> _respawnDelays = new();
		private readonly Position[] _positions;

		private readonly Random _rnd = new(RandomProvider.GetSeed());

		/// <summary>
		/// Returns the unique id of this spawner.
		/// </summary>
		public int Id { get; }
		public int MapId { get; }

		/// <summary>
		/// Returns the min amount of monsters this spawner spawns at a time.
		/// </summary>
		public int MinAmount { get; }

		/// <summary>
		/// Returns the max amount of monsters this spawner spawns at a time.
		/// </summary>
		public int MaxAmount { get; }

		/// <summary>
		/// Returns the  amount of monsters this spawner currently spawns at
		/// a time. This number may change based on how frequently monsters
		/// are killed.
		/// </summary>
		public int FlexAmount { get; private set; }

		/// <summary>
		/// Returns the amount of monsters currently spawned.
		/// </summary>
		public int Amount { get; private set; }

		/// <summary>
		/// Returns the amount of monsters queued up for respawning.
		/// </summary>
		public int QueuedAmount { get { lock (_respawnDelays) return _respawnDelays.Count; } }

		/// <summary>
		/// Returns the initial delay before the first spawn.
		/// </summary>
		public TimeSpan InitialDelay { get; }

		/// <summary>
		/// Returns the minimum time until a respawn after a monster
		/// disappeared.
		/// </summary>
		public TimeSpan MinRespawnDelay { get; }

		/// <summary>
		/// Returns the maximum time until a respawn after a monster
		/// disappeared.
		/// </summary>
		public TimeSpan MaxRespawnDelay { get; }

		/// <summary>
		/// Returns the monster data currently being used for this spawner.
		/// </summary>
		public MonsterData MonsterData { get { return _monsterData; } }

		/// <summary>
		/// Returns the default tendency for monsters spawned by this spawner.
		/// </summary>
		public TendencyType Tendency { get; }

		/// <summary>
		/// Returns overrides for the spawn monsters' properties.
		/// </summary>
		public PropertyOverrides PropertyOverrides { get; set; }

		/// <summary>
		/// Raised for every monster the spawner creates, just before it's
		/// added to the world.
		/// </summary>
		public event EventHandler<SpawnEventArgs> Spawning;

		/// <summary>
		/// Raised for every monster the spawner creates, just after it was
		/// added to the world.
		/// </summary>
		public event EventHandler<SpawnEventArgs> Spawned;


		/// <summary>
		/// Creates a new monster spawner.
		/// </summary>
		/// <param name="monsterClassId">Id of the monsters spawned by this instance.</param>
		/// <param name="minAmount">The minimum amount of monsters to spawn at a time.</param>
		/// <param name="maxAmount">The maximum amount of monsters to spawn at a time.</param>
		/// <param name="initialSpawnDelay">The initial delay before the spawner starts spawning monsters.</param>
		/// <param name="minRespawnDelay">The minimum delay before a monster is respawned after death.</param>
		/// <param name="maxRespawnDelay">The maximum delay before a monster is respawned after death.</param>
		/// <param name="tendency">Defines the spawned monsters' aggressive tendencies.</param>
		/// <exception cref="ArgumentException"></exception>
		public EventMonsterSpawner(int monsterClassId, int mapId, int minAmount, int maxAmount, TimeSpan initialSpawnDelay, TimeSpan minRespawnDelay, TimeSpan maxRespawnDelay, TendencyType tendency, params Position[] positions)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterClassId, out _monsterData))
				throw new ArgumentException($"MonsterSpawner: No monster data found for '{monsterClassId}'.");

			minAmount = Math.Max(1, minAmount);
			maxAmount = Math.Max(minAmount, maxAmount);

			initialSpawnDelay = Math2.Max(TimeSpan.Zero, initialSpawnDelay);
			minRespawnDelay = Math2.Max(TimeSpan.Zero, minRespawnDelay);
			maxRespawnDelay = Math2.Max(TimeSpan.Zero, maxRespawnDelay);

			this.Id = Interlocked.Increment(ref Ids);
			this.MapId = mapId;
			this.MinAmount = minAmount;
			this.MaxAmount = maxAmount;
			this.FlexAmount = this.MinAmount;
			this.InitialDelay = initialSpawnDelay;
			this.MinRespawnDelay = minRespawnDelay;
			this.MaxRespawnDelay = maxRespawnDelay;
			this.Tendency = tendency;

			_flexSpawnDelay = this.InitialDelay;
			_positions = positions;
		}

		/// <summary>
		/// Initializes the population by setting current monster amount to zero
		/// </summary>
		public void InitializePopulation()
		{
			this.Amount = 0;
		}

		/// <summary>
		/// Spawns the given number of monsters in a random spawn area.
		/// </summary>
		/// <param name="amount"></param>
		public void Spawn(int amount)
		{
			for (var i = 0; i < amount; ++i)
			{
				if (!ZoneServer.Instance.World.Maps.TryGet(this.MapId, out var map))
					throw new InvalidOperationException($"Map '{0}' not found.");

				var monster = new Mob(_monsterData.Id, RelationType.Enemy);
				if (monster.Rank == MonsterRank.Boss)
				{
					monster.Position = this.GetSafeBossSpawnPosition(map);
				}
				else if (_positions != null && _positions.Length > 0)
				{
					monster.Position = _positions[_rnd.Next(_positions.Length)];
				}
				else
				{
					for (var j = 0; j < 100; j++)
					{
						if (map.Ground.TryGetRandomPosition(out var pos))
						{
							monster.Position = pos;
							break;
						}
					}
				}
				monster.FromGround = true;
				monster.Tendency = this.Tendency;
				monster.Died += this.OnMonsterDied;

				// Apply map overrides first
				this.OverrideProperties(monster, map);

				// Then apply boss rank overrides from conf file.
				if (monster.Rank == MonsterRank.Boss)
				{
					var worldConf = ZoneServer.Instance.Conf.World;
					var propertyOverrides = new PropertyOverrides();
					propertyOverrides.Add(PropertyName.MHP, monster.Properties.GetFloat(PropertyName.MHP) * worldConf.BossHPSPRate / 100f);
					propertyOverrides.Add(PropertyName.MSP, monster.Properties.GetFloat(PropertyName.MSP) * worldConf.BossHPSPRate / 100f);
					propertyOverrides.Add(PropertyName.MINPATK, monster.Properties.GetFloat(PropertyName.MINPATK) * worldConf.BossStatRate / 100f);
					propertyOverrides.Add(PropertyName.MAXPATK, monster.Properties.GetFloat(PropertyName.MAXPATK) * worldConf.BossStatRate / 100f);
					propertyOverrides.Add(PropertyName.MINMATK, monster.Properties.GetFloat(PropertyName.MINMATK) * worldConf.BossStatRate / 100f);
					propertyOverrides.Add(PropertyName.MAXMATK, monster.Properties.GetFloat(PropertyName.MAXMATK) * worldConf.BossStatRate / 100f);
					propertyOverrides.Add(PropertyName.DEF, monster.Properties.GetFloat(PropertyName.DEF) * worldConf.BossStatRate / 100f);
					propertyOverrides.Add(PropertyName.MDEF, monster.Properties.GetFloat(PropertyName.MDEF) * worldConf.BossStatRate / 100f);

					monster.ApplyOverrides(propertyOverrides);
					monster.InvalidateProperties();
					monster.Properties.SetFloat(PropertyName.HP, monster.MaxHp);
				}

				monster.Components.Add(new MovementComponent(monster));

				if (!string.IsNullOrWhiteSpace(monster.Data.AiName) && monster.Data.AiName != "None")
				{
					var aiName = monster.Data.AiName;
					if (!AiScript.Exists(aiName))
						aiName = "BasicMonster";
					monster.Components.Add(new AiComponent(monster, aiName));
				}

				this.Spawning?.Invoke(this, new SpawnEventArgs(this, monster));

				map.AddMonster(monster);
				monster.SpawnPosition = monster.Position;
				monster.PossiblyBecomeRare();

				this.Spawned?.Invoke(this, new SpawnEventArgs(this, monster));
			}

			this.Amount += amount;
		}

		/// <summary>
		/// Overrides monster's properties with the ones defined for
		/// this spawner or on the map.
		/// </summary>
		/// <remarks>
		/// The overrides on the spawner take precedence over the ones
		/// on the maps.
		/// </remarks>
		/// <param name="monster">The monster to override properties on.</param>
		/// <param name="map">The map to check for overrides.</param>
		private void OverrideProperties(Mob monster, Map map)
		{
			// Don't override anything if the feature is disabled
			if (!Feature.IsEnabled("SpawnPropertyOverrides"))
				return;

			// Check for overrides defined for this spawner first,
			// if there are none, check the overrides for the
			// map the spawner is on.
			var propertyOverrides = this.PropertyOverrides;
			if (propertyOverrides == null && !map.TryGetPropertyOverrides(_monsterData.Id, out propertyOverrides))
				return;

			monster.ApplyOverrides(propertyOverrides);
		}

		/// <summary>
		/// Called when one of the monsters this instance spawned died and
		/// disappeared.
		/// </summary>
		/// <param name="monster"></param>
		/// <param name="killer"></param>
		private void OnMonsterDied(Mob monster, ICombatEntity killer)
		{
			this.Amount--;
			_flexMeter += FlexMeterIncreasePerDeath;

			lock (_respawnDelays)
				_respawnDelays.Add(this.GetRandomRespawnDelay());
		}

		/// <summary>
		/// (Re)spawns monsters regularly.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.RespawnMonsters(elapsed);
			this.FlexSpawnMonsters(elapsed);
			this.BalanceSpawnAmounts(elapsed);
		}

		/// <summary>
		/// Respawns monsters that have been killed up until the current
		/// flex amount.
		/// </summary>
		/// <param name="elapsed"></param>
		private void RespawnMonsters(TimeSpan elapsed)
		{
			int expiredDelayCount;

			lock (_respawnDelays)
			{
				for (var i = 0; i < _respawnDelays.Count; ++i)
				{
					var spawnDelay = _respawnDelays[i];
					_respawnDelays[i] = spawnDelay - elapsed;
				}

				expiredDelayCount = _respawnDelays.Count(d => d <= TimeSpan.Zero);
				if (expiredDelayCount == 0)
					return;

				_respawnDelays.RemoveAll(d => d <= TimeSpan.Zero);
			}

			var spawnAmount = Math.Min(expiredDelayCount, this.FlexAmount - this.Amount);
			if (spawnAmount <= 0)
				return;

			this.Spawn(spawnAmount);
		}

		/// <summary>
		/// Spawns monsters until the current flex amount is reached.
		/// </summary>
		/// <param name="elapsed"></param>
		private void FlexSpawnMonsters(TimeSpan elapsed)
		{
			_flexSpawnDelay -= elapsed;
			if (_flexSpawnDelay > TimeSpan.Zero)
				return;

			_flexSpawnDelay = FlexSpawnInterval;

			var targetAmount = this.FlexAmount;
			var currentAmount = this.Amount;
			var queuedAmount = this.QueuedAmount;

			var potentialSpawnAmount = Math.Max(0, targetAmount - currentAmount - queuedAmount);
			if (potentialSpawnAmount > 0)
			{
				var spawnAmount = 1;

				// Spawn the full amount on the first flex spawn to get up
				// to the flex amount without delay.
				if (!_initialSpawnDone)
				{
					spawnAmount = potentialSpawnAmount;
					_initialSpawnDone = true;
				}

				this.Spawn(spawnAmount);
			}
		}

		/// <summary>
		/// Increases or decreases flex spawn amount based on the
		/// killing activity.
		/// </summary>
		/// <param name="elapsed"></param>
		private void BalanceSpawnAmounts(TimeSpan elapsed)
		{
			// Drain flex meter over time
			_flexMeter -= (float)(elapsed.TotalSeconds * FlexMeterDecreasePerSecond);

			// If the flex meter reached the increase limit, we increase
			// the spawn amount.
			if (_flexMeter > FlexIncreaseLimit)
			{
				this.FlexAmount = Math.Min(this.MaxAmount, this.FlexAmount + 1);
				_flexMeter = FlexMeterDefault;
			}
			// If the meter instead fell below the decrease limit, the
			// spawn amount decreases.
			else if (_flexMeter < FlexDecreaseLimit)
			{
				this.FlexAmount = Math.Max(this.MinAmount, this.FlexAmount - 1);
				_flexMeter = FlexMeterDefault;
			}
		}

		/// <summary>
		/// Returns a random delay between min and max respawn delay.
		/// </summary>
		/// <returns></returns>
		private TimeSpan GetRandomRespawnDelay()
			=> _rnd.Between(this.MinRespawnDelay, this.MaxRespawnDelay);

		/// <summary>
		/// Finds a safe spawn position for bosses, avoiding other bosses and warps.
		/// </summary>
		/// <param name="map">The map to spawn on.</param>
		/// <returns>A position that's safe for boss spawning.</returns>
		private Position GetSafeBossSpawnPosition(Map map)
		{
			var maxAttempts = 200; // Increase attempts for boss spawning
			var warpPositions = map.GetWarps().Select(w => w.Position).ToList();

			for (var attempt = 0; attempt < maxAttempts; attempt++)
			{
				if (!map.Ground.TryGetRandomPosition(out var candidatePosition))
					continue;

				// Check distance from warps
				var tooCloseToWarp = warpPositions.Any(warpPos =>
					candidatePosition.Get3DDistance(warpPos) < MinDistanceFromWarps);

				if (tooCloseToWarp)
					continue;

				// Check distance from other bosses
				var tooCloseToBoss = false;
				foreach (var existingMonster in map.GetMonsters())
				{
					if (existingMonster is Mob mob && mob.Rank == MonsterRank.Boss
						&& candidatePosition.Get3DDistance(existingMonster.Position) < MinDistanceBetweenBosses)
					{
						tooCloseToBoss = true;
						break;
					}
				}

				if (!tooCloseToBoss)
					return candidatePosition;
			}

			// Fallback: if we can't find a good position, use random position
			// This prevents infinite loops in crowded maps
			if (map.Ground.TryGetRandomPosition(out var fallbackPosition))
				return fallbackPosition;

			// Last resort: return default position
			return new Position(0, 0, 0);
		}
	}
}
