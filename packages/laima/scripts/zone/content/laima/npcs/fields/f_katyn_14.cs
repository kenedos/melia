//--- Melia Script ----------------------------------------------------------
// Saknis Plains
//--- Description -----------------------------------------------------------
// NPCs found in and around Saknis Plains.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn14NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(587, 40110, "Statue of Goddess Zemyna", "f_katyn_14", -2765, 306, -1058, 40, "F_KATYN_14_EV_55_001", "F_KATYN_14_EV_55_001", "F_KATYN_14_EV_55_001");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(601, 147392, "Lv1 Treasure Chest", "f_katyn_14", -379.14, 248.79, -1016.75, 90, "TREASUREBOX_LV_F_KATYN_14601", "", "");
	}
}
