//--- Melia Script ----------------------------------------------------------
// Miners' Village
//--- Description -----------------------------------------------------------
// NPCs found in and around Miners' Village.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliaiOutNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(5, "WARP_F_SIAULIAI_OUT", "f_siauliai_out", 190.5049, 42.7921, -1214.24, 0);
		
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue(10031, "F_SIAULIAI_OUT_EV_55_001", "f_siauliai_out", -2194, 40, -2055, 84);
	}
}
