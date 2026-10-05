//--- Melia Script -----------------------------------------------------------
// Carlyle's Mausoleum Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'id_catacomb_33_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb332MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("id_catacomb_33_2.Id1", MonsterId.Rootcrystal_05, min: 12, max: 15, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("id_catacomb_33_2.Id2", MonsterId.Sec_Spector_Gh_Purple, min: 9, max: 12, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_2.Id3", MonsterId.Slime_Dark_Green, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_2.Id4", MonsterId.Sec_Spector_Gh_Purple, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_2.Id5", MonsterId.Sec_Stoulet_Green, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_2.Id6", MonsterId.Sec_Colitile, min: 45, max: 60, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' GenType 2 Spawn Points
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-1548, 319, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-1190, 1075, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-930, 1079, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-524, 561, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-513, 272, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-796, -440, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-811, -744, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-492, -746, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(95, -1368, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-128, -1121, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-164, -156, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(317, 489, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(522, 221, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(804, -285, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(1254, -428, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(1312, 230, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(1008, 246, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(525, 713, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(263, 1328, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-26, 1352, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(28, 1019, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(1568, 1318, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(1191, 1323, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(728, 1122, 100));
		AddSpawnPoint("id_catacomb_33_2.Id1", "id_catacomb_33_2", Rectangle(-1267, 598, 100));

		// 'Sec_Spector_Gh_Purple' GenType 10 Spawn Points
		AddSpawnPoint("id_catacomb_33_2.Id2", "id_catacomb_33_2", Rectangle(-1277, 1237, 100));
		AddSpawnPoint("id_catacomb_33_2.Id2", "id_catacomb_33_2", Rectangle(-762, 1244, 100));
		AddSpawnPoint("id_catacomb_33_2.Id2", "id_catacomb_33_2", Rectangle(-1245, 1086, 100));
		AddSpawnPoint("id_catacomb_33_2.Id2", "id_catacomb_33_2", Rectangle(-799, 1088, 100));

		// 'Slime_Dark_Green' GenType 23 Spawn Points
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-779, 406, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-547, 537, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-579, 315, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-218, -88, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-827, -637, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-782, -449, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-653, -591, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-513, -651, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-624, -770, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(165, 140, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(133, 309, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(314, 385, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(284, 237, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(398, 94, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(492, 286, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(952, 120, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(1011, 327, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(1187, 457, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(1098, 148, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(1251, -6, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(1039, -406, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(930, -484, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(1130, -341, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(1224, -413, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(1143, -486, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(593, 1262, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(704, 1454, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(816, 1500, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(842, 1231, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(666, 1299, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(527, 1408, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(705, 901, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(670, 755, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(457, 641, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(339, 651, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(88, 340, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(52, 128, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-88, 1300, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-289, 1311, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-282, 1016, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-66, 1021, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-794, 611, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-840, 490, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-685, 649, 30));
		AddSpawnPoint("id_catacomb_33_2.Id3", "id_catacomb_33_2", Rectangle(-812, 849, 30));

		// 'Sec_Spector_Gh_Purple' GenType 24 Spawn Points
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(652, 1186, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(695, 1440, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(851, 1491, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(884, 1308, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(818, 1094, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(547, 1355, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(727, 1344, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(-299, 1304, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(-217, 1387, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(-158, 1279, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(-3, 1117, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(15, 999, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(-190, 1050, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(-315, 1037, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(220, 1322, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(339, 1352, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(716, 892, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(719, 814, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(645, 723, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(363, 695, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(134, 416, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(348, 414, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(486, 265, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(329, 221, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(1006, 208, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(1037, 355, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(1145, 438, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(1308, 432, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(1169, 63, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(1010, 31, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(876, 441, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(1281, 253, 30));
		AddSpawnPoint("id_catacomb_33_2.Id4", "id_catacomb_33_2", Rectangle(1295, 118, 30));

		// 'Sec_Stoulet_Green' GenType 25 Spawn Points
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-539, -637, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-658, -586, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-761, -493, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-486, -497, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-611, -762, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-374, -898, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(46, -1295, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-65, -1213, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-61, -1390, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(100, -1416, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(148, -1200, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-193, -187, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-397, 117, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-1320, -1141, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-1197, -961, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-1061, -1136, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-1203, -1114, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-1159, -1253, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-825, -806, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-903, -584, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-587, -396, 30));
		AddSpawnPoint("id_catacomb_33_2.Id5", "id_catacomb_33_2", Rectangle(-1284, -1028, 30));

		// 'Sec_Colitile' GenType 26 Spawn Points
		AddSpawnPoint("id_catacomb_33_2.Id6", "id_catacomb_33_2", Rectangle(223, 273, 9999));
	}
}
