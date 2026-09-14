//--- Melia Script ----------------------------------------------------------
// Topes Fortress 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Topes Fortress 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCastle671NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(17, "WARP_D_CASTLE_67_1", "d_castle_67_1", -1653.771, 0.258728, -1192.015, 45);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(231, 147392, "Lv1 Treasure Chest", "d_castle_67_1", -1386.86, 56.27, -269.79, 0, "TREASUREBOX_LV_D_CASTLE_67_1231", "", "");
	}
}
