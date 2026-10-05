//--- Melia Script -----------------------------------------------------------
// Seir Rainforest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_orchard_32_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard324MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_orchard_32_4.Id1", MonsterId.Rootcrystal_01, min: 15, max: 20, respawn: Minutes(1));
		AddSpawner("f_orchard_32_4.Id2", MonsterId.Ferret_Archer, min: 23, max: 30);
		AddSpawner("f_orchard_32_4.Id3", MonsterId.Ferret_Folk, min: 30, max: 40);
		AddSpawner("f_orchard_32_4.Id4", MonsterId.Ferret_Vendor, min: 19, max: 25);
		AddSpawner("f_orchard_32_4.Id5", MonsterId.Ferret_Bearer_Elite, min: 8, max: 10);
		AddSpawner("f_orchard_32_4.Id6", MonsterId.Ferret_Searcher, min: 30, max: 40);

		AddSpawner("f_orchard_32_4.Id7", MonsterId.Ferret_Archer, min: 30, max: 40);
		AddSpawner("f_orchard_32_4.Id8", MonsterId.Ferret_Folk, min: 20, max: 30);
		AddSpawner("f_orchard_32_4.Id9", MonsterId.Ferret_Vendor, min: 30, max: 40);
		AddSpawner("f_orchard_32_4.Id10", MonsterId.Ferret_Bearer_Elite, min: 10, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 8 Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(1198, -950, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(1003, -780, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(1435, 34, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(918, 165, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(905, 549, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(424, 116, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(411, 495, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(74, 1298, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-251, 1418, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(21, 692, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-155, 334, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(91, 182, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-39, -480, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-432, -738, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-673, -607, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-1018, -592, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-1620, -854, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-997, -374, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-513, 189, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-968, 222, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-1371, 217, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-1753, 862, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(-860, 901, 100));
		AddSpawnPoint("f_orchard_32_4.Id1", "f_orchard_32_4", Rectangle(1573, 862, 100));

		// 'Ferret_Archer' GenType 9 Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(1420, 41, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(1313, 163, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(874, 279, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(950, 72, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-58, 472, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(51, 796, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(4, 177, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-1018, -585, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-1355, -684, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-543, -769, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(996, -764, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(1087, -983, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(1651, 826, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(133, 1295, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-250, 1430, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-523, 207, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-943, 213, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-1376, 186, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(-1706, 843, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(369, 72, 100));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(431, 74, 40));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(1347, 70, 40));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(874, 168, 40));
		AddSpawnPoint("f_orchard_32_4.Id2", "f_orchard_32_4", Rectangle(474, 905, 40));

		// 'Ferret_Folk' GenType 11 Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1397, -62, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1445, 143, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1261, 142, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1238, -23, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1345, 45, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1005, 78, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(910, 149, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(819, 285, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(768, 74, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(956, 280, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(533, 53, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(495, 231, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(389, 260, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(316, 67, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(416, 181, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-155, 772, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(64, 633, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-92, 546, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(103, 412, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-87, 408, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(75, 186, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-45, 36, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(103, 36, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-584, -669, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-709, -654, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-587, -794, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-604, -688, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-684, -783, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-934, -615, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1041, -517, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1286, -666, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1437, -683, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1364, -780, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1461, -794, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1586, -749, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1467, -905, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(926, -612, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(850, -665, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(847, -804, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(946, -778, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1065, -715, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1032, -841, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1027, -993, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1076, -903, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1220, -984, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1152, -1025, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-453, 218, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-542, 281, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-580, 375, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-841, 195, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-950, 346, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1015, 190, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1027, 294, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1283, 180, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1351, 310, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1481, 290, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1388, 126, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1598, 803, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1710, 947, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1774, 838, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-1538, 951, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-643, 814, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-792, 916, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-660, 887, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-717, 813, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-279, 1481, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-283, 1379, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-36, 1467, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(65, 1439, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-96, 1285, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(-104, 1168, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(95, 1329, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(205, 1283, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1718, 848, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1656, 760, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1485, 963, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1413, 877, 25));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(431, 74, 40));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(1347, 70, 40));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(874, 168, 40));
		AddSpawnPoint("f_orchard_32_4.Id3", "f_orchard_32_4", Rectangle(474, 905, 40));

		// 'Ferret_Vendor' GenType 12 Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(1382, 479, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(1336, 170, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(735, 618, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1051, -660, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(332, 458, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-58, 676, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-97, -75, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-87, -247, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-223, -274, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-405, -227, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-529, -319, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-613, -238, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-715, -256, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-756, -545, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-755, -578, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1301, -589, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1110, -563, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-987, -421, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1160, -630, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1257, -572, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(148, -343, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(271, -302, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(639, -350, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(910, -785, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(867, 260, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-500, 607, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-611, 643, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-759, 606, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-886, 681, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1038, 637, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1322, 693, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1409, 629, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1524, 646, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-1627, 541, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-627, 858, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-746, 869, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(-818, 853, 25));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(431, 74, 40));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(1347, 70, 40));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(874, 168, 40));
		AddSpawnPoint("f_orchard_32_4.Id4", "f_orchard_32_4", Rectangle(474, 905, 40));

		// 'Ferret_Bearer_Elite' GenType 13 Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(1353, 113, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(891, 248, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-434, -801, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-100, 1372, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-42, 162, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-70, 508, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-1019, -595, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-814, -541, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(613, 581, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(378, 93, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(1099, -965, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(908, -692, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-527, 246, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-974, 274, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-1406, 249, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(-757, 872, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(975, 38, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(975, 38, 25));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(431, 74, 40));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(1347, 70, 40));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(874, 168, 40));
		AddSpawnPoint("f_orchard_32_4.Id5", "f_orchard_32_4", Rectangle(474, 905, 40));

		// 'Ferret_Searcher' GenType 42 Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-183, 1370, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-835, 992, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1714, 870, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(9, 669, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-202, 1439, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-295, 1420, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-253, 1331, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-103, 1289, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-63, 1444, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(22, 1364, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(162, 1318, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(96, 1257, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(9, 1240, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(78, 1414, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(58, 732, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-115, 511, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(50, 549, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-184, 666, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-116, 718, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-49, 613, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(83, 635, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-135, 600, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-38, 537, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-776, 927, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-869, 879, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-924, 938, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-777, 1060, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-719, 971, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-680, 1121, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-611, 983, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-679, 860, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-754, 834, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1800, 914, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1642, 1022, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1714, 950, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1583, 892, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1749, 780, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1632, 806, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1561, 999, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(-1479, 876, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(431, 74, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(1347, 70, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(874, 168, 40));
		AddSpawnPoint("f_orchard_32_4.Id6", "f_orchard_32_4", Rectangle(474, 905, 40));

		// 'Ferret_Archer' Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id7", "f_orchard_32_4", Rectangle(-955, 229, 9999));

		// 'Ferret_Folk' Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id8", "f_orchard_32_4", Rectangle(-1388, 231, 9999));

		// 'Ferret_Vendor' Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id9", "f_orchard_32_4", Rectangle(470, 912, 9999));

		// 'Ferret_Bearer_Elite' Spawn Points
		AddSpawnPoint("f_orchard_32_4.Id10", "f_orchard_32_4", Rectangle(431, 74, 9999));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Zawra, "f_orchard_32_4", 1, Hours(2), Hours(4));
	}
}
