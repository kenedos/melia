//--- Melia Script -----------------------------------------------------------
// Escanciu Village Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_remains_39'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains39MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_remains_39.Id1", MonsterId.Gravegolem, min: 8, max: 10);
		AddSpawner("f_remains_39.Id2", MonsterId.Zolem, min: 6, max: 8);
		AddSpawner("f_remains_39.Id3", MonsterId.Flying_Flog, min: 12, max: 15);
		AddSpawner("f_remains_39.Id4", MonsterId.Hook, min: 8, max: 10);
		AddSpawner("f_remains_39.Id5", MonsterId.Gravegolem, min: 12, max: 15);
		AddSpawner("f_remains_39.Id6", MonsterId.Zolem, min: 8, max: 10);
		AddSpawner("f_remains_39.Id7", MonsterId.Hook, min: 8, max: 10);
		AddSpawner("f_remains_39.Id8", MonsterId.Flying_Flog, min: 6, max: 8);
		AddSpawner("f_remains_39.Id9", MonsterId.Zolem, min: 8, max: 10);
		AddSpawner("f_remains_39.Id10", MonsterId.Gravegolem, min: 8, max: 10);
		AddSpawner("f_remains_39.Id11", MonsterId.Rootcrystal_01, min: 13, max: 17, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Gravegolem' GenType 2 Spawn Points
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(979, 295, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(853, 235, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(982, 80, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(972, 413, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(869, 523, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(992, 571, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(1148, 499, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(1156, 269, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(1112, 67, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(973, -76, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(1149, -116, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(842, 377, 25));
		AddSpawnPoint("f_remains_39.Id1", "f_remains_39", Rectangle(933, 742, 25));

		// 'Zolem' GenType 3 Spawn Points
		AddSpawnPoint("f_remains_39.Id2", "f_remains_39", Rectangle(470, -404, 25));
		AddSpawnPoint("f_remains_39.Id2", "f_remains_39", Rectangle(297, -523, 25));
		AddSpawnPoint("f_remains_39.Id2", "f_remains_39", Rectangle(235, -373, 25));
		AddSpawnPoint("f_remains_39.Id2", "f_remains_39", Rectangle(359, -426, 25));
		AddSpawnPoint("f_remains_39.Id2", "f_remains_39", Rectangle(508, -564, 25));
		AddSpawnPoint("f_remains_39.Id2", "f_remains_39", Rectangle(544, -362, 25));
		AddSpawnPoint("f_remains_39.Id2", "f_remains_39", Rectangle(646, -506, 25));
		AddSpawnPoint("f_remains_39.Id2", "f_remains_39", Rectangle(685, -375, 25));

		// 'Flying_Flog' GenType 4 Spawn Points
		AddSpawnPoint("f_remains_39.Id3", "f_remains_39", Rectangle(1105, 161, 9999));

		// 'Hook' GenType 7 Spawn Points
		AddSpawnPoint("f_remains_39.Id4", "f_remains_39", Rectangle(-356, 517, 9999));

		// 'Gravegolem' GenType 45 Spawn Points
		AddSpawnPoint("f_remains_39.Id5", "f_remains_39", Rectangle(-1113, -420, 9999));

		// 'Zolem' GenType 46 Spawn Points
		AddSpawnPoint("f_remains_39.Id6", "f_remains_39", Rectangle(1015, 1062, 9999));

		// 'Hook' GenType 47 Spawn Points
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-569, -67, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-623, -165, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-797, 98, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-704, 285, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-538, 143, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-682, 483, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-714, 636, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-393, 417, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-772, -45, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-391, -288, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-511, -335, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-595, -483, 25));
		AddSpawnPoint("f_remains_39.Id7", "f_remains_39", Rectangle(-767, -370, 25));

		// 'Flying_Flog' GenType 48 Spawn Points
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-560, 558, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-679, 448, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-718, 605, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-651, 748, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-379, 619, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-538, 268, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-722, 193, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-391, 384, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-777, 398, 25));
		AddSpawnPoint("f_remains_39.Id8", "f_remains_39", Rectangle(-733, -2, 25));

		// 'Zolem' GenType 50 Spawn Points
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-1331, -416, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-1222, -329, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-993, -432, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-921, -184, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-542, -198, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-662, 10, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-403, -370, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-489, -536, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-679, -443, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-531, -679, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-1142, -79, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-1048, -722, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-901, -590, 30));
		AddSpawnPoint("f_remains_39.Id9", "f_remains_39", Rectangle(-1087, -524, 30));

		// 'Gravegolem' GenType 51 Spawn Points
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(360, -408, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(546, -435, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(716, -423, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(665, -557, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(964, -372, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(1222, -336, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(933, -116, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(1125, -172, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(985, 103, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(485, -550, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(1047, -259, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(1040, -16, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(1014, -494, 30));
		AddSpawnPoint("f_remains_39.Id10", "f_remains_39", Rectangle(871, -409, 30));

		// 'Rootcrystal_01' GenType 54 Spawn Points
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(-1228, -364, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(-1233, -52, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(-997, -652, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(-884, -297, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(-536, -543, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(-504, -136, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(-566, 248, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(-593, 554, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(305, -300, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(591, -448, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(1078, -349, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(1025, -5, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(987, 364, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(934, 734, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(1010, 1079, 200));
		AddSpawnPoint("f_remains_39.Id11", "f_remains_39", Rectangle(996, 1415, 200));
	}
}
