//--- Melia Script -----------------------------------------------------------
// Lemprasa Pond Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_siauliai_16'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai16MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_siauliai_16.Id1", MonsterId.Rootcrystal_01, min: 15, max: 20, respawn: Seconds(20));
		AddSpawner("f_siauliai_16.Id2", MonsterId.Sec_Onion, min: 30, max: 40);
		AddSpawner("f_siauliai_16.Id3", MonsterId.Sec_Leaf_Diving, min: 12, max: 15);
		AddSpawner("f_siauliai_16.Id4", MonsterId.Sec_Weaver, min: 19, max: 25);
		AddSpawner("f_siauliai_16.Id5", MonsterId.Sec_Bokchoy, min: 19, max: 25);
		AddSpawner("f_siauliai_16.Id6", MonsterId.Sec_Chupacabra_Blue, min: 12, max: 15);
		AddSpawner("f_siauliai_16.Id7", MonsterId.Sec_Leaf_Diving, min: 19, max: 25);
		AddSpawner("f_siauliai_16.Id8", MonsterId.Sec_Bokchoy, min: 19, max: 25);
		AddSpawner("f_siauliai_16.Id9", MonsterId.Sec_Weaver, min: 6, max: 7);
		AddSpawner("f_siauliai_16.Id10", MonsterId.Sec_Chupaluka, min: 12, max: 15);
		AddSpawner("f_siauliai_16.Id11", MonsterId.Pappus_Kepa_Beige, min: 30, max: 40, respawn: Seconds(30));
		AddSpawner("f_siauliai_16.Id12", MonsterId.Sec_Onion, min: 23, max: 30);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 9 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-77, -1566, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1, -1925, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-536, -1830, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-188, -1283, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(127, -1208, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-134, -962, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(1372, -986, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(1597, -731, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(1180, -768, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(884, -642, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(1325, 727, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(1773, 979, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(2127, 434, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(1992, 118, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(1416, 110, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(1760, 160, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(737, 1242, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(540, 1278, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(166, 1066, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(256, 1429, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(228, 1693, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(700, 1609, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(847, 1813, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-163, 192, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-367, 215, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-397, -408, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-405, 514, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-544, 734, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-516, 1047, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-538, 1286, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-918, 974, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1103, 1163, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-956, 1267, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-871, 1592, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-895, 1884, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1165, 1754, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-485, 1638, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-319, 810, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-515, 448, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-225, 532, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1815, -77, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1745, 242, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1941, 218, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-2073, 384, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-2266, 240, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-2340, 30, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-2430, -177, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-2227, -254, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1948, -422, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1762, -304, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-824, 105, 150));
		AddSpawnPoint("f_siauliai_16.Id1", "f_siauliai_16", Rectangle(-1330, 233, 150));

		// 'Sec_Onion' GenType 17 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id2", "f_siauliai_16", Rectangle(-435, 743, 9999));

		// 'Sec_Leaf_Diving' GenType 18 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id3", "f_siauliai_16", Rectangle(-498, 665, 3000));

		// 'Sec_Weaver' GenType 19 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id4", "f_siauliai_16", Rectangle(-462, 866, 3000));

		// 'Sec_Bokchoy' GenType 20 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id5", "f_siauliai_16", Rectangle(-319, 532, 3000));

		// 'Sec_Chupacabra_Blue' GenType 21 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(-239, -1500, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(-603, -1882, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(-421, -1710, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(-87, -1543, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(14, -1704, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(2, -1895, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(-61, -1977, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(229, -1429, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(93, -1524, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(-161, -1335, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(63, -1128, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(74, -938, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(-175, -975, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(96, -1302, 30));
		AddSpawnPoint("f_siauliai_16.Id6", "f_siauliai_16", Rectangle(-86, -1174, 30));

		// 'Sec_Leaf_Diving' GenType 317 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1804, 74, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2028, -397, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2321, -37, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2207, -130, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2153, -34, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1905, -190, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1776, -207, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2270, -395, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1818, -420, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1711, -298, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1820, 363, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2033, 409, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2316, 341, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2475, -106, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2036, -71, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1603, 222, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1839, 225, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1184, 193, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1992, 244, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1892, 58, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-2333, -222, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1739, 176, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1347, 218, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(-1041, 213, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1308, 653, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1439, 693, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1327, 765, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1423, 849, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1603, 995, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1787, 896, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1886, 993, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(2015, 928, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1941, 845, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1864, 756, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(2005, 656, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(2116, 770, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(2000, 758, 30));
		AddSpawnPoint("f_siauliai_16.Id7", "f_siauliai_16", Rectangle(1738, 1031, 30));

		// 'Sec_Bokchoy' GenType 318 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-2035, 412, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-1945, 0, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-2153, -112, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-2222, -225, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-2426, -15, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-2268, 204, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-2140, 261, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-1718, -100, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-1752, -317, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-1927, -424, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-1950, -302, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-1954, 137, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-1813, 42, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-2496, -182, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(-2123, -387, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1246, 603, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1389, 638, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1295, 698, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1334, 817, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1462, 826, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1550, 1001, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1714, 977, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1780, 1046, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1855, 859, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1806, 779, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1997, 683, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(2027, 872, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1956, 788, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1977, 987, 25));
		AddSpawnPoint("f_siauliai_16.Id8", "f_siauliai_16", Rectangle(1878, 946, 25));

		// 'Sec_Weaver' GenType 319 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1739, -663, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1620, -731, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1523, -832, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1429, -932, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1360, -1048, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1355, -1147, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1305, -935, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1240, -787, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1105, -763, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1379, -821, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(1660, -589, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(907, -748, 25));
		AddSpawnPoint("f_siauliai_16.Id9", "f_siauliai_16", Rectangle(992, -770, 25));

		// 'Sec_Chupaluka' GenType 320 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-462, 586, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-629, 904, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-456, 1139, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-389, 1326, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-474, 1809, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-602, 1211, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-962, 1248, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-844, 1857, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-926, 1585, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-1134, 1644, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-1012, 1132, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-929, 914, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-1092, 997, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-1161, 1239, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-1161, 1370, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-1184, 1517, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-1015, 1920, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-621, 1515, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-560, 1692, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-821, 1701, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-503, 984, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-560, 711, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-404, 448, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-258, 707, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-393, 802, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-328, 987, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-272, 504, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-577, 507, 30));
		AddSpawnPoint("f_siauliai_16.Id10", "f_siauliai_16", Rectangle(-464, 338, 30));

		// 'Pappus_Kepa_Beige' GenType 321 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id11", "f_siauliai_16", Rectangle(-111, -1020, 9999));

		// 'Sec_Onion' GenType 322 Spawn Points
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-93, -1084, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(97, -1372, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(5, -1598, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-64, -1932, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-534, -1904, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-313, -1635, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-229, -1480, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-166, -1156, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-239, -909, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-528, 468, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-266, 618, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-483, 647, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-677, 870, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-523, 1038, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-429, 878, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-480, 1236, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-613, 1277, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-504, 1501, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-730, 1116, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-996, 1084, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1063, 963, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1102, 1318, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1187, 1166, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1132, 1288, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1073, 1640, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-945, 1598, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-859, 1767, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-914, 1914, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1119, 1820, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1009, 1210, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-396, 1148, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(122, 1090, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(222, 1278, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(256, 1425, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(559, 1313, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(666, 1004, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(908, 1668, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(576, 1615, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(709, 1102, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(666, 1218, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(564, 1043, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(775, 1263, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(810, 1648, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(352, 1647, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(132, 1758, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(103, -1193, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-466, -1744, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-25, -1820, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1607, 168, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1423, 273, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1912, 267, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1842, -101, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1913, -187, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-2338, -133, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-2120, 362, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-2041, 378, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-2366, 198, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-2232, -302, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1833, -308, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(-1894, 190, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(877, 1404, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(81, 1221, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(231, 1022, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(165, 1608, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(808, 1882, 30));
		AddSpawnPoint("f_siauliai_16.Id12", "f_siauliai_16", Rectangle(353, 1368, 30));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Lepus, "f_siauliai_16", 1, Hours(2), Hours(4));
		AddBossSpawner(MonsterId.Boss_Sparnas, "f_siauliai_16", 1, Hours(2), Hours(4));
	}
}
