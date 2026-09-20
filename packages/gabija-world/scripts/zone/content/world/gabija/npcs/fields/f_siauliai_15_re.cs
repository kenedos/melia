//--- Melia Script ----------------------------------------------------------
// Woods of the Linked Bridges
//--- Description -----------------------------------------------------------
// NPCs found in and around Woods of the Linked Bridges.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai15ReNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(10, "WARP_F_SIAULIAI_15RE", "f_siauliai_15_re", 372.0667, 878.1719, 194.3833, -4);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(643, 147392, "Lv1 Treasure Chest", "f_siauliai_15_re", 1575, 878, 435, 0, "TREASUREBOX_LV_F_SIAULIAI_15_RE643", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(644, 147392, "Lv1 Treasure Chest", "f_siauliai_15_re", 463.58, 922.86, 1401.50, 45, "TREASUREBOX_LV_F_SIAULIAI_15_RE644", "", "");

		// Lv1 Treasure Chest (STA Necklace)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147392, "Lv1 Treasure Chest", "f_siauliai_15_re", -1147, 742, -305, 270, "TREASUREBOX_LV_F_SIAULIAI_15_RE9001", "", "");

		// Lv1 Treasure Chest (Lv1 EXP Card)
		//-------------------------------------------------------------------------
		AddNpc(9002, 147392, "Lv1 Treasure Chest", "f_siauliai_15_re", 4, 1015, 2776, 0, "TREASUREBOX_LV_F_SIAULIAI_15_RE9002", "", "");

		// Lv1 Treasure Chest (Lv1 EXP Card)
		//-------------------------------------------------------------------------
		AddNpc(9003, 147392, "Lv1 Treasure Chest", "f_siauliai_15_re", 1792, 878, -1950, 90, "TREASUREBOX_LV_F_SIAULIAI_15_RE9003", "", "");

		// Lv1 Treasure Chest (Lv1 EXP Card)
		//-------------------------------------------------------------------------
		AddNpc(9004, 147392, "Lv1 Treasure Chest", "f_siauliai_15_re", -737, 1146, -2410, 0, "TREASUREBOX_LV_F_SIAULIAI_15_RE9004", "", "");
	}
}
