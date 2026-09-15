//--- Melia Script -----------------------------------------------------------
// Stele Road Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_remains_37'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains37MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_remains_37.Id1", MonsterId.Stub_Tree, min: 8, max: 10);
		AddSpawner("f_remains_37.Id2", MonsterId.Stub_Tree, min: 19, max: 25);
		AddSpawner("f_remains_37.Id3", MonsterId.Tama, min: 15, max: 20);
		AddSpawner("f_remains_37.Id4", MonsterId.TreeAmbulo, min: 8, max: 10);
		AddSpawner("f_remains_37.Id5", MonsterId.TreeAmbulo, min: 19, max: 25);
		AddSpawner("f_remains_37.Id6", MonsterId.Tama, min: 6, max: 8);
		AddSpawner("f_remains_37.Id7", MonsterId.TreeAmbulo, min: 8, max: 10);
		AddSpawner("f_remains_37.Id8", MonsterId.Rootcrystal_01, min: 4, max: 5, respawn: Minutes(1));

		// Monster Spawn Points -----------------------------

		// 'Stub_Tree' GenType 4 Spawn Points
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1428, 354, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1470, 721, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1384, 241, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1236, 389, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1270, 598, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1311, 708, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1609, 640, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1569, 444, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1545, 291, 25));
		AddSpawnPoint("f_remains_37.Id1", "f_remains_37", Rectangle(1446, 544, 25));

		// 'Stub_Tree' GenType 5 Spawn Points
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-735, -2267, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1241, -2312, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-973, -2347, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-821, -2455, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-512, -2372, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-946, -2141, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-800, -2045, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-659, -2470, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-429, -2170, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-362, -2323, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-1128, -2165, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-1364, -2181, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-608, -2110, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1283, -2190, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1078, -2264, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1179, -2415, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1311, -2528, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1415, -2338, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1408, -2242, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1377, -2455, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1330, -2331, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(1195, -2197, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-711, -2169, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(-796, -2327, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(154, -2271, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(207, -2227, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(794, -2242, 20));
		AddSpawnPoint("f_remains_37.Id2", "f_remains_37", Rectangle(877, -2215, 20));

		// 'Tama' GenType 9 Spawn Points
		AddSpawnPoint("f_remains_37.Id3", "f_remains_37", Rectangle(1458, 583, 9999));

		// 'TreeAmbulo' GenType 37 Spawn Points
		AddSpawnPoint("f_remains_37.Id4", "f_remains_37", Rectangle(479, 643, 9999));

		// 'TreeAmbulo' GenType 47 Spawn Points
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(593, -853, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(789, -871, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(795, -1160, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(729, -1246, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(558, -1380, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(370, -1243, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(396, -964, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(384, -1101, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(570, -964, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(430, -1333, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(657, -1291, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(723, -1118, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(740, -1012, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(699, -837, 100));
		AddSpawnPoint("f_remains_37.Id5", "f_remains_37", Rectangle(509, -880, 100));

		// 'Tama' GenType 48 Spawn Points
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(591, 664, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(1400, 613, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(602, 2777, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(369, 2639, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(365, 2797, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(676, 2574, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(506, 2680, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(633, 2896, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(822, 2770, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(803, 2641, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(884, 2486, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(816, 2962, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(971, 2695, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(187, 2457, 30));
		AddSpawnPoint("f_remains_37.Id6", "f_remains_37", Rectangle(328, 2454, 30));

		// 'TreeAmbulo' GenType 59 Spawn Points
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(508, 2767, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(354, 2697, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(888, 2584, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(563, 2918, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(734, 2905, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(769, 2741, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(765, 2568, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(459, 2545, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(197, 2481, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(615, 2606, 30));
		AddSpawnPoint("f_remains_37.Id7", "f_remains_37", Rectangle(921, 2715, 30));

		// 'Rootcrystal_01' GenType 60 Spawn Points
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(-1574, -2501, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(-1359, -2188, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(-854, -2172, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(-526, -2244, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(105, -2278, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(616, -2221, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(1064, -2241, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(1380, -2374, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(568, -1295, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(601, -891, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(815, -1062, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(530, -336, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(600, 478, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(376, 740, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(1164, 537, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(1472, 807, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(1562, 291, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(401, 1130, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(98, 1772, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(536, 1593, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(118, 2274, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(393, 2670, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(779, 2771, 250));
		AddSpawnPoint("f_remains_37.Id8", "f_remains_37", Rectangle(693, 2559, 250));
	}
}
