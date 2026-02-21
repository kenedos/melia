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
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_whitetrees_23_1", 1295.3, 305.91, -644.38, 90, "TREASUREBOX_LV_F_WHITETREES_23_11000", "", "");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(157041, "", "f_whitetrees_23_1", 527.8957, -140.5882, 190.1657, 43, "f_whitetrees_23_1_elt", 2, 1);

	}
}
