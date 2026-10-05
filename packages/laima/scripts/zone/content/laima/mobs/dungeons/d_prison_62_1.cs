//--- Melia Script -----------------------------------------------------------
// Ashaq Underground Prison 1F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_prison_62_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison621MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_prison_62_1.Id1", MonsterId.Wendigo_Blue, min: 75, max: 90, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_1.Id2", MonsterId.Rootcrystal_01, min: 19, max: 25, respawn: Seconds(25), tendency: TendencyType.Peaceful);
		AddSpawner("d_prison_62_1.Id3", MonsterId.Dumaro_Blue, min: 75, max: 90, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_1.Id4", MonsterId.Wendigo_Blue, min: 64, max: 75, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_1.Id5", MonsterId.Sec_Yekubite, min: 47, max: 55, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_1.Id6", MonsterId.Goblin_Miners_Blue, min: 65, max: 80, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Wendigo_Blue' GenType 11 Spawn Points
		AddSpawnPoint("d_prison_62_1.Id1", "d_prison_62_1", Rectangle(-1362, 52, 9999));

		// 'Rootcrystal_01' GenType 13 Spawn Points
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-519, 43, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1291, -1696, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1384, -1415, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1106, -1383, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1367, -967, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1325, -523, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1530, -16, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1190, 276, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1331, 798, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-1335, 1429, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-435, -1142, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(-465, -654, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(84, -1079, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(498, -1392, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(1291, -1408, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(1312, -1035, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(804, -1134, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(764, -222, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(1502, -141, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(80, -290, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(44, 1383, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(473, 1342, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(1005, 1405, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(1748, 1323, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(1957, 906, 50));
		AddSpawnPoint("d_prison_62_1.Id2", "d_prison_62_1", Rectangle(1686, 1090, 50));

		// 'Dumaro_Blue' GenType 28 Spawn Points
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-479, -1189, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-346, -1056, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-518, -1472, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-414, -591, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-397, -774, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(675, -74, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(19, -955, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-546, -745, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1073, -207, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1439, 195, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1603, 194, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1340, 244, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1183, 352, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1252, 15, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1478, 57, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1362, -569, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1386, -782, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1383, -925, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1388, -1076, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1373, -1345, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1450, -1522, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1430, -1802, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1284, -1657, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1185, -1343, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1043, -1340, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-978, -1461, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1111, -1583, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1111, -1583, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1346, 321, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1351, 555, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1350, 795, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1383, 1054, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1396, 1307, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1448, 1626, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1355, 1528, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1288, 1500, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1191, 1266, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1219, 1375, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1214, 1600, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1214, 131, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1533, -115, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-1632, -87, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(170, -310, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(752, -404, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(910, -392, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1107, -208, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1358, -196, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1544, -236, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1686, -78, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1636, -188, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1438, -11, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(829, -959, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(700, -1123, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(483, -983, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(331, -1252, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(638, -1222, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(815, -1304, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1250, -1249, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1382, -1254, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1258, -942, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1443, -1472, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1268, -1458, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1000, -1201, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1566, 592, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1588, 1046, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1950, 1341, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(86, 1414, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1003, 1337, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(357, 1353, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(-74, 1222, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(270, 1440, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(285, 1218, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1138, 1523, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1119, 1428, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(969, 1489, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1605, 1367, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1671, 1479, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1983, 1086, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1889, 1200, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1924, 803, 30));
		AddSpawnPoint("d_prison_62_1.Id3", "d_prison_62_1", Rectangle(1890, 588, 30));

		// 'Wendigo_Blue' GenType 29 Spawn Points
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(20, 1304, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(360, 1231, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(910, 1279, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1158, 1521, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(567, 1211, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1177, 1239, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(889, 1400, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(415, 1530, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-117, 1454, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(109, 1214, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(996, 1589, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1012, 1377, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1767, 1283, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1795, 1441, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1780, 1080, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1891, 973, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1676, 666, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1629, 907, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1789, 837, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1811, 696, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1977, 657, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1680, -335, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1389, -331, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1564, -51, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(913, -105, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(421, -316, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(820, -67, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1349, -1346, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1014, -995, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(1517, -1147, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(293, -981, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(196, -1339, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-503, -1265, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-377, -1386, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-439, -889, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(52, -1441, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-1261, -1805, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-1276, -1536, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-1154, -1449, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-1350, -236, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-1402, 445, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-1323, 1437, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-1475, 1518, 35));
		AddSpawnPoint("d_prison_62_1.Id4", "d_prison_62_1", Rectangle(-1310, 1634, 35));

		// 'Sec_Yekubite' GenType 30 Spawn Points
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1418, 1414, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1492, 1395, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1586, 296, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1138, 238, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1355, 948, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1339, 132, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1365, -111, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1433, -198, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1359, -441, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1180, -124, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1629, 116, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1343, -1209, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1278, -1430, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1468, -1694, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1334, -1722, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-1124, -1704, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-504, -1042, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(194, -1172, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-563, -626, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(329, -1429, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(45, -1256, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(83, -1085, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(474, -1195, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(532, -1359, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(981, -1396, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1501, -1296, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1455, -973, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1476, -1055, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1095, -1332, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1228, -1072, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1688, 10, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1538, -360, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1492, -121, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(846, -237, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(647, -308, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(704, -205, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(-14, -311, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1955, 855, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1578, 745, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1611, 1180, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1606, 1374, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(1214, 1411, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(541, 1359, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(123, 1386, 30));
		AddSpawnPoint("d_prison_62_1.Id5", "d_prison_62_1", Rectangle(249, 1301, 30));

		// 'Goblin_Miners_Blue' GenType 31 Spawn Points
		AddSpawnPoint("d_prison_62_1.Id6", "d_prison_62_1", Rectangle(1064, 1281, 9999));
	}
}
