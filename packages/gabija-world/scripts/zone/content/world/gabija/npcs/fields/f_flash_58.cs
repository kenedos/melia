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
	}
}
