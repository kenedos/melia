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
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(304, "WARP_F_BRACKEN_63_2", "f_bracken_63_2", 196.9207, 284.1552, 998.8207, 90);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(493, 147392, "Lv1 Treasure Chest", "f_bracken_63_2", 589, 84, -1976, 0, "TREASUREBOX_LV_F_BRACKEN_63_2493", "", "");
	}
}
