//--- Melia Script ----------------------------------------------------------
// Barha Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Barha Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard343NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_orchard_34_3", -855.38, 370.68, 410, 90, "TREASUREBOX_LV_F_ORCHARD_34_31000", "", "");
	}
}
