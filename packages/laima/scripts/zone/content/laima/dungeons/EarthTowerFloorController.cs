using System;
using System.Collections.Generic;
using Melia.Shared.World;
using Melia.Zone.World.Maps;
using Yggdrasil.Geometry;

public enum EarthTowerFloorType
{
	Mobs,
	Defense,
	Boss,
}

public sealed class EarthTowerFloorDefinition
{
	public int Floor { get; }
	public EarthTowerFloorType Type { get; }
	public int BossMonsterId { get; }

	public EarthTowerFloorDefinition(int floor, EarthTowerFloorType type, int bossMonsterId = 0)
	{
		this.Floor = floor;
		this.Type = type;
		this.BossMonsterId = bossMonsterId;
	}
}

public static class EarthTowerFloorController
{
	public const string MapClassName = "mission_groundtower_2";
	public const int MapId = 521;
	public const int MaximumFloor = 20;

	public const int MobFloorTotalKills = 40;
	public const int MobFloorMonstersPerWave = 40;

	public const float SpawnMinimumDistance = 10f;
	public const int SpawnPositionAttempts = 10000;
	public const int DefenseObjectHp = 50;
	public const int DefenseMonstersPerWave = 30;
	public const int DefenseMaxAliveMonsters = 120;

	public static readonly TimeSpan DefenseFloorDuration = TimeSpan.FromMinutes(1);
	public static readonly TimeSpan DefenseWaveInterval = TimeSpan.FromSeconds(10);

	public static readonly Position DefenseObjectPosition = new Position(-180f, 240f, -50f);
	public static readonly Position BossSpawnPosition = new Position(-180f, 240f, -50f);

	public const int NeopMonsterId = 100167;
	public const int OrganMonsterId = 100168;

	public static readonly int[] MonsterPool =
	{
		100016,
		100017,
		100018,
		100019,
		100020,
		100021,
		100022,
		100023,
		100024,
		100025,
		100026,
		100027,
		100028,
		100029,
		100030,
		100031,
		100032,
		100033,
		100034,
		100035,
		100039,
		100040,
		100041,
		100042,
		100043,
		100044,
		100045,
		100046,
		100047,
		100048,
		100049,
		100050,
		100051,
		100052,
		100054,
		100055,
		100056,
		100057,
		100058,
		100059,
		100060,
		100061,
		100062,
		100063,
		100064,
		100065,
		100066,
		100067,
		100068,
		100069,
		100070,
		100071,
		100083,
		100084,
		100085,
		100086,
		100087,
		100088,
		100089,
		100090,
		100091,
		100092,
		100093,
		100094,
		100095,
		100096,
		100097,
	};

	public static readonly Position[] SpawnPolygon =
	{
		new Position(-2f, 240f, 121f),
		new Position(-182f, 240f, 203f),
		new Position(-357f, 240f, 137f),
		new Position(-467f, 240f, -71f),
		new Position(-351f, 240f, -297f),
		new Position(-171f, 240f, -299f),
		new Position(-12f, 240f, -235f),
		new Position(70f, 240f, -58f),
	};

	public static readonly IReadOnlyDictionary<int, EarthTowerFloorDefinition> Floors =
	new Dictionary<int, EarthTowerFloorDefinition>
	{
		{ 1, new EarthTowerFloorDefinition(1, EarthTowerFloorType.Mobs) },
		{ 2, new EarthTowerFloorDefinition(2, EarthTowerFloorType.Mobs) },
		{ 3, new EarthTowerFloorDefinition(3, EarthTowerFloorType.Defense) },
		{ 4, new EarthTowerFloorDefinition(4, EarthTowerFloorType.Mobs) },
		{ 5, new EarthTowerFloorDefinition(5, EarthTowerFloorType.Mobs) },
		{ 6, new EarthTowerFloorDefinition(6, EarthTowerFloorType.Defense) },
		{ 7, new EarthTowerFloorDefinition(7, EarthTowerFloorType.Mobs) },
		{ 8, new EarthTowerFloorDefinition(8, EarthTowerFloorType.Mobs) },
		{ 9, new EarthTowerFloorDefinition(9, EarthTowerFloorType.Defense) },
		{ 10, new EarthTowerFloorDefinition(10, EarthTowerFloorType.Boss, NeopMonsterId) },

		{ 11, new EarthTowerFloorDefinition(11, EarthTowerFloorType.Mobs) },
		{ 12, new EarthTowerFloorDefinition(12, EarthTowerFloorType.Mobs) },
		{ 13, new EarthTowerFloorDefinition(13, EarthTowerFloorType.Defense) },
		{ 14, new EarthTowerFloorDefinition(14, EarthTowerFloorType.Mobs) },
		{ 15, new EarthTowerFloorDefinition(15, EarthTowerFloorType.Mobs) },
		{ 16, new EarthTowerFloorDefinition(16, EarthTowerFloorType.Defense) },
		{ 17, new EarthTowerFloorDefinition(17, EarthTowerFloorType.Mobs) },
		{ 18, new EarthTowerFloorDefinition(18, EarthTowerFloorType.Mobs) },
		{ 19, new EarthTowerFloorDefinition(19, EarthTowerFloorType.Defense) },
		{ 20, new EarthTowerFloorDefinition(20, EarthTowerFloorType.Boss, OrganMonsterId) },
	};

	public static List<Position> GenerateWaveSpawnPositions(Map map, Random random)
	{
		return GenerateSpawnPositions(map, random, MobFloorMonstersPerWave);
	}

	public static List<Position> GenerateSpawnPositions(Map map, Random random, int count)
	{
		var result = new List<Position>(count);
		var attempts = 0;

		while (result.Count < count && attempts < SpawnPositionAttempts)
		{
			attempts++;

			var x = -467f + (float)random.NextDouble() * 537f;
			var z = -299f + (float)random.NextDouble() * 502f;

			if (!IsInsideSpawnPolygon(x, z))
				continue;

			var candidate = new Position(x, 240f, z);

			if (!map.Ground.TryGetNearestValidPosition(candidate, out var validPosition))
				continue;

			if (!IsInsideSpawnPolygon(validPosition.X, validPosition.Z))
				continue;

			if (!IsFarEnoughFromExistingPositions(result, validPosition))
				continue;

			result.Add(validPosition);
		}

		return result;
	}

	private static bool IsInsideSpawnPolygon(float x, float z)
	{
		var inside = false;

		for (var i = 0; i < SpawnPolygon.Length; i++)
		{
			var j = i == 0 ? SpawnPolygon.Length - 1 : i - 1;

			var xi = SpawnPolygon[i].X;
			var zi = SpawnPolygon[i].Z;
			var xj = SpawnPolygon[j].X;
			var zj = SpawnPolygon[j].Z;

			var intersects =
				((zi > z) != (zj > z)) &&
				(x < (xj - xi) * (z - zi) / (zj - zi) + xi);

			if (intersects)
				inside = !inside;
		}

		return inside;
	}

	private static bool IsFarEnoughFromExistingPositions(List<Position> positions, Position candidate)
	{
		var minimumDistanceSquared = SpawnMinimumDistance * SpawnMinimumDistance;

		foreach (var position in positions)
		{
			var deltaX = position.X - candidate.X;
			var deltaZ = position.Z - candidate.Z;
			var distanceSquared = deltaX * deltaX + deltaZ * deltaZ;

			if (distanceSquared < minimumDistanceSquared)
				return false;
		}

		return true;
	}

	public static int GetRandomMonsterId(Random random)
	{
		return MonsterPool[random.Next(MonsterPool.Length)];
	}
}
