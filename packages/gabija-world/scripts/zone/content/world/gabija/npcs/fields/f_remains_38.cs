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
		AddNpc(74, 40120, "Statue of Goddess Vakarine", "f_remains_38", 340.66, 277.95, -457.09, 0, "WARP_F_REMAINS_38", "STOUP_CAMP", "STOUP_CAMP");
	}
}
