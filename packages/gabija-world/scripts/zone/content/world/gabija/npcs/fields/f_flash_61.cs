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
		AddNpc(13, 40120, "Statue of Goddess Vakarine", "f_flash_61", -99.65971, 435.358, 1297.89, 0, "WARP_F_FLASH_61", "STOUP_CAMP", "STOUP_CAMP");
	}
}
