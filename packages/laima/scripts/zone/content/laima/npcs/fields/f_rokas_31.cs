//--- Melia Script ----------------------------------------------------------
// Zachariel Crossroads
//--- Description -----------------------------------------------------------
// NPCs found in and around Zachariel Crossroads.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas31NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(621, 40110, "Statue of Goddess Zemyna", "f_rokas_31", 496, 107, -27, -10, "F_ROKAS_31_EV_55_001", "F_ROKAS_31_EV_55_001", "F_ROKAS_31_EV_55_001");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(629, 147392, "Lv1 Treasure Chest", "f_rokas_31", -736, 107.1, -1088, 90, "TREASUREBOX_LV_F_ROKAS_31629", "", "");
	}
}
