//--- Melia Script ----------------------------------------------------------
// Vilna Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Vilna Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai463NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(23, 147392, "Lv1 Treasure Chest", "f_siauliai_46_3", 2357.72, 202.21, -360.43, 90, "TREASUREBOX_LV_F_SIAULIAI_46_323", "", "");
	}
}
