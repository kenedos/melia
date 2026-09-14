//--- Melia Script ----------------------------------------------------------
// Novaha Annex
//--- Description -----------------------------------------------------------
// NPCs found in and around Novaha Annex.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey642NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(304, 153119, "Traveling Merchant Rose", "d_abbey_64_2", 920.0567, 399.358, -114.0447, 189, "ABBEY642_ROZE01", "", "");
		
		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddNpc(306, 153119, "Traveling Merchant Rose", "d_abbey_64_2", 11, 982.54, -1272, -4, "ABBEY642_ROZE02", "", "");

	}
}
