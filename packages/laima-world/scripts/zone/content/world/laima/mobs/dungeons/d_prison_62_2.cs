//--- Melia Script -----------------------------------------------------------
// Ashaq Underground Prison 2F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_prison_62_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison622MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_prison_62_2.Id1", MonsterId.Goblin_Spear_Blue, min: 45, max: 60, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_2.Id2", MonsterId.Sec_Goblin_Archer_Blue, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_2.Id3", MonsterId.Sec_Escape_Wendigo, min: 69, max: 75, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_2.Id4", MonsterId.Rootcrystal_01, min: 19, max: 25, respawn: Seconds(25), tendency: TendencyType.Peaceful);
		AddSpawner("d_prison_62_2.Id5", MonsterId.Goblin_Spear_Blue, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_2.Id6", MonsterId.Sec_Escape_Wendigo, min: 70, max: 90, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_2.Id7", MonsterId.Sec_Goblin_Archer_Blue, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_prison_62_2.Id8", MonsterId.Sec_Escape_Wendigo, min: 103, max: 120, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Goblin_Spear_Blue' GenType 8 Spawn Points
		AddSpawnPoint("d_prison_62_2.Id1", "d_prison_62_2", Rectangle(-191, 771, 9999));

		// 'Sec_Goblin_Archer_Blue' GenType 9 Spawn Points
		AddSpawnPoint("d_prison_62_2.Id2", "d_prison_62_2", Rectangle(-70, 1089, 9999));

		// 'Sec_Escape_Wendigo' GenType 10 Spawn Points
		AddSpawnPoint("d_prison_62_2.Id3", "d_prison_62_2", Rectangle(-163, 914, 9999));

		// 'Rootcrystal_01' GenType 11 Spawn Points
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-600, 44, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-231, -1895, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-129, -1641, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-183, -1081, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-59, -874, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-153, -603, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-1102, -176, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-1176, 127, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-1685, -1127, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-1655, -871, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-1622, 711, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-1602, 886, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-1617, 1577, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-436, 1613, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(-1016, 1709, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(276, 1487, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(804, 1042, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(799, 801, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(1356, 751, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(1355, 976, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(813, -41, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(1700, 26, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(1566, -160, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(798, -810, 200));
		AddSpawnPoint("d_prison_62_2.Id4", "d_prison_62_2", Rectangle(810, -1184, 200));

		// 'Goblin_Spear_Blue' GenType 26 Spawn Points
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-30, -1744, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-295, -1944, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-372, -1731, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-993, 20, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-34, -1870, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-139, -1800, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-228, -1826, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1738, -916, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1730, -1081, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1733, -1010, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1730, -950, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1631, -888, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-971, -112, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1006, -221, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1259, 91, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1198, -62, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1552, -1100, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1653, -913, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1791, -799, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1593, -798, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1020, -68, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(744, 800, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1636, -1015, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1313, -719, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1296, -556, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1281, -341, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1321, -203, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1111, -206, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1260, 254, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1288, -10, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1699, 610, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1684, 878, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1553, 963, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1622, 727, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1781, 1004, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1617, 1514, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1513, 1553, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1627, 1763, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1528, 1686, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-600, 1570, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-732, 1594, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-324, 1605, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-433, 1526, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-387, 1625, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(234, 1686, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(380, 1420, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(185, 1470, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(256, 1583, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(-1082, 82, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(421, 877, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(693, 1054, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(756, 952, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(761, 713, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(854, 672, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1483, 754, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1282, 841, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1265, 1056, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1393, 1100, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1280, 763, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1375, 657, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(996, -107, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(809, -130, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(734, 53, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(934, 56, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(844, -24, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1836, 100, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1685, -39, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1733, 99, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1654, 69, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(1476, -12, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(759, -811, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(756, -708, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(669, -1100, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(918, -1205, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(888, -732, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(658, -989, 30));
		AddSpawnPoint("d_prison_62_2.Id5", "d_prison_62_2", Rectangle(966, -931, 30));

		// 'Sec_Escape_Wendigo' GenType 27 Spawn Points
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1792, -203, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1485, -241, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1552, 112, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1737, -131, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1539, -152, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(864, -1119, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(771, -1068, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(682, -870, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(899, -1028, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(874, -129, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(651, -139, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(682, -18, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-36, -933, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(800, 268, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(823, 530, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(609, 893, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(500, 872, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(828, 1032, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(771, 709, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1451, 663, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1281, 700, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1308, 1119, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1509, 925, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1363, 1275, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(1380, 1342, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(781, 1335, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(425, 1311, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(143, 1250, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(326, 1279, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(322, 1747, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-40, -1013, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-227, 868, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-180, 1169, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-48, 923, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-109, -900, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-496, 1582, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-644, 1733, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-486, 1687, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-276, 1626, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-1615, 1613, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-1742, 1750, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-1816, 1849, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-1817, 1504, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-1458, 950, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-102, -1042, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-1814, 907, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-1573, 849, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-1552, 630, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-190, -1071, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-137, -975, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-239, -999, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-114, -905, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-193, -858, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-205, -928, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-165, -1025, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-139, -1536, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-343, -1465, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-277, -1624, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-407, -1934, 30));
		AddSpawnPoint("d_prison_62_2.Id6", "d_prison_62_2", Rectangle(-365, -1825, 30));

		// 'Sec_Goblin_Archer_Blue' GenType 28 Spawn Points
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-224, -1719, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-156, -1938, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-45, -1576, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-107, -1668, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1684, -862, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1637, -934, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1649, -1129, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1558, -819, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1259, 124, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1115, -181, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-147, 885, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1061, 103, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1319, -652, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1242, -593, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1278, -125, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1021, 50, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1093, -66, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1102, 72, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1827, 644, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1705, 805, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1660, 989, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1274, -7, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1729, 1580, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1789, 1670, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-1543, 1822, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-815, 1656, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-691, 1699, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-349, 1736, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-161, 1059, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-139, 690, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(-11, 677, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(313, 1494, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(278, 1370, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(95, 1685, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(376, 1577, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(690, 832, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(833, 822, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(736, 1134, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(836, 1112, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1378, 925, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1434, 857, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1261, 951, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1437, 993, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1403, 779, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1858, -126, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1680, -213, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1578, -53, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(1776, 20, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(820, -207, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(979, -22, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(843, 120, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(795, -970, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(737, -1189, 30));
		AddSpawnPoint("d_prison_62_2.Id7", "d_prison_62_2", Rectangle(864, -912, 30));

		// 'Sec_Bat' GenType 29 Spawn Points
		AddSpawnPoint("d_prison_62_2.Id8", "d_prison_62_2", Rectangle(-1025, -48, 9999));
	}
}
