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
		AddWarpStatue(25, "WARP_F_GELE_57_3", "f_gele_57_3", -407.211, -107.0825, -1328.491, 15);
	}
}
