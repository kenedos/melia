//--- Melia Script ----------------------------------------------------------
// Grynas Training Camp
//--- Description -----------------------------------------------------------
// NPCs found in and around Grynas Training Camp.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn452NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_katyn_45_2", 339.44, 165.83, 1384.53, 135, "TREASUREBOX_LV_F_KATYN_45_21000", "", "");
	}
}
