//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum Workers Lodge
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum Workers Lodge.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress591NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(38, 147392, "Lv1 Treasure Chest", "d_underfortress_59_1", 59, 236, -1362, 90, "TREASUREBOX_LV_D_UNDERFORTRESS_59_138", "", "");
	}
}
