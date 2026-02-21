//--- Melia Script ----------------------------------------------------------
// Grynas Hills
//--- Description -----------------------------------------------------------
// NPCs found in and around Grynas Hills.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn453NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(5, 40120, "Statue of Goddess Vakarine", "f_katyn_45_3", -463.6504, 81.97291, -370.6847, 0, "WARP_F_KATYN_45_3", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_katyn_45_3", -850.7, 265.21, 1381.16, 135, "TREASUREBOX_LV_F_KATYN_45_31000", "", "");
	}
}
