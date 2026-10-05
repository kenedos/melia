//--- Melia Script ----------------------------------------------------------
// Spring Light Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around Spring Light Woods.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai461NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(27, 147392, "Lv1 Treasure Chest", "f_siauliai_46_1", 954, 390.89, 1020.98, 90, "TREASUREBOX_LV_F_SIAULIAI_46_127", "", "");
	}
}
