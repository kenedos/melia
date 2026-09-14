//--- Melia Script -----------------------------------------------------------
// Tenants' Farm Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_farm_47_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm471MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_farm_47_1.Id1", MonsterId.Pino_White, min: 15, max: 20);
		AddSpawner("f_farm_47_1.Id2", MonsterId.Geppetto_White, min: 15, max: 20);
		AddSpawner("f_farm_47_1.Id3", MonsterId.Pino_White, min: 12, max: 15);
		AddSpawner("f_farm_47_1.Id4", MonsterId.Rootcrystal_01, min: 23, max: 30, respawn: Minutes(1));

		// Monster Spawn Points -----------------------------

		// 'Pino_White' GenType 45 Spawn Points
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-1279, 390, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-1004, 271, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-1147, 533, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-682, 909, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-524, 933, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-660, 759, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-891, 387, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-217, -1055, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-243, -1261, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-182, -170, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-32, -434, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(134, -318, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-13, -287, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(286, 373, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(399, 225, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(488, 441, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(55, 1172, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(137, 1349, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(291, 1112, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(61, 1001, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-1243, 700, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(414, 377, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-1070, 159, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-1288, 523, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(-568, 885, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(256, 273, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(539, 213, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(112, -442, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(272, 1283, 25));
		AddSpawnPoint("f_farm_47_1.Id1", "f_farm_47_1", Rectangle(193, 1102, 25));

		// 'Geppetto_White' GenType 46 Spawn Points
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(838, 287, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(918, 453, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1002, 288, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1378, 372, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1480, 194, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1303, 198, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(63, 32, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(288, -122, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(702, -100, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1256, -899, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1306, -1244, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1494, -1114, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1419, -905, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1198, -1092, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1293, 916, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1292, 1141, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1486, 1020, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(-217, 182, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(-353, -1088, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(-104, -1057, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(358, -1102, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(539, -1072, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(-136, -1217, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(510, -1269, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1341, -1106, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1199, 1065, 25));
		AddSpawnPoint("f_farm_47_1.Id2", "f_farm_47_1", Rectangle(1232, 312, 25));

		// 'Pino_White' GenType 47 Spawn Points
		AddSpawnPoint("f_farm_47_1.Id3", "f_farm_47_1", Rectangle(315, 471, 9999));

		// 'Rootcrystal_01' GenType 49 Spawn Points
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-1226, -216, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-994, -304, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-1104, -1123, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-750, -1112, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-297, -1146, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-398, -555, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(275, -1104, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(567, -1173, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-823, -838, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-568, -227, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-608, 99, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-1013, 237, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-1168, 613, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-587, 838, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-153, 686, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(62, 1073, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(208, 1297, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(-258, 230, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(218, 416, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(480, 409, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(3, -316, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(185, -58, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(597, -328, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(832, -646, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(1367, -823, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(1302, -1229, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(1341, 106, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(1481, 424, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(1047, 258, 20));
		AddSpawnPoint("f_farm_47_1.Id4", "f_farm_47_1", Rectangle(1117, 630, 20));
	}
}
