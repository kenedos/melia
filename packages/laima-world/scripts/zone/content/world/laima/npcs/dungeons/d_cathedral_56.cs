//--- Melia Script ----------------------------------------------------------
// Sanctuary
//--- Description -----------------------------------------------------------
// NPCs found in and around Sanctuary.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCathedral56NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(48, 147392, "Lv1 Treasure Chest", "d_cathedral_56", -21.49, 0.6, -18.27, 90, "TREASUREBOX_LV_D_CATHEDRAL_5648", "", "");
	}
}
