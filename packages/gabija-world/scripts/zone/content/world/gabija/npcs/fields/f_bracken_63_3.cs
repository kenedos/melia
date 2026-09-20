//--- Melia Script ----------------------------------------------------------
// Dadan Jungle
//--- Description -----------------------------------------------------------
// NPCs found in and around Dadan Jungle.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken633NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(5, 153119, "Traveling Merchant Rose", "f_bracken_63_3", 49.85, 189.58, 489.81, 246, "BRACKEN633_ROZE01", "", "");
		
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(6, 153119, "Traveling Merchant Rose", "f_bracken_63_3", -145, 77.33, -728, -54, "BRACKEN633_ROZE02", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(509, 147392, "Lv1 Treasure Chest", "f_bracken_63_3", -662, 1003, -919, 0, "TREASUREBOX_LV_F_BRACKEN_63_3509", "", "");
	}
}
