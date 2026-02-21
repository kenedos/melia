//--- Melia Script ----------------------------------------------------------
// Delmore Manor
//--- Description -----------------------------------------------------------
// NPCs found in and around Delmore Manor.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle652NpcScript : GeneralScript
{
	protected override void Load()
	{
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(155108, "", "f_castle_65_2", -849.7505, 220.4114, 516.4036, 0, "f_castle_65_2_elt", 2, 1);


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(261, 147392, "Lv1 Treasure Chest", "f_castle_65_2", -507, 923, 1369, 0, "TREASUREBOX_LV_F_CASTLE_65_2261", "", "");
	}
}
