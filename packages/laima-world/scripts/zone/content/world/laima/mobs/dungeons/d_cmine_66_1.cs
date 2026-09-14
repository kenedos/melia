//--- Melia Script -----------------------------------------------------------
// Nevellet Quarry 1F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_cmine_66_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine661MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_cmine_66_1.Id1", MonsterId.Rootcrystal_03, min: 14, max: 18, respawn: Seconds(5), tendency: TendencyType.Peaceful);
		AddSpawner("d_cmine_66_1.Id2", MonsterId.FD_Corylus, min: 25, max: 33, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_1.Id3", MonsterId.FD_Yognome_Sec, min: 29, max: 38, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_1.Id4", MonsterId.FD_Pawnd, min: 42, max: 55, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_1.Id5", MonsterId.FD_Glizardon, min: 9, max: 12, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_1.Id6", MonsterId.FD_Pawnd, min: 25, max: 33, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_1.Id7", MonsterId.FD_Pawnd, min: 25, max: 33, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 3 Spawn Points
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-407, -756, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(283, -1638, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(152, -1300, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(1008, -1586, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(1169, -1399, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(1375, -1644, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(1426, -638, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(1047, -823, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(1197, -545, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(1047, -285, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(215, -384, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(302, -658, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(0, -731, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-752, -299, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-856, -823, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-607, -1542, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-742, -1861, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-992, -1639, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-1405, -1504, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-1303, -606, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-1453, -1096, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-1714, 336, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-1587, 577, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-78, -472, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-698, 1372, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-1060, 1530, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-702, 1965, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(977, 2067, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(1249, 1551, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(918, 1373, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(6, 1459, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-123, 1770, 50));
		AddSpawnPoint("d_cmine_66_1.Id1", "d_cmine_66_1", Rectangle(-1297, 1701, 50));

		// 'FD_Corylus' GenType 100 Spawn Points
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(267, -264, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-23, -600, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-95, -321, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(174, -576, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(204, -818, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(365, -566, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-998, -1530, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-1022, -1782, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-816, -1791, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-786, -1577, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-611, -1827, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-636, -1684, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1049, -745, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(938, -599, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1092, -313, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1192, -437, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1417, -273, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1447, -728, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-1798, 499, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-1800, 323, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-1621, 421, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-1570, 600, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-1539, 288, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(985, -1599, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1085, -1337, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1167, -1711, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1314, -1578, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1308, -1365, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1490, -1519, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-967, -1665, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-817, -1675, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-829, -1930, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-84, -752, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(-144, -529, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(117, -387, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1170, -647, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1381, -574, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1004, -384, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(1121, -1492, 35));
		AddSpawnPoint("d_cmine_66_1.Id2", "d_cmine_66_1", Rectangle(927, -1395, 35));

		// 'FD_Yognome_Sec' GenType 101 Spawn Points
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(1207, -266, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(993, -410, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(1099, -588, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(1356, -457, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(1306, -646, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(1282, -762, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(1170, -861, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-821, -194, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(339, -1434, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(25, -442, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(111, -1216, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-327, -813, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-144, -627, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(102, -343, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(372, -446, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(307, -767, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(10, -832, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-924, -1895, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-526, -1569, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(1046, -701, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(79, -631, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(213, -509, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-154, -391, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-927, -1676, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-682, -1733, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(-781, -1861, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(905, -1525, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(544, -1387, 30));
		AddSpawnPoint("d_cmine_66_1.Id3", "d_cmine_66_1", Rectangle(549, -1471, 30));

		// 'FD_Pawnd' GenType 102 Spawn Points
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(-1506, -1326, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(-1614, -1094, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(-1294, -1268, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(-1327, -1046, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(-1366, -1449, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(-245, 1736, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(24, 1769, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(43, 2051, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(415, 1724, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(141, 1410, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(-168, 1489, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(193, 1828, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(400, 1466, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(153, 1608, 30));
		AddSpawnPoint("d_cmine_66_1.Id4", "d_cmine_66_1", Rectangle(-1455, -1159, 30));

		// 'FD_Glizardon' GenType 103 Spawn Points
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(-88, 1518, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(246, 1867, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(279, 1457, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(-857, -719, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(118, -1075, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(1027, -1720, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(919, -1417, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(1266, -1486, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(1439, -1411, 20));
		AddSpawnPoint("d_cmine_66_1.Id5", "d_cmine_66_1", Rectangle(-137, 1891, 20));

		// 'FD_Pawnd' GenType 104 Spawn Points
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-983, 1080, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-1069, 1338, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-635, 1340, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-1105, 1642, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-707, 1501, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-704, 1770, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-1055, 2039, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-925, 2344, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-661, 1984, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-913, 1666, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-809, 1189, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-884, 1387, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-899, 1962, 30));
		AddSpawnPoint("d_cmine_66_1.Id6", "d_cmine_66_1", Rectangle(-817, 2195, 30));

		// 'FD_Pawnd' GenType 105 Spawn Points
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(1123, 1139, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(1251, 1366, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(779, 1411, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(1313, 1625, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(1248, 1996, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(819, 1574, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(1158, 2347, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(811, 1790, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(990, 2184, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(942, 1257, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(1043, 1468, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(1080, 1664, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(1048, 1945, 30));
		AddSpawnPoint("d_cmine_66_1.Id7", "d_cmine_66_1", Rectangle(765, 1994, 30));
	}
}
