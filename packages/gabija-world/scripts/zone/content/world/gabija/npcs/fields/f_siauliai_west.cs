//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around West Siauliai Woods.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
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

		// Camp Guards
		//-------------------------------------------------------------------------
		AddNpc(10020, L("Camp Guard"), "SIAUL_WEST_CAMP_GUARD_1", "f_siauliai_west", -589, -822, 70, async dialog =>
		{
			dialog.SetTitle(L("Camp Guard"));

			await dialog.Msg(L("Nobody gets past this line without the knight's word. That's all there is to it."));
		});

		AddNpc(10020, L("Camp Guard"), "SIAUL_WEST_CAMP_GUARD_2", "f_siauliai_west", -509, -822, 288, async dialog =>
		{
			dialog.SetTitle(L("Camp Guard"));

			await dialog.Msg(L("Keep to the road and keep your voice down. We've had enough noise out of those woods."));
		});

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(2027, 147392, "Lv1 Treasure Chest", "f_siauliai_west", 1564, 210, -370, 270, "TREASUREBOX_LV_F_SIAULIAI_WEST2027", "", "");

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
