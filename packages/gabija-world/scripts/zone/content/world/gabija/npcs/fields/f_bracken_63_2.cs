//--- Melia Script ----------------------------------------------------------
// Knidos Jungle
//--- Description -----------------------------------------------------------
// NPCs found in and around Knidos Jungle.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken632NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(4, 153119, "Traveling Merchant Rose", "f_bracken_63_2", 394.4159, 284.3078, 1213.342, 90, "BRACKEN632_ROZE01", "", "");
		
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(5, 153119, "Traveling Merchant Rose", "f_bracken_63_2", 344.4644, 284.1552, 349.7711, 22, "BRACKEN632_ROZE02", "", "");
		
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(18, 153119, "Traveling Merchant Rose", "f_bracken_63_2", 856.3498, 284.1552, -314.2598, -71, "BRACKEN632_ROZE03", "", "");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(304, "WARP_F_BRACKEN_63_2", "f_bracken_63_2", 196.9207, 284.1552, 998.8207, 90);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(493, 147392, "Lv1 Treasure Chest", "f_bracken_63_2", 589, 84, -1976, 0, "TREASUREBOX_LV_F_BRACKEN_63_2493", "", "");
	}
}
