//--- Melia Script -----------------------------------------------------------
// Main Chamber Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_cathedral_53'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DCathedral53MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_cathedral_53.Id1", MonsterId.Loftlem_Blue, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_53.Id2", MonsterId.Rootcrystal_03, min: 19, max: 25, respawn: Seconds(5), tendency: TendencyType.Peaceful);
		AddSpawner("d_cathedral_53.Id3", MonsterId.Loftlem_Blue, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_53.Id4", MonsterId.Anchor_Mage, min: 6, max: 7, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_53.Id5", MonsterId.Colifly, min: 4, max: 5, tendency: TendencyType.Peaceful);
		AddSpawner("d_cathedral_53.Id6", MonsterId.Loftlem_Blue, min: 5, max: 6, tendency: TendencyType.Peaceful);
		AddSpawner("d_cathedral_53.Id7", MonsterId.Anchor_Mage, min: 9, max: 11, tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Loftlem_Blue' GenType 3 Spawn Points
		AddSpawnPoint("d_cathedral_53.Id1", "d_cathedral_53", Rectangle(-58, 195, 9999));

		// 'Rootcrystal_03' GenType 24 Spawn Points
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-8, -868, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-121, -626, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-358, -334, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-583, -126, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-883, -307, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-928, 178, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-1055, -41, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-1383, -72, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-1426, -260, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-1441, 226, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-1699, 23, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-1738, -322, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-1983, 66, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(-320, 345, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(82, 120, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(360, 55, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(16, 699, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(309, 820, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(710, 273, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(853, -34, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(1067, 264, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(895, -360, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(925, -804, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(936, -1063, 10));
		AddSpawnPoint("d_cathedral_53.Id2", "d_cathedral_53", Rectangle(867, -1290, 10));

		// 'Loftlem_Blue' GenType 48 Spawn Points
		AddSpawnPoint("d_cathedral_53.Id3", "d_cathedral_53", Rectangle(48, 542, 30));
		AddSpawnPoint("d_cathedral_53.Id3", "d_cathedral_53", Rectangle(-51, 304, 30));
		AddSpawnPoint("d_cathedral_53.Id3", "d_cathedral_53", Rectangle(-286, 430, 30));
		AddSpawnPoint("d_cathedral_53.Id3", "d_cathedral_53", Rectangle(-270, 729, 30));
		AddSpawnPoint("d_cathedral_53.Id3", "d_cathedral_53", Rectangle(70, 799, 30));
		AddSpawnPoint("d_cathedral_53.Id3", "d_cathedral_53", Rectangle(292, 683, 30));
		AddSpawnPoint("d_cathedral_53.Id3", "d_cathedral_53", Rectangle(328, 345, 30));
		AddSpawnPoint("d_cathedral_53.Id3", "d_cathedral_53", Rectangle(-97, 980, 30));

		// 'Anchor_Mage' GenType 53 Spawn Points
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1730, -54, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1566, -251, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1549, -42, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1514, 197, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1161, 0, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1244, -119, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1205, -306, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1045, 197, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1842, -260, 30));
		AddSpawnPoint("d_cathedral_53.Id4", "d_cathedral_53", Rectangle(-1802, 204, 30));

		// 'Colifly' GenType 55 Spawn Points
		AddSpawnPoint("d_cathedral_53.Id5", "d_cathedral_53", Rectangle(906, -286, 30));
		AddSpawnPoint("d_cathedral_53.Id5", "d_cathedral_53", Rectangle(705, -282, 30));
		AddSpawnPoint("d_cathedral_53.Id5", "d_cathedral_53", Rectangle(641, 108, 30));
		AddSpawnPoint("d_cathedral_53.Id5", "d_cathedral_53", Rectangle(978, 122, 30));
		AddSpawnPoint("d_cathedral_53.Id5", "d_cathedral_53", Rectangle(819, -72, 30));
		AddSpawnPoint("d_cathedral_53.Id5", "d_cathedral_53", Rectangle(1063, -118, 30));
		AddSpawnPoint("d_cathedral_53.Id5", "d_cathedral_53", Rectangle(919, -483, 30));

		// 'Loftlem_Blue' GenType 56 Spawn Points
		AddSpawnPoint("d_cathedral_53.Id6", "d_cathedral_53", Rectangle(930, -748, 25));
		AddSpawnPoint("d_cathedral_53.Id6", "d_cathedral_53", Rectangle(861, -1036, 25));
		AddSpawnPoint("d_cathedral_53.Id6", "d_cathedral_53", Rectangle(917, -895, 25));
		AddSpawnPoint("d_cathedral_53.Id6", "d_cathedral_53", Rectangle(902, -542, 25));

		// 'Anchor_Mage' GenType 59 Spawn Points
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1432, -304, 30));
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1265, -292, 30));
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1358, -24, 30));
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1935, -24, 30));
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1664, 214, 30));
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1378, 260, 30));
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1680, -291, 30));
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1032, -287, 30));
		AddSpawnPoint("d_cathedral_53.Id7", "d_cathedral_53", Rectangle(-1104, 3, 30));
	}
}
