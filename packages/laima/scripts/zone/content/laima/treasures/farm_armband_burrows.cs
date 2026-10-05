//--- Melia Script ----------------------------------------------------------
// Farm Armband Burrow Spawns
//--- Description -----------------------------------------------------------
// Spawns interactable soil burrows across the four farm maps. Players can
// dig them up to receive a random hidden armband. The Relic Compass given
// by Tailor Agne reads from ArmbandBurrowRegistry to point toward the
// nearest active burrow.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Spawning;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FarmArmbandBurrowSpawnsScript : GeneralScript
{
	protected override void Load()
	{
		var spawner = new FarmArmbandBurrowSpawner(
			maxPopulation: 3,
			respawnDelay: TimeSpan.FromMinutes(10)
		);

		ZoneServer.Instance.World.AddSpawner(spawner);
	}
}

public static class ArmbandBurrowRegistry
{
	private static readonly Dictionary<string, HashSet<Position>> ActiveBurrows = new();
	private static readonly object Lock = new();

	public static void Register(string mapName, Position position)
	{
		lock (Lock)
		{
			if (!ActiveBurrows.ContainsKey(mapName))
				ActiveBurrows[mapName] = new HashSet<Position>();
			ActiveBurrows[mapName].Add(position);
		}
	}

	public static void Unregister(string mapName, Position position)
	{
		lock (Lock)
		{
			if (ActiveBurrows.TryGetValue(mapName, out var positions))
				positions.Remove(position);
		}
	}

	public static (Position Position, float Distance)? FindNearest(string mapName, Position from)
	{
		lock (Lock)
		{
			if (!ActiveBurrows.TryGetValue(mapName, out var positions) || positions.Count == 0)
				return null;

			Position nearest = default;
			var nearestDistSq = float.MaxValue;
			foreach (var pos in positions)
			{
				var dx = pos.X - from.X;
				var dz = pos.Z - from.Z;
				var distSq = dx * dx + dz * dz;
				if (distSq < nearestDistSq)
				{
					nearestDistSq = distSq;
					nearest = pos;
				}
			}
			return (nearest, MathF.Sqrt(nearestDistSq));
		}
	}
}

public class FarmArmbandBurrowSpawner : ISpawner
{
	private static int Ids = 1200000000;
	private static int BurrowIds = 0;
	private static readonly object LockObj = new();
	private static readonly Dictionary<string, HashSet<Position>> UsedPositions = new();

	private static readonly int[] BurrowMonsterIds = { 155003 }; // dirt_heal_2
	private const int MAX_SPAWN_ATTEMPTS = 10;

	private static readonly int[] ArmbandIds =
	{
		11101, 11102, 11103, 11104, 11105, 11106,
		11107, 11108, 11109, 11110, 11111, 11112,
		11113, 11114, 11115, 11116, 11117, 11118,
		11122, 11123, 11124, 11125, 11126, 11127,
	};

	private readonly int _maxPopulation;
	private TimeSpan _timeSinceLastSpawn;

	public int Id { get; }
	public int Amount { get; private set; }
	public TimeSpan RespawnDelay { get; }

	public event EventHandler<SpawnEventArgs> Spawning;
	public event EventHandler<SpawnEventArgs> Spawned;

