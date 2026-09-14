//--- Melia Script -----------------------------------------------------------
// Inner Enceinte District Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_flash_64'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash64MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_flash_64.Id1", MonsterId.Repusbunny, min: 23, max: 30);
		AddSpawner("f_flash_64.Id2", MonsterId.Lemuria, min: 12, max: 15);
		AddSpawner("f_flash_64.Id3", MonsterId.Rubabos, min: 6, max: 7);
		AddSpawner("f_flash_64.Id4", MonsterId.Lemuria, min: 12, max: 16);
		AddSpawner("f_flash_64.Id5", MonsterId.Repusbunny, min: 12, max: 15);
		AddSpawner("f_flash_64.Id6", MonsterId.Lemuria, min: 12, max: 15);
		AddSpawner("f_flash_64.Id7", MonsterId.Saltisdaughter_Bow, min: 8, max: 10);
		AddSpawner("f_flash_64.Id8", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(20));
		AddSpawner("f_flash_64.Id9", MonsterId.Lemuria, min: 6, max: 8);
		AddSpawner("f_flash_64.Id10", MonsterId.Wood_Carving, amount: 3, respawn: Minutes(1));

		// Monster Spawn Points -----------------------------

		// 'Repusbunny' GenType 2 Spawn Points
		AddSpawnPoint("f_flash_64.Id1", "f_flash_64", Rectangle(2, -403, 9999));

		// 'Lemuria' GenType 3 Spawn Points
		AddSpawnPoint("f_flash_64.Id2", "f_flash_64", Rectangle(-1149, -243, 9999));

		// 'Rubabos' GenType 4 Spawn Points
		AddSpawnPoint("f_flash_64.Id3", "f_flash_64", Rectangle(1413, 730, 30));
		AddSpawnPoint("f_flash_64.Id3", "f_flash_64", Rectangle(1175, 1232, 30));
		AddSpawnPoint("f_flash_64.Id3", "f_flash_64", Rectangle(1384, 606, 30));
		AddSpawnPoint("f_flash_64.Id3", "f_flash_64", Rectangle(980, 535, 30));
		AddSpawnPoint("f_flash_64.Id3", "f_flash_64", Rectangle(1438, -136, 30));
		AddSpawnPoint("f_flash_64.Id3", "f_flash_64", Rectangle(1324, 87, 30));
		AddSpawnPoint("f_flash_64.Id3", "f_flash_64", Rectangle(1047, 62, 30));

		// 'Lemuria' GenType 5 Spawn Points
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-1046, -226, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-1352, -104, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-13, -569, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-260, -531, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-150, -695, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-392, -669, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-1177, -276, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-1080, 248, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-143, 1942, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-177, 1106, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-165, 1272, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-537, 354, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-982, 532, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-819, 714, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-989, 650, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-431, 948, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(-559, 806, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(917, 602, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(1050, 373, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(1464, 507, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(1417, 701, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(1191, 1225, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(742, 997, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(1194, -68, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(1232, 86, 30));
		AddSpawnPoint("f_flash_64.Id4", "f_flash_64", Rectangle(1487, -25, 30));

		// 'Repusbunny' GenType 6 Spawn Points
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-1238, -233, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-292, -558, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-64, -577, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-59, -1783, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-1064, -307, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-1134, 181, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-580, 353, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-219, 784, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-199, 532, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-289, 1485, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-37, 1786, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-270, 1845, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-65, -374, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(326, -1240, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(1293, 13, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(1439, -71, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(1250, -85, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(1094, 14, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(1074, 417, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(903, 647, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(1423, 604, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-70, 1555, 30));
		AddSpawnPoint("f_flash_64.Id5", "f_flash_64", Rectangle(-22, 2063, 30));

		// 'Lemuria' GenType 18 Spawn Points
		AddSpawnPoint("f_flash_64.Id6", "f_flash_64", Rectangle(1112, 378, 20));
		AddSpawnPoint("f_flash_64.Id6", "f_flash_64", Rectangle(975, 557, 20));
		AddSpawnPoint("f_flash_64.Id6", "f_flash_64", Rectangle(1365, 749, 20));
		AddSpawnPoint("f_flash_64.Id6", "f_flash_64", Rectangle(1482, 663, 20));
		AddSpawnPoint("f_flash_64.Id6", "f_flash_64", Rectangle(1461, 519, 20));
		AddSpawnPoint("f_flash_64.Id6", "f_flash_64", Rectangle(1164, 660, 20));

		// 'Saltisdaughter_Bow' GenType 22 Spawn Points
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-165, 1070, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-206, 1672, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-4, 1774, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-256, 1894, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(32, 1581, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-164, 790, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-303, 725, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-1091, 200, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-766, 358, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-169, 1518, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-389, 907, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-186, 582, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-1312, -161, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-1096, -221, 35));
		AddSpawnPoint("f_flash_64.Id7", "f_flash_64", Rectangle(-1150, -356, 35));

		// 'Rootcrystal_01' GenType 29 Spawn Points
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-130, -1794, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-100, -2324, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-365, -1609, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(50, -1493, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-254, -575, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-146, -431, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(3, -624, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-200, 91, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(338, 477, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-754, 374, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-1117, 250, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-1355, -64, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-1134, -283, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-1085, -530, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-944, 632, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-320, 725, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-102, 754, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-115, 1559, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(72, 1683, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-107, 1925, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-291, 1900, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(-165, 1062, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(1478, -12, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(1146, 17, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(1083, 376, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(1431, 654, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(846, 685, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(1184, 1155, 100));
		AddSpawnPoint("f_flash_64.Id8", "f_flash_64", Rectangle(627, 325, 100));

		// 'Lemuria' GenType 38 Spawn Points
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-401, -1735, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-292, -1658, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-106, -1768, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-48, -1513, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(31, -1600, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-62, -1641, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-179, -1675, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-88, -1860, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-47, -1947, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-72, -2045, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(-256, -1796, 20));
		AddSpawnPoint("f_flash_64.Id9", "f_flash_64", Rectangle(39, -1639, 20));

		// 'Wood_Carving' GenType 1001 Spawn Points
		AddSpawnPoint("f_flash_64.Id10", "f_flash_64", Rectangle(-450, -1710, 20));
		AddSpawnPoint("f_flash_64.Id10", "f_flash_64", Rectangle(-491, -1635, 20));
		AddSpawnPoint("f_flash_64.Id10", "f_flash_64", Rectangle(-442, -1575, 20));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Gargoyle, "f_flash_64", 1, Hours(2), Hours(4));
	}
}
