//--- Melia Script -----------------------------------------------------------
// Novaha Assembly Hall Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_abbey_64_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey641MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_abbey_64_1.Id1", MonsterId.Velwriggler_Mage_Red, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_1.Id2", MonsterId.Beeteroxia, min: 38, max: 50, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_1.Id3", MonsterId.Rootcrystal_01, min: 23, max: 30, respawn: Seconds(20), tendency: TendencyType.Peaceful);
		AddSpawner("d_abbey_64_1.Id4", MonsterId.LapeArcher, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_1.Id5", MonsterId.Carcashu_Green, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_1.Id6", MonsterId.Melatanun, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_1.Id7", MonsterId.Melatanun, min: 6, max: 7, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Velwriggler_Mage_Red' GenType 11 Spawn Points
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(416, -213, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(918, -1017, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1085, -911, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(973, -875, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(777, -724, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1003, -619, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1180, -744, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1086, -759, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(965, -772, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(995, -389, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(828, -266, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1082, -218, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(982, -212, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(869, -102, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1047, -50, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1158, -108, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(975, 122, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(843, 162, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(892, 285, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1008, 298, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1116, 222, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(1167, 99, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(209, -80, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(241, 86, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(465, 199, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(431, -23, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(234, -356, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-13, -289, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-51, -7, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(72, 166, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(134, 333, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-649, 715, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-894, 604, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-752, 366, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-574, 535, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-485, 359, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-345, 587, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-524, 733, 30));
		AddSpawnPoint("d_abbey_64_1.Id1", "d_abbey_64_1", Rectangle(-575, -2138, 30));

		// 'Beeteroxia' GenType 29 Spawn Points
		AddSpawnPoint("d_abbey_64_1.Id2", "d_abbey_64_1", Rectangle(95, -1698, 9999));

		// 'Rootcrystal_01' GenType 31 Spawn Points
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(664, 915, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(145, 902, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(222, 547, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(129, 1072, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-483, 1681, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-738, 866, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-328, 376, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-1043, 436, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-584, 474, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-1328, -846, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-822, -759, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-1121, -1195, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-1200, -1538, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-1102, -1838, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-850, -1668, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-180, -1703, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(114, -1681, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(467, -1748, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-281, -652, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-66, -1029, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(427, -1040, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(441, -647, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(139, -365, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(137, -8, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(-172, -27, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(840, -83, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(992, 223, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(924, -415, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(930, -780, 10));
		AddSpawnPoint("d_abbey_64_1.Id3", "d_abbey_64_1", Rectangle(1068, -1038, 10));

		// 'LapeArcher' GenType 33 Spawn Points
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(367, -598, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(260, -963, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(382, -910, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(322, -824, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(310, -729, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(433, -659, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(494, -775, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(545, -928, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(609, -702, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(449, -1073, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-416, -925, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-363, -740, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-207, -664, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-237, -789, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-97, -685, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-73, -805, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1, -668, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(2, -932, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-124, -934, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-169, -1092, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-270, -1050, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(13, -1005, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(425, -1259, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(384, -1134, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(364, -1410, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-186, -1204, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-226, -1281, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-154, -1387, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-216, -1407, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(240, -652, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-832, 466, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-986, 606, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-672, 713, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-855, 801, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-589, 507, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-504, 672, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-420, 425, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-414, 330, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1156, -1820, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1009, -1751, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1095, -1641, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1071, -1530, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-943, -1581, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-941, -1713, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-901, -1890, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-841, -1774, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1084, -1227, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1171, -1011, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1255, -777, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-991, -961, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-964, -819, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-821, -809, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-884, -961, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1093, -1091, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-1347, -916, 30));
		AddSpawnPoint("d_abbey_64_1.Id4", "d_abbey_64_1", Rectangle(-575, -2138, 30));

		// 'Shtayim_Mage' GenType 34 Spawn Points
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(40, -335, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-139, -148, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-185, -44, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-10, 136, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-18, -31, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(105, 141, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(129, -57, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(211, 201, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(218, -10, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(350, 127, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(321, -89, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(143, -185, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-14, -142, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-119, -283, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(97, -346, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(271, -370, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(399, -303, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(460, -169, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(453, -56, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(476, 113, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(452, 240, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(235, 322, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(220, 397, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(592, -47, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(639, 9, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-1195, -941, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-1185, -1115, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-1028, -980, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-1129, -744, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-903, -758, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-857, -945, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-947, -1112, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-1100, -1759, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-1014, -1629, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-1152, -1566, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-981, -1429, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-828, -1672, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-942, -1815, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-1226, -1686, 30));
		AddSpawnPoint("d_abbey_64_1.Id5", "d_abbey_64_1", Rectangle(-575, -2138, 30));

		// 'Sec_Spector_Gh' GenType 35 Spawn Points
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(26, -359, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(-190, -1687, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(418, -1717, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(9, -1580, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(313, -1877, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(141, -1732, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(-66, -1799, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(275, -1566, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(-113, -75, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(331, -134, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(400, -319, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(78, 190, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(269, 77, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(622, -25, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(847, -25, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(1062, -5, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(959, -327, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(1118, -245, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(1004, 226, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(889, -991, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(1051, -911, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(1147, -1000, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(982, -690, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(1135, -759, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(132, -110, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(60, 802, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(174, 623, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(443, 677, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(592, 810, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(339, 985, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(454, 1060, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(96, 1070, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(229, 853, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(240, -377, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(120, -1849, 30));
		AddSpawnPoint("d_abbey_64_1.Id6", "d_abbey_64_1", Rectangle(-575, -2138, 30));

		// 'Sec_Spector_Gh' GenType 36 Spawn Points
		AddSpawnPoint("d_abbey_64_1.Id7", "d_abbey_64_1", Rectangle(120, -1657, 9999));
	}
}
