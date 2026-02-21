//--- Melia Script ----------------------------------------------------------
// Escanciu Village
//--- Description -----------------------------------------------------------
// NPCs found in and around Escanciu Village.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains39NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(55, 147392, "Lv1 Treasure Chest", "f_remains_39", 479.87, 193.18, 656.24, 90, "TREASUREBOX_LV_F_REMAINS_3955", "", "");
	}
}
