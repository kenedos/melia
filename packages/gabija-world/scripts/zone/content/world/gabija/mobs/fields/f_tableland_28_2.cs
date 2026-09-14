//--- Melia Script -----------------------------------------------------------
// Stogas Plateau Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_tableland_28_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland282MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_tableland_28_2.Id1", MonsterId.Rootcrystal_03, min: 9, max: 11, respawn: Seconds(5));
		AddSpawner("f_tableland_28_2.Id2", MonsterId.Siaulav_Blue, min: 23, max: 30);
		AddSpawner("f_tableland_28_2.Id3", MonsterId.Siaulav_Mage_Blue, min: 12, max: 15);
		AddSpawner("f_tableland_28_2.Id4", MonsterId.Siaulav_Bow_Blue, min: 12, max: 15);
		AddSpawner("f_tableland_28_2.Id5", MonsterId.Lapasape_Blue, min: 6, max: 8);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 3 Spawn Points
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(1392, 1220, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(933, 1032, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(422, 1060, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-25, 1486, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-358, 1528, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(294, 678, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(230, 300, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-87, 490, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-957, -502, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-1467, -544, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-1253, 119, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-718, 308, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-1128, 586, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-1374, 1230, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(-1050, 1272, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(984, -470, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(1332, -405, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(1739, -31, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(92, -1378, 50));
		AddSpawnPoint("f_tableland_28_2.Id1", "f_tableland_28_2", Rectangle(173, -1863, 50));

		// 'Siaulav_Blue' GenType 30 Spawn Points
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(1217, 1253, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(1213, 1026, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(1114, 1162, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(569, 1074, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-110, 1527, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-269, 1468, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-376, 1662, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(232, 649, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-88, 501, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(145, 356, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(1424, 1347, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-733, 343, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-966, 585, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1101, 467, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1549, 432, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1353, 230, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-892, -514, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1210, 1353, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1066, 22, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1054, 1098, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-98, -1781, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(172, -1935, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(411, -1648, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-32, -1490, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(144, -1706, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1397, -541, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1107, -620, 30));
		AddSpawnPoint("f_tableland_28_2.Id2", "f_tableland_28_2", Rectangle(-1264, -346, 30));

		// 'Siaulav_Mage_Blue' GenType 35 Spawn Points
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-265, 1590, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-128, 1308, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(1138, -414, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(906, -411, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(1176, -623, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-74, -1530, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(274, -1431, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(174, -1852, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-117, -1685, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(226, -1633, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-1260, -391, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-1467, -569, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-1194, -685, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-1158, 267, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-1484, -310, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-956, 277, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-1254, 681, 30));
		AddSpawnPoint("f_tableland_28_2.Id3", "f_tableland_28_2", Rectangle(-443, 1455, 30));

		// 'Siaulav_Bow_Blue' GenType 40 Spawn Points
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-1418, -417, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-845, 541, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-938, 392, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-1326, 475, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-1344, 220, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-1374, 1129, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-1422, 1305, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-1222, 1229, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-1054, -476, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(164, -1489, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(62, -1830, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(1045, -402, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(1353, -337, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-758, -538, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-551, -692, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(351, 677, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(27, 536, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(-244, 1597, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(108, 1275, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(1082, 1030, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(1294, 1227, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(1669, 63, 30));
		AddSpawnPoint("f_tableland_28_2.Id4", "f_tableland_28_2", Rectangle(1237, -616, 30));

		// 'Lapasape_Blue' GenType 43 Spawn Points
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(639, 1081, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(407, 969, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(95, 437, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(-805, 398, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(-1382, 369, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(-1225, -539, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(38, -1354, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(34, -1799, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(892, -512, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(1294, -418, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(1717, -34, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(-1276, 1289, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(-223, 1540, 30));
		AddSpawnPoint("f_tableland_28_2.Id5", "f_tableland_28_2", Rectangle(183, 1237, 30));
	}
}
