//--- Melia Script ----------------------------------------------------------
// Crystal Mine 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Crystal Mine 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine02NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(1, "WARP_D_CMINE_02", "d_cmine_02", 1822, -10, 321, 390);
	}
}
