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
		AddNpc(24, 40120, "Statue of Goddess Vakarine", "f_castle_94", 1133.941, 295.1365, 726.7096, 90, "WARP_F_CASTLE_94", "STOUP_CAMP", "STOUP_CAMP");
	}
}
