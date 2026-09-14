//--- Melia Script -----------------------------------------------------------
// Sirdgela Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_thorn_20'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn20MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_thorn_20.Id1", MonsterId.Flower_Blue, min: 9, max: 12);
		AddSpawner("d_thorn_20.Id2", MonsterId.Groll, min: 9, max: 12);
		AddSpawner("d_thorn_20.Id3", MonsterId.Merog_Wogu, min: 12, max: 15);
		AddSpawner("d_thorn_20.Id4", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(30));
		AddSpawner("d_thorn_20.Id5", MonsterId.Groll, min: 15, max: 20);
		AddSpawner("d_thorn_20.Id6", MonsterId.Merog_Wizzard, min: 12, max: 15);
		AddSpawner("d_thorn_20.Id7", MonsterId.Bagworm, min: 15, max: 20);
		AddSpawner("d_thorn_20.Id8", MonsterId.Flower_Blue, min: 12, max: 15);
		AddSpawner("d_thorn_20.Id9", MonsterId.Merog_Wogu, min: 8, max: 10);

		// Monster Spawn Points -----------------------------

		// 'Flower_Blue' GenType 301 Spawn Points
		AddSpawnPoint("d_thorn_20.Id1", "d_thorn_20", Rectangle(-979, -1948, 9999));

		// 'Groll' GenType 308 Spawn Points
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(-203, -943, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(-365, -1128, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(-198, -1115, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(-425, -934, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(-317, -822, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(-438, -719, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(-271, -616, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(-131, -739, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(0, -886, 25));
		AddSpawnPoint("d_thorn_20.Id2", "d_thorn_20", Rectangle(34, -1054, 25));

		// 'Merog_Wogu' GenType 309 Spawn Points
		AddSpawnPoint("d_thorn_20.Id3", "d_thorn_20", Rectangle(-166, -722, 9999));

		// 'Rootcrystal_01' GenType 514 Spawn Points
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-242, -1817, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-431, -1927, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-403, -2133, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-200, -2212, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-6, -2023, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-218, -2043, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-249, -1113, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-384, -1019, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-193, -860, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-321, -796, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-898, -511, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-1105, -372, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-1363, 203, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-1427, 255, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-886, 1130, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-931, 960, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-683, 867, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-251, 381, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-278, 220, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-22, 18, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(141, 169, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(710, -909, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(1679, -1271, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(1798, -1362, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(1656, -1459, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2778, -1306, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2650, -1213, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2640, -1309, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2637, -1070, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2046, -173, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2213, -445, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2623, 656, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2851, 605, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2848, 408, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2577, 411, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(2856, -1189, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(1055, -1074, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(5, -967, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-1291, -1825, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-1214, -298, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-160, -27, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(1572, -1317, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(636, -930, 200));
		AddSpawnPoint("d_thorn_20.Id4", "d_thorn_20", Rectangle(-773, 1022, 200));

		// 'Groll' GenType 821 Spawn Points
		AddSpawnPoint("d_thorn_20.Id5", "d_thorn_20", Rectangle(-240, -560, 9999));

		// 'Merog_Wizzard' GenType 842 Spawn Points
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-1026, -469, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-1080, 927, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-886, 1167, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-800, 836, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-684, 1043, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-1154, -223, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-296, 430, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(148, 37, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-132, 58, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-1281, -332, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-1306, -188, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-858, 1014, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-397, 215, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-79, 365, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-245, -74, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-987, -336, 25));
		AddSpawnPoint("d_thorn_20.Id6", "d_thorn_20", Rectangle(-9, -106, 25));

		// 'Bagworm' GenType 843 Spawn Points
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(42, -901, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(1448, -1466, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(1503, -1209, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(1237, -1062, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(1174, -1123, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(1127, -1032, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(1101, -1112, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(1288, -1154, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(1261, -1180, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-1041, -298, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-878, 883, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-154, 215, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-439, -1004, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(0, -1090, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-235, -712, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-824, -522, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-1393, -110, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-1401, 319, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-1103, 854, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-1018, 1190, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-702, 1053, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-265, 179, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-426, 68, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-139, 422, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(94, -65, 30));
		AddSpawnPoint("d_thorn_20.Id7", "d_thorn_20", Rectangle(-269, -1206, 30));

		// 'Flower_Blue' GenType 846 Spawn Points
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-182, -1890, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-376, -2266, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-434, -2102, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-382, -1716, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-201, -1459, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-158, -1718, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-120, -2201, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-4, -2011, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(191, -1925, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(213, -1984, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(267, -1896, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(284, -2008, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(262, -1962, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-212, -1512, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-64, -1547, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-129, -1459, 30));
		AddSpawnPoint("d_thorn_20.Id8", "d_thorn_20", Rectangle(-142, -1518, 30));

		// 'Merog_Wogu' GenType 850 Spawn Points
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-208, -1690, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-190, -2164, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-195, -1144, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-386, -865, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-153, -651, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-10, -862, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-468, -1950, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(86, -1973, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-208, -1911, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-524, -2221, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-177, -894, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-799, -633, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-671, -619, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-183, -414, 25));
		AddSpawnPoint("d_thorn_20.Id9", "d_thorn_20", Rectangle(-120, -313, 25));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Archon, "d_thorn_20", 1, Hours(6), Hours(12));
		AddBossSpawner(MonsterId.Boss_Spector_Gh, "d_thorn_20", 1, Hours(6), Hours(12));
	}
}
