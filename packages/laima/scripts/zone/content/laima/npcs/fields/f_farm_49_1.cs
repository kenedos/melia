//--- Melia Script ----------------------------------------------------------
// Greene Manor
//--- Description -----------------------------------------------------------
// NPCs found in and around Greene Manor.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm491NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(40, 40120, "Statue of Goddess Vakarine", "f_farm_49_1", -1180, 0, 1031, 405, "WARP_F_FARM_49_1", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(42, 147392, "Lv1 Treasure Chest", "f_farm_49_1", -1043, 83, -1664, -135, "TREASUREBOX_LV_F_FARM_49_142", "", "");

		// (Custom) Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_farm_49_1", -505, 65, -1257, 45, "TREASUREBOX_LV_F_SIAULIAI_35_11000", "", "");
	}
}
