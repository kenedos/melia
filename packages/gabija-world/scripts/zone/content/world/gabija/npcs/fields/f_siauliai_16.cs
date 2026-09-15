//--- Melia Script ----------------------------------------------------------
// Lemprasa Pond
//--- Description -----------------------------------------------------------
// NPCs found in and around Lemprasa Pond.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai16NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue(10, "SIAU16_SQ_06_EV_NPC", "f_siauliai_16", 13.88856, 79.7736, 1277.249, 60);
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(11, "WARP_F_SIAULIAI_16", "f_siauliai_16", 642.3004, 25.3504, 2.009714, 73);
	}
}
