//--- Melia Script -----------------------------------------------------------
// Sienakal Graveyard Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'id_catacomb_33_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb331MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("id_catacomb_33_1.Id1", MonsterId.Rootcrystal_05, min: 12, max: 15, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("id_catacomb_33_1.Id2", MonsterId.Sec_Maggot_Green, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_1.Id3", MonsterId.Sec_Leaf_Diving_Purple, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_1.Id4", MonsterId.Fisherman_Green, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_1.Id5", MonsterId.Sec_Maggot_Green, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_1.Id6", MonsterId.Sec_Leaf_Diving_Purple, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_33_1.Id7", MonsterId.Drapel, min: 30, max: 40, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_05' GenType 3 Spawn Points
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(18, -860, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(927, -61, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(642, 852, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(926, 690, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(1541, 175, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(1590, -453, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(1466, -654, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(906, 1423, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(662, 1704, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(1405, 1473, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(101, 1231, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-530, 922, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(26, 740, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(91, -397, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-1056, 149, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-582, 125, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-989, 1164, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-840, -150, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-1005, -1077, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-657, -836, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-363, -876, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(935, -283, 100));
		AddSpawnPoint("id_catacomb_33_1.Id1", "id_catacomb_33_1", Rectangle(-1088, 770, 100));

		// 'Sec_Maggot_Green' GenType 9 Spawn Points
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(678, -87, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(871, 1417, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(1540, 690, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(1483, -670, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(-529, 899, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(-996, -1071, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(944, -272, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(637, 1696, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(1457, 792, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(1469, -384, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(-701, -1101, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(-671, -814, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(-59, -952, 100));
		AddSpawnPoint("id_catacomb_33_1.Id2", "id_catacomb_33_1", Rectangle(1604, -995, 100));

		// 'Sec_Leaf_Diving_Purple' GenType 10 Spawn Points
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(629, 872, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(1512, 221, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(929, 1678, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(173, 1312, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(-1177, 1297, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(-1015, -832, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(919, 867, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(1619, -215, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(583, 1419, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(-28, 1294, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(-1065, 1129, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(-645, -815, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(-314, -974, 100));
		AddSpawnPoint("id_catacomb_33_1.Id3", "id_catacomb_33_1", Rectangle(1436, 1686, 100));

		// 'Fisherman_Green' GenType 11 Spawn Points
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-881, -392, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-779, -506, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-669, -441, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-558, -284, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-505, -433, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-509, -527, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-569, -187, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-576, -38, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-615, 109, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-579, 159, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-838, 121, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-906, 129, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-1225, -357, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-1062, 65, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-1180, 188, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-1238, 130, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-1075, -107, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-1034, -240, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-1063, -337, 40));
		AddSpawnPoint("id_catacomb_33_1.Id4", "id_catacomb_33_1", Rectangle(-1075, -449, 40));

		// 'Sec_Maggot_Green' GenType 23 Spawn Points
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1396, -857, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1445, -704, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1599, -544, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1619, -428, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1453, -212, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1622, 41, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(783, -362, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(841, -53, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(588, 715, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(758, 729, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(722, 1388, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(789, 1551, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1582, 1445, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1052, 1576, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(1450, 781, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(303, 775, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(77, 501, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-202, 498, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-172, 733, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(84, 202, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(98, -116, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-183, -418, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-942, 1302, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-1041, 1122, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-1052, 807, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-1071, 599, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-1083, 359, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-1211, -123, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-941, -1007, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-585, -899, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-553, -1087, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(-154, -851, 40));
		AddSpawnPoint("id_catacomb_33_1.Id5", "id_catacomb_33_1", Rectangle(34, -1056, 40));

		// 'Sec_Leaf_Diving_Purple' GenType 24 Spawn Points
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(103, -396, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(97, -261, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(96, 70, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-201, 646, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-85, 765, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(24, 737, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(75, 353, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(632, 577, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(908, 561, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(649, 1538, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(772, 1731, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1227, 1606, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1415, 1491, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1664, 1697, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1714, 1526, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(756, 323, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(827, -189, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(700, -31, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(688, -286, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1438, -1023, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1633, -791, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1502, -684, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1531, -67, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(1419, 30, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(429, -898, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(205, -884, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(7, -855, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-329, -837, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-440, -1044, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-809, -902, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-985, -1088, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-1029, -866, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-1207, -241, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-806, -416, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-570, -405, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-741, -161, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-610, 134, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-887, 126, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-532, 529, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-541, 1055, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-263, 1122, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(69, 1225, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(44, 1389, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-1057, 556, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-1075, 306, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-936, 703, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-1040, 990, 40));
		AddSpawnPoint("id_catacomb_33_1.Id6", "id_catacomb_33_1", Rectangle(-955, 1288, 40));

		// 'Drapel' GenType 26 Spawn Points
		AddSpawnPoint("id_catacomb_33_1.Id7", "id_catacomb_33_1", Rectangle(661, -114, 9999));
	}
}
