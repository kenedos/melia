//--- Melia Script ----------------------------------------------------------
// Genar Field
//--- Description -----------------------------------------------------------
// NPCs found in and around Genar Field.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad49NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_49", -169.54, 294.01, -1567.06, 225, "TREASUREBOX_LV_F_PILGRIMROAD_491000", "", "");
	}
}
