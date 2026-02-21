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
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(679, 147392, "Lv1 Treasure Chest", "f_rokas_25", -2482.46, 268.73, -913.39, 90, "TREASUREBOX_LV_F_ROKAS_25679", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		// AddNpc(682, 147392, "Lv1 Treasure Chest", "f_rokas_25", 1111.726, 347.537, 1219.251, -50, "TREASUREBOX_LV_F_ROKAS_25682", "", "");
	}
}
