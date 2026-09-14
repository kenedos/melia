//--- Melia Script -----------------------------------------------------------
// Saknis Plains Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_katyn_14'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn14MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_katyn_14.Id1", MonsterId.Puragi_Blue, min: 15, max: 20);
		AddSpawner("f_katyn_14.Id2", MonsterId.Honey_Bee, min: 15, max: 20);
		AddSpawner("f_katyn_14.Id3", MonsterId.Fisherman_Red, min: 23, max: 30);
		AddSpawner("f_katyn_14.Id4", MonsterId.Mushroom_Ent_Blue, min: 6, max: 7);
		AddSpawner("f_katyn_14.Id5", MonsterId.Rootcrystal_02, min: 15, max: 20, respawn: Seconds(5));
		AddSpawner("f_katyn_14.Id6", MonsterId.Fisherman_Red, min: 12, max: 15);
		AddSpawner("f_katyn_14.Id7", MonsterId.Honey_Bee, min: 12, max: 15);

		// Monster Spawn Points -----------------------------

		// 'Puragi_Blue' GenType 500 Spawn Points
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2192, -1111, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(1950, -1099, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(1993, -1743, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2410, -2023, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2160, -1986, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(1938, -750, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2363, -530, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2287, 147, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2331, -276, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2136, -899, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2031, -645, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(961, -373, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(951, -1707, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(1132, -1629, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(1070, -544, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(1050, -272, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(931, 115, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(952, -1322, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(1088, -1440, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(804, 167, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(556, -1193, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(1308, -1613, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2114, 283, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2148, 396, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2845, -2205, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(3008, -2159, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2912, -1984, 30));
		AddSpawnPoint("f_katyn_14.Id1", "f_katyn_14", Rectangle(2797, -2032, 30));

		// 'Honey_Bee' GenType 501 Spawn Points
		AddSpawnPoint("f_katyn_14.Id2", "f_katyn_14", Rectangle(-309, -575, 9999));

		// 'Fisherman_Red' GenType 502 Spawn Points
		AddSpawnPoint("f_katyn_14.Id3", "f_katyn_14", Rectangle(-1810, -1158, 9999));

		// 'Mushroom_Ent_Blue' GenType 503 Spawn Points
		AddSpawnPoint("f_katyn_14.Id4", "f_katyn_14", Rectangle(-1849, 566, 9999));

		// 'Rootcrystal_02' GenType 514 Spawn Points
		AddSpawnPoint("f_katyn_14.Id5", "f_katyn_14", Rectangle(1249, -979, 30));
		AddSpawnPoint("f_katyn_14.Id5", "f_katyn_14", Rectangle(-143, -681, 30));
		AddSpawnPoint("f_katyn_14.Id5", "f_katyn_14", Rectangle(-2065, -1136, 30));
		AddSpawnPoint("f_katyn_14.Id5", "f_katyn_14", Rectangle(-1749, 735, 30));

		// 'Fisherman_Red' GenType 599 Spawn Points
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-304, 529, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-395, 147, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-494, 312, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-759, 360, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-1351, 734, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-1359, 1097, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-1656, 754, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-1870, 1100, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-2549, 517, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-2891, 608, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-2663, 795, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-2930, 1346, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-2579, 1623, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-1765, 565, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-1794, -1379, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-1808, -1126, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-2231, -1181, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-2964, -1343, 30));
		AddSpawnPoint("f_katyn_14.Id6", "f_katyn_14", Rectangle(-1051, -1156, 30));

		// 'Honey_Bee' GenType 600 Spawn Points
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-2148, -1208, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1845, -1176, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1855, -1380, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1790, -971, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1426, -1186, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1147, -1129, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-2480, -1239, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-2911, -1414, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-2929, -1208, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-346, 76, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-305, 485, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-871, 292, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1293, 647, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1274, 1025, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1548, 672, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-1837, 1089, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-2652, 664, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-2543, 416, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-2597, 1540, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-3235, 1390, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-3528, 1308, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-2749, 1317, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-584, -323, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-683, -487, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-481, -789, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-673, -938, 30));
		AddSpawnPoint("f_katyn_14.Id7", "f_katyn_14", Rectangle(-484, -563, 30));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Werewolf, "f_katyn_14", 1, Hours(6), Hours(12));
	}
}
