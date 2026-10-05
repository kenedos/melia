//--- Melia Script ----------------------------------------------------------
// Firetower Book Spawns
//--- Description -----------------------------------------------------------
// Spawns collectible books randomly across Mage Tower floors 1-5.
// Players can interact with them to receive a random Monster Card Album.
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

public class FiretowerBookSpawnsScript : GeneralScript
{
	protected override void Load()
	{
		var spawner = new FiretowerBookSpawner(
			maxPopulation: 5,
			respawnDelay: TimeSpan.FromMinutes(15)
		);

		ZoneServer.Instance.World.AddSpawner(spawner);
	}
}

public class FiretowerBookSpawner : ISpawner
{
	private static int Ids = 1100000000;
	private static int BookIds = 0;
	private static readonly object LockObj = new();
	private static readonly Dictionary<string, HashSet<Position>> UsedPositions = new();

	private static readonly int[] BookMonsterIds = { 151131, 151132 }; // d_firetower_book_pile_1, d_firetower_book_pile_2
	private const int MAX_SPAWN_ATTEMPTS = 10;

	private static readonly int[] ItemIds = { 670001, 670002, 670003, 670004 };

	private readonly int _maxPopulation;
	private TimeSpan _timeSinceLastSpawn;

	public int Id { get; }
	public int Amount { get; private set; }
	public TimeSpan RespawnDelay { get; }

	public event EventHandler<SpawnEventArgs> Spawning;
	public event EventHandler<SpawnEventArgs> Spawned;

