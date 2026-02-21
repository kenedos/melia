//--- Melia Script ----------------------------------------------------------
// Cranto Coast
//--- Description -----------------------------------------------------------
// NPCs found in and around Cranto Coast.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral321NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_coral_32_1", 376.18, 236.88, -1042.87, 225, "TREASUREBOX_LV_F_CORAL_32_11000", "", "");
	}
}
