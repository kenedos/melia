//--- Melia Script ----------------------------------------------------------
// Arrow Path
//--- Description -----------------------------------------------------------
// NPCs found in and around Arrow Path.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn133NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(10025, 147392, "Lv1 Treasure Chest", "f_katyn_13_3", -262, 199, -707, 45, "TREASUREBOX_LV_F_KATYN_710025", "", "");
	}
}
