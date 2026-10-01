//--- Melia Script ----------------------------------------------------------
// Videntis Shrine
//--- Description -----------------------------------------------------------
// NPCs found in and around Videntis Shrine.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb381NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "id_catacomb_38_1", -1782.17, 201.95, -1355.33, 135, "TREASUREBOX_LV_ID_CATACOMB_38_11000", "", "");
	}
}