	private static readonly (string Map, float X, float Y, float Z, float Dir)[] SpawnPoints =
	{
		// f_farm_47_1
		("f_farm_47_1", -296f, -122f, -246f, 0f),
		("f_farm_47_1", -610f, -122f, -643f, 45f),
		("f_farm_47_1", -369f, -204f, -357f, 0f),
		("f_farm_47_1", -197f, 7f, -1301f, 180f),
		("f_farm_47_1", 563f, 58f, -927f, 0f),
		("f_farm_47_1", -1015f, 41f, -975f, 0f),
		("f_farm_47_1", -806f, -41f, 404f, 315f),
		("f_farm_47_1", -1043f, -41f, 121f, 135f),
		("f_farm_47_1", -1193f, 7f, 789f, 0f),
		("f_farm_47_1", -526f, -41f, 985f, 315f),
		("f_farm_47_1", 340f, 2f, 1083f, 225f),
		("f_farm_47_1", 140f, -41f, 226f, 90f),
		("f_farm_47_1", 538f, -41f, 503f, 0f),
		("f_farm_47_1", 581f, 6f, -121f, 45f),
		("f_farm_47_1", 1379f, -21f, 474f, 0f),
		("f_farm_47_1", 1445f, 6f, 824f, 224f),
		// f_farm_47_2
		("f_farm_47_2", 1709f, -43f, 1132f, 315f),
		("f_farm_47_2", 1314f, -43f, 1266f, 45f),
		("f_farm_47_2", 1557f, 1f, 1865f, 315f),
		("f_farm_47_2", 1091f, 0f, 1635f, 90f),
		("f_farm_47_2", 340f, -63f, 2007f, 0f),
		("f_farm_47_2", 350f, 0f, 1297f, 0f),
		("f_farm_47_2", 94f, 40f, 652f, 90f),
		("f_farm_47_2", -227f, 40f, 338f, 90f),
		("f_farm_47_2", 425f, -72f, -366f, 315f),
		("f_farm_47_2", -73f, -92f, -1793f, 90f),
		("f_farm_47_2", -970f, 0f, -1011f, 0f),
		("f_farm_47_2", 929f, 73f, -855f, 0f),
		("f_farm_47_2", 2429f, 222f, -1183f, 0f),
		// f_farm_47_3
		("f_farm_47_3", -2099f, -9f, 943f, 0f),
		("f_farm_47_3", -2207f, -9f, 761f, 135f),
		("f_farm_47_3", -1322f, -9f, 850f, 315f),
		("f_farm_47_3", -1910f, 32f, -94f, 44f),
		("f_farm_47_3", -1211f, 109f, -251f, 0f),
		("f_farm_47_3", -534f, 163f, -456f, 225f),
		("f_farm_47_3", 7f, 90f, -590f, 315f),
		("f_farm_47_3", -651f, 72f, 882f, 45f),
		("f_farm_47_3", -494f, 72f, 603f, 0f),
		("f_farm_47_3", -671f, 108f, -41f, 135f),
		("f_farm_47_3", 17f, 157f, 145f, 315f),
		("f_farm_47_3", 391f, 205f, 338f, 45f),
		// f_farm_49_2
		("f_farm_49_2", 1628f, 205f, 1400f, 0f),
		("f_farm_49_2", 789f, 145f, 1447f, 0f),
		("f_farm_49_2", 556f, 145f, 946f, 90f),
		("f_farm_49_2", 317f, 145f, 413f, 90f),
		("f_farm_49_2", 856f, 145f, 249f, 225f),
		("f_farm_49_2", 349f, 76f, -289f, 45f),
		("f_farm_49_2", 812f, 76f, -660f, 224f),
		("f_farm_49_2", 1025f, 127f, -1359f, 0f),
		("f_farm_49_2", 1683f, 198f, -1069f, 0f),
		("f_farm_49_2", 360f, 127f, -1540f, 90f),
		("f_farm_49_2", -216f, 65f, 768f, 180f),
		("f_farm_49_2", -1267f, 78f, 818f, 315f),
		("f_farm_49_2", -1908f, 78f, 930f, 44f),
		("f_farm_49_2", -1939f, 78f, 424f, 135f),
		("f_farm_49_2", -518f, 53f, 33f, 45f),
		("f_farm_49_2", -64f, 53f, 90f, 315f),
		("f_farm_49_2", -298f, 0f, -1074f, 45f),
		("f_farm_49_2", -1061f, -36f, -1194f, 0f),
		("f_farm_49_2", -1574f, -36f, -1572f, 135f),
	};

	public FarmArmbandBurrowSpawner(int maxPopulation, TimeSpan respawnDelay)
	{
		this.Id = Interlocked.Increment(ref Ids);
		_maxPopulation = maxPopulation;
		this.RespawnDelay = respawnDelay;

		this.InitializePopulation();
	}

