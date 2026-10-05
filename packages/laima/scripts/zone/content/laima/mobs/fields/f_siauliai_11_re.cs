//--- Melia Script -----------------------------------------------------------
// Paupys Crossing Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_siauliai_11_re'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai11ReMobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_siauliai_11_re.Id1", MonsterId.Sec_Popolion_Blue, min: 19, max: 25);
		AddSpawner("f_siauliai_11_re.Id2", MonsterId.Sec_Hanaming, min: 23, max: 30);
		AddSpawner("f_siauliai_11_re.Id3", MonsterId.Sec_Onion_Red, min: 27, max: 35);
		AddSpawner("f_siauliai_11_re.Id4", MonsterId.Woodin, min: 19, max: 25);
		AddSpawner("f_siauliai_11_re.Id5", MonsterId.Rootcrystal_01, min: 19, max: 25, respawn: Seconds(25));
		AddSpawner("f_siauliai_11_re.Id6", MonsterId.Sec_Popolion_Blue, min: 30, max: 40);
		AddSpawner("f_siauliai_11_re.Id7", MonsterId.Sec_Hanaming, min: 27, max: 35);
		AddSpawner("f_siauliai_11_re.Id8", MonsterId.Sec_Onion_Red, min: 30, max: 40);
		AddSpawner("f_siauliai_11_re.Id9", MonsterId.Woodin, min: 30, max: 40);
		AddSpawner("f_siauliai_11_re.Id10", MonsterId.Sec_Onion_Red, min: 12, max: 15);
		AddSpawner("f_siauliai_11_re.Id11", MonsterId.Onion_Red_Elite, amount: 1);

		// Monster Spawn Points -----------------------------

		// 'Sec_Popolion_Blue' GenType 13 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id1", "f_siauliai_11_re", Rectangle(1468, 187, 5000));

		// 'Sec_Hanaming' GenType 14 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id2", "f_siauliai_11_re", Rectangle(1648, 444, 5000));

		// 'Sec_Onion_Red' GenType 15 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id3", "f_siauliai_11_re", Rectangle(1442, 436, 5000));

		// 'Woodin' GenType 16 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id4", "f_siauliai_11_re", Rectangle(1604, 299, 5000));

		// 'Rootcrystal_01' GenType 17 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1462, 266, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(2403, -560, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(2164, -870, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(2057, -465, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(2266, -90, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(2384, 370, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(2598, 606, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(2693, 869, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(2314, 982, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1552, -170, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1175, -227, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1524, -699, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1266, -570, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(592, -1333, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(751, -1064, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1144, -1174, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1230, -1397, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1083, -1090, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(681, -596, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(781, -382, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(860, -65, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(576, 0, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(669, 296, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(922, 487, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1228, 442, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1628, 183, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(853, 1359, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1102, 1492, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(1047, 989, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(825, 777, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(627, 581, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(294, 2102, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(294, 1612, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-169, 1133, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-406, 1540, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-354, 826, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-342, 429, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-394, 81, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-94, 256, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-652, 359, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-954, 760, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-1189, 1001, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-1080, 1433, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-1301, 1856, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-1518, 1441, 150));
		AddSpawnPoint("f_siauliai_11_re.Id5", "f_siauliai_11_re", Rectangle(-951, 152, 150));

		// 'Sec_Popolion_Blue' GenType 313 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2272, 1005, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2566, 813, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2701, 884, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2588, 509, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2496, 376, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2276, 401, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2369, 618, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2548, 602, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2660, 661, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2240, 201, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2258, 120, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2272, -23, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2313, -178, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2239, -281, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2109, -319, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2020, -405, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2061, -531, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2177, -450, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2152, -753, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2150, -896, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2126, -992, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(1839, -407, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2026, -674, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2141, -95, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2411, 870, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(1952, -329, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(1912, -558, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2141, -644, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2111, -235, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2211, 42, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2311, 512, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2451, 495, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2365, 706, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2452, 756, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2601, 747, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2679, 855, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2352, 1010, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2196, 950, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2243, 1079, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(2579, 690, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(456, -1482, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(552, -1317, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(764, -1233, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(551, -1081, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(577, -986, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(907, -949, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(998, -874, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(1096, -1239, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(1115, -1377, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(1011, -778, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(745, -403, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(760, -319, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(579, -500, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(830, -644, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(927, -526, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(829, -539, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(710, -1360, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(514, -1168, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(782, -1076, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(1056, -1108, 30));
		AddSpawnPoint("f_siauliai_11_re.Id6", "f_siauliai_11_re", Rectangle(1229, -1286, 30));

		// 'Sec_Hanaming' GenType 314 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1720, 448, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1591, 210, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1443, 375, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1343, 319, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1270, 70, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1365, -76, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1509, -210, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1186, -189, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1298, -430, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1242, -565, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1371, -729, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1102, -194, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(867, -1, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(685, -70, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(935, -600, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(693, -605, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(817, -432, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(505, -1034, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(713, -1368, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(664, -1197, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(368, -1377, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(556, -1412, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(802, -1096, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1166, -1268, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1392, -1508, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1479, -1504, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1489, -1441, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1027, -1065, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1371, -1370, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(949, -884, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1617, 379, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1541, -681, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(622, -403, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(900, 58, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(112, 1335, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(217, 1503, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(269, 1424, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(484, -1152, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(700, -1092, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(816, -1270, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(960, -1149, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(871, -1000, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1102, -1326, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1202, -1498, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1583, -1493, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1161, -575, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1232, -436, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1413, -664, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1230, -44, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1236, 224, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1274, 356, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1517, 533, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1574, -1385, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1462, -1313, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1575, -1283, 30));
		AddSpawnPoint("f_siauliai_11_re.Id7", "f_siauliai_11_re", Rectangle(1290, -1423, 30));

		// 'Sec_Onion_Red' GenType 315 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(770, 360, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(559, 510, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(749, 763, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(905, 919, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1070, 1009, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1050, 1410, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1064, 1246, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(917, 1536, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(273, 2213, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(262, 1853, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(289, 1630, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(169, 1387, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(864, 1315, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(345, 741, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(655, 289, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(46, 775, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-152, 781, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-409, 794, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-415, 642, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1107, 1563, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(178, 2070, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(325, 2002, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(171, 1585, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(723, 604, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(981, 737, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(944, 557, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(858, 661, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1102, 588, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(181, 1968, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(284, 1942, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(234, 2168, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(354, 2256, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(279, 1495, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-18, 1283, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-163, 1146, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-317, 1019, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-429, 1017, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-275, 814, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-336, 664, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-549, 712, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-412, 567, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(852, 1455, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(957, 1424, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1077, 1520, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1018, 952, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(899, 792, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(794, 866, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(993, 628, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1091, 759, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1001, 1616, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(1163, 1417, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(331, 1397, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-293, 883, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-537, 1335, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-553, 1572, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-333, 1492, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-349, 1324, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-403, 1519, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-607, 1508, 30));
		AddSpawnPoint("f_siauliai_11_re.Id8", "f_siauliai_11_re", Rectangle(-421, 1364, 30));

		// 'Woodin' GenType 316 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-263, 1023, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-392, 802, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-437, 305, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-351, 67, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-778, 583, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-989, 197, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-851, 213, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-941, 776, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-534, 644, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1198, 1239, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1266, 1709, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1335, 1876, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1319, 2030, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1555, 1485, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1407, 1626, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1162, 1536, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-674, 334, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-334, 602, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-385, 1098, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1016, 1331, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1495, 1852, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-222, 803, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-423, 156, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-512, -5, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-284, 254, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-398, -24, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-291, 101, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-498, 83, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1004, 104, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-818, 57, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-926, 37, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-900, 349, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-753, 632, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-597, 477, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-636, 694, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1009, 871, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1100, 834, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1070, 997, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1220, 984, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-1224, 1163, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-417, 950, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-417, 880, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(195, 1622, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-579, 1452, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-487, 1630, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-305, 1427, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-429, 1428, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-515, 1503, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-246, 1075, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-106, 1203, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-391, 704, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-269, 875, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-380, 392, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-268, 783, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-516, 742, 30));
		AddSpawnPoint("f_siauliai_11_re.Id9", "f_siauliai_11_re", Rectangle(-648, 433, 30));

		// 'Sec_Onion_Red' GenType 318 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-629, 343, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-508, 719, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-532, 428, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-425, 404, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-487, 274, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-346, 269, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-304, 375, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-357, 566, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-350, 712, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-222, 643, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-683, 661, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-669, 539, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-789, 605, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-992, 693, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1113, 872, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-948, 782, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1143, 1123, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1216, 1098, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-478, 84, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-335, 0, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-392, 183, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-223, 219, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-253, 277, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1388, 1598, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1245, 1661, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1263, 1924, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1497, 1835, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1331, 1680, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1221, 1770, 30));
		AddSpawnPoint("f_siauliai_11_re.Id10", "f_siauliai_11_re", Rectangle(-1350, 1881, 30));

		// 'Onion_Red_Elite' GenType 319 Spawn Points
		AddSpawnPoint("f_siauliai_11_re.Id11", "f_siauliai_11_re", Rectangle(-474, 291, 9999));
		AddSpawnPoint("f_siauliai_11_re.Id11", "f_siauliai_11_re", Rectangle(-355, 853, 9999));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Woodspirit, "f_siauliai_11_re", 1, Hours(2), Hours(4));
	}
}
