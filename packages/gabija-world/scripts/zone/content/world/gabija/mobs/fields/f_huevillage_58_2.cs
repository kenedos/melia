//--- Melia Script -----------------------------------------------------------
// Vieta Gorge Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_huevillage_58_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage582MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_huevillage_58_2.Id1", MonsterId.Ultanun, min: 8, max: 10);
		AddSpawner("f_huevillage_58_2.Id2", MonsterId.Zibu_Maize, min: 6, max: 8);
		AddSpawner("f_huevillage_58_2.Id3", MonsterId.Zibu_Maize, min: 6, max: 8);
		AddSpawner("f_huevillage_58_2.Id4", MonsterId.Ultanun, min: 9, max: 12);
		AddSpawner("f_huevillage_58_2.Id5", MonsterId.Zibu_Maize, min: 8, max: 10);
		AddSpawner("f_huevillage_58_2.Id6", MonsterId.Rootcrystal_01, min: 9, max: 11, respawn: Seconds(30));
		AddSpawner("f_huevillage_58_2.Id7", MonsterId.Ultanun, min: 12, max: 15);
		AddSpawner("f_huevillage_58_2.Id8", MonsterId.Zibu_Maize, min: 5, max: 6);
		AddSpawner("f_huevillage_58_2.Id9", MonsterId.Rudas_Loxodon, amount: 3);

		// Monster Spawn Points -----------------------------

		// 'Ultanun' GenType 29 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id1", "f_huevillage_58_2", Rectangle(702, 119, 9999));

		// 'Zibu_Maize' GenType 31 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id2", "f_huevillage_58_2", Rectangle(636, 19, 9999));

		// 'Zibu_Maize' GenType 32 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-820, -122, 35));
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-944, -452, 35));
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-154, 13, 35));
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-5, -139, 35));
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-6, 185, 35));
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-393, -81, 35));
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-757, -340, 35));
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-653, -511, 35));
		AddSpawnPoint("f_huevillage_58_2.Id3", "f_huevillage_58_2", Rectangle(-540, -294, 35));

		// 'Ultanun' GenType 38 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(-864, -353, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(343, -855, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(477, -1103, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(677, -770, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(565, -939, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(580, -624, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(-688, -169, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(397, -596, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(-339, -78, 30));
		AddSpawnPoint("f_huevillage_58_2.Id4", "f_huevillage_58_2", Rectangle(-794, -98, 30));

		// 'Zibu_Maize' GenType 39 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id5", "f_huevillage_58_2", Rectangle(414, -1103, 30));
		AddSpawnPoint("f_huevillage_58_2.Id5", "f_huevillage_58_2", Rectangle(598, -964, 30));
		AddSpawnPoint("f_huevillage_58_2.Id5", "f_huevillage_58_2", Rectangle(409, -791, 30));
		AddSpawnPoint("f_huevillage_58_2.Id5", "f_huevillage_58_2", Rectangle(250, -639, 30));
		AddSpawnPoint("f_huevillage_58_2.Id5", "f_huevillage_58_2", Rectangle(505, -536, 30));
		AddSpawnPoint("f_huevillage_58_2.Id5", "f_huevillage_58_2", Rectangle(587, -800, 30));
		AddSpawnPoint("f_huevillage_58_2.Id5", "f_huevillage_58_2", Rectangle(286, -944, 30));

		// 'Rootcrystal_01' GenType 40 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(246, -1747, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(-348, -1390, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(-634, -1128, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(497, -603, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(-853, -557, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(-369, -111, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(-161, 1099, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(849, 754, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(705, -8, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(1575, 672, 200));
		AddSpawnPoint("f_huevillage_58_2.Id6", "f_huevillage_58_2", Rectangle(113, 461, 200));

		// 'Ultanun' GenType 43 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-440, 253, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-77, 1188, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-586, 144, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-497, 394, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-408, 117, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-584, 320, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-219, 1117, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-78, 1349, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(60, 1118, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(198, 1266, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-251, 1253, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-284, 212, 30));
		AddSpawnPoint("f_huevillage_58_2.Id7", "f_huevillage_58_2", Rectangle(-557, 246, 30));

		// 'Zibu_Maize' GenType 44 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id8", "f_huevillage_58_2", Rectangle(-444, 229, 100));

		// 'Rudas_Loxodon' GenType 45 Spawn Points
		AddSpawnPoint("f_huevillage_58_2.Id9", "f_huevillage_58_2", Rectangle(-57, 81, 9999));
	}
}
