//--- Melia Script ----------------------------------------------------------
// Tevhrin Stalactite Cave Section 5
//--- Description -----------------------------------------------------------
// NPCs found in and around Tevhrin Stalactite Cave Section 5.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DLimestonecave525NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(25, 40120, "Statue of Goddess Vakarine", "d_limestonecave_52_5", -1319.36, 818.53, -1057.98, -15, "WARP_D_LIMESTONE_52_5", "STOUP_CAMP", "STOUP_CAMP");
	}
}
