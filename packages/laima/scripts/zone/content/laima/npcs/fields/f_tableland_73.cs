//--- Melia Script ----------------------------------------------------------
// Kadumel Cliff
//--- Description -----------------------------------------------------------
// NPCs found in and around Kadumel Cliff.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland73NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_tableland_73", 175.91, 525.56, -214.12, 180, "TREASUREBOX_LV_F_TABLELAND_731000", "", "");
	}
}
