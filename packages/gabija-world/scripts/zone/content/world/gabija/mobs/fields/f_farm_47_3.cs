//--- Melia Script -----------------------------------------------------------
// Myrkiti Farm Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_farm_47_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm473MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_farm_47_3.Id1", MonsterId.Kepo_Seed_Violet, min: 15, max: 20);
		AddSpawner("f_farm_47_3.Id2", MonsterId.Ellom_Violet, min: 15, max: 20);
		AddSpawner("f_farm_47_3.Id3", MonsterId.Kepo_Seed_Violet, min: 15, max: 20);
		AddSpawner("f_farm_47_3.Id4", MonsterId.Rootcrystal_01, min: 23, max: 30, respawn: Minutes(1));
		AddSpawner("f_farm_47_3.Id5", MonsterId.Cronewt_Bow, min: 15, max: 20);
		AddSpawner("f_farm_47_3.Id6", MonsterId.Operor_White, min: 19, max: 25);

		// Monster Spawn Points -----------------------------

		// 'Kepo_Seed_Violet' GenType 3 Spawn Points
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-605, -402, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-626, -275, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-694, -399, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-1328, -476, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-1264, -326, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-1114, -406, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-1187, -365, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-253, -824, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-13, -589, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-258, -610, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-150, -713, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-1084, -279, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-466, -330, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-627, 1, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-590, 167, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-491, 38, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-193, 52, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-145, 105, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-12, -27, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-86, -59, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-77, 18, 25));
		AddSpawnPoint("f_farm_47_3.Id1", "f_farm_47_3", Rectangle(-1293, -395, 25));

		// 'Ellom_Violet' GenType 4 Spawn Points
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-630, -278, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-608, -493, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-88, 144, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-610, 34, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-830, 514, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-614, 909, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1203, 576, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1748, 72, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1233, -467, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-803, -389, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-276, -668, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-447, 909, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-153, -28, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-47, -739, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-346, -776, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-193, -546, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-123, -824, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1236, 411, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-6, 95, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-648, 135, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1844, -20, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1409, 663, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1599, 802, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1356, 844, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-2033, 776, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-2161, 772, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1306, 458, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1985, 905, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-2107, 860, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1511, 702, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1423, 753, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1354, 680, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1175, 509, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-555, 820, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-412, 840, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-477, 731, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-777, -309, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-660, -354, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-546, -292, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1252, -392, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1378, -423, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1018, -336, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-936, -371, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1168, -225, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1896, -178, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1736, -281, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1599, -182, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1569, 19, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1645, 71, 25));
		AddSpawnPoint("f_farm_47_3.Id2", "f_farm_47_3", Rectangle(-1711, -127, 25));

		// 'Kepo_Seed_Violet' GenType 33 Spawn Points
		AddSpawnPoint("f_farm_47_3.Id3", "f_farm_47_3", Rectangle(-695, -468, 9999));

		// 'Rootcrystal_01' GenType 35 Spawn Points
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(810, 73, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(493, 173, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(241, 46, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-93, 114, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-675, 69, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-830, 517, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-688, 835, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1156, 572, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1478, 802, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-2058, 930, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-2209, 794, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1768, 421, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1744, -52, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1713, -308, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1356, -394, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-991, -338, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-612, -526, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-401, -723, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-15, -749, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1674, -520, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1654, -720, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1420, 463, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1181, -300, 10));
		AddSpawnPoint("f_farm_47_3.Id4", "f_farm_47_3", Rectangle(-1851, 287, 10));

		// 'Cronewt_Bow' GenType 37 Spawn Points
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-559, -372, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1734, -200, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1615, -52, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1736, 0, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1861, -86, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1207, -301, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1162, -431, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-713, -308, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-376, -637, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-84, -652, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-2064, 726, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-2098, 944, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1948, 806, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-2181, 871, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1823, -167, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1400, -373, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-2020, 868, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1524, 773, 30));
		AddSpawnPoint("f_farm_47_3.Id5", "f_farm_47_3", Rectangle(-1464, 840, 30));

		// 'Operor_White' GenType 38 Spawn Points
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1777, -103, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1821, -200, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1742, -15, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1254, 490, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-924, 494, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1125, 512, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-607, 785, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-532, 879, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-454, 793, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(377, -26, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(357, 250, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(498, 99, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(611, -73, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(311, 109, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(705, 107, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-527, 727, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1274, 549, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-744, -398, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-606, -319, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-600, -426, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-342, -683, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-168, -847, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(12, -660, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-157, -549, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-33, 81, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-162, 37, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1173, 418, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1649, -115, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1327, -313, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1105, -344, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1270, -517, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-1051, -437, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-535, -432, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-709, -235, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-841, 367, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-861, 535, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-722, 515, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-856, 181, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-815, 736, 30));
		AddSpawnPoint("f_farm_47_3.Id6", "f_farm_47_3", Rectangle(-755, 822, 30));
	}
}
