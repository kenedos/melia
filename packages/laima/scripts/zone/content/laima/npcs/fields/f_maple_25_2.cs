//--- Melia Script ----------------------------------------------------------
// Svalphinghas Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Svalphinghas Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FMaple252NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(22, 40120, "Statue of Goddess Vakarine", "f_maple_25_2", 1112.25, 641.79, 806.54, 90, "WARP_F_MAPLE_25_2", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_maple_25_2", -123.66, -0.19, 237.22, 90, "TREASUREBOX_LV_F_MAPLE_25_21000", "", "");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(157035, "", "f_maple_25_2", -78.02134, 34.58647, 382.6509, 320, "f_maple_25_2_elt", 0);

	}
}
