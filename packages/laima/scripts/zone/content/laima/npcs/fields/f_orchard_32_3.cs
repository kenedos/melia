//--- Melia Script ----------------------------------------------------------
// Bellai Rainforest
//--- Description -----------------------------------------------------------
// NPCs found in and around Bellai Rainforest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard323NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(31, 40120, "Statue of Goddess Vakarine", "f_orchard_32_3", -356.5365, 0.8661499, 716.4241, 90, "WARP_F_ORCHARD_32_3", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(228, 147392, "Lv1 Treasure Chest", "f_orchard_32_3", 156.49, 0.97, 627.23, -135, "TREASUREBOX_LV_F_ORCHARD_32_3228", "", "");
	}
}
