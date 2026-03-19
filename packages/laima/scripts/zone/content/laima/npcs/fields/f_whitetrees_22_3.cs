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
		AddNpc(34, 40120, "Statue of Goddess Vakarine", "f_whitetrees_22_3", -491.0911, 331.9633, 227.7959, 90, "WARP_WHITETREES_22_3", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(37, 147392, "Lv1 Treasure Chest", "f_whitetrees_22_3", 1737.40, 789.35, -1086.66, -45, "TREASUREBOX_LV_F_WHITETREES_22_337", "", "");
	}
}
