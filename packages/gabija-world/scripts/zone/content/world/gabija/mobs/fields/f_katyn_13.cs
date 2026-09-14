//--- Melia Script -----------------------------------------------------------
// Poslinkis Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_katyn_13'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn13MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_katyn_13.Id1", MonsterId.Rootcrystal_02, min: 15, max: 20, respawn: Seconds(5));
		AddSpawner("f_katyn_13.Id2", MonsterId.HighBube_Spear, min: 15, max: 20);
		AddSpawner("f_katyn_13.Id3", MonsterId.HighBube_Archer, min: 15, max: 20);
		AddSpawner("f_katyn_13.Id4", MonsterId.Arburn_Pokubu_Green, min: 12, max: 15);
		AddSpawner("f_katyn_13.Id5", MonsterId.HighBube_Spear, min: 12, max: 15);
		AddSpawner("f_katyn_13.Id6", MonsterId.HighBube_Archer, min: 8, max: 10);
		AddSpawner("f_katyn_13.Id7", MonsterId.Arburn_Pokubu_Green, min: 12, max: 15);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_02' GenType 514 Spawn Points
		AddSpawnPoint("f_katyn_13.Id1", "f_katyn_13", Rectangle(803, -786, 30));
		AddSpawnPoint("f_katyn_13.Id1", "f_katyn_13", Rectangle(-2186, 1225, 30));
		AddSpawnPoint("f_katyn_13.Id1", "f_katyn_13", Rectangle(-1117, 2238, 30));
		AddSpawnPoint("f_katyn_13.Id1", "f_katyn_13", Rectangle(804, -1065, 30));

		// 'HighBube_Spear' GenType 755 Spawn Points
		AddSpawnPoint("f_katyn_13.Id2", "f_katyn_13", Rectangle(-675, -1798, 9999));

		// 'HighBube_Archer' GenType 756 Spawn Points
		AddSpawnPoint("f_katyn_13.Id3", "f_katyn_13", Rectangle(1124, -549, 9999));

		// 'Arburn_Pokubu_Green' GenType 763 Spawn Points
		AddSpawnPoint("f_katyn_13.Id4", "f_katyn_13", Rectangle(-1253, 674, 9999));

		// 'HighBube_Spear' GenType 770 Spawn Points
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(492, -813, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-780, -1019, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-671, -589, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-421, -793, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-789, 144, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-1265, 298, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-1407, 847, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-1148, 1309, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-1136, 933, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-940, 1360, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-1101, 2142, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-106, 1337, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(-18, 1531, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(913, -475, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(987, -982, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1200, -390, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1333, -712, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1944, -547, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(2058, -405, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(2232, -521, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1132, 379, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(909, 999, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1206, 1253, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1136, 880, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1135, 2115, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1017, 2089, 30));
		AddSpawnPoint("f_katyn_13.Id5", "f_katyn_13", Rectangle(1585, 420, 30));

		// 'HighBube_Archer' GenType 771 Spawn Points
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(-757, -990, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(-580, -506, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(-1289, 949, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(-1143, 1445, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(55, 1611, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(-344, 1118, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(-1094, 2271, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1237, 1921, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1304, 1385, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1148, 846, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1018, 1162, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(788, -560, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(785, -937, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1168, -428, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1135, -756, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1992, -279, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(2215, -342, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1833, -621, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1180, -7, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(12, -796, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(-2193, 1136, 30));
		AddSpawnPoint("f_katyn_13.Id6", "f_katyn_13", Rectangle(1343, 1129, 30));

		// 'Arburn_Pokubu_Green' GenType 772 Spawn Points
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-981, 1040, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1339, 1451, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1624, 1025, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1242, 526, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1325, 316, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1027, 177, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-541, -222, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-766, -627, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-610, -859, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-362, -758, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-834, 1424, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-630, 1042, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-943, 2237, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1158, 2331, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(146, 1442, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-240, 1176, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1099, 839, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1214, 1188, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-835, 1195, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-1260, 856, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(32, 1479, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-893, -838, 30));
		AddSpawnPoint("f_katyn_13.Id7", "f_katyn_13", Rectangle(-653, 78, 30));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Mushwort, "f_katyn_13", 1, Hours(2), Hours(4));
		AddBossSpawner(MonsterId.Boss_Woodspirit_Green, "f_katyn_13", 1, Hours(2), Hours(4));
	}
}
