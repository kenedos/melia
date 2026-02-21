//--- Melia Script ----------------------------------------------------------
// Crystal Mine Lot 2 - 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Crystal Mine Lot 2 - 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine8NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv3 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1078, 147393, "Lv3 Treasure Chest", "d_cmine_8", 2615, -185, 2262, 0, "TREASUREBOX_LV_D_CMINE_81078", "", "");
		
		// Lv2 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1109, 40030, "Lv2 Treasure Chest", "d_cmine_8", 1805, -179, 1854, 90, "TREASUREBOX_LV_D_CMINE_81109", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(700, 147392, "Lv1 Treasure Chest", "d_cmine_8", 140.29, 9.16, 191.32, 268, "TREASUREBOX_LV_D_CMINE_8700", "", "");
	}
}
