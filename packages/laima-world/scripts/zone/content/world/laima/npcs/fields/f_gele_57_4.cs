//--- Melia Script ----------------------------------------------------------
// Tenet Garden
//--- Description -----------------------------------------------------------
// NPCs found in and around Tenet Garden.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele574NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(50, "WARP_F_GELE_57_4", "f_gele_57_4", -755, -80, 491, 35);
		
		// Lv2 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(51, 40030, "Lv2 Treasure Chest", "f_gele_57_4", -1823, 7.31, -728, 180, "TREASUREBOX_LV_F_GELE_57_451", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(60, 147392, "Lv1 Treasure Chest", "f_gele_57_4", -1854.78, -29.81, 158.67, 0, "TREASUREBOX_LV_F_GELE_57_460", "", "");
	}
}
