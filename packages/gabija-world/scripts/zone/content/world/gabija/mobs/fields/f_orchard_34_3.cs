//--- Melia Script -----------------------------------------------------------
// Barha Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_orchard_34_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard343MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_orchard_34_3.Id1", MonsterId.Mushroom_Ent_Red, min: 3, max: 4);
		AddSpawner("f_orchard_34_3.Id2", MonsterId.Rafflesia_Green, min: 11, max: 14);
		AddSpawner("f_orchard_34_3.Id3", MonsterId.Big_Cockatries_Red, min: 15, max: 20);
		AddSpawner("f_orchard_34_3.Id4", MonsterId.Flying_Flog_White, min: 23, max: 30);
		AddSpawner("f_orchard_34_3.Id5", MonsterId.Rafflesia_Green, min: 19, max: 25);
		AddSpawner("f_orchard_34_3.Id6", MonsterId.Flying_Flog_White, min: 8, max: 10);
		AddSpawner("f_orchard_34_3.Id7", MonsterId.Rootcrystal_01, min: 15, max: 19, respawn: Minutes(1));

		// Monster Spawn Points -----------------------------

		// 'Mushroom_Ent_Red' GenType 26 Spawn Points
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(58, 566, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(-88, 353, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(-80, 559, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(-300, 564, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(-465, 453, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(-514, 635, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(63, 748, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(192, 442, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(217, 613, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(357, 527, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(549, 627, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(-498, 835, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(-599, 1158, 50));
		AddSpawnPoint("f_orchard_34_3.Id1", "f_orchard_34_3", Rectangle(524, 453, 50));

		// 'Rafflesia_Green' GenType 27 Spawn Points
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1103, -281, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(884, -424, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(789, -214, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(948, -230, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(958, -99, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(919, 30, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1169, -129, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1349, -142, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1225, -355, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1230, -501, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1065, -431, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1007, -621, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(851, -657, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1131, -606, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1053, -479, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1300, -539, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1447, -402, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1355, -392, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1155, -1, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1212, -173, 30));
		AddSpawnPoint("f_orchard_34_3.Id2", "f_orchard_34_3", Rectangle(1327, -243, 30));

		// 'Big_Cockatries_Red' GenType 28 Spawn Points
		AddSpawnPoint("f_orchard_34_3.Id3", "f_orchard_34_3", Rectangle(-357, -184, 9999));

		// 'Flying_Flog_White' GenType 29 Spawn Points
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-734, -315, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1087, -369, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1237, -176, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-861, -147, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-919, -305, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1745, -320, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1525, -252, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1574, -434, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1443, -677, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1593, -846, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1404, -1003, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1509, -1150, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1283, -1170, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1195, -1056, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1339, -1121, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-1499, -892, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(941, -781, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(822, -973, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1009, -1031, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1140, -893, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1021, -911, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1213, 589, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1239, 679, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1354, 776, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1439, 670, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1419, 541, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1220, 477, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1333, 564, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(987, 610, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(889, 41, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(789, 126, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(539, 695, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-283, -371, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-80, 56, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-89, 246, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-237, 203, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-82, 488, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-217, 600, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-785, 587, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-618, 470, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-496, 1045, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-609, 1152, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-531, 1165, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(-470, 784, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(844, 587, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1117, -537, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(922, -343, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1131, -195, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1218, -304, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1129, -381, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1320, -484, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1667, -265, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1793, -203, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1682, -399, 30));
		AddSpawnPoint("f_orchard_34_3.Id4", "f_orchard_34_3", Rectangle(1593, -163, 30));

		// 'Rafflesia_Green' GenType 30 Spawn Points
		AddSpawnPoint("f_orchard_34_3.Id5", "f_orchard_34_3", Rectangle(-365, -145, 9999));

		// 'Flying_Flog_White' GenType 31 Spawn Points
		AddSpawnPoint("f_orchard_34_3.Id6", "f_orchard_34_3", Rectangle(444, -693, 200));

		// 'Rootcrystal_01' GenType 38 Spawn Points
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-1459, -979, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-1671, -304, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-1147, -248, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-797, -271, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-378, -254, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-262, -855, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(381, -680, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(1007, -911, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(1305, -331, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(965, -419, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(1758, -288, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(815, 175, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(1155, 662, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(655, 582, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(0, 504, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-77, 128, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-598, 1238, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-647, 620, 100));
		AddSpawnPoint("f_orchard_34_3.Id7", "f_orchard_34_3", Rectangle(-1366, 500, 100));
	}
}
