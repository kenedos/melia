//--- Melia Script ----------------------------------------------------------
// Emmet Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Emmet Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees231NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue("F_WHITETREES_23_1_ZEMYNA", "f_whitetrees_23_1", 1596.0, -487.4, 0);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_whitetrees_23_1", 1295.3, 305.91, -644.38, 90, "TREASUREBOX_LV_F_WHITETREES_23_11000", "", "");

	}
}
