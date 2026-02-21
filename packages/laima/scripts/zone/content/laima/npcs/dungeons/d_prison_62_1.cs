//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Ashaq Underground Prison 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison621NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(6, 40120, "Statue of Goddess Vakarine", "d_prison_62_1", -511.9751, 326.1438, 86.08859, -3, "WARP_D_PRISON_62_1", "STOUP_CAMP", "STOUP_CAMP");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(154059, "", "d_prison_62_1", -241.3938, 200.532, 501.1691, 0, "d_prison_62_1_elt", 2, 1);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(270, 147392, "Lv1 Treasure Chest", "d_prison_62_1", -1501, 408, 1232, 0, "TREASUREBOX_LV_D_PRISON_62_1270", "", "");
	}
}
