//--- Melia Script -----------------------------------------------------------
// Bellai Rainforest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_orchard_32_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard323MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_orchard_32_3.Id1", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Minutes(1));
		AddSpawner("f_orchard_32_3.Id2", MonsterId.Ferret_Patter, min: 15, max: 20);
		AddSpawner("f_orchard_32_3.Id3", MonsterId.Ferret_Slinger, min: 15, max: 20);
		AddSpawner("f_orchard_32_3.Id4", MonsterId.Papayam, min: 15, max: 20);
		AddSpawner("f_orchard_32_3.Id5", MonsterId.Ferret_Patter, min: 12, max: 15);
		AddSpawner("f_orchard_32_3.Id6", MonsterId.Ferret_Slinger, min: 12, max: 15);
		AddSpawner("f_orchard_32_3.Id7", MonsterId.Ferret_Searcher, min: 15, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 3 Spawn Points
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(245, -1167, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-424, -1061, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-1068, -1069, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-1343, -329, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-784, -336, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-1003, 388, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-1431, 787, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-223, -318, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-55, 29, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(223, -194, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(1069, -934, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(1343, -854, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(769, -594, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(789, 351, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(1191, 54, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(1198, 1064, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(934, 771, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(188, 435, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-522, 127, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-234, 682, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(117, 1388, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-15, 1127, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-699, 1024, 100));
		AddSpawnPoint("f_orchard_32_3.Id1", "f_orchard_32_3", Rectangle(-939, -856, 100));

		// 'Ferret_Patter' GenType 13 Spawn Points
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(1068, -914, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(800, 358, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(908, 981, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(1398, 208, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(186, 501, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(-43, 1104, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(-239, -319, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(-1454, 787, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(-951, 335, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(432, -1162, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(-321, -1115, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(-981, -1223, 130));
		AddSpawnPoint("f_orchard_32_3.Id2", "f_orchard_32_3", Rectangle(-1403, -323, 130));

		// 'Ferret_Slinger' GenType 14 Spawn Points
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(903, 173, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(1213, 50, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(1197, 1051, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(342, 332, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(82, 1052, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(-1179, 465, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(-1343, 951, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(-180, -409, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(-1105, -212, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(-952, -942, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(-383, -999, 130));
		AddSpawnPoint("f_orchard_32_3.Id3", "f_orchard_32_3", Rectangle(305, -1180, 130));

		// 'Papayam' GenType 15 Spawn Points
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(510, -1147, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(470, -1066, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(353, -995, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(225, -1088, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(342, -1213, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-278, -1120, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-384, -1117, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-366, -969, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-908, -1201, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-991, -1238, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1097, -1154, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1011, -965, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-885, -958, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1009, -1094, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1118, -163, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1200, -66, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1432, -278, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1341, -356, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1156, -284, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-267, -327, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-162, -414, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-173, -268, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-948, 292, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1002, 393, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1154, 398, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1081, 222, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1265, 689, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1381, 655, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1490, 774, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(-1325, 916, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(759, -579, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(620, -468, 25));
		AddSpawnPoint("f_orchard_32_3.Id4", "f_orchard_32_3", Rectangle(410, -341, 25));

		// 'Ferret_Patter' GenType 16 Spawn Points
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(463, -1024, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(429, -1172, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(243, -1010, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(243, -1105, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-302, -1061, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-471, -1056, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-956, -910, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-989, -1056, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-956, -1185, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-803, -1086, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-1132, -203, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-1180, -106, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-1357, -304, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-1242, -340, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-963, 314, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-1061, 294, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-1325, 684, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-1409, 823, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-1362, 960, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(196, 567, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(163, 470, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(188, 340, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(53, 7, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(203, -185, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(406, -302, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(584, -421, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(1051, -871, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(1094, -1024, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(1165, -1174, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(1321, -1210, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(1321, 211, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(1227, 104, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(1222, -7, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(1199, 1020, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(993, 970, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(942, 802, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(870, 642, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-304, -326, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-230, -234, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-223, -396, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-126, -279, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-128, -462, 25));
		AddSpawnPoint("f_orchard_32_3.Id5", "f_orchard_32_3", Rectangle(-48, -384, 25));

		// 'Ferret_Slinger' GenType 17 Spawn Points
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(899, 997, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(1137, 1057, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(958, 900, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(883, 730, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(1271, 166, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(1206, 34, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(1437, 138, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(1311, -877, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(1390, -995, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(1273, -1049, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(1192, -875, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(713, -546, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(486, -407, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(341, -281, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(332, -176, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(355, 373, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(187, 516, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(176, 376, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1000, 445, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1011, 213, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1476, 696, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1434, 905, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1108, -218, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1302, -257, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1313, -59, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-931, -997, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1120, -1126, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-1224, -1054, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-427, -1056, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-265, -967, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(340, -994, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(409, -1167, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(442, -929, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-269, -385, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-194, -251, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-216, -317, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-97, -320, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-305, -247, 25));
		AddSpawnPoint("f_orchard_32_3.Id6", "f_orchard_32_3", Rectangle(-121, -441, 25));

		// 'Ferret_Searcher' GenType 32 Spawn Points
		AddSpawnPoint("f_orchard_32_3.Id7", "f_orchard_32_3", Rectangle(-52, 63, 9999));
	}
}
