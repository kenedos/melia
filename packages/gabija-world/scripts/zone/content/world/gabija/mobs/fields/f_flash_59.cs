//--- Melia Script -----------------------------------------------------------
// Verkti Square Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_flash_59'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash59MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_flash_59.Id1", MonsterId.Jukopus_Gray, min: 23, max: 30);
		AddSpawner("f_flash_59.Id2", MonsterId.Rambear, min: 12, max: 15);
		AddSpawner("f_flash_59.Id3", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(20));
		AddSpawner("f_flash_59.Id4", MonsterId.Rambear, min: 9, max: 12);
		AddSpawner("f_flash_59.Id5", MonsterId.Goblin2_Wand1, min: 8, max: 10);
		AddSpawner("f_flash_59.Id6", MonsterId.Jukopus_Gray, min: 12, max: 15);
		AddSpawner("f_flash_59.Id7", MonsterId.Rambear, min: 4, max: 5);
		AddSpawner("f_flash_59.Id8", MonsterId.Jukopus_Gray, min: 6, max: 8);

		// Monster Spawn Points -----------------------------

		// 'Jukopus_Gray' GenType 7 Spawn Points
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(86, 207, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(612, -182, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(760, -289, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(823, -87, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-99, -338, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-327, -342, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(712, 100, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(82, -313, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(919, 538, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(965, 765, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1042, 640, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1133, 483, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1151, 668, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(317, 231, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(131, 450, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(249, 457, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1107, -228, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1090, -29, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1183, 100, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1269, -240, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1344, -82, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1195, -78, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(575, -614, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(791, -595, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-797, 382, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-913, 481, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-827, -1201, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-999, -980, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-643, -1039, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-808, -849, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-815, -1011, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-1459, -380, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-1087, -347, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-1291, -374, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(921, 654, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1066, 754, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1166, 560, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1004, 502, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(814, 565, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(1221, 502, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(853, 451, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(872, 770, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-950, -1140, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-955, -1022, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-910, -917, 30));
		AddSpawnPoint("f_flash_59.Id1", "f_flash_59", Rectangle(-765, -946, 30));

		// 'Rambear' GenType 8 Spawn Points
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-1459, -416, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-1716, -351, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-1392, -270, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-1147, -415, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-1316, -401, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-1584, -319, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-1103, -269, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-907, -1062, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-908, -892, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-721, -1096, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-664, -868, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-789, -985, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-910, -1213, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-686, -1216, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-498, -345, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-381, -265, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-381, -366, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-172, -298, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-19, -412, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-4, -300, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(929, 535, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(1113, 768, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(1168, 578, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-904, 370, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-686, 441, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-721, 298, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-884, 481, 30));
		AddSpawnPoint("f_flash_59.Id2", "f_flash_59", Rectangle(-777, 374, 30));

		// 'Rootcrystal_01' GenType 28 Spawn Points
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-39, 509, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(1037, 746, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(1157, 555, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(875, 492, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(568, -69, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(621, -255, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(413, -266, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(727, 92, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(1224, -58, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(1162, -157, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(763, -581, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(540, -574, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-109, -264, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-387, -351, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-909, -1064, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-744, -957, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-1457, -338, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-1147, -322, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-796, 352, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-712, 441, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-840, -31, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(-436, 427, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(360, 266, 100));
		AddSpawnPoint("f_flash_59.Id3", "f_flash_59", Rectangle(299, 492, 100));

		// 'Rambear' GenType 30 Spawn Points
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(-13, 92, 30));
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(-54, 299, 30));
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(295, 169, 30));
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(308, 504, 30));
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(343, 348, 30));
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(172, 463, 30));
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(24, 509, 30));
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(171, 310, 30));
		AddSpawnPoint("f_flash_59.Id4", "f_flash_59", Rectangle(55, 216, 30));

		// 'Goblin2_Wand1' GenType 31 Spawn Points
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(642, -250, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(751, -73, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(603, -674, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(832, -592, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(1224, 94, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(1179, -201, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(-792, -1166, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(-904, -963, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(508, -507, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(1047, -49, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(1332, -62, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(723, 102, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(969, 600, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(1130, 645, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(982, 767, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(864, 539, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(1101, 524, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(888, 706, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(-1346, -381, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(-722, -356, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(-677, -196, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(-272, -348, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(-35, -76, 25));
		AddSpawnPoint("f_flash_59.Id5", "f_flash_59", Rectangle(-2, -314, 25));

		// 'Jukopus_Gray' GenType 32 Spawn Points
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-719, -1173, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-812, -1179, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-889, -1071, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-960, -935, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-854, -896, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-707, -910, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-633, -900, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-627, -1020, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-732, -1007, 50));
		AddSpawnPoint("f_flash_59.Id6", "f_flash_59", Rectangle(-813, -1035, 50));

		// 'Rambear' GenType 33 Spawn Points
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-799, -1240, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-987, -1087, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-950, -984, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-857, -922, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-664, -881, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-572, -920, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-618, -1091, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-681, -1191, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-970, -1220, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-848, -1145, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-701, -1024, 40));
		AddSpawnPoint("f_flash_59.Id7", "f_flash_59", Rectangle(-822, -1058, 40));

		// 'Jukopus_Gray' GenType 34 Spawn Points
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(572, -645, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(526, -555, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(599, -484, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(685, -507, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(665, -558, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(862, -574, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(811, -656, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(713, -695, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(620, -721, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(732, -621, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(511, -624, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(653, -447, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(772, -530, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(794, -600, 30));
		AddSpawnPoint("f_flash_59.Id8", "f_flash_59", Rectangle(902, -569, 30));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Sparnashorn_2, "f_flash_59", 1, Hours(2), Hours(4));
	}
}
