//--- Melia Script -----------------------------------------------------------
// Royal Mausoleum 3F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_zachariel_34'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel34MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------

		// Spawn Buffs -------------------------------------
		AddSpawnBuff("d_zachariel_34", MonsterId.Echad, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_34", MonsterId.Wolf_Statue_Mage_Pollution, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_34", MonsterId.Shtayim, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_34", MonsterId.Echad_Bow, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_34", MonsterId.Wolf_Statue_Mage, BuffId.EliteMonsterBuff, chance: 100);

		// Monster Spawners ---------------------------------

		AddSpawner("d_zachariel_34.Id1", MonsterId.Rootcrystal_05, min: 6, max: 8, Seconds(25), tendency: TendencyType.Peaceful);

		// Room 1 (1772, 13)
		AddSpawner("d_zachariel_34.Id2", MonsterId.Shtayim, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_34.Id3", MonsterId.Wolf_Statue_Mage_Pollution, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 2 (964, 282)
		AddSpawner("d_zachariel_34.Id4", MonsterId.Echad, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_34.Id5", MonsterId.Echad_Bow, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 3 (-347, 198)
		AddSpawner("d_zachariel_34.Id6", MonsterId.Wolf_Statue_Mage, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_34.Id7", MonsterId.Shtayim, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 4 (-273, 1168)
		AddSpawner("d_zachariel_34.Id8", MonsterId.Echad_Bow, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_34.Id9", MonsterId.Wolf_Statue_Mage, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 5 (-1128, 1150)
		AddSpawner("d_zachariel_34.Id10", MonsterId.Echad, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_34.Id11", MonsterId.Wolf_Statue_Mage_Pollution, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Room 6 (-1423, 200)
		AddSpawner("d_zachariel_34.Id12", MonsterId.Shtayim, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_34.Id13", MonsterId.Echad_Bow, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 7 (-1454, -1094)
		AddSpawner("d_zachariel_34.Id14", MonsterId.Wolf_Statue_Mage, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_34.Id15", MonsterId.Echad, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 8 (-2792, 137)
		AddSpawner("d_zachariel_34.Id16", MonsterId.Wolf_Statue_Mage_Pollution, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_34.Id17", MonsterId.Shtayim, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Corridor 1 (-2315, 192)
		AddSpawner("d_zachariel_34.Id18", MonsterId.Echad, min: 1, max: 1, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Corridor 2 (-1359, 718)
		AddSpawner("d_zachariel_34.Id19", MonsterId.Wolf_Statue_Mage, min: 1, max: 2, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Corridor 3 (-865, 201)
		AddSpawner("d_zachariel_34.Id20", MonsterId.Shtayim, min: 1, max: 1, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Corridor 4 (363, 187)
		AddSpawner("d_zachariel_34.Id21", MonsterId.Echad_Bow, min: 1, max: 2, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Corridor 5 (1342, 286)
		AddSpawner("d_zachariel_34.Id22", MonsterId.Wolf_Statue_Mage_Pollution, min: 1, max: 1, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Corridor 6 (2405, -2)
		AddSpawner("d_zachariel_34.Id23", MonsterId.Echad, min: 1, max: 2, respawn: Seconds(10), tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' Spawn Points
		AddSpawnPoint("d_zachariel_34.Id1", "d_zachariel_34", Rectangle(-1313, 226, 9999));

		// Room 1 (1772, 13)
		AddSpawnPoint("d_zachariel_34.Id2", "d_zachariel_34", Rectangle(1772, 13, 400));
		AddSpawnPoint("d_zachariel_34.Id3", "d_zachariel_34", Rectangle(1772, 13, 400));

		// Room 2 (964, 282)
		AddSpawnPoint("d_zachariel_34.Id4", "d_zachariel_34", Rectangle(964, 282, 400));
		AddSpawnPoint("d_zachariel_34.Id5", "d_zachariel_34", Rectangle(964, 282, 400));

		// Room 3 (-347, 198)
		AddSpawnPoint("d_zachariel_34.Id6", "d_zachariel_34", Rectangle(-347, 198, 400));
		AddSpawnPoint("d_zachariel_34.Id7", "d_zachariel_34", Rectangle(-347, 198, 400));

		// Room 4 (-273, 1168)
		AddSpawnPoint("d_zachariel_34.Id8", "d_zachariel_34", Rectangle(-273, 1168, 400));
		AddSpawnPoint("d_zachariel_34.Id9", "d_zachariel_34", Rectangle(-273, 1168, 400));

		// Room 5 (-1128, 1150)
		AddSpawnPoint("d_zachariel_34.Id10", "d_zachariel_34", Rectangle(-1128, 1150, 400));
		AddSpawnPoint("d_zachariel_34.Id11", "d_zachariel_34", Rectangle(-1128, 1150, 400));

		// Room 6 (-1423, 200)
		AddSpawnPoint("d_zachariel_34.Id12", "d_zachariel_34", Rectangle(-1423, 200, 400));
		AddSpawnPoint("d_zachariel_34.Id13", "d_zachariel_34", Rectangle(-1423, 200, 400));

		// Room 7 (-1454, -1094)
		AddSpawnPoint("d_zachariel_34.Id14", "d_zachariel_34", Rectangle(-1454, -1094, 400));
		AddSpawnPoint("d_zachariel_34.Id15", "d_zachariel_34", Rectangle(-1454, -1094, 400));

		// Room 8 (-2792, 137)
		AddSpawnPoint("d_zachariel_34.Id16", "d_zachariel_34", Rectangle(-2792, 137, 400));
		AddSpawnPoint("d_zachariel_34.Id17", "d_zachariel_34", Rectangle(-2792, 137, 400));

		// Corridor 1 (-2315, 192)
		AddSpawnPoint("d_zachariel_34.Id18", "d_zachariel_34", Rectangle(-2315, 192, 400));

		// Corridor 2 (-1359, 718)
		AddSpawnPoint("d_zachariel_34.Id19", "d_zachariel_34", Rectangle(-1359, 718, 400));

		// Corridor 3 (-865, 201)
		AddSpawnPoint("d_zachariel_34.Id20", "d_zachariel_34", Rectangle(-865, 201, 400));

		// Corridor 4 (363, 187)
		AddSpawnPoint("d_zachariel_34.Id21", "d_zachariel_34", Rectangle(363, 187, 400));

		// Corridor 5 (1342, 286)
		AddSpawnPoint("d_zachariel_34.Id22", "d_zachariel_34", Rectangle(1342, 286, 400));

		// Corridor 6 (2405, -2)
		AddSpawnPoint("d_zachariel_34.Id23", "d_zachariel_34", Rectangle(2405, -2, 400));
	}
}
