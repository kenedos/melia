//--- Melia Script ----------------------------------------------------------
// Cobalt Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Cobalt Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage583NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(51, 147392, "Lv1 Treasure Chest", "f_huevillage_58_3", 384, -117, -454, 315, "TREASUREBOX_LV_F_HUEVILLAGE_58_351", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(50, 147392, "Lv1 Treasure Chest", "f_huevillage_58_3", 384, -117, -454, 315, "TREASUREBOX_LV_F_HUEVILLAGE_58_150", "", "");
	}
}
