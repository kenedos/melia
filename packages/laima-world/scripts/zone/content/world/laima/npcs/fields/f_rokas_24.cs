//--- Melia Script ----------------------------------------------------------
// Gateway of the Great King
//--- Description -----------------------------------------------------------
// NPCs found in and around Gateway of the Great King.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas24NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(3, "WARP_F_ROKAS_24", "f_rokas_24", 913, 123, -1882, 0);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(727, 147392, "Lv1 Treasure Chest", "f_rokas_24", -677.85, 724.39, -2528, 90, "TREASUREBOX_LV_F_ROKAS_24727", "", "");
	}
}
