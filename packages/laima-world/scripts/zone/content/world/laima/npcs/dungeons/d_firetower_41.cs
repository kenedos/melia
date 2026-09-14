//--- Melia Script ----------------------------------------------------------
// Mage Tower 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Mage Tower 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower41NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(116, "WARP_D_FIRETOWER_41", "d_firetower_41", 2005.266, 1446.488, -1369.808, 30);
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(126, 147392, "Lv1 Treasure Chest", "d_firetower_41", -2714.8, 1552.83, -1436.66, 90, "TREASUREBOX_LV_D_FIRETOWER_41126", "", "");
		
		// Track NPCs
		//---------------------------------------------------------------------------

	}
}
