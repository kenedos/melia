//--- Melia Script -----------------------------------------------------------
// Gateway of the Great King Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_rokas_24'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas24MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_rokas_24.Id1", MonsterId.Rootcrystal_05, min: 12, max: 16, respawn: Seconds(5));
		AddSpawner("f_rokas_24.Id2", MonsterId.Tontus, min: 24, max: 38);
		AddSpawner("f_rokas_24.Id3", MonsterId.Dandel, min: 27, max: 35);
		AddSpawner("f_rokas_24.Id4", MonsterId.Tontus, min: 16, max: 20);
		AddSpawner("f_rokas_24.Id5", MonsterId.Pino, min: 9, max: 12);
		AddSpawner("f_rokas_24.Id6", MonsterId.Geppetto, min: 15, max: 20);
		AddSpawner("f_rokas_24.Id7", MonsterId.Dandel, min: 6, max: 7);
		AddSpawner("f_rokas_24.Id8", MonsterId.Tontus, min: 12, max: 15);
		AddSpawner("f_rokas_24.Id9", MonsterId.Dandel, min: 8, max: 10);

		AddSpawner("f_rokas_24.Id10", MonsterId.Hogma_Warrior, min: 30, max: 50);
		AddSpawner("f_rokas_24.Id11", MonsterId.Hogma_Combat, min: 30, max: 50);

		AddSpawner("f_rokas_24.Id12", MonsterId.Cockatries, min: 40, max: 70);
		AddSpawner("f_rokas_24.Id13", MonsterId.Big_Cockatries_Red, min: 18, max: 30);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' GenType 600 Spawn Points
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-424, -3381, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-1389, -1593, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-150, -39, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-2093, -17, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(685, -935, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(963, 72, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-178, 1294, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-457, -2061, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-368, -2650, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(219, -2952, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(747, -2240, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(1653, -106, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(1138, 750, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(791, 728, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-453, 89, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-831, -27, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-1209, -530, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-1817, -946, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(-934, -2266, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(481, 450, 30));
		AddSpawnPoint("f_rokas_24.Id1", "f_rokas_24", Rectangle(296, -1911, 30));

		// 'Tontus' GenType 627 Spawn Points
		AddSpawnPoint("f_rokas_24.Id2", "f_rokas_24", Rectangle(-716, -217, 9999));

		// 'Dandel' GenType 628 Spawn Points
		AddSpawnPoint("f_rokas_24.Id3", "f_rokas_24", Rectangle(-48, 1338, 9999));

		// 'Tontus' GenType 631 Spawn Points
		AddSpawnPoint("f_rokas_24.Id4", "f_rokas_24", Rectangle(-213, -612, 9999));

		// 'Pino' GenType 632 Spawn Points
		AddSpawnPoint("f_rokas_24.Id5", "f_rokas_24", Rectangle(-230, -1756, 9999));

		// 'Geppetto' GenType 633 Spawn Points
		AddSpawnPoint("f_rokas_24.Id6", "f_rokas_24", Rectangle(-210, -1973, 9999));

		// 'Dandel' GenType 634 Spawn Points
		AddSpawnPoint("f_rokas_24.Id7", "f_rokas_24", Rectangle(-459, -1904, 9999));

		// 'Tontus' GenType 701 Spawn Points
		AddSpawnPoint("f_rokas_24.Id8", "f_rokas_24", Rectangle(-118, -66, 9999));

		// 'Dandel' GenType 706 Spawn Points
		AddSpawnPoint("f_rokas_24.Id9", "f_rokas_24", Rectangle(722, -591, 9999));

		// 'Hogma_Warrior' Spawn Points
		AddSpawnPoint("f_rokas_24.Id10", "f_rokas_24", Rectangle(-585, -59, 700));
		AddSpawnPoint("f_rokas_24.Id10", "f_rokas_24", Rectangle(-2037, -142, 700));
		AddSpawnPoint("f_rokas_24.Id10", "f_rokas_24", Rectangle(-1444, -1348, 300));
		AddSpawnPoint("f_rokas_24.Id10", "f_rokas_24", Rectangle(-88, -113, 300));
		AddSpawnPoint("f_rokas_24.Id10", "f_rokas_24", Rectangle(-248, -2037, 700));
		AddSpawnPoint("f_rokas_24.Id10", "f_rokas_24", Rectangle(-435, -2686, 700));
		AddSpawnPoint("f_rokas_24.Id10", "f_rokas_24", Rectangle(-764, -3542, 200));
		AddSpawnPoint("f_rokas_24.Id10", "f_rokas_24", Rectangle(-428, -3342, 200));

		// 'Hogma_Combat' Spawn Points
		AddSpawnPoint("f_rokas_24.Id11", "f_rokas_24", Rectangle(-585, -59, 700));
		AddSpawnPoint("f_rokas_24.Id11", "f_rokas_24", Rectangle(-2037, -142, 700));
		AddSpawnPoint("f_rokas_24.Id11", "f_rokas_24", Rectangle(-1444, -1348, 300));
		AddSpawnPoint("f_rokas_24.Id11", "f_rokas_24", Rectangle(-88, -113, 300));
		AddSpawnPoint("f_rokas_24.Id11", "f_rokas_24", Rectangle(-248, -2037, 700));
		AddSpawnPoint("f_rokas_24.Id11", "f_rokas_24", Rectangle(-435, -2686, 700));
		AddSpawnPoint("f_rokas_24.Id11", "f_rokas_24", Rectangle(-764, -3542, 200));
		AddSpawnPoint("f_rokas_24.Id11", "f_rokas_24", Rectangle(-428, -3342, 200));

		// 'Cockatries' Spawn Points
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(883, -722, 700));
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(863, 608, 700));
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(402, 1095, 300));
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(-158, 1320, 300));
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(-857, 1594, 300));
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(-172, 2198, 300));
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(1684, -299, 300));
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(1091, 622, 300));
		AddSpawnPoint("f_rokas_24.Id12", "f_rokas_24", Rectangle(734, 712, 300));

		// 'Big_Cockatries_Red' Spawn Points
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(883, -722, 700));
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(863, 608, 700));
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(402, 1095, 300));
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(-158, 1320, 300));
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(-857, 1594, 300));
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(-172, 2198, 300));
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(1684, -299, 300));
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(1091, 622, 300));
		AddSpawnPoint("f_rokas_24.Id13", "f_rokas_24", Rectangle(734, 712, 300));

	}
}
