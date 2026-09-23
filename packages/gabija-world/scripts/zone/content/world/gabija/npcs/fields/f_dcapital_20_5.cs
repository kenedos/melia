//--- Melia Script ----------------------------------------------------------
// Jeromel Park
//--- Description -----------------------------------------------------------
// NPCs found in and around Jeromel Park.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital205NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_dcapital_20_5", 535.79, -10.96, 128.14, 90, "TREASUREBOX_LV_F_DCAPITAL_20_51000", "", "");
	}
}
