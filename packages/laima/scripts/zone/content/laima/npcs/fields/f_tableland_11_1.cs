//--- Melia Script ----------------------------------------------------------
// Vedas Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Vedas Plateau.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland111NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Wagon Manager
		//-------------------------------------------------------------------------
		AddTrackStartingNPC(20150, "Wagon Manager", "f_tableland_11_1", -2319.81, 2045.69, 14, "cart_slide", -2319.81f, 801.67f, 2045.69f);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_tableland_11_1", -680.03, -109.71, 584.73, -45, "TREASUREBOX_LV_F_TABLELAND_11_11000", "", "");
	}
}
