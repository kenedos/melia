//--- Melia Script ----------------------------------------------------------
// Goddess' Ancient Garden
//--- Description -----------------------------------------------------------
// NPCs found in and around Goddess' Ancient Garden.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains38NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(74, "WARP_F_REMAINS_38", "f_remains_38", 340.66, 277.95, -457.09, 0);
	}
}
