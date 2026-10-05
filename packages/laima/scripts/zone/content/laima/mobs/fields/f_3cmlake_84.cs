//--- Melia Script -----------------------------------------------------------
// Absenta Reservoir Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_3cmlake_84'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class F3Cmlake84MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_3cmlake_84.Id1", MonsterId.Rajapearlite_Purple, min: 30, max: 40);
		AddSpawner("f_3cmlake_84.Id2", MonsterId.Slime_Dark_Blue, min: 23, max: 30);
		AddSpawner("f_3cmlake_84.Id3", MonsterId.Sowpent, min: 27, max: 35);
		AddSpawner("f_3cmlake_84.Id4", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Rajapearlite_Purple' GenType 12 Spawn Points
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-65, -1337, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-356, -1005, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-954, -645, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-1201, -313, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-1600, -288, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-1719, -521, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-1297, 1345, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-1327, 1122, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-619, 631, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-691, 711, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-883, 362, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-555, 358, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-101, 1261, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-60, 835, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-419, 1106, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-196, 1131, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(53, 915, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(374, 650, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(404, 370, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-117, -344, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-273, -354, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(446, -31, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(504, -329, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(684, -854, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(409, -964, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-315, -1327, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-136, -1211, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-121, -915, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(131, -1102, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(366, -725, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(373, -271, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(628, -198, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(653, -88, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-1285, 1252, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-1187, 1131, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-738, 417, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-848, 574, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-525, 548, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-101, -250, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-76, -478, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-1, -363, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(-272, -1458, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(317, -822, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(564, -985, 30));
		AddSpawnPoint("f_3cmlake_84.Id1", "f_3cmlake_84", Rectangle(661, -1047, 30));

		// 'Slime_Dark_Blue' GenType 26 Spawn Points
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-455, -467, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-201, -585, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(329, -1028, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(566, -1048, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(623, -960, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-321, -236, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-58, -209, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-301, -520, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(307, 340, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(501, 373, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(376, 442, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(410, 560, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(448, 749, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(479, 600, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(281, 598, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(577, 441, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(557, 634, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(234, 466, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-804, 1156, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-678, 1181, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-757, 1094, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-828, 1022, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-666, 1031, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-256, 1608, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-235, 1766, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-126, 1626, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-123, 1763, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-59, 1643, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-26, 1848, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(7, 1690, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(61, 1762, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-74, 1483, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(-309, 1692, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(398, 308, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(302, 734, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(392, -170, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(379, -388, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(571, -416, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(714, -349, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(733, -170, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(553, -114, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(615, -32, 30));
		AddSpawnPoint("f_3cmlake_84.Id2", "f_3cmlake_84", Rectangle(528, -231, 30));

		// 'Sowpent' GenType 27 Spawn Points
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-153, 1162, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-21, 1001, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-176, 860, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-276, 870, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-255, 1163, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-188, 1037, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(579, 405, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(568, 579, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(577, 690, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-645, 359, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-602, 476, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-751, 523, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-818, 464, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-230, -480, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-209, -275, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-26, -194, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-298, -184, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-154, -1367, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-227, -1245, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-196, -1167, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(466, -932, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(437, -845, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(624, -725, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(618, -804, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(731, -750, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(732, -938, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(534, -860, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(467, -726, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(779, -869, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-1066, 1122, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-1165, 1020, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-1296, 1003, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-1438, 1072, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-1467, 1204, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-1419, 1313, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-1291, 1400, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-1179, 1372, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-37, 1102, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-141, 799, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-177, -181, 30));
		AddSpawnPoint("f_3cmlake_84.Id3", "f_3cmlake_84", Rectangle(-35, -427, 30));

		// 'Rootcrystal_01' GenType 28 Spawn Points
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(679, -787, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(295, -972, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-35, -1286, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-237, -932, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-205, -372, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-910, -281, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-1208, -572, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-1647, -401, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-728, 477, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-1176, 1202, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-745, 1152, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(219, 398, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(548, -56, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-208, 1527, 50));
		AddSpawnPoint("f_3cmlake_84.Id4", "f_3cmlake_84", Rectangle(-108, 1054, 50));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Rajapearl, "f_3cmlake_83", 1, Hours(2), Hours(4));
	}
}
