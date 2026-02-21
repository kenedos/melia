//--- Melia Script ----------------------------------------------------------
// King's Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around King's Plateau.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas30NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(650, 40120, "Statue of Goddess Vakarine", "f_rokas_30", 825.83, 148.17, -989.87, 90, "WARP_F_ROKAS_30", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(668, 147392, "Lv1 Treasure Chest", "f_rokas_30", -433.94, 348.79, 306.96, 90, "TREASUREBOX_LV_F_ROKAS_30668", "", "");
	}
}
