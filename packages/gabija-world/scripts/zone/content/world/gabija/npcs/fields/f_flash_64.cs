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
		AddStatPointStatue(7, "F_FLASH_64_EV_55_001", "f_flash_64", -570.1172, 885.3875, 1373.78, 105);
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(27, "WARP_F_FLASH_64", "f_flash_64", -142.5398, 745.6932, -1353.881, 0);
	}
}
