//--- Melia Script ----------------------------------------------------------
// Crystal Mine 3F
//--- Description -----------------------------------------------------------
// NPCs found in and around Crystal Mine 3F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine6NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(525, 40120, "Statue of Goddess Vakarine", "d_cmine_6", -2175.529, 360.2849, -1773.89, 90, "WARP_D_CMINE_6", "STOUP_CAMP", "STOUP_CAMP");
	}
}
