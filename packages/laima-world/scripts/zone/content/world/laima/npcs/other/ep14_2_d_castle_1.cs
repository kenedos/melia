//--- Melia Script ----------------------------------------------------------
// Delmore Waiting Area
//--- Description -----------------------------------------------------------
// NPCs found in and around Delmore Waiting Area.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep142DCastle1NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(7, "WARP_EP14_2_DCASTLE_1", "ep14_2_d_castle_1", 1133.609, 1.014136, 37.23258, 92);
	}
}
