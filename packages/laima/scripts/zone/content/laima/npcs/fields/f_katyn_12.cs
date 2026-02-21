//--- Melia Script ----------------------------------------------------------
// Letas Stream
//--- Description -----------------------------------------------------------
// NPCs found in and around Letas Stream.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn12NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(24, 40120, "Statue of Goddess Vakarine", "f_katyn_12", 29.33316, 249.4619, -758.8959, 45, "WARP_F_KATYN_12", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(32, 147392, "Lv1 Treasure Chest", "f_katyn_12", 1642, 248.86, 1686, -90, "TREASUREBOX_LV_F_KATYN_1232", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(335, 147392, "Lv1 Treasure Chest", "f_katyn_12", 2148, 249, 965, -45, "TREASUREBOX_LV_F_KATYN_12335", "", "");
	}
}
