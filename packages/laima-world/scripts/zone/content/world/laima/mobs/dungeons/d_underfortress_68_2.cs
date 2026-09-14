//--- Melia Script -----------------------------------------------------------
// Sicarius 2F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_underfortress_68_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress682MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_underfortress_68_2.Id1", MonsterId.FD_Blindlem, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_2.Id2", MonsterId.FD_Tower_Of_Firepuppet_Black, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_2.Id3", MonsterId.FD_Chromadog, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_2.Id4", MonsterId.FD_Fire_Dragon_Purple, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_2.Id5", MonsterId.Rootcrystal_03, min: 10, max: 13, respawn: Minutes(1), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'FD_Blindlem' GenType 9 Spawn Points
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-935, 827, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-661, 819, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-760, 358, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1548, 351, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1437, 829, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1630, 590, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1251, 1260, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1292, 1466, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1051, 1496, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-581, 1437, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-480, 1785, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1536, 1062, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1702, 810, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1532, 1756, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1186, 1664, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-338, 1619, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-690, 1652, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-689, 560, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1332, 599, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-1055, 1202, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-239, 1407, 40));
		AddSpawnPoint("d_underfortress_68_2.Id1", "d_underfortress_68_2", Rectangle(-398, 1279, 40));

		// 'FD_Tower_Of_Firepuppet_Black' GenType 11 Spawn Points
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(477, -289, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(316, -151, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(-202, -476, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(18, -170, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(-215, -723, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(754, -808, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(226, -334, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(-120, -939, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(-42, -547, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(-204, -303, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(338, -678, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(209, -33, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(56, -859, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(658, -909, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(-21, -908, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(451, -905, 30));
		AddSpawnPoint("d_underfortress_68_2.Id2", "d_underfortress_68_2", Rectangle(1341, -239, 30));

		// 'FD_Chromadog' GenType 12 Spawn Points
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(1156, 20, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(1297, -265, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(991, -236, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(1389, -1579, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(1162, -1499, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(1408, -1355, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(-1182, -1381, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(-1135, -1648, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(-999, -1324, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(1326, 13, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(1283, -1254, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(1168, -262, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(550, -920, 100));
		AddSpawnPoint("d_underfortress_68_2.Id3", "d_underfortress_68_2", Rectangle(-843, -1521, 100));

		// 'FD_Fire_Dragon_Purple' GenType 13 Spawn Points
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-112, 787, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-212, -840, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(945, -41, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(552, 657, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(751, 658, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-877, 952, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(34, 728, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-639, 352, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-154, -228, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-1026, -952, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(346, -806, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(1243, 673, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(1042, 774, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(741, 510, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(880, 738, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(283, 675, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-183, 657, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-829, 702, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-584, 1745, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-822, -617, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(60, -746, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(682, 33, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(5, -329, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(191, -209, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(445, -154, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-1508, 708, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(960, 526, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-176, 478, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-945, 580, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(-44, 516, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(72, -926, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(540, -647, 40));
		AddSpawnPoint("d_underfortress_68_2.Id4", "d_underfortress_68_2", Rectangle(675, -764, 40));

		// 'Rootcrystal_03' GenType 14 Spawn Points
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(-714, -1, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(-749, 725, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(-1559, 781, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(-1129, 1440, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(-392, 1395, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(-34, 641, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(847, 632, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(431, -755, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(-157, -667, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(148, -111, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(1050, -95, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(1325, -1437, 50));
		AddSpawnPoint("d_underfortress_68_2.Id5", "d_underfortress_68_2", Rectangle(-1026, -1430, 50));
	}
}
