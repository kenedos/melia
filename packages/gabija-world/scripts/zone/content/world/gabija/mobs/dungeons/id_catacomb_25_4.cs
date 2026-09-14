//--- Melia Script -----------------------------------------------------------
// Timerys Temple Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'id_catacomb_25_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb254MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("id_catacomb_25_4.Id1", MonsterId.Pagclamper_Yellow, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_25_4.Id2", MonsterId.PagDoper_Blue, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_25_4.Id3", MonsterId.PagNurse_Green, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_25_4.Id4", MonsterId.Pagshearer_Yellow, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_25_4.Id5", MonsterId.Rootcrystal_02, min: 19, max: 25, respawn: Seconds(30), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Pagclamper_Yellow' GenType 22 Spawn Points
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(1004, 997, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(1040, 917, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(842, 939, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(727, 935, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(609, 949, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(476, 937, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(527, 979, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(498, 633, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(489, 732, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(597, 732, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(595, 653, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(228, 714, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(228, 866, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(87, 867, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(69, 711, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-72, 697, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-74, 867, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(171, 795, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-11, 793, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-432, 723, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-336, 746, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-673, 728, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-789, 733, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-893, 596, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-690, 543, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-883, 374, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-747, 371, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-791, 478, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-788, 633, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-939, 838, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1220, 611, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1144, 552, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1858, 439, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1740, 481, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1666, 421, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1556, 499, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1689, 603, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1719, 813, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1485, 604, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1872, 663, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1880, 781, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1759, 728, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-1587, 703, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(305, 134, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(233, 31, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(246, 398, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(118, 378, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-55, 390, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(9, 465, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-78, -376, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(123, -593, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(239, -354, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(130, -412, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-90, -171, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(274, -205, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(-130, -509, 40));
		AddSpawnPoint("id_catacomb_25_4.Id1", "id_catacomb_25_4", Rectangle(540, 590, 40));

		// 'PagDoper_Blue' GenType 24 Spawn Points
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-30, -1031, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-92, -897, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(146, -921, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(168, -1034, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(75, -657, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(256, -619, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(268, -354, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(91, -458, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-84, -159, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(253, -145, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(233, 98, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-30, 73, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(160, 437, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-7, 457, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-360, 386, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(241, 348, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(86, 292, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(108, -200, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-85, -335, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-50, -573, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-135, 351, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(315, 465, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-370, 474, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-457, 400, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-879, 338, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-726, 366, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-788, 482, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-916, 538, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-741, 773, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-526, 708, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-673, 533, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-799, 645, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-106, -682, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(841, -698, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(-123, -1073, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(850, -549, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(868, -289, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(1004, -266, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(1078, -613, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(1037, -756, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(945, -812, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(949, -623, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(981, -498, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(755, -391, 40));
		AddSpawnPoint("id_catacomb_25_4.Id2", "id_catacomb_25_4", Rectangle(714, -578, 40));

		// 'PagNurse_Green' GenType 25 Spawn Points
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-939, -418, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-751, -366, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-702, -182, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-845, -178, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-971, -32, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-902, 126, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-701, 143, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-768, -17, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-963, -270, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-628, -551, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-395, -632, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1161, 51, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1246, 105, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1351, 46, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1432, 104, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1462, 31, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1750, -39, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1610, 57, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1717, 193, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1763, -533, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1684, -285, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1744, -370, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1601, -453, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1563, -547, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1370, -412, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1231, -401, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1231, -483, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-1085, -451, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-828, -515, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-70, -1034, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(186, -997, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(57, -906, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(57, -733, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-86, -647, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-3, -470, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-67, -300, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(107, -82, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(255, -197, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(173, -482, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(198, -663, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-109, -397, 40));
		AddSpawnPoint("id_catacomb_25_4.Id3", "id_catacomb_25_4", Rectangle(-122, -129, 40));

		// 'Pagshearer_Yellow' GenType 26 Spawn Points
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-1805, -515, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-1615, -516, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-1618, -301, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-1306, -413, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-1235, -413, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-1244, -486, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-978, -457, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-832, -204, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-966, -238, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-790, -408, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-649, -246, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-846, -34, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-770, 101, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-898, 120, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-1234, 77, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-1095, 44, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-642, -569, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-406, -599, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-239, -618, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-109, -686, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(2, -523, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(189, -455, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-77, -400, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(76, -715, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(232, -672, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(41, -915, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-87, -1038, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(137, -1074, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(248, -983, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(195, -878, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-73, -875, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(438, -617, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(585, -496, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(595, -595, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(907, -260, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(1027, -388, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(1024, -570, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(801, -312, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(947, -454, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(807, -496, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(914, -853, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(1019, -741, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(754, -743, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-683, -64, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(56, -263, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(204, -229, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(230, -104, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(-65, -124, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(517, -571, 30));
		AddSpawnPoint("id_catacomb_25_4.Id4", "id_catacomb_25_4", Rectangle(917, -681, 30));

		// 'Rootcrystal_02' GenType 27 Spawn Points
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(1292, 454, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(1234, 748, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(975, 941, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(595, 882, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(472, 685, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(118, 768, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-123, 785, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-590, 789, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-849, 729, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-815, 321, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-1217, 541, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-1457, 730, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-1656, 458, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-1912, 803, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-307, 410, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(22, 381, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(302, 62, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-120, -227, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(235, -521, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-85, -777, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(266, -1070, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-634, -504, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-783, -125, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-1394, 10, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-1536, -372, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(-1869, 194, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(848, -826, 15));
		AddSpawnPoint("id_catacomb_25_4.Id5", "id_catacomb_25_4", Rectangle(1064, -493, 15));
	}
}
