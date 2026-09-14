//--- Melia Script -----------------------------------------------------------
// Maven Abbey Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_abbey_41_6'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey416MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_abbey_41_6.Id1", MonsterId.Rootcrystal_01, min: 12, max: 16, respawn: Seconds(5));
		AddSpawner("d_abbey_41_6.Id2", MonsterId.Ticen_Bow_Red, min: 12, max: 15);
		AddSpawner("d_abbey_41_6.Id3", MonsterId.Ticen_Red, min: 15, max: 20);
		AddSpawner("d_abbey_41_6.Id4", MonsterId.Ticen_Mage_Red, min: 12, max: 15);
		AddSpawner("d_abbey_41_6.Id5", MonsterId.Nuo_Red, min: 15, max: 20);
		AddSpawner("d_abbey_41_6.Id6", MonsterId.Ticen_Mage_Red, amount: 2);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 2 Spawn Points
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(-672, -1443, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(-723, -1040, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(-819, -722, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(-568, -820, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(-306, -879, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(-142, -1090, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(-16, -898, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(122, -736, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(280, -337, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(413, -651, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(614, -440, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(471, -92, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(795, -255, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(720, 81, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(-306, -170, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(39, 184, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(494, 499, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(709, -1052, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(1052, -845, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(1381, -442, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(1009, 430, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(1528, 262, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(1396, 802, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(1473, 570, 50));
		AddSpawnPoint("d_abbey_41_6.Id1", "d_abbey_41_6", Rectangle(999, 796, 50));

		// 'Ticen_Bow_Red' GenType 300 Spawn Points
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1037, 471, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1130, 753, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1449, 823, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1636, 611, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1649, 324, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1413, 188, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1171, 296, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1154, 593, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1370, 756, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1551, 610, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1538, 428, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1392, 345, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1256, 404, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(-574, -1317, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1164, -453, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1280, -577, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1414, -530, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1274, -297, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1295, -482, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(358, 387, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(268, 506, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(386, 643, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(504, 555, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(418, 490, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(528, 185, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(28, 107, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(-56, 204, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(132, 269, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(219, 17, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(848, -741, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(994, -880, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(1135, -793, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(987, -596, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(759, -552, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(999, -306, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(585, 83, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(604, -103, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(880, -212, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(641, -338, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(528, -482, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(297, -280, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(566, -85, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(239, -55, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(392, -151, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(182, -361, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(-74, -235, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(68, -393, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(313, -602, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(435, -750, 25));
		AddSpawnPoint("d_abbey_41_6.Id2", "d_abbey_41_6", Rectangle(362, -456, 25));

		// 'Ticen_Red' GenType 301 Spawn Points
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1070, 562, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1387, 826, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1569, 769, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1646, 508, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1476, 239, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1210, 257, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1041, 747, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1549, 892, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1010, 346, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1706, 301, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1409, 89, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-813, -1409, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-823, -1303, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-599, -1494, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-644, -1329, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1157, -446, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1259, -351, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1298, -532, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1395, -474, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(269, 457, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(412, 580, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(435, 345, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(538, 187, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(727, 8, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(835, -113, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(535, -108, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(716, -282, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(408, -217, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(584, -470, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(311, -375, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(430, -540, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(205, -467, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(375, -692, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-19, -302, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(240, -40, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(77, 105, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-350, -218, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-274, -123, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-22, 242, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(71, 235, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(703, -963, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(594, -1056, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(697, -1210, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(876, -779, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1017, -668, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(1053, -866, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(895, -662, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(158, -734, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-11, -935, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-119, -1081, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-185, -1122, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-265, -1031, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-209, -857, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-828, -978, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-815, -765, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-589, -716, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-676, -809, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-678, -959, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-643, -1084, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-525, -903, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-694, -1373, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-667, -1440, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-775, -1516, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-682, -1524, 25));
		AddSpawnPoint("d_abbey_41_6.Id3", "d_abbey_41_6", Rectangle(-715, -1165, 25));

		// 'Ticen_Mage_Red' GenType 302 Spawn Points
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(198, -454, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(332, -324, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(427, -154, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(725, 56, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(870, -156, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(739, -366, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(600, -503, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(372, -699, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1263, 456, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1294, 587, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1402, 637, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1514, 526, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1429, 476, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1365, 344, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1477, 399, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-446, -173, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-278, -71, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-186, -102, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-294, -206, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(500, -1075, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(632, -967, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(697, -901, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(780, -993, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(697, -1084, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(676, -1245, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(382, -529, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(688, -233, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(958, -686, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(975, -811, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1115, -727, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1264, -442, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1437, -397, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1315, -250, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1177, -363, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(38, 155, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(46, 255, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(317, 444, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(370, 579, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(508, 589, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(491, 188, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(638, -22, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(258, -115, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1090, 253, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1008, 640, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1287, 890, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1592, 850, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1722, 535, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1683, 199, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(1390, 131, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-271, -1028, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-61, -1039, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-162, -888, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-244, -831, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-117, -794, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-21, -899, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-591, -874, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-727, -999, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-856, -898, 25));
		AddSpawnPoint("d_abbey_41_6.Id4", "d_abbey_41_6", Rectangle(-686, -660, 25));

		// 'Nuo_Red' GenType 303 Spawn Points
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-852, -958, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-805, -788, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-613, -726, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-673, -958, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-364, -837, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-180, -1037, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-70, -850, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-47, -957, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(175, -741, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(321, -582, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(100, -442, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(315, -274, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(554, -53, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(732, -6, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(794, -247, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(618, -424, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(512, -521, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(635, -923, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(687, -1127, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(539, -1111, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(896, -702, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1042, -672, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1070, -859, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1182, -404, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1382, -477, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1331, -501, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(378, 462, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(380, 629, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(263, 507, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(59, 240, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-43, 126, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(140, 69, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-312, -90, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-396, -239, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(-71, -207, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1316, 483, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1024, 474, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1406, 822, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1656, 667, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1646, 435, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1512, 228, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1274, 265, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1101, 696, 25));
		AddSpawnPoint("d_abbey_41_6.Id5", "d_abbey_41_6", Rectangle(1453, 633, 25));

		// 'Ticen_Mage_Red' GenType 304 Spawn Points
		AddSpawnPoint("d_abbey_41_6.Id6", "d_abbey_41_6", Rectangle(-752, -1329, 25));
		AddSpawnPoint("d_abbey_41_6.Id6", "d_abbey_41_6", Rectangle(-735, -1453, 25));
		AddSpawnPoint("d_abbey_41_6.Id6", "d_abbey_41_6", Rectangle(-602, -1402, 25));
	}
}
