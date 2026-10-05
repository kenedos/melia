//--- Melia Script ----------------------------------------------------------
// Novaha Institute
//--- Description -----------------------------------------------------------
// NPCs found in and around Novaha Institute.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep151FAbbey1NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(4, 40120, "Statue of Goddess Vakarine", "ep15_1_f_abbey_1", 1128.452, 510.3857, -383.0549, 90, "WARP_EP15_1_F_ABBEY_1", "STOUP_CAMP", "STOUP_CAMP");
	}
}
