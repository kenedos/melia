//--- Melia Script ----------------------------------------------------------
// Uskis Arable Land
//--- Description -----------------------------------------------------------
// NPCs found in and around Uskis Arable Land.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai462NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(27, 147392, "Lv1 Treasure Chest", "f_siauliai_46_2", 920.99, 5.56, 5094.94, 90, "TREASUREBOX_LV_F_SIAULIAI_46_227", "", "");
	}
}
