//--- Melia Script -----------------------------------------------------------
// Mesafasla Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_tableland_28_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland281MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_tableland_28_1.Id1", MonsterId.Rootcrystal_03, min: 10, max: 13, respawn: Seconds(5));
		AddSpawner("f_tableland_28_1.Id2", MonsterId.Repusbunny_Green, min: 23, max: 30);
		AddSpawner("f_tableland_28_1.Id3", MonsterId.Repusbunny_Bow_Green, min: 19, max: 25);
		AddSpawner("f_tableland_28_1.Id4", MonsterId.Saltisdaughter_Mage_Red, min: 15, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 4 Spawn Points
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(2, -524, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(1219, -609, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(1528, -431, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(2020, -107, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(2257, 205, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(1937, 599, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(678, -17, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(710, 308, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(327, 236, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-128, 231, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-442, 387, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-681, 616, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-969, 699, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-1257, 165, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-758, 1081, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-778, 1483, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-1712, 602, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-1710, 1105, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-1431, 1313, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-2195, 1317, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-2819, 1055, 50));
		AddSpawnPoint("f_tableland_28_1.Id1", "f_tableland_28_1", Rectangle(-3211, 885, 50));

		// 'Repusbunny_Green' GenType 32 Spawn Points
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(1963, 537, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(1758, -184, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(1360, -671, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(1451, -501, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(1242, -372, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-39, -471, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(8, -628, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(431, 107, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(600, 171, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(723, 72, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(566, 325, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-616, 557, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-334, 298, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(10, 234, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(137, 257, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(1776, 680, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(1869, 854, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(1631, 913, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1582, -26, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1331, -1, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1762, -195, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1327, -241, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1529, 124, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1016, 585, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-927, 738, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-809, 1044, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1219, 623, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1574, 498, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1826, 636, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1666, 680, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1681, 1247, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1401, 1341, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1785, 971, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-3125, 1089, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1548, 1394, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-2969, 1138, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-3231, 981, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-2535, 1233, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-2357, 1307, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-2053, 1356, 25));
		AddSpawnPoint("f_tableland_28_1.Id2", "f_tableland_28_1", Rectangle(-1304, 1256, 25));

		// 'Repusbunny_Bow_Green' GenType 35 Spawn Points
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1383, -129, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1331, 208, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1164, -9, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1622, -231, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1666, 141, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1130, 394, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-2906, 1064, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-2208, 1345, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1802, 943, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-851, 624, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1534, 1249, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1829, 779, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1707, 579, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1344, 711, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-851, 837, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1367, 1426, 25));
		AddSpawnPoint("f_tableland_28_1.Id3", "f_tableland_28_1", Rectangle(-1779, -6, 25));

		// 'Saltisdaughter_Mage_Red' GenType 39 Spawn Points
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(2163, 328, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(-126, -520, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(577, 28, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(406, 264, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(745, 253, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(1173, -558, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(1431, -291, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(1533, -692, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(1607, -481, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(2103, -70, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(1802, 905, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(-520, 450, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(-295, 264, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(170, 256, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(2, -641, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(1438, -527, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(1822, -176, 25));
		AddSpawnPoint("f_tableland_28_1.Id4", "f_tableland_28_1", Rectangle(1739, 737, 25));
	}
}
