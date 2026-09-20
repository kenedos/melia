//--- Melia Script ----------------------------------------------------------
// Zeraha
//--- Description -----------------------------------------------------------
// NPCs found in and around Zeraha.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard342NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(314, 147392, "Lv1 Treasure Chest", "f_orchard_34_2", -2120, 1429, -103, 0, "TREASUREBOX_LV_F_ORCHARD_34_2314", "", "");

		// Lv3 Treasure Chest (Feather Hat)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147393, "Lv3 Treasure Chest", "f_orchard_34_2", 1242, 293, 288, 0, "TREASUREBOX_LV_F_ORCHARD_34_29001", "", "");
	}
}
