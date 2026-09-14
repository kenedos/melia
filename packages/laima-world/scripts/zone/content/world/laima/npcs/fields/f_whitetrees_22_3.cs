//--- Melia Script ----------------------------------------------------------
// Izoliacjia Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Izoliacjia Plateau.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees223NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(34, "WARP_WHITETREES_22_3", "f_whitetrees_22_3", -491.0911, 331.9633, 227.7959, 90);

		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue("F_WHITETREES_22_3_ZEMYNA", "f_whitetrees_22_3", 1266.2, -693.2, 0);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(37, 147392, "Lv1 Treasure Chest", "f_whitetrees_22_3", 1737.40, 789.35, -1086.66, -45, "TREASUREBOX_LV_F_WHITETREES_22_337", "", "");

		// Lv3 Treasure Chest (Campaign Hat)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147393, "Lv3 Treasure Chest", "f_whitetrees_22_3", 519, 333, 585, 0, "TREASUREBOX_LV_F_WHITETREES_22_39001", "", "");
	}
}
