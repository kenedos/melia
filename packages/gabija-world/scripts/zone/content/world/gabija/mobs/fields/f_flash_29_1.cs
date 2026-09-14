//--- Melia Script -----------------------------------------------------------
// Coastal Fortress Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_flash_29_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash291MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_flash_29_1.Id1", MonsterId.Rootcrystal_03, min: 15, max: 20, respawn: Minutes(1));
		AddSpawner("f_flash_29_1.Id2", MonsterId.Minos_Orange, min: 19, max: 25);
		AddSpawner("f_flash_29_1.Id3", MonsterId.Infroholder_Bow_Red, min: 12, max: 15);
		AddSpawner("f_flash_29_1.Id4", MonsterId.Minos_Mage_Green, min: 9, max: 12);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 3 Spawn Points
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-1719, -170, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-1435, -356, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-1075, -587, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-950, -932, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-469, -796, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-230, -627, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-203, -329, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-494, -325, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-766, -339, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(178, -709, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(89, -69, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(133, 245, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(6, 569, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(1, 844, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(61, 1145, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(568, 173, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(850, -44, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(816, 362, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(1212, 348, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(1331, 560, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(1596, 423, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(1235, -34, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(1147, -503, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-457, 157, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-737, 99, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-1000, 75, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-1178, 302, 10));
		AddSpawnPoint("f_flash_29_1.Id1", "f_flash_29_1", Rectangle(-1391, 308, 10));

		// 'Minos_Orange' GenType 28 Spawn Points
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-1165, -537, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-1035, -739, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-983, -548, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-862, -837, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-742, -664, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-591, -835, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-486, -649, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-569, -321, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-70, -132, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-128, -253, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(57, -631, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(236, -803, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(363, -750, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(230, -639, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(583, 171, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(744, 125, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(938, 26, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(1169, -61, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(1319, 140, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(1406, 55, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(1094, -488, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(952, -504, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(1080, -292, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(1070, 393, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(1309, 400, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(779, 360, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(28, 732, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(34, 524, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-634, 90, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-1044, 98, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-1275, 238, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-1512, 333, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(-868, 131, 20));
		AddSpawnPoint("f_flash_29_1.Id2", "f_flash_29_1", Rectangle(853, -83, 20));

		// 'Infroholder_Bow_Red' GenType 29 Spawn Points
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(-1306, 389, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(-761, 112, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(-810, 12, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(-948, -416, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(-739, -336, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(-823, -714, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(-2, -475, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(-180, -348, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(907, 322, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(1108, 348, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(1330, 559, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(1248, 347, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(1137, 66, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(1235, -138, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(1168, -449, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(1055, -527, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(976, -407, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(28, 885, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(68, 581, 20));
		AddSpawnPoint("f_flash_29_1.Id3", "f_flash_29_1", Rectangle(14, 1011, 20));

		// 'Minos_Mage_Green' GenType 30 Spawn Points
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(-670, 181, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(-946, 73, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(-1392, 299, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(-796, -375, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(-22, -403, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(110, -777, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(861, 71, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(1050, -312, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(1142, -503, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(1219, -37, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(1570, 430, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(944, 349, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(67, 593, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(-46, 933, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(176, 909, 20));
		AddSpawnPoint("f_flash_29_1.Id4", "f_flash_29_1", Rectangle(30, 734, 20));
	}
}
