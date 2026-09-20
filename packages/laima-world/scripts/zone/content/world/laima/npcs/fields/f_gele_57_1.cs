//--- Melia Script ----------------------------------------------------------
// Srautas Gorge
//--- Description -----------------------------------------------------------
// NPCs found in and around Srautas Gorge.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele571NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(133, "WARP_F_GELE_57_1", "f_gele_57_1", -132.3, 168.82, -571.54, -9);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(158, 147392, "Lv1 Treasure Chest", "f_gele_57_1", 22, 168.92, -979.04, 90, "TREASUREBOX_LV_F_GELE_57_1158", "", "");

		// Lv1 Treasure Chest (Panto Necklace)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147392, "Lv1 Treasure Chest", "f_gele_57_1", 816, 169, -315, 315, "TREASUREBOX_LV_F_GELE_57_19001", "", "");
	}
}
