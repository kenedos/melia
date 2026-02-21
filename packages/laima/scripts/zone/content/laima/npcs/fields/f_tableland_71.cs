//--- Melia Script ----------------------------------------------------------
// Grand Yard Mesa
//--- Description -----------------------------------------------------------
// NPCs found in and around Grand Yard Mesa.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland71NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_tableland_71", 186.31, 443.54, -125.44, -45, "TREASUREBOX_LV_F_TABLELAND_711000", "", "");
	}
}
