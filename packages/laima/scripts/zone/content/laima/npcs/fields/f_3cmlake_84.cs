//--- Melia Script ----------------------------------------------------------
// Absenta Reservoir
//--- Description -----------------------------------------------------------
// NPCs found in and around Absenta Reservoir.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class F3cmlake84NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(183, 147392, "Lv1 Treasure Chest", "f_3cmlake_84", 808, 273, -339, 180, "TREASUREBOX_LV_F_3CMLAKE_84183", "", "");

		// Lv4 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(40, 147394, "Lv4 Treasure Chest", "f_3cmlake_84", -406, 261, -1097, 0, "TREASUREBOX_LV_F_3CMLAKE_8440", "", "");
	}
}
