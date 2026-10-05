//--- Melia Script ----------------------------------------------------------
// Crystal Mine Lot 2 - 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Crystal Mine Lot 2 - 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine9NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1054, 147392, "Lv1 Treasure Chest", "d_cmine_9", 667, 229, 1447, 90, "TREASUREBOX_LV_D_CMINE_91054", "", "");
	}
}
