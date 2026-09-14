//--- Melia Script -----------------------------------------------------------
// Pilgrim Path Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_pilgrimroad_47'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad47MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_pilgrimroad_47.Id1", MonsterId.Triffid, min: 8, max: 10);
		AddSpawner("f_pilgrimroad_47.Id2", MonsterId.Rondo_Red, min: 23, max: 30);
		AddSpawner("f_pilgrimroad_47.Id3", MonsterId.Rubabos_Red, min: 3, max: 4);
		AddSpawner("f_pilgrimroad_47.Id4", MonsterId.Rondo_Red, min: 8, max: 10);
		AddSpawner("f_pilgrimroad_47.Id5", MonsterId.Rootcrystal_01, min: 4, max: 5, respawn: Seconds(5));
		AddSpawner("f_pilgrimroad_47.Id6", MonsterId.Spell_Crystal_Red, min: 9, max: 11);
		AddSpawner("f_pilgrimroad_47.Id7", MonsterId.Triffid, min: 15, max: 20);

		// Monster Spawn Points -----------------------------

		// 'Triffid' GenType 7 Spawn Points
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-993, 797, 25));
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-1335, 819, 25));
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-1297, 1080, 25));
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-1309, 1277, 25));
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-965, 1068, 25));
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-906, 643, 25));
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-725, 684, 25));
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-663, 991, 25));
		AddSpawnPoint("f_pilgrimroad_47.Id1", "f_pilgrimroad_47", Rectangle(-803, 1261, 25));

		// 'Rondo_Red' GenType 8 Spawn Points
		AddSpawnPoint("f_pilgrimroad_47.Id2", "f_pilgrimroad_47", Rectangle(95, 54, 9999));

		// 'Rubabos_Red' GenType 9 Spawn Points
		AddSpawnPoint("f_pilgrimroad_47.Id3", "f_pilgrimroad_47", Rectangle(1569, 620, 9999));

		// 'Rondo_Red' GenType 11 Spawn Points
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-929, -1038, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-1136, -1135, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-1184, -1333, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-904, -1387, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-880, -1209, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-689, -1323, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-704, -1103, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-518, -1162, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-793, -866, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id4", "f_pilgrimroad_47", Rectangle(-1159, -857, 30));

		// 'Rootcrystal_01' GenType 20 Spawn Points
		AddSpawnPoint("f_pilgrimroad_47.Id5", "f_pilgrimroad_47", Rectangle(-923, -1081, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id5", "f_pilgrimroad_47", Rectangle(-1025, 831, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id5", "f_pilgrimroad_47", Rectangle(1179, -351, 30));
		AddSpawnPoint("f_pilgrimroad_47.Id5", "f_pilgrimroad_47", Rectangle(1055, 735, 30));

		// 'Spell_Crystal_Red' GenType 23 Spawn Points
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(-81, -353, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(150, -459, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(392, -388, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(142, -142, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(40, 134, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(526, -31, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(-534, -1107, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(-732, -1343, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(-1085, -1330, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(-1110, -955, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(-913, -1482, 100));
		AddSpawnPoint("f_pilgrimroad_47.Id6", "f_pilgrimroad_47", Rectangle(-809, -1053, 100));

		// 'Triffid' GenType 33 Spawn Points
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1098, -414, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1329, -714, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1670, -711, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1682, -456, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1496, -246, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1151, -787, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1512, -871, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1798, -909, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1852, -336, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1887, -650, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(1453, -523, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-2235, -1915, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-2140, -1705, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-1886, -1985, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-2030, -1907, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-1896, -1793, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-2001, -1580, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-1783, -1655, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-1718, -1852, 20));
		AddSpawnPoint("f_pilgrimroad_47.Id7", "f_pilgrimroad_47", Rectangle(-1596, -1641, 20));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Glutton, "f_pilgrimroad_47", 1, Hours(2), Hours(4));
	}
}
