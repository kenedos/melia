//--- Melia Script -----------------------------------------------------------
// Aqueduct Bridge Area Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_farm_47_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm472MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_farm_47_2.Id1", MonsterId.Dandel_Orange, min: 18, max: 23);
		AddSpawner("f_farm_47_2.Id2", MonsterId.Cronewt_Mage, min: 9, max: 12);
		AddSpawner("f_farm_47_2.Id3", MonsterId.Kepari_Mage, min: 14, max: 18);
		AddSpawner("f_farm_47_2.Id4", MonsterId.Dandel_Orange, min: 12, max: 15);
		AddSpawner("f_farm_47_2.Id5", MonsterId.Ashrong, min: 12, max: 15);
		AddSpawner("f_farm_47_2.Id6", MonsterId.Ashrong, min: 15, max: 20);
		AddSpawner("f_farm_47_2.Id7", MonsterId.Rootcrystal_01, min: 23, max: 30, respawn: Minutes(1));
		AddSpawner("f_farm_47_2.Id8", MonsterId.Ashrong, amount: 3);

		// Monster Spawn Points -----------------------------

		// 'Dandel_Orange' GenType 4 Spawn Points
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(-708, 1023, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(-432, 1196, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(-493, 960, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(-666, 1165, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(-615, 1724, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(-393, 1858, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(-327, 1662, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(365, 1897, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(97, 1701, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(410, 1733, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(1302, 1772, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(1322, 1609, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(905, 1800, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(1175, 1866, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(1085, 1768, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(176, 398, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(176, 111, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(460, 4, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(529, 287, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(1280, 1034, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(1493, 1122, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(1463, 771, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(500, 530, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(328, -146, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(-733, 1606, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(277, 1775, 25));
		AddSpawnPoint("f_farm_47_2.Id1", "f_farm_47_2", Rectangle(1515, 1707, 25));

		// 'Cronewt_Mage' GenType 37 Spawn Points
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(69, -1104, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(221, -552, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(97, -1565, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(136, -930, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(864, -1647, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(1123, -1727, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(910, -1324, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(1559, -1136, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(2117, -1247, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(2206, -1387, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(2240, -1119, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(2021, -1397, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(991, -1017, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(882, -1453, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(1514, -1526, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(1325, -1677, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(2004, -1158, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(8, -1957, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(142, -2060, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(227, -1795, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(35, -1740, 30));
		AddSpawnPoint("f_farm_47_2.Id2", "f_farm_47_2", Rectangle(756, -1082, 30));

		// 'Kepari_Mage' GenType 38 Spawn Points
		AddSpawnPoint("f_farm_47_2.Id3", "f_farm_47_2", Rectangle(302, -582, 9999));

		// 'Dandel_Orange' GenType 39 Spawn Points
		AddSpawnPoint("f_farm_47_2.Id4", "f_farm_47_2", Rectangle(134, 1026, 1500));

		// 'Ashrong' GenType 40 Spawn Points
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(-33, 388, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(384, 209, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(102, -380, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(102, -1070, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(157, -1445, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(282, -1279, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(965, -1698, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(985, -1157, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(1523, -1291, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(1279, -1718, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(828, -1450, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(362, -152, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(612, -61, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(604, 264, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(190, 523, 25));
		AddSpawnPoint("f_farm_47_2.Id5", "f_farm_47_2", Rectangle(-115, 165, 25));

		// 'Ashrong' GenType 41 Spawn Points
		AddSpawnPoint("f_farm_47_2.Id6", "f_farm_47_2", Rectangle(844, -1315, 9999));

		// 'Rootcrystal_01' GenType 42 Spawn Points
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1982, -1230, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1734, -1396, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1440, -1121, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(926, -1366, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1242, -1682, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(2268, -1476, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(327, -1014, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(42, -1265, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(-477, -1130, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(-990, -1138, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(133, -1834, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(51, -516, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(460, 215, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(88, 446, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(229, -2, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1135, 541, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1215, 50, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1514, 772, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1415, 1142, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(1382, 1600, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(988, 1784, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(444, 1722, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(110, 1725, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(188, 1177, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(120, 887, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(-403, 969, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(-755, 1132, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(-613, 1665, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(-1137, 1088, 10));
		AddSpawnPoint("f_farm_47_2.Id7", "f_farm_47_2", Rectangle(-1528, 1019, 10));

		// 'Ashrong' GenType 56 Spawn Points
		AddSpawnPoint("f_farm_47_2.Id8", "f_farm_47_2", Rectangle(298, -487, 25));
		AddSpawnPoint("f_farm_47_2.Id8", "f_farm_47_2", Rectangle(234, -362, 25));
		AddSpawnPoint("f_farm_47_2.Id8", "f_farm_47_2", Rectangle(386, -350, 25));
		AddSpawnPoint("f_farm_47_2.Id8", "f_farm_47_2", Rectangle(187, -651, 25));
		AddSpawnPoint("f_farm_47_2.Id8", "f_farm_47_2", Rectangle(73, -593, 25));
		AddSpawnPoint("f_farm_47_2.Id8", "f_farm_47_2", Rectangle(374, -678, 25));
		AddSpawnPoint("f_farm_47_2.Id8", "f_farm_47_2", Rectangle(458, -472, 25));
		AddSpawnPoint("f_farm_47_2.Id8", "f_farm_47_2", Rectangle(132, -516, 25));
	}
}
