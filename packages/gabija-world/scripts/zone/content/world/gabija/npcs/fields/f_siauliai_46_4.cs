//--- Melia Script ----------------------------------------------------------
// Dina Bee Farm
//--- Description -----------------------------------------------------------
// NPCs found in and around Dina Bee Farm.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai464NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(6, "WARP_F_SIAULIAI_46_4", "f_siauliai_46_4", -435.1169, 148.2241, -1247.06, 91);
	}
}
