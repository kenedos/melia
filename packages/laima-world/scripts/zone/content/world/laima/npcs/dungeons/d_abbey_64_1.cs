//--- Melia Script ----------------------------------------------------------
// Novaha Assembly Hall
//--- Description -----------------------------------------------------------
// NPCs found in and around Novaha Assembly Hall.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey641NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(32, "WARP_D_ABBEY_64_1", "d_abbey_64_1", 229.2671, 13.27824, 862.174, 90);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(262, 147392, "Lv1 Treasure Chest", "d_abbey_64_1", 797, 33, 362, 0, "TREASUREBOX_LV_D_ABBEY_64_1262", "", "");
	}
}
