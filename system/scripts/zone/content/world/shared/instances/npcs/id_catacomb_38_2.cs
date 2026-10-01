//--- Melia Script ----------------------------------------------------------
// Mokusul Chamber
//--- Description -----------------------------------------------------------
// NPCs found in and around Mokusul Chamber.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb382NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(18, "WARP_ID_CATACOMB_38_2", "id_catacomb_38_2", 265.5167, -279.9601, 1848.505, 0);
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "id_catacomb_38_2", 1424.84, 2.36, -371.59, 0, "TREASUREBOX_LV_ID_CATACOMB_38_21000", "", "");
	}
}
