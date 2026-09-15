//--- Melia Script ----------------------------------------------------------
// Sicarius 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Sicarius 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress681NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(24, "WARP_D_UNDERFORTRESS_68_1", "d_underfortress_68_1", -1378.934, 227.4717, 224.0348, 0);
	}
}
