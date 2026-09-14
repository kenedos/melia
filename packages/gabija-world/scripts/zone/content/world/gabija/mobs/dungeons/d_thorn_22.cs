//--- Melia Script -----------------------------------------------------------
// Dvasia Peak Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_thorn_22'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn22MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_thorn_22.Id1", MonsterId.Rootcrystal_01, min: 8, max: 10, respawn: Seconds(15));
		AddSpawner("d_thorn_22.Id2", MonsterId.Meleech, min: 15, max: 20, respawn: Seconds(15));
		AddSpawner("d_thorn_22.Id3", MonsterId.RavineLerva, min: 12, max: 15, respawn: Seconds(15));
		AddSpawner("d_thorn_22.Id4", MonsterId.Wood_Goblin, min: 23, max: 30, respawn: Seconds(15));
		AddSpawner("d_thorn_22.Id5", MonsterId.Meleech, min: 12, max: 15, respawn: Seconds(20));
		AddSpawner("d_thorn_22.Id6", MonsterId.RavineLerva, min: 11, max: 14, respawn: Seconds(20));
		AddSpawner("d_thorn_22.Id7", MonsterId.TreeGool, min: 10, max: 13, respawn: Seconds(20));
		AddSpawner("d_thorn_22.Id8", MonsterId.Wood_Goblin, min: 9, max: 12, respawn: Seconds(5));
		AddSpawner("d_thorn_22.Id9", MonsterId.RavineLerva, min: 4, max: 5, respawn: Seconds(15));

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 600 Spawn Points
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-833, -1971, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-789, -1422, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-928, -1380, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-1308, -2181, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-1585, -1877, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-2061, -1094, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-2053, -825, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-2033, -1166, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(100, -1307, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(99, -1405, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(457, -1481, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(1039, -1602, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(1249, -1560, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(926, -1197, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(923, -985, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(759, -881, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(14, -586, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(765, 559, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(731, 802, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(500, 540, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-487, 929, 50));
		AddSpawnPoint("d_thorn_22.Id1", "d_thorn_22", Rectangle(-1344, -58, 50));

		// 'Meleech' GenType 818 Spawn Points
		AddSpawnPoint("d_thorn_22.Id2", "d_thorn_22", Rectangle(1003, -1516, 9999));

		// 'RavineLerva' GenType 819 Spawn Points
		AddSpawnPoint("d_thorn_22.Id3", "d_thorn_22", Rectangle(-1576, -1901, 9999));

		// 'Wood_Goblin' GenType 821 Spawn Points
		AddSpawnPoint("d_thorn_22.Id4", "d_thorn_22", Rectangle(-45, 507, 9999));

		// 'Meleech' GenType 822 Spawn Points
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(964, -1437, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(-938, -1221, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(-1281, -1465, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(-881, -1485, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(-414, -1353, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(-1046, -1014, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(-505, -1391, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(722, -1493, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(429, -1479, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(1223, -1664, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(1041, -1788, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(1274, -1321, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(1467, -1647, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(1483, -1443, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(1128, -1547, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(887, -1621, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(457, -1349, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(-832, -1431, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(-342, -1417, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(1286, -1437, 30));
		AddSpawnPoint("d_thorn_22.Id5", "d_thorn_22", Rectangle(1375, -1595, 30));

		// 'RavineLerva' GenType 823 Spawn Points
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(725, -1229, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(650, -1087, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(902, -832, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(780, -989, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1015, -1041, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(907, -1207, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(670, -762, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(448, -871, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(362, -905, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1324, -804, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(393, -1451, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1292, -1627, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(932, -1524, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1204, -1375, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(469, -1365, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(921, -708, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(798, -1072, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1137, -1803, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1081, -1689, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1222, -1458, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1452, -1660, 25));
		AddSpawnPoint("d_thorn_22.Id6", "d_thorn_22", Rectangle(1383, -1406, 25));

		// 'TreeGool' GenType 824 Spawn Points
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1313, -1483, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(987, -1819, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(813, -1425, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1276, -1335, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1372, -1697, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1212, -1686, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(975, -1661, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1485, -1442, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1141, -1412, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1095, -1642, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1262, -1297, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1362, -1574, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(989, -1476, 20));
		AddSpawnPoint("d_thorn_22.Id7", "d_thorn_22", Rectangle(1479, -1574, 20));

		// 'Wood_Goblin' GenType 827 Spawn Points
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(1240, -1264, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(1124, -1681, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(1376, -1620, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(1484, -1417, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(1090, -1510, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(1347, -1433, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(453, -1371, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(402, -1530, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(677, -1443, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(1048, -1844, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-1307, -308, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-1412, -71, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-1283, 334, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-1334, 90, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-1230, 121, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-1206, 465, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(692, -1208, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(907, -1176, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(926, -1084, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(803, -1005, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-1353, -1538, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-742, -1762, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-785, -1604, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-824, -1308, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-982, -1132, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-862, -1193, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-769, -1371, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-766, -1544, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-816, -1821, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-613, -1376, 30));
		AddSpawnPoint("d_thorn_22.Id8", "d_thorn_22", Rectangle(-834, -1442, 30));

		// 'RavineLerva' GenType 830 Spawn Points
		AddSpawnPoint("d_thorn_22.Id9", "d_thorn_22", Rectangle(-1186, 412, 20));
		AddSpawnPoint("d_thorn_22.Id9", "d_thorn_22", Rectangle(-1248, 100, 20));
		AddSpawnPoint("d_thorn_22.Id9", "d_thorn_22", Rectangle(-1336, -77, 20));
		AddSpawnPoint("d_thorn_22.Id9", "d_thorn_22", Rectangle(-1151, 584, 20));
		AddSpawnPoint("d_thorn_22.Id9", "d_thorn_22", Rectangle(-1280, 297, 20));
		AddSpawnPoint("d_thorn_22.Id9", "d_thorn_22", Rectangle(-1386, 80, 20));
		AddSpawnPoint("d_thorn_22.Id9", "d_thorn_22", Rectangle(-1194, 210, 20));
		AddSpawnPoint("d_thorn_22.Id9", "d_thorn_22", Rectangle(-1272, 512, 20));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_GiantWoodGoblin_Red, "d_thorn_22", 1, Hours(6), Hours(12));
		AddBossSpawner(MonsterId.Boss_Ironbaum, "d_thorn_22", 1, Hours(6), Hours(12));
	}
}
