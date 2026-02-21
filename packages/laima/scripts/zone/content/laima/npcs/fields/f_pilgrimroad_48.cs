//--- Melia Script ----------------------------------------------------------
// Manahas
//--- Description -----------------------------------------------------------
// NPCs found in and around Manahas.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad48NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(20, 40120, "Statue of Goddess Vakarine", "f_pilgrimroad_48", -260.2179, 382.684, 19.69579, 45, "WARP_F_PILGRIMROAD_48", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(26, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_48", 234, 546, 1479, 90, "TREASUREBOX_LV_F_PILGRIMROAD_4826", "", "");
	}
}
