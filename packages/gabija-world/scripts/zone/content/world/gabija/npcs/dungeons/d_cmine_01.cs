//--- Melia Script ----------------------------------------------------------
// Crystal Mine 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Crystal Mine 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine01NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(529, 40120, "Statue of Goddess Vakarine", "d_cmine_01", -1222.77, 316.34, -1230.72, 60, "WARP_D_CMINE_01", "STOUP_CAMP", "STOUP_CAMP");
	}
}
