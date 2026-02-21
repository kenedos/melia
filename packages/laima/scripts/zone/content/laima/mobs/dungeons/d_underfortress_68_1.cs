//--- Melia Script -----------------------------------------------------------
// Sicarius 1F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_underfortress_68_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress681MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_underfortress_68_1.Id1", MonsterId.FD_InfroRocktor_Red, min: 38, max: 50, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_1.Id2", MonsterId.FD_Pyran, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_1.Id3", MonsterId.FD_Fire_Dragon, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_1.Id4", MonsterId.FD_Tower_Of_Firepuppet, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_1.Id5", MonsterId.FD_Tower_Of_Firepuppet, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68_1.Id6", MonsterId.Rootcrystal_03, min: 11, max: 14, respawn: Minutes(1), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'FD_InfroRocktor_Red' GenType 11 Spawn Points
		AddSpawnPoint("d_underfortress_68_1.Id1", "d_underfortress_68_1", Rectangle(245, -890, 9999));

		// 'FD_Pyran' GenType 18 Spawn Points
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-1532, -793, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-1027, -1468, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-595, -1524, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-295, -711, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-999, -720, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-687, -800, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-661, -994, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-996, -994, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-829, 296, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(-883, -43, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(973, -1061, 100));
		AddSpawnPoint("d_underfortress_68_1.Id2", "d_underfortress_68_1", Rectangle(1041, -772, 100));

		// 'FD_Fire_Dragon' GenType 20 Spawn Points
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-868, 1094, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-1035, 803, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-1107, 960, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-561, 966, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-214, 934, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-892, 1629, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-1544, 928, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(176, -739, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(471, -868, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(165, -1355, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(475, -1377, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(402, -1049, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(888, -657, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(1243, -715, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(1192, -1270, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(874, -1338, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(570, -1084, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-854, 728, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-746, 791, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-1489, 980, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-938, 1555, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(-213, 1067, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(917, -970, 40));
		AddSpawnPoint("d_underfortress_68_1.Id3", "d_underfortress_68_1", Rectangle(1238, -935, 40));

		// 'FD_Tower_Of_Firepuppet' GenType 21 Spawn Points
		AddSpawnPoint("d_underfortress_68_1.Id4", "d_underfortress_68_1", Rectangle(995, 197, 100));
		AddSpawnPoint("d_underfortress_68_1.Id4", "d_underfortress_68_1", Rectangle(1051, -237, 100));
		AddSpawnPoint("d_underfortress_68_1.Id4", "d_underfortress_68_1", Rectangle(1300, -62, 100));
		AddSpawnPoint("d_underfortress_68_1.Id4", "d_underfortress_68_1", Rectangle(1368, 284, 100));
		AddSpawnPoint("d_underfortress_68_1.Id4", "d_underfortress_68_1", Rectangle(1195, 144, 100));

		// 'FD_Tower_Of_Firepuppet' GenType 22 Spawn Points
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(1067, 460, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(716, -890, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(182, -1087, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-55, -732, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-774, -766, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-849, 837, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-995, 112, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-571, 170, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(1020, 11, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(1371, 153, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-1083, -845, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-844, -998, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-977, 924, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-832, 1238, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-787, 1072, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-679, -1528, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(353, -1220, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(997, -1189, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(1179, -763, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-249, -802, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-668, 0, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-1564, 1035, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-826, 1654, 40));
		AddSpawnPoint("d_underfortress_68_1.Id5", "d_underfortress_68_1", Rectangle(-182, 970, 40));

		// 'Rootcrystal_03' GenType 26 Spawn Points
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-1423, 73, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-756, 148, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-726, 956, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-893, -834, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(258, -1245, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(1089, -874, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(1088, 321, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-144, -796, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-877, 1706, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-1598, 953, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-97, 1004, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-1556, -867, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-1006, -1570, 50));
		AddSpawnPoint("d_underfortress_68_1.Id6", "d_underfortress_68_1", Rectangle(-609, -1446, 50));
	}
}
