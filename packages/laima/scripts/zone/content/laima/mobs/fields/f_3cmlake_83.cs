//--- Melia Script -----------------------------------------------------------
// Pelke Shrine Ruins Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_3cmlake_83'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class F3Cmlake83MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_3cmlake_83.Id1", MonsterId.Rajatadpole, min: 38, max: 50);
		AddSpawner("f_3cmlake_83.Id2", MonsterId.Sec_Merog_Wogu, min: 23, max: 30);
		AddSpawner("f_3cmlake_83.Id3", MonsterId.Sec_Merog_Wizzard, min: 27, max: 35);
		AddSpawner("f_3cmlake_83.Id4", MonsterId.Rajatadpole, min: 23, max: 30);
		AddSpawner("f_3cmlake_83.Id5", MonsterId.Rootcrystal_01, min: 11, max: 14, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Rajatadpole' GenType 33 Spawn Points
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(509, -123, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(503, 19, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(634, 176, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(773, 171, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(492, 167, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(620, 26, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(653, -101, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(770, 37, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(356, 114, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-470, 128, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-929, 336, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-828, 90, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(431, 134, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-935, 179, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-836, 216, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1250, 232, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1029, 447, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-915, 914, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-671, 745, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1065, -512, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-867, -551, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-753, -375, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-705, -719, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1030, 27, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-800, 318, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-988, 276, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1093, 580, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1092, 833, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-642, 845, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-713, 793, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-677, 484, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-807, 427, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-934, 436, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-806, 643, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-806, 804, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-986, 832, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-980, 686, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-990, 603, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1598, 858, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1819, 1010, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1800, 1015, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1615, 1181, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1558, 1134, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1456, 928, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1728, 790, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1053, -353, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-801, -353, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-696, -582, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-771, -663, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-756, -774, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-664, -758, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-967, -489, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1417, 165, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1636, 187, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1682, 57, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1358, -88, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1601, -135, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1496, -512, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1626, -670, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1751, -495, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1422, -567, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1018, 160, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1341, -508, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1332, -576, 30));
		AddSpawnPoint("f_3cmlake_83.Id1", "f_3cmlake_83", Rectangle(-1424, -683, 30));

		// 'Sec_Merog_Wogu' GenType 34 Spawn Points
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-2190, -484, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1434, -149, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1512, 752, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1506, -758, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1463, 1009, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-975, -432, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-791, -474, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-871, -712, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-757, -212, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1574, 168, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1663, 826, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1606, -916, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1608, 1016, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1514, -536, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1549, 1098, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1592, 849, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1585, 1343, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1543, 858, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1519, -178, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1793, 927, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1637, 717, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1631, 136, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1557, 340, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1551, 230, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1426, 87, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1414, 330, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1614, 324, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1542, 75, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1640, 370, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1340, 396, 30));
		AddSpawnPoint("f_3cmlake_83.Id2", "f_3cmlake_83", Rectangle(-1376, -827, 30));

		// 'Sec_Merog_Wizzard' GenType 35 Spawn Points
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-394, 741, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-73, 724, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-1449, -513, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-1441, -434, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-2141, -503, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-1499, -744, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-982, 884, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-1636, -525, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-1480, -322, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-1378, -568, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-1995, -506, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-260, 854, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-858, 610, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-805, 884, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-989, 709, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-951, 630, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-798, 728, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-861, 877, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-210, 651, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-352, 864, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-389, 628, 30));
		AddSpawnPoint("f_3cmlake_83.Id3", "f_3cmlake_83", Rectangle(-389, 628, 30));

		// 'Rajatadpole' GenType 40 Spawn Points
		AddSpawnPoint("f_3cmlake_83.Id4", "f_3cmlake_83", Rectangle(-1469, 251, 9999));

		// 'Rootcrystal_01' GenType 41 Spawn Points
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(620, 162, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-909, 250, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-704, -358, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-1084, -665, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-1491, 188, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-1495, -372, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-1470, -888, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-2121, -445, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-1669, 747, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-1434, 995, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-1579, 1405, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-932, 644, 50));
		AddSpawnPoint("f_3cmlake_83.Id5", "f_3cmlake_83", Rectangle(-388, 772, 50));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Rocksodon, "f_3cmlake_83", 1, Hours(2), Hours(4));
		AddBossSpawner(MonsterId.Boss_Hydra, "f_3cmlake_83", 1, Hours(2), Hours(4));
	}
}
