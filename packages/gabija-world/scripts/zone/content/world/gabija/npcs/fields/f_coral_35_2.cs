//--- Melia Script ----------------------------------------------------------
// Vera Coast
//--- Description -----------------------------------------------------------
// NPCs found in and around Vera Coast.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral352NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_coral_35_2", -275.22, 2.83, 683.81, 180, "TREASUREBOX_LV_F_CORAL_35_21000", "", "");
	}
}
