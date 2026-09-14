//--- Melia Script ----------------------------------------------------------
// Tevhrin Stalactite Cave Section 1
//--- Description -----------------------------------------------------------
// NPCs found in and around Tevhrin Stalactite Cave Section 1.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DLimestonecave521NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(25, 40120, "Statue of Goddess Vakarine", "d_limestonecave_52_1", -514.34, 0, -617.36, -13, "WARP_D_LIMESTONE_52_1", "STOUP_CAMP", "STOUP_CAMP");
	}
}
