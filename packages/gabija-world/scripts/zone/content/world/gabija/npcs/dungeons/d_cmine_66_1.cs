//--- Melia Script ----------------------------------------------------------
// Nevellet Quarry 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Nevellet Quarry 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine661NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(106, 40120, "Statue of Goddess Vakarine", "d_cmine_66_1", -83.88259, 414.5081, -1486.561, 90, "WARP_D_CMINE_66_1", "STOUP_CAMP", "STOUP_CAMP");

	}
}
