//--- Melia Script -----------------------------------------------------------
// Grand Yard Mesa Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_tableland_71'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland71MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_tableland_71.Id1", MonsterId.Hohen_Ritter_Purple, min: 23, max: 30, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id2", MonsterId.Cronewt_Bow_Blue, min: 8, max: 10, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id3", MonsterId.Hohen_Barkle_Blue, min: 8, max: 10, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id4", MonsterId.Tiny_Blue, min: 8, max: 10, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id5", MonsterId.Rootcrystal_03, min: 27, max: 35, respawn: Seconds(30));
		AddSpawner("f_tableland_71.Id6", MonsterId.Hohen_Ritter_Purple, amount: 3, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id7", MonsterId.Hohen_Barkle_Blue, amount: 2, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id8", MonsterId.Tiny_Blue, amount: 1, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id9", MonsterId.Cronewt_Bow_Blue, amount: 1, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id10", MonsterId.Tiny_Blue, min: 6, max: 7, respawn: Seconds(25));
		AddSpawner("f_tableland_71.Id11", MonsterId.Cronewt_Bow_Blue, min: 3, max: 4, respawn: Minutes(1));
		AddSpawner("f_tableland_71.Id12", MonsterId.Hohen_Ritter_Purple, min: 8, max: 10, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Hohen_Ritter_Purple' GenType 1 Spawn Points
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(486, -193, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(543, 34, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(271, -169, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-858, -160, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-649, -100, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-787, 67, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-465, 124, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-238, -557, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(29, -579, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-121, -684, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-252, -366, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-1219, 25, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-1407, -138, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-1540, 83, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-1401, 85, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-291, 752, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-236, -70, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-199, 288, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-332, 586, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-264, 488, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-640, 1057, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(-858, 1305, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(232, 482, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(296, 738, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(504, 334, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(801, 451, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(1145, 634, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(1289, 680, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(1344, 484, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(1328, 82, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(1227, -56, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(1345, -171, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(1427, -5, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(1076, 459, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(713, -121, 25));
		AddSpawnPoint("f_tableland_71.Id1", "f_tableland_71", Rectangle(482, -390, 25));

		// 'Cronewt_Bow_Blue' GenType 2 Spawn Points
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(-1469, 5, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(-331, 1044, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(-774, 1228, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(30, 783, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(-442, 661, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(-373, 531, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(-265, 365, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(-501, 871, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(49, 621, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(166, 439, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(356, 534, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(418, 858, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(354, 221, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(715, 499, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(663, 300, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(716, 114, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(353, -29, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(320, -328, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(455, -375, 25));
		AddSpawnPoint("f_tableland_71.Id2", "f_tableland_71", Rectangle(680, -312, 25));

		// 'Hohen_Barkle_Blue' GenType 3 Spawn Points
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-748, -169, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-782, 6, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-1301, -28, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-1604, 0, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-1479, -82, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-990, 63, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-786, 169, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-651, -138, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-466, -97, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-344, 140, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-516, 157, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-392, 22, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-928, -205, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-979, -644, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-987, -861, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-839, -938, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-810, -705, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-914, -805, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-739, -862, 25));
		AddSpawnPoint("f_tableland_71.Id3", "f_tableland_71", Rectangle(-877, -444, 25));

		// 'Tiny_Blue' GenType 4 Spawn Points
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(-886, -769, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(-920, -941, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(-1287, -94, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(-1316, 94, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(-627, -46, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(-208, 1037, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(-256, -281, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(291, -135, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(664, -307, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1183, -88, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1331, -137, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1343, 113, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1306, -8, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1401, 217, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1447, 485, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1244, 490, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1040, 384, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1073, 585, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1355, 766, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1268, 647, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(851, 412, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(667, 502, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(661, 244, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(365, 682, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(531, 425, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(391, 273, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(250, 483, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(257, 746, 25));
		AddSpawnPoint("f_tableland_71.Id4", "f_tableland_71", Rectangle(1460, 777, 25));

		// 'Rootcrystal_03' GenType 9 Spawn Points
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-269, -1156, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-663, -888, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-991, -919, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-1012, -679, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-944, -256, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-876, 116, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-407, -116, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-420, 201, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-285, -440, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-55, -630, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-309, 542, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-475, 884, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-538, 672, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-121, 920, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(149, 711, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(221, 464, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(403, 213, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(728, 128, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(555, -49, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(303, -170, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(537, -416, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(589, 465, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(961, 391, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(1182, 503, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(1382, 692, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(1285, -96, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(1703, 619, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(1802, 460, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-666, 1117, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-825, 704, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-1148, 677, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-1020, 566, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-1542, -43, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-1243, 123, 40));
		AddSpawnPoint("f_tableland_71.Id5", "f_tableland_71", Rectangle(-1257, -177, 40));

		// 'Hohen_Ritter_Purple' GenType 37 Spawn Points
		AddSpawnPoint("f_tableland_71.Id6", "f_tableland_71", Rectangle(-657, 842, 40));
		AddSpawnPoint("f_tableland_71.Id6", "f_tableland_71", Rectangle(-370, 763, 40));
		AddSpawnPoint("f_tableland_71.Id6", "f_tableland_71", Rectangle(-317, 939, 40));

		// 'Hohen_Barkle_Blue' GenType 38 Spawn Points
		AddSpawnPoint("f_tableland_71.Id7", "f_tableland_71", Rectangle(-387, 845, 40));
		AddSpawnPoint("f_tableland_71.Id7", "f_tableland_71", Rectangle(-465, 737, 40));

		// 'Tiny_Blue' GenType 39 Spawn Points
		AddSpawnPoint("f_tableland_71.Id8", "f_tableland_71", Rectangle(-498, 964, 40));

		// 'Cronewt_Bow_Blue' GenType 40 Spawn Points
		AddSpawnPoint("f_tableland_71.Id9", "f_tableland_71", Rectangle(-455, 826, 40));

		// 'Tiny_Blue' GenType 41 Spawn Points
		AddSpawnPoint("f_tableland_71.Id10", "f_tableland_71", Rectangle(1240, -155, 40));
		AddSpawnPoint("f_tableland_71.Id10", "f_tableland_71", Rectangle(1248, 43, 40));
		AddSpawnPoint("f_tableland_71.Id10", "f_tableland_71", Rectangle(1411, -71, 40));
		AddSpawnPoint("f_tableland_71.Id10", "f_tableland_71", Rectangle(1167, 462, 40));
		AddSpawnPoint("f_tableland_71.Id10", "f_tableland_71", Rectangle(1371, 456, 40));
		AddSpawnPoint("f_tableland_71.Id10", "f_tableland_71", Rectangle(1361, 663, 40));
		AddSpawnPoint("f_tableland_71.Id10", "f_tableland_71", Rectangle(1173, 680, 40));

		// 'Cronewt_Bow_Blue' GenType 42 Spawn Points
		AddSpawnPoint("f_tableland_71.Id11", "f_tableland_71", Rectangle(227, 381, 40));
		AddSpawnPoint("f_tableland_71.Id11", "f_tableland_71", Rectangle(452, 402, 40));
		AddSpawnPoint("f_tableland_71.Id11", "f_tableland_71", Rectangle(421, 747, 40));
		AddSpawnPoint("f_tableland_71.Id11", "f_tableland_71", Rectangle(168, 727, 40));

		// 'Hohen_Ritter_Purple' GenType 43 Spawn Points
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(406, 698, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(164, 306, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(280, 220, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(588, 278, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(223, 666, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(681, 481, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(351, 506, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(432, 278, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(140, 519, 40));
		AddSpawnPoint("f_tableland_71.Id12", "f_tableland_71", Rectangle(603, 638, 40));
	}
}
