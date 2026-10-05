//--- Melia Script ----------------------------------------------------------
// Laukyme Swamp
//--- Description -----------------------------------------------------------
// NPCs found in and around Laukyme Swamp.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn393NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_thorn_39_3", -2505.74, 115.31, -124.41, 180, "TREASUREBOX_LV_D_THORN_39_31000", "", "");
	}
}
