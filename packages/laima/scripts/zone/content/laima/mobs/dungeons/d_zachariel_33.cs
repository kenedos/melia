//--- Melia Script -----------------------------------------------------------
// Royal Mausoleum 2F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_zachariel_33'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel33MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------

		// Spawn Buffs -------------------------------------
		AddSpawnBuff("d_zachariel_33", MonsterId.Beetle, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_33", MonsterId.Vesper, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_33", MonsterId.Wolf_Statue, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_33", MonsterId.Tombsinker, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_33", MonsterId.Beetle_Elite, BuffId.EliteMonsterBuff, chance: 100);

		// Monster Spawners ---------------------------------

		AddSpawner("d_zachariel_33.Id1", MonsterId.Rootcrystal_05, min: 18, max: 24, respawn: Seconds(15), tendency: TendencyType.Peaceful);
		AddSpawner("d_zachariel_33.Id2", MonsterId.Beetle_Elite, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 1 (-143, -1965)
		AddSpawner("d_zachariel_33.Id3", MonsterId.Vesper, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_33.Id4", MonsterId.Tombsinker, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 2 (-199, -730)
		AddSpawner("d_zachariel_33.Id5", MonsterId.Beetle, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_33.Id6", MonsterId.Wolf_Statue, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 3 (869, -631)
		AddSpawner("d_zachariel_33.Id7", MonsterId.Vesper, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_33.Id8", MonsterId.Beetle, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 4 (-119, 210)
		AddSpawner("d_zachariel_33.Id9", MonsterId.Wolf_Statue, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_33.Id10", MonsterId.Tombsinker, min: 2, max: 2, tendency: TendencyType.Aggressive);

		// Room 5 (69, 1606)
		AddSpawner("d_zachariel_33.Id11", MonsterId.Beetle, min: 2, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_33.Id12", MonsterId.Vesper, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Room 6 (-1538, 253)
		AddSpawner("d_zachariel_33.Id13", MonsterId.Tombsinker, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_33.Id14", MonsterId.Wolf_Statue, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 7 (-1533, -804)
		AddSpawner("d_zachariel_33.Id15", MonsterId.Beetle, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_33.Id16", MonsterId.Tombsinker, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Corridor 1 (-146, -1317)
		AddSpawner("d_zachariel_33.Id17", MonsterId.Wolf_Statue, min: 1, max: 1, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Corridor 2 (-197, -198)
		AddSpawner("d_zachariel_33.Id18", MonsterId.Tombsinker, min: 1, max: 2, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Corridor 3 (44, 787)
		AddSpawner("d_zachariel_33.Id19", MonsterId.Beetle, min: 1, max: 1, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Corridor 4 (-823, 242)
		AddSpawner("d_zachariel_33.Id20", MonsterId.Vesper, min: 1, max: 2, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' Spawn Points
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-192, -551, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-133, -1213, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-95, -2356, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-143, -1979, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(188, -675, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-494, -754, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-183, 42, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-397, 302, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(119, 215, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-734, 123, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-1121, 256, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-1548, 248, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-1542, 12, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-1575, -563, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-1354, -853, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-1902, -791, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(1187, 252, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(1952, 243, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(2025, -337, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(2282, -36, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(2243, 448, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(38, 844, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(219, 1539, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(-41, 1403, 30));
		AddSpawnPoint("d_zachariel_33.Id1", "d_zachariel_33", Rectangle(30, 1848, 30));

		// 'Beetle_Elite' Roaming Spawn
		AddSpawnPoint("d_zachariel_33.Id2", "d_zachariel_33", Rectangle(-22, -713, 9999));

		// Room 1 (-143, -1965)
		AddSpawnPoint("d_zachariel_33.Id3", "d_zachariel_33", Rectangle(-143, -1965, 400));
		AddSpawnPoint("d_zachariel_33.Id4", "d_zachariel_33", Rectangle(-143, -1965, 400));

		// Room 2 (-199, -730)
		AddSpawnPoint("d_zachariel_33.Id5", "d_zachariel_33", Rectangle(-199, -730, 400));
		AddSpawnPoint("d_zachariel_33.Id6", "d_zachariel_33", Rectangle(-199, -730, 400));

		// Room 3 (869, -631)
		AddSpawnPoint("d_zachariel_33.Id7", "d_zachariel_33", Rectangle(869, -631, 400));
		AddSpawnPoint("d_zachariel_33.Id8", "d_zachariel_33", Rectangle(869, -631, 400));

		// Room 4 (-119, 210)
		AddSpawnPoint("d_zachariel_33.Id9", "d_zachariel_33", Rectangle(-119, 210, 500));
		AddSpawnPoint("d_zachariel_33.Id10", "d_zachariel_33", Rectangle(-119, 210, 500));

		// Room 5 (69, 1606)
		AddSpawnPoint("d_zachariel_33.Id11", "d_zachariel_33", Rectangle(69, 1606, 500));
		AddSpawnPoint("d_zachariel_33.Id12", "d_zachariel_33", Rectangle(69, 1606, 500));

		// Room 6 (-1538, 253)
		AddSpawnPoint("d_zachariel_33.Id13", "d_zachariel_33", Rectangle(-1538, 253, 400));
		AddSpawnPoint("d_zachariel_33.Id14", "d_zachariel_33", Rectangle(-1538, 253, 400));

		// Room 7 (-1533, -804)
		AddSpawnPoint("d_zachariel_33.Id15", "d_zachariel_33", Rectangle(-1533, -804, 400));
		AddSpawnPoint("d_zachariel_33.Id16", "d_zachariel_33", Rectangle(-1533, -804, 400));

		// Corridor 1 (-146, -1317)
		AddSpawnPoint("d_zachariel_33.Id17", "d_zachariel_33", Rectangle(-146, -1317, 400));

		// Corridor 2 (-197, -198)
		AddSpawnPoint("d_zachariel_33.Id18", "d_zachariel_33", Rectangle(-197, -198, 400));

		// Corridor 3 (44, 787)
		AddSpawnPoint("d_zachariel_33.Id19", "d_zachariel_33", Rectangle(44, 787, 400));

		// Corridor 4 (-823, 242)
		AddSpawnPoint("d_zachariel_33.Id20", "d_zachariel_33", Rectangle(-823, 242, 400));
	}
}
