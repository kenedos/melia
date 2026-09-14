//--- Melia Script -----------------------------------------------------------
// Royal Mausoleum 1F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_zachariel_32'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel32MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------

		// Spawn Buffs -------------------------------------
		AddSpawnBuff("d_zachariel_32", MonsterId.Zinutekas, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_32", MonsterId.Stoulet, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_32", MonsterId.Moving_Trap, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_32", MonsterId.Karas, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_32", MonsterId.Zinutekas_Elite, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_zachariel_32", MonsterId.Varv, BuffId.EliteMonsterBuff, chance: 100);

		// Monster Spawners ---------------------------------

		AddSpawner("d_zachariel_32.Id1", MonsterId.Rootcrystal_05, min: 12, max: 16, respawn: Seconds(5), tendency: TendencyType.Peaceful);
		AddSpawner("d_zachariel_32.Id2", MonsterId.Zinutekas, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id3", MonsterId.Zinutekas_Elite, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 1 (40, -977) - South Center
		AddSpawner("d_zachariel_32.Id4", MonsterId.Stoulet, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id5", MonsterId.Varv, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 2 (-1016, -972) - South West
		AddSpawner("d_zachariel_32.Id6", MonsterId.Moving_Trap, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id7", MonsterId.Karas, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 3 (-987, 125) - Mid West
		AddSpawner("d_zachariel_32.Id8", MonsterId.Stoulet, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id9", MonsterId.Karas, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 4 (55, 125) - Mid Center
		AddSpawner("d_zachariel_32.Id10", MonsterId.Moving_Trap, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id11", MonsterId.Varv, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Room 5 (1098, 130) - Mid East
		AddSpawner("d_zachariel_32.Id12", MonsterId.Stoulet, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id13", MonsterId.Karas, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 6 (1098, -962) - South East
		AddSpawner("d_zachariel_32.Id14", MonsterId.Moving_Trap, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id15", MonsterId.Varv, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 7 (45, 1408) - North Center
		AddSpawner("d_zachariel_32.Id16", MonsterId.Karas, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id17", MonsterId.Varv, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Room 8 (-1018, 1414) - North West
		AddSpawner("d_zachariel_32.Id18", MonsterId.Stoulet, min: 1, max: 1, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id19", MonsterId.Moving_Trap, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Room 9 (1108, 1408) - North East
		AddSpawner("d_zachariel_32.Id20", MonsterId.Varv, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_32.Id21", MonsterId.Moving_Trap, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' Spawn Points
		AddSpawnPoint("d_zachariel_32.Id1", "d_zachariel_32", Rectangle(-42, -1123, 30));
		AddSpawnPoint("d_zachariel_32.Id1", "d_zachariel_32", Rectangle(1230, 72, 30));
		AddSpawnPoint("d_zachariel_32.Id1", "d_zachariel_32", Rectangle(-872, 75, 30));
		AddSpawnPoint("d_zachariel_32.Id1", "d_zachariel_32", Rectangle(52, 1357, 30));

		// 'Zinutekas' Roaming Spawn
		AddSpawnPoint("d_zachariel_32.Id2", "d_zachariel_32", Rectangle(56, 94, 9999));

		// 'Zinutekas_Elite' Roaming Spawn
		AddSpawnPoint("d_zachariel_32.Id3", "d_zachariel_32", Rectangle(56, 94, 9999));

		// Room 1 (40, -977) - South Center
		AddSpawnPoint("d_zachariel_32.Id4", "d_zachariel_32", Rectangle(40, -977, 400));
		AddSpawnPoint("d_zachariel_32.Id5", "d_zachariel_32", Rectangle(40, -977, 400));

		// Room 2 (-1016, -972) - South West
		AddSpawnPoint("d_zachariel_32.Id6", "d_zachariel_32", Rectangle(-1016, -972, 400));
		AddSpawnPoint("d_zachariel_32.Id7", "d_zachariel_32", Rectangle(-1016, -972, 400));

		// Room 3 (-987, 125) - Mid West
		AddSpawnPoint("d_zachariel_32.Id8", "d_zachariel_32", Rectangle(-987, 125, 400));
		AddSpawnPoint("d_zachariel_32.Id9", "d_zachariel_32", Rectangle(-987, 125, 400));

		// Room 4 (55, 125) - Mid Center
		AddSpawnPoint("d_zachariel_32.Id10", "d_zachariel_32", Rectangle(55, 125, 400));
		AddSpawnPoint("d_zachariel_32.Id11", "d_zachariel_32", Rectangle(55, 125, 400));

		// Room 5 (1098, 130) - Mid East
		AddSpawnPoint("d_zachariel_32.Id12", "d_zachariel_32", Rectangle(1098, 130, 400));
		AddSpawnPoint("d_zachariel_32.Id13", "d_zachariel_32", Rectangle(1098, 130, 400));

		// Room 6 (1098, -962) - South East
		AddSpawnPoint("d_zachariel_32.Id14", "d_zachariel_32", Rectangle(1098, -962, 400));
		AddSpawnPoint("d_zachariel_32.Id15", "d_zachariel_32", Rectangle(1098, -962, 400));

		// Room 7 (45, 1408) - North Center
		AddSpawnPoint("d_zachariel_32.Id16", "d_zachariel_32", Rectangle(45, 1408, 400));
		AddSpawnPoint("d_zachariel_32.Id17", "d_zachariel_32", Rectangle(45, 1408, 400));

		// Room 8 (-1018, 1414) - North West
		AddSpawnPoint("d_zachariel_32.Id18", "d_zachariel_32", Rectangle(-1018, 1414, 400));
		AddSpawnPoint("d_zachariel_32.Id19", "d_zachariel_32", Rectangle(-1018, 1414, 400));

		// Room 9 (1108, 1408) - North East
		AddSpawnPoint("d_zachariel_32.Id20", "d_zachariel_32", Rectangle(1108, 1408, 400));
		AddSpawnPoint("d_zachariel_32.Id21", "d_zachariel_32", Rectangle(1108, 1408, 400));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Achat, "d_zachariel_32", 1, Hours(2), Hours(4));
	}
}
