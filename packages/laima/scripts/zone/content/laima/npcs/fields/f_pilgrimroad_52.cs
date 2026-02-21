//--- Melia Script ----------------------------------------------------------
// Apsimesti Crossroads
//--- Description -----------------------------------------------------------
// NPCs found in and around Apsimesti Crossroads.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad52NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Hidden Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(46, 40030, "Hidden Treasure Chest", "f_pilgrimroad_52", 2652.372, 156.583, -374.2712, 90, "PILGRIM52_BOX", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(64, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_52", 991.91, 234, 1860.75, 90, "TREASUREBOX_LV_F_PILGRIMROAD_5264", "", "");
	}
}
