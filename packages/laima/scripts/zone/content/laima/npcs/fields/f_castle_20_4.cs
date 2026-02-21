//--- Melia Script ----------------------------------------------------------
// City Wall District 8
//--- Description -----------------------------------------------------------
// NPCs found in and around City Wall District 8.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle204NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_castle_20_4", -1521.71, -89.02, -380.4, 90, "TREASUREBOX_LV_F_CASTLE_20_41000", "", "");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(157025, "", "f_castle_20_4", 1157.179, 126.4701, 271.2875, 0, "d_castle_20_4_elt", 2, 1);

	}
}
