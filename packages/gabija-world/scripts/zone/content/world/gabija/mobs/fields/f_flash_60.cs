//--- Melia Script -----------------------------------------------------------
// Roxona Market Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_flash_60'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash60MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_flash_60.Id1", MonsterId.Moya, min: 15, max: 20);
		AddSpawner("f_flash_60.Id2", MonsterId.Bavon, min: 15, max: 20);
		AddSpawner("f_flash_60.Id3", MonsterId.Moya, min: 12, max: 15);
		AddSpawner("f_flash_60.Id4", MonsterId.Bavon, min: 15, max: 20);
		AddSpawner("f_flash_60.Id5", MonsterId.Saltisdaughter_Mage, min: 8, max: 10);
		AddSpawner("f_flash_60.Id6", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(20));

		// Monster Spawn Points -----------------------------

		// 'Moya' GenType 3 Spawn Points
		AddSpawnPoint("f_flash_60.Id1", "f_flash_60", Rectangle(465, 6, 9999));

		// 'Bavon' GenType 17 Spawn Points
		AddSpawnPoint("f_flash_60.Id2", "f_flash_60", Rectangle(288, -27, 9999));

		// 'Moya' GenType 25 Spawn Points
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-609, -958, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-327, -1097, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-235, -948, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(327, -1034, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(475, -1078, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(453, -922, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-1265, -1083, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-1190, -805, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-1305, -938, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-614, -822, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-391, -950, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-136, -1110, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(312, -916, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(182, -772, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(108, -302, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(328, -249, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(353, -27, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(217, -143, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(379, 99, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(218, 440, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(65, 522, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(144, 680, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-1193, -307, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-1275, -402, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-1177, -498, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-669, -85, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-826, 438, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-697, 43, 25));
		AddSpawnPoint("f_flash_60.Id3", "f_flash_60", Rectangle(-728, -253, 25));

		// 'Bavon' GenType 26 Spawn Points
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(399, 11, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-316, -1097, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-723, -981, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-691, -850, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-90, 0, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(71, 472, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(213, 1193, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(251, 1453, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(713, 1509, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(879, 1406, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-480, -966, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-261, -77, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(141, 604, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-194, 478, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(125, 1313, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(112, 1057, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(904, 1563, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(494, 1480, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-241, -959, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-497, 337, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(164, -206, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-480, -1105, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(358, -1066, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(443, -905, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(236, -842, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(305, -930, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(190, -1162, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(298, -335, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-671, -515, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-679, -107, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-1247, -332, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-1178, -497, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-1329, -1009, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-1211, -773, 25));
		AddSpawnPoint("f_flash_60.Id4", "f_flash_60", Rectangle(-1097, -1037, 25));

		// 'Saltisdaughter_Mage' GenType 27 Spawn Points
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(144, 695, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(280, 316, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(331, -385, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(149, 1156, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(267, 1566, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(182, 1314, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(263, -24, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(-273, -38, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(0, -311, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(443, -42, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(657, 1497, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(911, 1312, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(-659, -219, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(907, 1599, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(240, -180, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(-360, -1073, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(-630, -921, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(-1297, -1041, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(-1195, -728, 40));
		AddSpawnPoint("f_flash_60.Id5", "f_flash_60", Rectangle(-1134, -380, 40));

		// 'Rootcrystal_01' GenType 32 Spawn Points
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(881, -383, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(314, -263, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(175, 497, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-148, -37, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-1184, 1279, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-1013, 1124, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-795, 558, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-670, 400, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-1261, 440, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-622, -159, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-735, -944, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-359, -1026, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-177, -966, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-1256, -1073, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-1179, -778, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(-1202, -370, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(192, 1120, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(246, 1329, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(218, 1562, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(635, 1506, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(859, 1454, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(377, -1130, 100));
		AddSpawnPoint("f_flash_60.Id6", "f_flash_60", Rectangle(434, -925, 100));
	}
}
