//--- Melia Script ----------------------------------------------------------
// Grand Corridor
//--- Description -----------------------------------------------------------
// NPCs found in and around Grand Corridor.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCathedral54NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(59, 147392, "Lv1 Treasure Chest", "d_cathedral_54", -1353.72, 3.95, -736.48, 90, "TREASUREBOX_LV_D_CATHEDRAL_5459", "", "");
	}
}
