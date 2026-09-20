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


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_tableland_11_1", -680.03, -109.71, 584.73, -45, "TREASUREBOX_LV_F_TABLELAND_11_11000", "", "");
	}
}
