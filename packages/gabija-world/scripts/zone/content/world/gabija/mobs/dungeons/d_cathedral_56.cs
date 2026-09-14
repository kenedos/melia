//--- Melia Script -----------------------------------------------------------
// Sanctuary Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_cathedral_56'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DCathedral56MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("d_cathedral_56.Id1", MonsterId.Pawnd_Purple, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_56.Id2", MonsterId.Pawndel_Blue, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_56.Id3", MonsterId.Rootcrystal_03, min: 19, max: 25, respawn: Minutes(1), tendency: TendencyType.Peaceful);
		AddSpawner("d_cathedral_56.Id4", MonsterId.Pawnd_Purple, min: 6, max: 8, tendency: TendencyType.Aggressive);
		AddSpawner("d_cathedral_56.Id5", MonsterId.NightMaiden_Bow, min: 6, max: 8, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Pawnd_Purple' GenType 2 Spawn Points
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-965, -50, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-1519, -546, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-1102, 50, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-981, 197, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-624, -46, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-1536, 93, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-369, -87, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-1049, -215, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-1555, -348, 25));
		AddSpawnPoint("d_cathedral_56.Id1", "d_cathedral_56", Rectangle(-1546, -90, 25));

		// 'Pawndel_Blue' GenType 4 Spawn Points
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(66, -127, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(-1029, -72, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(434, 210, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(508, 646, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(1124, 893, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(1425, 604, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(768, 927, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(196, -372, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(-372, -90, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(-645, -87, 30));
		AddSpawnPoint("d_cathedral_56.Id2", "d_cathedral_56", Rectangle(192, 157, 30));

		// 'Rootcrystal_03' GenType 32 Spawn Points
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(1672, -469, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(2049, -538, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(1911, -84, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(1956, 445, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(1622, 386, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(1425, 632, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(1243, 893, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(746, 906, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(496, 671, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(757, 186, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(215, 89, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-120, -131, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-434, -111, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-1027, -43, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-926, -536, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-468, -635, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-207, -965, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-1282, -523, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-1549, -703, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-1837, -638, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-2084, -506, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-2057, -124, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-1503, -305, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-1567, -77, 10));
		AddSpawnPoint("d_cathedral_56.Id3", "d_cathedral_56", Rectangle(-1571, 171, 10));

		// 'Pawnd_Purple' GenType 33 Spawn Points
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(1703, -403, 30));
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(1766, 413, 30));
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(-2094, -501, 30));
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(-2291, 428, 30));
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(-2104, -688, 30));
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(-2166, -296, 30));
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(-2011, -97, 30));
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(-2030, -211, 30));
		AddSpawnPoint("d_cathedral_56.Id4", "d_cathedral_56", Rectangle(-2176, 436, 30));

		// 'NightMaiden_Bow' GenType 42 Spawn Points
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1535, 378, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1806, -499, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1384, 496, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1779, 574, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1486, -378, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1547, -578, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1885, -369, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1397, 787, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(820, 912, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(1874, 248, 30));
		AddSpawnPoint("d_cathedral_56.Id5", "d_cathedral_56", Rectangle(2138, 95, 30));
	}
}
