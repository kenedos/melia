//--- Melia Script ----------------------------------------------------------
// Feretory Hills
//--- Description -----------------------------------------------------------
// NPCs found in and around Feretory Hills.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad311NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(159, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_31_1", 174, 75, 1269, 0, "TREASUREBOX_LV_F_PILGRIMROAD_31_1159", "", "");
	}
}
