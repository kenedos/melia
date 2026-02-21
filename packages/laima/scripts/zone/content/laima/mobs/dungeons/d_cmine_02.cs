//--- Melia Script -----------------------------------------------------------
// Crystal Mine 2F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_cmine_02'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine02MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------


		// Monster Spawners ---------------------------------

		AddSpawner("d_cmine_02.Id1", MonsterId.Rootcrystal_01, min: 23, max: 30, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("d_cmine_02.Id2", MonsterId.Yekubite, min: 20, max: 50, respawn: Minutes(3), tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_02.Id3", MonsterId.Yekubite, min: 70, max: 120, respawn: Seconds(30), tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_02.Id4", MonsterId.Shredded, min: 90, max: 120, respawn: Seconds(30), tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_02.Id5", MonsterId.Bubbe_Mage_Normal, min: 40, max: 90, respawn: Seconds(30), tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_02.Id6", MonsterId.Goblin_Archer, min: 40, max: 95, respawn: Seconds(30), tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_02.Id7", MonsterId.Bat, min: 90, max: 170, respawn: Seconds(30), tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 500 Spawn Points
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-2426, -778, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1769, -31, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1894, -86, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1509, 155, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1240, 98, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1373, -185, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1241, -414, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1200, -701, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1347, -879, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-694, -797, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1035, -1055, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-958, -867, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1457, 229, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1504, 755, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1585, 1116, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1748, 997, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1592, 748, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1703, 762, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1375, 916, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-1015, 123, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-580, 341, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-272, 8, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-154, -275, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(69, -156, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-220, 774, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-139, 945, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-335, 981, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-772, 1174, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(-592, 854, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(650, 33, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(687, -258, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(685, -503, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1097, -340, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(968, -765, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1717, -567, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1621, -728, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1239, -1599, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1963, -1420, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(2161, -1289, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(2060, -1508, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(491, 1406, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(477, 1107, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(997, 1124, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(984, 1242, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(947, 303, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1503, 221, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(648, 631, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1378, 444, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1538, 661, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1701, 833, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(2059, 1514, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1743, 1516, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1969, 1616, 10));
		AddSpawnPoint("d_cmine_02.Id1", "d_cmine_02", Rectangle(1679, 1380, 10));

		// 'Yekubite' GenType 515 Spawn Points
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(-1094, -1105, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(-1955, -137, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(-1059, 31, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(-235, -109, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(-146, 82, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(-1132, -736, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(0, -200, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(997, 1113, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(992, 1310, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(853, 1005, 30));
		AddSpawnPoint("d_cmine_02.Id2", "d_cmine_02", Rectangle(659, 1034, 30));

		// 'Yekubite' GenType 516 Spawn Points
		AddSpawnPoint("d_cmine_02.Id3", "d_cmine_02", Rectangle(-133, -27, 9999));

		// 'Shredded' GenType 531 Spawn Points
		AddSpawnPoint("d_cmine_02.Id4", "d_cmine_02", Rectangle(-1494, 895, 9999));

		// 'Bubbe_Mage_Normal' GenType 532 Spawn Points
		AddSpawnPoint("d_cmine_02.Id5", "d_cmine_02", Rectangle(-101, -336, 9999));

		// 'Goblin_Archer' GenType 535 Spawn Points
		AddSpawnPoint("d_cmine_02.Id6", "d_cmine_02", Rectangle(-587, 318, 9999));

		// 'Bat' Custom Gentype Spawn Points
		AddSpawnPoint("d_cmine_02.Id7", "d_cmine_02", Rectangle(-587, 318, 9999));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Ginklas, "d_cmine_02", 1, Hours(2), Hours(4));
		AddBossSpawner(MonsterId.Boss_Stone_Whale, "d_cmine_02", 1, Hours(2), Hours(4));
		AddBossSpawner(MonsterId.Boss_Carapace, "d_cmine_02", 1, Hours(2), Hours(4));
	}
}
