//--- Melia Script -----------------------------------------------------------
// Woods of the Linked Bridges Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_siauliai_15_re'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai15ReMobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_siauliai_15_re.Id1", MonsterId.Sec_Jukopus, min: 30, max: 40);
		AddSpawner("f_siauliai_15_re.Id2", MonsterId.Onion_Green, min: 30, max: 40);
		AddSpawner("f_siauliai_15_re.Id3", MonsterId.Sec_Pokubu, min: 23, max: 30);
		AddSpawner("f_siauliai_15_re.Id4", MonsterId.Rootcrystal_01, min: 19, max: 25, respawn: Seconds(25));
		AddSpawner("f_siauliai_15_re.Id5", MonsterId.Sec_Jukopus, min: 15, max: 20);
		AddSpawner("f_siauliai_15_re.Id6", MonsterId.Sec_Pokubu, min: 30, max: 40);
		AddSpawner("f_siauliai_15_re.Id7", MonsterId.Sec_Arburn_Pokubu, min: 15, max: 20);
		AddSpawner("f_siauliai_15_re.Id8", MonsterId.Onion_Green, min: 23, max: 30);
		AddSpawner("f_siauliai_15_re.Id9", MonsterId.Onion_Green, min: 23, max: 30);
		AddSpawner("f_siauliai_15_re.Id10", MonsterId.Sec_Jukopus, min: 23, max: 30);
		AddSpawner("f_siauliai_15_re.Id11", MonsterId.Sec_Arburn_Pokubu, min: 12, max: 15);
		AddSpawner("f_siauliai_15_re.Id12", MonsterId.Sec_Jukopus, min: 8, max: 10);
		AddSpawner("f_siauliai_15_re.Id13", MonsterId.Onion_Green, min: 10, max: 15, respawn: Seconds(20));
		AddSpawner("f_siauliai_15_re.Id14", MonsterId.Sec_Pokubu, min: 15, max: 20, respawn: Seconds(20));

		// Monster Spawn Points -----------------------------

		// 'Sec_Jukopus' GenType 11 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id1", "f_siauliai_15_re", Rectangle(-820, -67, 9999));

		// 'Onion_Green' GenType 12 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id2", "f_siauliai_15_re", Rectangle(-786, -82, 9999));

		// 'Sec_Pokubu' GenType 13 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id3", "f_siauliai_15_re", Rectangle(-739, -111, 9999));

		// 'Rootcrystal_01' GenType 15 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(463, -103, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2806, 52, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2592, -39, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2375, -473, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2508, -1231, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2047, -1053, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1951, -1544, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2171, -1577, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1921, -2014, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2196, -1952, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2089, -6, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(2161, -742, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1494, -41, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1450, 467, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1420, 995, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1011, 1033, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(648, 1247, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1443, 1723, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1290, 1568, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(914, 1921, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(719, 2229, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(288, 2416, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-82, 2701, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-392, 2318, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(194, 1105, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1, 903, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-403, 1052, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-627, 1322, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-1249, 1231, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(1101, -65, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(720, -3, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(701, -426, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-43, -39, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-16, -571, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-294, -772, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-2, -1075, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-138, -1282, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-290, -1597, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(52, -1604, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(75, -2267, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(4, -2530, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-265, -2454, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-525, -2502, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-578, -2841, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-312, -2827, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-397, -3127, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-811, -110, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-1004, 161, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-1302, 73, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-1606, 327, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-1752, 28, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-1407, -271, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-1812, -220, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-2187, -114, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-2361, -419, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-2626, -147, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-3025, -151, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-3370, -132, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-2889, 274, 150));
		AddSpawnPoint("f_siauliai_15_re.Id4", "f_siauliai_15_re", Rectangle(-2749, 648, 150));

		// 'Sec_Jukopus' GenType 314 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2567, 50, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2792, -53, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2612, -188, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2418, -325, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2119, -23, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2252, 94, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2392, 80, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2676, -20, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2887, 0, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2501, -157, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2285, -4, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2305, -234, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2371, -431, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2672, -82, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2821, 74, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2520, -325, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2346, -115, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2258, -410, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2477, -448, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2427, -225, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2539, -52, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(2071, 90, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1404, -157, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1304, -94, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1206, -154, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1263, -9, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1244, 127, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1393, -9, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1542, 66, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1547, -95, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1672, 59, 20));
		AddSpawnPoint("f_siauliai_15_re.Id5", "f_siauliai_15_re", Rectangle(1907, 64, 20));

		// 'Sec_Pokubu' GenType 315 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(551, -351, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(410, -343, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(310, -192, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(471, -188, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(670, -42, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(623, -229, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(810, -21, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(787, -149, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-474, -50, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(1092, 30, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(790, -434, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(675, -480, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(1068, -217, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(952, 20, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-43, -505, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(33, -47, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-53, -313, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-285, -53, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-992, 162, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-1007, 178, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-1475, 249, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-1780, 341, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-1696, 46, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-1308, -252, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-1476, -275, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-1841, -202, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-2165, -46, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-2410, -13, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-2303, -341, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-2412, -340, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-2830, -66, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-3350, -148, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-3408, -113, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-2929, 446, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-2937, 767, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-3011, 952, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(-2917, 553, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(458, -266, 20));
		AddSpawnPoint("f_siauliai_15_re.Id6", "f_siauliai_15_re", Rectangle(677, -416, 20));

		// 'Sec_Arburn_Pokubu' GenType 316 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-643, -2819, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-421, -2974, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-468, -3166, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-346, -3097, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-557, -2947, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-319, -2850, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-616, -2656, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-634, -2529, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-286, -2596, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-18, -2365, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(65, -2486, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-89, -2542, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-226, -2430, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-414, -2526, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-443, -2745, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(2054, -2100, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(1968, -2006, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(1918, -1727, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(2016, -1487, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(2176, -1605, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(2225, -1880, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(2144, -2023, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(1871, -1906, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(1840, -1995, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(2420, -1293, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(2297, -1166, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(2076, -1128, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(1999, -1252, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(32, -2240, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(88, -1893, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(100, -1595, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-66, -1581, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-14, -1388, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-234, -1290, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-344, -1535, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-271, -1693, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-120, -1442, 30));
		AddSpawnPoint("f_siauliai_15_re.Id7", "f_siauliai_15_re", Rectangle(-177, -1155, 30));

		// 'Onion_Green' GenType 317 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(-154, 802, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(-101, 884, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(36, 1035, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1126, 1092, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(290, 1094, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(514, 1210, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(688, 1176, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(797, 1278, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(688, 1021, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(474, 1027, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(42, 940, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(67, 840, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(178, 1138, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(-433, 1125, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(-649, 1302, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(-368, 961, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(884, 1089, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1048, 994, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(927, 1227, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(968, 1369, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(660, 1400, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2161, -2002, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2065, -2028, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1831, -1746, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1951, -1583, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2212, -1627, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2292, -1382, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2171, -1132, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1885, -1261, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2060, -1544, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2106, -1958, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1952, -2006, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2074, -1918, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(2185, -1767, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1109, -80, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1155, 107, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1205, -57, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1191, -189, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1370, -214, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1392, 26, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1394, -128, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1518, -175, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1492, -277, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1379, -331, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1498, 107, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1643, 12, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1032, -172, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(1003, 51, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(996, -62, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(-611, 1219, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(-283, 871, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(-48, 994, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(149, 1220, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(70, 1107, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(181, 1052, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(645, 1119, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(477, 1145, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(572, 1380, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(865, 1180, 20));
		AddSpawnPoint("f_siauliai_15_re.Id8", "f_siauliai_15_re", Rectangle(870, 1180, 20));

		// 'Onion_Green' GenType 321 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(-385, 2256, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(-400, 2314, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(-405, 2378, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(-280, 2748, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(-119, 2757, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(25, 2461, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(48, 2355, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(200, 2479, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(217, 2362, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(377, 2386, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(480, 2296, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(470, 2126, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(622, 2083, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(600, 2221, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(847, 2361, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(683, 2428, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(513, 2434, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(812, 2160, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(761, 1999, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(945, 1988, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(968, 1817, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(968, 1735, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1102, 1685, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1351, 1564, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1357, 1753, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1478, 1812, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1643, 1647, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1531, 1635, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1297, 1630, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1270, 1492, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1182, 1424, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1490, 1495, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1445, 1722, 30));
		AddSpawnPoint("f_siauliai_15_re.Id9", "f_siauliai_15_re", Rectangle(1418, 1780, 30));

		// 'Sec_Jukopus' GenType 322 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(193, 2420, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(494, 2432, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(512, 2208, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(740, 2344, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(709, 2151, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(774, 2017, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(933, 2064, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(909, 1903, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(896, 1728, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1088, 1673, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1444, 1588, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1337, 1736, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1304, 1575, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1257, 1443, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1442, 1459, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1482, 1738, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1378, 1875, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1622, 1749, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(1633, 1636, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-472, 2277, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-408, 2120, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-331, 2234, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-514, 2465, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-390, 2616, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-246, 2801, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-177, 2836, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-125, 2617, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-478, 2604, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(-63, 2448, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(119, 2391, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(152, 2506, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(447, 2451, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(461, 2281, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(542, 2344, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(560, 2152, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(861, 2255, 30));
		AddSpawnPoint("f_siauliai_15_re.Id10", "f_siauliai_15_re", Rectangle(798, 2403, 30));

		// 'Sec_Arburn_Pokubu' GenType 323 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(-431, 2209, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(-484, 2394, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(-437, 2609, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(-185, 2704, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(-32, 2401, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(286, 2438, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(399, 2431, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(626, 2273, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(728, 2274, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(915, 2044, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(860, 1961, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(921, 1740, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(1219, 1548, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(1246, 1727, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(1476, 1681, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(1568, 1857, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(1612, 1554, 30));
		AddSpawnPoint("f_siauliai_15_re.Id11", "f_siauliai_15_re", Rectangle(1439, 1492, 30));

		// 'Sec_Jukopus' GenType 324 Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(272, -198, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(361, -255, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(411, -186, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(372, -100, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(600, -98, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(647, -159, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(508, -121, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(477, -287, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(349, -327, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(225, -255, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(561, -407, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(743, -119, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(631, -271, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(443, -365, 25));
		AddSpawnPoint("f_siauliai_15_re.Id12", "f_siauliai_15_re", Rectangle(491, -215, 25));

		// Onion_Green Spawn points
		AddSpawnPoint("f_siauliai_15_re.Id13", "f_siauliai_15_re", Rectangle(2060, -1595, 300));

		// Sec_Pokubu Spawn Points
		AddSpawnPoint("f_siauliai_15_re.Id14", "f_siauliai_15_re", Rectangle(710, 2260, 300));
		AddSpawnPoint("f_siauliai_15_re.Id14", "f_siauliai_15_re", Rectangle(-175, 2718, 300));
		AddSpawnPoint("f_siauliai_15_re.Id14", "f_siauliai_15_re", Rectangle(1400, 1625, 300));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Bebraspion, "f_siauliai_15_re", 1, Hours(2), Hours(4));
	}
}
