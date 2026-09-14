//--- Melia Script ----------------------------------------------------------
// Mochia Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Mochia Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad313NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(21, "WARP_F_PILGRIMROAD_31_3", "f_pilgrimroad_31_3", -1138.227, 125.4098, -1534.654, 0);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(123, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_31_3", -545.98, 125.48, -1684.74, 135, "TREASUREBOX_LV_F_PILGRIMROAD_31_3123", "", "");
	}
}
