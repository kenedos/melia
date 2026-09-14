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
		AddNpc(5, 40120, "Statue of Goddess Vakarine", "f_siauliai_out", 190.5049, 42.7921, -1214.24, 0, "WARP_F_SIAULIAI_OUT", "STOUP_CAMP", "STOUP_CAMP");
		
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(10031, 40110, "Statue of Goddess Zemyna", "f_siauliai_out", -2194, 40, -2055, 84, "F_SIAULIAI_OUT_EV_55_001", "F_SIAULIAI_OUT_EV_55_001", "F_SIAULIAI_OUT_EV_55_001");
	}
}
