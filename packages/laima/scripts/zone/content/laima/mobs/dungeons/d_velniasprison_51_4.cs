//--- Melia Script -----------------------------------------------------------
// Demon Prison District 3 Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'd_velniasprison_51_4'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison514MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------


		// Monster Spawners ---------------------------------

		AddSpawner("d_velniasprison_51_4.Id1", MonsterId.Elma, min: 50, max: 70, tendency: TendencyType.Aggressive);
		AddSpawner("d_velniasprison_51_4.Id2", MonsterId.Elma, min: 50, max: 70, tendency: TendencyType.Aggressive);
		AddSpawner("d_velniasprison_51_4.Id3", MonsterId.Nuo, min: 50, max: 70, tendency: TendencyType.Aggressive);
		AddSpawner("d_velniasprison_51_4.Id4", MonsterId.Nuo, min: 50, max: 70, tendency: TendencyType.Aggressive);
		AddSpawner("d_velniasprison_51_4.Id5", MonsterId.Harugal, min: 10, max: 20, tendency: TendencyType.Aggressive);
		AddSpawner("d_velniasprison_51_4.Id6", MonsterId.Rootcrystal_05, min: 14, max: 18, respawn: Seconds(30), tendency: TendencyType.Peaceful);
		AddSpawner("d_velniasprison_51_4.Id7", MonsterId.Mushroom_Ent_Green, min: 4, max: 5, tendency: TendencyType.Aggressive);
		AddSpawner("d_velniasprison_51_4.Id8", MonsterId.Harugal, min: 6, max: 9, respawn: Seconds(20), tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Elma' GenType 19 Spawn Points
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2583, 1556, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2715, 1757, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2524, 1821, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-3137, 120, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2842, 183, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2621, -3, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2407, 151, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-3488, 326, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2844, 844, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2828, 649, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-3071, 1507, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-3396, 1201, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2748, 1620, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2570, 140, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2585, 899, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2637, 764, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-3385, 113, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-3449, 689, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2488, 1667, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2355, 1417, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2102, 1282, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2065, 1147, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-1986, 1472, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-1889, 1354, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-1922, 1225, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2976, 744, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-3414, 842, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id1", "d_velniasprison_51_4", Rectangle(-2495, 657, 40));

		// 'Elma' GenType 20 Spawn Points
		AddSpawnPoint("d_velniasprison_51_4.Id2", "d_velniasprison_51_4", Rectangle(-2001, 1308, 9999));

		// 'Nuo' GenType 21 Spawn Points
		AddSpawnPoint("d_velniasprison_51_4.Id3", "d_velniasprison_51_4", Rectangle(240, -175, 9999));

		// 'Nuo' GenType 22 Spawn Points
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(3, 358, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-786, -422, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-293, 364, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(105, -103, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-782, -1041, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-1437, -963, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-1464, 74, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-1300, 418, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-600, -287, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-400, -148, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-1580, -596, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-1045, -1011, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-853, -858, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-601, -1009, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-681, -1241, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(122, -332, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(345, -267, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(200, -882, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(21, -931, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-66, -876, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-640, -132, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-559, -566, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(217, 338, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(-399, -365, 40));
		AddSpawnPoint("d_velniasprison_51_4.Id4", "d_velniasprison_51_4", Rectangle(240, -195, 40));

		// 'Socket' GenType 26 Spawn Points
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-569, -472, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-525, -218, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-767, -465, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-1278, -987, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(135, -268, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-789, -1020, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-58, -888, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(225, -828, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-847, -156, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(35, -43, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-51, 377, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(203, 380, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(293, -243, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-485, -1066, 50));
		AddSpawnPoint("d_velniasprison_51_4.Id5", "d_velniasprison_51_4", Rectangle(-641, -1242, 50));

		// 'Rootcrystal_05' GenType 27 Spawn Points
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-1012, 1079, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-831, -398, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-458, -313, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-1602, -281, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-786, -1125, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(328, -830, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(240, -235, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-589, 366, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-1498, -964, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-2023, 657, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-1985, 1308, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-2483, 1748, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-2708, 1662, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-3522, 949, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-3407, 663, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-3264, 66, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-2809, 76, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-2875, 645, 200));
		AddSpawnPoint("d_velniasprison_51_4.Id6", "d_velniasprison_51_4", Rectangle(-2593, 851, 200));

		// 'Mushroom_Ent_Green' GenType 30 Spawn Points
		AddSpawnPoint("d_velniasprison_51_4.Id7", "d_velniasprison_51_4", Rectangle(-1360, 368, 9999));

		// Harugal Spawn Points
		AddSpawnPoint("d_velniasprison_51_4.Id8", "d_velniasprison_51_4", Rectangle(71, 743, 200));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Dionys, "d_velniasprison_51_4", 1, Hours(2), Hours(4));
	}
}
