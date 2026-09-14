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
		AddWarpStatue(7, "WARP_D_PRISON_62_2", "d_prison_62_2", 3.445572, 381.2892, 231.4913, 2);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(322, 147392, "Lv1 Treasure Chest", "d_prison_62_2", 141, 242, 1375, 90, "TREASUREBOX_LV_D_PRISON_62_2322", "", "");

		// Lv1 Treasure Chest (Yellow Gem)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147392, "Lv1 Treasure Chest", "d_prison_62_2", -436, 416, -1878, 90, "TREASUREBOX_LV_D_PRISON_62_29001", "", "");
	}
}
