//--- Melia Script -----------------------------------------------------------
// Tiltas Valley Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_rokas_28'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas28MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_rokas_28.Id1", MonsterId.Hogma_Archer, min: 8, max: 10);
		AddSpawner("f_rokas_28.Id2", MonsterId.Rootcrystal_05, min: 8, max: 10, respawn: Seconds(5));
		AddSpawner("f_rokas_28.Id3", MonsterId.Hogma_Archer, min: 8, max: 10);
		AddSpawner("f_rokas_28.Id4", MonsterId.Lauzinute, min: 23, max: 30);
		AddSpawner("f_rokas_28.Id5", MonsterId.Lauzinute, min: 4, max: 5);
		AddSpawner("f_rokas_28.Id6", MonsterId.Hogma_Archer, min: 8, max: 10);
		AddSpawner("f_rokas_28.Id7", MonsterId.Templeslave_Mage, min: 8, max: 10);

		// Monster Spawn Points -----------------------------

		// 'Hogma_Archer' GenType 56 Spawn Points
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-1390, -639, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-730, 155, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-1626, -674, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-1504, -353, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-1770, -486, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-1520, -496, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-699, -742, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-375, -162, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-620, -23, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-775, 156, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-536, 121, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-297, 34, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-171, -1650, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-53, -1464, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-161, -1320, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-166, -1114, 30));
		AddSpawnPoint("f_rokas_28.Id1", "f_rokas_28", Rectangle(-97, -1253, 30));

		// 'Rootcrystal_05' GenType 600 Spawn Points
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(-715, -768, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(-289, 101, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(1201, 708, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(-834, 170, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(-71, -1273, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(-218, -1535, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(-1289, -390, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(-1857, -593, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(377, 805, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(1192, 1050, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(1251, 2108, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(1638, 1597, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(177, -337, 30));
		AddSpawnPoint("f_rokas_28.Id2", "f_rokas_28", Rectangle(849, -481, 30));

		// 'Hogma_Archer' GenType 1024 Spawn Points
		AddSpawnPoint("f_rokas_28.Id3", "f_rokas_28", Rectangle(887, 566, 9999));

		// 'Lauzinute' GenType 1026 Spawn Points
		AddSpawnPoint("f_rokas_28.Id4", "f_rokas_28", Rectangle(966, 608, 9999));

		// 'Lauzinute' GenType 1027 Spawn Points
		AddSpawnPoint("f_rokas_28.Id5", "f_rokas_28", Rectangle(414, 734, 40));
		AddSpawnPoint("f_rokas_28.Id5", "f_rokas_28", Rectangle(276, 567, 40));
		AddSpawnPoint("f_rokas_28.Id5", "f_rokas_28", Rectangle(704, 621, 40));
		AddSpawnPoint("f_rokas_28.Id5", "f_rokas_28", Rectangle(588, 384, 40));
		AddSpawnPoint("f_rokas_28.Id5", "f_rokas_28", Rectangle(821, 527, 40));

		// 'Hogma_Archer' GenType 1028 Spawn Points
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(573, 357, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(655, 565, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(294, 612, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(512, 743, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(456, 454, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(1187, 858, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(1322, 760, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(1401, 1033, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(1225, 1106, 20));
		AddSpawnPoint("f_rokas_28.Id6", "f_rokas_28", Rectangle(1339, 917, 20));

		// 'Templeslave_Mage' GenType 1031 Spawn Points
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(696, 518, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(612, 405, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(1256, 769, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(1382, 1018, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(1178, 1029, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(1612, 1629, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(1336, 1733, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(1142, 1777, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(267, 602, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(427, 712, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(1420, 1375, 30));
		AddSpawnPoint("f_rokas_28.Id7", "f_rokas_28", Rectangle(1479, 843, 30));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Ravinepede, "f_rokas_28", 1, Hours(6), Hours(12));
	}
}
