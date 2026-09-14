//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Ashaq Underground Prison 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep132DPrison1NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(16, "WARP_EP13_2_D_PRISON_1", "ep13_2_d_prison_1", -713, 326, 532, 0);
	}
}
