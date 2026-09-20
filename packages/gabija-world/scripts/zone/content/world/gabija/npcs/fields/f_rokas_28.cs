//--- Melia Script ----------------------------------------------------------
// Tiltas Valley
//--- Description -----------------------------------------------------------
// NPCs found in and around Tiltas Valley.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas28NpcScript : GeneralScript
{
	protected override void Load()
	{


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1040, 147392, "Lv1 Treasure Chest", "f_rokas_28", 630.55, 1156.98, -159.62, 270, "TREASUREBOX_LV_F_ROKAS_281040", "", "");

		// Lv1 Treasure Chest (Bronza Medal)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147392, "Lv1 Treasure Chest", "f_rokas_28", 174, 1121, -1510, 270, "TREASUREBOX_LV_F_ROKAS_289001", "", "");
	}
}
