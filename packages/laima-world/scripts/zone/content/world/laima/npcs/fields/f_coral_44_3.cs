//--- Melia Script ----------------------------------------------------------
// Epherotao Coast
//--- Description -----------------------------------------------------------
// NPCs found in and around Epherotao Coast.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral443NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(33, 147392, "Lv1 Treasure Chest", "f_coral_44_3", 174.68, 83.20, 845.56, -45, "TREASUREBOX_LV_F_CORAL_44_333", "", "");
	}
}
