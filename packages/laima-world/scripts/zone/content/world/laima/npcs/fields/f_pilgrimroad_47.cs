//--- Melia Script ----------------------------------------------------------
// Pilgrim Path
//--- Description -----------------------------------------------------------
// NPCs found in and around Pilgrim Path.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad47NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(26, 147392, "Treasure Chest", "f_pilgrimroad_47", -1489.342, 154.5892, 1074.925, 86, "PILGRIM47_SQ_080_BOX", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(40, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_47", 221.46, 137.09, 219.43, 90, "TREASUREBOX_LV_F_PILGRIMROAD_4740", "", "");
	}
}
