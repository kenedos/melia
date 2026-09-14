//--- Melia Script -----------------------------------------------------------
// Crystal Mine Lot 2 - 2F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_cmine_9'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine9MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_cmine_9.Id1", MonsterId.Rootcrystal_01, amount: 2, respawn: Seconds(5), tendency: TendencyType.Peaceful);
		AddSpawner("d_cmine_9.Id2", MonsterId.FD_Bubbe_Fighter, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_9.Id3", MonsterId.FD_Bubbe_Mage_Ice, min: 4, max: 5, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_9.Id4", MonsterId.FD_Bubbe_Fighter, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_9.Id5", MonsterId.FD_Stoulet_Mage, min: 8, max: 10, tendency: TendencyType.Peaceful);
		AddSpawner("d_cmine_9.Id6", MonsterId.FD_Bubbe_Mage_Ice, min: 6, max: 7, tendency: TendencyType.Peaceful);
		AddSpawner("d_cmine_9.Id7", MonsterId.FD_Bat_Big, min: 6, max: 7, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 514 Spawn Points
		AddSpawnPoint("d_cmine_9.Id1", "d_cmine_9", Rectangle(119, -1162, 30));
		AddSpawnPoint("d_cmine_9.Id1", "d_cmine_9", Rectangle(-538, -70, 30));
		AddSpawnPoint("d_cmine_9.Id1", "d_cmine_9", Rectangle(522, -24, 30));
		AddSpawnPoint("d_cmine_9.Id1", "d_cmine_9", Rectangle(-560, 521, 30));

		// 'FD_Bubbe_Fighter' GenType 1020 Spawn Points
		AddSpawnPoint("d_cmine_9.Id2", "d_cmine_9", Rectangle(-616, 443, 9999));

		// 'FD_Bubbe_Mage_Ice' GenType 1022 Spawn Points
		AddSpawnPoint("d_cmine_9.Id3", "d_cmine_9", Rectangle(172, -1077, 9999));

		// 'FD_Bubbe_Fighter' GenType 1023 Spawn Points
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(-206, -508, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(-867, -646, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(-577, -163, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(542, -64, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(-74, 616, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(683, 1023, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(-488, 767, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(-643, 291, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(697, 22, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(-463, 378, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(-1216, 790, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(906, -27, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(681, -214, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(937, 949, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(806, 1306, 30));
		AddSpawnPoint("d_cmine_9.Id4", "d_cmine_9", Rectangle(385, 518, 30));

		// 'FD_Stoulet_Mage' GenType 1050 Spawn Points
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(581, -53, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(712, 121, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(497, 252, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(667, 1049, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(953, 918, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(758, 1238, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(-460, 742, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(-661, 235, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(-591, 509, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(648, 882, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(557, -225, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(-793, 570, 25));
		AddSpawnPoint("d_cmine_9.Id5", "d_cmine_9", Rectangle(-516, 290, 25));

		// 'FD_Bubbe_Mage_Ice' GenType 1051 Spawn Points
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(203, -1106, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(556, -67, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-646, -556, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-817, -706, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-568, -113, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-224, -490, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(710, 1131, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(914, 740, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-671, 382, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-467, 417, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-471, 724, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-163, 849, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(217, 535, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(654, 178, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-1137, 791, 30));
		AddSpawnPoint("d_cmine_9.Id6", "d_cmine_9", Rectangle(-1622, 649, 30));

		// 'FD_Bat_Big' GenType 1052 Spawn Points
		AddSpawnPoint("d_cmine_9.Id7", "d_cmine_9", Rectangle(556, 115, 9999));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Kubas, "d_cmine_9", 1, Hours(6), Hours(12));
		AddBossSpawner(MonsterId.FD_Boss_Kubas, "d_cmine_9", 1, Hours(2), Hours(4));
	}
}
