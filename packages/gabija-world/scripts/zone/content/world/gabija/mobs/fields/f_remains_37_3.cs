//--- Melia Script -----------------------------------------------------------
// Istora Ruins Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_remains_37_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains373MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_remains_37_3.Id1", MonsterId.Rootcrystal_04, min: 30, max: 40, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("f_remains_37_3.Id2", MonsterId.Hallowventor_Bow, min: 14, max: 18, tendency: TendencyType.Aggressive);
		AddSpawner("f_remains_37_3.Id3", MonsterId.Hallowventor_Mage, min: 18, max: 23, tendency: TendencyType.Aggressive);
		AddSpawner("f_remains_37_3.Id4", MonsterId.Gravegolem_Blue, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("f_remains_37_3.Id5", MonsterId.Gravegolem_Blue, min: 23, max: 30, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_04' GenType 3 Spawn Points
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-550, -1423, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-484, -1088, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-773, -992, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1002, -1137, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1443, -1248, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-671, -816, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-756, -306, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1176, -434, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1406, -511, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1708, -476, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-2020, -466, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-42, -497, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(433, -545, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(236, -989, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(145, -1310, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(569, -1434, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-581, -627, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1167, 209, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1449, 490, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1327, 1146, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-1702, 1401, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-2258, 1321, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-2732, 1541, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-2718, 1971, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-745, 470, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(-368, 186, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(104, 37, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(509, 134, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(770, -147, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(881, 409, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(1223, -15, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(1613, 109, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(2057, 26, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(2438, -162, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(750, 815, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(910, 1120, 40));
		AddSpawnPoint("f_remains_37_3.Id1", "f_remains_37_3", Rectangle(479, 1207, 40));

		// 'Hallowventor_Bow' GenType 19 Spawn Points
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1935, -576, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1698, -532, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1795, -342, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1606, 1337, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1292, 1200, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1188, 1412, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1588, 185, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1403, 13, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1162, 355, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1398, 434, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1413, 264, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-2737, 1523, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-2503, 1302, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-2503, 1517, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1490, -1237, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1356, -1396, 40));
		AddSpawnPoint("f_remains_37_3.Id2", "f_remains_37_3", Rectangle(-1321, -1011, 40));

		// 'Hallowventor_Mage' GenType 20 Spawn Points
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(83, -443, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(404, -621, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(450, -418, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(656, 38, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(966, 80, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(1113, 267, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(685, 301, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(-754, -982, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(-574, -1121, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(-533, -1001, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(-262, 225, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(-39, 44, 40));
		AddSpawnPoint("f_remains_37_3.Id3", "f_remains_37_3", Rectangle(919, -175, 40));

		// 'Gravegolem_Blue' GenType 37 Spawn Points
		AddSpawnPoint("f_remains_37_3.Id4", "f_remains_37_3", Rectangle(834, 84, 9999));

		// 'Gravegolem_Blue' GenType 40 Spawn Points
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-14, -486, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(249, -464, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(205, -579, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(493, -525, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1559, -1203, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1467, -1030, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1295, -1262, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1986, -449, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1804, -268, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1684, -595, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-735, -1141, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-675, -966, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-446, -1134, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(890, 53, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(835, 252, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(1218, -9, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(899, -226, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(568, 123, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(718, -72, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(779, 393, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-302, 146, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-219, 284, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-11, 139, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(609, 1126, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(701, 1305, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(878, 1133, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(604, 1291, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1539, 121, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1417, 394, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1299, 121, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1424, 236, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1399, 511, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1333, 1264, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1613, 1289, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1395, 1486, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(383, -286, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(55, -660, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-43, 65, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1612, 361, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1122, 1396, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1553, 1441, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1679, -479, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(-1700, -1103, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(400, -477, 20));
		AddSpawnPoint("f_remains_37_3.Id5", "f_remains_37_3", Rectangle(344, 1240, 20));
	}
}
