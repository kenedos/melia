//--- Melia Script -----------------------------------------------------------
// Feretory Hills Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_pilgrimroad_31_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad311MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_pilgrimroad_31_1.Id1", MonsterId.Sec_Ellomago, min: 45, max: 60);
		AddSpawner("f_pilgrimroad_31_1.Id2", MonsterId.Sec_Hook, min: 27, max: 35);
		AddSpawner("f_pilgrimroad_31_1.Id3", MonsterId.Sec_Hallowventor, min: 27, max: 35);
		AddSpawner("f_pilgrimroad_31_1.Id4", MonsterId.Sec_Ellomago, min: 15, max: 20);
		AddSpawner("f_pilgrimroad_31_1.Id5", MonsterId.Rootcrystal_01, min: 8, max: 10, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Sec_Ellomago' GenType 13 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_1.Id1", "f_pilgrimroad_31_1", Rectangle(55, 292, 9999));

		// 'Sec_Hook' GenType 14 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-850, 1140, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-757, 1479, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-732, 1242, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-864, 1372, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-947, 937, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-554, 1029, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-532, 1274, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-694, 940, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-98, 1048, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-3, 1189, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(113, 1047, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(-52, 839, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(129, 870, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(246, 1057, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(121, 1216, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(566, 705, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(779, 913, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(588, 862, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(677, 731, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(713, 417, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(941, 441, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(971, 787, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(767, 543, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(926, 600, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id2", "f_pilgrimroad_31_1", Rectangle(852, 677, 30));

		// 'Sec_Hallowventor' GenType 15 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(299, -902, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(376, -744, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(410, -1080, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(561, -1004, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(600, -907, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(283, -370, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(154, -236, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(347, -228, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(279, -538, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(427, -961, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(271, -1003, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(583, -785, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(474, -842, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1071, -569, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1184, -472, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1269, -647, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1329, -406, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(936, -207, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1070, -95, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1031, -351, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1393, -579, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1481, -468, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(115, -383, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1101, -723, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1231, -125, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(961, 157, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1478, -150, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1367, -259, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id3", "f_pilgrimroad_31_1", Rectangle(1165, -258, 30));

		// 'Sec_Ellomago' GenType 16 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-580, 169, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-545, 295, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-478, 142, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-418, 327, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-686, -399, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-627, -275, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-544, -386, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-496, -286, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-675, -507, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-492, -181, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-543, -526, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-429, -454, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-374, 193, 30));
		AddSpawnPoint("f_pilgrimroad_31_1.Id4", "f_pilgrimroad_31_1", Rectangle(-455, 252, 30));

		// 'Rootcrystal_01' GenType 17 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(-486, -387, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(454, -918, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(245, -344, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(145, 273, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(-794, 1325, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(74, 1055, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(774, 681, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(1252, -409, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(-461, 249, 150));
		AddSpawnPoint("f_pilgrimroad_31_1.Id5", "f_pilgrimroad_31_1", Rectangle(-978, -813, 150));
	}
}
