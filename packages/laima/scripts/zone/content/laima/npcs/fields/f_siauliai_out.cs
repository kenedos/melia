//--- Melia Script ----------------------------------------------------------
// Miners' Village
//--- Description -----------------------------------------------------------
// NPCs found in and around Miners' Village.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliaiOutNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(5, 40120, "Statue of Goddess Vakarine", "f_siauliai_out", 190.5049, 42.7921, -1214.24, 0, "WARP_F_SIAULIAI_OUT", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(10026, 147392, "Lv1 Treasure Chest", "f_siauliai_out", 1651.87, 147.35, 427.34, 90, "TREASUREBOX_BUBE", "", "");
		
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(10031, 40110, "Statue of Goddess Zemyna", "f_siauliai_out", -2194, 40, -2055, 84, "F_SIAULIAI_OUT_EV_55_001", "F_SIAULIAI_OUT_EV_55_001", "F_SIAULIAI_OUT_EV_55_001");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(10054, 147392, "Lv1 Treasure Chest", "f_siauliai_out", 1224.35, 198.02, 279.95, 90, "TREASUREBOX_LV_F_SIAULIAI_OUT10054", "", "");

		// Lv2 Treasure Chest
		//-------------------------------------------------------------------------
		 AddNpc(10023, 40030, "Lv2 Treasure Chest", "f_siauliai_out", 1451, 229, 577, 0, "TREASUREBOX_LV_F_SIAULIAI_210023", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		 AddNpc(10039, 147392, "Lv1 Treasure Chest", "f_siauliai_out", -1810, 170, -952, 90, "TREASUREBOX_LV_F_SIAULIAI_210039", "", "");
	}
}
