//--- Melia Script ----------------------------------------------------------
// Nefritas Cliff
//--- Description -----------------------------------------------------------
// NPCs found in and around Nefritas Cliff.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele573NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(25, 40120, "Statue of Goddess Vakarine", "f_gele_57_3", -407.211, -107.0825, -1328.491, 15, "WARP_F_GELE_57_3", "STOUP_CAMP", "STOUP_CAMP");
	}
}
