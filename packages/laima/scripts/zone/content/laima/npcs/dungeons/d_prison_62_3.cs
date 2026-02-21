//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 3F
//--- Description -----------------------------------------------------------
// NPCs found in and around Ashaq Underground Prison 3F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison623NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(22, 40120, "Statue of Goddess Vakarine", "d_prison_62_3", 877.5243, 997.5414, 20.58967, 7, "WARP_D_PRISON_62_3", "STOUP_CAMP", "STOUP_CAMP");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(154058, "", "d_prison_62_3", -324.5829, 1013.862, 329.7611, 0, "d_prison_62_3_elt", 2, 1);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(245, 147392, "Lv1 Treasure Chest", "d_prison_62_3", 1721, 798, -2159, 90, "TREASUREBOX_LV_D_PRISON_62_3245", "", "");
	}
}
