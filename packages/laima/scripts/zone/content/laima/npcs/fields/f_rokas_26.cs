//--- Melia Script ----------------------------------------------------------
// Overlong Bridge Valley
//--- Description -----------------------------------------------------------
// NPCs found in and around Overlong Bridge Valley.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas26NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(531, 147392, "Lv1 Treasure Chest", "f_rokas_26", 1919, 1722, 80, 315, "TREASUREBOX_LV_F_ROKAS_26531", "", "");
	}
}
