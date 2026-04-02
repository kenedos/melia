//--- Melia Script -----------------------------------------------------------
// Royal Mausoleum 5F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_zachariel_36'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel36MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------

		// Spawn Buffs -------------------------------------
		AddSpawnBuff("d_zachariel_36", MonsterId.Schlesien_Darkmage, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_36", MonsterId.Schlesien_Heavycavarly, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_36", MonsterId.Schlesien_Claw, BuffId.EliteMonsterBuff, chance: 100);
		// Monster Spawners ---------------------------------

		AddSpawner("d_zachariel_36.Id1", MonsterId.Rootcrystal_05, min: 8, max: 10, respawn: Seconds(15), tendency: TendencyType.Peaceful);
		AddSpawner("d_zachariel_36.Id2", MonsterId.Schlesien_Heavycavarly, min: 4, max: 4, tendency: TendencyType.Aggressive);

		// Room 1 (-2491, -4561)
		AddSpawner("d_zachariel_36.Id3", MonsterId.Schlesien_Claw, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_36.Id4", MonsterId.Schlesien_Darkmage, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 2 (-2499, -3595)
		AddSpawner("d_zachariel_36.Id5", MonsterId.Schlesien_Darkmage, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_36.Id6", MonsterId.Schlesien_Claw, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 3 (-2493, -2595)
		AddSpawner("d_zachariel_36.Id7", MonsterId.Schlesien_Darkmage, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_36.Id8", MonsterId.Schlesien_Claw, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 4 (-3429, -2590)
		AddSpawner("d_zachariel_36.Id9", MonsterId.Schlesien_Claw, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_36.Id10", MonsterId.Schlesien_Darkmage, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 5 (-1710, -2596)
		AddSpawner("d_zachariel_36.Id11", MonsterId.Schlesien_Darkmage, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_36.Id12", MonsterId.Schlesien_Claw, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Room 6 (-2490, -1088) - Big room, pop 5
		AddSpawner("d_zachariel_36.Id13", MonsterId.Schlesien_Darkmage, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_36.Id14", MonsterId.Schlesien_Claw, min: 2, max: 2, tendency: TendencyType.Aggressive);

		// Room 7 (-2664, 482) - Big room, pop 5
		AddSpawner("d_zachariel_36.Id15", MonsterId.Schlesien_Claw, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_36.Id16", MonsterId.Schlesien_Darkmage, min: 2, max: 2, tendency: TendencyType.Aggressive);

		// Corridor 1 (-2516, -1851)
		AddSpawner("d_zachariel_36.Id17", MonsterId.Schlesien_Darkmage, min: 1, max: 1, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Corridor 2 (-2492, -365)
		AddSpawner("d_zachariel_36.Id18", MonsterId.Schlesien_Claw, min: 1, max: 2, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' Spawn Points
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-2596, -4677, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-2412, -2689, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-2468, -1043, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-3548, -2598, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-2483, -3645, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-2744, -4492, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-2655, -5529, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-1584, -2551, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-2609, -1073, 30));
		AddSpawnPoint("d_zachariel_36.Id1", "d_zachariel_36", Rectangle(-2755, -2496, 30));

		// 'Schlesien_Heavycavarly' Roaming Spawn
		AddSpawnPoint("d_zachariel_36.Id2", "d_zachariel_36", Rectangle(-2493, -2595, 9999));

		// Room 1 (-2491, -4561)
		AddSpawnPoint("d_zachariel_36.Id3", "d_zachariel_36", Rectangle(-2491, -4561, 400));
		AddSpawnPoint("d_zachariel_36.Id4", "d_zachariel_36", Rectangle(-2491, -4561, 400));

		// Room 2 (-2499, -3595)
		AddSpawnPoint("d_zachariel_36.Id5", "d_zachariel_36", Rectangle(-2499, -3595, 400));
		AddSpawnPoint("d_zachariel_36.Id6", "d_zachariel_36", Rectangle(-2499, -3595, 400));

		// Room 3 (-2493, -2595)
		AddSpawnPoint("d_zachariel_36.Id7", "d_zachariel_36", Rectangle(-2493, -2595, 400));
		AddSpawnPoint("d_zachariel_36.Id8", "d_zachariel_36", Rectangle(-2493, -2595, 400));

		// Room 4 (-3429, -2590)
		AddSpawnPoint("d_zachariel_36.Id9", "d_zachariel_36", Rectangle(-3429, -2590, 400));
		AddSpawnPoint("d_zachariel_36.Id10", "d_zachariel_36", Rectangle(-3429, -2590, 400));

		// Room 5 (-1710, -2596)
		AddSpawnPoint("d_zachariel_36.Id11", "d_zachariel_36", Rectangle(-1710, -2596, 400));
		AddSpawnPoint("d_zachariel_36.Id12", "d_zachariel_36", Rectangle(-1710, -2596, 400));

		// Room 6 (-2490, -1088) - Big room
		AddSpawnPoint("d_zachariel_36.Id13", "d_zachariel_36", Rectangle(-2490, -1088, 500));
		AddSpawnPoint("d_zachariel_36.Id14", "d_zachariel_36", Rectangle(-2490, -1088, 500));

		// Room 7 (-2664, 482) - Big room
		AddSpawnPoint("d_zachariel_36.Id15", "d_zachariel_36", Rectangle(-2664, 482, 500));
		AddSpawnPoint("d_zachariel_36.Id16", "d_zachariel_36", Rectangle(-2664, 482, 500));

		// Corridor 1 (-2516, -1851)
		AddSpawnPoint("d_zachariel_36.Id17", "d_zachariel_36", Rectangle(-2516, -1851, 400));

		// Corridor 2 (-2492, -365)
		AddSpawnPoint("d_zachariel_36.Id18", "d_zachariel_36", Rectangle(-2492, -365, 400));
	}
}
