//--- Melia Script ----------------------------------------------------------
// Underground Grave of Ritinis
//--- Description -----------------------------------------------------------
// NPCs found in and around Underground Grave of Ritinis.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class IdCatacomb04NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "id_catacomb_04", -126.31, 265, 1200.43, -45, "TREASUREBOX_LV_ID_CATACOMB_041000", "", "");
	}
}
