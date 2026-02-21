//--- Melia Script -----------------------------------------------------------
// Koru Jungle Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_bracken_63_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken631MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_bracken_63_1.Id1", MonsterId.Ferrot, min: 30, max: 40, respawn: Seconds(20));
		AddSpawner("f_bracken_63_1.Id2", MonsterId.Folibu, min: 30, max: 40, respawn: Seconds(20));
		AddSpawner("f_bracken_63_1.Id3", MonsterId.Leafnut, min: 30, max: 40, respawn: Seconds(20));
		AddSpawner("f_bracken_63_1.Id4", MonsterId.Rootcrystal_01, min: 22, max: 29, respawn: Seconds(20));
		AddSpawner("f_bracken_63_1.Id5", MonsterId.Folibu, min: 30, max: 40, respawn: Seconds(20));
		AddSpawner("f_bracken_63_1.Id6", MonsterId.Ferrot, min: 19, max: 25, respawn: Seconds(20));
		AddSpawner("f_bracken_63_1.Id7", MonsterId.Leafnut, min: 19, max: 25, respawn: Seconds(20));
		AddSpawner("f_bracken_63_1.Id8", MonsterId.Folibu, min: 30, max: 40, respawn: Seconds(20));

		// Monster Spawn Points -----------------------------

		// 'Ferrot' GenType 12 Spawn Points
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-331, 190, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-811, -723, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-621, -599, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-557, -506, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-794, -386, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-954, -116, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-978, 65, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-791, 233, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-768, -78, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-358, -1004, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-181, -1384, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-253, -1614, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-309, -1681, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-82, -1890, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(92, -1863, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(219, -1782, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-47, -1691, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(133, -1389, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(206, -1185, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-103, -925, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-223, -580, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(31, -773, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(324, -821, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(514, -669, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(549, -897, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(439, -945, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(349, -677, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(323, -418, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(380, -217, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(331, -373, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(868, -884, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1025, -708, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1369, -782, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1344, -583, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1339, -405, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1596, -335, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1589, -568, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1649, -753, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1231, -832, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(1190, -520, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(127, 743, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(170, 623, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-96, 388, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-304, 286, 30));
		AddSpawnPoint("f_bracken_63_1.Id1", "f_bracken_63_1", Rectangle(-454, -42, 30));

		// 'Folibu' GenType 13 Spawn Points
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-856, 723, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-530, 1609, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(674, 1403, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(1142, 873, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(877, 531, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(436, -820, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(1425, -599, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-268, -1693, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1718, -1440, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-442, -936, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(811, 631, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(793, 781, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(910, 856, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(972, 665, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(1324, 859, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(1191, 1076, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(993, 1082, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(859, 999, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(766, 1187, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(613, 1217, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(413, 1447, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(591, 1552, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(507, 1685, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(404, 1582, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(521, 1792, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-700, 1423, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-763, 1657, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-625, 1497, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-639, 1652, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-651, 1870, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-475, 1739, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-419, 1517, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-442, 1385, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-568, 1369, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-386, 1653, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-294, 1818, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-558, 2016, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-711, 1972, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-929, 1416, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-876, 1548, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-712, 1090, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-613, 1212, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-703, 1135, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-594, 1089, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-922, 595, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-680, 600, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-675, 790, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-852, 644, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-932, 810, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-838, 963, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1063, 868, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1094, 1111, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1196, 1165, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1115, 1290, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-886, 982, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-881, 507, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-235, 668, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-210, 882, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-16, 867, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-4, 707, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-144, 754, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-380, 554, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-622, 334, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-139, -1810, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-11, -1715, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-203, -1532, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-85, -1351, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(138, -1721, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1663, -1119, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1454, -1234, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1517, -1448, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1548, -1564, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1261, -1548, 30));
		AddSpawnPoint("f_bracken_63_1.Id2", "f_bracken_63_1", Rectangle(-1276, -1359, 30));

		// 'Leafnut' GenType 14 Spawn Points
		AddSpawnPoint("f_bracken_63_1.Id3", "f_bracken_63_1", Rectangle(-1388, -1154, 9999));

		// 'Rootcrystal_01' GenType 306 Spawn Points
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(458, -790, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(177, -92, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(432, 477, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(897, 585, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(1234, 920, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(815, 1120, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(533, 1692, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(1156, -702, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(1767, -749, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(1515, -396, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-159, -754, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-94, -1314, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(153, -1511, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-153, -1894, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-530, -840, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-742, -209, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-629, 245, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-230, 680, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-402, 58, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-829, 503, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-1138, 1050, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-782, 803, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-622, 1128, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-882, 1672, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-516, 1461, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-483, 1888, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-1215, -1554, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-1554, -1607, 10));
		AddSpawnPoint("f_bracken_63_1.Id4", "f_bracken_63_1", Rectangle(-1637, -1131, 10));

		// 'Sec_Bubbe_Chaser' GenType 308 Spawn Points
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-544, -826, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-1502, -1474, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-770, -301, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-100, 642, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-831, 693, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-1137, 1113, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-606, 1143, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-537, 1696, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(133, -1464, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(1703, -701, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(953, 574, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(1105, 997, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(527, 1524, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-609, 14, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(1303, 854, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-175, 887, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-194, -1742, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-740, -503, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-958, 102, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-1638, -1528, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(64, 814, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-530, 413, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-927, 619, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-675, 1743, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(733, 1233, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(930, 843, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(1439, -827, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(1525, -337, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(896, 1035, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(715, 514, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-200, -1435, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(1241, -531, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(1511, -536, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-1348, -1264, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-1301, -1616, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(143, 555, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-199, 360, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-374, 165, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-42, -1548, 30));
		AddSpawnPoint("f_bracken_63_1.Id5", "f_bracken_63_1", Rectangle(-834, -721, 30));

		// 'Ferrot' GenType 310 Spawn Points
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1274, -782, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1217, -598, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1343, -528, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1477, -357, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1656, -509, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1575, -582, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1771, -701, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1629, -875, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1579, -771, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1474, -929, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1268, -897, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1409, -769, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1194, -430, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1015, -719, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1490, -195, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1781, -591, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1731, -695, 30));
		AddSpawnPoint("f_bracken_63_1.Id6", "f_bracken_63_1", Rectangle(1377, -703, 30));

		// 'Leafnut' GenType 311 Spawn Points
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-839, -182, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-659, -167, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-827, 24, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-644, 44, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-831, 207, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-717, 357, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-687, 232, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-537, 513, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-371, 533, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-589, 394, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-928, -4, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-712, -459, 25));
		AddSpawnPoint("f_bracken_63_1.Id7", "f_bracken_63_1", Rectangle(-583, -355, 25));

		// 'Sec_Bubbe_Chaser' GenType 312 Spawn Points
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(359, -922, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(320, -735, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(318, -643, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(523, -753, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(460, -888, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(571, -946, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(652, -779, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-64, -1730, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-215, -1880, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-4, -1912, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(149, -1757, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(19, -1733, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-12, -1622, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-225, -1592, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-310, -1602, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-287, -1439, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-84, -1352, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-46, -1426, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-849, -99, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-658, 81, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-978, 51, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-832, 221, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-825, 90, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-665, 246, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-702, 344, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-568, 451, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-989, 134, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-997, -17, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-682, 20, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-818, 329, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-930, 529, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-824, 575, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-692, 589, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-685, 687, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-855, 869, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-1041, 776, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-1116, 940, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-884, 999, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-968, 942, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-1007, 1090, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-1202, 1258, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-721, 1491, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-695, 1714, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-905, 1599, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-904, 1424, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-624, 1479, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-424, 1634, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-624, 1561, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-765, 1690, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-473, 1785, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-137, 819, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-52, 875, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-81, 695, 25));
		AddSpawnPoint("f_bracken_63_1.Id8", "f_bracken_63_1", Rectangle(-218, 599, 25));
	}
}
