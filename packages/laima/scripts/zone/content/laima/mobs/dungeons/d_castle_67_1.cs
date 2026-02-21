//--- Melia Script -----------------------------------------------------------
// Topes Fortress 1F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_castle_67_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DCastle671MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_castle_67_1.Id1", MonsterId.Rootcrystal_03, min: 12, max: 15, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("d_castle_67_1.Id2", MonsterId.FD_Flamme_Priest, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_1.Id3", MonsterId.FD_Flamme_Archer, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_1.Id4", MonsterId.FD_Flak, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_1.Id5", MonsterId.FD_Flamme_Priest, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_1.Id6", MonsterId.FD_Flamme_Archer, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_1.Id7", MonsterId.FD_Flak, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_1.Id8", MonsterId.FD_Stoulet_Mage_Blue, min: 15, max: 20, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 4 Spawn Points
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-1194, -333, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-833, -737, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-326, -992, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-358, -458, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(582, -1095, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-1350, -883, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(247, -886, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(323, 142, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(1493, -1213, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(1614, 140, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(1200, 57, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(1433, -818, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(1465, 756, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(710, 698, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(224, 1003, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-675, -512, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(509, 911, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-443, 917, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-657, 730, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-1106, 271, 100));
		AddSpawnPoint("d_castle_67_1.Id1", "d_castle_67_1", Rectangle(-1339, 806, 100));

		// 'FD_Flamme_Priest' GenType 5 Spawn Points
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(-197, -1037, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(-209, -460, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(916, -499, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(1241, 51, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(1475, 758, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(-623, 996, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(-532, 661, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(-1230, 371, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(-1540, 1016, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(-1150, -213, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(-653, -552, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(323, 958, 100));
		AddSpawnPoint("d_castle_67_1.Id2", "d_castle_67_1", Rectangle(695, 654, 100));

		// 'FD_Flamme_Archer' GenType 6 Spawn Points
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(-1468, 943, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(-1086, 370, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(-1023, 194, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(-534, 895, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(-1109, -307, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(-752, -600, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(274, 1052, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(653, 747, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(-165, -515, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(-232, -995, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(312, 131, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(1582, -1212, 100));
		AddSpawnPoint("d_castle_67_1.Id3", "d_castle_67_1", Rectangle(1370, 788, 100));

		// 'FD_Flak' GenType 7 Spawn Points
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(-1108, -306, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(-749, -597, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(-231, -994, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(-318, -456, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(580, -917, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(1372, 863, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(653, 748, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(276, 1053, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(-453, 817, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(-624, 998, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(-1084, 372, 100));
		AddSpawnPoint("d_castle_67_1.Id4", "d_castle_67_1", Rectangle(-1466, 945, 100));

		// 'FD_Flamme_Priest' GenType 8 Spawn Points
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-1075, -146, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-1168, -475, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-1019, -650, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-612, -586, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-774, -816, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-380, -857, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-68, -1204, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(248, -881, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(535, -1202, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-355, -329, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-60, -586, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(281, 210, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(536, -111, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(732, -332, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(1100, -686, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(1246, -949, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(1586, -1185, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(1254, 189, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(1596, -105, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(1276, 958, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(1657, 576, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(401, 1162, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(197, 839, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(137, 701, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(720, 726, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-694, 936, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-413, 780, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-780, 801, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-815, 690, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-1016, 655, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-1517, 846, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(-1310, 995, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(80, 962, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(258, -58, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(425, -513, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(425, -681, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(426, -347, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(749, 46, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(914, 48, 40));
		AddSpawnPoint("d_castle_67_1.Id5", "d_castle_67_1", Rectangle(1082, 47, 40));

		// 'FD_Flamme_Archer' GenType 9 Spawn Points
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1282, -329, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-926, -297, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-706, -443, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-804, -864, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-247, -270, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-46, -858, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(258, -1241, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(528, -909, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(1314, -1221, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(1612, -891, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(768, -623, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(1044, -388, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(245, -106, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(583, 189, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-278, -625, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-97, -394, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(1270, -32, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(1618, 145, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(1277, 607, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(1622, 960, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(615, 440, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(677, 837, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(362, 1083, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(251, 983, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-454, 819, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-635, 757, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-536, 741, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-948, 267, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1095, 194, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1060, 303, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1511, 723, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1234, 1015, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1204, 370, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1133, -259, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-913, -609, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1509, 981, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(-1288, 772, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(429, -87, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(771, -1034, 40));
		AddSpawnPoint("d_castle_67_1.Id6", "d_castle_67_1", Rectangle(989, -1072, 40));

		// 'FD_Flak' GenType 10 Spawn Points
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1210, -213, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1040, -379, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-925, -494, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-690, -721, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-398, -1013, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-205, -920, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-192, -638, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-37, -358, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(170, -1058, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(633, -1034, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(1190, -1057, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(1444, -781, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(1438, -216, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(1401, 147, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(1220, 698, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(1498, 672, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(657, 48, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(230, -104, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(207, 1078, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(396, 966, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(579, 826, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(766, 627, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-740, 991, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-368, 899, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-811, 718, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-954, 293, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1052, 626, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-570, 560, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1086, 373, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1020, 196, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1509, 781, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1312, 829, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(1438, -611, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(1457, -479, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1504, 982, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1308, 703, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-313, -357, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-320, -1219, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(314, -1172, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(436, -837, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-1118, -546, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(-696, -463, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(539, -162, 40));
		AddSpawnPoint("d_castle_67_1.Id7", "d_castle_67_1", Rectangle(427, -207, 40));

		// 'FD_Stoulet_Mage_Blue' GenType 18 Spawn Points
		AddSpawnPoint("d_castle_67_1.Id8", "d_castle_67_1", Rectangle(-209, -464, 9999));
	}
}
