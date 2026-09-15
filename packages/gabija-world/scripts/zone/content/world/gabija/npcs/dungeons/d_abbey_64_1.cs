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
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(4, 153119, "Traveling Merchant Rose", "d_abbey_64_1", -379, 209.52, -1989, 258, "ABBEY641_ROZE01", "", "");
		
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(5, 153119, "Traveling Merchant Rose", "d_abbey_64_1", -160.9, 97.02, -1115.83, 164, "ABBEY641_ROZE02", "", "");
		
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(21, 153119, "Traveling Merchant Rose", "d_abbey_64_1", 661.5251, 2.345154, 909.4955, -40, "ABBEY641_ROZE03", "", "");
		
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(30, 153119, "Traveling Merchant Rose", "d_abbey_64_1", -246.2318, 93.88463, -1009.49, 192, "ABBEY641_ROZE04", "", "");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(32, "WARP_D_ABBEY_64_1", "d_abbey_64_1", 229.2671, 13.27824, 862.174, 90);
		
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(39, 153119, "Traveling Merchant Rose", "d_abbey_64_1", -395.47, 209.6, -1984.68, -55, "ABBEY641_ROZE05", "", "");

	}
}
