//--- Melia Script -----------------------------------------------------------
// Tyla Monastery Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_abbey_39_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey394MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_abbey_39_4.Id1", MonsterId.Rootcrystal_01, min: 9, max: 11, respawn: Seconds(5));
		AddSpawner("d_abbey_39_4.Id2", MonsterId.Malstatue, min: 8, max: 10);
		AddSpawner("d_abbey_39_4.Id3", MonsterId.Malstatue, min: 17, max: 22);
		AddSpawner("d_abbey_39_4.Id4", MonsterId.Velaphid_Red, min: 15, max: 20);
		AddSpawner("d_abbey_39_4.Id5", MonsterId.Pumpflap, min: 8, max: 10);
		AddSpawner("d_abbey_39_4.Id6", MonsterId.Kowak_Orange, min: 9, max: 12);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 2 Spawn Points
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(1325, -1512, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(1528, -1293, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(1345, -867, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(883, -894, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(718, -564, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(918, -170, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(999, 236, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(707, 363, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(178, 1042, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(-92, 1357, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(-286, 1044, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(-1040, 409, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(-1585, 184, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(-1169, 165, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(-340, 221, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(71, -758, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(-102, -1254, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(695, -1285, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(140, 241, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(-45, -520, 50));
		AddSpawnPoint("d_abbey_39_4.Id1", "d_abbey_39_4", Rectangle(295, -1551, 50));

		// 'Malstatue' GenType 30 Spawn Points
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(629, -670, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(632, -618, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(600, -851, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(599, -782, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(713, -1000, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1383, -985, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1300, -680, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1459, -589, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1526, -803, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1515, -932, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1383, -849, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1375, -1542, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1383, -1421, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1379, -1270, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1495, -1266, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1492, -1429, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(1492, -1564, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(501, -1238, 20));
		AddSpawnPoint("d_abbey_39_4.Id2", "d_abbey_39_4", Rectangle(567, -1238, 20));

		// 'Malstatue' GenType 31 Spawn Points
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(-125, 17, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(47, 15, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(170, 139, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(167, 309, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(-247, 310, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(-247, 136, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(-122, 430, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(45, 435, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(873, 315, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(873, 171, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(1015, 172, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(1017, 314, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(811, 376, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(1074, 375, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(1076, 111, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(811, 111, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(-96, 1266, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(42, 1269, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(37, 1129, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(-98, 1133, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(-161, 1067, 20));
		AddSpawnPoint("d_abbey_39_4.Id3", "d_abbey_39_4", Rectangle(98, 1070, 20));

		// 'Velaphid_Red' GenType 34 Spawn Points
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-104, 512, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-376, 74, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(303, 390, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-15, 231, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-436, 912, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-27, 1392, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(109, 1009, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(223, 1308, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(759, 240, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(941, 230, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(1123, 49, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(765, 12, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(1118, 467, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-41, 1265, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(117, -30, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(754, 387, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(903, 465, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(496, 705, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(393, 836, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-614, 699, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-250, 1160, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(263, 165, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-146, -51, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-323, 336, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-248, 188, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(209, 244, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(125, 1150, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-62, 1138, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-253, 1378, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(-133, 982, 25));
		AddSpawnPoint("d_abbey_39_4.Id4", "d_abbey_39_4", Rectangle(292, 946, 25));

		// 'Pumpflap' GenType 37 Spawn Points
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(92, -88, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-272, -47, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-140, 204, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-261, 445, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(292, 267, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-1097, 145, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-1175, 407, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-935, 301, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(969, 97, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(718, 495, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(1174, 253, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(941, 240, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(699, 178, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(1074, 506, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(51, 333, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(116, 550, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-1238, 238, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-997, 225, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-1008, 406, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(241, -5, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-357, 317, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(902, -152, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(964, 443, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(785, 297, 25));
		AddSpawnPoint("d_abbey_39_4.Id5", "d_abbey_39_4", Rectangle(-33, 18, 25));

		// 'Kowak_Orange' GenType 58 Spawn Points
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(-88, -1242, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(108, -1254, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(-78, -1466, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(71, -1416, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(252, -1357, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(254, -1547, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(106, -1615, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(-108, -1636, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(-91, -768, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(68, -674, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(-82, -605, 25));
		AddSpawnPoint("d_abbey_39_4.Id6", "d_abbey_39_4", Rectangle(62, -508, 25));
	}
}
