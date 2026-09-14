//--- Melia Script -----------------------------------------------------------
// Apsimesti Crossroads Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_pilgrimroad_52'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad52MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_pilgrimroad_52.Id1", MonsterId.Hook_Old, min: 15, max: 20);
		AddSpawner("f_pilgrimroad_52.Id2", MonsterId.TreeAmbulo_Red, min: 19, max: 25);
		AddSpawner("f_pilgrimroad_52.Id3", MonsterId.Rootcrystal_01, min: 8, max: 10, respawn: Seconds(5));
		AddSpawner("f_pilgrimroad_52.Id4", MonsterId.TreeAmbulo_Red, min: 12, max: 15);
		AddSpawner("f_pilgrimroad_52.Id5", MonsterId.Hook_Old, min: 8, max: 10);
		AddSpawner("f_pilgrimroad_52.Id6", MonsterId.Lichenclops_Mage, min: 6, max: 8);

		// Monster Spawn Points -----------------------------

		// 'Hook_Old' GenType 6 Spawn Points
		AddSpawnPoint("f_pilgrimroad_52.Id1", "f_pilgrimroad_52", Rectangle(-10, 373, 9999));

		// 'TreeAmbulo_Red' GenType 7 Spawn Points
		AddSpawnPoint("f_pilgrimroad_52.Id2", "f_pilgrimroad_52", Rectangle(-193, 166, 9999));

		// 'Rootcrystal_01' GenType 19 Spawn Points
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(-606, -2750, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(-1110, -1748, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(48, -937, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(367, 470, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(-361, 454, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(-1468, 169, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(-355, 1215, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(973, 1263, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(1423, 2103, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id3", "f_pilgrimroad_52", Rectangle(722, -183, 30));

		// 'TreeAmbulo_Red' GenType 38 Spawn Points
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-803, -2121, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-786, -2325, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-187, 1237, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-360, 1042, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-442, 1271, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-715, 1381, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-643, 1418, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-179, 1426, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-253, 1570, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-493, 1551, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-621, 1173, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(-292, 820, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(911, 2070, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(987, 2324, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(1272, 2522, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(1622, 2019, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(1326, 2158, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(1100, 2021, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(862, 2268, 25));
		AddSpawnPoint("f_pilgrimroad_52.Id4", "f_pilgrimroad_52", Rectangle(1404, 1944, 25));

		// 'Hook_Old' GenType 48 Spawn Points
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-607, -1516, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1290, -1882, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1371, -1584, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1141, -1414, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1074, -1729, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-916, -1953, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-868, -1625, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-743, -1821, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1615, -28, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1363, 186, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1375, -241, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1311, -15, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1044, -247, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id5", "f_pilgrimroad_52", Rectangle(-1008, 78, 30));

		// 'Lichenclops_Mage' GenType 49 Spawn Points
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(966, 1207, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(1419, 2170, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(440, 392, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(897, 2007, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(1567, 1899, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(199, 424, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(797, 2150, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(1399, 2466, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(514, 642, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(1290, 1762, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(1175, 2394, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(742, 958, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(74, 136, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-40, 340, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-254, 41, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-273, 756, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-457, 1158, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(211, 237, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-1249, -330, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-1554, -82, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-1351, 310, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-907, -125, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-617, 1268, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-588, 1598, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-221, 1586, 30));
		AddSpawnPoint("f_pilgrimroad_52.Id6", "f_pilgrimroad_52", Rectangle(-360, 1249, 30));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Succubus, "f_pilgrimroad_52", 1, Hours(2), Hours(4));
	}
}
