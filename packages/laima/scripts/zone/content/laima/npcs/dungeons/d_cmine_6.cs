//--- Melia Script ----------------------------------------------------------
// Crystal Mine 3F
//--- Description -----------------------------------------------------------
// NPCs found in and around Crystal Mine 3F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine6NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(525, 40120, "Statue of Goddess Vakarine", "d_cmine_6", -2175.529, 360.2849, -1773.89, 90, "WARP_D_CMINE_6", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(530, 147392, "Lv1 Treasure Chest", "d_cmine_6", -1145.18, 303.59, 103.15, 0, "TREASUREBOX_LV_D_CMINE_6530", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(540, 147392, "Lv1 Treasure Chest", "d_cmine_6", -874.95, 184.05, -970.45, 90, "TREASUREBOX_LV_D_CMINE_6540", "", "");
	}
}
