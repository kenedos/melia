//--- Melia Script -----------------------------------------------------------
// Zeraha Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_orchard_34_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard342MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_orchard_34_2.Id1", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Minutes(1));
		AddSpawner("f_orchard_34_2.Id2", MonsterId.Ferret_Slinger, min: 15, max: 20);
		AddSpawner("f_orchard_34_2.Id3", MonsterId.Ferret_Patter, min: 15, max: 20);
		AddSpawner("f_orchard_34_2.Id4", MonsterId.Ferret_Folk, min: 19, max: 25);
		AddSpawner("f_orchard_34_2.Id5", MonsterId.Ferret_Slinger, min: 12, max: 15);
		AddSpawner("f_orchard_34_2.Id6", MonsterId.Ferret_Patter, min: 15, max: 20);
		AddSpawner("f_orchard_34_2.Id7", MonsterId.Ferret_Folk, min: 30, max: 40);
		AddSpawner("f_orchard_34_2.Id8", MonsterId.Ferret_Loader, min: 30, max: 40);
		AddSpawner("f_orchard_34_2.Id9", MonsterId.Ferret_Searcher, min: 20, max: 30);
		AddSpawner("f_orchard_34_2.Id10", MonsterId.Ferret_Patter, min: 20, max: 30);
		AddSpawner("f_orchard_34_2.Id11", MonsterId.Ferret_Slinger, min: 15, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 10 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(1162, 1185, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(549, 1006, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(149, 895, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-293, 995, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-740, 1153, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-1080, 1135, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-653, 1674, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-18, 1477, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(355, 1625, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(990, 103, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(1285, -160, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(855, -775, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(224, -607, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-478, -813, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-779, -657, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-338, -77, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-858, 111, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-1213, -178, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-1293, -1147, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-1714, -428, 100));
		AddSpawnPoint("f_orchard_34_2.Id1", "f_orchard_34_2", Rectangle(-2307, 210, 100));

		// 'Ferret_Slinger' GenType 11 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(1251, -210, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(1808, -674, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(281, 1661, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-75, 1493, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-681, 1697, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-831, 111, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-290, -39, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-445, -791, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-1233, -1136, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-1694, -349, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-2224, 334, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-980, 1088, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-255, 1012, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(223, 928, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(527, 1006, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(839, -653, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(216, -485, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(-1115, -239, 100));
		AddSpawnPoint("f_orchard_34_2.Id2", "f_orchard_34_2", Rectangle(1210, 1193, 100));

		// 'Ferret_Patter' GenType 12 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(1011, 82, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(1784, -699, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(742, -953, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(256, -738, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-763, -666, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-517, -208, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-821, 100, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-1145, -210, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-1306, -1098, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-1931, -492, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-2282, 192, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-1023, 1102, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-604, 1674, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-371, 932, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(109, 835, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(529, 1055, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(-9, 1531, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(361, 1652, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(1239, 1107, 100));
		AddSpawnPoint("f_orchard_34_2.Id3", "f_orchard_34_2", Rectangle(9, 1527, 100));

		// 'Ferret_Folk' GenType 13 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1284, 1118, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1193, 1220, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1018, 1173, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1054, 1062, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(629, 955, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(501, 1064, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(436, 888, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(305, 996, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(192, 816, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(91, 1008, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-121, 905, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-125, 1067, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-291, 936, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-334, 1092, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-531, 935, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-641, 1139, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-456, -947, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-402, -779, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-645, -624, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-598, -809, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(70, -652, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(229, -531, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(320, -676, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(679, -951, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(825, -689, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(930, -891, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1318, -196, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1315, 64, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(932, 35, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(920, -265, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1222, -247, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(923, 150, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1859, -503, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(1782, -667, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(2008, -785, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(2023, -601, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-303, 45, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-199, -96, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-508, -220, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-586, -106, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-807, -242, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-763, 50, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-992, 105, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(-904, -238, 25));
		AddSpawnPoint("f_orchard_34_2.Id4", "f_orchard_34_2", Rectangle(9, 1527, 100));

		// 'Ferret_Slinger' GenType 14 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(1271, 1091, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(1156, 1223, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(363, 1772, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(240, 1660, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(403, 1532, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-81, 1431, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(78, 1445, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(11, 1613, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-651, 1759, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-650, 1552, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-516, 1627, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-641, 993, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-350, 965, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(39, 949, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(411, 945, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(888, -13, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(1248, 112, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(1219, -200, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(2022, -671, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(1943, -552, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(1799, -720, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(858, -614, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(1001, -724, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(657, -973, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(855, -973, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(230, -471, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(297, -702, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(147, -730, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(86, -536, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-380, -776, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-446, -685, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-731, -617, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-511, -908, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-1188, -1015, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-1303, -1093, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-1176, -1237, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-239, -128, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-378, -57, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-722, 122, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-890, 134, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-1020, -265, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-1328, -320, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-2070, 196, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-2167, 303, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-2332, 279, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-2249, 412, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-2186, 59, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-1755, -381, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-1601, -347, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(-1668, -422, 25));
		AddSpawnPoint("f_orchard_34_2.Id5", "f_orchard_34_2", Rectangle(9, 1527, 100));

		// 'Ferret_Patter' GenType 15 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(1018, 1128, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(1180, 1136, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(408, 1702, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(321, 1600, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(331, 1478, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-85, 1559, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1, 1518, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(66, 1551, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-536, 1767, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-768, 1708, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-565, 1563, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-374, 1106, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-150, 959, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(295, 985, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(532, 938, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(1070, 43, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(1165, -278, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(908, -199, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(790, -679, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(957, -652, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(784, -1000, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(653, -889, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(242, -765, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(299, -600, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(142, -436, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(218, -636, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-386, -874, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-440, -732, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-759, -720, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-788, -615, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-638, -820, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-568, -695, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-333, -1, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-319, -140, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-783, 81, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-801, 193, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1165, -409, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1292, -484, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1222, -1096, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1164, -949, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1099, -1202, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1618, -292, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1694, -304, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-1709, -419, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-2086, 156, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-2095, 332, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-2209, 376, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-2338, 353, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(-2318, 192, 25));
		AddSpawnPoint("f_orchard_34_2.Id6", "f_orchard_34_2", Rectangle(9, 1527, 100));

		// 'Ferret_Folk' GenType 26 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id7", "f_orchard_34_2", Rectangle(-632, 1666, 9999));

		// 'Ferret_Loader' GenType 27 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id8", "f_orchard_34_2", Rectangle(-622, 1682, 9999));

		// 'Ferret_Searcher' GenType 33 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id9", "f_orchard_34_2", Rectangle(1262, 1202, 9999));

		// 'Ferret_Patter' GenType 34 Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id10", "f_orchard_34_2", Rectangle(887, -733, 9999));

		// 'Ferret_Slinder' Spawn Points
		AddSpawnPoint("f_orchard_34_2.Id11", "f_orchard_34_2", Rectangle(887, -733, 9999));
	}
}
