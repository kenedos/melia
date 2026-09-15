//--- Melia Script -----------------------------------------------------------
// Veja Ravine Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_huevillage_58_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage581MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_huevillage_58_1.Id1", MonsterId.Tanu, min: 8, max: 10);
		AddSpawner("f_huevillage_58_1.Id2", MonsterId.Tipio, min: 15, max: 20);
		AddSpawner("f_huevillage_58_1.Id3", MonsterId.Beetow, min: 9, max: 12);
		AddSpawner("f_huevillage_58_1.Id4", MonsterId.Rootcrystal_01, min: 9, max: 12, respawn: Seconds(30));
		AddSpawner("f_huevillage_58_1.Id5", MonsterId.Tipio, min: 12, max: 15);
		AddSpawner("f_huevillage_58_1.Id6", MonsterId.Doyor, min: 9, max: 12);
		AddSpawner("f_huevillage_58_1.Id7", MonsterId.Tipio, min: 4, max: 5);
		AddSpawner("f_huevillage_58_1.Id8", MonsterId.Siaulav_Bow, min: 8, max: 10);

		// Monster Spawn Points -----------------------------

		// 'Tanu' GenType 19 Spawn Points
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1229, 1061, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1322, 1162, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1409, 813, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1562, 883, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1314, 946, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1470, 1247, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1251, 824, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(871, 1002, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(986, 879, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1082, 956, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1575, 1061, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1422, 1066, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1476, 932, 20));
		AddSpawnPoint("f_huevillage_58_1.Id1", "f_huevillage_58_1", Rectangle(1511, 753, 20));

		// 'Tipio' GenType 20 Spawn Points
		AddSpawnPoint("f_huevillage_58_1.Id2", "f_huevillage_58_1", Rectangle(708, -1080, 9999));

		// 'Beetow' GenType 22 Spawn Points
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(888, 909, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(1286, 1006, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(-727, 920, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(1425, 773, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(-281, 765, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(1587, 1000, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(-222, 964, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(-451, 957, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(-967, 874, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(-883, 1195, 20));
		AddSpawnPoint("f_huevillage_58_1.Id3", "f_huevillage_58_1", Rectangle(1434, 1090, 20));

		// 'Rootcrystal_01' GenType 27 Spawn Points
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(1027, -1010, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(447, -1097, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(-588, -1229, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(324, -544, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(706, -290, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(-456, -99, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(-927, 607, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(-1169, 958, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(-861, 1202, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(558, 911, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(1482, 1196, 100));
		AddSpawnPoint("f_huevillage_58_1.Id4", "f_huevillage_58_1", Rectangle(1446, 779, 100));

		// 'Tipio' GenType 30 Spawn Points
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(-459, -1253, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(-383, -1049, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(-676, -1187, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(-591, -1035, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(-265, -1190, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(434, -598, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(290, -602, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(-350, -169, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(-364, -398, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(-521, -240, 35));
		AddSpawnPoint("f_huevillage_58_1.Id5", "f_huevillage_58_1", Rectangle(242, -444, 35));

		// 'Doyor' GenType 31 Spawn Points
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(-576, -1294, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(-561, -1003, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(-234, -1316, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(-101, -1113, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(283, -554, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(472, -597, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(417, -457, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(-35, -406, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(-329, -334, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(-519, -272, 35));
		AddSpawnPoint("f_huevillage_58_1.Id6", "f_huevillage_58_1", Rectangle(-482, -137, 35));

		// 'Tipio' GenType 37 Spawn Points
		AddSpawnPoint("f_huevillage_58_1.Id7", "f_huevillage_58_1", Rectangle(-184, 746, 50));
		AddSpawnPoint("f_huevillage_58_1.Id7", "f_huevillage_58_1", Rectangle(-166, 972, 50));
		AddSpawnPoint("f_huevillage_58_1.Id7", "f_huevillage_58_1", Rectangle(-461, 904, 50));

		// 'Siaulav_Bow' GenType 44 Spawn Points
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-168, -1259, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-414, -1027, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(1371, 855, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(1412, 1144, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-422, 895, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-118, 850, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-229, 1013, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-713, 916, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-276, 720, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(1217, 1013, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(1500, 992, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-450, -325, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-359, -143, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-545, -185, 35));
		AddSpawnPoint("f_huevillage_58_1.Id8", "f_huevillage_58_1", Rectangle(-605, -1110, 35));
	}
}
