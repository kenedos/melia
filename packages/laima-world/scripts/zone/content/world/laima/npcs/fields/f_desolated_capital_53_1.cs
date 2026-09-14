//--- Melia Script ----------------------------------------------------------
// Rinksmas Ruins
//--- Description -----------------------------------------------------------
// NPCs found in and around Rinksmas Ruins.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDesolatedCapital531NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(9, "WARP_DCAPITAL53_1", "f_desolated_capital_53_1", 1515.549, 111.8744, 2274.188, 45);
	}
}
