//--- Melia Script -----------------------------------------------------------
// Topes Fortress 2F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_castle_67_2'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DCastle672MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_castle_67_2.Id1", MonsterId.Rootcrystal_03, min: 12, max: 15, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("d_castle_67_2.Id2", MonsterId.FD_Flamil, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_2.Id3", MonsterId.FD_Flamil, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_2.Id4", MonsterId.FD_Flamme_Mage, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_2.Id5", MonsterId.FD_Flamag, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_castle_67_2.Id6", MonsterId.FD_Stoulet_Mage_Green, min: 15, max: 20, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_03' GenType 4 Spawn Points
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(-788, -739, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(-437, -643, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(-1532, -472, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(-1515, 122, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(-1506, 745, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(-536, 127, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(123, -212, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(275, -729, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(576, -446, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(-32, -1221, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(796, -1078, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(1536, -1101, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(1130, -404, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(725, 251, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(893, 979, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(1305, 275, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(1357, 43, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(232, -1242, 100));
		AddSpawnPoint("d_castle_67_2.Id1", "d_castle_67_2", Rectangle(-261, 233, 100));

		// 'FD_Flamil' GenType 6 Spawn Points
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(-873, -628, 100));
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(693, 1056, 100));
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(-31, -1221, 100));
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(-1706, -551, 100));
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(-1516, 123, 100));
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(-485, 134, 100));
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(7, -344, 100));
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(1396, -406, 100));
		AddSpawnPoint("d_castle_67_2.Id2", "d_castle_67_2", Rectangle(-480, 988, 100));

		// 'FD_Flamil' GenType 9 Spawn Points
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-829, -759, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-691, -585, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-1480, -294, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-857, -627, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-416, -505, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-777, -851, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-1703, 14, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(91, -238, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(326, -726, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-436, -646, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(173, -1191, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-253, -1369, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(403, -1074, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(963, -1001, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(1031, -294, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(799, -1085, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(1101, 138, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(732, 358, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(892, 984, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(1223, 737, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(463, 422, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-1700, -383, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-1348, -671, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-1702, 324, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-1341, 17, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-1612, 885, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-1439, 580, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-463, 206, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-297, 40, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(351, 389, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(557, 219, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(493, -181, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(49, -589, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(531, -575, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-93, -1205, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(189, -1321, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(1311, -957, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(1738, -1269, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(1499, 768, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(1333, 348, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(472, 938, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-559, 1247, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-189, 1190, 40));
		AddSpawnPoint("d_castle_67_2.Id3", "d_castle_67_2", Rectangle(-460, 926, 40));

		// 'FD_Flamme_Mage' GenType 10 Spawn Points
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(464, -313, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(473, -637, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(178, -490, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-481, -567, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-715, -755, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1370, -401, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1472, -704, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1440, 174, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1601, 93, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1511, 597, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1517, 908, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-561, 12, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-202, 232, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-125, -1144, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(128, -1246, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(730, -1074, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(1495, -1095, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(1357, -970, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(1515, -421, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(1222, -403, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(1223, 107, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(1415, 419, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(624, 772, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(436, 233, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(733, 363, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(860, 855, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(1022, -402, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(703, -962, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(427, -1068, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(859, -1168, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-349, -958, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-317, -595, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-169, 97, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(675, 160, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(400, 548, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1700, 12, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1334, 317, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1622, -379, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-1636, -567, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-614, 1170, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-265, 1011, 40));
		AddSpawnPoint("d_castle_67_2.Id4", "d_castle_67_2", Rectangle(-470, 1219, 40));

		// 'FD_Flamag' GenType 11 Spawn Points
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-668, -901, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-854, -640, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-529, -817, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-183, -1137, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-332, -977, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(156, -1187, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-53, -1369, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(424, -1082, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(792, -946, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(838, -1262, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(984, -1091, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(1464, -938, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(1561, -1225, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(481, -703, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(539, -599, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(409, -52, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(69, -401, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-159, 88, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(33, -157, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-586, 58, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-1429, 42, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-1614, 225, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-1412, 852, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-1619, 649, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-1617, 883, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-1698, -392, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-1677, -640, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-1327, -591, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(1019, -401, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(1442, -519, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(1265, 189, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(1430, 786, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(1241, 453, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(830, 801, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(502, 1062, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(553, 558, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(557, 202, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-361, 1298, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-531, 1063, 40));
		AddSpawnPoint("d_castle_67_2.Id5", "d_castle_67_2", Rectangle(-389, 962, 40));

		// 'FD_Stoulet_Mage_Green' GenType 27 Spawn Points
		AddSpawnPoint("d_castle_67_2.Id6", "d_castle_67_2", Rectangle(219, -334, 9999));
	}
}
