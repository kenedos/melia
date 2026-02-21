using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Maps;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using Yggdrasil.Geometry;

namespace Melia.Zone.World.Spawning
{
	/// <summary>
	/// Spawns treasure chests in a list of TreasureSpawnPointData.
	/// </summary>
	public class TreasureChestSpawner : ISpawner
	{
		private static int Ids = 1000000000;
		private static int ChestIds = 0;
		private static readonly object LockObj = new();
		private static readonly Dictionary<string, HashSet<Position>> UsedPositions = new();
		private const float MinSpawnDistance = 5.0f;
		private const int MAX_SPAWN_ATTEMPTS = 10;

		private readonly TreasureDropDb _dropDb;
		private readonly string _baseUniqueName;
		private readonly TreasureSpawnPointData[] _spawnPoints;
		private readonly Random _rnd;
		private readonly int _maxPopulation;
		private TimeSpan _timeSinceLastSpawn;

		public int Id { get; }
		public int MonsterClassId { get; } = 147394;
		public int Amount { get; private set; }
		public TimeSpan RespawnDelay { get; }

		public event EventHandler<SpawnEventArgs> Spawning;
		public event EventHandler<SpawnEventArgs> Spawned;

		/// <summary>
		/// Creates a spawner for treasure chests using locations
		/// in spawnPoints and items in dropDb.
		/// </summary>
		/// <param name="uniqueName"></param>
		/// <param name="dropDb"></param>
		/// <param name="maxPopulation"></param>
		/// <param name="respawnDelay"></param>
		/// <param name="spawnPoints"></param>
		public TreasureChestSpawner(
			string uniqueName,
			TreasureDropDb dropDb,
			int maxPopulation,
			TimeSpan respawnDelay,
			params TreasureSpawnPointData[] spawnPoints)
		{
			this.Id = Interlocked.Increment(ref Ids);
			_baseUniqueName = uniqueName;
			_dropDb = dropDb;
			_spawnPoints = spawnPoints;
			_maxPopulation = maxPopulation;
			this.RespawnDelay = respawnDelay;
			_rnd = new Random(RandomProvider.GetSeed());

			this.InitializePopulation();
		}

		public void InitializePopulation()
		{
			this.Amount = 0;
			_timeSinceLastSpawn = this.RespawnDelay;
		}

		private bool IsPositionAvailable(string mapName, Position position)
		{
			lock (LockObj)
			{
				if (!UsedPositions.TryGetValue(mapName, out var mapPositions))
					return true;

				return !mapPositions.Any(pos =>
					Math.Sqrt(Math.Pow(pos.X - position.X, 2) + Math.Pow(pos.Z - position.Z, 2)) < MinSpawnDistance);
			}
		}

		private void AddPosition(string mapName, Position position)
		{
			lock (LockObj)
			{
				if (!UsedPositions.ContainsKey(mapName))
					UsedPositions[mapName] = new HashSet<Position>();

				UsedPositions[mapName].Add(position);
			}
		}

		private void RemovePosition(string mapName, Position position)
		{
			lock (LockObj)
			{
				if (UsedPositions.TryGetValue(mapName, out var mapPositions))
					mapPositions.Remove(position);
			}
		}

		/// <summary>
		/// Spawns the given amount of chests to population
		/// </summary>
		/// <param name="amount"></param>
		/// <exception cref="InvalidOperationException"></exception>
		public void Spawn(int amount)
		{
			for (var i = 0; i < amount && this.Amount < _maxPopulation; i++)
			{
				var spawnResult = this.TryGetAvailableSpawnPoint();
				if (!spawnResult.Success)
				{
					Log.Warning("Failed to find available spawn point for treasure chest");
					break;
				}

				var mapClassName = spawnResult.SpawnPoint.MapClassName;
				var position = spawnResult.SpawnPoint.Position;
				var direction = spawnResult.SpawnPoint.Direction.DegreeAngle;

				if (!ZoneServer.Instance.World.Maps.TryGet(mapClassName, out _))
				{
					//throw new InvalidOperationException($"Map '{mapClassName}' not found.");
					ZoneServer.Instance.World.RemoveSpawner(this);
					break;
				}

				var uniqueChestId = Interlocked.Increment(ref ChestIds);
				var uniqueName = $"{_baseUniqueName}_{uniqueChestId}";
				var itemId = this.GetWeightedRandomDrop(_dropDb);

				if (itemId == 0)
					break;

				var chest = Shortcuts.AddTreasureChest(
					uniqueName,
					mapClassName,
					position.X,
					position.Z,
					direction,
					itemId,
					1,
					"Treasure Chest",
					this.MonsterClassId
				);

				this.Spawning?.Invoke(this, new SpawnEventArgs(this, chest));

				this.AddPosition(mapClassName, position);

				Log.Info("Treasure chest spawned at {0} ({1:F1}, {2:F1}, {3:F1}) with item {4}", mapClassName, position.X, position.Y, position.Z, itemId);

				chest.OnDisappear += () =>
				{
					this.Amount--;
					this.RemovePosition(mapClassName, position);
				};

				this.Amount++;

				this.Spawned?.Invoke(this, new SpawnEventArgs(this, chest));
				_timeSinceLastSpawn = TimeSpan.Zero;
			}
		}

		private (bool Success, TreasureSpawnPointData? SpawnPoint) TryGetAvailableSpawnPoint()
		{
			var shuffledPoints = _spawnPoints.OrderBy(x => _rnd.Next()).ToArray();

			if (shuffledPoints.Length == 0)
			{
				return (false, null);
			}

			for (var attempt = 0; attempt < MAX_SPAWN_ATTEMPTS; attempt++)
			{
				var spawnPoint = shuffledPoints[_rnd.Next(shuffledPoints.Length)];
				if (this.IsPositionAvailable(spawnPoint.MapClassName, spawnPoint.Position))
				{
					return (true, spawnPoint);
				}
			}
			return (false, null);
		}

		private int GetWeightedRandomDrop(TreasureDropDb dropDb)
		{
			var totalWeight = dropDb.Entries.Values.Sum(d => d.ProbabilityFactor);
			var random = (float)(_rnd.NextDouble() * totalWeight);

			float currentWeight = 0;
			foreach (var drop in dropDb.Entries.Values)
			{
				currentWeight += drop.ProbabilityFactor;
				if (random <= currentWeight)
					return drop.Item.Id;
			}

			return dropDb.Entries.Values.FirstOrDefault()?.Item.Id ?? 0;
		}

		public void Update(TimeSpan elapsed)
		{
			_timeSinceLastSpawn += elapsed;

			if (_timeSinceLastSpawn >= this.RespawnDelay && this.Amount < _maxPopulation
				&& this.TryGetAvailableSpawnPoint().Success)
				this.Spawn(1);
		}
	}
}
