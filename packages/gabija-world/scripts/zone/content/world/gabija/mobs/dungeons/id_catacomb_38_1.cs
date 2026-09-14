//--- Melia Script -----------------------------------------------------------
// Videntis Shrine Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'id_catacomb_38_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb381MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("id_catacomb_38_1.Id1", MonsterId.Rootcrystal_01, min: 6, max: 7, respawn: Seconds(30), tendency: TendencyType.Peaceful);
		AddSpawner("id_catacomb_38_1.Id2", MonsterId.Socket_Bow, min: 9, max: 12, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_38_1.Id3", MonsterId.Socket_Mage, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_38_1.Id4", MonsterId.Velffigy_Green, min: 12, max: 15, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 4 Spawn Points
		AddSpawnPoint("id_catacomb_38_1.Id1", "id_catacomb_38_1", Rectangle(1282, 18, 100));
		AddSpawnPoint("id_catacomb_38_1.Id1", "id_catacomb_38_1", Rectangle(-1027, -1510, 100));
		AddSpawnPoint("id_catacomb_38_1.Id1", "id_catacomb_38_1", Rectangle(419, -522, 100));
		AddSpawnPoint("id_catacomb_38_1.Id1", "id_catacomb_38_1", Rectangle(-547, 36, 100));
		AddSpawnPoint("id_catacomb_38_1.Id1", "id_catacomb_38_1", Rectangle(-1369, 1038, 100));
		AddSpawnPoint("id_catacomb_38_1.Id1", "id_catacomb_38_1", Rectangle(358, 1123, 100));
		AddSpawnPoint("id_catacomb_38_1.Id1", "id_catacomb_38_1", Rectangle(1397, 1438, 100));

		// 'Socket_Bow' GenType 16 Spawn Points
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-756, 267, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-313, 219, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-813, -5, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-1413, 813, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-1267, 1062, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1568, 1137, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(333, -64, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(333, 147, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-1350, 944, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1211, -93, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1316, -604, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1617, -545, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1630, -95, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1117, -531, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1408, -306, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1525, 32, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-496, 338, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-1132, 991, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-1572, 855, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(-447, 60, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1278, 1141, 30));
		AddSpawnPoint("id_catacomb_38_1.Id2", "id_catacomb_38_1", Rectangle(1122, 979, 30));

		// 'Socket_Mage' GenType 17 Spawn Points
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(-1001, -1603, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(-1144, -1364, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(-1216, -1564, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(-931, -1416, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(-750, -655, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(-502, -676, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(-674, -863, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(185, -698, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(233, -460, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(442, -633, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(1214, -338, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(1711, -355, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(1444, -620, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(1446, -89, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(1453, -432, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(1214, -51, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(158, -590, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(-494, -816, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(258, -62, 30));
		AddSpawnPoint("id_catacomb_38_1.Id3", "id_catacomb_38_1", Rectangle(500, 4, 30));

		// 'Velffigy_Green' GenType 18 Spawn Points
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(136, 1173, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(369, 1032, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(1306, 912, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(1309, 1287, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(1510, 1003, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(1529, 1384, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(1073, 1161, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-9, 922, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-647, 378, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-324, 77, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-1466, 960, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-1268, 816, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-1493, 1092, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-648, 116, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-525, 177, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(579, -11, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(332, 68, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(111, -15, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(1098, 1391, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(961, 1441, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-825, 267, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-531, -104, 30));
		AddSpawnPoint("id_catacomb_38_1.Id4", "id_catacomb_38_1", Rectangle(-472, 423, 30));
	}
}
