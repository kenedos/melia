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
		AddWarpStatue(44, "WARP_D_ABBEY_64_3", "d_abbey_64_3", 735.2632, 451.2133, 487.0141, 45);

	}
}
