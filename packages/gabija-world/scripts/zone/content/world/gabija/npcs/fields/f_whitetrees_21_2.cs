//--- Melia Script ----------------------------------------------------------
// Nobreer Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Nobreer Forest.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees212NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(8, "WARP_WHITETREES_21_2", "f_whitetrees_21_2", 793.81, -52.46, 118.36, 0);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_whitetrees_21_2", 498.81, 114.39, 1433.28, 90, "TREASUREBOX_LV_F_WHITETREES_21_21000", "", "");

		// Lv1 Treasure Chest (Formine Necklace)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147392, "Lv1 Treasure Chest", "f_whitetrees_21_2", -826, 120, -216, 0, "TREASUREBOX_LV_F_WHITETREES_21_29001", "", "");
	}
}
