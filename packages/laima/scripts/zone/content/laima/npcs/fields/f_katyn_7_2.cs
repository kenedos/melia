//--- Melia Script ----------------------------------------------------------
// Owl Burial Ground
//--- Description -----------------------------------------------------------
// NPCs found in and around Owl Burial Ground.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn72NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(1, 40120, "Statue of Goddess Vakarine", "f_katyn_7_2", -188, 256, -2292, 91, "WARP_F_KATYN_7_2", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(10029, 147392, "Lv1 Treasure Chest", "f_katyn_7_2", 3396.75, 183.1, -4706.59, 90, "TREASUREBOX_LV_F_KATYN_7_210029", "", "");
	}
}
