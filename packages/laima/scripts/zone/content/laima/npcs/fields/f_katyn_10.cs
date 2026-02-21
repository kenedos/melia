//--- Melia Script ----------------------------------------------------------
// Karolis Springs
//--- Description -----------------------------------------------------------
// NPCs found in and around Karolis Springs.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn10NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(649, 147392, "Lv1 Treasure Chest", "f_katyn_10", -969, 165, -605, 0, "TREASUREBOX_LV_F_KATYN_10649", "", "");
	}
}
