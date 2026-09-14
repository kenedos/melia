//--- Melia Script ----------------------------------------------------------
// Sutatis Trade Route
//--- Description -----------------------------------------------------------
// NPCs found in and around Sutatis Trade Route.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad312NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(108, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_31_2", -88.32, 264.63, -1125.45, 135, "TREASUREBOX_LV_F_PILGRIMROAD_31_2108", "", "");
	}
}
