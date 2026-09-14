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
		AddNpc(10, 40110, "Statue of Goddess Zemyna", "f_siauliai_16", 13.88856, 79.7736, 1277.249, 60, "SIAU16_SQ_06_EV_NPC", "", "");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(11, 40120, "Statue of Goddess Vakarine", "f_siauliai_16", 642.3004, 25.3504, 2.009714, 73, "WARP_F_SIAULIAI_16", "STOUP_CAMP", "STOUP_CAMP");
	}
}
