//--- Melia Script -----------------------------------------------------------
// Akmens Ridge Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_rokas_27'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas27MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_rokas_27.Id1", MonsterId.Rootcrystal_05, min: 12, max: 16, respawn: Seconds(20));
		AddSpawner("f_rokas_27.Id2", MonsterId.Sauga_S, min: 12, max: 15);
		AddSpawner("f_rokas_27.Id3", MonsterId.Tucen, min: 8, max: 10);
		AddSpawner("f_rokas_27.Id4", MonsterId.Tucen, min: 10, max: 13);
		AddSpawner("f_rokas_27.Id5", MonsterId.Loftlem, min: 8, max: 10);
		AddSpawner("f_rokas_27.Id6", MonsterId.Sauga_S, min: 15, max: 20);
		AddSpawner("f_rokas_27.Id7", MonsterId.Ticen, min: 15, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' GenType 600 Spawn Points
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(328, -2523, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(782, -2293, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(1110, -1049, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(1364, 108, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(1948, -134, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(317, -2173, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(-550, -1778, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(-86, -1210, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(-639, -2216, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(1165, 706, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(1725, -298, 30));
		AddSpawnPoint("f_rokas_27.Id1", "f_rokas_27", Rectangle(-470, -3090, 30));

		// 'Sauga_S' GenType 621 Spawn Points
		AddSpawnPoint("f_rokas_27.Id2", "f_rokas_27", Rectangle(1635, 467, 9999));

		// 'Tucen' GenType 624 Spawn Points
		AddSpawnPoint("f_rokas_27.Id3", "f_rokas_27", Rectangle(949, -618, 9999));

		// 'Tucen' GenType 627 Spawn Points
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(2295, -201, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(2113, -359, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(2212, 42, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(2336, -54, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(2126, -231, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(2242, -86, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(1940, -382, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(837, -734, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(1014, -548, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(1104, -699, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(-873, -2305, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(-715, -2155, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(67, -1787, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(213, -1751, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(146, -1692, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(961, -2000, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(986, -1897, 20));
		AddSpawnPoint("f_rokas_27.Id4", "f_rokas_27", Rectangle(987, -1382, 20));

		// 'Loftlem' GenType 629 Spawn Points
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1434, 403, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1413, 129, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1713, 338, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1485, 626, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1551, 227, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1294, 540, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1270, 270, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1172, 148, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1586, 431, 20));
		AddSpawnPoint("f_rokas_27.Id5", "f_rokas_27", Rectangle(1683, 588, 20));

		// 'Sauga_S' GenType 701 Spawn Points
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(457, -2283, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(-801, -2072, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(566, -2177, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(435, -2653, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(275, -2549, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(747, -2526, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(658, -2387, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(534, -2479, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(637, -2659, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(329, -2339, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(792, -749, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(1001, -721, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(898, -472, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(1061, -597, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(1088, -467, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(-870, -2286, 30));
		AddSpawnPoint("f_rokas_27.Id6", "f_rokas_27", Rectangle(-715, -2225, 30));

		// 'Ticen' GenType 704 Spawn Points
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(396, -2549, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(612, -2341, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(549, -2688, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(464, -2224, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(707, -2269, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(802, -2499, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(357, -2289, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(565, -2471, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(-128, -2695, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(92, -2781, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(191, -2521, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(1220, 342, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(1325, 554, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(1533, 655, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(1574, 389, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(1403, 267, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(-872, -2292, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(-697, -2140, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(-720, -2256, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(844, -712, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(1108, -632, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(938, -628, 30));
		AddSpawnPoint("f_rokas_27.Id7", "f_rokas_27", Rectangle(1073, -459, 30));
	}
}
