//--- Melia Script ----------------------------------------------------------
// Delmore Hamlet
//--- Description -----------------------------------------------------------
// NPCs found in and around Delmore Hamlet.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle651NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(29, 40120, "Statue of Goddess Vakarine", "f_castle_65_1", 1083.615, -8.393933, -1060.086, 90, "WARP_CASTLE_65_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(155112, "", "f_castle_65_1", -728.6979, -8.345245, 768.4816, 0, "f_castle_65_1_elt", 2, 1);


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(202, 147392, "Lv1 Treasure Chest", "f_castle_65_1", 1317, -8.21, 1574.19, -90, "TREASUREBOX_LV_F_CASTLE_65_1202", "", "");
	}
}
