//--- Melia Script -----------------------------------------------------------
// Sentry Bailey Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_underfortress_65'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress65MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_underfortress_65.Id1", MonsterId.Silvertransporter_Qm, min: 3, max: 4, tendency: TendencyType.Peaceful);
		AddSpawner("d_underfortress_65.Id2", MonsterId.Ticen_Blue, min: 6, max: 7, tendency: TendencyType.Peaceful);
		AddSpawner("d_underfortress_65.Id3", MonsterId.Ticen_Bow_Blue, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_65.Id4", MonsterId.Socket_Red, min: 23, max: 30, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_65.Id5", MonsterId.Silvertransporter_Qm, amount: 1, tendency: TendencyType.Peaceful);
		AddSpawner("d_underfortress_65.Id6", MonsterId.Silvertransporter_Qm, amount: 1, tendency: TendencyType.Peaceful);
		AddSpawner("d_underfortress_65.Id7", MonsterId.Silvertransporter_Qm, amount: 1, tendency: TendencyType.Peaceful);
		AddSpawner("d_underfortress_65.Id8", MonsterId.Silvertransporter_Qm, amount: 1, tendency: TendencyType.Peaceful);
		AddSpawner("d_underfortress_65.Id9", MonsterId.Socket_Bow_Purple, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_underfortress_65.Id10", MonsterId.Rootcrystal_05, min: 11, max: 14, respawn: Seconds(20), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Silvertransporter_Qm' GenType 6 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id1", "d_underfortress_65", Rectangle(458, -709, 20));

		// 'Ticen_Blue' GenType 10 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(942, 50, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(853, -411, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-53, -447, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(652, -941, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-756, -36, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(50, -1078, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-1280, 92, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(58, -157, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(462, 416, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-592, -560, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-678, -961, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-208, -936, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(781, -103, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-1511, 653, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-1869, 263, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-564, -253, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-61, -166, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(280, -721, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(626, -457, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(410, -71, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(355, -1132, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-601, -725, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-315, -624, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-968, -57, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(-1517, 258, 60));
		AddSpawnPoint("d_underfortress_65.Id2", "d_underfortress_65", Rectangle(424, 640, 60));

		// 'Ticen_Bow_Blue' GenType 11 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(442, 743, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(460, 353, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(574, -331, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(639, -581, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(467, -720, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(195, -721, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-250, -649, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-454, -581, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-569, -935, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-596, -773, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-49, -591, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(307, -1099, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-123, -1073, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(867, -125, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(1059, -184, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(835, -385, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(595, 514, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(303, 545, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-1019, -18, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-739, 6, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-1736, 168, 30));
		AddSpawnPoint("d_underfortress_65.Id3", "d_underfortress_65", Rectangle(-1470, 311, 30));

		// 'Socket_Red' GenType 12 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1528, 493, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-927, -2, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-623, -94, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-521, -565, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-611, -917, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-195, -933, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-59, -1249, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(253, -1047, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-85, -1097, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(175, -1151, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-49, -715, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(8, -466, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(154, -199, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-33, -132, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(273, -774, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(606, -339, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(378, -51, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(377, 468, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(582, 723, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(357, 627, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(990, 322, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(894, -157, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(1044, -178, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(890, -385, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(844, 45, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1809, 206, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1549, 240, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1818, 470, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1624, 38, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-594, -745, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-231, -640, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-394, -953, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-723, -95, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1060, -10, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1836, 340, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(30, -263, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-109, -452, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-50, -657, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-57, -583, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-182, -448, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(422, -702, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(648, -516, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(130, -1039, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(644, -973, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(892, -801, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(860, -203, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(795, -869, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(900, -567, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(466, 252, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(506, 79, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(488, 675, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(550, 533, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(606, -575, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-648, -290, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1253, 63, 30));
		AddSpawnPoint("d_underfortress_65.Id4", "d_underfortress_65", Rectangle(-1418, 845, 30));

		// 'Silvertransporter_Qm' GenType 17 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id5", "d_underfortress_65", Rectangle(965, -746, 20));

		// 'Silvertransporter_Qm' GenType 18 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id6", "d_underfortress_65", Rectangle(-22, -446, 20));

		// 'Silvertransporter_Qm' GenType 19 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id7", "d_underfortress_65", Rectangle(-649, -107, 20));

		// 'Silvertransporter_Qm' GenType 21 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id8", "d_underfortress_65", Rectangle(550, -1030, 20));

		// 'Socket_Bow_Purple' GenType 33 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id9", "d_underfortress_65", Rectangle(-560, -256, 9999));

		// 'Rootcrystal_05' GenType 37 Spawn Points
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(26, -1146, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(1009, -867, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(1022, -57, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(933, 410, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(-593, -1026, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(-767, -108, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(-1499, 170, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(-1530, 1015, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(105, -311, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(635, -742, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(488, 17, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(494, 606, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(263, 1151, 40));
		AddSpawnPoint("d_underfortress_65.Id10", "d_underfortress_65", Rectangle(28, 1564, 40));
	}
}
