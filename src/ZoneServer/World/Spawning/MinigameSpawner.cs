using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
	/// Spawns minigames in a list of MinigameSpawnPointData.
	/// </summary>
	public class MinigameSpawner : ISpawner
	{
		private static int Ids = 2100000000;
		private static readonly object LockObj = new();
		private static readonly Dictionary<string, HashSet<Position>> UsedPositions = new();
		private const float MinSpawnDistance = 50.0f;
		private const int MAX_SPAWN_ATTEMPTS = 10;

		private readonly MinigameSpawnPointData[] _spawnPoints;
		private readonly Random _rnd;
		private readonly int _maxPopulation;
		private TimeSpan _timeSinceLastSpawn;
		private readonly List<IMinigameScript> _availableMinigames;
		private readonly Dictionary<int, IMinigameInstance> _activeMinigames;
		private readonly Dictionary<int, TimeSpan> _idleTime;
		private readonly TimeSpan _rotationTimeout;

		public int Id { get; }
		public int Amount { get; private set; }
		public TimeSpan RespawnDelay { get; }

		public event EventHandler<SpawnEventArgs> Spawning;
		public event EventHandler<SpawnEventArgs> Spawned;

		/// <summary>
		/// Creates a spawner for minigames using locations in spawnPoints.
		/// </summary>
		/// <param name="maxPopulation"></param>
		/// <param name="respawnDelay"></param>
		/// <param name="spawnPoints"></param>
		public MinigameSpawner(
			int maxPopulation,
			TimeSpan respawnDelay,
			TimeSpan rotationTimeout,
			params MinigameSpawnPointData[] spawnPoints)
		{
			this.Id = Interlocked.Increment(ref Ids);
			_spawnPoints = spawnPoints;
			_maxPopulation = maxPopulation;
			this.RespawnDelay = respawnDelay;
			_rotationTimeout = rotationTimeout;
			_rnd = new Random(RandomProvider.GetSeed());
			_availableMinigames = new List<IMinigameScript>();
			_activeMinigames = new Dictionary<int, IMinigameInstance>();
			_idleTime = new Dictionary<int, TimeSpan>();

			this.LoadMinigameScripts();
			this.InitializePopulation();
		}

		public void InitializePopulation()
		{
			// End all active minigame instances and clear them
			// This ensures proper cleanup when scripts are reloaded
			foreach (var minigame in _activeMinigames.Values.ToArray())
			{
				minigame.End();
			}
			_activeMinigames.Clear();

			// Clear all used positions since minigames are being reset
			lock (LockObj)
			{
				UsedPositions.Clear();
			}

			this.Amount = 0;
			_idleTime?.Clear();
			_timeSinceLastSpawn = this.RespawnDelay;
		}

		/// <summary>
		/// Loads all minigame scripts from the registered minigames.
		/// </summary>
		private void LoadMinigameScripts()
		{
			// Minigames will be registered via RegisterMinigame method
			// from script initialization
		}

		/// <summary>
		/// Registers a minigame script with this spawner.
		/// </summary>
		public void RegisterMinigame(IMinigameScript minigameScript)
		{
			if (minigameScript == null)
				throw new ArgumentNullException(nameof(minigameScript));

			_availableMinigames.Add(minigameScript);
			Log.Info("Registered minigame script: {0}", minigameScript.GetType().Name);
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
		/// Spawns the given amount of minigames.
		/// </summary>
		/// <param name="amount"></param>
		public void Spawn(int amount)
		{
			if (_availableMinigames.Count == 0)
				return;

			for (var i = 0; i < amount && this.Amount < _maxPopulation; i++)
			{
				var spawnResult = this.TryGetAvailableSpawnPoint();
				if (!spawnResult.Success)
				{
					Log.Warning("Failed to find available spawn point for minigame");
					break;
				}

				var mapClassName = spawnResult.SpawnPoint.MapClassName;
				var position = spawnResult.SpawnPoint.Position;
				var direction = spawnResult.SpawnPoint.Direction;

				if (!ZoneServer.Instance.World.Maps.TryGet(mapClassName, out var map))
				{
					Log.Warning("Map '{0}' not found for minigame spawn.", mapClassName);
					continue;
				}

				// Select random minigame
				var minigameScript = _availableMinigames[_rnd.Next(_availableMinigames.Count)];

				// Create minigame instance
				var instance = minigameScript.CreateInstance(map, position, direction);
				if (instance == null)
				{
					Log.Error("Failed to create minigame instance from script {0}", minigameScript.GetType().Name);
					continue;
				}

				var minigameId = instance.Id;
				_activeMinigames[minigameId] = instance;

				this.Spawning?.Invoke(this, new SpawnEventArgs(this, null));

				this.AddPosition(mapClassName, position);

				// Subscribe to minigame end event
				instance.OnEnd += () =>
				{
					this.Amount--;
					this.RemovePosition(mapClassName, position);
					_activeMinigames.Remove(minigameId);
				};

				// Start the minigame
				instance.Start();

				this.Amount++;

				this.Spawned?.Invoke(this, new SpawnEventArgs(this, null));
				_timeSinceLastSpawn = TimeSpan.Zero;

				Log.Info("Minigame spawned: {0} at {1} ({2:F1}, {3:F1}, {4:F1})", minigameScript.GetType().Name, mapClassName, position.X, position.Y, position.Z);
			}
		}

		private (bool Success, MinigameSpawnPointData? SpawnPoint) TryGetAvailableSpawnPoint()
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

		public void Update(TimeSpan elapsed)
		{
			_timeSinceLastSpawn += elapsed;

			// Update all active minigames
			foreach (var minigame in _activeMinigames.Values.ToArray())
			{
				minigame.Update(elapsed);

				// Rotate out minigames on empty maps
				if (!minigame.Map.HasCharacters)
				{
					_idleTime.TryGetValue(minigame.Id, out var idle);
					idle += elapsed;

					if (idle >= _rotationTimeout)
					{
						minigame.End();
						_idleTime.Remove(minigame.Id);
						continue;
					}

					_idleTime[minigame.Id] = idle;
				}
				else
				{
					_idleTime.Remove(minigame.Id);
				}
			}

			// Spawn new minigames if needed
			if (_timeSinceLastSpawn >= this.RespawnDelay && this.Amount < _maxPopulation
				&& this.TryGetAvailableSpawnPoint().Success)
				this.Spawn(1);
		}
	}

	/// <summary>
	/// Interface for minigame scripts.
	/// </summary>
	public interface IMinigameScript
	{
		/// <summary>
		/// Creates an instance of the minigame at the specified location.
		/// </summary>
		IMinigameInstance CreateInstance(Map map, Position position, Direction direction);
	}

	/// <summary>
	/// Interface for minigame instances.
	/// </summary>
	public interface IMinigameInstance
	{
		int Id { get; }
		Map Map { get; }
		event Action OnEnd;
		void Start();
		void Update(TimeSpan elapsed);
		void End();
	}
}
