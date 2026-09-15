//--- Melia Script -----------------------------------------------------------
// Downtown Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_flash_63'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash63MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_flash_63.Id1", MonsterId.Lemur, min: 12, max: 15);
		AddSpawner("f_flash_63.Id2", MonsterId.Lemur, min: 15, max: 20);
		AddSpawner("f_flash_63.Id3", MonsterId.Goblin2_Hammer, min: 19, max: 25);
		AddSpawner("f_flash_63.Id4", MonsterId.Goblin2_Wand3, min: 12, max: 15);
		AddSpawner("f_flash_63.Id5", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(20));
		AddSpawner("f_flash_63.Id6", MonsterId.Lemur, min: 9, max: 12);
		AddSpawner("f_flash_63.Id7", MonsterId.Goblin2_Wand3, min: 15, max: 20);
		AddSpawner("f_flash_63.Id8", MonsterId.Lemur, min: 6, max: 7);

		// Monster Spawn Points -----------------------------

		// 'Lemur' GenType 3 Spawn Points
		AddSpawnPoint("f_flash_63.Id1", "f_flash_63", Rectangle(489, 876, 9999));

		// 'Lemur' GenType 32 Spawn Points
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-539, -178, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(1041, -171, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(31, -2045, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-509, 438, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(967, -402, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-485, -611, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(831, -184, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-88, -2246, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(1009, -1318, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(946, -1475, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(915, -1273, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(1049, -1220, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-208, -2269, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(15, -2127, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(447, -1561, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(467, -1432, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-298, -719, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-310, -485, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-588, -328, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-484, -71, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-529, 555, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(952, -264, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(1175, -232, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(1098, -58, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(802, -368, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(921, 15, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-169, -2277, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-261, -2232, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-6, -2279, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-421, -2148, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-42, -2170, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-420, -2034, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(-343, -2179, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(909, -1400, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(1095, -1360, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(1141, -1235, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(795, -1315, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(679, -1251, 25));
		AddSpawnPoint("f_flash_63.Id2", "f_flash_63", Rectangle(1061, -1450, 25));

		// 'Goblin2_Hammer' GenType 37 Spawn Points
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-290, -675, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-522, -679, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-550, -325, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-568, -77, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-559, 1009, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-460, 1160, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(348, 875, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(544, 998, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(717, 1876, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(1139, 1966, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-599, 831, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(646, 1708, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(541, 794, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(458, 1134, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(549, 1856, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(1296, 1953, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(1282, 1832, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(154, 852, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-599, 581, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-450, 1087, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-534, 1133, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-384, 1179, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-442, 939, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-470, 933, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-406, -734, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-388, -603, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-311, -567, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-203, -640, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-202, -691, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-480, -469, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-486, -337, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-518, -170, 25));
		AddSpawnPoint("f_flash_63.Id3", "f_flash_63", Rectangle(-560, -173, 25));

		// 'Goblin2_Wand3' GenType 38 Spawn Points
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(-474, -683, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(-488, -214, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(-486, 1081, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(-260, 639, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(463, 925, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(943, -271, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(1314, 1931, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(1569, 2300, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(1632, 2002, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(600, 1896, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(1087, -170, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(146, 806, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(475, 1216, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(-200, -715, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(-441, 929, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(-511, 972, 35));
		AddSpawnPoint("f_flash_63.Id4", "f_flash_63", Rectangle(-476, 1312, 35));

		// 'Rootcrystal_01' GenType 41 Spawn Points
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(-212, -2227, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(64, -2017, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(421, -1486, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(600, -1283, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1004, -1345, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1081, -1165, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1013, -423, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(900, -205, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1079, -199, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(916, 164, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1078, 413, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(544, 774, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(534, 1050, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(243, 803, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1599, 904, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1465, 863, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1261, 1453, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1707, 2026, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1628, 2273, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1473, 2107, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(1242, 2012, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(755, 1757, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(544, 1882, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(-501, 896, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(-374, 1219, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(-564, 579, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(-510, -62, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(-478, -344, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(-483, -637, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(-246, -689, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(248, -906, 100));
		AddSpawnPoint("f_flash_63.Id5", "f_flash_63", Rectangle(420, -644, 100));

		// 'Lemur' GenType 45 Spawn Points
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-236, -2330, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-423, -2128, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-333, -2161, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-252, -2247, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-67, -2255, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-5, -2223, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(33, -1973, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(91, -2003, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(82, -2091, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(40, -2166, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-128, -2250, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-142, -2311, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(158, -1900, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(129, -1876, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(46, -1966, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-272, -2292, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-99, -2249, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-7, -2172, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(11, -2046, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(18, -2014, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(52, -2065, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-4, -2253, 50));
		AddSpawnPoint("f_flash_63.Id6", "f_flash_63", Rectangle(-34, -2296, 50));

		// 'Goblin2_Wand3' GenType 46 Spawn Points
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-381, -659, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-528, -710, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-431, -573, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-290, -541, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-190, -704, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-289, -720, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-525, -369, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-446, -176, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-489, 88, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-548, 64, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-553, -104, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-581, -225, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-463, -318, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-56, -725, 50));
		AddSpawnPoint("f_flash_63.Id7", "f_flash_63", Rectangle(-477, -636, 50));

		// 'Lemur' GenType 47 Spawn Points
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1049, -1065, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1091, -1150, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1100, -1251, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1100, -1302, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1065, -1352, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1026, -1418, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(978, -1423, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(915, -1294, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(935, -1254, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1025, -1300, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(923, -1362, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1031, -1330, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1044, -1201, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1131, -1286, 50));
		AddSpawnPoint("f_flash_63.Id8", "f_flash_63", Rectangle(1103, -1363, 50));
	}
}
