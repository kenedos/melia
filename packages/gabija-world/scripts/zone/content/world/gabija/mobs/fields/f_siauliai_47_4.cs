//--- Melia Script -----------------------------------------------------------
// Baron Allerno  Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_siauliai_47_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai474MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_siauliai_47_4.Id1", MonsterId.Haming_Orange, min: 12, max: 15);
		AddSpawner("f_siauliai_47_4.Id2", MonsterId.Popolion_Orange, min: 12, max: 15);
		AddSpawner("f_siauliai_47_4.Id3", MonsterId.Popolion_Orange, min: 12, max: 15);
		AddSpawner("f_siauliai_47_4.Id4", MonsterId.Popolion_Orange, min: 4, max: 5);
		AddSpawner("f_siauliai_47_4.Id5", MonsterId.Rootcrystal_01, min: 20, max: 26, respawn: Minutes(1));
		AddSpawner("f_siauliai_47_4.Id6", MonsterId.Spion_Mage, min: 8, max: 10);

		// Monster Spawn Points -----------------------------

		// 'Haming_Orange' GenType 4 Spawn Points
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(1751, -254, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(1634, -164, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(1568, -418, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(1922, -403, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(-532, -1445, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(-372, -1329, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(1098, -925, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(1108, -750, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(685, -283, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(375, -218, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(384, -448, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(433, 6, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(985, -656, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(1358, -898, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(-457, -1151, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(711, -49, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(153, -361, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(-565, -1260, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(207, -143, 40));
		AddSpawnPoint("f_siauliai_47_4.Id1", "f_siauliai_47_4", Rectangle(-599, -1566, 40));

		// 'Popolion_Orange' GenType 5 Spawn Points
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(330, -347, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(616, -129, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(234, -209, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(-981, -208, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(-910, -4, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(-657, -125, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(-210, 1012, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(134, 1001, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(9, 867, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(-472, 708, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(-363, 487, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(1305, 359, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(1129, 386, 25));
		AddSpawnPoint("f_siauliai_47_4.Id2", "f_siauliai_47_4", Rectangle(620, -347, 25));

		// 'Popolion_Orange' GenType 6 Spawn Points
		AddSpawnPoint("f_siauliai_47_4.Id3", "f_siauliai_47_4", Rectangle(52, 991, 1500));

		// 'Popolion_Orange' GenType 23 Spawn Points
		AddSpawnPoint("f_siauliai_47_4.Id4", "f_siauliai_47_4", Rectangle(-519, -1442, 40));
		AddSpawnPoint("f_siauliai_47_4.Id4", "f_siauliai_47_4", Rectangle(-423, -1139, 40));
		AddSpawnPoint("f_siauliai_47_4.Id4", "f_siauliai_47_4", Rectangle(-186, -1285, 40));
		AddSpawnPoint("f_siauliai_47_4.Id4", "f_siauliai_47_4", Rectangle(-629, -1268, 40));

		// 'Rootcrystal_01' GenType 24 Spawn Points
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-558, -1247, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(203, -1247, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(638, -1281, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(1077, -990, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(1157, -591, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(1611, -473, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(2077, -255, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(2384, -823, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(1637, -85, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(1423, 366, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(1083, 388, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(615, 53, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(496, -327, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(198, -31, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(179, 964, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-90, 966, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(617, 1386, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(1298, 1091, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(1308, 842, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-523, 677, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-363, 429, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-846, 58, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-1005, -345, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-1062, -853, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-1557, 83, 10));
		AddSpawnPoint("f_siauliai_47_4.Id5", "f_siauliai_47_4", Rectangle(-1966, -143, 10));

		// 'Spion_Mage' GenType 25 Spawn Points
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(1429, -310, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(1863, -227, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(1715, -448, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(1029, -606, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(1225, -880, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(2360, -999, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(2454, -772, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(2638, -960, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(-491, -1456, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(-505, -1152, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(-278, -1301, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(418, -309, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(661, -211, 40));
		AddSpawnPoint("f_siauliai_47_4.Id6", "f_siauliai_47_4", Rectangle(230, -114, 40));
	}
}
