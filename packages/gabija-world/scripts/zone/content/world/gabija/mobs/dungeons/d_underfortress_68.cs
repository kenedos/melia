//--- Melia Script -----------------------------------------------------------
// Storage Quarter Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_underfortress_68'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress68MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_underfortress_68.Id1", MonsterId.Deadbornscab_Red, min: 19, max: 25, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68.Id2", MonsterId.Infroholder_Green, min: 27, max: 35, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68.Id3", MonsterId.Deadbornscab_Mage_Red, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_68.Id4", MonsterId.Deadbornscab_Red, min: 6, max: 7, tendency: TendencyType.Peaceful);
		AddSpawner("d_underfortress_68.Id5", MonsterId.Rootcrystal_05, min: 18, max: 23, respawn: Seconds(20), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Deadbornscab_Red' GenType 9 Spawn Points
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-1279, -1978, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(110, -1587, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-1289, -299, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-480, 352, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(545, 350, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(892, 495, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(281, -620, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(1398, -585, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(1889, -550, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(1939, 1260, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(250, -1505, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-251, -729, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(2145, 714, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(2149, 336, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(2541, -639, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(2243, -187, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(2551, -213, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(690, 530, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(891, 311, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(126, 523, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-386, -873, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-709, -971, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-1392, -1127, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-429, -1677, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-862, -1854, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-1091, -1058, 30));
		AddSpawnPoint("d_underfortress_68.Id1", "d_underfortress_68", Rectangle(-801, 118, 30));

		// 'Infroholder_Green' GenType 16 Spawn Points
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1232, -1074, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1523, -1205, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1454, -986, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1379, -1178, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1318, -1332, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1364, -717, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1366, -558, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1273, -1594, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1305, -1883, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-923, -996, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1968, -1377, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1759, -1448, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1258, -178, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1508, 41, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1147, -74, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-652, 250, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(8, 481, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(742, 306, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(630, 477, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(843, 568, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-994, -1825, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-587, -1703, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-283, -839, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(200, -641, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(835, -602, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(1496, -566, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(1942, -462, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1278, -1741, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1140, -1916, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-411, -1633, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-831, -1784, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1419, -1076, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1315, -933, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1604, -1092, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-1185, -302, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-586, -925, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(-424, -881, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(694, -600, 30));
		AddSpawnPoint("d_underfortress_68.Id2", "d_underfortress_68", Rectangle(1665, -579, 30));

		// 'Deadbornscab_Mage_Red' GenType 18 Spawn Points
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(565, 416, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(753, 654, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(681, 530, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(891, 286, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(131, -1647, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(48, -1501, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(-110, -1618, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(68, -1737, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(1601, -615, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(1601, -615, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(1239, -571, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2498, -619, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2167, -472, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2547, -297, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2501, -47, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2184, 326, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2317, 525, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(780, -641, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(-94, -708, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(120, -589, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(141, -807, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(52, -700, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(641, -610, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(916, 503, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(244, -1584, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2422, -391, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2403, 154, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2133, 211, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2126, 533, 30));
		AddSpawnPoint("d_underfortress_68.Id3", "d_underfortress_68", Rectangle(2648, -568, 30));

		// 'Deadbornscab_Red' GenType 22 Spawn Points
		AddSpawnPoint("d_underfortress_68.Id4", "d_underfortress_68", Rectangle(-1304, -122, 400));

		// 'Rootcrystal_05' GenType 23 Spawn Points
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-2566, -1286, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-1699, -1474, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-1378, -1071, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-1275, -1698, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-1280, -2093, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-216, -1617, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(337, -1498, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-1353, -453, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-1147, 14, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-257, 464, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(952, 567, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(669, 216, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-715, -981, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(-91, -849, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(660, -690, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(1380, -575, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(1897, -575, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(2555, -581, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(2589, -94, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(2130, 270, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(2136, 778, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(1793, 1335, 40));
		AddSpawnPoint("d_underfortress_68.Id5", "d_underfortress_68", Rectangle(1999, 1660, 40));
	}
}
