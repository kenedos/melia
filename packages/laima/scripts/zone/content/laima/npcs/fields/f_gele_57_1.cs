//--- Melia Script ----------------------------------------------------------
// Srautas Gorge
//--- Description -----------------------------------------------------------
// NPCs found in and around Srautas Gorge.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele571NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(133, 40120, "Statue of Goddess Vakarine", "f_gele_57_1", -132.3, 168.82, -571.54, -9, "WARP_F_GELE_57_1", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(158, 147392, "Lv1 Treasure Chest", "f_gele_57_1", 22, 168.92, -979.04, 90, "TREASUREBOX_LV_F_GELE_57_1158", "", "");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153006, "", "f_gele_57_1", -305.9306, 168.8233, -402.949, 0, "f_gele_57_1_elt", 2, 1);

		// Emoticon Chest
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

	}
}
