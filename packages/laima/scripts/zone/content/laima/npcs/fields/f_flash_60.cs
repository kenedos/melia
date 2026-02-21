//--- Melia Script ----------------------------------------------------------
// Roxona Market
//--- Description -----------------------------------------------------------
// NPCs found in and around Roxona Market.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash60NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_60", -920.79, 300.74, 1251.18, -45, "TREASUREBOX_LV_F_FLASH_601000", "", "");
	}
}
