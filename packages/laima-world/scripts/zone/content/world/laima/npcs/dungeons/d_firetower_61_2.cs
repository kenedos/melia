//--- Melia Script ----------------------------------------------------------
// Roxona Reconstruction Agency East Building
//--- Description -----------------------------------------------------------
// NPCs found in and around Roxona Reconstruction Agency East Building.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower612NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_firetower_61_2", 905.93, 332.71, 400.7, 225, "TREASUREBOX_LV_D_FIRETOWER_61_21000", "", "");
	}
}
