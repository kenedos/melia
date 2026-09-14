//--- Melia Script -----------------------------------------------------------
// Roxona Reconstruction Agency East Building Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_firetower_61_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower612MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_firetower_61_2.Id1", MonsterId.Velffigy, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_61_2.Id2", MonsterId.Glyquare, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_61_2.Id3", MonsterId.Colifly_Black, min: 9, max: 11, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_61_2.Id4", MonsterId.Glyquare, min: 6, max: 7, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_61_2.Id5", MonsterId.Altarcrystal_R1, amount: 1, respawn: Minutes(2), tendency: TendencyType.Peaceful);
		AddSpawner("d_firetower_61_2.Id6", MonsterId.Altarcrystal_R1, amount: 2, respawn: Minutes(2), tendency: TendencyType.Peaceful);
		AddSpawner("d_firetower_61_2.Id7", MonsterId.Altarcrystal_R1, amount: 2, respawn: Minutes(3), tendency: TendencyType.Peaceful);
		AddSpawner("d_firetower_61_2.Id8", MonsterId.Rootcrystal_04, min: 19, max: 25, respawn: Seconds(30), tendency: TendencyType.Peaceful);
		AddSpawner("d_firetower_61_2.Id9", MonsterId.Colifly_Black, min: 15, max: 20, tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Velffigy' GenType 1 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1431, -373, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1197, -313, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1091, -407, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1112, -192, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1115, -945, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1013, -1053, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-990, -895, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1021, -338, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-25, -329, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-20, -103, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(95, -365, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1158, -830, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-1171, -1055, 25));
		AddSpawnPoint("d_firetower_61_2.Id1", "d_firetower_61_2", Rectangle(-204, -416, 25));

		// 'Glyquare' GenType 5 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-80, -553, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-373, -218, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(315, -289, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-132, 282, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-264, -1636, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-1156, -96, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-1298, -380, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-988, -387, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-1830, -789, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-1930, -761, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-1761, -1094, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-877, 414, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-1127, 391, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(13, 391, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(72, 442, 20));
		AddSpawnPoint("d_firetower_61_2.Id2", "d_firetower_61_2", Rectangle(-111, 420, 20));

		// 'Colifly_Black' GenType 6 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-220, -1462, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(52, -1500, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-168, -1681, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(18, -1726, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(130, -1365, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-68, -1553, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-181, 430, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-89, 622, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-13, 330, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(80, 517, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-152, -306, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-102, -489, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(135, -198, 30));
		AddSpawnPoint("d_firetower_61_2.Id3", "d_firetower_61_2", Rectangle(-51, 477, 30));

		// 'Glyquare' GenType 7 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id4", "d_firetower_61_2", Rectangle(844, -217, 20));
		AddSpawnPoint("d_firetower_61_2.Id4", "d_firetower_61_2", Rectangle(807, -398, 20));
		AddSpawnPoint("d_firetower_61_2.Id4", "d_firetower_61_2", Rectangle(1096, -388, 20));
		AddSpawnPoint("d_firetower_61_2.Id4", "d_firetower_61_2", Rectangle(1125, -507, 20));
		AddSpawnPoint("d_firetower_61_2.Id4", "d_firetower_61_2", Rectangle(1057, 287, 20));
		AddSpawnPoint("d_firetower_61_2.Id4", "d_firetower_61_2", Rectangle(984, 363, 20));
		AddSpawnPoint("d_firetower_61_2.Id4", "d_firetower_61_2", Rectangle(1022, -1164, 20));
		AddSpawnPoint("d_firetower_61_2.Id4", "d_firetower_61_2", Rectangle(956, -522, 20));

		// 'Altarcrystal_R1' GenType 10 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(-1012, 664, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(-1072, -309, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(-1056, -993, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(-40, -292, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(-57, 507, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(737, 843, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(1048, -327, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(979, -960, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(-19, -1491, 300));
		AddSpawnPoint("d_firetower_61_2.Id5", "d_firetower_61_2", Rectangle(-1848, -1009, 300));

		// 'Altarcrystal_R1' GenType 11 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(-1005, 569, 300));
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(-1056, -287, 300));
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(-1882, -1012, 300));
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(6, -313, 300));
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(1043, -326, 300));
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(-4, 460, 300));
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(-84, -1530, 300));
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(993, -1007, 300));
		AddSpawnPoint("d_firetower_61_2.Id6", "d_firetower_61_2", Rectangle(735, 770, 300));

		// 'Altarcrystal_R1' GenType 12 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id7", "d_firetower_61_2", Rectangle(-906, 644, 300));
		AddSpawnPoint("d_firetower_61_2.Id7", "d_firetower_61_2", Rectangle(-1831, -1013, 300));
		AddSpawnPoint("d_firetower_61_2.Id7", "d_firetower_61_2", Rectangle(-1073, -321, 300));
		AddSpawnPoint("d_firetower_61_2.Id7", "d_firetower_61_2", Rectangle(-25, -283, 300));
		AddSpawnPoint("d_firetower_61_2.Id7", "d_firetower_61_2", Rectangle(1052, -319, 300));

		// 'Rootcrystal_04' GenType 21 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-1873, -1277, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-1889, -892, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-1826, -795, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-1173, -400, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-1074, -210, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-1052, -948, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-197, -262, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(76, -293, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-205, -1448, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(131, -1497, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(934, -1043, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(1015, -250, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(741, 474, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(702, 783, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-144, 435, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-981, 744, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-1099, 540, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-2019, 1533, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(-2059, 1090, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(1575, -1660, 100));
		AddSpawnPoint("d_firetower_61_2.Id8", "d_firetower_61_2", Rectangle(1795, -1404, 100));

		// 'Colifly_Black' GenType 24 Spawn Points
		AddSpawnPoint("d_firetower_61_2.Id9", "d_firetower_61_2", Rectangle(-1080, -276, 9999));
	}
}
