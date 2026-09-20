//--- Melia Script ----------------------------------------------------------
// Platform Puzzles Custom NPC
//--- Description -----------------------------------------------------------
// Emoticon Chest platform puzzles found across the world.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomNpcPlatformPuzzle : GeneralScript
{
	protected override void Load()
	{
		// Fedimian - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("c_fedimian", 139, 945, 867, 0, "blue");
		AddPlatformNpc("c_fedimian", 99, 985, 867, 0, "yellow");
		AddPlatformNpc("c_fedimian", 59, 1025, 867, 0, "red");
		AddPlatformNpc("c_fedimian", 19, 1065, 867, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.c_fedimian.Chest1", "c_fedimian", 19, 1065, 867, 0, ItemId.EmoticonItem_59_63, monsterId: 147393);
		AddPlatformNpc("c_fedimian", -21, 1025, 867, 0, "red");
		AddPlatformNpc("c_fedimian", -61, 985, 867, 0, "yellow");
		AddPlatformNpc("c_fedimian", -101, 945, 867, 0, "blue");
		AddPlatformNpc("c_fedimian", -101, 905, 827, 0, "green");
		AddPlatformNpc("c_fedimian", -101, 865, 787, 0, "red");

		// Orsha - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("c_orsha", -423, 489, 92, 0, "green");
		AddPlatformNpc("c_orsha", -423, 529, 92, 0, "blue");
		AddPlatformNpc("c_orsha", -423, 569, 92, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.Orsha.Chest1", "c_orsha", -423, 569, 92, 0, ItemId.EmoticonItem_55_58, monsterId: 147393);

		// Koru Jungle - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_bracken_63_1", -597, 955, 1080, 45, "blue");
		AddMovingPlatformNpc("f_bracken_63_1",
			new Position(-678, 995, 1050),
			new Position(-564, 995, 1162),
			TimeSpan.FromSeconds(1), direction: 45, color: "green");
		AddMovingPlatformNpc("f_bracken_63_1",
			new Position(-614, 1035, 1212),
			new Position(-728, 1035, 1100),
			TimeSpan.FromSeconds(1), direction: 45, color: "yellow");
		AddPlatformNpc("f_bracken_63_1", -687, 1075, 1170, 45, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_bracken_63_1.Chest1", "f_bracken_63_1", -687, 1075, 1170, 45, ItemId.EmoticonItem_64_69, monsterId: 147393);

		// Srautas Gorge - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_gele_57_1", 1094, 408, -1438, 0, "green");
		AddPlatformNpc("f_gele_57_1", 1064, 448, -1468, 0, "green");
		AddPlatformNpc("f_gele_57_1", 1034, 488, -1498, 0, "green");
		AddPlatformNpc("f_gele_57_1", 1004, 528, -1528, 0, "green");
		AddPlatformNpc("f_gele_57_1", 974, 568, -1558, 0, "green");
		AddPlatformNpc("f_gele_57_1", 944, 608, -1588, 0, "green");
		AddPlatformNpc("f_gele_57_1", 914, 648, -1618, 0, "green");
		AddMovingPlatformNpc("f_gele_57_1",
			new Position(884, 678, -1548),
			new Position(884, 678, -1312),
			TimeSpan.FromSeconds(1), direction: 0, color: "red");
		AddPlatformNpc("f_gele_57_1", 884, 708, -1244, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_gele_57_1.Chest1", "f_gele_57_1", 884, 709, -1244, 0, ItemId.EmoticonItem_Gabija_EarringRaid_5_8, monsterId: 147393);

		// Gele Plateau - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_gele_57_2", 1085, 543, -1204, 0, "green");
		AddPlatformNpc("f_gele_57_2", 1045, 583, -1204, 0, "blue");
		AddPlatformNpc("f_gele_57_2", 1005, 623, -1204, 0, "yellow");
		AddPlatformNpc("f_gele_57_2", 980, 663, -1134, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_gele_57_2.Chest1", "f_gele_57_2", 980, 663, -1134, 0, ItemId.EmoticonItem_Gabija_EarringRaid_1_4, monsterId: 147393);

		// Tenet Garden - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_gele_57_4", 2314, 144, -607, 45, "blue");
		AddPlatformNpc("f_gele_57_4", 2274, 184, -647, 45, "red");
		AddPlatformNpc("f_gele_57_4", 2234, 224, -687, 45, "green");
		AddPlatformNpc("f_gele_57_4", 2194, 264, -727, 45, "yellow");
		AddMovingPlatformNpc("f_gele_57_4",
			new Position(2154, 304, -767),
			new Position(2283, 304, -896),
			TimeSpan.FromSeconds(2), direction: 45, color: "blue");
		AddMovingPlatformNpc("f_gele_57_4",
			new Position(2114, 344, -807),
			new Position(2243, 344, -936),
			TimeSpan.FromSeconds(1), direction: 45, color: "green");
		AddPlatformNpc("f_gele_57_4", 2302, 384, -992, 45, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_gele_57_4.Chest1", "f_gele_57_4", 2302, 384, -992, 45, ItemId.Unity_Emotion146, monsterId: 147393);

		// Septyni Glen - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_huevillage_58_4", 210, 56, 966, 0, "blue");
		AddPlatformNpc("f_huevillage_58_4", 210, 96, 916, 0, "green");
		AddPlatformNpc("f_huevillage_58_4", 210, 136, 866, 0, "red");
		AddPlatformNpc("f_huevillage_58_4", 142, 176, 826, 0, "yellow");
		AddPlatformNpc("f_huevillage_58_4", 142, 216, 746, 0, "blue");
		AddPlatformNpc("f_huevillage_58_4", 85, 256, 696, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_huevillage_58_4.Chest1", "f_huevillage_58_4", 85, 256, 696, 0, ItemId.Pajauta_Emoticon_152_154, monsterId: 147393);

		// Seir Rainforest - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_orchard_32_4", 1053, 556, 802, 0, "yellow");
		AddPlatformNpc("f_orchard_32_4", 1053, 596, 802, 0, "blue");
		AddPlatformNpc("f_orchard_32_4", 1053, 636, 802, 0, "red");
		AddPlatformNpc("f_orchard_32_4", 1053, 676, 802, 0, "green");
		AddPlatformNpc("f_orchard_32_4", 1053, 716, 802, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_orchard_32_4.Chest1", "f_orchard_32_4", 1053, 716, 802, 0, ItemId.EmoticonItem_2404_Popo, monsterId: 147393);

		// Gateway of the Great King - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_rokas_24", -1872, 935, -893, 0, "yellow");
		AddPlatformNpc("f_rokas_24", -1872, 975, -893, 0, "blue");
		AddPlatformNpc("f_rokas_24", -1872, 1005, -893, 0, "red");
		AddMovingPlatformNpc("f_rokas_24",
			new Position(-1832, 1045, -893),
			new Position(-1832, 1045, -1064),
			TimeSpan.FromSeconds(1), direction: 0, color: "green");
		AddMovingPlatformNpc("f_rokas_24",
			new Position(-1792, 1085, -893),
			new Position(-1792, 1085, -949),
			TimeSpan.FromSeconds(1), direction: 0, color: "yellow");
		AddPlatformNpc("f_rokas_24", -1709, 1115, -940, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_rokas_24.Chest1", "f_rokas_24", -1709, 1115, -940, 0, ItemId.Event_Gosu_Emoticon_Box_2, monsterId: 147393);

		// Tiltas Valley - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_rokas_28", 315, 1389, 765, 0, "yellow");
		AddPlatformNpc("f_rokas_28", 365, 1429, 765, 0, "green");
		AddPlatformNpc("f_rokas_28", 415, 1469, 765, 0, "blue");
		AddMovingPlatformNpc("f_rokas_28",
			new Position(465, 1519, 765),
			new Position(465, 1519, 608),
			TimeSpan.FromSeconds(1), direction: 0, color: "red");
		AddPlatformNpc("f_rokas_28", 415, 1559, 608, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_rokas_28.Chest1", "f_rokas_28", 415, 1559, 608, 0, ItemId.EmoticonItem_109_110, monsterId: 147393);

		// Woods of the Linked Bridges - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_siauliai_15_re", -903, 962, 1335, 0, "red");
		AddMovingPlatformNpc("f_siauliai_15_re",
			new Position(-903, 1002, 1277),
			new Position(-903, 1002, 1145),
			TimeSpan.FromSeconds(3), color: "blue");
		AddPlatformNpc("f_siauliai_15_re", -973, 1032, 1145, 0, "yellow");
		AddMovingPlatformNpc("f_siauliai_15_re",
			new Position(-973, 1062, 1195),
			new Position(-1133, 1062, 1195),
			TimeSpan.FromSeconds(3), color: "green");
		AddPlatformNpc("f_siauliai_15_re", -1187, 1097, 1193, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_siauliai_15_re.Chest1", "f_siauliai_15_re", -1187, 1097, 1193, 90, ItemId.EmoticonItem_77_79, monsterId: 147393);

		// Nobreer Forest - Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_whitetrees_21_2", 1398, -33, 549, 0, "green");
		AddPlatformNpc("f_whitetrees_21_2", 1459, -3, 549, 0, "yellow");
		AddPlatformNpc("f_whitetrees_21_2", 1537, 27, 549, 0, "red");
		AddMovingPlatformNpc("f_whitetrees_21_2",
			new Position(1615, 62, 704),
			new Position(1615, 62, 403),
			TimeSpan.FromSeconds(1), direction: 0, color: "blue");
		AddMovingPlatformNpc("f_whitetrees_21_2",
			new Position(1676, 62, 403),
			new Position(1779, 62, 403),
			TimeSpan.FromSeconds(1), direction: 0, color: "red");
		AddMovingPlatformNpc("f_whitetrees_21_2",
			new Position(1676, 62, 704),
			new Position(1779, 62, 704),
			TimeSpan.FromSeconds(1), direction: 0, color: "red");
		AddPlatformNpc("f_whitetrees_21_2", 1779, 92, 473, 0, "yellow");
		AddPlatformNpc("f_whitetrees_21_2", 1779, 92, 634, 0, "yellow");
		AddPlatformNpc("f_whitetrees_21_2", 1779, 122, 554, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_whitetrees_21_2.Chest1", "f_whitetrees_21_2", 1779, 122, 554, 0, ItemId.EmoticonItem_111_112, monsterId: 147393);
	}
}
