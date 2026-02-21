//--- Melia Script ----------------------------------------------------------
// Mage Tower 5F
//--- Description -----------------------------------------------------------
// NPCs found in and around Mage Tower 5F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower45NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(210, 40120, "Statue of Goddess Vakarine", "d_firetower_45", -1689.079, 420.4852, -643.7197, 90, "WARP_D_FIRETOWER_45", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(219, 147392, "Lv1 Treasure Chest", "d_firetower_45", -1706.06, 607.02, -187.25, 90, "TREASUREBOX_LV_D_FIRETOWER_45219", "", "");
	}
}
