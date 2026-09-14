//--- Melia Script -----------------------------------------------------------
// Uskis Arable Land Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_siauliai_46_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai462MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_siauliai_46_2.Id1", MonsterId.Zigri_Red, min: 30, max: 40);
		AddSpawner("f_siauliai_46_2.Id2", MonsterId.Mushroom_Ent_Black, min: 3, max: 4);
		AddSpawner("f_siauliai_46_2.Id3", MonsterId.Zigri_Red, min: 19, max: 25);
		AddSpawner("f_siauliai_46_2.Id4", MonsterId.Siaumire, min: 12, max: 15);
		AddSpawner("f_siauliai_46_2.Id5", MonsterId.Big_Siaulamb, min: 5, max: 6);
		AddSpawner("f_siauliai_46_2.Id6", MonsterId.Big_Siaulamb, amount: 3);
		AddSpawner("f_siauliai_46_2.Id7", MonsterId.Rootcrystal_01, min: 9, max: 12, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Zigri_Red' GenType 4 Spawn Points
		AddSpawnPoint("f_siauliai_46_2.Id1", "f_siauliai_46_2", Rectangle(-44, 4247, 2000));

		// 'Mushroom_Ent_Black' GenType 5 Spawn Points
		AddSpawnPoint("f_siauliai_46_2.Id2", "f_siauliai_46_2", Rectangle(1398, 5894, 1500));

		// 'Zigri_Red' GenType 20 Spawn Points
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(-751, 4244, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(-674, 4370, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(-561, 4243, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(-449, 4331, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(-4, 4207, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(59, 4307, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(316, 5851, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(645, 5859, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(988, 5852, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1147, 5673, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1238, 5983, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1419, 5837, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(955, 5322, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1236, 5285, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1245, 6435, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1297, 6678, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1066, 6493, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(767, 5287, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(779, 5376, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(869, 5466, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1052, 5417, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1101, 5597, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(477, 5871, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(161, 5774, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(246, 5979, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(511, 5996, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(869, 5898, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1144, 6070, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1187, 5820, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1176, 6275, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1073, 6380, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1166, 6731, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1208, 6599, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1636, 5859, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1875, 5921, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1869, 5766, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(2029, 5877, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1915, 6025, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1750, 5996, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1899, 5245, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1875, 5598, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1817, 5357, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1939, 5143, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1094, 5251, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1249, 5202, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(836, 5168, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1553, 5877, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1969, 5782, 30));
		AddSpawnPoint("f_siauliai_46_2.Id3", "f_siauliai_46_2", Rectangle(1950, 5334, 30));

		// 'Siaumire' GenType 21 Spawn Points
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(974, 5339, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1144, 5345, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1092, 5805, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(434, 5821, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(627, 5889, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1835, 5924, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1940, 5705, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1413, 5864, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(862, 5864, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1184, 6284, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1255, 6515, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1092, 6461, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(797, 5269, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(985, 5228, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1105, 5543, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1164, 5946, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1131, 6307, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1110, 6635, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1620, 5860, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1986, 5849, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(1905, 5522, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(211, 5943, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(254, 5702, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(505, 5960, 30));
		AddSpawnPoint("f_siauliai_46_2.Id4", "f_siauliai_46_2", Rectangle(836, 5873, 30));

		// 'Big_Siaulamb' GenType 22 Spawn Points
		AddSpawnPoint("f_siauliai_46_2.Id5", "f_siauliai_46_2", Rectangle(272, 5873, 9999));

		// 'Big_Siaulamb' GenType 23 Spawn Points
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(350, 5693, 40));
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(677, 5868, 40));
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(2005, 5880, 40));
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(1154, 5691, 40));
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(1843, 5853, 40));
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(1185, 5978, 40));
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(1087, 5384, 40));
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(218, 5812, 40));
		AddSpawnPoint("f_siauliai_46_2.Id6", "f_siauliai_46_2", Rectangle(1899, 5439, 40));

		// 'Rootcrystal_01' GenType 24 Spawn Points
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(-1862, 3158, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(-1358, 3735, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(-568, 4333, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(-653, 3589, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(145, 4198, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(995, 4292, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(1924, 5237, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(1907, 5850, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(818, 5296, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(1181, 5867, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(366, 5828, 200));
		AddSpawnPoint("f_siauliai_46_2.Id7", "f_siauliai_46_2", Rectangle(1243, 6517, 200));
	}
}
