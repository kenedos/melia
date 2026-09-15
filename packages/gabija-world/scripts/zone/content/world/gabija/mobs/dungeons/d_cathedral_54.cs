//--- Melia Script -----------------------------------------------------------
// Grand Corridor Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_cathedral_54'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DCathedral54MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_cathedral_54.Id1", MonsterId.Stoulet_Blue, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_54.Id2", MonsterId.NightMaiden_Mage, min: 6, max: 8, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_54.Id3", MonsterId.Rootcrystal_03, min: 16, max: 21, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("d_cathedral_54.Id4", MonsterId.Stoulet_Blue, min: 6, max: 7, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_54.Id5", MonsterId.Velwriggler_Blue, min: 9, max: 12, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Stoulet_Blue' GenType 2 Spawn Points
		AddSpawnPoint("d_cathedral_54.Id1", "d_cathedral_54", Rectangle(1000, -164, 9999));

		// 'NightMaiden_Mage' GenType 7 Spawn Points
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(563, -585, 30));
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(277, -734, 30));
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(269, -1210, 30));
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(64, -1316, 30));
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(688, -961, 30));
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(-316, -1276, 30));
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(-298, -957, 30));
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(761, -1308, 30));
		AddSpawnPoint("d_cathedral_54.Id2", "d_cathedral_54", Rectangle(-333, -625, 30));

		// 'Rootcrystal_03' GenType 41 Spawn Points
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(1029, -311, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(1013, 413, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(718, -975, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(649, 675, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(526, -588, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(299, -889, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(286, -1268, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-346, 1070, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(250, 1211, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-152, -1264, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-319, -662, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-27, 1322, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-1012, -533, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-771, -943, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(928, 1262, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-1583, -795, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-1446, -555, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-1599, 628, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-1411, 928, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-738, 1011, 10));
		AddSpawnPoint("d_cathedral_54.Id3", "d_cathedral_54", Rectangle(-491, 639, 10));

		// 'Stoulet_Blue' GenType 42 Spawn Points
		AddSpawnPoint("d_cathedral_54.Id4", "d_cathedral_54", Rectangle(857, 661, 30));
		AddSpawnPoint("d_cathedral_54.Id4", "d_cathedral_54", Rectangle(1080, 357, 30));
		AddSpawnPoint("d_cathedral_54.Id4", "d_cathedral_54", Rectangle(651, 606, 30));
		AddSpawnPoint("d_cathedral_54.Id4", "d_cathedral_54", Rectangle(913, 1079, 30));
		AddSpawnPoint("d_cathedral_54.Id4", "d_cathedral_54", Rectangle(709, 1115, 30));
		AddSpawnPoint("d_cathedral_54.Id4", "d_cathedral_54", Rectangle(926, 1227, 30));
		AddSpawnPoint("d_cathedral_54.Id4", "d_cathedral_54", Rectangle(700, 1302, 30));

		// 'Velwriggler_Blue' GenType 44 Spawn Points
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-1529, 655, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-799, 847, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-352, 1058, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(288, 1260, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-1530, 848, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-933, 581, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-940, 996, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-747, 1003, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-1223, 954, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-434, 625, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-5, 1354, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-366, 1347, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-1192, 675, 30));
		AddSpawnPoint("d_cathedral_54.Id5", "d_cathedral_54", Rectangle(-666, 672, 30));
	}
}
