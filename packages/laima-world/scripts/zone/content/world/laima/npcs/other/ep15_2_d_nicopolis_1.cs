//--- Melia Script ----------------------------------------------------------
// Secret Laboratory
//--- Description -----------------------------------------------------------
// NPCs found in and around Secret Laboratory.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep152DNicopolis1NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(5, "WARP_EP15_2_D_NICOPOLIS_1", "ep15_2_d_nicopolis_1", 462.7657, -16.0284, -1101.348, 90);
	}
}
