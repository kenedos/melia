//--- Melia Script ----------------------------------------------------------
// Dina Bee Farm
//--- Description -----------------------------------------------------------
// NPCs found in and around Dina Bee Farm.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai464NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(6, 40120, "Statue of Goddess Vakarine", "f_siauliai_46_4", -435.1169, 148.2241, -1247.06, 91, "WARP_F_SIAULIAI_46_4", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(34, 147392, "Lv1 Treasure Chest", "f_siauliai_46_4", 1459.06, 188.66, -121.23, 90, "TREASUREBOX_LV_F_SIAULIAI_46_434", "", "");
	}
}
