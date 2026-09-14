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
		AddWarpStatue(6, "WARP_D_PRISON_62_1", "d_prison_62_1", -511.9751, 326.1438, 86.08859, -3);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(270, 147392, "Lv1 Treasure Chest", "d_prison_62_1", -1501, 408, 1232, 0, "TREASUREBOX_LV_D_PRISON_62_1270", "", "");

		// Lv1 Treasure Chest (Red Gem)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147392, "Lv1 Treasure Chest", "d_prison_62_1", -703, 326, 561, 0, "TREASUREBOX_LV_D_PRISON_62_19001", "", "");

		// Lv1 Treasure Chest (Green Gem)
		//-------------------------------------------------------------------------
		AddNpc(9002, 147392, "Lv1 Treasure Chest", "d_prison_62_1", -5, 340, -930, 45, "TREASUREBOX_LV_D_PRISON_62_19002", "", "");
	}
}
