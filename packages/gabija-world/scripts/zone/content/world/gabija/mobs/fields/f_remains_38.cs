//--- Melia Script -----------------------------------------------------------
// Goddess' Ancient Garden Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_remains_38'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains38MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_remains_38.Id1", MonsterId.InfroBurk, min: 9, max: 12);
		AddSpawner("f_remains_38.Id2", MonsterId.Lizardman, min: 8, max: 10);
		AddSpawner("f_remains_38.Id3", MonsterId.Long_Arm, min: 8, max: 10);
		AddSpawner("f_remains_38.Id4", MonsterId.Lizardman, min: 8, max: 10);
		AddSpawner("f_remains_38.Id5", MonsterId.Lizardman, min: 8, max: 10);
		AddSpawner("f_remains_38.Id6", MonsterId.InfroBurk, min: 8, max: 10);
		AddSpawner("f_remains_38.Id7", MonsterId.Rootcrystal_01, min: 14, max: 18, respawn: Minutes(1));
		AddSpawner("f_remains_38.Id8", MonsterId.Stub_Tree_Mage, min: 8, max: 10);
		AddSpawner("f_remains_38.Id9", MonsterId.Long_Arm, min: 23, max: 30);

		// Monster Spawn Points -----------------------------

		// 'InfroBurk' GenType 3 Spawn Points
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-2034, 708, 20));
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-1846, 503, 20));
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-1875, 720, 20));
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-1688, 599, 20));
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-1684, 501, 20));
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-1862, 623, 20));
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-2000, 856, 20));
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-2011, 570, 20));
		AddSpawnPoint("f_remains_38.Id1", "f_remains_38", Rectangle(-1743, 706, 20));

		// 'Lizardman' GenType 5 Spawn Points
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-716, 571, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-351, 340, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-958, -534, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-820, -409, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-918, 377, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-1028, -117, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-1069, -327, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-1007, 282, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-622, 753, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-419, 187, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-876, 62, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-1183, -34, 30));
		AddSpawnPoint("f_remains_38.Id2", "f_remains_38", Rectangle(-607, 284, 30));

		// 'Long_Arm' GenType 71 Spawn Points
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(126, -1149, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(-143, -961, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(563, -726, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(407, -1061, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(-185, -1114, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(399, -835, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(233, -810, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(262, -1323, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(-924, -1183, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(-1210, -1129, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(-1192, -971, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(-1544, -889, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(-1737, -782, 20));
		AddSpawnPoint("f_remains_38.Id3", "f_remains_38", Rectangle(-714, -1003, 20));

		// 'Lizardman' GenType 72 Spawn Points
		AddSpawnPoint("f_remains_38.Id4", "f_remains_38", Rectangle(-955, -54, 9999));

		// 'Lizardman' GenType 76 Spawn Points
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1406, 765, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1348, 375, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1557, 1006, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1172, 957, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1255, 645, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1579, 411, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1465, 529, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1433, 939, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1077, 790, 25));
		AddSpawnPoint("f_remains_38.Id5", "f_remains_38", Rectangle(1205, 521, 25));

		// 'InfroBurk' GenType 77 Spawn Points
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1084, -841, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1069, -1042, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1186, -848, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1327, -742, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1473, -762, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1558, -603, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1084, -1165, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1403, -634, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1496, -877, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1441, -489, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1293, -1011, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(954, -922, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1197, -1043, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1286, -1143, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1454, -1039, 25));
		AddSpawnPoint("f_remains_38.Id6", "f_remains_38", Rectangle(1402, -889, 25));

		// 'Rootcrystal_01' GenType 84 Spawn Points
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-1298, -1125, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-1454, -1766, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-1812, -1565, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-779, -1108, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-883, -605, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-1020, -247, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-987, 74, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-905, 370, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-1849, 641, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-1276, 565, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-655, 651, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-324, 1060, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(138, 1147, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(214, 556, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(120, 248, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-270, 356, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(-15, -957, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(219, -1143, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(376, -1172, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(687, -997, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(1347, -1039, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(1384, -591, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(1074, -1056, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(1555, -228, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(1477, 505, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(1178, 848, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(1539, 892, 100));
		AddSpawnPoint("f_remains_38.Id7", "f_remains_38", Rectangle(1420, 1384, 100));

		// 'Stub_Tree_Mage' GenType 85 Spawn Points
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(-117, -1006, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(170, -1240, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(214, -1110, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(-1107, -252, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(-955, 162, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(-135, 317, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(-761, 421, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(-834, -359, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(1428, 502, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(1193, 765, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(1559, 928, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(1405, 721, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(1135, -1109, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(1401, -839, 40));
		AddSpawnPoint("f_remains_38.Id8", "f_remains_38", Rectangle(1132, -858, 40));

		// 'Long_Arm' GenType 89 Spawn Points
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-69, -1042, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(12, -1134, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(112, -1226, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(194, -1282, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(300, -1306, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(321, -1206, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(213, -1101, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(135, -981, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(69, -921, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(115, -1093, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-26, -953, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1575, -1687, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1613, -1795, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1620, -1898, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1507, -2029, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1431, -2101, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1309, -2087, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1394, -1988, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1470, -1902, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1461, -1764, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1368, -1846, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1291, -1936, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1163, -1886, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1252, -1803, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(-1352, -1717, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1473, 349, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1373, 416, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1284, 570, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1222, 703, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1187, 834, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1259, 982, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1390, 785, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1427, 635, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1486, 511, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1593, 387, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1570, 616, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1508, 754, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1456, 923, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1572, 927, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1613, 1070, 20));
		AddSpawnPoint("f_remains_38.Id9", "f_remains_38", Rectangle(1244, 477, 20));
	}
}
