//--- Melia Script ----------------------------------------------------------
// Glade Hillroad
//--- Description -----------------------------------------------------------
// NPCs found in and around Glade Hillroad.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn392NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_thorn_39_2", -2216.17, -234.93, -875.64, 90, "TREASUREBOX_LV_D_THORN_39_21000", "", "");
	}
}
