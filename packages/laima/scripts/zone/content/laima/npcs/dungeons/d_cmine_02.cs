//--- Melia Script ----------------------------------------------------------
// Crystal Mine 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Crystal Mine 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine02NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(1, 40120, "Statue of Goddess Vakarine", "d_cmine_02", 1822, -10, 321, 390, "WARP_D_CMINE_02", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(550, 147392, "Lv1 Treasure Chest", "d_cmine_02", 1509.99, 80.69, 853.82, 90, "TREASUREBOX_LV_D_CMINE_02550", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(551, 147392, "Lv1 Treasure Chest", "d_cmine_02", -349.39, -27.2, 1235.15, 90, "TREASUREBOX_LV_D_CMINE_02551", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(553, 147392, "Lv1 Treasure Chest", "d_cmine_02", 142.58, 8.91, 189.01, 90, "TREASUREBOX_LV_D_CMINE_02553", "", "");
	}
}
