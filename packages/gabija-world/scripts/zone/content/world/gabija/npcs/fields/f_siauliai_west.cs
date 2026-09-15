//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around West Siauliai Woods.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliaiWestNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(4, "WARP_F_SIAULIAI_WEST", "f_siauliai_west", -525, 260, -435, 0);
		
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(2026, 20026, "Statue of Goddess Zemyna", "f_siauliai_west", 1705.19, 285.05, 390.19, 90, "", "SIAUL_WEST_LAIMONAS3_TRIGGER", "");
	}
}
