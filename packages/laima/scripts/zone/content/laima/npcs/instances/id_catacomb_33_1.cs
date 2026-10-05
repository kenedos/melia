//--- Melia Script ----------------------------------------------------------
// Sienakal Graveyard
//--- Description -----------------------------------------------------------
// NPCs found in and around Sienakal Graveyard.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb331NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(25, 40120, "Statue of Goddess Vakarine", "id_catacomb_33_1", 589.2645, 235.7689, -714.1481, 45, "WARP_ID_CATACOMB_33_1", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(248, 147392, "Lv1 Treasure Chest", "id_catacomb_33_1", -1281, 160, 1123, 0, "TREASUREBOX_LV_ID_CATACOMB_33_1248", "", "");
	}
}
