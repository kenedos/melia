//--- Melia Script -----------------------------------------------------------
// Mochia Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_pilgrimroad_31_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad313MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_pilgrimroad_31_3.Id1", MonsterId.Sec_Popolion_Orange, min: 45, max: 60);
		AddSpawner("f_pilgrimroad_31_3.Id2", MonsterId.Sec_Popolion_Orange, min: 15, max: 20);
		AddSpawner("f_pilgrimroad_31_3.Id3", MonsterId.Sec_Spion_Mage, min: 23, max: 30);
		AddSpawner("f_pilgrimroad_31_3.Id4", MonsterId.Sec_Templeslave_Mage, min: 27, max: 35);
		AddSpawner("f_pilgrimroad_31_3.Id5", MonsterId.Rootcrystal_01, min: 6, max: 8, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Sec_Popolion_Orange' GenType 22 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_3.Id1", "f_pilgrimroad_31_3", Rectangle(-242, -474, 9999));

		// 'Sec_Popolion_Orange' GenType 23 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1086, 1184, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1408, 855, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1420, 1489, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1297, 1278, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1065, 1525, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1341, 1627, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(951, 1481, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1470, 1362, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1151, 1304, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1395, 1079, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1564, 1232, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1222, 1545, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1230, 1018, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id2", "f_pilgrimroad_31_3", Rectangle(1606, 1048, 30));

		// 'Sec_Spion_Mage' GenType 24 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-1227, 0, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-1310, 285, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-1100, 394, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-1177, 95, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-1003, -41, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-989, 244, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-845, 269, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-822, -45, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-1091, 198, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(-1214, 387, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(396, -112, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(624, 150, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(855, 139, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(818, -117, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(1287, 362, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(1383, 550, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(1481, 406, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(1569, 224, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id3", "f_pilgrimroad_31_3", Rectangle(1694, 490, 30));

		// 'Sec_Templeslave_Mage' GenType 25 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(707, -1499, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(848, -1510, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(699, -1668, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(802, -1760, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(937, -1681, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(1023, -1500, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(945, -1439, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(814, -1621, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(-280, -1711, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(-139, -1683, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(-130, -1470, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(-94, -1586, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(115, -1673, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(209, -1642, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(-221, -1612, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(19, -1657, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(55, -1552, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(524, 66, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(603, -73, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(787, 126, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(696, 38, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(1436, 354, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(1285, 417, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(1483, 603, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(1650, 488, 30));
		AddSpawnPoint("f_pilgrimroad_31_3.Id4", "f_pilgrimroad_31_3", Rectangle(1366, 657, 30));

		// 'Rootcrystal_01' GenType 26 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_3.Id5", "f_pilgrimroad_31_3", Rectangle(-866, -1624, 150));
		AddSpawnPoint("f_pilgrimroad_31_3.Id5", "f_pilgrimroad_31_3", Rectangle(-478, -706, 150));
		AddSpawnPoint("f_pilgrimroad_31_3.Id5", "f_pilgrimroad_31_3", Rectangle(-1117, 330, 150));
		AddSpawnPoint("f_pilgrimroad_31_3.Id5", "f_pilgrimroad_31_3", Rectangle(-49, -1640, 150));
		AddSpawnPoint("f_pilgrimroad_31_3.Id5", "f_pilgrimroad_31_3", Rectangle(974, -1552, 150));
		AddSpawnPoint("f_pilgrimroad_31_3.Id5", "f_pilgrimroad_31_3", Rectangle(783, 44, 150));
		AddSpawnPoint("f_pilgrimroad_31_3.Id5", "f_pilgrimroad_31_3", Rectangle(1458, 373, 150));
		AddSpawnPoint("f_pilgrimroad_31_3.Id5", "f_pilgrimroad_31_3", Rectangle(1127, 1128, 150));
	}
}
