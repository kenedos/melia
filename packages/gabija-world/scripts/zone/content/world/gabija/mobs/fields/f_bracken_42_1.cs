//--- Melia Script -----------------------------------------------------------
// Khonot Forest Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_bracken_42_1'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken421MobScript : GeneralScript
{
	protected override void Load()
	{
		// Monster Spawners ---------------------------------

		AddSpawner("f_bracken_42_1.Id1", MonsterId.Rootcrystal_01, min: 12, max: 15, respawn: Seconds(5));
		AddSpawner("f_bracken_42_1.Id2", MonsterId.Tanu_Blue, min: 6, max: 8);
		AddSpawner("f_bracken_42_1.Id3", MonsterId.Doyor_Blue, min: 12, max: 15);
		AddSpawner("f_bracken_42_1.Id4", MonsterId.Gosaru_Blue, min: 6, max: 8);
		AddSpawner("f_bracken_42_1.Id5", MonsterId.Folibu_Yellow, min: 9, max: 12);
		AddSpawner("f_bracken_42_1.Id6", MonsterId.Gosaru_Blue, min: 12, max: 15);

		// Monster Spawn Points -----------------------------

		// 'Rootcrystal_01' GenType 5 Spawn Points
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-1019, -528, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-813, -482, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-824, -295, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-506, 85, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-467, 448, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-1183, 289, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-344, -341, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-446, -625, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(-743, 893, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(63, 822, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(168, 961, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(762, 674, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(1079, 563, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(274, 422, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(356, -178, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(92, 86, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(134, -450, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(905, -599, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(711, -172, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(1004, -2, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(1787, -172, 50));
		AddSpawnPoint("f_bracken_42_1.Id1", "f_bracken_42_1", Rectangle(2007, -389, 50));

		// 'Tanu_Blue' GenType 100 Spawn Points
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-906, -327, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-1015, -422, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-991, -582, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-815, -465, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-1044, -572, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-913, -519, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-898, -429, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-520, 87, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-861, 174, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-728, 218, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-572, 223, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-1152, 341, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-476, 176, 30));
		AddSpawnPoint("f_bracken_42_1.Id2", "f_bracken_42_1", Rectangle(-963, 272, 30));

		// 'Doyor_Blue' GenType 101 Spawn Points
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-603, 239, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-554, 57, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-423, -32, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-415, 226, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-475, -547, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-416, -717, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-369, -516, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-384, -395, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-223, -510, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-213, -580, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-498, -480, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-760, 772, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-882, 848, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-766, 936, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-751, 848, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-687, 619, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-646, 795, 30));
		AddSpawnPoint("f_bracken_42_1.Id3", "f_bracken_42_1", Rectangle(-290, -453, 30));

		// 'Gosaru_Blue' GenType 103 Spawn Points
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(74, 91, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(-22, -142, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(146, -255, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(325, -277, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(340, 18, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(167, -157, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(64, 9, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(305, -80, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(417, -174, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(208, 220, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(-4, 767, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(68, 891, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(104, 799, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(180, 924, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(192, 532, 30));
		AddSpawnPoint("f_bracken_42_1.Id4", "f_bracken_42_1", Rectangle(213, 346, 30));

		// 'Folibu_Yellow' GenType 104 Spawn Points
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(843, 739, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(731, 603, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(1023, 493, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(1079, 660, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(830, 464, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(907, 540, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(956, 674, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(1080, 791, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(878, 318, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(1110, 473, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(748, -411, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(880, -533, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(869, -392, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(730, -129, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(887, 12, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(1043, -126, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(987, -122, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(854, -146, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(750, 8, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(993, -271, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(702, -268, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(855, -255, 30));
		AddSpawnPoint("f_bracken_42_1.Id5", "f_bracken_42_1", Rectangle(1016, 803, 30));

		// 'Gosaru_Blue' GenType 105 Spawn Points
		AddSpawnPoint("f_bracken_42_1.Id6", "f_bracken_42_1", Rectangle(188, 362, 9999));
	}
}
