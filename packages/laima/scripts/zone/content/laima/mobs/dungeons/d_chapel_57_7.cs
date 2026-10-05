//--- Melia Script -----------------------------------------------------------
// Tenet Church 2F Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_chapel_57_7'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DChapel577MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------


		// Monster Spawners ---------------------------------

		AddSpawner("d_chapel_57_7.Id1", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(30), tendency: TendencyType.Peaceful);
		AddSpawner("d_chapel_57_7.Id2", MonsterId.Spector_Gh, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_chapel_57_7.Id3", MonsterId.Egnome, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_chapel_57_7.Id4", MonsterId.Spector_Gh, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_chapel_57_7.Id5", MonsterId.Egnome, min: 15, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_chapel_57_7.Id6", MonsterId.Colitile, min: 30, max: 40, tendency: TendencyType.Aggressive);
		AddSpawner("d_chapel_57_7.Id7", MonsterId.Infroholder_Bow, min: 15, max: 20, tendency: TendencyType.Peaceful);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 21 Spawn Points
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-312, -984, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-331, -688, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-17, -794, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-987, -262, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-807, 36, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-425, -113, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(66, -118, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(461, -122, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(831, -190, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(957, -64, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(1412, -144, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-92, 390, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(18, 649, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(99, 1181, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-121, 1352, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(1644, -2, 200));
		AddSpawnPoint("d_chapel_57_7.Id1", "d_chapel_57_7", Rectangle(-369, -458, 200));

		// 'Spector_Gh' GenType 31 Spawn Points
		AddSpawnPoint("d_chapel_57_7.Id2", "d_chapel_57_7", Rectangle(13, 641, 200));
		AddSpawnPoint("d_chapel_57_7.Id2", "d_chapel_57_7", Rectangle(-35, 314, 200));
		AddSpawnPoint("d_chapel_57_7.Id2", "d_chapel_57_7", Rectangle(689, -112, 200));
		AddSpawnPoint("d_chapel_57_7.Id2", "d_chapel_57_7", Rectangle(976, -114, 200));

		// 'Egnome' GenType 52 Spawn Points
		AddSpawnPoint("d_chapel_57_7.Id3", "d_chapel_57_7", Rectangle(1645, -32, 100));
		AddSpawnPoint("d_chapel_57_7.Id3", "d_chapel_57_7", Rectangle(1617, -231, 100));
		AddSpawnPoint("d_chapel_57_7.Id3", "d_chapel_57_7", Rectangle(1435, -266, 100));
		AddSpawnPoint("d_chapel_57_7.Id3", "d_chapel_57_7", Rectangle(1396, 13, 100));
		AddSpawnPoint("d_chapel_57_7.Id3", "d_chapel_57_7", Rectangle(1528, 32, 100));
		AddSpawnPoint("d_chapel_57_7.Id3", "d_chapel_57_7", Rectangle(1319, -111, 100));

		// 'Spector_Gh' GenType 53 Spawn Points
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(804, -225, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(1036, -136, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(725, -14, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(-653, -161, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(-466, -61, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(-1002, -237, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(-1037, 25, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(-1161, -80, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(-662, -62, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(834, -121, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(903, 54, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(1453, 17, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(1667, -92, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(1413, -233, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(1590, -223, 80));
		AddSpawnPoint("d_chapel_57_7.Id4", "d_chapel_57_7", Rectangle(684, -193, 80));

		// 'Egnome' GenType 55 Spawn Points
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-1009, 85, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-577, -85, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-1033, -292, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(21, 600, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-34, 271, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-59, 1184, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-107, 1477, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(141, 1516, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(163, 1242, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-739, -31, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-796, -177, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-1019, -5, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-1031, -204, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-1077, -113, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-682, -195, 100));
		AddSpawnPoint("d_chapel_57_7.Id5", "d_chapel_57_7", Rectangle(-831, 16, 100));

		// 'Colitile' GenType 56 Spawn Points
		AddSpawnPoint("d_chapel_57_7.Id6", "d_chapel_57_7", Rectangle(-72, 625, 9999));

		// 'Infroholder_Bow' GenType 58 Spawn Points
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(1388, -219, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(1472, 25, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(1593, -229, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(1612, 22, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-1056, -299, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-1095, 49, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-1137, -199, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-141, 1235, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-19, 1069, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(107, 1283, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-843, 80, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(912, -172, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(845, 6, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(713, -178, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-150, 673, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-4, 393, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(113, 652, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-882, -202, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-965, 76, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(1620, -106, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-158, 477, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-851, -75, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-983, -222, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-1117, -98, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-839, -273, 40));
		AddSpawnPoint("d_chapel_57_7.Id7", "d_chapel_57_7", Rectangle(-783, -15, 40));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Necrovanter, "d_chapel_57_7", 1, Hours(2), Hours(4));
	}
}
