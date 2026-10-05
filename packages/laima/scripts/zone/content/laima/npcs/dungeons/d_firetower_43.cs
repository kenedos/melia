//--- Melia Script ----------------------------------------------------------
// Mage Tower 3F
//--- Description -----------------------------------------------------------
// NPCs found in and around Mage Tower 3F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower43NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(241, 147392, "Lv1 Treasure Chest", "d_firetower_43", -149.72, 364.48, -1281.47, 90, "TREASUREBOX_LV_D_FIRETOWER_43241", "", "");
	}
}