	private static readonly (string Map, float X, float Y, float Z, float Dir)[] SpawnPoints =
	{
		// d_firetower_41
		("d_firetower_41", -808.2201f, 1391.8729f, -1685.7788f, 44f),
		("d_firetower_41", -601.2848f, 1391.8729f, -1577.8951f, 44f),
		("d_firetower_41", -446.06323f, 1391.8729f, -1601.798f, 0f),
		("d_firetower_41", -483.1238f, 1391.8729f, -2103.457f, 225f),
		("d_firetower_41", -798.332f, 1391.8729f, -2027.2869f, 135f),
		("d_firetower_41", -1391.7844f, 1552.8285f, -1492.0951f, 315f),
		("d_firetower_41", -1458.3905f, 1552.8285f, -1284.8904f, 0f),
		("d_firetower_41", -1683.9572f, 1552.8285f, -1236.0916f, 0f),
		("d_firetower_41", -1789.8885f, 1552.8285f, -1327.5258f, 90f),
		("d_firetower_41", -1962.033f, 1552.8285f, -1365.9541f, 45f),
		("d_firetower_41", -2226.4883f, 1490.1371f, -2094.8838f, 135f),
		("d_firetower_41", -2810.21f, 1489.3549f, -1840.4286f, 90f),
		("d_firetower_41", -2614.7903f, 1552.8285f, -1473.6727f, 89f),
		("d_firetower_41", -2661.0017f, 1552.8285f, -1347.074f, 89f),
		("d_firetower_41", -2516.4011f, 1552.8285f, -1256.1807f, 359f),
		("d_firetower_41", -2364.482f, 1552.8285f, -1285.984f, 359f),
		("d_firetower_41", -2146.1062f, 1552.8285f, -1430.0417f, 270f),
		("d_firetower_41", 307.59708f, 1602.349f, -1120.2412f, 90f),
		("d_firetower_41", 402.48276f, 1602.349f, -883.64636f, 0f),
		("d_firetower_41", 399.275f, 1602.349f, -962.2808f, 90f),
		("d_firetower_41", 517.98175f, 1602.349f, -859.6041f, 0f),
		("d_firetower_41", 646.3579f, 1602.349f, -860.3187f, 0f),
		("d_firetower_41", 1224.9481f, 1602.349f, -907.90265f, 0f),
		("d_firetower_41", 1312.713f, 1602.349f, -812.4494f, 90f),
		("d_firetower_41", 1473.2233f, 1602.349f, -814.9802f, 0f),
		("d_firetower_41", 1574.8008f, 1602.349f, -980.1698f, 270f),
		("d_firetower_41", 1542.177f, 1602.349f, -1176.6857f, 225f),
		("d_firetower_41", 1253.4557f, 1334.3827f, -2218.19f, 90f),
		("d_firetower_41", 1269.0573f, 1334.3827f, -2358.3357f, 90f),
		("d_firetower_41", 1612.0684f, 1334.3828f, -2403.5283f, 225f),
		("d_firetower_41", 2358.64f, 1334.3829f, -2049.185f, 0f),
		("d_firetower_41", 2448.4475f, 1334.3829f, -2102.9863f, 0f),
		("d_firetower_41", 2526.5442f, 1334.3828f, -2254.5166f, 0f),
		("d_firetower_41", 2229.4392f, 1334.3827f, -2440.661f, 90f),
		("d_firetower_41", 2431.8564f, 1396.4327f, -1809.2506f, 359f),
		("d_firetower_41", 1881.2726f, 1460.5518f, -1436.6616f, 89f),
		("d_firetower_41", 2036.6581f, 1460.5518f, -1253.1079f, 0f),
		("d_firetower_41", 2856.5132f, 1460.5864f, -1232.3658f, 359f),
		("d_firetower_41", 3014.8794f, 1460.5864f, -1217.1534f, 359f),
		("d_firetower_41", 3171.99f, 1460.5864f, -1456.5128f, 359f),
		("d_firetower_41", 3151.4946f, 1396.4327f, -1837.0109f, 0f),
		("d_firetower_41", 2608.2185f, 1397.8356f, -1874.0854f, 90f),
		// d_firetower_42
		("d_firetower_42", 2363.0127f, 186.6614f, -536.82465f, 89f),
		("d_firetower_42", 2626.93f, 71.656296f, -274.4265f, 224f),
		("d_firetower_42", 2591.459f, 71.656296f, 69.14803f, 0f),
		("d_firetower_42", 2445.4329f, 71.656296f, 28.922218f, 315f),
		("d_firetower_42", 2337.9634f, 71.65629f, -53.3227f, 89f),
		("d_firetower_42", 2070.7817f, 71.6562f, -28.80421f, 0f),
		("d_firetower_42", 1951.6559f, 71.656296f, 61.202503f, 0f),
		("d_firetower_42", 1804.4603f, 71.656296f, -20.22932f, 90f),
		("d_firetower_42", 1772.4902f, 71.656296f, -155.80424f, 90f),
		("d_firetower_42", 1904.5372f, 71.656296f, -225.12892f, 0f),
		("d_firetower_42", 1811.8141f, 20.576601f, -516.33826f, 90f),
		("d_firetower_42", 1824.1263f, 28.525301f, -1174.386f, 89f),
		("d_firetower_42", 1732.5378f, 28.525301f, -1259.8438f, 90f),
		("d_firetower_42", 2069.921f, 28.525301f, -1225.3975f, 359f),
		("d_firetower_42", 1982.037f, 28.525301f, -1417.3882f, 359f),
		("d_firetower_42", 1882.2114f, 28.525301f, -1404.3694f, 0f),
		("d_firetower_42", 1868.8936f, 28.525301f, -1516.0839f, 0f),
		("d_firetower_42", 2009.7577f, 28.525301f, -1510.8518f, 359f),
		("d_firetower_42", 1171.3126f, 75.1989f, -1246.036f, 0f),
		("d_firetower_42", 905.98724f, 75.1989f, -1261.078f, 89f),
		("d_firetower_42", 847.4138f, 75.1989f, -1355.3529f, 90f),
		("d_firetower_42", 867.1146f, 75.1989f, -1490.9094f, 90f),
		("d_firetower_42", 1055.7529f, 75.1989f, -1464.4465f, 0f),
		("d_firetower_42", 776.4278f, 71.3507f, -518.4701f, 90f),
		("d_firetower_42", 1216.2096f, 71.3507f, -508.98743f, 0f),
		("d_firetower_42", 1152.166f, 71.3507f, 3.7404943f, 0f),
		("d_firetower_42", 1121.1946f, 71.3507f, 296.1398f, 0f),
		("d_firetower_42", 971.6063f, 71.3507f, 306.42123f, 0f),
		("d_firetower_42", 829.8734f, 71.3507f, 242.67197f, 90f),
		("d_firetower_42", 477.55267f, 71.3787f, 237.13144f, 0f),
		("d_firetower_42", 372.18265f, 71.3787f, 312.6984f, 0f),
		("d_firetower_42", 248.80573f, 71.3787f, 284.22305f, 90f),
		("d_firetower_42", 179.83305f, 71.3787f, 127.22585f, 90f),
		("d_firetower_42", 353.9629f, 71.3787f, 144.25653f, 180f),
		("d_firetower_42", 561.881f, 73.65105f, -514.01685f, 315f),
		("d_firetower_42", -148.89699f, 73.264f, -387.40317f, 0f),
		("d_firetower_42", -242.81456f, 73.264f, -396.6973f, 0f),
		("d_firetower_42", -364.17535f, 73.264f, -406.2157f, 90f),
		("d_firetower_42", -431.0956f, 73.264f, -488.15353f, 90f),
		("d_firetower_42", -416.7841f, 73.264f, -572.8266f, 90f),
		("d_firetower_42", -323.72076f, 59.3703f, -602.58923f, 135f),
		("d_firetower_42", -907.7897f, 9.733103f, -1163.881f, 89f),
		("d_firetower_42", -975.85016f, 2.3923965f, -498.31796f, 0f),
		("d_firetower_42", -866.2608f, 2.3923965f, -589.6382f, 315f),
		("d_firetower_42", -1140.736f, 2.3923965f, -545.4625f, 0f),
		("d_firetower_42", -1191.119f, 2.3923965f, -639.67615f, 90f),
		("d_firetower_42", -1187.176f, 2.3923965f, -735.832f, 90f),
		("d_firetower_42", -999.98114f, 2.3923965f, -699.9709f, 0f),
		("d_firetower_42", -1372.6145f, 2.3923965f, -1021.08856f, 0f),
		("d_firetower_42", -1473.5182f, 2.3923965f, -1032.5823f, 0f),
		("d_firetower_42", -1656.5887f, 2.3923965f, -1119.6825f, 90f),
		("d_firetower_42", -1653.8494f, 2.3923965f, -1218.7938f, 90f),
		("d_firetower_42", -1029.443f, 10.249407f, -1286.2219f, 0f),
		("d_firetower_42", -1392.9005f, 8.445709f, -2198.82f, 0f),
		("d_firetower_42", -1441.281f, 8.445706f, -2303.8506f, 90f),
		("d_firetower_42", -1444.3187f, 8.445711f, -2463.35f, 180f),
		("d_firetower_42", -1166.8418f, 8.445711f, -2590.1196f, 135f),
		("d_firetower_42", -1029.2435f, 8.445709f, -2294.74f, 0f),
		// d_firetower_43
		("d_firetower_43", -2754.3887f, 576.3475f, -91.513596f, 89f),
		("d_firetower_43", -2742.727f, 576.3475f, 160.559f, 89f),
		("d_firetower_43", -2623.1482f, 532.11285f, 294.717f, 89f),
		("d_firetower_43", -2380.2954f, 532.11285f, 269.87253f, 0f),
		("d_firetower_43", -1735.277f, 537.0874f, 687.3252f, 90f),
		("d_firetower_43", -1549.898f, 537.0874f, 874.87335f, 0f),
		("d_firetower_43", -1275.3779f, 379.69977f, 37.01989f, 0f),
		("d_firetower_43", -1190.1783f, 379.23376f, -206.6874f, 90f),
		("d_firetower_43", -1132.4739f, 358.99908f, -2.1047738f, 90f),
		("d_firetower_43", -954.2613f, 358.999f, 82.1672f, 0f),
		("d_firetower_43", -622.3653f, 451.95105f, 557.3013f, 315f),
		("d_firetower_43", -578.1026f, 358.99896f, -457.14932f, 90f),
		("d_firetower_43", -269.5736f, 358.999f, -448.90982f, 0f),
		("d_firetower_43", -309.0744f, 322.4343f, -620.59094f, 0f),
		("d_firetower_43", -1348.614f, 435.5823f, -873.2341f, 90f),
		("d_firetower_43", -1120.4216f, 435.5824f, -899.49097f, 180f),
		("d_firetower_43", -438.09744f, 435.4824f, -1273.2202f, 0f),
		("d_firetower_43", -276.87286f, 364.47537f, -1345.9663f, 0f),
		("d_firetower_43", -403.50806f, 364.4753f, -1384.676f, 0f),
		("d_firetower_43", 574.664f, 359.41293f, -498.53415f, 0f),
		("d_firetower_43", 848.1769f, 359.4129f, -527.2498f, 0f),
		("d_firetower_43", 644.79266f, 359.41293f, -905.3257f, 0f),
		("d_firetower_43", 784.1568f, 283.9241f, -1276.6217f, 0f),
		("d_firetower_43", 709.68695f, 310.12302f, -1578.3896f, 0f),
		("d_firetower_43", 1484.2256f, 432.84192f, -656.09393f, 0f),
		("d_firetower_43", 1363.0024f, 432.8419f, -656.89667f, 0f),
		("d_firetower_43", 809.8801f, 358.99893f, 130.39406f, 0f),
		("d_firetower_43", 1167.654f, 358.999f, 54.97758f, 0f),
		("d_firetower_43", 1425.916f, 323.70792f, -136.91592f, 270f),
		("d_firetower_43", 865.39886f, 451.2979f, 447.98923f, 180f),
		("d_firetower_43", 1756.2965f, 509.412f, 816.1576f, 0f),
		("d_firetower_43", 1665.9756f, 509.412f, 678.33057f, 90f),
		// d_firetower_44
		("d_firetower_44", 1142.859f, 358.999f, 65.93101f, 0f),
		("d_firetower_44", 1407.351f, 327.91495f, 4.3063865f, 0f),
		("d_firetower_44", 1291.7736f, 323.70786f, -213.96112f, 180f),
		("d_firetower_44", 178.62619f, 440.1674f, 311.51767f, 0f),
		("d_firetower_44", 0.4908285f, 440.08725f, 199.9494f, 90f),
		("d_firetower_44", -621.24f, 359.29242f, 130.73103f, 0f),
		("d_firetower_44", -861.84094f, 394.48877f, -843.73456f, 90f),
		("d_firetower_44", -368.19418f, 364.4753f, -1349.8508f, 0f),
		("d_firetower_44", -389.84216f, 372.1525f, -1579.259f, 89f),
		("d_firetower_44", -64.40803f, 435.4824f, -1206.4785f, 315f),
		("d_firetower_44", -247.14613f, 358.999f, -479.29718f, 0f),
		("d_firetower_44", -950.39886f, 358.999f, 75.71588f, 0f),
		("d_firetower_44", -1027.3634f, 358.999f, 43.602272f, 0f),
		("d_firetower_44", -1254.7715f, 379.42957f, -18.733456f, 0f),
		("d_firetower_44", -908.19073f, 378.0495f, -168.98401f, 270f),
		("d_firetower_44", -784.0229f, 451.715f, 529.9929f, 0f),
		("d_firetower_44", -1731.5397f, 537.0874f, 799.6176f, 90f),
		("d_firetower_44", -2538.9624f, 525.16785f, -183.4927f, 134f),
		// d_firetower_45
		("d_firetower_45", -1122.7567f, 185.1728f, -1979.9756f, 0f),
		("d_firetower_45", -1323.815f, 184.81018f, -2163.8079f, 135f),
		("d_firetower_45", -1723.3497f, 418.82397f, -1230.6351f, 0f),
		("d_firetower_45", -685.90985f, 282.4687f, -1142.9702f, 0f),
		("d_firetower_45", -559.3063f, 282.5837f, -1110.0055f, 0f),
		("d_firetower_45", -598.52924f, 270.3351f, -1291.1067f, 315f),
		("d_firetower_45", -629.942f, 282.221f, -1388.3397f, 179f),
		("d_firetower_45", -1760.3556f, 430.7928f, -643.44165f, 0f),
		("d_firetower_45", -647.3512f, 467.273f, -579.2726f, 45f),
		("d_firetower_45", -561.3298f, 467.4636f, -496.63052f, 0f),
		("d_firetower_45", -508.2792f, 467.4635f, -550.36285f, 0f),
		("d_firetower_45", -612.7362f, 467.10083f, -800.85236f, 315f),
		("d_firetower_45", -1624.1656f, 607.02026f, -162.0476f, 0f),
		("d_firetower_45", -1269.687f, 591.59357f, -4.8835278f, 90f),
		("d_firetower_45", -1083.491f, 619.5389f, -21.379147f, 0f),
		("d_firetower_45", -955.5125f, 668.2239f, -4.3248844f, 0f),
		("d_firetower_45", -1003.70825f, 512.18256f, -215.53433f, 0f),
		("d_firetower_45", -793.17456f, 512.1825f, -11.108732f, 0f),
		("d_firetower_45", -541.6746f, 511.90482f, -12.552185f, 0f),
		("d_firetower_45", -645.8834f, 511.87894f, 1.4816966f, 0f),
		("d_firetower_45", 71.56344f, 373.53336f, 211.37074f, 0f),
		("d_firetower_45", -108.40368f, 246.9436f, 1170.7324f, 0f),
		("d_firetower_45", -231.9237f, 246.63426f, 1125.9866f, 45f),
		("d_firetower_45", -267.1237f, 246.58086f, 1026.6128f, 90f),
		("d_firetower_45", 70.37077f, 150.48903f, 1662.1412f, 0f),
		("d_firetower_45", -138.90761f, 150.12599f, 1530.2765f, 89f),
		("d_firetower_45", 1088.7805f, 194.35695f, 1286.2102f, 0f),
		("d_firetower_45", 549.26355f, 254.26668f, 2371.2302f, 90f),
		("d_firetower_45", 740.31555f, 254.26651f, 2508.389f, 0f),
		("d_firetower_45", 852.2392f, 254.26656f, 2539.471f, 0f),
		("d_firetower_45", 968.6392f, 254.26662f, 2479.874f, 0f),
		("d_firetower_45", 1126.5188f, 254.26668f, 2278.0479f, 0f),
	};

