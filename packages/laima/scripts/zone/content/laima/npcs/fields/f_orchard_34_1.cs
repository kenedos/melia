//--- Melia Script ----------------------------------------------------------
// Alemeth Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Alemeth Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard341NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_orchard_34_1", -1538.84, -96.2, -433.86, 90, "TREASUREBOX_LV_F_ORCHARD_34_11000", "", "");
	}
}
