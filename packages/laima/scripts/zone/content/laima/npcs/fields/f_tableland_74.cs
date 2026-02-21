//--- Melia Script ----------------------------------------------------------
// Steel Heights
//--- Description -----------------------------------------------------------
// NPCs found in and around Steel Heights.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland74NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_tableland_74", 505, 669, 162, 0, "TREASUREBOX_LV_F_TABLELAND_741000", "", "");
	}
}
