//--- Melia Script ----------------------------------------------------------
// Gele Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Gele Plateau.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele572NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv4 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(150, 147394, "Lv4 Treasure Chest", "f_gele_57_2", -659, 418.99, -72.39, 90, "TREASUREBOX_LV_F_GELE_57_2150", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(153, 147392, "Lv1 Treasure Chest", "f_gele_57_2", -1049.22, 418.99, -641.81, 90, "TREASUREBOX_LV_F_GELE_57_2153", "", "");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153007, "", "f_gele_57_2", -85.66, 381.08, 841.97, 0, "f_gele57_2_cablecar", 2, 5);

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_gele_57_2", 1085, 543, -1204, 0, "green");
		AddPlatformNpc("f_gele_57_2", 1045, 583, -1204, 0, "blue");
		AddPlatformNpc("f_gele_57_2", 1005, 623, -1204, 0, "yellow");
		AddPlatformNpc("f_gele_57_2", 980, 663, -1134, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_gele_57_2.Chest1", "f_gele_57_2", 980, 663, -1134, 0, ItemId.EmoticonItem_Gabija_EarringRaid_1_4, monsterId: 147393);

	}
}
