//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum Storage
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum Storage.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress593NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(34, 147392, "Lv1 Treasure Chest", "d_underfortress_59_3", -1125.904, 0.7999708, 1047.743, 45, "TREASUREBOX_LV_D_UNDERFORTRESS_59_334", "", "");
	}
}
