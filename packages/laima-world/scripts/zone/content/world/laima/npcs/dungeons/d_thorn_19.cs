//--- Melia Script ----------------------------------------------------------
// Gate Route
//--- Description -----------------------------------------------------------
// NPCs found in and around Gate Route.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn19NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(664, "WARP_D_THORN_19", "d_thorn_19", -208.2775, 622.5202, -3814.656, 35);
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(687, 147392, "Lv1 Treasure Chest", "d_thorn_19", 627, 600, 1910, 0, "TREASUREBOX_LV_D_THORN_19687", "", "");

		// Lv3 Treasure Chest (Paper Crane)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147393, "Lv3 Treasure Chest", "d_thorn_19", -1232, 526, -3124, 90, "TREASUREBOX_LV_D_THORN_199001", "", "");
	}
}
