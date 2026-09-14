//--- Melia Script ----------------------------------------------------------
// Ramstis Ridge
//--- Description -----------------------------------------------------------
// NPCs found in and around Ramstis Ridge.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas25NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue("F_ROKAS_25_ZEMYNA", "f_rokas_25", -2017.5, 830.8, 45);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(679, 147392, "Lv1 Treasure Chest", "f_rokas_25", -2482.46, 268.73, -913.39, 90, "TREASUREBOX_LV_F_ROKAS_25679", "", "");
		
		// Lv1 Treasure Chest (Wooden Bangle)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147392, "Lv1 Treasure Chest", "f_rokas_25", -2001, 269, -1335, 90, "TREASUREBOX_LV_F_ROKAS_259001", "", "");

		// Lv1 Treasure Chest (Lv1 EXP Card)
		//-------------------------------------------------------------------------
		AddNpc(9002, 147392, "Lv1 Treasure Chest", "f_rokas_25", 2788, 72, -1059, 45, "TREASUREBOX_LV_F_ROKAS_259002", "", "");

		// Lv1 Treasure Chest (Lv1 EXP Card)
		//-------------------------------------------------------------------------
		AddNpc(9003, 147392, "Lv1 Treasure Chest", "f_rokas_25", 175, 268, 742, 45, "TREASUREBOX_LV_F_ROKAS_259003", "", "");
	}
}
