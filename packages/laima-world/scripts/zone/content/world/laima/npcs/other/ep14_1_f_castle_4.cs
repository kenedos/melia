//--- Melia Script ----------------------------------------------------------
// Delmore Citadel
//--- Description -----------------------------------------------------------
// NPCs found in and around Delmore Citadel.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep141FCastle4NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(84, "WARP_EP14_1_F_CASTLE_4", "ep14_1_f_castle_4", 1442.492, 58.91404, -2066.39, 220);
	}
}
