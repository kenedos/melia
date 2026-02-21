//--- Melia Script ----------------------------------------------------------
// Guards Graveyard
//--- Description -----------------------------------------------------------
// NPCs found in and around Guards Graveyard.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb01NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(8, 40120, "Statue of Goddess Vakarine", "id_catacomb_01", 1344.74, 887.0438, -1033.726, 90, "WARP_ID_CATACOMB_01", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(700, 147392, "Lv1 Treasure Chest", "id_catacomb_01", 180.11, 935.79, -523.76, 90, "TREASUREBOX_LV_ID_CATACOMB_01700", "", "");
	}
}
