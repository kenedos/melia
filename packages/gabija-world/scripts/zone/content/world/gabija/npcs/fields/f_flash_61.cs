//--- Melia Script ----------------------------------------------------------
// Ruklys Street
//--- Description -----------------------------------------------------------
// NPCs found in and around Ruklys Street.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash61NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(13, "WARP_F_FLASH_61", "f_flash_61", -99.65971, 435.358, 1297.89, 0);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_61", -735.61, 511.26, 189.44, 0, "TREASUREBOX_LV_F_FLASH_611000", "", "");
	}
}
