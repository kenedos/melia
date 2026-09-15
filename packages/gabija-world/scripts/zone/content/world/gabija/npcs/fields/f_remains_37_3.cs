//--- Melia Script ----------------------------------------------------------
// Istora Ruins
//--- Description -----------------------------------------------------------
// NPCs found in and around Istora Ruins.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains373NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(33, "WARP_F_REMAINS_37_3", "f_remains_37_3", 455.1151, 80.3744, -1175.304, 0);
		
		// [Kedoran Merchant Alliance]{nl} Relic Collector
		//-------------------------------------------------------------------------
		AddNpc(44, 154074, "[Kedoran Merchant Alliance]{nl} Relic Collector", "f_remains_37_3", -2617.604, 52.30331, 2692.306, 5, "GT_RELICSHOP_NPC", "", "");
		
		// [Kedoran Merchant Alliance]{nl} Operator
		//-------------------------------------------------------------------------
		AddNpc(1001, 20100, "[Kedoran Merchant Alliance]{nl} Operator", "f_remains_37_3", -2808.465, 52.30331, 2596.691, 90, "GT_RELICSHOP_NPC2", "", "");
	}
}
