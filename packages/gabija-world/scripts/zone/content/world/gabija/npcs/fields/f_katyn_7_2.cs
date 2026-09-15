//--- Melia Script ----------------------------------------------------------
// Owl Burial Ground
//--- Description -----------------------------------------------------------
// NPCs found in and around Owl Burial Ground.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn72NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(1, "WARP_F_KATYN_7_2", "f_katyn_7_2", -188, 256, -2292, 91);
	}
}
