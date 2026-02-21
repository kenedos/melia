//--- Melia Script ----------------------------------------------------------
// Nevellet Quarry 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Nevellet Quarry 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine662NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(292, 147392, "Lv1 Treasure Chest", "d_cmine_66_2", -953, 566, 338, 225, "TREASUREBOX_LV_D_CMINE_66_2292", "", "");
	}
}
