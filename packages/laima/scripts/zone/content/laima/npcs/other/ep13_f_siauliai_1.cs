//--- Melia Script ----------------------------------------------------------
// Lemprasa Pond
//--- Description -----------------------------------------------------------
// NPCs found in and around Lemprasa Pond.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep13FSiauliai1NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(7, 40110, "Statue of Goddess Zemyna", "ep13_f_siauliai_1", 16.08543, 79.7736, 1297.291, 68, "SIAU16_SQ_06_EV_NPC", "SIAU16_SQ_06_EV_NPC", "SIAU16_SQ_06_EV_NPC");
	}
}
