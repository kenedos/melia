//--- Melia Script -----------------------------------------------------------
// Dadan Jungle Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_bracken_63_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken633MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_bracken_63_3.Id1", MonsterId.Gosaru, min: 30, max: 40, respawn: Minutes(1));
		AddSpawner("f_bracken_63_3.Id2", MonsterId.Ellogua, min: 9, max: 11, respawn: Minutes(1));
		AddSpawner("f_bracken_63_3.Id3", MonsterId.Raffly, min: 30, max: 40, respawn: Minutes(1));
		AddSpawner("f_bracken_63_3.Id4", MonsterId.Rootcrystal_01, min: 17, max: 22, respawn: Seconds(20));
		AddSpawner("f_bracken_63_3.Id5", MonsterId.Bush_Beetle, min: 34, max: 45);
		AddSpawner("f_bracken_63_3.Id6", MonsterId.Bush_Beetle, min: 29, max: 38);
		AddSpawner("f_bracken_63_3.Id7", MonsterId.Blossom_Beetle, min: 5, max: 6);

		// Monster Spawn Points -----------------------------

		// 'Gosaru' GenType 16 Spawn Points
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(87, 16, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-555, -750, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-671, -590, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-463, -519, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-516, -327, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-355, -571, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-447, -676, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-268, -682, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-225, -620, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-561, -503, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-681, -404, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-303, -462, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-677, -754, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-134, 118, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-360, 175, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-269, 350, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(50, 428, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-68, 265, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(231, 124, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(401, 137, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(331, -9, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(758, -307, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(748, -91, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(574, -66, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(483, 32, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(824, -66, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(903, -173, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(641, -892, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(761, -736, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(854, -654, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(925, -800, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(813, -957, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(721, -943, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(882, -938, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(954, -725, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(902, -522, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(73, -527, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(111, -380, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-19, -299, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(55, -227, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(223, -445, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(27, -31, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(232, -257, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(1146, 564, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(959, 658, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(1219, 353, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(1124, 223, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(304, 987, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(397, 1236, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(507, 959, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(704, 813, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(1280, 284, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-1134, 1113, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-1421, 1207, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-1518, 1154, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-1183, 1370, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-1070, 1479, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-882, 1251, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-964, 1174, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-895, 1468, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-752, 1300, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-1080, 808, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-796, 679, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-784, 870, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-745, 793, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-670, 876, 30));
		AddSpawnPoint("f_bracken_63_3.Id1", "f_bracken_63_3", Rectangle(-688, 957, 30));

		// 'Ellogua' GenType 17 Spawn Points
		AddSpawnPoint("f_bracken_63_3.Id2", "f_bracken_63_3", Rectangle(810, -248, 9999));

		// 'Raffly' GenType 308 Spawn Points
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-1134, 1211, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-873, 834, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-349, -1311, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-414, -1181, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-352, -1042, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-266, -958, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-251, -1100, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-245, -1248, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-193, -1311, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-63, -1221, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-87, -1090, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-168, -998, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-136, -890, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-134, -1228, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(1200, 171, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(1325, 389, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(1203, 297, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(1022, 351, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(1119, 455, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(1127, 659, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(914, 619, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(646, 554, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(947, 758, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(781, 677, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(601, 918, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(416, 1022, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(269, 1143, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(406, 1321, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(470, 1180, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(239, 924, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(128, 906, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(264, 751, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(257, 714, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(478, 1088, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(337, 1273, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(1193, 481, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-1076, 627, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-1058, 742, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-955, 646, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-737, 846, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-738, 927, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-601, 914, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-731, 710, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-902, 756, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-1067, 1189, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-1220, 1164, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-973, 1383, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-911, 1255, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-1037, 1298, 30));
		AddSpawnPoint("f_bracken_63_3.Id3", "f_bracken_63_3", Rectangle(-798, 1200, 30));

		// 'Rootcrystal_01' GenType 320 Spawn Points
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-684, -701, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-579, -337, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-352, -702, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-1319, 1083, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-1151, 1366, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-774, 734, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-1209, 225, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-435, 229, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(402, 1274, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(236, 922, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(735, 766, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(800, 462, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(1303, 203, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(1170, 663, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(193, -20, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(741, -150, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(747, -865, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(239, -471, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-416, -1121, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(132, -984, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(-46, -403, 10));
		AddSpawnPoint("f_bracken_63_3.Id4", "f_bracken_63_3", Rectangle(149, 386, 10));

		// 'Sec_Grummer' GenType 321 Spawn Points
		AddSpawnPoint("f_bracken_63_3.Id5", "f_bracken_63_3", Rectangle(71, 31, 9999));

		// 'Sec_Bubbe_Fighter' GenType 322 Spawn Points
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(668, -808, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-206, -1260, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(202, -650, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(795, -164, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-488, -630, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-545, 69, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-1112, 1276, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-878, 746, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-1180, 167, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(368, 1099, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(1094, 482, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-307, -962, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(698, 39, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(902, -309, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(945, -100, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(689, -228, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(863, -627, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(826, -992, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-790, -6, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-1121, 53, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(263, 3, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(560, -16, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(451, 135, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(1069, 714, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(731, 585, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(1241, 303, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(328, 933, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(369, 1305, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-1409, 1070, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-1094, 1445, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-892, 1291, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-548, -295, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-322, -633, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-901, 240, 30));
		AddSpawnPoint("f_bracken_63_3.Id6", "f_bracken_63_3", Rectangle(-815, 117, 30));

		// 'Sec_Bubbe_Fighter' GenType 323 Spawn Points
		AddSpawnPoint("f_bracken_63_3.Id7", "f_bracken_63_3", Rectangle(834, -176, 400));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Gremlin, "f_bracken_63_3", 1, Hours(2), Hours(4));
	}
}
