//--- Melia Script -----------------------------------------------------------
// Sjarejo Chamber Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_zachariel_79_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel791MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_zachariel_79_1.Id1", MonsterId.ERD_Sauga_S, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id2", MonsterId.ERD_Beetle, min: 17, max: 22, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id3", MonsterId.ERD_NightMaiden_Mage_Red, min: 12, max: 16, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id4", MonsterId.ERD_Moving_Trap, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id5", MonsterId.ERD_Wolf_Statue, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id6", MonsterId.ERD_Varv, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id7", MonsterId.ERD_Echad, min: 14, max: 18, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id8", MonsterId.ERD_Lemur, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id9", MonsterId.ERD_Chupacabra_Desert, min: 9, max: 12, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id10", MonsterId.ERD_Upent, min: 4, max: 5, respawn: Minutes(30), tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_1.Id11", MonsterId.Rootcrystal_05, min: 12, max: 15, respawn: Seconds(30), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'ERD_Sauga_S' GenType 902 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id1", "d_zachariel_79_1", Rectangle(192, 71, 9999));

		// 'ERD_Beetle' GenType 903 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-257, 1080, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-157, 1211, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(62, 1208, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-34, 1063, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-103, 853, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(98, 818, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(106, 1080, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(63, 923, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-74, 1456, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-67, 1666, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-179, 1877, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-166, 1976, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(108, 1950, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-73, 1793, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(88, 1783, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(336, 952, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-73, 1342, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-173, 815, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-365, 821, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-759, 846, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-863, 762, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-667, 506, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-768, 499, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-632, 711, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-543, 679, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-45, 1236, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-55, 2027, 40));
		AddSpawnPoint("d_zachariel_79_1.Id2", "d_zachariel_79_1", Rectangle(-25, 1908, 40));

		// 'ERD_NightMaiden_Mage_Red' GenType 904 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1541, 40, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1412, 316, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1344, 138, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1343, -173, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1095, 3, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1382, -53, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1240, 130, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1308, 406, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1063, 286, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1571, 145, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1264, -178, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1135, -122, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1149, 149, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1588, 230, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1674, -75, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(-1330, -27, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1504, 305, 40));
		AddSpawnPoint("d_zachariel_79_1.Id3", "d_zachariel_79_1", Rectangle(1581, -105, 40));

		// 'ERD_Moving_Trap' GenType 905 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(763, -37, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(812, -372, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(587, -311, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(677, -406, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(534, -501, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(600, -780, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(770, -740, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(683, -557, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(845, -630, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(252, -326, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(151, -1032, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(357, -1112, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(120, -1183, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(-20, -925, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(520, -669, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(186, -592, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(-130, -497, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(16, -228, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(9, -729, 40));
		AddSpawnPoint("d_zachariel_79_1.Id4", "d_zachariel_79_1", Rectangle(216, -972, 40));

		// 'ERD_Wolf_Statue' GenType 906 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(65, -391, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-167, -472, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-390, -564, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-571, -472, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-793, -482, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-571, -647, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-676, -480, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-759, -251, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-705, -79, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-732, 77, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-711, -631, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-240, -532, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(22, -593, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(278, -427, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(201, -533, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(227, -838, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(117, -869, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(31, -1087, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(193, -1123, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(318, -965, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(270, -1190, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(329, -1304, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(161, -699, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-25, -258, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-725, 301, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-850, 594, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-784, 852, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-727, 634, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-553, 572, 40));
		AddSpawnPoint("d_zachariel_79_1.Id5", "d_zachariel_79_1", Rectangle(-558, 742, 40));

		// 'ERD_Varv' GenType 907 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id6", "d_zachariel_79_1", Rectangle(-65, 61, 9999));

		// 'ERD_Echad' GenType 908 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1072, 103, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1193, 333, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1283, 107, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1528, 344, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1626, 151, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1589, -57, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1329, -32, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1528, 195, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1299, 351, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1280, 164, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1156, 177, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1217, -95, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1374, -189, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1625, 34, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(-1457, 300, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1139, -125, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1399, -134, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1304, -37, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1529, 17, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1359, 409, 40));
		AddSpawnPoint("d_zachariel_79_1.Id7", "d_zachariel_79_1", Rectangle(1448, 215, 40));

		// 'ERD_Lemur' GenType 909 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(76, -1815, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(248, -1851, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(128, -1713, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(246, -1632, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(33, -1618, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(141, -1428, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(-371, -26, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(-193, 245, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(14, -194, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(119, 146, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(10, 261, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(276, 259, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(239, 33, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(478, -33, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(627, 100, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(124, -54, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(-39, 60, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(-393, 205, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(-725, 87, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(-271, 84, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(355, 120, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(462, 246, 40));
		AddSpawnPoint("d_zachariel_79_1.Id8", "d_zachariel_79_1", Rectangle(-504, 71, 40));

		// 'ERD_Chupacabra_Desert' GenType 910 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(824, -1023, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(1067, -936, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(952, -1115, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(813, -1150, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(915, -1258, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(1083, -1208, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(934, -924, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(-515, -871, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(-655, -922, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(-745, -1051, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(-599, -1043, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(-598, -1217, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(-470, -1143, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(-439, -1000, 40));
		AddSpawnPoint("d_zachariel_79_1.Id9", "d_zachariel_79_1", Rectangle(1120, -1094, 40));

		// 'ERD_Upent' GenType 911 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id10", "d_zachariel_79_1", Rectangle(217, -436, 9999));

		// 'Rootcrystal_05' GenType 915 Spawn Points
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(1319, 202, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(1201, 22, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(1577, 38, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(1245, 209, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(722, 754, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-32, 1132, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-141, 1215, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-100, 946, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-160, 1925, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(0, 1865, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-812, 766, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-821, 574, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-599, 568, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-1337, 74, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-715, -577, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-24, -507, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(425, -439, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(726, -450, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(547, -689, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-452, -912, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-708, -1080, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(958, -1090, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(903, -1243, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(1080, -1221, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(1072, -944, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(846, -979, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(390, -1049, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(386, -568, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(4, -1057, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(94, -1625, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(215, -1732, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(139, -1893, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(2, 29, 50));
		AddSpawnPoint("d_zachariel_79_1.Id11", "d_zachariel_79_1", Rectangle(-3, 195, 50));
	}
}
