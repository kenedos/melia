//--- Melia Script ----------------------------------------------------------
// Topes Fortress 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Topes Fortress 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCastle671NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(17, 40120, "Statue of Goddess Vakarine", "d_castle_67_1", -1653.771, 0.258728, -1192.015, 45, "WARP_D_CASTLE_67_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153129, "", "d_castle_67_1", 183.9423, 277.7617, 330.8154, 21, "d_castle_67_1_elt", 3, 2);


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(231, 147392, "Lv1 Treasure Chest", "d_castle_67_1", -1386.86, 56.27, -269.79, 0, "TREASUREBOX_LV_D_CASTLE_67_1231", "", "");
	}
}
