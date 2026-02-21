//--- Melia Script ----------------------------------------------------------
// Dvasia Peak
//--- Description -----------------------------------------------------------
// NPCs found in and around Dvasia Peak.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn22NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(7, 40120, "Statue of Goddess Vakarine", "d_thorn_22", 66.92315, 559.9864, -1211.507, 45, "WARP_D_THORN_22", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(832, 147392, "Lv1 Treasure Chest", "d_thorn_22", 1109.28, 446.61, -1282.3, 90, "TREASUREBOX_LV_D_THORN_22832", "", "");
	}
}