	public void InitializePopulation()
	{
		this.Amount = 0;
		_timeSinceLastSpawn = this.RespawnDelay;
	}

	public void NotifyDormancy(int removedCount)
	{
	}

	private bool IsPositionAvailable(string mapName, Position position)
	{
		lock (LockObj)
		{
			if (!UsedPositions.TryGetValue(mapName, out var mapPositions))
				return true;

			return !mapPositions.Any(pos => pos.InRange2D(position, 5.0f));
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

	public void Spawn(int amount)
	{
		for (var i = 0; i < amount && this.Amount < _maxPopulation; i++)
		{
			var spawnResult = this.TryGetAvailableSpawnPoint();
			if (!spawnResult.Success)
			{
				Log.Warning("FarmArmbandBurrowSpawner: Failed to find available spawn point.");
				break;
			}

			var sp = spawnResult.Point.Value;
			var mapClassName = sp.Map;
			var position = new Position(sp.X, sp.Y, sp.Z);

			if (!ZoneServer.Instance.World.Maps.TryGet(mapClassName, out var map))
			{
				ZoneServer.Instance.World.RemoveSpawner(this);
				break;
			}

			var uniqueId = Interlocked.Increment(ref BurrowIds);
			var uniqueName = $"farm_armband_burrow_{uniqueId}";
			var armbandId = ArmbandIds[RandomProvider.Get().Next(ArmbandIds.Length)];

			var location = new Location(map.Id, position);
			var dir = new Direction(sp.Dir);
			var monsterId = BurrowMonsterIds[RandomProvider.Get().Next(BurrowMonsterIds.Length)];

			var burrow = new Npc(monsterId, "Disturbed Soil", location, dir);
			burrow.UniqueName = uniqueName;

			burrow.SetClickTrigger("DYNAMIC_DIALOG", async dialog =>
			{
				var character = dialog.Player;
				var npc = dialog.Npc;

				if (!character.Position.InRange3D(npc.Position, 30))
					return;

				var result = await character.TimeActions.StartAsync(
					"Digging up the burrow...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed && npc.Vars.ActivateOnce($"Npc.{uniqueName}"))
				{
					character.Inventory.Add(armbandId, 1, InventoryAddType.PickUp);
					ArmbandBurrowRegistry.Unregister(mapClassName, position);
					npc.Map?.RemoveMonster(npc);
				}
			});

			this.Spawning?.Invoke(this, new SpawnEventArgs(this, burrow));

			this.AddPosition(mapClassName, position);
			ArmbandBurrowRegistry.Register(mapClassName, position);

			map.AddMonster(burrow);

			burrow.OnDisappear += () =>
			{
				this.Amount--;
				this.RemovePosition(mapClassName, position);
				ArmbandBurrowRegistry.Unregister(mapClassName, position);
			};

			this.Amount++;

			this.Spawned?.Invoke(this, new SpawnEventArgs(this, burrow));
			_timeSinceLastSpawn = TimeSpan.Zero;
		}
	}

	private (bool Success, (string Map, float X, float Y, float Z, float Dir)? Point) TryGetAvailableSpawnPoint()
	{
		for (var attempt = 0; attempt < MAX_SPAWN_ATTEMPTS; attempt++)
		{
			var sp = SpawnPoints[RandomProvider.Get().Next(SpawnPoints.Length)];
			var position = new Position(sp.X, sp.Y, sp.Z);

			if (this.IsPositionAvailable(sp.Map, position))
				return (true, sp);
		}
		return (false, null);
	}

	public void Update(TimeSpan elapsed)
	{
		_timeSinceLastSpawn += elapsed;

		if (_timeSinceLastSpawn >= this.RespawnDelay && this.Amount < _maxPopulation
			&& this.TryGetAvailableSpawnPoint().Success)
			this.Spawn(1);
	}
}
