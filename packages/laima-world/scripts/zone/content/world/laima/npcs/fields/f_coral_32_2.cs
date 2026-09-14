//--- Melia Script ----------------------------------------------------------
// Igti Coast
//--- Description -----------------------------------------------------------
// NPCs found in and around Igti Coast.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral322NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_coral_32_2", 930.68, 223.15, 1887.74, 0, "TREASUREBOX_LV_F_CORAL_32_21000", "", "");

	}
}
