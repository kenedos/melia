//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Ashaq Underground Prison 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison622NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(7, 40120, "Statue of Goddess Vakarine", "d_prison_62_2", 3.445572, 381.2892, 231.4913, 2, "WARP_D_PRISON_62_2", "STOUP_CAMP", "STOUP_CAMP");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(154058, "", "d_prison_62_2", 5.702232, -707.8696, -1245.999, 0, "d_prison_62_2_elt", 2, 1);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(322, 147392, "Lv1 Treasure Chest", "d_prison_62_2", 141, 242, 1375, 90, "TREASUREBOX_LV_D_PRISON_62_2322", "", "");
	}
}
