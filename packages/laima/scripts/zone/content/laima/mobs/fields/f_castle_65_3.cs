//--- Melia Script -----------------------------------------------------------
// Delmore Outskirts Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_castle_65_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle653MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_castle_65_3.Id1", MonsterId.Rootcrystal_03, min: 23, max: 30, respawn: Seconds(10));
		AddSpawner("f_castle_65_3.Id2", MonsterId.PagNurse, min: 30, max: 40);
		AddSpawner("f_castle_65_3.Id3", MonsterId.PagEmitter, min: 30, max: 40);
		AddSpawner("f_castle_65_3.Id4", MonsterId.PagDoper, min: 30, max: 40);
		AddSpawner("f_castle_65_3.Id5", MonsterId.Sec_Zibu_Maize, min: 30, max: 40);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 4 Spawn Points
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1807, -1234, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1736, -969, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1546, -507, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1567, -91, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1887, 355, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1648, 483, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1889, 610, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1914, 973, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1699, 775, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1745, 1000, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1352, 713, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1172, 1119, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-1035, 1298, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-776, 1365, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-497, 1410, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-547, 1047, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-797, 1030, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-774, 812, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-654, 385, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-652, -454, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-186, -328, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-116, -67, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(105, -227, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(278, -44, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(379, -413, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-649, -791, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-318, -963, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-148, -1232, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(115, -1065, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(422, -1134, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(284, -1587, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(427, -1716, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(550, -1572, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(782, -748, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(715, -960, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(970, -1152, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(852, -1360, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(1018, -1631, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(1358, -1419, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(1489, -1002, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(1391, -838, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(1634, -771, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(79, 570, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(184, 408, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(324, 655, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(143, 873, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(481, 317, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(768, 126, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-142, 955, 50));
		AddSpawnPoint("f_castle_65_3.Id1", "f_castle_65_3", Rectangle(-113, 1287, 50));

		// 'PagNurse' GenType 300 Spawn Points
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1963, 403, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1834, 328, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1881, 646, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1811, 476, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1876, 865, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1788, 1016, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1668, 1068, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1636, 698, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1610, 547, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1764, 595, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-2042, 746, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1971, 979, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1755, 766, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1631, 853, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-2011, 562, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1933, 278, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1681, 388, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1413, 749, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1274, 835, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-1177, 1132, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-936, 1050, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-908, 1271, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-650, 1064, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-788, 1454, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-584, 1314, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(-764, 821, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(654, -1036, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(864, -1268, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(917, -1491, 30));
		AddSpawnPoint("f_castle_65_3.Id2", "f_castle_65_3", Rectangle(1088, -1740, 30));

		// 'PagEmitter' GenType 301 Spawn Points
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-919, 1156, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-1009, 943, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-1092, 1304, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-972, 1466, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-769, 1316, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-551, 1418, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-454, 1425, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-633, 1200, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-513, 901, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-652, 866, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-752, 989, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-591, 647, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-579, 771, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-823, 753, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-669, 577, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-850, 369, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-695, 274, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-618, 411, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-749, 449, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-770, 581, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-1874, -1156, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-1730, -1220, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-1775, -1061, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-1868, -909, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-1684, -948, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-1646, -1123, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-401, -1046, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-290, -937, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-166, -1079, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(51, -1066, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(209, -1126, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(325, -1039, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(30, -1351, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(224, -1487, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(348, -1518, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(314, -1677, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(480, -1762, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(568, -1645, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(438, -1588, 30));
		AddSpawnPoint("f_castle_65_3.Id3", "f_castle_65_3", Rectangle(-120, -1297, 30));

		// 'PagDoper' GenType 302 Spawn Points
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(625, -820, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(812, -735, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(880, -968, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(759, -879, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(842, -1078, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(746, -1343, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(882, -1634, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(1010, -1215, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(964, -1741, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(1098, -1600, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(979, -1338, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(701, -1177, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(975, -1081, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(-32, -321, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(26, -22, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(216, -20, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(-101, -53, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(231, -133, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(192, -302, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(-12, -195, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(57, 570, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(253, 845, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(209, 517, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(57, 786, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(181, 649, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(202, 407, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(386, 771, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(169, 926, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(-210, 722, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(26, 436, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(86, 98, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(439, 406, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(596, 221, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(340, 388, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(770, 34, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(708, 189, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(644, 74, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(805, 133, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(313, 681, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(341, 568, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(408, 893, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(-179, 1244, 30));
		AddSpawnPoint("f_castle_65_3.Id4", "f_castle_65_3", Rectangle(-172, 1371, 30));

		// 'Sec_Zibu_Maize' GenType 303 Spawn Points
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1296, -955, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1590, -671, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1470, -874, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1346, -775, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1465, -1015, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1513, -1107, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1670, -829, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1658, -716, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1745, -872, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1389, -914, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1316, -844, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1555, -762, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1567, -870, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1740, -720, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1346, -1480, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1477, -1216, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1377, -1353, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(1469, -1290, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(645, 143, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(693, 14, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(734, 119, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(64, 902, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(-8, 701, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(109, 477, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(391, 686, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(305, 924, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(317, 481, 30));
		AddSpawnPoint("f_castle_65_3.Id5", "f_castle_65_3", Rectangle(176, 755, 30));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Rambandgad_Red, "f_castle_65_3", 1, Hours(2), Hours(4));
	}
}
