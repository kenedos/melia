//--- Melia Script ----------------------------------------------------------
// Zima Suecourt
//--- Description -----------------------------------------------------------
// NPCs found in and around Zima Suecourt.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower691NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_firetower_69_1", -224.96, -874.21, 489.19, 90, "TREASUREBOX_LV_D_FIRETOWER_69_11000", "", "");
	}
}
