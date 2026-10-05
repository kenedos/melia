//--- Melia Script ----------------------------------------------------------
// Mage Tower 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Mage Tower 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower42NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(223, 147392, "Lv1 Treasure Chest", "d_firetower_42", 2066.69, 20.58, -593.81, 90, "TREASUREBOX_LV_D_FIRETOWER_42223", "", "");
	}
}
