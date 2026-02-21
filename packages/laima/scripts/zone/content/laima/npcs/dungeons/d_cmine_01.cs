//--- Melia Script ----------------------------------------------------------
// Crystal Mine 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Crystal Mine 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine01NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(529, 40120, "Statue of Goddess Vakarine", "d_cmine_01", -1222.77, 316.34, -1230.72, 60, "WARP_D_CMINE_01", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv2 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(533, 40030, "Lv2 Treasure Chest", "d_cmine_01", 1316, 4, 730, 360, "TREASUREBOX_LV_D_CMINE_01533", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(541, 147392, "Lv1 Treasure Chest", "d_cmine_01", 645.39, 17.75, -27.96, 0, "TREASUREBOX_LV_D_CMINE_01541", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(542, 147392, "Lv1 Treasure Chest", "d_cmine_01", 331.21, 111.48, 494.6, 0, "TREASUREBOX_LV_D_CMINE_01542", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(543, 147392, "Lv1 Treasure Chest", "d_cmine_01", -1542.96, 154.8, 494.07, 90, "TREASUREBOX_LV_D_CMINE_01543", "", "");
	}
}
