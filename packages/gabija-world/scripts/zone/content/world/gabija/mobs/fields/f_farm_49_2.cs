//--- Melia Script -----------------------------------------------------------
// Shaton Farm Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_farm_49_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm492MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_farm_49_2.Id1", MonsterId.Stub_Tree_Orange, min: 19, max: 25, respawn: Seconds(25), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_2.Id2", MonsterId.Stub_Tree_Orange, min: 19, max: 25, respawn: Seconds(25), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_2.Id3", MonsterId.Cyst, min: 22, max: 29, respawn: Seconds(25), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_2.Id4", MonsterId.Flying_Flog_Green, min: 8, max: 10, respawn: Seconds(25), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_2.Id5", MonsterId.Flying_Flog_Green, min: 12, max: 15, respawn: Seconds(25), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_2.Id6", MonsterId.Rootcrystal_01, min: 9, max: 12, respawn: Seconds(5), tendency: TendencyType.Peaceful);
		AddSpawner("f_farm_49_2.Id7", MonsterId.Pendinmire_Paviesa, amount: 1, respawn: Minutes(30), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Stub_Tree_Orange' GenType 3 Spawn Points
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(1601, -1158, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(1464, -1332, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(1415, -1472, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(1638, -1444, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(1299, -1343, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(1437, -1190, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(1712, -1322, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(1599, -1286, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(379, -568, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(435, -359, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(599, -283, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(583, -447, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(567, -563, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(521, -706, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(799, -588, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(828, -432, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(475, 278, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(407, 466, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(558, 392, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(612, 565, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(705, 435, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(654, 260, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(812, 333, 25));
		AddSpawnPoint("f_farm_49_2.Id1", "f_farm_49_2", Rectangle(737, 611, 25));

		// 'Stub_Tree_Orange' GenType 5 Spawn Points
		AddSpawnPoint("f_farm_49_2.Id2", "f_farm_49_2", Rectangle(-258, -25, 9999));

		// 'Cyst' GenType 6 Spawn Points
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(970, 1103, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(548, 1095, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(604, 1253, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(806, 1251, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(953, 1316, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-1411, -1611, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-1343, -1430, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-74, -1294, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-191, -1139, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-1108, -1393, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-1202, -1471, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-1289, -1260, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-1299, -1619, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-270, -1352, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(90, -1239, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(62, -969, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-200, -984, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(13, -1096, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(748, 1046, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(607, 1394, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(806, 1426, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(1606, 1202, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(1687, 1406, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(1806, 1221, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(1899, 1339, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(2043, 1264, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(2061, 1385, 30));
		AddSpawnPoint("f_farm_49_2.Id3", "f_farm_49_2", Rectangle(-1541, -1416, 30));

		// 'Flying_Flog_Green' GenType 8 Spawn Points
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-529, -375, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-308, -481, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-128, -388, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(11, -216, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-132, -25, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-418, -108, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-584, -114, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-318, 77, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(1656, 1269, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(1905, 1323, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(822, 1234, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(504, 1182, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(756, 1070, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-1392, -1479, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-1331, -1328, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-1152, -1529, 25));
		AddSpawnPoint("f_farm_49_2.Id4", "f_farm_49_2", Rectangle(-354, -293, 25));

		// 'Flying_Flog_Green' GenType 10 Spawn Points
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1371, 520, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1492, 668, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1668, 779, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1638, 585, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1983, 642, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1767, 513, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-856, 579, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-768, 516, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-973, 395, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-835, 411, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1124, -131, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1118, -245, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-959, -176, 25));
		AddSpawnPoint("f_farm_49_2.Id5", "f_farm_49_2", Rectangle(-1074, -23, 25));

		// 'Rootcrystal_01' GenType 40 Spawn Points
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(1544, -1310, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(507, -705, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(624, -409, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(695, 275, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(701, 608, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(698, 1041, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(982, 1313, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(1653, 1168, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(1921, 1313, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-796, 548, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-1415, 591, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-1807, 751, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-544, -237, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-139, -228, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-170, -1059, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-637, -1230, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-1141, -1423, 50));
		AddSpawnPoint("f_farm_49_2.Id6", "f_farm_49_2", Rectangle(-1456, -1519, 50));

		// 'Pendinmire_Paviesa' GenType 50 Spawn Points
		AddSpawnPoint("f_farm_49_2.Id7", "f_farm_49_2", Rectangle(1554, -1325, 250));
	}
}
