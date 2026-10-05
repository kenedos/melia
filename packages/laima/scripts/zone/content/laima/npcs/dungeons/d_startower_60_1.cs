//--- Melia Script ----------------------------------------------------------
// Residence of the Fallen Legwyn Family
//--- Description -----------------------------------------------------------
// NPCs found in and around Residence of the Fallen Legwyn Family.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DStartower601NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(21, 40120, "Statue of Goddess Vakarine", "d_startower_60_1", -28.21613, -106.1569, -2381.822, 90, "WARP_D_STARTOWER_60_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(27, 147392, "Lv1 Treasure Chest", "d_startower_60_1", 265, 76, 1427, 90, "TREASUREBOX_LV_D_STARTOWER_60_127", "", "");
	}
}
