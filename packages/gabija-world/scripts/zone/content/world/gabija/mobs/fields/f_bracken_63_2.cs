//--- Melia Script -----------------------------------------------------------
// Knidos Jungle Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_bracken_63_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken632MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_bracken_63_2.Id1", MonsterId.Loktanun, min: 23, max: 30);
		AddSpawner("f_bracken_63_2.Id2", MonsterId.Ponpon, min: 23, max: 30);
		AddSpawner("f_bracken_63_2.Id3", MonsterId.Kanchobird, min: 23, max: 30);
		AddSpawner("f_bracken_63_2.Id4", MonsterId.Lapasape_Mage, min: 12, max: 15);
		AddSpawner("f_bracken_63_2.Id5", MonsterId.Rootcrystal_01, min: 24, max: 31, respawn: Seconds(20));
		AddSpawner("f_bracken_63_2.Id6", MonsterId.Ponpon, min: 27, max: 35);
		AddSpawner("f_bracken_63_2.Id7", MonsterId.Loktanun, min: 8, max: 10);
		AddSpawner("f_bracken_63_2.Id8", MonsterId.Lapasape_Mage, min: 8, max: 10);
		AddSpawner("f_bracken_63_2.Id9", MonsterId.Lapasape_Mage, min: 16, max: 21);

		// Monster Spawn Points -----------------------------

		// 'Loktanun' GenType 23 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1264, -249, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(652, 1861, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-543, 1380, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(409, -1187, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(881, -1873, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1049, -1060, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(415, -125, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-1416, -1422, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-1592, -529, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-1538, 126, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-570, 1554, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-349, 1750, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-346, 1517, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-517, 1253, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-593, 1686, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-285, 1444, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-114, 1208, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-59, 950, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-84, 869, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-653, 1395, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-298, 1609, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(-180, 1356, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(669, 33, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(737, -156, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(583, -403, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(244, -374, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(248, -102, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(202, 99, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(514, 40, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(467, -316, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1328, 375, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1480, 460, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1447, 552, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1278, 540, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1342, 662, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1333, 681, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1163, 777, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(949, 981, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1077, 1034, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1171, 1184, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1014, 1192, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1111, 950, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1323, 1022, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1333, 871, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1237, 952, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1531, 266, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1567, 97, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1427, 322, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1566, 406, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1211, 444, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(1057, 874, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(719, 490, 30));
		AddSpawnPoint("f_bracken_63_2.Id1", "f_bracken_63_2", Rectangle(879, 503, 30));

		// 'Ponpon' GenType 24 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1299, -233, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(562, 1616, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(-475, 1478, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(368, 1585, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(362, 1748, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(559, 1834, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(514, 2015, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(725, 2178, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(568, 2119, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(779, 1910, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(863, 1920, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(741, 1750, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(683, 1946, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(455, 1628, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(284, 1530, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1191, -424, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1263, -350, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1162, -126, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1392, -95, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1393, -281, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1316, -517, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1210, -301, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1154, -177, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1058, -216, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1021, -301, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1125, -46, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1432, -219, 30));
		AddSpawnPoint("f_bracken_63_2.Id2", "f_bracken_63_2", Rectangle(1266, -66, 30));

		// 'Kanchobird' GenType 25 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(-1456, -348, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(718, -1855, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(755, -1724, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(1015, -1648, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(922, -1811, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(893, -1931, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(420, -1464, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(226, -1264, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(306, -977, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(484, -931, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(562, -1227, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(607, -1436, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(920, -1045, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(902, -963, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(992, -834, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(1177, -1031, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(1101, -1181, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(399, -388, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(136, -471, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(477, -467, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(693, -253, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(753, -113, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(632, -38, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(517, -268, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(289, 38, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(88, 124, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(854, -204, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(600, -489, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(115, -1351, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(359, -1194, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(117, -1108, 30));
		AddSpawnPoint("f_bracken_63_2.Id3", "f_bracken_63_2", Rectangle(-50, -1113, 30));

		// 'Lapasape_Mage' GenType 27 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1419, 231, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1397, -1641, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1538, -1609, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1337, -1462, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1544, -1313, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1719, -1236, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1302, -1237, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1669, -1389, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1606, -1623, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1498, -661, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1655, -342, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1449, -367, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1468, -521, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1285, -510, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1269, -716, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1194, -421, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1710, -615, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1613, 84, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1608, 242, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1444, 348, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1393, 159, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1467, 41, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1290, 74, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1242, 205, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1692, 359, 30));
		AddSpawnPoint("f_bracken_63_2.Id4", "f_bracken_63_2", Rectangle(-1688, 104, 30));

		// 'Rootcrystal_01' GenType 303 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(666, 2012, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(543, 1732, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(308, 1067, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(639, 726, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(393, 411, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(1397, 394, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(1350, -416, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(1205, -112, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(927, 1013, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(1265, 792, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(1286, 1156, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(155, -455, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(143, 30, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(656, -397, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(630, 118, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(487, 17, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(1140, -1226, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(863, -823, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(209, -1172, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(658, -1281, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(774, -1714, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(919, -1934, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-1643, -1436, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-1507, -1133, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-1334, -1613, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-1892, -506, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-1635, -329, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-1209, -526, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-1638, 172, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-1357, 222, 10));
		AddSpawnPoint("f_bracken_63_2.Id5", "f_bracken_63_2", Rectangle(-460, 1502, 10));

		// 'Ponpon' GenType 305 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id6", "f_bracken_63_2", Rectangle(576, -200, 9999));

		// 'Loktanun' GenType 306 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id7", "f_bracken_63_2", Rectangle(-1458, -1449, 300));

		// 'Lapasape_Mage' GenType 307 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id8", "f_bracken_63_2", Rectangle(-1569, -450, 340));

		// 'Lapasape_Mage' GenType 313 Spawn Points
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(845, -1804, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(716, -1910, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(868, -1944, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(950, -1700, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(675, -1639, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(556, -1418, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(248, -1331, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(338, -975, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1063, -951, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1152, -1152, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(922, -1035, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(898, -849, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1332, -149, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1367, -354, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1191, -341, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1396, -272, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1445, 441, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1554, 297, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1385, 339, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1379, -609, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(1225, -139, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(213, -1168, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(216, -1322, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(256, -1082, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(382, -1246, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(648, -1217, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(434, -1673, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(373, -1431, 100));
		AddSpawnPoint("f_bracken_63_2.Id9", "f_bracken_63_2", Rectangle(566, -965, 100));
	}
}
