//--- Melia Script ----------------------------------------------------------
// Lemprasa Pond
//--- Description -----------------------------------------------------------
// NPCs found in and around Lemprasa Pond.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai16NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue(10, "SIAU16_SQ_06_EV_NPC", "f_siauliai_16", 13.88856, 79.7736, 1277.249, 60);
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(11, "WARP_F_SIAULIAI_16", "f_siauliai_16", 642.3004, 25.3504, 2.009714, 73);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(581, 147392, "Lv1 Treasure Chest", "f_siauliai_16", -2294, 116, -1886, 90, "TREASUREBOX_LV_F_SIAULIAI_16581", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(582, 147392, "Lv1 Treasure Chest", "f_siauliai_16", -594, 25, -1993, 45, "TREASUREBOX_LV_F_SIAULIAI_16582", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(583, 147392, "Lv1 Treasure Chest", "f_siauliai_16", 1750, 26, -564, 270, "TREASUREBOX_LV_F_SIAULIAI_16583", "", "");
	}
}
