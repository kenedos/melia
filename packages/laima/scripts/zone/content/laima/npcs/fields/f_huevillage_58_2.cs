//--- Melia Script ----------------------------------------------------------
// Vieta Gorge
//--- Description -----------------------------------------------------------
// NPCs found in and around Vieta Gorge.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage582NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(34, 40120, "Statue of Goddess Vakarine", "f_huevillage_58_2", -515.8, 271.89, -1541.66, 125, "WARP_F_HUEVILLAGE_58_2", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(49, 147392, "Lv1 Treasure Chest", "f_huevillage_58_2", -159.95, 274.31, -1274.28, 90, "TREASUREBOX_LV_F_HUEVILLAGE_58_249", "", "");
	}
}
