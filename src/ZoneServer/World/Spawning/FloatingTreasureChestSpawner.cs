using System;
using System.Threading;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.World.Spawning
{
	/// <summary>
	/// Spawns a single floating treasure chest at a fixed location with random respawn timer.
	/// </summary>
	public class FloatingTreasureChestSpawner : ISpawner
	{
		private static int Ids = 2000000000;
		private static int ChestIds = 0;

		private readonly string _baseUniqueName;
		private readonly string _mapClassName;
		private readonly Position _position;
		private readonly double _direction;
		private readonly int _itemId;
		private readonly int _amount;
		private readonly int _monsterId;
		private readonly string _chestName;
		private readonly TimeSpan _minRespawnDelay;
		private readonly TimeSpan _maxRespawnDelay;
		private readonly Random _rnd;

		private Npc _currentChest;
		private TimeSpan _timeSinceDisappeared;
		private TimeSpan _nextRespawnDelay;

		public int Id { get; }
		public int MonsterClassId => _monsterId;
		public int Amount { get; private set; }

		public event EventHandler<SpawnEventArgs> Spawning;
		public event EventHandler<SpawnEventArgs> Spawned;

		/// <summary>
		/// Creates a spawner for a single floating treasure chest with random respawn time.
		/// </summary>
		public FloatingTreasureChestSpawner(
			string uniqueName,
			string mapClassName,
			double x,
			double y,
			double z,
			double direction,
			int itemId,
			int amount,
			TimeSpan minRespawnDelay,
			TimeSpan maxRespawnDelay,
			string chestName = "Treasure Chest",
			int monsterId = 147392)
		{
			this.Id = Interlocked.Increment(ref Ids);
			_baseUniqueName = uniqueName;
			_mapClassName = mapClassName;
			_position = new Position((float)x, (float)y, (float)z);
			_direction = direction;
			_itemId = itemId;
			_amount = amount;
			_monsterId = monsterId;
			_chestName = chestName;
			_minRespawnDelay = minRespawnDelay;
			_maxRespawnDelay = maxRespawnDelay;
			_rnd = new Random(RandomProvider.GetSeed());

			this.InitializePopulation();
		}

		public void InitializePopulation()
		{
			this.Amount = 0;
			_nextRespawnDelay = this.GetRandomRespawnDelay();
			_timeSinceDisappeared = _nextRespawnDelay; // Start ready to spawn immediately
		}

		private TimeSpan GetRandomRespawnDelay()
		{
			var minMs = _minRespawnDelay.TotalMilliseconds;
			var maxMs = _maxRespawnDelay.TotalMilliseconds;
			var randomMs = minMs + (_rnd.NextDouble() * (maxMs - minMs));
			return TimeSpan.FromMilliseconds(randomMs);
		}

		public void Spawn(int amount)
		{
			if (this.Amount >= 1 || amount < 1)
				return;

			if (!ZoneServer.Instance.World.Maps.TryGet(_mapClassName, out _))
			{
				ZoneServer.Instance.World.RemoveSpawner(this);
				return;
			}

			var uniqueChestId = Interlocked.Increment(ref ChestIds);
			var uniqueName = $"{_baseUniqueName}_{uniqueChestId}";

			_currentChest = Shortcuts.AddFloatingTreasureChest(
				uniqueName,
				_mapClassName,
				_position.X,
				_position.Y,
				_position.Z,
				_direction,
				_itemId,
				_amount,
				_chestName,
				_monsterId
			);

			this.Spawning?.Invoke(this, new SpawnEventArgs(this, _currentChest));

			_currentChest.OnDisappear += () =>
			{
				this.Amount--;
				_timeSinceDisappeared = TimeSpan.Zero;
				_nextRespawnDelay = this.GetRandomRespawnDelay();
				_currentChest = null;
			};

			this.Amount++;

			this.Spawned?.Invoke(this, new SpawnEventArgs(this, _currentChest));
		}

		public void Update(TimeSpan elapsed)
		{
			if (this.Amount >= 1)
				return;

			_timeSinceDisappeared += elapsed;

			if (_timeSinceDisappeared >= _nextRespawnDelay)
			{
				this.Spawn(1);
			}
		}
	}
}
