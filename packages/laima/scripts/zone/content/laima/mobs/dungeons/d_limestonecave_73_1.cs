//--- Melia Script -----------------------------------------------------------
// Tavorh Cave Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_limestonecave_73_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DLimestonecave731MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_limestonecave_73_1.Id1", MonsterId.ERD_Loftlem, min: 15, max: 20, tendency: TendencyType.Peaceful);
		AddSpawner("d_limestonecave_73_1.Id2", MonsterId.ERD_Tanu, min: 15, max: 20, tendency: TendencyType.Peaceful);
		AddSpawner("d_limestonecave_73_1.Id3", MonsterId.ERD_Vesper, min: 30, max: 40, tendency: TendencyType.Peaceful);
		AddSpawner("d_limestonecave_73_1.Id4", MonsterId.ERD_Lizardman, min: 15, max: 20, tendency: TendencyType.Peaceful);
		AddSpawner("d_limestonecave_73_1.Id5", MonsterId.ERD_Groll, min: 15, max: 20, tendency: TendencyType.Peaceful);
		AddSpawner("d_limestonecave_73_1.Id6", MonsterId.ERD_Corylus, min: 15, max: 20, tendency: TendencyType.Peaceful);
		AddSpawner("d_limestonecave_73_1.Id7", MonsterId.ERD_Chafperor, min: 15, max: 20, tendency: TendencyType.Peaceful);
		AddSpawner("d_limestonecave_73_1.Id8", MonsterId.Rootcrystal_03, min: 15, max: 20, respawn: Seconds(30), tendency: TendencyType.Peaceful);
		AddSpawner("d_limestonecave_73_1.Id9", MonsterId.ERD_StonOrca, min: 27, max: 35, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'ERD_Loftlem' GenType 901 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(255, 420, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(409, 307, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(157, 532, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(85, 720, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(5, 874, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(154, 942, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(-54, 1005, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(-162, 1139, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(19, 1164, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(-112, 1335, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(86, 1404, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(134, 1289, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(285, 1280, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(249, 1081, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(424, 238, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(611, 347, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(288, 295, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(444, 358, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(911, 342, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(1102, 317, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(1154, 223, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(389, 532, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(460, 636, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id1", "d_limestonecave_73_1", Rectangle(561, 459, 35));

		// 'ERD_Tanu' GenType 902 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1709, 728, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1408, 844, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1575, 736, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1476, 904, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1558, 520, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1662, 611, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1247, 769, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1392, 694, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1761, 399, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1776, 305, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1725, 175, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1499, -136, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1322, 262, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1457, 275, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1641, 49, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1337, -122, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1384, 26, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1197, 123, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1748, -24, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1511, 100, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1415, 542, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1662, 870, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id2", "d_limestonecave_73_1", Rectangle(1406, -280, 35));

		// 'ERD_Vesper' GenType 903 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id3", "d_limestonecave_73_1", Rectangle(548, -386, 9999));

		// 'ERD_Lizardman' GenType 904 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(244, -830, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(440, -808, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(273, -653, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(527, -393, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(433, -529, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(690, -469, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(867, -470, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(1097, -302, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(1163, -494, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(1232, -359, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(1040, -444, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(334, -415, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(589, -260, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(270, -261, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(94, -107, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(119, -26, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(-99, 15, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(-297, -19, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(-384, 132, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(-63, 329, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(-251, 261, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(-226, 139, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(-61, 180, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(1169, -237, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(1123, -652, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(893, -380, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(497, -646, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id4", "d_limestonecave_73_1", Rectangle(360, -977, 35));

		// 'ERD_Groll' GenType 905 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-665, -747, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-592, -412, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-327, -222, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-751, -641, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-490, -714, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-460, -381, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-388, -806, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-424, -520, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-1008, -19, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-880, -19, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-865, 180, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-661, 220, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-790, 73, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-770, -73, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-620, 64, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-949, 90, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-572, -566, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-358, -652, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-1488, -200, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-1346, -103, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-1314, -248, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-1301, -446, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id5", "d_limestonecave_73_1", Rectangle(-1115, -389, 35));

		// 'ERD_Corylus' GenType 906 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1720, -589, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1535, -1226, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1405, -1096, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1415, -1317, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1478, -1470, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1311, -1416, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1155, -1295, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1325, -1233, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1590, -1395, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1828, -1228, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1789, -967, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1673, -815, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1826, -697, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1703, -491, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1445, -800, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1891, -572, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1868, -864, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1797, -1031, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1222, -1166, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1500, -405, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1373, -253, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1444, -126, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id6", "d_limestonecave_73_1", Rectangle(-1181, -313, 35));

		// 'ERD_Chafperor' GenType 907 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1160, 543, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1074, 633, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-890, 652, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-992, 547, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-940, 440, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1314, 537, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1505, 441, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1670, 459, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1574, 615, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1454, 716, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1309, 720, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1051, 732, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-776, 619, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-746, 449, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-755, 313, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1022, 352, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-841, 759, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(-1439, 552, 35));
		AddSpawnPoint("d_limestonecave_73_1.Id7", "d_limestonecave_73_1", Rectangle(1482, 137, 35));

		// 'Rootcrystal_03' GenType 909 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(310, 505, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(572, 545, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(425, 270, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-153, 1248, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(54, 1283, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(249, 1173, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(70, 956, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1307, 800, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1591, 851, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1437, 617, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1649, 698, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1274, 190, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1285, 23, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1365, 129, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1484, 18, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1493, -85, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1678, -8, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1040, -295, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1041, -372, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1068, -518, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1176, -393, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(1186, -574, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(694, -306, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(510, -335, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(335, -535, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(667, -419, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(438, -616, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(355, -707, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(363, -868, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(250, -782, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-50, 106, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-151, -16, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-234, 78, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-120, 209, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-598, -518, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-612, -664, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-538, -744, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-379, -739, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-421, -573, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-838, 41, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-715, -5, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1045, 480, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-901, 501, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-772, 680, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-914, 699, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-964, 614, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1129, 615, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1476, 489, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1641, 561, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1418, 662, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1517, 502, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1467, -260, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1281, -172, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1211, -360, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1317, -397, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1571, -565, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1748, -539, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1835, -661, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1842, -786, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1678, -732, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1558, -751, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1595, -1212, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1431, -1121, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1482, -1324, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1439, -1389, 100));
		AddSpawnPoint("d_limestonecave_73_1.Id8", "d_limestonecave_73_1", Rectangle(-1244, -1298, 100));

		// 'ERD_StonOrca' GenType 910 Spawn Points
		AddSpawnPoint("d_limestonecave_73_1.Id9", "d_limestonecave_73_1", Rectangle(125, -82, 9999));
	}
}
