//--- Melia Script ----------------------------------------------------------
// Elgos Abbey Main Building
//--- Description -----------------------------------------------------------
// NPCs found in and around Elgos Abbey Main Building.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey354NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(20, "WARP_D_ABBEY_35_4", "d_abbey_35_4", 1284.374, 0.3437, -1020.557, 77);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_abbey_35_4", -1478.5, 125.11, -700.37, 180, "TREASUREBOX_LV_D_ABBEY_35_41000", "", "");
	}
}
