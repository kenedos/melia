//--- Melia Script ----------------------------------------------------------
// Novaha Institute
//--- Description -----------------------------------------------------------
// NPCs found in and around Novaha Institute.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey643NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(7, 153119, "Traveling Merchant Rose", "d_abbey_64_3", -1459, 626.92, 175, 13, "ABBEY643_ROZE01", "", "");

		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(44, 40120, "Statue of Goddess Vakarine", "d_abbey_64_3", 735.2632, 451.2133, 487.0141, 45, "WARP_D_ABBEY_64_3", "STOUP_CAMP", "STOUP_CAMP");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153114, "", "d_abbey_64_3", -1155.956, 411.8994, -1264.598, 90, "d_abbey_64_3_elt", 2, 1);


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(242, 147392, "Lv1 Treasure Chest", "d_abbey_64_3", 1372, 510, -812, 0, "TREASUREBOX_LV_D_ABBEY_64_3242", "", "");
	}
}
