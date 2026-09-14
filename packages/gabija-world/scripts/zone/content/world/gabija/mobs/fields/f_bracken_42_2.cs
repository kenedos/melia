//--- Melia Script -----------------------------------------------------------
// Khamadon Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_bracken_42_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken422MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_bracken_42_2.Id1", MonsterId.Rootcrystal_03, min: 10, max: 13, respawn: Seconds(5), tendency: TendencyType.Peaceful);
		AddSpawner("f_bracken_42_2.Id2", MonsterId.Duckey_Red, min: 6, max: 8, tendency: TendencyType.Aggressive);
		AddSpawner("f_bracken_42_2.Id3", MonsterId.Moglan_Blue, min: 6, max: 8, tendency: TendencyType.Aggressive);
		AddSpawner("f_bracken_42_2.Id4", MonsterId.Beetow_Blue, min: 9, max: 12, tendency: TendencyType.Peaceful);
		AddSpawner("f_bracken_42_2.Id5", MonsterId.Beetow_Blue, min: 12, max: 15, tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 2 Spawn Points
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(-1573, 129, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(-1250, 95, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(-981, -255, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(-453, -632, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(212, -891, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(495, -853, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(758, -1121, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(1243, -818, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(1408, -662, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(-314, 159, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(495, 222, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(426, -161, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(1025, -246, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(1334, 25, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(-88, 746, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(164, 861, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(705, 790, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(985, 1092, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(1341, 1493, 50));
		AddSpawnPoint("f_bracken_42_2.Id1", "f_bracken_42_2", Rectangle(1479, 1242, 50));

		// 'Duckey_Red' GenType 101 Spawn Points
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(248, -21, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(335, -160, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(284, 171, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(434, 10, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(598, -39, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(494, 215, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(970, -170, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(953, -282, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(1112, -202, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(1270, 26, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(1310, -173, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(1244, -199, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(468, 77, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(498, -99, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(1375, -95, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(1494, -35, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(1476, -170, 35));
		AddSpawnPoint("f_bracken_42_2.Id2", "f_bracken_42_2", Rectangle(1158, -90, 35));

		// 'Moglan_Blue' GenType 103 Spawn Points
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(654, 917, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(816, 869, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(883, 1127, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(925, 821, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(912, 935, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1093, 1027, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1336, 1417, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1383, 1255, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1655, 1348, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1439, 1537, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1708, 1492, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1567, 1671, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1511, 1320, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1561, 1461, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1003, 1037, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(1026, 922, 50));
		AddSpawnPoint("f_bracken_42_2.Id3", "f_bracken_42_2", Rectangle(829, 1011, 50));

		// 'Beetow_Blue' GenType 104 Spawn Points
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-511, -686, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-575, -780, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-354, -732, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-326, -850, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(245, -1050, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(297, -894, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(383, -1020, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(595, -1152, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(678, -925, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(1321, -762, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(1463, -739, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(1546, -869, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(1378, -974, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(1222, -903, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(412, -798, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(675, -1090, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(501, -957, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(1442, -829, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-704, -636, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-902, -369, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-953, -607, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-764, -467, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-496, -538, 30));
		AddSpawnPoint("f_bracken_42_2.Id4", "f_bracken_42_2", Rectangle(-190, -797, 30));

		// 'Beetow_Blue' GenType 105 Spawn Points
		AddSpawnPoint("f_bracken_42_2.Id5", "f_bracken_42_2", Rectangle(296, 79, 9999));
	}
}
