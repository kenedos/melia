//--- Melia Script ----------------------------------------------------------
// Dina Bee Farm Wild Hives
//--- Description -----------------------------------------------------------
// Spawns wild beehives across f_siauliai_46_4 (Dina Bee Farm). Interacting
// with one rouses every Rabbee / Honeymeli / Honeybean within 400 units,
// summons a fresh swarm of Rabbees and Honeybeans, and rewards a raw
// honeycomb that Rasa exchanges for potions.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

public class DinaBeeFarmHiveSpawnsScript : GeneralScript
{
	protected override void Load()
	{
		var spawner = new DinaBeeFarmHiveSpawner(
			maxPopulation: 3,
			respawnDelay: TimeSpan.FromMinutes(6)
		);

		ZoneServer.Instance.World.AddSpawner(spawner);
	}
}

public class DinaBeeFarmHiveSpawner : ISpawner
{
	private static int Ids = 1100100000;
	private static int HiveIds = 0;
	private static readonly object LockObj = new();
	private static readonly Dictionary<string, HashSet<Position>> UsedPositions = new();

	private static readonly int[] HiveMonsterIds = { 151025, 151026, 151027 }; // beehive_1, beehive_2, beehive_3
	private static readonly int[] BeeMonsterIds = { MonsterId.Rabbee, MonsterId.Honeymeli, MonsterId.Honeybean };
	private const int MAX_SPAWN_ATTEMPTS = 10;

	private readonly int _maxPopulation;
	private TimeSpan _timeSinceLastSpawn;

	public int Id { get; }
	public int Amount { get; private set; }
	public TimeSpan RespawnDelay { get; }

	public event EventHandler<SpawnEventArgs> Spawning;
	public event EventHandler<SpawnEventArgs> Spawned;

	private static readonly (string Map, float X, float Y, float Z, float Dir)[] SpawnPoints =
	{
		("f_siauliai_46_4", 88f, 148f, -846f, 90f),
		("f_siauliai_46_4", 203f, 148f, -980f, 180f),
		("f_siauliai_46_4", 341f, 148f, -984f, 270f),
		("f_siauliai_46_4", 235f, 148f, -688f, 0f),
		("f_siauliai_46_4", 348f, 148f, -768f, 270f),
		("f_siauliai_46_4", 484f, 148f, -719f, 270f),
		("f_siauliai_46_4", 388f, 148f, -584f, 0f),
		("f_siauliai_46_4", 933f, 148f, -714f, 0f),
		("f_siauliai_46_4", 879f, 148f, -813f, 180f),
		("f_siauliai_46_4", 1020f, 148f, -914f, 180f),
		("f_siauliai_46_4", 1070f, 148f, -1004f, 180f),
		("f_siauliai_46_4", 1215f, 148f, -1021f, 225f),
		("f_siauliai_46_4", 1318f, 148f, -996f, 225f),
		("f_siauliai_46_4", 1350f, 148f, -889f, 270f),
		("f_siauliai_46_4", 1201f, 148f, -781f, 270f),
		("f_siauliai_46_4", 1376f, 148f, -770f, 270f),
		("f_siauliai_46_4", 1319f, 148f, -603f, 315f),
		("f_siauliai_46_4", 1139f, 148f, -603f, 0f),
		("f_siauliai_46_4", 980f, 148f, -645f, 0f),
	};

	public DinaBeeFarmHiveSpawner(int maxPopulation, TimeSpan respawnDelay)
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
				Log.Warning("DinaBeeFarmHiveSpawner: Failed to find available spawn point.");
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

			var uniqueId = Interlocked.Increment(ref HiveIds);
			var uniqueName = $"dina_bee_farm_hive_{uniqueId}";

			var location = new Location(map.Id, position);
			var dir = new Direction(sp.Dir);
			var monsterId = HiveMonsterIds[RandomProvider.Get().Next(HiveMonsterIds.Length)];

			var hive = new Npc(monsterId, "Wild Beehive", location, dir);
			hive.UniqueName = uniqueName;

			hive.SetClickTrigger("DYNAMIC_DIALOG", async dialog =>
			{
				var character = dialog.Player;
				var npc = dialog.Npc;

				if (!character.Position.InRange3D(npc.Position, 30))
					return;

				if (npc.Vars.ActivateOnce($"Npc.{uniqueName}.Disturbed"))
				{
					var swarmSize = RandomProvider.Get().Next(14, 19);
					for (var s = 0; s < swarmSize; s++)
					{
						var beeId = RandomProvider.Get().Next(2) == 0 ? MonsterId.Rabbee : MonsterId.Honeybean;
						SpawnTempMonsters(character, beeId, 1, 150, TimeSpan.FromMinutes(2));
					}

					var luredCount = LureNearbyEnemies(character, BeeMonsterIds, 400, 300);
					if (luredCount > 0)
						character.ServerMessage("{#FF6666}The hive shakes - the swarm is on you!{/}");
				}

				var result = await character.TimeActions.StartAsync(
					"Cracking the hive...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(2)
				);

				if (result == TimeActionResult.Completed && npc.Vars.ActivateOnce($"Npc.{uniqueName}.Looted"))
				{
					character.Inventory.Add(ItemId.SIAULIAI_46_3_MQ_01_ITEM, 1, InventoryAddType.PickUp);
					npc.Map?.RemoveMonster(npc);
				}
			});

			this.Spawning?.Invoke(this, new SpawnEventArgs(this, hive));

			this.AddPosition(mapClassName, position);

			map.AddMonster(hive);

			hive.OnDisappear += () =>
			{
				this.Amount--;
				this.RemovePosition(mapClassName, position);
			};

			this.Amount++;

			this.Spawned?.Invoke(this, new SpawnEventArgs(this, hive));
			_timeSinceLastSpawn = TimeSpan.Zero;
		}
	}

	private (bool Success, (string Map, float X, float Y, float Z, float Dir)? Point) TryGetAvailableSpawnPoint()
	{
		if (SpawnPoints.Length == 0)
			return (false, null);

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
