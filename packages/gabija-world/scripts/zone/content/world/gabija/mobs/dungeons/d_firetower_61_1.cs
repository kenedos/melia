//--- Melia Script -----------------------------------------------------------
// Roxona Reconstruction Agency West Building Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_firetower_61_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower611MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_firetower_61_1.Id1", MonsterId.Raider_Bow, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_61_1.Id2", MonsterId.Socket_Bow_Brown, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_61_1.Id3", MonsterId.Anchor, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_61_1.Id4", MonsterId.Socket_Bow_Brown, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("d_firetower_61_1.Id5", MonsterId.Rootcrystal_04, min: 15, max: 20, respawn: Seconds(30), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Raider_Bow' GenType 1 Spawn Points
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(-124, 719, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(-100, 907, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(-113, 560, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(34, 740, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(-1018, 767, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(-994, 1117, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(-926, 906, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(759, 1028, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(764, 734, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(598, 1175, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(614, 843, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(-920, 619, 35));
		AddSpawnPoint("d_firetower_61_1.Id1", "d_firetower_61_1", Rectangle(-234, 743, 35));

		// 'Socket_Bow_Brown' GenType 2 Spawn Points
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-845, -1414, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-956, -1260, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-682, -1031, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1510, -667, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1378, -878, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1308, -637, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1913, -1106, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1724, -1098, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1295, -1704, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1145, -1684, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1283, -1579, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-757, -1217, 30));
		AddSpawnPoint("d_firetower_61_1.Id2", "d_firetower_61_1", Rectangle(-1283, -450, 30));

		// 'Anchor' GenType 3 Spawn Points
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(978, -479, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(687, -807, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(690, -1054, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(621, -1042, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(554, -1173, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(626, -1129, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(1098, -626, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(1066, -682, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(1152, -730, 30));
		AddSpawnPoint("d_firetower_61_1.Id3", "d_firetower_61_1", Rectangle(364, -1143, 30));

		// 'Socket_Bow_Brown' GenType 4 Spawn Points
		AddSpawnPoint("d_firetower_61_1.Id4", "d_firetower_61_1", Rectangle(-140, -1266, 9999));

		// 'Rootcrystal_04' GenType 28 Spawn Points
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-362, -1921, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(327, -1252, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-136, -1713, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(765, -1437, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(575, -1081, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(840, -729, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(1182, -390, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(1713, -102, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(1931, 212, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-141, -1020, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-781, -1240, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-1163, -1639, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-1790, -1063, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-1299, -768, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-83, 605, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-58, 955, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-617, 825, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-955, 733, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-903, 1052, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(753, 711, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(718, 1023, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(1042, 1410, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(1326, 1752, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(1254, 1854, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(-133, 1612, 100));
		AddSpawnPoint("d_firetower_61_1.Id5", "d_firetower_61_1", Rectangle(8, 1774, 100));
	}
}
