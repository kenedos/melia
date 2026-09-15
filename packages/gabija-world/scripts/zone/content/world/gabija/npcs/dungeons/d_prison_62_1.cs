//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Ashaq Underground Prison 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison621NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(6, "WARP_D_PRISON_62_1", "d_prison_62_1", -511.9751, 326.1438, 86.08859, -3);

	}
}
