//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum Constructors' Chapel
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum Constructors' Chapel.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress592NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(28, 40110, "Statue of Goddess Zemyna", "d_underfortress_59_2", -951.0473, 0.377, 696.8472, 0, "UNDERF592_ZEMINA_STATUE", "UNDERF592_ZEMINA_STATUE", "UNDERF592_ZEMINA_STATUE");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(37, 40120, "Statue of Goddess Vakarine", "d_underfortress_59_2", 449.585, 0.377, -208.2484, -30, "WARP_D_UNDERFORTRESS_59_2", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(45, 147392, "Lv1 Treasure Chest", "d_underfortress_59_2", -963, 0, -1128, 90, "TREASUREBOX_LV_D_UNDERFORTRESS_59_245", "", "");
	}
}
