//--- Melia Script ----------------------------------------------------------
// Inner Wall District 10
//--- Description -----------------------------------------------------------
// NPCs found in and around Inner Wall District 10.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle94NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(24, "WARP_F_CASTLE_94", "f_castle_94", 1133.941, 295.1365, 726.7096, 90);
	}
}
