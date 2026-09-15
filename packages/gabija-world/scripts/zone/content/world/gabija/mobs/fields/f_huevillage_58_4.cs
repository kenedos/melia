//--- Melia Script -----------------------------------------------------------
// Septyni Glen Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_huevillage_58_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage584MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_huevillage_58_4.Id1", MonsterId.Beeteros, min: 15, max: 20);
		AddSpawner("f_huevillage_58_4.Id2", MonsterId.Mentiwood, amount: 3);
		AddSpawner("f_huevillage_58_4.Id3", MonsterId.Carcashu, min: 12, max: 15);
		AddSpawner("f_huevillage_58_4.Id4", MonsterId.Rootcrystal_01, min: 9, max: 11, respawn: Seconds(30));
		AddSpawner("f_huevillage_58_4.Id5", MonsterId.Carcashu, min: 8, max: 10);
		AddSpawner("f_huevillage_58_4.Id6", MonsterId.Tiny_Mage, min: 6, max: 8);
		AddSpawner("f_huevillage_58_4.Id7", MonsterId.Tiny_Mage, min: 6, max: 8);

		// Monster Spawn Points -----------------------------

		// 'Beeteros' GenType 21 Spawn Points
		AddSpawnPoint("f_huevillage_58_4.Id1", "f_huevillage_58_4", Rectangle(70, -748, 9999));

		// 'Mentiwood' GenType 22 Spawn Points
		AddSpawnPoint("f_huevillage_58_4.Id2", "f_huevillage_58_4", Rectangle(538, -194, 9999));

		// 'Carcashu' GenType 23 Spawn Points
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-1022, -533, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-1074, -812, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-756, -846, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-734, -671, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-911, -840, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-888, -467, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-1153, -432, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-1010, -248, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-1401, -389, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-621, -803, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-889, -963, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-890, -696, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-1256, -225, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-1059, -44, 25));
		AddSpawnPoint("f_huevillage_58_4.Id3", "f_huevillage_58_4", Rectangle(-1097, -661, 25));

		// 'Rootcrystal_01' GenType 31 Spawn Points
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(-951, -482, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(-310, 300, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(-217, -365, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(205, -745, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(330, -194, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(825, -128, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(1223, -479, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(1385, 689, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(915, 825, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(254, 680, 200));
		AddSpawnPoint("f_huevillage_58_4.Id4", "f_huevillage_58_4", Rectangle(-793, -945, 200));

		// 'Carcashu' GenType 35 Spawn Points
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(-498, 335, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(-886, 230, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(-349, 186, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(315, 575, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(482, 726, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(1254, 669, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(576, -172, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(607, -381, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(994, 938, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(1126, 73, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(1629, -38, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(-722, 315, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(729, -275, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(891, -124, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(906, -401, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(846, 774, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(1307, 822, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(375, 861, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(-673, 454, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(-560, 575, 25));
		AddSpawnPoint("f_huevillage_58_4.Id5", "f_huevillage_58_4", Rectangle(-647, 203, 25));

		// 'Tiny_Mage' GenType 36 Spawn Points
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-961, -594, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-822, -782, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-1026, -847, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-1219, -378, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-764, -526, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-655, -643, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-1074, -45, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-1396, -363, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-842, -983, 25));
		AddSpawnPoint("f_huevillage_58_4.Id6", "f_huevillage_58_4", Rectangle(-619, -827, 25));

		// 'Tiny_Mage' GenType 37 Spawn Points
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(809, -366, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(807, -65, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(1329, 249, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(1399, -102, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(1199, 647, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(1015, 893, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(486, 624, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(506, 820, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(1270, -250, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(1309, 787, 25));
		AddSpawnPoint("f_huevillage_58_4.Id7", "f_huevillage_58_4", Rectangle(267, 665, 25));
	}
}
