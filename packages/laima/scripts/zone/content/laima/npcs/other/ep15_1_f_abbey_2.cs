//--- Melia Script ----------------------------------------------------------
// Novaha Library
//--- Description -----------------------------------------------------------
// NPCs found in and around Novaha Library.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep151FAbbey2NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(4, 40120, "Statue of Goddess Vakarine", "ep15_1_f_abbey_2", 844.3311, 156.7779, -353.6386, 149, "WARP_EP15_1_F_ABBEY_2", "STOUP_CAMP", "STOUP_CAMP");
		
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(5, 40110, "Statue of Goddess Zemyna", "ep15_1_f_abbey_2", 400.6598, 327.7045, 1414.301, 90, "EP15_1_FABBEY2_ZEMINA", "EP15_1_FABBEY2_ZEMINA", "EP15_1_FABBEY2_ZEMINA");
	}
}
