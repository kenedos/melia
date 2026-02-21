//--- Melia Script -----------------------------------------------------------
// Nevellet Quarry 2F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_cmine_66_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine662MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_cmine_66_2.Id1", MonsterId.Rootcrystal_03, min: 14, max: 18, respawn: Seconds(5), tendency: TendencyType.Peaceful);
		AddSpawner("d_cmine_66_2.Id2", MonsterId.FD_Blok_Archer, min: 18, max: 23, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_2.Id3", MonsterId.FD_Blok_Wizard, min: 18, max: 23, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_2.Id4", MonsterId.FD_Kenol, min: 25, max: 33, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_2.Id5", MonsterId.FD_Pawndel, min: 42, max: 55, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_2.Id6", MonsterId.FD_Kenol, min: 50, max: 66, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_2.Id7", MonsterId.FD_Blok_Archer, min: 18, max: 23, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_2.Id8", MonsterId.FD_Pawndel, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_2.Id9", MonsterId.FD_Blok_Wizard, min: 18, max: 23, tendency: TendencyType.Aggressive);
		AddSpawner("d_cmine_66_2.Id10", MonsterId.FD_Pawndel, min: 8, max: 10, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 2 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(2060, -272, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1803, 37, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(2157, 168, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(2000, -864, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1607, -1049, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1224, -653, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1437, -198, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1373, 343, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1388, 798, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1812, 1066, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(2073, 657, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(2141, -3535, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1944, -3217, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1719, -3486, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(1596, -3021, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(2292, -3050, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(109, -851, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(3, -1162, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-267, -850, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-565, -1102, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-559, -354, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-366, -100, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-581, 260, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-597, 843, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-386, 1129, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-124, 849, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(122, 996, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-1177, -198, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-1062, 256, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-1510, 241, 50));
		AddSpawnPoint("d_cmine_66_2.Id1", "d_cmine_66_2", Rectangle(-1603, -240, 50));

		// 'FD_Blok_Archer' GenType 49 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1900, -292, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(2050, 187, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1869, -865, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1510, -743, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1449, -494, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1465, 104, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1373, 421, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1436, 946, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1878, 790, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1522, -1103, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1700, -804, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1156, -764, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1118, -31, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1312, -44, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1246, 617, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1280, 903, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1351, -527, 30));
		AddSpawnPoint("d_cmine_66_2.Id2", "d_cmine_66_2", Rectangle(1689, -855, 30));

		// 'FD_Blok_Wizard' GenType 50 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(1875, -1092, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(1203, -514, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(1400, -120, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(1238, 340, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(1517, 727, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(2065, 1058, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1517, -307, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1718, 304, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1692, -41, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1068, 240, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1318, -306, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(2123, -724, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(1379, -1045, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(1676, 1081, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(2197, 641, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(2161, -146, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(1858, 98, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1386, -113, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1330, 236, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1102, -9, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-997, -228, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-1365, 49, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-585, -137, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-446, 72, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-646, 100, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-525, -365, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-742, -95, 30));
		AddSpawnPoint("d_cmine_66_2.Id3", "d_cmine_66_2", Rectangle(-938, -5, 30));

		// 'FD_Kenol' GenType 51 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1368, -92, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(1577, -3460, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(1509, -3175, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(1983, -2966, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(1857, -3172, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(1929, -3584, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(2306, -3180, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(2179, -3497, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(2171, -2861, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(1670, -2916, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1710, -275, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1686, 59, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1523, 241, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1531, -179, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1374, -318, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1078, -269, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1033, -13, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1280, 320, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1051, 242, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1746, 311, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1170, -98, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-1194, 116, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-845, -11, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-605, 5, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-493, 212, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-356, 109, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-503, -221, 30));
		AddSpawnPoint("d_cmine_66_2.Id4", "d_cmine_66_2", Rectangle(-714, -283, 30));

		// 'FD_Pawndel' GenType 52 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1717, -748, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1200, -705, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1459, -396, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1215, -26, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1340, 765, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1193, 1063, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(2060, 874, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1834, 601, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(2002, 60, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1804, -185, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1624, -80, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(2133, -1047, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1531, -1103, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1212, -894, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(-557, -354, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(-643, 136, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(-553, 323, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(-317, 348, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(-438, 82, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(-384, -239, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(-617, -93, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1327, -521, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1583, -834, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1961, -987, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(2017, -762, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(2030, -195, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1414, 2, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1472, 452, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1255, 543, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1443, 194, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1436, 927, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1845, 919, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1935, -849, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1084, -278, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1703, 1014, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1119, -1026, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1286, -1167, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1481, -964, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1735, -1017, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(2040, -1162, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1540, 742, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1781, 793, 30));
		AddSpawnPoint("d_cmine_66_2.Id5", "d_cmine_66_2", Rectangle(1977, 735, 30));

		// 'FD_Kenol' GenType 53 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id6", "d_cmine_66_2", Rectangle(982, -22, 2000));

		// 'FD_Blok_Archer' GenType 54 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-223, -1001, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-562, -1161, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-568, -891, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(10, -882, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(144, -851, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-138, -1139, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(144, -1176, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(95, -1040, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-130, -898, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-512, -1036, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-435, -1203, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-267, -1158, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-353, -991, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-318, -832, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-711, -1085, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-759, -1053, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-1124, -1043, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-985, -941, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-938, -1058, 30));
		AddSpawnPoint("d_cmine_66_2.Id7", "d_cmine_66_2", Rectangle(-682, -983, 30));

		// 'FD_Pawndel' GenType 55 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(-377, -1032, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(62, -1007, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(-567, -1162, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(-589, -979, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(-412, -843, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(-236, -1008, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(-92, -1193, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(-58, -868, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(156, -869, 35));
		AddSpawnPoint("d_cmine_66_2.Id8", "d_cmine_66_2", Rectangle(156, -1187, 35));

		// 'FD_Blok_Wizard' GenType 56 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-592, 775, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-451, 1061, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-289, 831, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-123, 1077, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(91, 818, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(145, 1056, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-575, 912, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-414, 860, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-240, 1018, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(2, 779, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(51, 946, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-634, 1102, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(50, 1193, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-129, 853, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-741, 916, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-967, 809, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-1035, 962, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-888, 935, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(-1140, 835, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(254, 868, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(177, 737, 30));
		AddSpawnPoint("d_cmine_66_2.Id9", "d_cmine_66_2", Rectangle(1566, 778, 30));

		// 'FD_Pawndel' GenType 57 Spawn Points
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(-446, 944, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(21, 952, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(-532, 726, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(-601, 937, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(-335, 1119, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(-237, 904, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(-52, 765, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(171, 791, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(216, 1045, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(-77, 1086, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(103, 1171, 30));
		AddSpawnPoint("d_cmine_66_2.Id10", "d_cmine_66_2", Rectangle(-543, 1154, 30));
	}
}
