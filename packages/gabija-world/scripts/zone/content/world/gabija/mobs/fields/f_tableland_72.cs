//--- Melia Script -----------------------------------------------------------
// Sventimas Exile Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_tableland_72'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland72MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_tableland_72.Id1", MonsterId.Spion_White, min: 15, max: 20);
		AddSpawner("f_tableland_72.Id2", MonsterId.Cronewt_Mage_Blue, min: 15, max: 20);
		AddSpawner("f_tableland_72.Id3", MonsterId.Hohen_Orben_Red, min: 6, max: 8);
		AddSpawner("f_tableland_72.Id4", MonsterId.Lapasape_Brown, min: 23, max: 30);
		AddSpawner("f_tableland_72.Id5", MonsterId.Rootcrystal_03, min: 27, max: 36, respawn: Minutes(1));
		AddSpawner("f_tableland_72.Id6", MonsterId.Spion_White, min: 5, max: 6);

		// Monster Spawn Points -----------------------------

		// 'Spion_White' GenType 1 Spawn Points
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(286, -1124, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(187, -925, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(446, -698, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(487, -979, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(584, -1169, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-470, -1034, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-442, -804, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-1178, -454, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(60, 587, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-1165, -301, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-1027, -212, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-1317, -266, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(82, 551, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-907, -307, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(399, 48, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(578, -158, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(626, 27, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1029, -5, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1228, 39, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1275, -467, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1137, -608, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1232, -742, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1496, 70, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1794, 37, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1853, 202, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1368, 498, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1153, 630, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1345, 764, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(1069, 493, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-279, 889, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-530, 614, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-240, 579, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-595, 792, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-853, 608, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-1102, 509, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-1011, 304, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(-936, 520, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(246, 569, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(756, 1243, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(558, 1194, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(564, 1400, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(374, 1416, 30));
		AddSpawnPoint("f_tableland_72.Id1", "f_tableland_72", Rectangle(798, 1348, 30));

		// 'Cronewt_Mage_Blue' GenType 2 Spawn Points
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(-433, 696, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(-143, 704, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(-1060, -84, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(-1051, -272, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(-241, -903, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(334, 694, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(-438, -845, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(-436, -1070, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(591, -1028, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(610, -850, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(172, -957, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(298, -649, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(341, -1194, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(568, -1224, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(440, -979, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(1274, 643, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(1101, 603, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(469, -122, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(612, -85, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(1194, -649, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(1240, -474, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(1431, -551, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(1727, 13, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(1872, 169, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(1948, 71, 30));
		AddSpawnPoint("f_tableland_72.Id2", "f_tableland_72", Rectangle(-1227, 643, 30));

		// 'Hohen_Orben_Red' GenType 3 Spawn Points
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(-377, -994, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(488, -1227, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(462, -105, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(-1304, -159, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(-1125, 402, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(1256, 586, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(1737, 50, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(1309, -509, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(-326, 742, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(552, -919, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(583, 1242, 30));
		AddSpawnPoint("f_tableland_72.Id3", "f_tableland_72", Rectangle(445, -791, 30));

		// 'Lapasape_Brown' GenType 4 Spawn Points
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-464, 883, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-152, 840, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-391, 487, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-952, 650, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-1262, 469, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-1007, 460, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-1354, -199, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(748, 11, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(481, 879, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-575, -843, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(306, -984, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(461, 1203, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(701, 1393, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(713, 1123, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(509, 74, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(588, -257, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(802, -150, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(299, -65, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1261, -591, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1285, -390, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1084, -681, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1373, -682, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1691, 112, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1950, 94, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1076, 752, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1170, 546, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1481, 683, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1356, 497, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(136, -903, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(489, -709, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(444, -1024, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(660, -972, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(458, -1252, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(285, -647, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-540, -1172, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-588, -1018, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-450, -777, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-291, -981, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-1333, -368, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-1114, -55, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-934, -118, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(-1047, -427, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(384, -257, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(552, -127, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1287, 100, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1002, -26, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1442, 63, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(1277, -42, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(220, -1155, 30));
		AddSpawnPoint("f_tableland_72.Id4", "f_tableland_72", Rectangle(376, -867, 30));

		// 'Rootcrystal_03' GenType 10 Spawn Points
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(332, -727, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(855, -1332, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(524, -1335, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(357, -1026, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(65, -909, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-347, -1197, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-623, -925, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-382, -728, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-412, -295, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-155, -107, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-613, -118, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-305, 192, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(245, -101, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(816, 44, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(563, -162, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(1319, 42, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(1196, -697, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(1310, -409, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(1220, 456, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(1385, 697, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(1651, 100, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(2005, 97, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-885, -196, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-1341, -107, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-1105, -352, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-1182, 442, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-847, 481, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-596, 738, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-343, 556, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(-108, 786, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(228, 570, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(535, 939, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(461, 1309, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(705, 1377, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(741, 1778, 40));
		AddSpawnPoint("f_tableland_72.Id5", "f_tableland_72", Rectangle(606, -769, 40));

		// 'Spion_White' GenType 32 Spawn Points
		AddSpawnPoint("f_tableland_72.Id6", "f_tableland_72", Rectangle(-365, 644, 40));
		AddSpawnPoint("f_tableland_72.Id6", "f_tableland_72", Rectangle(-549, 631, 40));
		AddSpawnPoint("f_tableland_72.Id6", "f_tableland_72", Rectangle(-308, 481, 40));
		AddSpawnPoint("f_tableland_72.Id6", "f_tableland_72", Rectangle(-221, 594, 40));
		AddSpawnPoint("f_tableland_72.Id6", "f_tableland_72", Rectangle(-299, 819, 40));
		AddSpawnPoint("f_tableland_72.Id6", "f_tableland_72", Rectangle(-485, 766, 40));
		AddSpawnPoint("f_tableland_72.Id6", "f_tableland_72", Rectangle(-463, 471, 40));
		AddSpawnPoint("f_tableland_72.Id6", "f_tableland_72", Rectangle(-211, 724, 40));
	}
}
