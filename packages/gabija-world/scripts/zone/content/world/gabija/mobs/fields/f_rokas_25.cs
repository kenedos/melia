//--- Melia Script -----------------------------------------------------------
// Ramstis Ridge Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_rokas_25'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas25MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_rokas_25.Id1", MonsterId.Zinute, min: 15, max: 20);
		AddSpawner("f_rokas_25.Id2", MonsterId.Zinute, min: 9, max: 12);
		AddSpawner("f_rokas_25.Id3", MonsterId.Chupacabra_Desert, min: 12, max: 15);
		AddSpawner("f_rokas_25.Id4", MonsterId.Rootcrystal_05, min: 10, max: 13, respawn: Seconds(5));
		AddSpawner("f_rokas_25.Id5", MonsterId.Chupacabra_Desert, min: 19, max: 25);
		AddSpawner("f_rokas_25.Id6", MonsterId.Chupacabra_Desert, min: 15, max: 20);
		AddSpawner("f_rokas_25.Id7", MonsterId.Lichenclops, min: 12, max: 15);

		// Monster Spawn Points -----------------------------

		// 'Zinute' GenType 40 Spawn Points
		AddSpawnPoint("f_rokas_25.Id1", "f_rokas_25", Rectangle(-1184, 614, 9999));

		// 'Zinute' GenType 41 Spawn Points
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-2058, -865, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-1033, 488, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-2347, -713, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-1816, -1222, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-2163, -1204, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-672, 637, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(1968, -522, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(2032, -19, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(2295, -373, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-2236, -1012, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-1979, -1195, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-1830, -860, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-2218, -636, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-1854, -1034, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-2044, -1050, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-2518, -688, 25));
		AddSpawnPoint("f_rokas_25.Id2", "f_rokas_25", Rectangle(-2346, -875, 25));

		// 'Chupacabra_Desert' GenType 43 Spawn Points
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2235, -724, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(1749, -410, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(2335, -112, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(2364, -504, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2077, -1125, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(810, 1060, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(1934, -41, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(2031, -502, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2101, 320, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2362, -924, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-1945, -1045, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2064, -739, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2464, -701, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-1810, -855, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-1835, -1271, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-1722, -1173, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-1701, -931, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2146, -921, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2032, -838, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-1996, 422, 25));
		AddSpawnPoint("f_rokas_25.Id3", "f_rokas_25", Rectangle(-2296, 402, 25));

		// 'Rootcrystal_05' GenType 600 Spawn Points
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1994, -546, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1054, 502, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(925, -111, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2194, -429, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-2010, -1146, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1798, -904, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-2357, -827, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-2348, -541, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-2200, 326, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-2161, 499, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1987, 292, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1851, 937, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1902, 822, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-2097, 724, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1680, 920, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1491, 670, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-793, 544, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-1017, 810, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-716, 230, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-939, 293, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(-11, 571, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(148, 666, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(306, 478, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(367, 594, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(454, 401, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(888, 1131, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(653, 1063, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(1182, -285, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(1264, -34, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(1735, -33, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(1839, -611, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2042, 22, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2472, -586, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2399, -104, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2109, 604, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2307, 474, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(1989, 279, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2847, -97, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(3128, -171, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2927, -435, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2179, -819, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2728, -905, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(2352, -1139, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(1423, -933, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(1686, -849, 150));
		AddSpawnPoint("f_rokas_25.Id4", "f_rokas_25", Rectangle(1688, -1246, 150));

		// 'Chupacabra_Desert' GenType 657 Spawn Points
		AddSpawnPoint("f_rokas_25.Id5", "f_rokas_25", Rectangle(-2047, 704, 9999));

		// 'Chupacabra_Desert' GenType 670 Spawn Points
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(1078, -178, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(1853, -159, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(2243, -444, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(1805, -441, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(1966, 60, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(2130, -600, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(2497, -570, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(2474, -159, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(1604, -290, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(1069, -308, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(916, -76, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(1155, -4, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-953, 462, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-960, 657, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-681, 714, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-834, 560, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-636, 427, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-1224, 494, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-1109, 635, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-553, 545, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(-941, 860, 25));
		AddSpawnPoint("f_rokas_25.Id6", "f_rokas_25", Rectangle(2048, -294, 25));

		// 'Lichenclops' GenType 671 Spawn Points
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(-957, 599, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(-764, 396, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(-743, 630, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1021, -164, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1153, 10, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1783, -326, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2128, -508, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2421, -292, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2200, -49, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2828, -193, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2941, -424, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1745, -1093, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1446, -949, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2478, -1179, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2640, -938, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2460, -877, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(-1081, 479, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(-1246, 676, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1152, -356, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1996, -173, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1685, -540, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(2502, -603, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(3056, -268, 30));
		AddSpawnPoint("f_rokas_25.Id7", "f_rokas_25", Rectangle(1568, -1166, 30));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_BiteRegina, "f_rokas_25", 1, Hours(6), Hours(12));
	}
}
