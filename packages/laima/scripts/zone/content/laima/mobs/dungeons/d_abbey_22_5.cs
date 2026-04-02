//--- Melia Script -----------------------------------------------------------
// Narvas Temple Annex Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_abbey_22_5'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey225MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------

		// Spawn Buffs -------------------------------------
		AddSpawnBuff("d_abbey_22_5", MonsterId.Hohen_Orben_Black, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_abbey_22_5", MonsterId.Harugal_Black, BuffId.EliteMonsterBuff, chance: 100);
		AddSpawnBuff("d_abbey_22_5", MonsterId.Hohen_Mage_Black, BuffId.EliteMonsterBuff, chance: 100);

		// Monster Spawners ---------------------------------

		AddSpawner("d_abbey_22_5.Id1", MonsterId.Rootcrystal_01, min: 16, max: 21, respawn: Minutes(1));
		AddSpawner("d_abbey_22_5.Id4", MonsterId.Drooper, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id7", MonsterId.Drooper, amount: 2, respawn: Seconds(40), tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id10", MonsterId.Drooper, amount: 1, respawn: Minutes(1), tendency: TendencyType.Aggressive);

		// Harugal_Black Roaming Spawn
		AddSpawner("d_abbey_22_5.Id11", MonsterId.Harugal_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Huge Room 1 (-25, 782)
		AddSpawner("d_abbey_22_5.Id12", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id13", MonsterId.Hohen_Orben_Black, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Huge Room 2 (-1213, 1644)
		AddSpawner("d_abbey_22_5.Id14", MonsterId.Hohen_Mage_Black, min: 3, max: 4, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id15", MonsterId.Hohen_Orben_Black, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Huge Room 3 (30, -96)
		AddSpawner("d_abbey_22_5.Id16", MonsterId.Hohen_Mage_Black, min: 3, max: 4, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id17", MonsterId.Hohen_Orben_Black, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Huge Room 4 (-1005, -74)
		AddSpawner("d_abbey_22_5.Id18", MonsterId.Hohen_Mage_Black, min: 3, max: 4, tendency: TendencyType.Aggressive);

		// Huge Room 5 (1071, -917)
		AddSpawner("d_abbey_22_5.Id19", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id20", MonsterId.Hohen_Orben_Black, min: 1, max: 2, tendency: TendencyType.Aggressive);

		// Normal Room 6 (-1849, -199)
		AddSpawner("d_abbey_22_5.Id21", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Normal Room 7 (-1470, -883)
		AddSpawner("d_abbey_22_5.Id22", MonsterId.Hohen_Mage_Black, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id23", MonsterId.Hohen_Orben_Black, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Normal Room 8 (-856, -822)
		AddSpawner("d_abbey_22_5.Id24", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Normal Room 9 (1080, 1391)
		AddSpawner("d_abbey_22_5.Id25", MonsterId.Hohen_Mage_Black, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id26", MonsterId.Hohen_Orben_Black, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Normal Room 10 (1793, 826)
		AddSpawner("d_abbey_22_5.Id27", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Normal Room 11 (-1088, 884)
		AddSpawner("d_abbey_22_5.Id28", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Normal Room 12 (-1789, 896)
		AddSpawner("d_abbey_22_5.Id29", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Normal Room 13 (1084, -33)
		AddSpawner("d_abbey_22_5.Id30", MonsterId.Hohen_Mage_Black, min: 1, max: 2, tendency: TendencyType.Aggressive);
		AddSpawner("d_abbey_22_5.Id31", MonsterId.Hohen_Orben_Black, min: 1, max: 1, tendency: TendencyType.Aggressive);

		// Normal Room 14 (1785, -4)
		AddSpawner("d_abbey_22_5.Id32", MonsterId.Hohen_Mage_Black, min: 2, max: 3, tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 28 Spawn Points
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-371, 794, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(304, 795, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(1022, 710, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(1300, 846, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(1067, 1363, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(1783, 796, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(797, -39, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(1289, -39, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(809, -900, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(1266, -957, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(1, -73, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-836, -28, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-1286, -38, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-1888, -228, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-1553, -789, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-1176, -836, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-876, -846, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-1776, 931, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-1164, 884, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-1453, 1738, 10));
		AddSpawnPoint("d_abbey_22_5.Id1", "d_abbey_22_5", Rectangle(-949, 1636, 10));

		// 'Harugal_Black' Roaming Spawn
		AddSpawnPoint("d_abbey_22_5.Id11", "d_abbey_22_5", Rectangle(30, -96, 9999));

		// 'Drooper' GenType 31 Spawn Points
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1738, 767, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1735, 1047, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1323, 719, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1183, 723, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1058, 721, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1446, 1571, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1466, 1842, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-992, 1567, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-946, 1844, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1952, -292, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1718, -44, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1947, -565, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1789, -702, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-578, -17, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(597, -23, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(1479, -6, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(1074, -434, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1224, 1462, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(-1223, 1725, 20));
		AddSpawnPoint("d_abbey_22_5.Id4", "d_abbey_22_5", Rectangle(1076, -691, 20));

		// Elite Room Spawn Points

		// Huge Room 1 (-25, 782)
		AddSpawnPoint("d_abbey_22_5.Id12", "d_abbey_22_5", Rectangle(-25, 782, 500));
		AddSpawnPoint("d_abbey_22_5.Id13", "d_abbey_22_5", Rectangle(-25, 782, 500));

		// Huge Room 2 (-1213, 1644)
		AddSpawnPoint("d_abbey_22_5.Id14", "d_abbey_22_5", Rectangle(-1213, 1644, 500));
		AddSpawnPoint("d_abbey_22_5.Id15", "d_abbey_22_5", Rectangle(-1213, 1644, 500));

		// Huge Room 3 (30, -96)
		AddSpawnPoint("d_abbey_22_5.Id16", "d_abbey_22_5", Rectangle(30, -96, 500));
		AddSpawnPoint("d_abbey_22_5.Id17", "d_abbey_22_5", Rectangle(30, -96, 500));

		// Huge Room 4 (-1005, -74)
		AddSpawnPoint("d_abbey_22_5.Id18", "d_abbey_22_5", Rectangle(-1005, -74, 500));

		// Huge Room 5 (1071, -917)
		AddSpawnPoint("d_abbey_22_5.Id19", "d_abbey_22_5", Rectangle(1071, -917, 500));
		AddSpawnPoint("d_abbey_22_5.Id20", "d_abbey_22_5", Rectangle(1071, -917, 500));

		// Normal Room 6 (-1849, -199)
		AddSpawnPoint("d_abbey_22_5.Id21", "d_abbey_22_5", Rectangle(-1849, -199, 400));

		// Normal Room 7 (-1470, -883)
		AddSpawnPoint("d_abbey_22_5.Id22", "d_abbey_22_5", Rectangle(-1470, -883, 400));
		AddSpawnPoint("d_abbey_22_5.Id23", "d_abbey_22_5", Rectangle(-1470, -883, 400));

		// Normal Room 8 (-856, -822)
		AddSpawnPoint("d_abbey_22_5.Id24", "d_abbey_22_5", Rectangle(-856, -822, 400));

		// Normal Room 9 (1080, 1391)
		AddSpawnPoint("d_abbey_22_5.Id25", "d_abbey_22_5", Rectangle(1080, 1391, 400));
		AddSpawnPoint("d_abbey_22_5.Id26", "d_abbey_22_5", Rectangle(1080, 1391, 400));

		// Normal Room 10 (1793, 826)
		AddSpawnPoint("d_abbey_22_5.Id27", "d_abbey_22_5", Rectangle(1793, 826, 400));

		// Normal Room 11 (-1088, 884)
		AddSpawnPoint("d_abbey_22_5.Id28", "d_abbey_22_5", Rectangle(-1088, 884, 400));

		// Normal Room 12 (-1789, 896)
		AddSpawnPoint("d_abbey_22_5.Id29", "d_abbey_22_5", Rectangle(-1789, 896, 400));

		// Normal Room 13 (1084, -33)
		AddSpawnPoint("d_abbey_22_5.Id30", "d_abbey_22_5", Rectangle(1084, -33, 400));
		AddSpawnPoint("d_abbey_22_5.Id31", "d_abbey_22_5", Rectangle(1084, -33, 400));

		// Normal Room 14 (1785, -4)
		AddSpawnPoint("d_abbey_22_5.Id32", "d_abbey_22_5", Rectangle(1785, -4, 400));

		// 'Drooper' GenType 35 Spawn Points
		AddSpawnPoint("d_abbey_22_5.Id7", "d_abbey_22_5", Rectangle(-1218, 1607, 20));
		AddSpawnPoint("d_abbey_22_5.Id7", "d_abbey_22_5", Rectangle(-1011, 1767, 20));

		// 'Drooper' GenType 38 Spawn Points
		AddSpawnPoint("d_abbey_22_5.Id10", "d_abbey_22_5", Rectangle(-1556, -788, 20));
		AddSpawnPoint("d_abbey_22_5.Id10", "d_abbey_22_5", Rectangle(-1344, -912, 20));
		AddSpawnPoint("d_abbey_22_5.Id10", "d_abbey_22_5", Rectangle(-1442, -709, 20));
		AddSpawnPoint("d_abbey_22_5.Id10", "d_abbey_22_5", Rectangle(-1368, -751, 20));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Durahan, "d_abbey_22_5", 1, Hours(2), Hours(4));
	}
}
