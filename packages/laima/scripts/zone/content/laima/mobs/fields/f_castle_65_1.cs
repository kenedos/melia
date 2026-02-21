//--- Melia Script -----------------------------------------------------------
// Delmore Hamlet Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_castle_65_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle651MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_castle_65_1.Id1", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(5));
		AddSpawner("f_castle_65_1.Id2", MonsterId.PagAmpullar, min: 29, max: 38);
		AddSpawner("f_castle_65_1.Id3", MonsterId.Charcoal_Walker, min: 21, max: 28);
		AddSpawner("f_castle_65_1.Id4", MonsterId.PagSawyer, min: 21, max: 28);
		AddSpawner("f_castle_65_1.Id5", MonsterId.Pagclamper, min: 27, max: 35);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 3 Spawn Points
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(1202, -956, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(1261, -713, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(1039, -399, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(1115, -142, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(1017, -1339, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(866, -1544, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(504, -1672, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(294, -820, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(131, -1499, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-291, -1613, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-983, -1488, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-1557, -1080, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-365, -866, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-318, -651, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-385, -322, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(1012, 397, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(859, 635, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(947, 1170, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(941, 1460, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(1205, 1300, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(127, 1379, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(138, 1082, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(255, 300, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(90, 432, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-576, 561, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-1588, 716, 50));
		AddSpawnPoint("f_castle_65_1.Id1", "f_castle_65_1", Rectangle(-1826, 774, 50));

		// 'PagAmpullar' GenType 100 Spawn Points
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-564, -1052, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-580, -742, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-403, -755, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-305, -691, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-532, -482, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-407, -366, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-274, -297, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-356, -529, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-479, -625, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1077, -1576, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1082, -1457, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-946, -1540, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-932, -1431, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-813, -1513, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-835, -1375, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-823, -1186, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-990, -1207, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1126, -1124, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1170, -1301, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1610, -1163, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1528, -1114, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1518, -999, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1337, -1062, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1271, -1184, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-1193, -1234, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-675, -1603, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-429, -1661, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-373, -895, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-488, -929, 30));
		AddSpawnPoint("f_castle_65_1.Id2", "f_castle_65_1", Rectangle(-540, -948, 30));

		// 'Charcoal_Walker' GenType 101 Spawn Points
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(-340, -1710, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(-309, -1417, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(-232, -1611, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(-57, -1455, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(25, -1569, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(98, -1681, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(266, -1706, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(234, -1568, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(164, -1796, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(192, -1447, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(368, -1468, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(501, -1645, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(571, -1477, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(626, -1589, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(770, -1671, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(953, -1544, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(724, -1325, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(838, -1182, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(898, -1359, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(1111, -1449, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(1154, -1287, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(991, -1264, 30));
		AddSpawnPoint("f_castle_65_1.Id3", "f_castle_65_1", Rectangle(806, -1528, 30));

		// 'PagSawyer' GenType 102 Spawn Points
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1121, -868, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1223, -827, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1276, -699, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1146, -725, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1072, -593, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1164, -540, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1018, -395, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1063, -254, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(922, -207, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1105, -38, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(834, 499, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(916, 686, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(989, 546, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1141, -398, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(957, -478, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1259, -268, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1292, -937, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(1344, -774, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(-6, 402, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(179, 527, 30));
		AddSpawnPoint("f_castle_65_1.Id4", "f_castle_65_1", Rectangle(309, 525, 30));

		// 'Pagclamper' GenType 103 Spawn Points
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(968, 430, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(869, 557, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(1040, 567, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(1056, 682, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(998, 823, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(1026, 1096, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(882, 1241, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(924, 1395, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(977, 1274, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(1169, 1244, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(1237, 1427, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(1280, 1536, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(1307, 1214, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(843, 1534, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(783, 683, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(780, 422, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(-22, 1211, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(41, 1349, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(125, 1228, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(177, 1060, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(302, 1155, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(190, 1361, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(244, 1468, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(-64, 272, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(-69, 503, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(54, 627, 30));
		AddSpawnPoint("f_castle_65_1.Id5", "f_castle_65_1", Rectangle(237, 584, 30));
	}
}
