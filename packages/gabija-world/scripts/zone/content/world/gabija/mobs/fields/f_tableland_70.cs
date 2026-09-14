//--- Melia Script -----------------------------------------------------------
// Ibre Plateau Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_tableland_70'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland70MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_tableland_70.Id1", MonsterId.Hohen_Mane_Purple, min: 8, max: 10, respawn: Seconds(25));
		AddSpawner("f_tableland_70.Id2", MonsterId.Hohen_Mage_Blue, min: 9, max: 12, respawn: Seconds(25));
		AddSpawner("f_tableland_70.Id3", MonsterId.Cronewt_Blue, min: 8, max: 10, respawn: Seconds(25));
		AddSpawner("f_tableland_70.Id4", MonsterId.Lapasape_Bow_Blue, min: 15, max: 20, respawn: Seconds(25));
		AddSpawner("f_tableland_70.Id5", MonsterId.Rootcrystal_03, min: 20, max: 26, respawn: Seconds(30));
		AddSpawner("f_tableland_70.Id6", MonsterId.Lapasape_Bow_Blue, min: 15, max: 20, respawn: Minutes(1));

		// Monster Spawn Points -----------------------------

		// 'Hohen_Mane_Purple' GenType 1 Spawn Points
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2179, -2786, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2252, -2714, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(1991, -3028, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2664, -3468, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2764, -3282, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2835, -3426, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(1271, -3142, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(972, -3035, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(1245, -2963, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2343, -2041, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2538, -1857, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(1722, -3032, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2206, -1871, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2568, -2124, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(2561, -2273, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(3962, -4134, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(3898, -4347, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(3938, -2976, 25));
		AddSpawnPoint("f_tableland_70.Id1", "f_tableland_70", Rectangle(4127, -2442, 25));

		// 'Hohen_Mage_Blue' GenType 2 Spawn Points
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2828, -3529, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2465, -2824, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(1156, -3042, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(1294, -3067, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(1585, -3009, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2513, -2170, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2651, -2321, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2222, -2579, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2410, -1947, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(4423, -2879, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(4096, -2560, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(4245, -2707, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(4526, -2534, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(4105, -2707, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2143, -2996, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2730, -3347, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(3984, -3970, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(2673, -3562, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(4158, -4149, 25));
		AddSpawnPoint("f_tableland_70.Id2", "f_tableland_70", Rectangle(3929, -4220, 25));

		// 'Cronewt_Blue' GenType 3 Spawn Points
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2035, -2893, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2283, -2786, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2429, -2652, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2330, -3041, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2663, -3388, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2818, -3645, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2923, -3502, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2890, -3333, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2173, -1936, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2504, -1906, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(2481, -2018, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3580, -3805, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3568, -3195, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3841, -3910, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3755, -3928, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3812, -2099, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3881, -2012, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4057, -1788, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4276, -2008, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4141, -2031, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4248, -2596, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4035, -2889, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4486, -2695, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4294, -2919, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4183, -2438, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3437, -3475, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3610, -3245, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3408, -3284, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3579, -3463, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(1082, -3150, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(1077, -2950, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3347, -2942, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(4053, -4057, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3959, -3861, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3746, -3860, 25));
		AddSpawnPoint("f_tableland_70.Id3", "f_tableland_70", Rectangle(3355, -3643, 25));

		// 'Lapasape_Bow_Blue' GenType 4 Spawn Points
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(4136, -2807, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(4138, -1769, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3917, -1871, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2336, -2568, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(4110, -3828, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3965, -2126, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3335, -3372, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3377, -3202, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3247, -3494, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3573, -3326, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(4283, -2823, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(4399, -2582, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(4171, -1947, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3977, -1974, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2138, -2939, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2302, -2699, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2563, -2875, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2369, -2889, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2712, -3362, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2687, -3543, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2841, -3656, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2929, -3458, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3516, -3540, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(4224, -2581, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(3986, -1814, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2232, -2821, 25));
		AddSpawnPoint("f_tableland_70.Id4", "f_tableland_70", Rectangle(2559, -2175, 25));

		// 'Rootcrystal_03' GenType 7 Spawn Points
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(4362, -4438, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(4051, -4192, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(3719, -3880, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(3450, -3635, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(3699, -3309, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(3302, -3252, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(3490, -2612, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(3139, -2331, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(3489, -2048, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(3793, -2010, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(4250, -1995, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(4195, -1657, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(4160, -2537, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(4070, -2880, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(4503, -2784, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(2382, -2625, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(2139, -2828, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(2243, -3110, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(2576, -3556, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(2935, -3576, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(2798, -3249, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(2516, -2111, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(2338, -1778, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(1339, -3068, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(1043, -3205, 40));
		AddSpawnPoint("f_tableland_70.Id5", "f_tableland_70", Rectangle(938, -2932, 40));

		// 'Lapasape_Bow_Blue' GenType 12 Spawn Points
		AddSpawnPoint("f_tableland_70.Id6", "f_tableland_70", Rectangle(3215, -3293, 9999));
	}
}
