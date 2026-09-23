//--- Melia Script ----------------------------------------------------------
// Coastal Fortress
//--- Description -----------------------------------------------------------
// NPCs found in and around Coastal Fortress.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash291NpcScript : GeneralScript
{
	protected override void Load()
	{


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_29_1", -1126.75, 0.12, -833.51, 135, "TREASUREBOX_LV_F_FLASH_29_11000", "", "");
	}
}
