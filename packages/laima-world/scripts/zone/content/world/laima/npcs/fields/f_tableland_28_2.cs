//--- Melia Script ----------------------------------------------------------
// Stogas Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Stogas Plateau.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland282NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(45, "WARP_F_TABLELAND_28_2", "f_tableland_28_2", 863.4312, 247.0137, 1283.108, 90);
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_tableland_28_2", 1084.08, 247.78, 1423.66, 90, "TREASUREBOX_LV_F_TABLELAND_28_21000", "", "");
	}
}
