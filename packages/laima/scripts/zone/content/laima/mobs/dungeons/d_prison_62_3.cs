//--- Melia Script -----------------------------------------------------------
// Ashaq Underground Prison 3F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_prison_62_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison623MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_prison_62_3.Id1", MonsterId.Sec_Bubbe_Mage_Priest, min: 42, max: 55, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_3.Id2", MonsterId.GoblinWarrior_Blue, min: 46, max: 60, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_3.Id3", MonsterId.Sec_Bubbe_Mage_Priest, min: 48, max: 60, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_3.Id4", MonsterId.Rootcrystal_01, min: 19, max: 25, respawn: Seconds(25), tendency: TendencyType.Peaceful);
		AddSpawner("d_prison_62_3.Id5", MonsterId.Sec_Goblin_Archer_Blue, min: 77, max: 85, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_3.Id6", MonsterId.GoblinWarrior_Blue, min: 34, max: 42, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Sec_Banshee_Purple' GenType 6 Spawn Points
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(474, -15, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-64, 543, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-402, 540, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-355, 767, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(12, 840, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-193, 681, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-898, 795, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-1010, 1041, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-796, 1035, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-667, 862, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-475, 1067, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-661, 1229, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-898, 1384, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-770, 1599, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-626, 1576, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-446, 1575, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-395, 1372, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-155, 1480, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-170, 1711, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-313, 1650, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-309, 2351, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-168, 2485, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1, 2337, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-130, 2190, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-141, 2283, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-342, 187, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(155, 199, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-170, 140, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(116, -299, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-52, -264, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-8, -553, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(245, -451, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(160, -1036, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(43, -1507, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(127, -1249, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(334, -1510, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(139, -1686, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(288, -1916, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-9, -1918, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-256, -1673, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-391, -1328, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-159, -934, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-429, -1093, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(-85, -1313, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1812, -141, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1432, -84, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1582, -252, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1795, -234, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1547, -73, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1612, 10, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1802, 77, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1606, 281, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1629, 678, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1614, 560, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1600, 917, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1735, 1251, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1847, 1094, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1760, 1116, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1612, 1167, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1899, 925, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1877, 1288, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1542, -2005, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1694, -1950, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1789, -1894, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1563, -1474, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1473, -1190, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1441, -852, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1759, -1179, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1742, -1484, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(2003, -1483, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(2100, -1610, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(2264, -1575, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(2174, -966, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(2252, -818, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1650, -700, 35));
		AddSpawnPoint("d_prison_62_3.Id1", "d_prison_62_3", Rectangle(1634, -556, 35));

		// 'GoblinWarrior_Blue' GenType 7 Spawn Points
		AddSpawnPoint("d_prison_62_3.Id2", "d_prison_62_3", Rectangle(1706, -1001, 9999));

		// 'Sec_Bubbe_Mage_Priest' GenType 8 Spawn Points
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1626, -161, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(-280, -1252, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(-53, -1161, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(-284, -980, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(-21, -1728, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(95, -1829, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(274, -1751, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(214, -1122, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(166, -804, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(-117, -403, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(25, -411, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(251, -283, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(171, -581, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(-110, 254, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(146, 758, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(463, 811, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(333, 685, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(449, 1121, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(417, 997, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(337, 917, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1629, 1285, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1925, 1177, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1782, 957, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1627, 1057, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1941, 1020, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1706, 914, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1585, 777, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1614, 615, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1735, -96, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1700, -276, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1416, -210, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1487, 31, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1694, -495, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1664, -607, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1458, -1511, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1668, -877, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1899, -1133, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1920, -961, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(2113, -866, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(2310, -1082, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(2102, -1050, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(2190, -1488, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(2276, -1358, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(2083, -1378, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1876, -1419, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1735, -1779, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1585, -1883, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1668, -2063, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1856, -1791, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1477, -1308, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1792, -2000, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1570, -1789, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1672, -1343, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1470, -1005, 35));
		AddSpawnPoint("d_prison_62_3.Id3", "d_prison_62_3", Rectangle(1592, -1113, 35));

		// 'Rootcrystal_01' GenType 9 Spawn Points
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(1549, -85, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(1709, -549, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(1670, -1936, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(1514, -1462, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(2162, -1418, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(2087, -949, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(1847, 1212, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(1702, 1010, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(1133, 1105, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(1005, 820, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(812, 119, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(800, -204, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(430, 56, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(-23, 191, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(275, 689, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(-192, 702, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(-244, 1553, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(-195, 2245, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(-923, 908, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(-256, -1171, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(89, -378, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(187, -1705, 200));
		AddSpawnPoint("d_prison_62_3.Id4", "d_prison_62_3", Rectangle(208, -1155, 200));

		// 'Sec_Varv' GenType 27 Spawn Points
		AddSpawnPoint("d_prison_62_3.Id5", "d_prison_62_3", Rectangle(-276, -1106, 9999));

		// 'Sec_Varv' GenType 33 Spawn Points
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1709, -2038, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1848, -1984, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1839, -1780, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1567, -1773, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1544, -1904, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1570, -1995, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1722, -1802, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1752, -1936, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2256, -1374, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2264, -1474, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2091, -1580, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2040, -1528, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2070, -1315, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2181, -1508, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2251, -1585, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2252, -1022, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2080, -953, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2146, -852, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2233, -856, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2246, -940, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(2124, -1065, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1760, -925, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1602, -1070, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1648, -1353, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1652, -1540, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1853, -1496, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1884, -1243, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1836, -1141, 30));
		AddSpawnPoint("d_prison_62_3.Id6", "d_prison_62_3", Rectangle(1707, -758, 30));
	}
}
