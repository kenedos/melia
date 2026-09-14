//--- Melia Script -----------------------------------------------------------
// Dingofasil District Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_flash_58'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash58MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_flash_58.Id1", MonsterId.Rootcrystal_03, min: 19, max: 25, respawn: Minutes(1));
		AddSpawner("f_flash_58.Id2", MonsterId.Infroholder_Red, min: 19, max: 25);
		AddSpawner("f_flash_58.Id3", MonsterId.Socket_Purple, min: 19, max: 25);
		AddSpawner("f_flash_58.Id4", MonsterId.Infroholder_Mage_Green, min: 12, max: 15);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 3 Spawn Points
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-133, -1664, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-299, -1079, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-971, -1293, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-1086, -873, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-884, -392, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-1245, 149, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-526, 501, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-150, -22, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(304, -397, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(1045, -1151, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(1680, -1140, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(1581, -403, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(2264, -483, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(2345, -80, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(1313, 218, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(1885, 328, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(1756, 1051, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(917, 1101, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(416, 1167, 10));
		AddSpawnPoint("f_flash_58.Id1", "f_flash_58", Rectangle(-204, 1308, 10));

		// 'Infroholder_Red' GenType 21 Spawn Points
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(692, -156, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1216, -284, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1212, 31, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1303, 189, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(892, 517, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1171, 1021, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1643, 1084, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1853, 351, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1730, -34, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(2188, 46, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(2188, -380, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(2053, -198, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1596, -571, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(916, -1196, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1092, -1077, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(1082, -1383, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(686, -1210, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(425, -1073, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(263, -268, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(30, -262, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-143, -45, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-424, 562, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-134, 1113, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-383, 1371, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-623, 895, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-439, 939, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-929, 275, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-1241, 247, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-870, -484, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-1128, -855, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-1061, -1176, 25));
		AddSpawnPoint("f_flash_58.Id2", "f_flash_58", Rectangle(-1118, 15, 25));

		// 'Socket_Purple' GenType 22 Spawn Points
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-363, 523, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-147, -65, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(224, -236, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(159, -415, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(429, -119, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(796, -217, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-1038, 70, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-903, -536, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-1074, -676, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-1151, -1012, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(822, 1097, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-157, 1137, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-368, 874, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-494, 1213, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1381, 989, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1852, 1088, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1840, 530, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1581, 958, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1668, 61, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1526, -165, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-644, 917, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-166, 1344, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-278, 1003, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-652, 521, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(-1058, 231, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(21, -228, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1201, 81, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1310, 341, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(1021, 425, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(874, 343, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(2139, -315, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(2277, -481, 25));
		AddSpawnPoint("f_flash_58.Id3", "f_flash_58", Rectangle(2366, -354, 25));

		// 'Infroholder_Mage_Green' GenType 23 Spawn Points
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(19, -254, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(-174, -34, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(-390, 594, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(-825, 470, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(-883, 325, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(-1202, 283, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(-438, 1177, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(-209, 869, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(635, 1187, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1350, 903, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1479, 1066, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1780, 869, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1543, 849, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1446, 166, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1175, 205, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1272, -14, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1012, -60, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1044, -323, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1469, -293, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1770, 0, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(1746, 730, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(2146, 49, 25));
		AddSpawnPoint("f_flash_58.Id4", "f_flash_58", Rectangle(2270, -333, 25));
	}
}
