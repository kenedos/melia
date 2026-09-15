//--- Melia Script ----------------------------------------------------------
// Dvasia Peak
//--- Description -----------------------------------------------------------
// NPCs found in and around Dvasia Peak.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn22NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(7, "WARP_D_THORN_22", "d_thorn_22", 66.92315, 559.9864, -1211.507, 45);
	}
}
