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
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(44, "WARP_D_ABBEY_64_3", "d_abbey_64_3", 735.2632, 451.2133, 487.0141, 45);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(242, 147392, "Lv1 Treasure Chest", "d_abbey_64_3", 1372, 510, -812, 0, "TREASUREBOX_LV_D_ABBEY_64_3242", "", "");
	}
}
