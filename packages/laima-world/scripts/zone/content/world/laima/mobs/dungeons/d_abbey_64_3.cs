//--- Melia Script -----------------------------------------------------------
// Novaha Institute Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_abbey_64_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey643MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_abbey_64_3.Id1", MonsterId.Lapemiter, min: 28, max: 36, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id2", MonsterId.Lapflammer, min: 28, max: 36, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id3", MonsterId.Lapeman, min: 28, max: 36, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id4", MonsterId.Rootcrystal_03, min: 18, max: 24, respawn: Seconds(20), tendency: TendencyType.Peaceful);
		AddSpawner("d_abbey_64_3.Id5", MonsterId.Humming_Bud_Purple, min: 38, max: 50, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id6", MonsterId.Humming_Bud_Purple, min: 20, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id7", MonsterId.Lapflammer, min: 20, max: 26, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id8", MonsterId.Lapeman, min: 15, max: 20, tendency: TendencyType.Aggressive);

		AddSpawner("d_abbey_64_3.Id9", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id10", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id11", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id12", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id13", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id14", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id15", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id16", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_64_3.Id17", MonsterId.Hepatica_Green, respawn: Seconds(60), min: 18, max: 28, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Lapemiter' GenType 11 Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-230, -9, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-207, -407, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-138, -164, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-328, -199, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-1524, -1026, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(507, -574, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(699, -419, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(652, -500, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(709, -633, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(608, -732, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(397, -759, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(460, -640, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(726, -827, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(839, -660, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(754, -731, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(803, -498, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(264, -944, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(158, -905, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(118, -959, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(158, -583, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(48, -553, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-46, -566, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-209, -688, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-229, -578, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-407, -527, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-77, -467, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-421, -1353, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-234, -1158, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-71, -1297, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-158, -1457, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-340, -1536, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-332, -1401, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-367, -1211, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-123, -1123, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(19, -1363, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-212, -1625, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-624, -1361, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-772, -1297, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-991, -1372, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-1012, -1202, 30));
		AddSpawnPoint("d_abbey_64_3.Id1", "d_abbey_64_3", Rectangle(-918, -1329, 30));

		// 'Lapflammer' GenType 12 Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-851, -305, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-163, -309, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-325, -363, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-379, -523, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-231, -614, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-238, -761, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-240, -874, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-369, -157, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-155, -97, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-331, 112, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-274, 312, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-150, 245, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-156, -23, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-206, -187, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-57, -190, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-535, -190, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-704, -140, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-800, -247, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-955, -408, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-890, -552, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-760, -378, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-725, -633, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-848, -652, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-641, -345, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-890, 152, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-855, 306, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-749, 96, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-800, 28, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-770, 211, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-692, 287, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-1084, 182, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-1059, 255, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-1138, 198, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(114, -114, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(129, 0, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(114, 61, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(190, 147, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(130, 227, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(227, 283, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(301, 281, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-833, 562, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-851, 787, 30));
		AddSpawnPoint("d_abbey_64_3.Id2", "d_abbey_64_3", Rectangle(-837, 1061, 30));

		// 'Lapeman' GenType 34 Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1501, -881, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-997, 1198, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-845, 1201, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-828, 1351, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-747, 1437, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-615, 1268, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-436, 1239, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-231, 1281, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-58, 1378, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(71, 1196, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-41, 1222, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-64, 1071, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1597, -690, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1413, -624, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1499, -747, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1593, -924, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1545, -1138, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1511, -1246, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1323, -1200, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1396, -1090, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1446, -1140, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-983, -1349, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-876, -1232, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-890, -1365, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-663, -1349, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-406, -1427, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-298, -1214, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-118, -1242, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-103, -1466, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-225, -1505, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-419, -1291, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-34, -1385, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-928, 1315, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-1023, 1415, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-673, 1139, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-816, 998, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(-107, 1335, 30));
		AddSpawnPoint("d_abbey_64_3.Id3", "d_abbey_64_3", Rectangle(17, 1272, 30));

		// 'Rootcrystal_03' GenType 43 Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(216, -917, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(635, -428, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(619, -821, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(1185, -488, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(1142, -828, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(1107, 397, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(549, 375, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(929, -106, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(1317, 139, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-194, 254, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-178, -665, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-355, -122, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-284, -1176, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-273, -1630, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-917, -1362, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-795, -574, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-937, -106, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-703, 324, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-1044, 1242, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-619, 1355, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-45, 1147, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-1506, -1181, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-1556, -719, 10));
		AddSpawnPoint("d_abbey_64_3.Id4", "d_abbey_64_3", Rectangle(-1398, 104, 10));

		// 'Humming_Bud_Purple' GenType 45 Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id5", "d_abbey_64_3", Rectangle(-911, -375, 9999));

		// 'Humming_Bud_Purple' GenType 46 Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id6", "d_abbey_64_3", Rectangle(666, -635, 9999));

		// 'Lapeflammer' GenType 47 Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(1293, -751, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(1293, -516, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-241, -293, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-184, 78, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-285, -531, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-673, -551, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-891, -240, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-761, -50, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-782, 304, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-623, -171, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-679, 1200, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-927, 1259, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-499, 1263, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-205, -1557, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-497, -1359, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-92, -1392, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-157, -1262, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-1059, -1261, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-843, -1324, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-1528, -1101, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-1398, -907, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(-1596, -759, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(595, -584, 30));
		AddSpawnPoint("d_abbey_64_3.Id7", "d_abbey_64_3", Rectangle(810, -793, 30));

		// 'Lapeman' GenType 48 Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(743, 69, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(855, -40, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(707, -88, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1068, -107, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1211, 328, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(923, 101, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1126, 23, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1106, 310, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1222, 122, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1137, -323, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1302, -411, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1193, -151, 40));
		AddSpawnPoint("d_abbey_64_3.Id8", "d_abbey_64_3", Rectangle(1111, -523, 40));

		// 'Hepatica_Green' Spawn Points
		AddSpawnPoint("d_abbey_64_3.Id9", "d_abbey_64_3", Rectangle(155, -917, 90));
		AddSpawnPoint("d_abbey_64_3.Id10", "d_abbey_64_3", Rectangle(-233, -862, 90));
		AddSpawnPoint("d_abbey_64_3.Id11", "d_abbey_64_3", Rectangle(-237, -1718, 90));
		AddSpawnPoint("d_abbey_64_3.Id12", "d_abbey_64_3", Rectangle(-1450, -941, 90));
		AddSpawnPoint("d_abbey_64_3.Id13", "d_abbey_64_3", Rectangle(-1152, 235, 90));
		AddSpawnPoint("d_abbey_64_3.Id14", "d_abbey_64_3", Rectangle(-823, 886, 90));
		AddSpawnPoint("d_abbey_64_3.Id15", "d_abbey_64_3", Rectangle(-785, -487, 90));
		AddSpawnPoint("d_abbey_64_3.Id16", "d_abbey_64_3", Rectangle(-268, -1336, 90));
		AddSpawnPoint("d_abbey_64_3.Id17", "d_abbey_64_3", Rectangle(161, -113, 90));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Deathweaver, "d_abbey_64_3", 1, Hours(2), Hours(4));
	}
}
