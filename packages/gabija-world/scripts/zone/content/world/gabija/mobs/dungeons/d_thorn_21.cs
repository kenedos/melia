//--- Melia Script -----------------------------------------------------------
// Kvailas Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_thorn_21'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn21MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_thorn_21.Id1", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(30));
		AddSpawner("d_thorn_21.Id2", MonsterId.Matsum, min: 15, max: 20);
		AddSpawner("d_thorn_21.Id3", MonsterId.Matsum, min: 15, max: 20);
		AddSpawner("d_thorn_21.Id4", MonsterId.Chafperor, min: 19, max: 25);
		AddSpawner("d_thorn_21.Id5", MonsterId.Chafperor, min: 15, max: 20);
		AddSpawner("d_thorn_21.Id6", MonsterId.Ammon, min: 12, max: 15);
		AddSpawner("d_thorn_21.Id7", MonsterId.Matsum, min: 3, max: 4);
		AddSpawner("d_thorn_21.Id8", MonsterId.Infroholder_Mage, min: 6, max: 8);
		AddSpawner("d_thorn_21.Id9", MonsterId.Infroholder_Mage, min: 6, max: 7);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 20 Spawn Points
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(72, -39, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(921, 118, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1247, 45, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(769, 81, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(878, 599, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(895, 923, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1409, 1102, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1476, 1295, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1746, 1241, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1668, 1037, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2281, 1436, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2040, 1364, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2360, 1313, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2931, 1213, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(5141, -203, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(3295, 1118, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(3120, 1211, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(823, -562, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(888, -1175, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1338, -1039, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(920, -208, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1120, -1272, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(993, -1468, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1595, -1067, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1827, -1008, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2422, -1160, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2725, -1130, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2608, -1376, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1807, 27, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1934, -36, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(1934, -203, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2599, 422, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(3309, -276, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(3467, -479, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(3726, -439, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(4407, -189, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(4650, -124, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(4796, -225, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(5540, -228, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(5879, -236, 200));
		AddSpawnPoint("d_thorn_21.Id1", "d_thorn_21", Rectangle(2827, 337, 200));

		// 'Matsum' GenType 36 Spawn Points
		AddSpawnPoint("d_thorn_21.Id2", "d_thorn_21", Rectangle(1757, 27, 9999));

		// 'Matsum' GenType 107 Spawn Points
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1380, 1073, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1428, 1293, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1648, 1044, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1664, 1320, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(913, 857, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(965, 1043, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(846, 944, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1551, 1083, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1550, 1228, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1748, 1194, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1902, 1404, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1968, 1305, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(1346, 1184, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(2468, -1242, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(2526, -1376, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(2734, -1286, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(2746, -1148, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(2519, -1123, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(2599, -1212, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(2664, -1398, 25));
		AddSpawnPoint("d_thorn_21.Id3", "d_thorn_21", Rectangle(2396, -1143, 25));

		// 'Chafperor' GenType 116 Spawn Points
		AddSpawnPoint("d_thorn_21.Id4", "d_thorn_21", Rectangle(1685, 1220, 9999));

		// 'Chafperor' GenType 122 Spawn Points
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1909, 14, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1555, -1009, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1147, -1086, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1778, -1006, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(890, -1300, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1128, -1253, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(2158, 1370, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(2251, 1470, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1648, 1029, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1486, 1304, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1702, -194, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(973, -1160, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1684, -1093, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1721, 49, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1623, -70, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1923, -113, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1410, 1169, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1692, 1144, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1078, -1364, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1668, -972, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(890, -1136, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(1865, -222, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(3016, 1057, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(3144, 1306, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(3214, 1027, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(2979, 1276, 30));
		AddSpawnPoint("d_thorn_21.Id5", "d_thorn_21", Rectangle(3097, 1180, 30));

		// 'Ammon' GenType 124 Spawn Points
		AddSpawnPoint("d_thorn_21.Id6", "d_thorn_21", Rectangle(794, 83, 9999));

		// 'Matsum' GenType 135 Spawn Points
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(223, 59, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(182, -32, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(463, 74, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(726, 217, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(757, 53, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(953, 99, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(288, -28, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(639, 147, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(849, -15, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(845, 120, 20));
		AddSpawnPoint("d_thorn_21.Id7", "d_thorn_21", Rectangle(828, 437, 20));

		// 'Infroholder_Mage' GenType 139 Spawn Points
		AddSpawnPoint("d_thorn_21.Id8", "d_thorn_21", Rectangle(815, -1084, 30));
		AddSpawnPoint("d_thorn_21.Id8", "d_thorn_21", Rectangle(1091, -1300, 30));
		AddSpawnPoint("d_thorn_21.Id8", "d_thorn_21", Rectangle(1243, -1065, 30));
		AddSpawnPoint("d_thorn_21.Id8", "d_thorn_21", Rectangle(1637, -944, 30));
		AddSpawnPoint("d_thorn_21.Id8", "d_thorn_21", Rectangle(1815, -1084, 30));

		// 'Infroholder_Mage' GenType 140 Spawn Points
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(3414, -459, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(3673, -418, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(4456, -76, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(4546, -218, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(4788, -198, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(4673, -88, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(3541, -263, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(3381, -287, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(1601, 1254, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(1401, 1058, 30));
		AddSpawnPoint("d_thorn_21.Id9", "d_thorn_21", Rectangle(1385, 1353, 30));
	}
}
