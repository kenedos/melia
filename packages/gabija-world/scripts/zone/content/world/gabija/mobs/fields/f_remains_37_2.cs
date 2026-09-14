//--- Melia Script -----------------------------------------------------------
// Namu Temple Ruins Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_remains_37_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains372MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_remains_37_2.Id1", MonsterId.Rootcrystal_04, min: 30, max: 40, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("f_remains_37_2.Id2", MonsterId.Lizardman_Mage, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("f_remains_37_2.Id3", MonsterId.Minos, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("f_remains_37_2.Id4", MonsterId.Minos_Bow, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("f_remains_37_2.Id5", MonsterId.Minos, min: 15, max: 20, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_04' GenType 3 Spawn Points
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(1757, -451, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(1552, -419, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(1245, -432, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(970, -301, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(868, -453, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(880, -1, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(856, -701, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(849, -1148, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(1247, -1202, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(872, 272, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(982, 714, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(685, 838, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(728, 1338, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(754, 1860, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(1126, 221, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(371, 257, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(16, 246, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-289, 267, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-603, 271, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-890, 269, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-1358, 273, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-1642, 32, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(80, -258, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(30, -659, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-149, -918, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-48, -1200, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(100, -1492, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-507, -812, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-1059, -799, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-764, 555, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-775, 896, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-240, 945, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(84, 813, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-1059, 994, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-1377, 919, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-1515, 1248, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-1358, 1470, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-1422, 686, 40));
		AddSpawnPoint("f_remains_37_2.Id1", "f_remains_37_2", Rectangle(-789, 1408, 40));

		// 'Lizardman_Mage' GenType 23 Spawn Points
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-1114, -926, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-994, -780, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-871, -923, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-317, -748, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-111, -553, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(123, -754, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-89, -779, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-133, -1329, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-89, -1531, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(174, -1421, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(0, -1417, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(893, -1222, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(1007, -1078, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(1120, -1353, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(1298, -1140, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(1141, -1197, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(942, -472, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(1088, -178, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(61, -1290, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-74, -982, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-789, -781, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-1256, -726, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(1320, -1319, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(1156, -1004, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(145, -568, 25));
		AddSpawnPoint("f_remains_37_2.Id2", "f_remains_37_2", Rectangle(-239, -928, 25));

		// 'Minos' GenType 24 Spawn Points
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(997, -227, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(699, -192, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(1134, -529, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(1193, -332, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(682, 804, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(929, 904, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(931, 695, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(784, 600, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(609, 1695, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(786, 1792, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(823, 1510, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1500, 1163, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1304, 929, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-240, 1086, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(965, 1893, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1777, 252, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1508, 213, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-69, 414, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-9, 242, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(888, -327, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(582, 1452, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(114, 846, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1450, 859, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(791, 1304, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(1036, 1592, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(1229, 564, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1485, 637, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1342, -58, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1398, 1501, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1247, 1238, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-86, 883, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-256, 251, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(1136, 748, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(541, 1979, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(793, -409, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-110, -804, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-269, -977, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(24, -897, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-273, -666, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-118, -628, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(105, -753, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1045, -935, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1098, -731, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-856, -715, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-922, -916, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-972, -831, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-160, 354, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(215, 239, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1581, 987, 40));
		AddSpawnPoint("f_remains_37_2.Id3", "f_remains_37_2", Rectangle(-1591, 44, 40));

		// 'Minos_Bow' GenType 25 Spawn Points
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-203, 268, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-1593, 1178, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-1567, 85, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-1301, 76, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-121, 927, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-407, 1000, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-7, 1057, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-1373, 670, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-1620, 923, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-1261, 1243, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(97, 250, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-1503, 329, 40));
		AddSpawnPoint("f_remains_37_2.Id4", "f_remains_37_2", Rectangle(-1340, 1529, 40));

		// 'Minos' GenType 31 Spawn Points
		AddSpawnPoint("f_remains_37_2.Id5", "f_remains_37_2", Rectangle(-156, 929, 9999));
	}
}
