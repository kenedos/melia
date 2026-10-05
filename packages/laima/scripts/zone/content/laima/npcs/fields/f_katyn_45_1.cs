//--- Melia Script ----------------------------------------------------------
// Grynas Trails
//--- Description -----------------------------------------------------------
// NPCs found in and around Grynas Trails.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn451NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(7, 40120, "Statue of Goddess Vakarine", "f_katyn_45_1", -2121.582, 128.0495, -254.6491, 0, "WARP_F_KATYN_45_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_katyn_45_1", -489.59, 159.83, 627.17, -45, "TREASUREBOX_LV_F_KATYN_45_11000", "", "");
	}
}
