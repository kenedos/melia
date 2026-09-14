//--- Melia Script -----------------------------------------------------------
// Sekta Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_pilgrimroad_41_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad414MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_pilgrimroad_41_4.Id1", MonsterId.Rootcrystal_05, min: 12, max: 16, respawn: Seconds(5));
		AddSpawner("f_pilgrimroad_41_4.Id2", MonsterId.Dumaro_Yellow, min: 9, max: 12);
		AddSpawner("f_pilgrimroad_41_4.Id3", MonsterId.Repusbunny_Purple, min: 38, max: 50);
		AddSpawner("f_pilgrimroad_41_4.Id4", MonsterId.Repusbunny_Bow_Purple, min: 9, max: 12);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' GenType 4 Spawn Points
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(-112, 1084, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(-53, 846, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(477, 740, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(698, 799, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(773, 535, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(1086, 258, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(1371, 220, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(1322, -110, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(445, -200, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(297, -386, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(-166, -14, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(-924, -640, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(-171, -636, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(-263, -969, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(150, -1397, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(591, -1666, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(626, -1338, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(1184, -684, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(1458, -941, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(1855, -885, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(1870, -1153, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(1785, 10, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(2052, 84, 50));
		AddSpawnPoint("f_pilgrimroad_41_4.Id1", "f_pilgrimroad_41_4", Rectangle(-519, 24, 50));

		// 'Dumaro_Yellow' GenType 100 Spawn Points
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1052, -779, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(2007, -1125, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1741, -906, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1724, -1020, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(2023, -944, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1904, -1166, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1523, -51, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(564, 560, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(752, 507, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(824, 697, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(778, 849, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(134, -355, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(302, -484, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(550, -440, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(650, -224, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(356, -349, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(280, -288, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(428, -92, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(259, -75, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-77, 1, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-394, 159, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-473, -78, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-202, -114, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(89, -289, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-23, -501, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-247, -580, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(34, -752, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-166, -769, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-505, -699, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(882, -55, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1352, -539, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1463, -790, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1294, -827, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1171, -849, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1001, -651, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1337, -934, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1075, -960, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1131, -683, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1221, -520, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1584, 75, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1134, -581, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(1557, -921, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-1009, -826, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-989, -617, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-1203, -592, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-1023, -341, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-709, -539, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-780, -741, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-919, -798, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-1081, -727, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id2", "f_pilgrimroad_41_4", Rectangle(-902, -503, 25));

		// 'Repusbunny_Purple' GenType 101 Spawn Points
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(421, -217, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(483, -424, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-55, -147, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-387, 102, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(257, -477, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(74, -632, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-130, -705, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-351, -886, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-88, -955, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-68, -1163, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(282, -1298, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(270, -1522, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(461, -1713, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(663, -1572, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(492, -1534, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(658, -1312, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(737, -1456, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1136, -916, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1249, -573, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1138, -754, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1441, -789, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1926, -829, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1395, -675, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1294, -702, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1925, -1223, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1269, -931, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(554, 708, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(672, 769, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(640, 565, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(450, 662, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(118, 880, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-36, 837, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-446, -192, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(-292, -623, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1745, 29, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1905, 109, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1984, -42, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1574, -1044, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1815, -797, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1956, -1028, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1835, -1037, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1412, -894, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(219, -294, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(438, -97, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id3", "f_pilgrimroad_41_4", Rectangle(1386, -1005, 25));

		// 'Repusbunny_Bow_Purple' GenType 102 Spawn Points
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(278, 746, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(563, 883, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(473, 745, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(507, 567, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(809, 662, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(814, 483, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(1818, 43, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(1918, 176, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(1962, 58, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(2097, 87, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(2078, -75, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(1809, -106, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(1955, -99, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(181, -1494, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(376, -1623, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(476, -1795, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(656, -1705, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(531, -1491, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(778, -1482, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(741, -1269, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(466, -1312, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(198, -1304, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-505, 133, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-244, 215, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-145, -87, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-568, -100, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-893, -793, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-1061, -693, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-1036, -508, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-938, -347, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-738, -539, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-736, -709, 25));
		AddSpawnPoint("f_pilgrimroad_41_4.Id4", "f_pilgrimroad_41_4", Rectangle(-416, -99, 25));
	}
}
