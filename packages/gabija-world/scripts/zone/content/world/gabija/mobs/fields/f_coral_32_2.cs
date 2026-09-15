//--- Melia Script -----------------------------------------------------------
// Igti Coast Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_coral_32_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral322MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_coral_32_2.Id1", MonsterId.Colimen_Blue, min: 19, max: 25);
		AddSpawner("f_coral_32_2.Id2", MonsterId.Repusbunny_Red, min: 19, max: 25);
		AddSpawner("f_coral_32_2.Id3", MonsterId.Repusbunny_Bow_Red, min: 12, max: 15);
		AddSpawner("f_coral_32_2.Id4", MonsterId.Repusbunny_Bow_Red, min: 4, max: 5);
		AddSpawner("f_coral_32_2.Id5", MonsterId.Colimen_Blue, min: 4, max: 5);
		AddSpawner("f_coral_32_2.Id6", MonsterId.Repusbunny_Red, min: 4, max: 5);
		AddSpawner("f_coral_32_2.Id7", MonsterId.Rootcrystal_01, min: 15, max: 20, respawn: Seconds(20));

		// Monster Spawn Points -----------------------------

		// 'Colimen_Blue' GenType 24 Spawn Points
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-901, 101, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-887, -9, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-634, -78, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-491, -244, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1299, -499, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1241, -678, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1168, -568, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1115, -475, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-17, 412, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(44, 606, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-106, 614, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-21, 43, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-36, -186, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-677, 614, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-825, 657, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-672, 824, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-869, 832, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1432, 886, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1569, 694, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1668, 880, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1412, 661, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1451, 815, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1142, -491, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1280, -788, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1342, -660, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-270, 783, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(525, 1090, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1119, -762, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(901, 983, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1009, 1200, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(911, 1304, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(831, 1577, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1066, 1453, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(920, 1473, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1214, 823, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1364, 823, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1042, 950, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1482, 1019, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1346, 498, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(1058, 341, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(896, 424, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(962, 200, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(877, 33, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(790, -186, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(571, -34, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(639, -302, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(231, -401, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-203, -538, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-24, -455, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-67, -876, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(102, -721, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1088, 292, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(-1282, 485, 30));
		AddSpawnPoint("f_coral_32_2.Id1", "f_coral_32_2", Rectangle(836, 175, 30));

		// 'Repusbunny_Red' GenType 25 Spawn Points
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-1457, 798, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-1529, 674, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-1544, 843, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-1309, 735, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(1217, 978, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(1355, 1143, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(1495, 874, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(1382, 940, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(753, 1441, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(952, 1612, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(171, -526, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-39, -747, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-46, -597, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-233, -715, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-29, 38, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-18, -148, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-20, -237, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-26, 684, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-491, 925, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-105, 364, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-97, 513, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(745, 323, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(730, 8, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(1015, 109, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(1031, 514, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(673, -160, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-797, 941, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(1122, -632, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-7, 189, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(1070, -879, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-1101, 434, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-1578, 1052, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-1191, 272, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-654, -260, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-754, -80, 30));
		AddSpawnPoint("f_coral_32_2.Id2", "f_coral_32_2", Rectangle(-531, -105, 30));

		// 'Repusbunny_Bow_Red' GenType 26 Spawn Points
		AddSpawnPoint("f_coral_32_2.Id3", "f_coral_32_2", Rectangle(-739, 745, 9999));

		// 'Repusbunny_Bow_Red' GenType 29 Spawn Points
		AddSpawnPoint("f_coral_32_2.Id4", "f_coral_32_2", Rectangle(1044, -741, 30));
		AddSpawnPoint("f_coral_32_2.Id4", "f_coral_32_2", Rectangle(1211, -712, 30));
		AddSpawnPoint("f_coral_32_2.Id4", "f_coral_32_2", Rectangle(1286, -662, 30));
		AddSpawnPoint("f_coral_32_2.Id4", "f_coral_32_2", Rectangle(1374, -687, 30));
		AddSpawnPoint("f_coral_32_2.Id4", "f_coral_32_2", Rectangle(1275, -804, 30));
		AddSpawnPoint("f_coral_32_2.Id4", "f_coral_32_2", Rectangle(1128, -911, 30));
		AddSpawnPoint("f_coral_32_2.Id4", "f_coral_32_2", Rectangle(1106, -563, 30));

		// 'Colimen_Blue' GenType 30 Spawn Points
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(-106, 91, 30));
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(-11, 126, 30));
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(-38, 293, 30));
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(-55, 384, 30));
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(52, 221, 30));
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(44, 30, 30));
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(37, -98, 30));
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(-21, -254, 30));
		AddSpawnPoint("f_coral_32_2.Id5", "f_coral_32_2", Rectangle(-87, -20, 30));

		// 'Repusbunny_Red' GenType 31 Spawn Points
		AddSpawnPoint("f_coral_32_2.Id6", "f_coral_32_2", Rectangle(-792, 683, 20));
		AddSpawnPoint("f_coral_32_2.Id6", "f_coral_32_2", Rectangle(-689, 542, 20));
		AddSpawnPoint("f_coral_32_2.Id6", "f_coral_32_2", Rectangle(-693, 637, 20));
		AddSpawnPoint("f_coral_32_2.Id6", "f_coral_32_2", Rectangle(-664, 713, 20));
		AddSpawnPoint("f_coral_32_2.Id6", "f_coral_32_2", Rectangle(-634, 765, 20));
		AddSpawnPoint("f_coral_32_2.Id6", "f_coral_32_2", Rectangle(-579, 587, 20));
		AddSpawnPoint("f_coral_32_2.Id6", "f_coral_32_2", Rectangle(-825, 851, 20));
		AddSpawnPoint("f_coral_32_2.Id6", "f_coral_32_2", Rectangle(-714, 893, 20));

		// 'Rootcrystal_01' GenType 33 Spawn Points
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-148, -601, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(675, -134, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(1184, -718, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(942, 305, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(1383, 953, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(881, 965, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(407, 1211, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(109, 1563, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-33, 473, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(0, -65, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-827, -56, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-1434, 724, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-700, 788, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-1383, -454, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-974, -827, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-472, -232, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(957, 1568, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-1122, 346, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-1644, 1048, 100));
		AddSpawnPoint("f_coral_32_2.Id7", "f_coral_32_2", Rectangle(-44, 945, 100));
	}
}
