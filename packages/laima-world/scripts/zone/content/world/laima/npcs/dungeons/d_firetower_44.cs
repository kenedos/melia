//--- Melia Script ----------------------------------------------------------
// Mage Tower 4F
//--- Description -----------------------------------------------------------
// NPCs found in and around Mage Tower 4F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower44NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(230, 147392, "Lv1 Treasure Chest", "d_firetower_44", 744.7, 451.3, 554.75, 90, "TREASUREBOX_LV_D_FIRETOWER_44230", "", "");
	}
}
