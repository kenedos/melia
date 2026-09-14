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
		AddStatPointStatue(7, "SIAU16_SQ_06_EV_NPC", "ep13_f_siauliai_1", 16.08543, 79.7736, 1297.291, 68);
	}
}
