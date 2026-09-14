//--- Melia Script ----------------------------------------------------------
// Sventimas Exile
//--- Description -----------------------------------------------------------
// NPCs found in and around Sventimas Exile.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland72NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(9, "WARP_F_TABLELAND_72", "f_tableland_72", -272.5474, 406.1369, -74.31588, 45);
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_tableland_72", -581.2, 487.47, 149.56, 45, "TREASUREBOX_LV_F_TABLELAND_721000", "", "");
	}
}
