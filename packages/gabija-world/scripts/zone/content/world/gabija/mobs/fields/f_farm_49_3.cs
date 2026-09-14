//--- Melia Script -----------------------------------------------------------
// Shaton Reservoir Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_farm_49_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm493MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_farm_49_3.Id1", MonsterId.Melatanun, min: 15, max: 19, respawn: Seconds(20), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_3.Id2", MonsterId.Melatanun, min: 19, max: 25, respawn: Seconds(30), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_3.Id3", MonsterId.Carcashu_Green, min: 9, max: 12, respawn: Seconds(20), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_3.Id4", MonsterId.Tree_Root_Mole_Pink, min: 12, max: 15, respawn: Seconds(30), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_3.Id5", MonsterId.Tree_Root_Mole_Pink, min: 12, max: 15, respawn: Seconds(35), tendency: TendencyType.Aggressive);
		AddSpawner("f_farm_49_3.Id6", MonsterId.Rootcrystal_01, min: 8, max: 10, respawn: Seconds(5), tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Melatanun' GenType 2 Spawn Points
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(868, -744, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(800, -540, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(938, -621, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(-62, -635, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(36, -520, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(187, -637, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(-53, -218, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(42, -11, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(-176, -124, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(1509, -99, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(1599, 40, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(1429, 51, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(84, -644, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(116, -188, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(-61, -84, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(18, -778, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(1069, -615, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(1509, -244, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(1501, 172, 25));
		AddSpawnPoint("f_farm_49_3.Id1", "f_farm_49_3", Rectangle(1646, -161, 25));

		// 'Melatanun' GenType 5 Spawn Points
		AddSpawnPoint("f_farm_49_3.Id2", "f_farm_49_3", Rectangle(705, -83, 9999));

		// 'Carcashu_Green' GenType 6 Spawn Points
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(785, -653, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(978, -682, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(909, -507, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(-76, -557, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(61, -768, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(246, -539, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(-183, -233, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(-81, 27, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(1500, -197, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(1575, 30, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(20, -255, 25));
		AddSpawnPoint("f_farm_49_3.Id3", "f_farm_49_3", Rectangle(1438, 60, 25));

		// 'Tree_Root_Mole_Pink' GenType 54 Spawn Points
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1430, 1029, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1474, 1225, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1328, 1400, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1257, 1187, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-469, 1706, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-371, 1947, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(68, 1785, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-189, 1699, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-185, 1870, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1155, -1058, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1432, -1292, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1322, -1451, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1770, -201, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1629, 42, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1452, -235, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1365, 2, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1167, -869, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1563, -102, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1863, -57, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(-1583, 1414, 30));
		AddSpawnPoint("f_farm_49_3.Id4", "f_farm_49_3", Rectangle(71, 1950, 30));

		// 'Tree_Root_Mole_Pink' GenType 57 Spawn Points
		AddSpawnPoint("f_farm_49_3.Id5", "f_farm_49_3", Rectangle(-1210, -1135, 9999));

		// 'Rootcrystal_01' GenType 58 Spawn Points
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(879, -626, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(38, -652, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-11, -242, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(1480, -52, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-1107, -1103, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-1373, -1370, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-1817, -845, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-1843, -126, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-1403, -189, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-1435, 1117, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-852, 804, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-346, 1871, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(-13, 1837, 50));
		AddSpawnPoint("f_farm_49_3.Id6", "f_farm_49_3", Rectangle(968, 1254, 50));
	}
}
