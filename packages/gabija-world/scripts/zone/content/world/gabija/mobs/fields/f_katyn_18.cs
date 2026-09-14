//--- Melia Script -----------------------------------------------------------
// Kule Peak Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_katyn_18'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn18MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_katyn_18.Id1", MonsterId.Zibu_Maize_Red, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("f_katyn_18.Id2", MonsterId.Siaulav_Red, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("f_katyn_18.Id3", MonsterId.Siaulav_Bow_Black, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("f_katyn_18.Id4", MonsterId.Siaulav_Mage_Black, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("f_katyn_18.Id5", MonsterId.Siaulav_Bow_Black, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("f_katyn_18.Id6", MonsterId.Rootcrystal_01, min: 16, max: 21, respawn: Minutes(1), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Zibu_Maize_Red' GenType 21 Spawn Points
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(181, -890, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(123, -657, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(163, -389, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(243, -350, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(537, -754, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(459, -1082, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(688, -1196, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(875, -810, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(567, 385, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(546, 932, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(670, 1315, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(1038, 1349, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(1566, 1292, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2036, 1332, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2223, 914, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2101, 864, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2146, 465, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2222, 469, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2175, 609, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2161, 601, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2121, 138, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(1918, -56, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(1741, -400, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2013, -482, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(2114, -135, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(1950, -264, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(1503, -111, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(1493, 105, 40));
		AddSpawnPoint("f_katyn_18.Id1", "f_katyn_18", Rectangle(1878, 85, 40));

		// 'Siaulav_Red' GenType 22 Spawn Points
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(893, -2607, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(876, -2463, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(800, -2334, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(830, -2138, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(820, -1886, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(783, -1762, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(793, -1333, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(854, -1298, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1171, -1388, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1403, -1315, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1692, -1339, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1633, -1124, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(791, -1095, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(522, -1018, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(722, -1529, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1376, -2010, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1221, -2328, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1100, -2200, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1240, -2106, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1497, -2543, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1521, -2464, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1915, -2075, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1649, -2046, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2002, -1846, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2273, -1702, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2394, -1381, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2460, -960, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2301, -1028, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2248, -779, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2205, -560, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2000, -579, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2053, -276, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1775, -336, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1812, -155, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2014, 45, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(889, -1677, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(701, -2216, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(610, -2166, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(176, -2479, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(216, -2255, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(80, -2361, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(99, -2231, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(100, -1925, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(-77, -2236, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(-114, -2476, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(2289, -1975, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1585, -225, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1433, -285, 40));
		AddSpawnPoint("f_katyn_18.Id2", "f_katyn_18", Rectangle(1448, -479, 40));

		// 'Siaulav_Bow_Black' GenType 23 Spawn Points
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-495, -2445, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-646, -2521, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1015, -2366, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1132, -2258, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1178, -1794, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-997, -1660, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1285, -1633, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1275, -1347, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1381, -1193, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1239, -1220, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1423, -766, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-979, -742, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1078, -431, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1148, -561, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1035, -165, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1735, -257, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1549, -702, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1750, 324, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1866, 258, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1688, 78, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1805, 651, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-982, -1070, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1120, -1138, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1264, -1488, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-1238, -1982, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-975, -2477, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-801, -2448, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-328, -2481, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-37, -2298, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(-24, -2447, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(64, -2519, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(193, -2298, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(128, -2337, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(273, -2422, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(486, -2119, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(293, -2191, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(101, -2042, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(195, -1960, 40));
		AddSpawnPoint("f_katyn_18.Id3", "f_katyn_18", Rectangle(318, -2000, 40));

		// 'Siaulav_Mage_Black' GenType 24 Spawn Points
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1761, -1203, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1576, -1481, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1510, -1187, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1295, -1386, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1059, -1263, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(848, -1477, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(688, -978, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(297, -893, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(365, -334, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(98, -590, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(394, -646, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(627, -515, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1416, -260, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1345, -104, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1540, 209, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1694, -163, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1791, -531, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(2066, -246, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1970, 150, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1687, -413, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1305, -129, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(1167, 23, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1135, -1627, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1277, -1818, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-982, -1628, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1074, -1849, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1116, -1710, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1367, -1361, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1237, -917, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1428, -1035, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1437, -766, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1102, -841, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1018, -411, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1065, -580, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1059, -148, 40));
		AddSpawnPoint("f_katyn_18.Id4", "f_katyn_18", Rectangle(-1237, -1107, 40));

		// 'Siaulav_Bow_Black' GenType 25 Spawn Points
		AddSpawnPoint("f_katyn_18.Id5", "f_katyn_18", Rectangle(1510, -263, 9999));

		// 'Rootcrystal_01' GenType 26 Spawn Points
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(716, 1235, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(931, 1295, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(2161, 1061, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(2053, -362, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(1273, -383, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(1675, 18, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(886, -685, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(515, -521, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(533, 278, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(733, -1430, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(1597, -1273, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(2425, -1110, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(2168, -2029, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(1242, -2209, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(89, -2305, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(-424, -2468, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(-978, -2415, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(-1056, -1742, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(-1332, -1245, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(-1017, -883, 100));
		AddSpawnPoint("f_katyn_18.Id6", "f_katyn_18", Rectangle(-1765, 222, 100));
	}
}
