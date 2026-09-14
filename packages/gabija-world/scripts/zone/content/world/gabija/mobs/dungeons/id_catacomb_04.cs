//--- Melia Script -----------------------------------------------------------
// Underground Grave of Ritinis Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'id_catacomb_04'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb04MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("id_catacomb_04.Id1", MonsterId.Rootcrystal_01, min: 9, max: 11, respawn: Seconds(30), tendency: TendencyType.Peaceful);
		AddSpawner("id_catacomb_04.Id2", MonsterId.Candlespider_Yellow, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_04.Id3", MonsterId.Moyabu_Yellow, min: 12, max: 15, tendency: TendencyType.Aggressive);
		AddSpawner("id_catacomb_04.Id4", MonsterId.Ticen_Mage, min: 9, max: 12, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 5 Spawn Points
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(-389, -1535, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(-803, -1272, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(-7, -1217, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(-12, -640, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(945, -514, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(154, 0, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(-284, 642, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(-461, 1136, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(1581, 1034, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(2213, 1057, 100));
		AddSpawnPoint("id_catacomb_04.Id1", "id_catacomb_04", Rectangle(-1826, -707, 100));

		// 'Candlespider_Yellow' GenType 13 Spawn Points
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-1029, -777, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-1048, -506, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-737, -485, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-768, -810, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-772, -1531, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-756, -1234, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-36, -1563, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(15, -1530, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-1574, -763, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-1495, -729, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-1394, -452, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-1456, -481, 30));
		AddSpawnPoint("id_catacomb_04.Id2", "id_catacomb_04", Rectangle(-971, -416, 30));

		// 'Moyabu_Yellow' GenType 14 Spawn Points
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(-39, -735, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(172, -635, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(2, -547, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(25, -180, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(62, -31, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(-20, 313, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(-75, 595, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(-562, 970, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(-652, 673, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(-682, 1292, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(17, 1219, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(936, -374, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(993, -180, 30));
		AddSpawnPoint("id_catacomb_04.Id3", "id_catacomb_04", Rectangle(738, -643, 30));

		// 'Ticen_Mage' GenType 15 Spawn Points
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(-393, 664, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(-777, 951, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(-398, 1278, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(7, 933, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(406, 1189, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(762, 1172, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(611, 1221, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(1111, 1169, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(1162, 1198, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(-5, 464, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(-52, -264, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(181, -77, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(527, -648, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(936, -636, 30));
		AddSpawnPoint("id_catacomb_04.Id4", "id_catacomb_04", Rectangle(1149, -233, 30));
	}
}
