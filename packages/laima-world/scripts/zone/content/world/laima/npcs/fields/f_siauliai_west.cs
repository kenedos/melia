//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around West Siauliai Woods.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Scripting.Extensions.LivelyDialog;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliaiWestNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(4, "WARP_F_SIAULIAI_WEST", "f_siauliai_west", -525, 260, -435, 0);

		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(2026, 20026, "Statue of Goddess Zemyna", "f_siauliai_west", 1705.19, 285.05, 390.19, 90, "", "SIAUL_WEST_LAIMONAS3_TRIGGER", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(2027, 147392, "Lv1 Treasure Chest", "f_siauliai_west", 1564, 210, -370, 270, "TREASUREBOX_LV_F_SIAULIAI_WEST2027", "", "");

		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue(2029, "F_SIAULIAI_WEST_EV_55_001", "f_siauliai_west", 1687, 285.05, 366, 20);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(2032, 147392, "Lv1 Treasure Chest", "f_siauliai_west", -580, 260, -1417, 180, "TREASUREBOX_LV_F_SIAULIAI_WEST2032", "", "");

		// Lv3 Treasure Chest (Cow Headband)
		//-------------------------------------------------------------------------
		AddNpc(2035, 147393, "Lv3 Treasure Chest", "f_siauliai_west", 185.81, 210.31, -856.9, 90, "TREASUREBOX_LV_F_SIAULIAI_WEST2035", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(2036, 147392, "Lv1 Treasure Chest", "f_siauliai_west", 1346.05, 210.31, -1087.24, 90, "TREASUREBOX_LV_F_SIAULIAI_WEST2036", "", "");

		// Lv3 Treasure Chest (Muscharia Hat)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147393, "Lv3 Treasure Chest", "f_siauliai_west", 1738, 283, 449, 315, "TREASUREBOX_LV_F_SIAULIAI_WEST9001", "", "");

		// Lv1 Treasure Chest (Lv1 EXP Card)
		//-------------------------------------------------------------------------
		AddNpc(9002, 147392, "Lv1 Treasure Chest", "f_siauliai_west", -441, 360, 1561, 0, "TREASUREBOX_LV_F_SIAULIAI_WEST9002", "", "");

		// Lv1 Treasure Chest (Lv1 EXP Card)
		//-------------------------------------------------------------------------
		AddNpc(9003, 147392, "Lv1 Treasure Chest", "f_siauliai_west", -2153, 261, -465, 90, "TREASUREBOX_LV_F_SIAULIAI_WEST9003", "", "");
	}
}
