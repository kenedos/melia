//--- Melia Script -----------------------------------------------------------
// Laukyme Swamp Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_thorn_39_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn393MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_thorn_39_3.Id1", MonsterId.Rootcrystal_01, min: 9, max: 12, respawn: Seconds(5));
		AddSpawner("d_thorn_39_3.Id2", MonsterId.Stonacorn, min: 9, max: 11);
		AddSpawner("d_thorn_39_3.Id3", MonsterId.Loftlem_Green, min: 15, max: 20);
		AddSpawner("d_thorn_39_3.Id4", MonsterId.Cire_Mage, amount: 3);
		AddSpawner("d_thorn_39_3.Id5", MonsterId.Hepatica_Green, min: 15, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 5 Spawn Points
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(3038, -385, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(2636, -511, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(2249, -1123, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(2301, -1727, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(2319, -2104, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(1674, -498, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(1065, -1778, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(1518, -1976, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(659, -935, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(463, -171, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(232, 645, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(-33, 1083, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(-745, 564, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(-1024, 829, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(-1590, 323, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(-1995, -183, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(-1555, 1313, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(88, -210, 50));
		AddSpawnPoint("d_thorn_39_3.Id1", "d_thorn_39_3", Rectangle(-2146, 284, 50));

		// 'Stonacorn' GenType 18 Spawn Points
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(218, -939, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(398, -1086, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(645, -819, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(383, -839, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(132, -129, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(434, -99, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(343, -285, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(2044, -1089, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(2284, -970, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(2232, -1270, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(2479, -1198, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(2052, -862, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(-52, 623, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(347, 594, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(392, 1001, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(-135, 1053, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(191, 841, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(-289, 839, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(312, 69, 35));
		AddSpawnPoint("d_thorn_39_3.Id2", "d_thorn_39_3", Rectangle(741, -992, 35));

		// 'Loftlem_Green' GenType 19 Spawn Points
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-2223, -215, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-2024, -110, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-1868, 61, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-1961, 284, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-2244, 300, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-999, 537, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-824, 480, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-747, 723, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-946, 794, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-1718, 1190, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-1493, 1151, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-1683, 1342, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(-183, 865, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(10, 1031, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(235, 660, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(387, 915, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(302, -851, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(582, -742, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(498, -1133, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(682, -1014, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(501, -908, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2141, -1206, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2397, -1158, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2077, -1050, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2284, -1758, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2221, -944, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2157, -1794, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2186, -2012, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2642, -486, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2739, -299, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2899, -452, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2846, -695, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2993, -301, 30));
		AddSpawnPoint("d_thorn_39_3.Id3", "d_thorn_39_3", Rectangle(2673, -259, 30));

		// 'Cire_Mage' GenType 23 Spawn Points
		AddSpawnPoint("d_thorn_39_3.Id4", "d_thorn_39_3", Rectangle(258, -92, 5000));

		// 'Hepatica_Green' GenType 24 Spawn Points
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(332, -581, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-1490, 1069, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-1656, 1078, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-1895, 1232, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-1391, 1338, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-785, 902, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-679, 824, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(529, 686, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-732, 391, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-935, 372, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-1289, 578, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-259, 644, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-1959, 422, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-2561, 124, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-2417, 231, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-2197, -75, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(97, 1206, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(578, 789, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-1746, -58, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-1876, -250, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(225, -363, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(52, -14, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(70, -99, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(385, -52, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(378, -138, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(439, 473, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(455, 479, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(455, 479, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(366, 1140, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(176, 1182, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(275, 1160, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(136, 826, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(87, 746, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(0, 729, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(-32, 452, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(12, 376, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(82, 399, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(1928, -1179, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(1977, -1245, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(2916, -112, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(2924, -445, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(2747, -489, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(2527, -1208, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(2478, -527, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(2738, -170, 10));
		AddSpawnPoint("d_thorn_39_3.Id5", "d_thorn_39_3", Rectangle(2422, -851, 10));
	}
}
