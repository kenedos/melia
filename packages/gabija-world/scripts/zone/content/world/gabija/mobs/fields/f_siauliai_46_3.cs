//--- Melia Script -----------------------------------------------------------
// Vilna Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_siauliai_46_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai463MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_siauliai_46_3.Id1", MonsterId.Chupaluka, min: 12, max: 15, respawn: Seconds(20));
		AddSpawner("f_siauliai_46_3.Id2", MonsterId.Spion, min: 12, max: 15, respawn: Seconds(200));
		AddSpawner("f_siauliai_46_3.Id3", MonsterId.Siaulago, min: 15, max: 20, respawn: Seconds(30));
		AddSpawner("f_siauliai_46_3.Id4", MonsterId.Honeymeli, min: 3, max: 4, respawn: Seconds(30));
		AddSpawner("f_siauliai_46_3.Id5", MonsterId.Rootcrystal_01, min: 11, max: 14, respawn: Seconds(30));
		AddSpawner("f_siauliai_46_3.Id6", MonsterId.Spion, min: 15, max: 20, respawn: Seconds(20));
		AddSpawner("f_siauliai_46_3.Id7", MonsterId.Siaulago, min: 8, max: 10, respawn: Seconds(20));
		AddSpawner("f_siauliai_46_3.Id8", MonsterId.Honeymeli, min: 3, max: 4, respawn: Minutes(1));
		AddSpawner("f_siauliai_46_3.Id9", MonsterId.Chupaluka, min: 14, max: 18, respawn: Minutes(1));

		// Monster Spawn Points -----------------------------

		// 'Chupaluka' GenType 4 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(1052, 838, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(1264, 1015, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(1023, 1018, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(1450, 913, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(393, 1493, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(670, 1476, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(447, 1061, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(353, 891, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(192, 1138, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(46, 1007, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(893, 960, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(914, 1626, 30));
		AddSpawnPoint("f_siauliai_46_3.Id1", "f_siauliai_46_3", Rectangle(1166, 1548, 30));

		// 'Spion' GenType 6 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id2", "f_siauliai_46_3", Rectangle(1190, 1054, 9999));

		// 'Siaulago' GenType 18 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-590, -230, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-870, -301, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-884, -628, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-1825, -1412, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-508, -662, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-435, -403, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-1656, -461, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-1978, -1085, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-1518, -1399, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-1766, -643, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-1718, -1320, 40));
		AddSpawnPoint("f_siauliai_46_3.Id3", "f_siauliai_46_3", Rectangle(-693, -461, 40));

		// 'Honeymeli' GenType 19 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id4", "f_siauliai_46_3", Rectangle(-403, 1513, 1500));

		// 'Rootcrystal_01' GenType 20 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(-1864, -1447, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(-1701, -629, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(-840, -612, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(-524, -192, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(-1224, 1062, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(-388, 1559, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(-576, 996, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(451, 1010, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(691, 1578, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(1591, 1762, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(1002, 953, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(2287, -207, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(3345, 1020, 200));
		AddSpawnPoint("f_siauliai_46_3.Id5", "f_siauliai_46_3", Rectangle(358, -911, 200));

		// 'Spion' GenType 22 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id6", "f_siauliai_46_3", Rectangle(182, -882, 20));
		AddSpawnPoint("f_siauliai_46_3.Id6", "f_siauliai_46_3", Rectangle(391, -897, 20));
		AddSpawnPoint("f_siauliai_46_3.Id6", "f_siauliai_46_3", Rectangle(433, -772, 20));
		AddSpawnPoint("f_siauliai_46_3.Id6", "f_siauliai_46_3", Rectangle(425, -614, 20));
		AddSpawnPoint("f_siauliai_46_3.Id6", "f_siauliai_46_3", Rectangle(330, -978, 20));
		AddSpawnPoint("f_siauliai_46_3.Id6", "f_siauliai_46_3", Rectangle(520, -882, 20));

		// 'Siaulago' GenType 24 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id7", "f_siauliai_46_3", Rectangle(28, -426, 25));
		AddSpawnPoint("f_siauliai_46_3.Id7", "f_siauliai_46_3", Rectangle(62, -543, 25));
		AddSpawnPoint("f_siauliai_46_3.Id7", "f_siauliai_46_3", Rectangle(-77, -501, 25));
		AddSpawnPoint("f_siauliai_46_3.Id7", "f_siauliai_46_3", Rectangle(109, -886, 25));
		AddSpawnPoint("f_siauliai_46_3.Id7", "f_siauliai_46_3", Rectangle(285, -823, 25));
		AddSpawnPoint("f_siauliai_46_3.Id7", "f_siauliai_46_3", Rectangle(259, -978, 25));

		// 'Honeymeli' GenType 25 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id8", "f_siauliai_46_3", Rectangle(-608, -747, 25));
		AddSpawnPoint("f_siauliai_46_3.Id8", "f_siauliai_46_3", Rectangle(-783, -765, 25));
		AddSpawnPoint("f_siauliai_46_3.Id8", "f_siauliai_46_3", Rectangle(-847, -334, 25));
		AddSpawnPoint("f_siauliai_46_3.Id8", "f_siauliai_46_3", Rectangle(-443, -281, 25));

		// 'Chupaluka' GenType 26 Spawn Points
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1762, 1575, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1771, 1747, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1589, 1798, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1472, 1855, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1488, 1543, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1700, 1497, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1537, 1486, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1648, 1578, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1591, 1913, 25));
		AddSpawnPoint("f_siauliai_46_3.Id9", "f_siauliai_46_3", Rectangle(1413, 1720, 25));
	}
}
