//--- Melia Script -----------------------------------------------------------
// Baubas Cave Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_limestonecave_71_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DLimestonecave711MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_limestonecave_71_1.Id1", MonsterId.ERD_Stonacorn_Yellow, min: 8, max: 10);
		AddSpawner("d_limestonecave_71_1.Id2", MonsterId.ERD_Poncer, min: 57, max: 75);
		AddSpawner("d_limestonecave_71_1.Id3", MonsterId.ERD_Yishoneer, min: 49, max: 65);
		AddSpawner("d_limestonecave_71_1.Id4", MonsterId.ERD_Yishontorcher, min: 6, max: 8);
		AddSpawner("d_limestonecave_71_1.Id5", MonsterId.ERD_Tala_Sorcerer, min: 23, max: 30);
		AddSpawner("d_limestonecave_71_1.Id6", MonsterId.ERD_Tala_Combat, min: 23, max: 30);
		AddSpawner("d_limestonecave_71_1.Id7", MonsterId.ERD_Warleader_Tala, min: 4, max: 5);
		AddSpawner("d_limestonecave_71_1.Id8", MonsterId.ERD_Rocktanon, min: 23, max: 30);
		AddSpawner("d_limestonecave_71_1.Id9", MonsterId.ERD_Rockon, min: 12, max: 15);
		AddSpawner("d_limestonecave_71_1.Id10", MonsterId.ERD_Rockoff, min: 12, max: 15);
		AddSpawner("d_limestonecave_71_1.Id11", MonsterId.ERD_Zolem_Green, min: 12, max: 15);
		AddSpawner("d_limestonecave_71_1.Id12", MonsterId.ERD_Rubblem_Green, min: 8, max: 10);
		AddSpawner("d_limestonecave_71_1.Id13", MonsterId.ERD_Bavon_Green, min: 8, max: 10);

		// Monster Spawn Points -----------------------------

		// 'ERD_Stonacorn_Yellow' GenType 22 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(1755, -850, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(1671, -1110, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(386, -1666, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(564, -1610, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(-103, -1419, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(-242, -1511, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(-1439, -1285, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(-1430, -1392, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(-475, -791, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id1", "d_limestonecave_71_1", Rectangle(-335, -857, 20));

		// 'ERD_Poncer' GenType 23 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1311, -639, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1345, -710, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1447, -885, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1287, -944, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1307, -1077, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1503, -1106, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1683, -1032, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1634, -750, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1561, -683, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1262, -863, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1358, -1009, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1429, -1070, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1176, -870, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1539, -936, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1162, -1007, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(812, -934, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(663, -999, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(562, -957, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(469, -901, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(454, -780, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(517, -680, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(562, -548, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(687, -595, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(803, -596, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(868, -844, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(721, -915, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(557, -856, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(619, -707, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(838, -762, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(760, -763, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1224, -676, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1190, -792, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1559, -769, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1356, -567, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(1620, -1100, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(393, -1221, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(185, -1490, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(430, -1288, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(547, -1578, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(380, -1640, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(553, -1320, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(369, -1336, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(218, -1379, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(276, -1434, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(332, -1498, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-356, -1458, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-345, -1391, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-300, -1339, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-220, -1317, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-277, -1427, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-239, -1484, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-138, -1465, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-97, -1405, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-113, -1324, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-156, -1356, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1150, -1188, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1242, -1225, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1385, -1308, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1318, -1404, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1201, -1416, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-997, -1276, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1015, -1429, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-901, -1310, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-909, -1420, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-771, -1321, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-644, -1470, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-761, -1411, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-735, -1559, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-909, -1604, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-996, -1501, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1133, -495, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-950, -399, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-867, -629, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-888, -707, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-981, -793, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-913, -451, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1060, -497, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-1159, -728, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-878, -540, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id2", "d_limestonecave_71_1", Rectangle(-974, -716, 20));

		// 'ERD_Yishoneer' GenType 24 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1207, -947, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1378, -934, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1483, -976, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1569, -1025, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1533, -849, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1638, -665, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1341, -793, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1472, -669, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1624, -904, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1728, -920, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1347, -864, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1261, -782, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1456, -592, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1393, -651, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(1615, -829, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(786, -847, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(640, -929, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(553, -754, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(692, -844, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(718, -667, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(799, -685, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(617, -803, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(494, -837, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(596, -596, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(657, -522, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(401, -1557, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(213, -1623, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(358, -1416, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(457, -1365, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(585, -1409, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(483, -1477, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(522, -1395, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(485, -1629, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(300, -1652, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(427, -1436, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(254, -1541, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(293, -1353, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(565, -1497, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(500, -1252, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(319, -1280, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-193, -1418, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-161, -1288, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-211, -1219, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-275, -1270, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-308, -1521, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-905, -1510, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-838, -1452, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-719, -1493, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1078, -1270, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1159, -1333, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1167, -1252, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1309, -1326, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1032, -1179, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1395, -1381, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1098, -1409, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-947, -598, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-955, -513, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1121, -408, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1117, -600, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1066, -701, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1174, -646, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1110, -745, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1057, -779, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1043, -557, 20));
		AddSpawnPoint("d_limestonecave_71_1.Id3", "d_limestonecave_71_1", Rectangle(-1031, -424, 20));

		// 'ERD_Yishontorcher' GenType 25 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id4", "d_limestonecave_71_1", Rectangle(682, -761, 120));
		AddSpawnPoint("d_limestonecave_71_1.Id4", "d_limestonecave_71_1", Rectangle(-1017, -637, 120));
		AddSpawnPoint("d_limestonecave_71_1.Id4", "d_limestonecave_71_1", Rectangle(-1307, -1281, 120));
		AddSpawnPoint("d_limestonecave_71_1.Id4", "d_limestonecave_71_1", Rectangle(398, -1472, 120));
		AddSpawnPoint("d_limestonecave_71_1.Id4", "d_limestonecave_71_1", Rectangle(-243, -1365, 120));
		AddSpawnPoint("d_limestonecave_71_1.Id4", "d_limestonecave_71_1", Rectangle(-776, -1457, 120));
		AddSpawnPoint("d_limestonecave_71_1.Id4", "d_limestonecave_71_1", Rectangle(-141, -544, 120));
		AddSpawnPoint("d_limestonecave_71_1.Id4", "d_limestonecave_71_1", Rectangle(1450, -790, 120));

		// 'ERD_Tala_Sorcerer' GenType 28 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-1128, 271, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-1368, -107, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-1302, 249, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-831, 126, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-1508, 232, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-1019, 1211, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-775, 1438, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-606, 1222, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-567, 1114, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-969, 1042, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(100, 1605, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(143, 1517, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(79, 1690, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-159, 1740, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(130, 1754, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(735, 852, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(294, 505, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(566, 528, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(376, 474, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(716, 891, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-316, 481, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-302, 395, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-350, 366, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-665, 705, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-778, 549, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-598, 749, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-358, 749, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-477, 260, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-609, 336, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id5", "d_limestonecave_71_1", Rectangle(-757, 458, 25));

		// 'ERD_Tala_Combat' GenType 29 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-1000, 292, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-865, 16, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-1407, 261, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-1133, -92, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-1531, 42, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-817, 972, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-897, 1001, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-1014, 1126, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-953, 1365, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-858, 1445, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-251, 1648, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-54, 1759, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(27, 1767, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(78, 1827, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(70, 1444, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(159, 1670, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-219, 1552, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-27, 1547, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-21, 1473, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-169, 1462, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(178, 536, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(556, 1082, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(687, 969, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(472, 493, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(635, 1032, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-264, 571, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-425, 721, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-732, 626, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-551, 280, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id6", "d_limestonecave_71_1", Rectangle(-454, 353, 25));

		// 'ERD_Warleader_Tala' GenType 30 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id7", "d_limestonecave_71_1", Rectangle(-845, 1231, 150));
		AddSpawnPoint("d_limestonecave_71_1.Id7", "d_limestonecave_71_1", Rectangle(-978, 100, 150));
		AddSpawnPoint("d_limestonecave_71_1.Id7", "d_limestonecave_71_1", Rectangle(-1434, 93, 150));
		AddSpawnPoint("d_limestonecave_71_1.Id7", "d_limestonecave_71_1", Rectangle(-77, 1598, 150));
		AddSpawnPoint("d_limestonecave_71_1.Id7", "d_limestonecave_71_1", Rectangle(427, 789, 150));

		// 'ERD_Rocktanon' GenType 31 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1105, 162, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1117, 3, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1029, 16, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1175, 77, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1011, 181, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1252, 163, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1407, -22, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1341, 104, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1306, 15, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(-1495, -59, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(653, 684, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(582, 723, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(536, 615, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(501, 747, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(638, 794, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(548, 933, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(474, 1004, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(467, 868, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(383, 1050, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(473, 1092, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(281, 673, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(268, 591, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(177, 721, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(343, 756, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(364, 679, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(255, 798, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(150, 837, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(304, 991, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(232, 1066, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id8", "d_limestonecave_71_1", Rectangle(148, 959, 25));

		// 'ERD_Rockon' GenType 32 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-680, 482, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-672, 565, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-450, 456, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-386, 643, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-531, 681, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-932, 1262, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-854, 1337, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-762, 1256, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-700, 1182, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-920, 1160, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-868, 1097, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-662, 1112, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-816, 1157, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-742, 1033, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id9", "d_limestonecave_71_1", Rectangle(-749, 1119, 25));

		// 'ERD_Rockoff' GenType 33 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-580, 610, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-534, 409, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-607, 454, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-469, 590, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-358, 554, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-94, 1484, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(60, 1535, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-137, 1557, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-258, 1453, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-340, 1636, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-305, 1544, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-92, 1685, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-28, 1675, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(15, 1622, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id10", "d_limestonecave_71_1", Rectangle(-171, 1630, 25));

		// 'ERD_Zolem_Green' GenType 34 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-166, -644, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-350, -669, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-271, -666, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-417, -615, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-281, -500, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-211, -377, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-161, -436, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-293, -420, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-260, -298, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-105, -366, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-85, -464, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-3, -447, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(-77, -577, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(86, -472, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id11", "d_limestonecave_71_1", Rectangle(29, -590, 25));

		// 'ERD_Rubblem_Green' GenType 35 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(23, -682, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(-161, -781, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(-282, -790, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(-371, -750, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(-450, -781, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(-391, -535, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(-376, -452, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(-224, -560, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(-28, -327, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id12", "d_limestonecave_71_1", Rectangle(62, -357, 25));

		// 'ERD_Bavon_Green' GenType 36 Spawn Points
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-50, -778, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-66, -672, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-164, -304, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-85, -267, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(7, -245, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-334, -845, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-438, -692, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-310, -587, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-225, -728, 25));
		AddSpawnPoint("d_limestonecave_71_1.Id13", "d_limestonecave_71_1", Rectangle(-363, -366, 25));
	}
}
