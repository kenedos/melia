//--- Melia Script -----------------------------------------------------------
// Nefritas Cliff Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners for 'f_gele_57_3'.
//---------------------------------------------------------------------------

using System;
using Melia.Zone.Scripting;
using Melia.Shared.Game.Const;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele573MobScript : GeneralScript
{
	protected override void Load()
	{
		// Property Overrides -------------------------------


		// Monster Spawners ---------------------------------

		AddSpawner("f_gele_57_3.Id1", MonsterId.Zigri_Brown, min: 15, max: 20);
		AddSpawner("f_gele_57_3.Id2", MonsterId.Banshee, min: 15, max: 20);
		AddSpawner("f_gele_57_3.Id3", MonsterId.Firent, amount: 3);
		AddSpawner("f_gele_57_3.Id4", MonsterId.Puragi_Green, min: 10, max: 13, respawn: Seconds(30), tendency: TendencyType.Aggressive);
		AddSpawner("f_gele_57_3.Id5", MonsterId.Rootcrystal_01, min: 10, max: 13, respawn: Minutes(1));
		AddSpawner("f_gele_57_3.Id6", MonsterId.Banshee, min: 8, max: 10, tendency: TendencyType.Aggressive);
		AddSpawner("f_gele_57_3.Id7", MonsterId.Humming_Bud, min: 8, max: 10);
		AddSpawner("f_gele_57_3.Id8", MonsterId.Deadbornscab_Mage, min: 6, max: 8);
		AddSpawner("f_gele_57_3.Id9", MonsterId.Puragi_Green, min: 8, max: 10, respawn: Seconds(30), tendency: TendencyType.Aggressive);

		// Monster Spawn Points -----------------------------

		// 'Zigri_Brown' GenType 19 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(945, -510, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(923, 32, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(810, 14, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(847, 189, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(955, 143, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(782, 110, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(815, -454, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(708, -565, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(767, -721, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(1014, -658, 20));
		AddSpawnPoint("f_gele_57_3.Id1", "f_gele_57_3", Rectangle(1056, -314, 20));

		// 'Banshee' GenType 22 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id2", "f_gele_57_3", Rectangle(97, 275, 9999));

		// 'Firent' GenType 23 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id3", "f_gele_57_3", Rectangle(-92, 155, 9999));

		// 'Puragi_Green' GenType 24 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(855, -6, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(803, 183, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(436, 851, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(-332, 551, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(907, 491, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(890, 663, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(1020, 769, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(984, 134, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(246, 1057, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(173, 869, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(959, 20, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(741, 74, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(821, 411, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(1066, 360, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(961, 375, 20));
		AddSpawnPoint("f_gele_57_3.Id4", "f_gele_57_3", Rectangle(849, 94, 20));

		// 'Rootcrystal_01' GenType 27 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(269, 944, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-464, -1399, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-568, -1034, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-513, -566, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-23, -764, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(363, -693, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(768, -667, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(966, -437, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(899, 49, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(898, 208, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(833, 436, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(970, 918, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(345, 519, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(157, 422, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-354, 574, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-183, 765, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(69, 299, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-16, -259, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(318, -94, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-516, -137, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-850, -299, 10));
		AddSpawnPoint("f_gele_57_3.Id5", "f_gele_57_3", Rectangle(-1214, -206, 10));

		// 'Banshee' GenType 30 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(294, -756, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(844, -750, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(40, -767, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(716, -502, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(966, -483, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(184, -627, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(85, -808, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(234, -662, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(159, -859, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(115, -637, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(259, -815, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(96, -721, 20));
		AddSpawnPoint("f_gele_57_3.Id6", "f_gele_57_3", Rectangle(60, -851, 20));

		// 'Humming_Bud' GenType 33 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(-471, -851, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(-691, -633, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(698, -740, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(-654, -830, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(731, -322, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(935, -473, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(-543, -689, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(-302, -520, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(871, 100, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(-491, -536, 35));
		AddSpawnPoint("f_gele_57_3.Id7", "f_gele_57_3", Rectangle(-579, -739, 35));

		// 'Deadbornscab_Mage' GenType 34 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id8", "f_gele_57_3", Rectangle(947, 860, 30));
		AddSpawnPoint("f_gele_57_3.Id8", "f_gele_57_3", Rectangle(784, -301, 30));
		AddSpawnPoint("f_gele_57_3.Id8", "f_gele_57_3", Rectangle(858, -644, 30));
		AddSpawnPoint("f_gele_57_3.Id8", "f_gele_57_3", Rectangle(874, 699, 30));
		AddSpawnPoint("f_gele_57_3.Id8", "f_gele_57_3", Rectangle(925, 641, 30));
		AddSpawnPoint("f_gele_57_3.Id8", "f_gele_57_3", Rectangle(299, 949, 30));
		AddSpawnPoint("f_gele_57_3.Id8", "f_gele_57_3", Rectangle(975, -340, 30));
		AddSpawnPoint("f_gele_57_3.Id8", "f_gele_57_3", Rectangle(662, -590, 30));

		// 'Puragi_Green' GenType 37 Spawn Points
		AddSpawnPoint("f_gele_57_3.Id9", "f_gele_57_3", Rectangle(777, 65, 20));
		AddSpawnPoint("f_gele_57_3.Id9", "f_gele_57_3", Rectangle(887, -34, 20));
		AddSpawnPoint("f_gele_57_3.Id9", "f_gele_57_3", Rectangle(994, 65, 20));
		AddSpawnPoint("f_gele_57_3.Id9", "f_gele_57_3", Rectangle(983, 151, 20));
		AddSpawnPoint("f_gele_57_3.Id9", "f_gele_57_3", Rectangle(915, 86, 20));
		AddSpawnPoint("f_gele_57_3.Id9", "f_gele_57_3", Rectangle(840, -28, 20));
		AddSpawnPoint("f_gele_57_3.Id9", "f_gele_57_3", Rectangle(853, 209, 20));

		// Boss Spawners ---------------------------------
		AddBossSpawner(MonsterId.Boss_Minotaurs, "f_gele_57_3", 1, Hours(2), Hours(4));
	}
}
