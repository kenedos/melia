//--- Melia Script -----------------------------------------------------------
// Netanmalek Mausoleum Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_zachariel_79_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel792MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_zachariel_79_2.Id1", MonsterId.ERD_Wolf_Statue_Mage, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id2", MonsterId.ERD_Tiny_Blue, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id3", MonsterId.ERD_Echad_Bow, min: 12, max: 16, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id4", MonsterId.ERD_Colimen_Brown, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id5", MonsterId.ERD_Rubblem, min: 14, max: 18, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id6", MonsterId.ERD_Blindlem, min: 14, max: 18, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id7", MonsterId.ERD_Tiny_Mage_Brown, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id8", MonsterId.ERD_Dog_Of_King, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id9", MonsterId.ERD_Schlesien_Darkmage, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id10", MonsterId.ERD_Schlesien_Claw, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id11", MonsterId.ERD_Glyquare, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id12", MonsterId.ERD_Schlesien_Heavycavarly, min: 4, max: 5, respawn: Minutes(30), tendency: TendencyType.Aggressive);
		AddSpawner("d_zachariel_79_2.Id13", MonsterId.Rootcrystal_05, min: 15, max: 20, respawn: Seconds(30), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'ERD_Wolf_Statue_Mage' GenType 901 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-41, -2439, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-23, -2212, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-59, -2154, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-65, -1982, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-81, -1881, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-5, -1746, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-158, -1727, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-114, -1535, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-94, -1619, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(108, -1617, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-71, -1414, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-72, -1053, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-64, -1030, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-9, -930, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-154, -826, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(26, -807, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-28, -1741, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(147, -1676, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(229, -1678, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-289, -1723, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-376, -1653, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-45, -2308, 40));
		AddSpawnPoint("d_zachariel_79_2.Id1", "d_zachariel_79_2", Rectangle(-28, -1301, 40));

		// 'ERD_Tiny_Blue' GenType 902 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-1192, -1711, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-653, -1720, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-986, -1549, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-945, -1766, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-305, -1697, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-833, -1669, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-478, -1698, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-180, -1577, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-105, -1704, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(5, -1755, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-37, -1592, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(198, -1671, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(449, -1650, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(652, -1702, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(282, -1713, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(936, -1649, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(1071, -1694, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(1206, -1758, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(836, -1769, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(1199, -1590, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-609, -1657, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-1205, -1888, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-1061, -1927, 40));
		AddSpawnPoint("d_zachariel_79_2.Id2", "d_zachariel_79_2", Rectangle(-1140, -1508, 40));

		// 'ERD_Echad_Bow' GenType 903 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(-1234, -628, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(-1027, -690, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(-952, -527, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(-659, -607, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(-445, -518, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(-349, -671, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(-172, -596, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(-85, -783, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(38, -467, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(36, -630, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(247, -633, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(337, -554, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(421, -629, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(557, -531, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(654, -630, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(720, -519, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(810, -637, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(912, -560, 40));
		AddSpawnPoint("d_zachariel_79_2.Id3", "d_zachariel_79_2", Rectangle(1027, -581, 40));

		// 'ERD_Colimen_Brown' GenType 904 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(913, -739, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(989, -477, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(974, -375, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1289, -354, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1136, -568, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1249, -671, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1089, -331, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1088, -869, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(997, -912, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1033, -1005, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(981, -1164, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1053, -1315, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1021, -1470, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(838, -1601, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(940, -1748, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1166, -1785, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1055, -1619, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1265, -1477, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(929, -1941, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1236, -1614, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1189, -1680, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(983, -1059, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1027, -604, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(844, -541, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1249, -462, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(890, -1840, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(942, -1504, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(973, -1240, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1011, -1650, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(980, -999, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(887, -782, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(829, -626, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(972, -629, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(888, -410, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1146, -465, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1153, -663, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(771, -1662, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(849, -1710, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1145, -1566, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1092, -1770, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(1097, -557, 40));
		AddSpawnPoint("d_zachariel_79_2.Id4", "d_zachariel_79_2", Rectangle(33, -1469, 40));

		// 'ERD_Rubblem' GenType 905 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1295, -642, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1079, -556, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1222, -487, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1166, -307, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-969, -396, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-938, -725, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1133, -791, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1130, -976, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1053, -1047, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1120, -1222, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1052, -1400, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1200, -1504, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1094, -1678, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1281, -1783, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1202, -1984, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1063, -1936, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1136, -1852, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-919, -1704, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-938, -1583, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1261, -1618, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-887, -574, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1127, -627, 40));
		AddSpawnPoint("d_zachariel_79_2.Id5", "d_zachariel_79_2", Rectangle(-1083, -1138, 40));

		// 'ERD_Blindlem' GenType 906 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-4, -40, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-68, -250, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(0, -225, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-84, -81, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-9, 227, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-16, 378, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-85, 125, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-87, 524, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(13, 656, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-63, 686, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-19, 819, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-45, 1040, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-45, 951, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-57, 1226, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-19, 1318, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-77, 1412, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(-175, 735, 40));
		AddSpawnPoint("d_zachariel_79_2.Id6", "d_zachariel_79_2", Rectangle(116, 690, 40));

		// 'ERD_Tiny_Mage_Brown' GenType 907 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-1152, 508, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-868, 588, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-1177, 665, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-1088, 935, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-1295, 797, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-656, 718, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-544, 647, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-362, 751, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-182, 615, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-30, 809, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-2, 573, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(206, 718, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(572, 688, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(653, 756, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(809, 690, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(875, 955, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(1015, 743, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(1055, 568, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(1311, 722, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(1401, 805, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(1377, 513, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(1189, 625, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(859, 515, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(1128, 864, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(1442, 1045, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-1107, 811, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-924, 853, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-1018, 699, 40));
		AddSpawnPoint("d_zachariel_79_2.Id7", "d_zachariel_79_2", Rectangle(-1004, 543, 40));

		// 'ERD_Dog_Of_King' GenType 908 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(398, 1283, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(402, 1634, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(372, 1477, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(566, 1389, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(864, 1624, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(903, 1343, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1161, 1447, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(945, 1739, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(438, 2020, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(750, 2207, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1337, 2156, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1325, 1911, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1459, 1404, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1604, 1319, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1713, 1572, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1571, 1522, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1363, 1572, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1122, 1589, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1447, 2317, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1669, 2368, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1649, 2128, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1551, 1908, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(1309, 1378, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(869, 1109, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(730, 1369, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(591, 1500, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(611, 1242, 40));
		AddSpawnPoint("d_zachariel_79_2.Id8", "d_zachariel_79_2", Rectangle(756, 1558, 40));

		// 'ERD_Schlesien_Darkmage' GenType 909 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-2454, 1625, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1877, 1945, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1717, 1755, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1667, 1924, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1483, 1956, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1251, 2020, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1278, 1790, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1086, 1367, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1009, 2082, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-745, 2036, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-836, 1687, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-635, 1889, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-380, 1904, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-384, 2178, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-227, 2150, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-185, 1978, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-67, 2000, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(47, 2047, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-95, 1864, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(0, 1720, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-98, 1635, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-911, 1933, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-746, 1826, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-852, 2303, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-2706, 1934, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-2488, 1820, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-2454, 1986, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-2375, 1894, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-2225, 2005, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-2073, 1895, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1906, 1774, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1095, 1882, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1085, 1561, 40));
		AddSpawnPoint("d_zachariel_79_2.Id9", "d_zachariel_79_2", Rectangle(-1084, 1710, 40));

		// 'ERD_Schlesien_Claw' GenType 910 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id10", "d_zachariel_79_2", Rectangle(598, 1447, 9999));

		// 'ERD_Glyquare' GenType 911 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id11", "d_zachariel_79_2", Rectangle(-1042, 1966, 9999));

		// 'ERD_Schlesien_Heavycavarly' GenType 912 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id12", "d_zachariel_79_2", Rectangle(407, 698, 9999));

		// 'Rootcrystal_05' GenType 913 Spawn Points
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-2472, 1926, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1104, 1914, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-794, 1884, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-799, 1752, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-16, 1911, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-39, 1819, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1148, 773, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1164, 621, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-974, 661, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-965, 788, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(427, 1372, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(450, 1544, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(546, 1452, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(654, 1380, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1431, 1487, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1609, 1516, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1615, 1371, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1476, 1341, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(930, 816, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1084, 810, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(949, 659, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1077, 628, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(985, -403, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1024, -540, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1160, -584, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(921, -756, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-22, -588, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-214, -543, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(146, -597, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1248, -471, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1189, -721, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1060, -387, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1179, -1511, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1094, -1689, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-1171, -1884, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-28, -1691, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-192, -1692, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-49, -1843, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1213, -1572, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1227, -1653, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1179, -1769, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(1038, -1668, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(872, -1611, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(887, -1747, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-47, -2570, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-30, -2751, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-38, -2881, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-28, -3026, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-44, -2368, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-23, 137, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-31, -53, 50));
		AddSpawnPoint("d_zachariel_79_2.Id13", "d_zachariel_79_2", Rectangle(-425, 706, 50));
	}
}
