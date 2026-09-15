//--- Melia Script ----------------------------------------------------------
// Grynas Hills
//--- Description -----------------------------------------------------------
// NPCs found in and around Grynas Hills.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn453NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(5, "WARP_F_KATYN_45_3", "f_katyn_45_3", -463.6504, 81.97291, -370.6847, 0);
	}
}
