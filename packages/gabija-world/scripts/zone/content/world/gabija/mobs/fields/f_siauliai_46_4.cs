//--- Melia Script -----------------------------------------------------------
// Dina Bee Farm Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_siauliai_46_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai464MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_siauliai_46_4.Id1", MonsterId.Lantern_Mushroom_Orange, min: 8, max: 10);
		AddSpawner("f_siauliai_46_4.Id2", MonsterId.Lantern_Mushroom_Orange, min: 15, max: 20);
		AddSpawner("f_siauliai_46_4.Id3", MonsterId.Siaulamb, min: 8, max: 10);
		AddSpawner("f_siauliai_46_4.Id4", MonsterId.Siaulogre, min: 3, max: 4);
		AddSpawner("f_siauliai_46_4.Id5", MonsterId.Rabbee, min: 8, max: 10);
		AddSpawner("f_siauliai_46_4.Id6", MonsterId.Honeybean, min: 8, max: 10);
		AddSpawner("f_siauliai_46_4.Id7", MonsterId.Rabbee, min: 12, max: 15);
		AddSpawner("f_siauliai_46_4.Id8", MonsterId.Rootcrystal_01, min: 12, max: 16, respawn: Seconds(30));
		AddSpawner("f_siauliai_46_4.Id9", MonsterId.Pendinmire, amount: 1, respawn: Hours(1));
		AddSpawner("f_siauliai_46_4.Id10", MonsterId.Siaulamb, min: 12, max: 15);
		AddSpawner("f_siauliai_46_4.Id11", MonsterId.Rabbee, min: 15, max: 20);
		AddSpawner("f_siauliai_46_4.Id12", MonsterId.Honeybean, min: 15, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Lantern_Mushroom_Orange' GenType 8 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id1", "f_siauliai_46_4", Rectangle(276, 2, 30));
		AddSpawnPoint("f_siauliai_46_4.Id1", "f_siauliai_46_4", Rectangle(456, -89, 30));
		AddSpawnPoint("f_siauliai_46_4.Id1", "f_siauliai_46_4", Rectangle(196, -92, 30));
		AddSpawnPoint("f_siauliai_46_4.Id1", "f_siauliai_46_4", Rectangle(216, -239, 30));
		AddSpawnPoint("f_siauliai_46_4.Id1", "f_siauliai_46_4", Rectangle(353, -342, 30));
		AddSpawnPoint("f_siauliai_46_4.Id1", "f_siauliai_46_4", Rectangle(473, -261, 30));
		AddSpawnPoint("f_siauliai_46_4.Id1", "f_siauliai_46_4", Rectangle(363, -158, 30));
		AddSpawnPoint("f_siauliai_46_4.Id1", "f_siauliai_46_4", Rectangle(592, -19, 30));

		// 'Lantern_Mushroom_Orange' GenType 21 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id2", "f_siauliai_46_4", Rectangle(439, 199, 9999));

		// 'Siaulamb' GenType 25 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(-356, 360, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(1271, 1906, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(-298, 152, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(1162, 1752, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(1009, 1545, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(-269, 907, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(-145, 773, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(-360, 755, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(1207, 1609, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(-548, 292, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(110, 306, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(313, 283, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(451, 329, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(420, 173, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(-169, 242, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(-394, 243, 25));
		AddSpawnPoint("f_siauliai_46_4.Id3", "f_siauliai_46_4", Rectangle(218, 373, 25));

		// 'Siaulogre' GenType 27 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id4", "f_siauliai_46_4", Rectangle(428, 233, 4000));

		// 'Rabbee' GenType 28 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id5", "f_siauliai_46_4", Rectangle(301, -748, 25));
		AddSpawnPoint("f_siauliai_46_4.Id5", "f_siauliai_46_4", Rectangle(207, -865, 25));
		AddSpawnPoint("f_siauliai_46_4.Id5", "f_siauliai_46_4", Rectangle(427, -651, 25));

		// 'Honeybean' GenType 29 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id6", "f_siauliai_46_4", Rectangle(429, -650, 25));
		AddSpawnPoint("f_siauliai_46_4.Id6", "f_siauliai_46_4", Rectangle(333, -860, 25));
		AddSpawnPoint("f_siauliai_46_4.Id6", "f_siauliai_46_4", Rectangle(232, -655, 25));
		AddSpawnPoint("f_siauliai_46_4.Id6", "f_siauliai_46_4", Rectangle(64, -887, 25));

		// 'Rabbee' GenType 30 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id7", "f_siauliai_46_4", Rectangle(916, -60, 9999));

		// 'Rootcrystal_01' GenType 31 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(927, -738, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(320, -936, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(-240, -1143, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(355, -146, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(1279, -259, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(-983, 288, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(-325, 225, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(-244, 800, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(196, 341, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(552, 227, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(1053, 213, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(248, 1580, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(1102, 1716, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(1360, 2698, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(1355, 2391, 150));
		AddSpawnPoint("f_siauliai_46_4.Id8", "f_siauliai_46_4", Rectangle(1325, -949, 150));

		// 'Pendinmire' GenType 33 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id9", "f_siauliai_46_4", Rectangle(220, 1433, 10));

		// 'Siaulamb' GenType 35 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id10", "f_siauliai_46_4", Rectangle(-1057, 193, 25));
		AddSpawnPoint("f_siauliai_46_4.Id10", "f_siauliai_46_4", Rectangle(-1107, 333, 25));
		AddSpawnPoint("f_siauliai_46_4.Id10", "f_siauliai_46_4", Rectangle(-1061, 436, 25));
		AddSpawnPoint("f_siauliai_46_4.Id10", "f_siauliai_46_4", Rectangle(-893, 471, 25));
		AddSpawnPoint("f_siauliai_46_4.Id10", "f_siauliai_46_4", Rectangle(-912, 190, 25));
		AddSpawnPoint("f_siauliai_46_4.Id10", "f_siauliai_46_4", Rectangle(-961, 118, 25));

		// 'Rabbee' GenType 36 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(834, -751, 25));
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(1050, -756, 25));
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(1137, -641, 25));
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(1219, -810, 25));
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(1120, -921, 25));
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(1293, -956, 25));
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(1201, -1046, 25));
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(1357, -846, 25));
		AddSpawnPoint("f_siauliai_46_4.Id11", "f_siauliai_46_4", Rectangle(1332, -719, 25));

		// 'Honeybean' GenType 37 Spawn Points
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(954, -753, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1067, -660, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1108, -781, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1238, -694, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1268, -824, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1182, -920, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1329, -960, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1236, -1060, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1394, -826, 25));
		AddSpawnPoint("f_siauliai_46_4.Id12", "f_siauliai_46_4", Rectangle(1370, -723, 25));
	}
}
