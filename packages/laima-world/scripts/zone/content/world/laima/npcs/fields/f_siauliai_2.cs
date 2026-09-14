//--- Melia Script ----------------------------------------------------------
// East Siauliai Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around East Siauliai Woods.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai2NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(33, "WARP_F_SIAULIAI_EST", "f_siauliai_2", 233, 157, 724, 0);
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		// AddNpc(10039, 147392, "Lv1 Treasure Chest", "f_siauliai_2", -2235.57, 130.12, 690.81, 90, "TREASUREBOX_LV_F_SIAULIAI_210039", "", "");
	}
}
