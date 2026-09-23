//--- Melia Script ----------------------------------------------------------
// Sicarius 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Sicarius 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress682NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(161, 147392, "Lv1 Treasure Chest", "d_underfortress_68_2", 172.89, 70.40, -848.49, 135, "TREASUREBOX_LV_D_UNDERFORTRESS_68_2161", "", "");
	}
}
