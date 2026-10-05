//--- Melia Script -----------------------------------------------------------
// Sutatis Trade Route Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_pilgrimroad_31_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad312MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_pilgrimroad_31_2.Id1", MonsterId.Sec_Tama, min: 38, max: 50);
		AddSpawner("f_pilgrimroad_31_2.Id2", MonsterId.Sec_Shredded, min: 23, max: 30);
		AddSpawner("f_pilgrimroad_31_2.Id3", MonsterId.Sec_New_Desmodus, min: 23, max: 30);
		AddSpawner("f_pilgrimroad_31_2.Id4", MonsterId.Sec_Tama, min: 23, max: 30);
		AddSpawner("f_pilgrimroad_31_2.Id5", MonsterId.Rootcrystal_01, min: 7, max: 9, respawn: Seconds(30));

		// Monster Spawn Points -----------------------------

		// 'Sec_Tama' GenType 17 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_2.Id1", "f_pilgrimroad_31_2", Rectangle(-50, -651, 9999));

		// 'Sec_Shredded' GenType 18 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1561, -1466, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1397, -1441, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1341, -1571, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1118, -1482, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1093, -1336, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1025, -1434, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-978, -1595, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-789, -1506, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-748, -1425, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1254, -1127, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1485, -1192, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1519, -1005, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1358, -1030, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1455, -678, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1313, -812, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1134, -729, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-983, -744, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-1252, -1385, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-439, 1801, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-444, 2026, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-332, 1908, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-326, 2143, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-167, 1921, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-68, 2113, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-15, 1823, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-234, 1696, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id2", "f_pilgrimroad_31_2", Rectangle(-339, 1378, 30));

		// 'Sec_New_Desmodus' GenType 19 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(732, 1294, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(854, 1457, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(873, 1246, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(751, 1128, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1002, 1045, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1201, 1079, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1057, 893, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(993, 689, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1141, 664, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1240, 780, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1359, 841, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1336, 584, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1160, 542, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1045, 490, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1298, 355, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1177, 220, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(970, 161, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(834, 165, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(693, 57, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(1260, 946, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id3", "f_pilgrimroad_31_2", Rectangle(521, 165, 30));

		// 'Sec_Tama' GenType 20 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-269, 480, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-328, 316, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-427, 150, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-278, 30, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-126, 186, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-110, 308, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(116, 141, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(234, 226, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(343, 121, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-309, 816, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-358, 1017, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-370, 748, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-272, -334, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-293, -538, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-163, -843, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-263, -924, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-484, -887, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-508, -1090, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-354, -722, 30));
		AddSpawnPoint("f_pilgrimroad_31_2.Id4", "f_pilgrimroad_31_2", Rectangle(-205, -716, 30));

		// 'Rootcrystal_01' GenType 21 Spawn Points
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(-1309, -885, 150));
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(-109, -1025, 150));
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(-224, 311, 150));
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(-348, 1122, 150));
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(-252, 2106, 150));
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(786, 1411, 150));
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(1226, 900, 150));
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(708, 127, 150));
		AddSpawnPoint("f_pilgrimroad_31_2.Id5", "f_pilgrimroad_31_2", Rectangle(-1556, -1484, 150));
	}
}
