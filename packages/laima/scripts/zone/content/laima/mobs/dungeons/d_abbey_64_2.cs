//--- Melia Script -----------------------------------------------------------
// Novaha Annex Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_abbey_64_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey642MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_abbey_64_2.Id1", MonsterId.Lapfighter, min: 55, max: 75, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id2", MonsterId.Sec_Whip_Vine_Ra, min: 27, max: 35, respawn: Seconds(60), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id3", MonsterId.Sec_Whip_Vine_Ra, min: 27, max: 35, respawn: Seconds(60), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id4", MonsterId.Sec_Whip_Vine_Ra, min: 27, max: 35, respawn: Seconds(60), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id5", MonsterId.Sec_Whip_Vine_Ra, min: 27, max: 35, respawn: Seconds(60), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id6", MonsterId.Sec_Whip_Vine_Ra, min: 27, max: 35, respawn: Seconds(60), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id7", MonsterId.Rootcrystal_01, min: 23, max: 30, respawn: Seconds(20), tendency: TendencyType.Peaceful);
		AddSpawner("d_abbey_64_2.Id8", MonsterId.Crocoman, min: 40, max: 65, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id9", MonsterId.Lapezard, min: 40, max: 50, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id10", MonsterId.Sec_Whip_Vine_Ra, min: 27, max: 35, respawn: Seconds(60), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id11", MonsterId.Sec_Whip_Vine_Ra, min: 27, max: 35, respawn: Seconds(60), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id12", MonsterId.Sec_Whip_Vine_Ra, min: 27, max: 35, respawn: Seconds(60), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_2.Id13", MonsterId.LapeArcher, min: 30, max: 40, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Lapfighter' GenType 317 Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(70, 352, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-398, 374, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-220, 189, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-157, 412, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(112, 212, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(218, 431, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(288, 203, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-497, 384, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-621, 368, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-689, 500, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-960, 511, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-1105, 552, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-1251, 746, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-1029, 833, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-1040, 664, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-895, 744, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-910, 559, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-779, 496, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(315, 373, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-26, 906, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(145, 960, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-3, 1089, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-99, 1183, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(84, 1228, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(99, 1128, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(243, 1175, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(231, 941, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(177, 1063, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-95, -802, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-216, -673, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-179, -461, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-1565, -106, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-1406, 31, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-1576, 171, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(946, 27, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(853, 123, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(1143, 135, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(1101, -14, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(1125, -119, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(1192, -211, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(1228, 131, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(1005, 150, 35));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-125, -1298, 200));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(100, -1298, 200));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(441, -1279, 50));
		AddSpawnPoint("d_abbey_64_2.Id1", "d_abbey_64_2", Rectangle(-506, -1279, 50));

		// 'Sec_Whip_Vine_Ra' Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id2", "d_abbey_64_2", Rectangle(-1789, 70, 90));

		// 'Sec_Whip_Vine_Ra' Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id3", "d_abbey_64_2", Rectangle(513, -294, 90));

		// 'Sec_Whip_Vine_Ra' Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id4", "d_abbey_64_2", Rectangle(1448, -36, 90));

		// 'Sec_Whip_Vine_Ra' Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id5", "d_abbey_64_2", Rectangle(784, 709, 90));

		// 'Sec_Whip_Vine_Ra' Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id6", "d_abbey_64_2", Rectangle(-638, 487, 90));

		// 'Rootcrystal_01' GenType 350 Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(1681, 631, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(1550, 366, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(1057, 459, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(784, 706, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(426, 161, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(167, 478, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-306, 415, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-544, 66, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(0, -448, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-121, -766, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-17, -1058, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-268, -1356, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(161, -1532, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-21, -1820, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(305, -1228, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-1276, 657, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-970, 660, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-517, 1218, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-669, 2129, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-1070, 2263, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-1033, 2009, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(142, 884, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(90, 1206, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-25, 1975, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(262, 1930, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(199, 2286, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(1189, 1752, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(746, 1496, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-1374, 23, 10));
		AddSpawnPoint("d_abbey_64_2.Id7", "d_abbey_64_2", Rectangle(-1680, 62, 10));

		// 'Crocoman' GenType 351 Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-1076, 2196, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-915, 2297, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-852, 2148, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-1002, 2037, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-825, 1972, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-680, 2030, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-726, 2150, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-609, 2216, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-586, 1948, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-623, 1743, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-74, 1985, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-71, 2129, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(54, 2021, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(115, 2162, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(287, 1909, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(311, 2168, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-27, 2200, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(200, 2226, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-1226, 2347, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(531, 1419, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(599, 1596, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(680, 1481, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(799, 1637, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(874, 1488, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(978, 1614, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1068, 1472, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1127, 1600, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1173, 1447, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1211, 1776, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(130, 1853, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-45, 1858, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(33, 1939, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(974, 1445, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(462, 1507, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(887, 1031, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(876, 855, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1149, 924, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1374, 775, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1449, 590, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1457, 390, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1560, 577, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1587, 413, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1660, 308, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1783, 527, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1724, 663, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1688, 560, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1256, 466, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1111, 452, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(1012, 534, 30));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-125, -1298, 200));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(100, -1298, 200));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(441, -1279, 50));
		AddSpawnPoint("d_abbey_64_2.Id8", "d_abbey_64_2", Rectangle(-506, -1279, 50));

		// 'Lapezard' GenType 352 Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(624, 514, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(356, 464, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(150, 320, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(1, 208, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-164, 429, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-246, 243, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-300, 132, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-84, 1056, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(54, 1027, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-7, 1175, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(185, 1179, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(245, 1005, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(70, 879, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1583, -69, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1469, 34, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1598, 160, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1484, 173, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1392, 110, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1250, 86, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-848, 80, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-712, 38, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-577, 142, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-487, 228, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-380, 149, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-354, 13, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-165, 9, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(105, 88, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(393, -13, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(497, -149, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(501, -378, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(465, -337, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(45, -753, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-75, -698, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-109, -589, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-12, -497, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(70, -633, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-154, -503, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-180, -757, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1025, 502, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-942, 715, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1030, 591, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1143, 765, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-972, 773, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-1250, 476, 30));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-125, -1298, 200));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(100, -1298, 200));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(441, -1279, 50));
		AddSpawnPoint("d_abbey_64_2.Id9", "d_abbey_64_2", Rectangle(-506, -1279, 50));

		// 'Sec_Whip_Vine_Ra' GenType 353 Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1204, 867, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1225, 885, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1183, 876, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1238, 916, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1202, 897, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1225, 932, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1258, 921, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1218, 917, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1233, 945, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1236, 860, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1253, 880, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1296, 911, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1253, 896, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1257, 942, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1281, 878, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1259, 852, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1246, 831, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1292, 850, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1314, 888, 20));
		AddSpawnPoint("d_abbey_64_2.Id10", "d_abbey_64_2", Rectangle(1283, 904, 20));

		// 'Sec_Whip_Vine_Ra' GenType 354 Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-807, 815, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-785, 819, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-767, 821, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-745, 821, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-736, 840, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-761, 842, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-787, 846, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-807, 848, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-806, 867, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-790, 873, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-761, 865, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-741, 864, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-721, 865, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-685, 880, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-703, 876, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-713, 843, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-707, 818, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-683, 817, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-691, 829, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-694, 850, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-731, 885, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-662, 877, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-673, 857, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-666, 821, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-643, 819, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-647, 841, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-643, 869, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-631, 895, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-624, 873, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-627, 848, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-617, 821, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-606, 851, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-598, 887, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-598, 898, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-820, 831, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-828, 861, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-830, 891, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-853, 843, 20));
		AddSpawnPoint("d_abbey_64_2.Id11", "d_abbey_64_2", Rectangle(-852, 817, 20));

		// 'Sec_Whip_Vine_Ra' GenType 355 Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(107, 1779, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(120, 1781, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(137, 1779, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(137, 1762, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(122, 1763, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(109, 1760, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(95, 1754, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(73, 1756, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(70, 1739, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(89, 1733, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(102, 1732, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(120, 1738, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(135, 1737, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(133, 1722, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(121, 1717, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(104, 1711, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(90, 1707, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(77, 1714, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(59, 1719, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(123, 1695, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(137, 1697, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(105, 1687, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(49, 1738, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(57, 1755, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(28, 1756, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(87, 1691, 20));
		AddSpawnPoint("d_abbey_64_2.Id12", "d_abbey_64_2", Rectangle(39, 1723, 20));

		// 'LapeArcher' GenType 356 Spawn Points
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(75, 208, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-312, 266, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(329, 300, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(43, 1019, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(81, 1191, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(103, 1868, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(166, 2142, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-944, 2148, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-682, 2084, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-987, 609, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-1102, 615, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-1636, 36, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-1438, 49, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(514, -385, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(783, 549, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(596, 861, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(596, 1539, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(759, 1577, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(1107, 1540, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(1512, 685, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(1689, 556, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(1599, 326, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-544, 253, 20));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-125, -1298, 200));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(100, -1298, 200));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(441, -1279, 50));
		AddSpawnPoint("d_abbey_64_2.Id13", "d_abbey_64_2", Rectangle(-506, -1279, 50));
	}
}
