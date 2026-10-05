//--- Melia Script ----------------------------------------------------------
// Nahash Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Nahash Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai351NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(11, 40120, "Statue of Goddess Vakarine", "f_siauliai_35_1", 168.528, -157.5367, 732.7125, 45, "WARP_F_SIAULIAI_35_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_siauliai_35_1", -1348.95, -79.35, 108.1, 135, "TREASUREBOX_LV_F_SIAULIAI_35_11000", "", "");
	}
}
