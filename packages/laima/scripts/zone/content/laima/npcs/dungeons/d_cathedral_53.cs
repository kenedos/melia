//--- Melia Script ----------------------------------------------------------
// Main Building
//--- Description -----------------------------------------------------------
// NPCs found in and around Main Building.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCathedral53NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(63, 147392, "Lv1 Treasure Chest", "d_cathedral_53", 228.81, 0.1, -313.9, 180, "TREASUREBOX_LV_D_CATHEDRAL_5363", "", "");
	}
}
