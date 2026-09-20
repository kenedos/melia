//--- Melia Script ----------------------------------------------------------
// Dingofasil District
//--- Description -----------------------------------------------------------
// NPCs found in and around Dingofasil District.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash58NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(24, "WARP_F_FLASH_58", "f_flash_58", -694.7843, 407.5999, -1093.407, 45);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_58", -994.79, 323.13, -1058.82, -45, "TREASUREBOX_LV_F_FLASH_581000", "", "");
	}
}
