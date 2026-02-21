//--- Melia Script ----------------------------------------------------------
// Verkti Square
//--- Description -----------------------------------------------------------
// NPCs found in and around Verkti Square.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash59NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_59", 818.07, 61.83, 798.19, 90, "TREASUREBOX_LV_F_FLASH_591000", "", "");
	}
}
