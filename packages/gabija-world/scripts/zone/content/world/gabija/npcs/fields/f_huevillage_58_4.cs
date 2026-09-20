//--- Melia Script ----------------------------------------------------------
// Septyni Glen
//--- Description -----------------------------------------------------------
// NPCs found in and around Septyni Glen.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage584NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(43, "WARP_F_HUEVILLAGE_58_4", "f_huevillage_58_4", 20.74365, -8.675209, -837.3439, 90);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(45, 147392, "Lv1 Treasure Chest", "f_huevillage_58_4", 240, -8.58, -735, 270, "TREASUREBOX_LV_F_HUEVILLAGE_58_445", "", "");
	}
}
