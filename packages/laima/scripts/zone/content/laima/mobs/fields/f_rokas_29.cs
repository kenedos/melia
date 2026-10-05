//--- Melia Script -----------------------------------------------------------
// Rukas Plateau Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_rokas_29'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas29MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_rokas_29.Id1", MonsterId.Hogma_Warrior, min: 19, max: 25);
		AddSpawner("f_rokas_29.Id2", MonsterId.Rootcrystal_05, min: 12, max: 15, respawn: Seconds(5));
		AddSpawner("f_rokas_29.Id3", MonsterId.Hogma_Combat, min: 9, max: 12, respawn: Seconds(35));
		AddSpawner("f_rokas_29.Id4", MonsterId.Hogma_Combat, min: 25, max: 33, respawn: Seconds(25));
		AddSpawner("f_rokas_29.Id5", MonsterId.Hogma_Combat, min: 18, max: 23);
		AddSpawner("f_rokas_29.Id6", MonsterId.Hogma_Warrior, min: 19, max: 25, respawn: Seconds(25));
		AddSpawner("f_rokas_29.Id7", MonsterId.Woodfung, min: 12, max: 15);

		// Monster Spawn Points -----------------------------

		// 'Hogma_Warrior' GenType 57 Spawn Points
		AddSpawnPoint("f_rokas_29.Id1", "f_rokas_29", Rectangle(227, 618, 9999));

		// 'Rootcrystal_05' GenType 600 Spawn Points
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(2292, 107, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(1089, 275, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(-725, 516, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(-652, -1930, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(-960, 1896, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(1098, -691, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(2539, 802, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(208, 718, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(-402, -335, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(37, -303, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(-697, -892, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(-767, -1614, 30));
		AddSpawnPoint("f_rokas_29.Id2", "f_rokas_29", Rectangle(596, 1987, 30));

		// 'Hogma_Combat' GenType 634 Spawn Points
		AddSpawnPoint("f_rokas_29.Id3", "f_rokas_29", Rectangle(-835, 645, 400));
		AddSpawnPoint("f_rokas_29.Id3", "f_rokas_29", Rectangle(-385, -1549, 400));

		// 'Hogma_Combat' GenType 635 Spawn Points
		AddSpawnPoint("f_rokas_29.Id4", "f_rokas_29", Rectangle(-110, -292, 9999));

		// 'Hogma_Combat' GenType 642 Spawn Points
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(2178, 29, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(1092, 369, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(2248, 245, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(2354, 428, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(2467, 297, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(2435, 88, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(2282, -220, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(1761, 386, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(2035, 214, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(1321, 425, 50));
		AddSpawnPoint("f_rokas_29.Id5", "f_rokas_29", Rectangle(1540, 529, 50));

		// 'Hogma_Warrior' GenType 643 Spawn Points
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(2268, 67, 40));
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(2248, 259, 40));
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(1834, 333, 40));
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(1572, 582, 40));
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(1315, 473, 40));
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(2484, 221, 40));
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(2251, -128, 40));
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(2170, 165, 40));
		AddSpawnPoint("f_rokas_29.Id6", "f_rokas_29", Rectangle(2301, 455, 40));

		// 'Woodfung' GenType 644 Spawn Points
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-727, -427, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-122, -292, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-2, -121, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-475, -382, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-1063, 547, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-931, 741, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-719, 514, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-1748, 837, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-1788, 1230, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-1660, 1037, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-1071, 1902, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(-922, 1960, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(392, 1773, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(443, 2094, 40));
		AddSpawnPoint("f_rokas_29.Id7", "f_rokas_29", Rectangle(232, 1948, 40));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Rajatoad, "f_rokas_29", 1, Hours(2), Hours(4));
	}
}
