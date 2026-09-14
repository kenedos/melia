//--- Melia Script -----------------------------------------------------------
// Spring Light Woods Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_siauliai_46_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai461MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_siauliai_46_1.Id1", MonsterId.Infro_Blud, min: 8, max: 10);
		AddSpawner("f_siauliai_46_1.Id2", MonsterId.Shardstatue, min: 9, max: 12);
		AddSpawner("f_siauliai_46_1.Id3", MonsterId.Shardstatue, min: 8, max: 10);
		AddSpawner("f_siauliai_46_1.Id4", MonsterId.Siaulav, min: 8, max: 10);
		AddSpawner("f_siauliai_46_1.Id5", MonsterId.Rootcrystal_01, min: 10, max: 13, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Infro_Blud' GenType 18 Spawn Points
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-618, -1133, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-715, -163, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-572, -851, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-441, -448, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-744, -525, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-845, 518, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-602, 507, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-199, 88, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-84, 556, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(138, 613, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-257, 371, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-579, -1342, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-439, -1305, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-412, -1423, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-282, -1331, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-76, -914, 25));
		AddSpawnPoint("f_siauliai_46_1.Id1", "f_siauliai_46_1", Rectangle(-202, -873, 25));

		// 'Shardstatue' GenType 19 Spawn Points
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(770, -59, 25));
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(528, 452, 25));
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(823, -253, 25));
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(1088, 546, 25));
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(850, 586, 25));
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(630, 726, 25));
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(950, 360, 25));
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(1035, -235, 25));
		AddSpawnPoint("f_siauliai_46_1.Id2", "f_siauliai_46_1", Rectangle(774, 414, 25));

		// 'Shardstatue' GenType 20 Spawn Points
		AddSpawnPoint("f_siauliai_46_1.Id3", "f_siauliai_46_1", Rectangle(-77, 48, 9999));

		// 'Siaulav' GenType 21 Spawn Points
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(206, 47, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(220, 305, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(304, -369, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(484, -261, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(346, -813, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(685, -861, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(356, -1040, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(482, -884, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(418, -653, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(575, -745, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(197, -899, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(-60, -990, 20));
		AddSpawnPoint("f_siauliai_46_1.Id4", "f_siauliai_46_1", Rectangle(-200, -994, 20));

		// 'Rootcrystal_01' GenType 22 Spawn Points
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(-417, -1358, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(-1823, 195, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(-571, -453, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(-766, 541, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(-233, 361, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(81, 618, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(286, 180, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(307, -365, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(437, -866, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(1024, -273, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(588, 497, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(756, 1104, 150));
		AddSpawnPoint("f_siauliai_46_1.Id5", "f_siauliai_46_1", Rectangle(1037, 511, 150));
	}
}
