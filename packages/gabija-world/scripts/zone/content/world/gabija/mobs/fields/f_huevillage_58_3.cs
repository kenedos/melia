//--- Melia Script -----------------------------------------------------------
// Cobalt Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_huevillage_58_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage583MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_huevillage_58_3.Id1", MonsterId.Caro, min: 12, max: 15);
		AddSpawner("f_huevillage_58_3.Id2", MonsterId.Upent, amount: 2);
		AddSpawner("f_huevillage_58_3.Id3", MonsterId.Upent, amount: 2);
		AddSpawner("f_huevillage_58_3.Id4", MonsterId.Caro, min: 15, max: 20);
		AddSpawner("f_huevillage_58_3.Id5", MonsterId.Rootcrystal_01, min: 9, max: 12, respawn: Seconds(30));
		AddSpawner("f_huevillage_58_3.Id6", MonsterId.Caro, min: 8, max: 10);
		AddSpawner("f_huevillage_58_3.Id7", MonsterId.Tiny_Bow, min: 15, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Caro' GenType 23 Spawn Points
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(1106, -482, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(1119, -1005, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-842, -646, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-344, -572, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(1148, -341, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(1024, -862, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-967, -439, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-940, -706, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-298, -410, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-983, -585, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-961, -247, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-455, -746, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(-506, -479, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(1054, -226, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(965, -395, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(1241, -956, 35));
		AddSpawnPoint("f_huevillage_58_3.Id1", "f_huevillage_58_3", Rectangle(983, -1046, 35));

		// 'Upent' GenType 24 Spawn Points
		AddSpawnPoint("f_huevillage_58_3.Id2", "f_huevillage_58_3", Rectangle(1090, -340, 150));
		AddSpawnPoint("f_huevillage_58_3.Id2", "f_huevillage_58_3", Rectangle(1119, -1005, 150));

		// 'Upent' GenType 25 Spawn Points
		AddSpawnPoint("f_huevillage_58_3.Id3", "f_huevillage_58_3", Rectangle(300, -667, 9999));

		// 'Caro' GenType 26 Spawn Points
		AddSpawnPoint("f_huevillage_58_3.Id4", "f_huevillage_58_3", Rectangle(-975, -749, 9999));

		// 'Rootcrystal_01' GenType 32 Spawn Points
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(235, -1400, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(939, -975, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(258, -955, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(-1262, -1098, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(-700, -1040, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(-954, -596, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(-289, -128, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(9, 343, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(518, 257, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(981, -388, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(1907, -584, 200));
		AddSpawnPoint("f_huevillage_58_3.Id5", "f_huevillage_58_3", Rectangle(-251, -611, 200));

		// 'Caro' GenType 44 Spawn Points
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1906, -739, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1739, -396, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1772, -537, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1743, -726, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1918, -511, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1734, -248, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1962, -647, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1863, -372, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1847, -631, 25));
		AddSpawnPoint("f_huevillage_58_3.Id6", "f_huevillage_58_3", Rectangle(1581, -184, 25));

		// 'Tiny_Bow' GenType 46 Spawn Points
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-746, -714, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-387, -473, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-1151, -851, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(1813, -659, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(1940, -556, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(1853, -398, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-817, -331, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-948, -459, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-413, -815, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(1754, -461, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(1698, -273, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-311, -686, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-258, -458, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-949, -732, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-1014, -294, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-788, -516, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-958, -598, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-835, -768, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-1249, -1028, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-481, -636, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-589, -344, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-336, 118, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-149, 154, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-160, 318, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(53, 405, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(-278, 223, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(494, 228, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(581, 366, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(684, 193, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(684, 193, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(649, 91, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(846, -32, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(430, 438, 25));
		AddSpawnPoint("f_huevillage_58_3.Id7", "f_huevillage_58_3", Rectangle(329, 224, 25));
	}
}
