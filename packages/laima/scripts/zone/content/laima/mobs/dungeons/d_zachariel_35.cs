//--- Melia Script -----------------------------------------------------------
// Royal Mausoleum 4F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_zachariel_35'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel35MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------

		// Spawn Buffs -------------------------------------
		AddSpawnBuff("d_zachariel_35", MonsterId.Dog_Of_King, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_35", MonsterId.Schlesien_Guard, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_35", MonsterId.Wolf_Statue_Bow, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_35", MonsterId.Karas_Mage, BuffId.EliteMonsterBuff, chance: 100);

		// Monster Spawners ---------------------------------

		AddSpawner("d_zachariel_35.Id1", MonsterId.Rootcrystal_05, min: 6, max: 8, respawn: Seconds(5), tendency: TendencyType.Peaceful);

		// Room 1 (876, -1152)
		AddSpawner("d_zachariel_35.Id2", MonsterId.Dog_Of_King, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_35.Id3", MonsterId.Schlesien_Guard, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 2 (1159, -47)
		AddSpawner("d_zachariel_35.Id4", MonsterId.Wolf_Statue_Bow, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_35.Id5", MonsterId.Karas_Mage, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 3 (67, -30)
		AddSpawner("d_zachariel_35.Id6", MonsterId.Dog_Of_King, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_35.Id7", MonsterId.Wolf_Statue_Bow, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 4 (-1073, -11)
		AddSpawner("d_zachariel_35.Id8", MonsterId.Schlesien_Guard, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_35.Id9", MonsterId.Karas_Mage, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 5 (-701, -1335)
		AddSpawner("d_zachariel_35.Id10", MonsterId.Wolf_Statue_Bow, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_35.Id11", MonsterId.Dog_Of_King, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Room 6 (-478, 1107)
		AddSpawner("d_zachariel_35.Id12", MonsterId.Karas_Mage, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_35.Id13", MonsterId.Schlesien_Guard, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 7 (585, 1074)
		AddSpawner("d_zachariel_35.Id14", MonsterId.Dog_Of_King, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_35.Id15", MonsterId.Wolf_Statue_Bow, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Corridor 1 (855, 1484)
		AddSpawner("d_zachariel_35.Id16", MonsterId.Schlesien_Guard, min: 1, max: 1, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Corridor 2 (1166, 777)
		AddSpawner("d_zachariel_35.Id17", MonsterId.Dog_Of_King, min: 1, max: 2, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Corridor 3 (684, -48)
		AddSpawner("d_zachariel_35.Id18", MonsterId.Karas_Mage, min: 1, max: 1, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Corridor 4 (-577, -8)
		AddSpawner("d_zachariel_35.Id19", MonsterId.Wolf_Statue_Bow, min: 1, max: 2, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Corridor 5 (-1048, -779)
		AddSpawner("d_zachariel_35.Id20", MonsterId.Schlesien_Guard, min: 1, max: 1, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Corridor 6 (-1045, 923)
		AddSpawner("d_zachariel_35.Id21", MonsterId.Dog_Of_King, min: 1, max: 2, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Corridor 7 (-483, 1528)
		AddSpawner("d_zachariel_35.Id22", MonsterId.Karas_Mage, min: 1, max: 1, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' Spawn Points
		AddSpawnPoint("d_zachariel_35.Id1", "d_zachariel_35", Rectangle(1059, -1508, 30));
		AddSpawnPoint("d_zachariel_35.Id1", "d_zachariel_35", Rectangle(1212, -118, 30));
		AddSpawnPoint("d_zachariel_35.Id1", "d_zachariel_35", Rectangle(1084, 1432, 30));
		AddSpawnPoint("d_zachariel_35.Id1", "d_zachariel_35", Rectangle(-56, -112, 30));
		AddSpawnPoint("d_zachariel_35.Id1", "d_zachariel_35", Rectangle(-669, 1159, 30));
		AddSpawnPoint("d_zachariel_35.Id1", "d_zachariel_35", Rectangle(-962, -1449, 30));
		AddSpawnPoint("d_zachariel_35.Id1", "d_zachariel_35", Rectangle(-1049, -50, 30));
		AddSpawnPoint("d_zachariel_35.Id1", "d_zachariel_35", Rectangle(496, 1507, 30));

		// Room 1 (876, -1152)
		AddSpawnPoint("d_zachariel_35.Id2", "d_zachariel_35", Rectangle(876, -1152, 400));
		AddSpawnPoint("d_zachariel_35.Id3", "d_zachariel_35", Rectangle(876, -1152, 400));

		// Room 2 (1159, -47)
		AddSpawnPoint("d_zachariel_35.Id4", "d_zachariel_35", Rectangle(1159, -47, 400));
		AddSpawnPoint("d_zachariel_35.Id5", "d_zachariel_35", Rectangle(1159, -47, 400));

		// Room 3 (67, -30)
		AddSpawnPoint("d_zachariel_35.Id6", "d_zachariel_35", Rectangle(67, -30, 400));
		AddSpawnPoint("d_zachariel_35.Id7", "d_zachariel_35", Rectangle(67, -30, 400));

		// Room 4 (-1073, -11)
		AddSpawnPoint("d_zachariel_35.Id8", "d_zachariel_35", Rectangle(-1073, -11, 400));
		AddSpawnPoint("d_zachariel_35.Id9", "d_zachariel_35", Rectangle(-1073, -11, 400));

		// Room 5 (-701, -1335)
		AddSpawnPoint("d_zachariel_35.Id10", "d_zachariel_35", Rectangle(-701, -1335, 400));
		AddSpawnPoint("d_zachariel_35.Id11", "d_zachariel_35", Rectangle(-701, -1335, 400));

		// Room 6 (-478, 1107)
		AddSpawnPoint("d_zachariel_35.Id12", "d_zachariel_35", Rectangle(-478, 1107, 400));
		AddSpawnPoint("d_zachariel_35.Id13", "d_zachariel_35", Rectangle(-478, 1107, 400));

		// Room 7 (585, 1074)
		AddSpawnPoint("d_zachariel_35.Id14", "d_zachariel_35", Rectangle(585, 1074, 400));
		AddSpawnPoint("d_zachariel_35.Id15", "d_zachariel_35", Rectangle(585, 1074, 400));

		// Corridor 1 (855, 1484)
		AddSpawnPoint("d_zachariel_35.Id16", "d_zachariel_35", Rectangle(855, 1484, 400));

		// Corridor 2 (1166, 777)
		AddSpawnPoint("d_zachariel_35.Id17", "d_zachariel_35", Rectangle(1166, 777, 400));

		// Corridor 3 (684, -48)
		AddSpawnPoint("d_zachariel_35.Id18", "d_zachariel_35", Rectangle(684, -48, 400));

		// Corridor 4 (-577, -8)
		AddSpawnPoint("d_zachariel_35.Id19", "d_zachariel_35", Rectangle(-577, -8, 400));

		// Corridor 5 (-1048, -779)
		AddSpawnPoint("d_zachariel_35.Id20", "d_zachariel_35", Rectangle(-1048, -779, 400));

		// Corridor 6 (-1045, 923)
		AddSpawnPoint("d_zachariel_35.Id21", "d_zachariel_35", Rectangle(-1045, 923, 400));

		// Corridor 7 (-483, 1528)
		AddSpawnPoint("d_zachariel_35.Id22", "d_zachariel_35", Rectangle(-483, 1528, 400));
	}
}
