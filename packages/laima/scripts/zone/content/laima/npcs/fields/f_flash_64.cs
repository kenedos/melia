//--- Melia Script ----------------------------------------------------------
// Inner Enceinte District
//--- Description -----------------------------------------------------------
// NPCs found in and around Inner Enceinte District.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash64NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(7, 40110, "Statue of Goddess Zemyna", "f_flash_64", -570.1172, 885.3875, 1373.78, 105, "F_FLASH_64_EV_55_001", "F_FLASH_64_EV_55_001", "F_FLASH_64_EV_55_001");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(27, 40120, "Statue of Goddess Vakarine", "f_flash_64", -142.5398, 745.6932, -1353.881, 0, "WARP_F_FLASH_64", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_64", -560.66, 673.74, 526.11, 135, "TREASUREBOX_LV_F_FLASH_641000", "", "");
	}
}
