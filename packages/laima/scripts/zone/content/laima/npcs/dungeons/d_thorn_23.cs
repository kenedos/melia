//--- Melia Script ----------------------------------------------------------
// Sunset Flag Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Sunset Flag Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn23NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(631, 147392, "Lv1 Treasure Chest", "d_thorn_23", -1369.76, 1041.88, -1847.77, 90, "TREASUREBOX_LV_D_THORN_23631", "", "");
	}
}
