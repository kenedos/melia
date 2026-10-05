//--- Melia Script ----------------------------------------------------------
// Nheto Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Nheto Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FMaple251NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_maple_25_1", 1467.73, -124.01, 684.4, 90, "TREASUREBOX_LV_F_MAPLE_25_11000", "", "");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(157034, "", "f_maple_25_1", -953.6499, -38.68602, 177.3624, 342, "f_maple_25_1_elt", 2, 1);

	}
}
