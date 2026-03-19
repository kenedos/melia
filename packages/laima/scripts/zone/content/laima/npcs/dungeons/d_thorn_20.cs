//--- Melia Script ----------------------------------------------------------
// Sirdgela Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Sirdgela Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn20NpcScript : GeneralScript
{
	protected override void Load()
	{
		

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(700, 147392, "Lv1 Treasure Chest", "d_thorn_20", 656.34, 490.35, -1853.62, -135, "TREASUREBOX_LV_D_THORN_20700", "", "");
	}
}
