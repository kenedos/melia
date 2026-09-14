//--- Melia Script -----------------------------------------------------------
// Resident Quarter Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_underfortress_67'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress67MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_underfortress_67.Id1", MonsterId.Rambear_Brown, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_67.Id2", MonsterId.Dandel_White, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_67.Id3", MonsterId.Rambear_Bow_Brown, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_67.Id4", MonsterId.Rambear_Mage_Brown, min: 9, max: 12, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_67.Id5", MonsterId.Rootcrystal_03, min: 18, max: 23, respawn: Seconds(20), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Rambear_Brown' GenType 19 Spawn Points
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-1004, 782, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-339, 1522, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-379, 348, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-641, -274, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-958, -525, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-1376, -1176, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-366, -1011, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(327, -729, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(786, -893, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-176, -1073, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(515, -965, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(310, -907, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(275, 928, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(469, 990, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-295, 936, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(334, 661, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(1238, 774, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(1400, 357, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(1275, -106, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(1338, -265, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(1310, 108, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(1545, -641, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(1259, -761, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(1410, -838, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-901, -738, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-725, -487, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-656, 168, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-661, 588, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-1438, 853, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-1160, 759, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-316, 599, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(-185, 1396, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(764, 614, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(117, 119, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(83, -93, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(417, 0, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(565, 84, 30));
		AddSpawnPoint("d_underfortress_67.Id1", "d_underfortress_67", Rectangle(159, 320, 30));

		// 'Dandel_White' GenType 20 Spawn Points
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-1157, -1136, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-703, -489, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(676, -932, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1395, 508, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1359, -710, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1523, 321, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1748, -1456, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(336, 815, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(888, 540, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-638, 615, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-1408, 754, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-278, 1862, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(404, -977, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-596, 61, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-1552, -1253, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(469, -890, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(82, -1127, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(157, 205, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(233, 554, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(812, 406, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(89, -940, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-974, -716, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-216, -1050, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1318, -841, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1499, -889, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1672, -1629, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1804, -1646, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1725, -1513, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1313, 738, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1342, 298, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1261, 70, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1276, -232, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1541, -290, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(697, 600, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(455, 641, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(418, 910, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(286, 969, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(141, 362, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(951, -785, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1223, -725, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1582, -437, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1557, -699, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1273, 554, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1683, 713, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(1826, 550, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-358, 1576, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-203, 1323, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-281, 919, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-397, 436, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-672, 197, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-666, -261, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-436, 259, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-865, 813, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-1051, 733, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-1446, 653, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-962, -545, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-374, -1062, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(94, -754, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(107, -40, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-588, 4, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(165, -463, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-339, 1289, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(43, -373, 30));
		AddSpawnPoint("d_underfortress_67.Id2", "d_underfortress_67", Rectangle(-713, -292, 30));

		// 'Rambear_Bow_Brown' GenType 23 Spawn Points
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-619, 516, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-656, 255, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-664, 120, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-608, 409, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-850, 480, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-473, 225, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-365, 507, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-279, 837, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(359, 601, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(464, 719, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(257, 694, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(852, 482, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(676, 360, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(907, 187, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(1315, 459, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(1453, -291, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(1273, 862, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-1552, -1202, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-1536, -1026, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-919, -550, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-723, -615, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-766, -345, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(-579, -249, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(616, -932, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(34, -1121, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(463, 38, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(923, -795, 30));
		AddSpawnPoint("d_underfortress_67.Id3", "d_underfortress_67", Rectangle(354, -730, 30));

		// 'Rambear_Mage_Brown' GenType 24 Spawn Points
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(542, -930, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(161, 305, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-1554, -1202, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-1334, -1200, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-706, -658, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-892, -458, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-645, 227, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-605, 584, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-377, 588, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-567, 386, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-909, 513, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-874, 824, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-1377, 687, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-1382, 847, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-1055, 798, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-173, 1411, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-172, 1554, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-301, 989, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-313, 1340, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-320, 1686, 30));
		AddSpawnPoint("d_underfortress_67.Id4", "d_underfortress_67", Rectangle(-715, -134, 30));

		// 'Rootcrystal_03' GenType 30 Spawn Points
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(258, -1359, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(53, -849, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(128, -157, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(420, 702, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(368, 934, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(853, 581, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(552, 385, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(580, -1023, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(1384, -781, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(1744, -1620, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(1658, -605, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(1302, -189, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(1522, 212, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(1417, 667, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-1335, -1172, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-860, -737, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-628, -242, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-755, 868, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-1425, 728, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-350, 330, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-287, 902, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-274, 1490, 40));
		AddSpawnPoint("d_underfortress_67.Id5", "d_underfortress_67", Rectangle(-403, 1990, 40));
	}
}