	public FiretowerBookSpawner(int maxPopulation, TimeSpan respawnDelay)
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
				Log.Warning("FiretowerBookSpawner: Failed to find available spawn point.");
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

			var uniqueId = Interlocked.Increment(ref BookIds);
			var uniqueName = $"firetower_book_{uniqueId}";
			var itemId = ItemIds[RandomProvider.Get().Next(ItemIds.Length)];

			var location = new Location(map.Id, position);
			var dir = new Direction(sp.Dir);
			var monsterId = BookMonsterIds[RandomProvider.Get().Next(BookMonsterIds.Length)];

			var book = new Npc(monsterId, "Magic Books", location, dir);
			book.UniqueName = uniqueName;

			book.SetClickTrigger("DYNAMIC_DIALOG", async dialog =>
			{
				var character = dialog.Player;
				var npc = dialog.Npc;

				if (!character.Position.InRange3D(npc.Position, 30))
					return;

				// Aggro nearby monsters
				LureNearbyEnemies(character, 400, 150);

				var result = await character.TimeActions.StartAsync(
					"Collecting book...", "Cancel", "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed && npc.Vars.ActivateOnce($"Npc.{uniqueName}"))
				{
					character.Inventory.Add(itemId, 1, InventoryAddType.PickUp);
					npc.Map?.RemoveMonster(npc);
				}
			});

			this.Spawning?.Invoke(this, new SpawnEventArgs(this, book));

			this.AddPosition(mapClassName, position);

			map.AddMonster(book);

			book.OnDisappear += () =>
			{
				this.Amount--;
				this.RemovePosition(mapClassName, position);
			};

			this.Amount++;

			this.Spawned?.Invoke(this, new SpawnEventArgs(this, book));
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
