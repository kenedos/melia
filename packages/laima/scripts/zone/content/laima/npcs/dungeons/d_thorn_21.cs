//--- Melia Script ----------------------------------------------------------
// Kvailas Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Kvailas Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn21NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(142, 147392, "Lv1 Treasure Chest", "d_thorn_21", -456.07, 221.33, -407.21, 90, "TREASUREBOX_LV_D_THORN_21142", "", "");

		// Lv3 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(865, 147393, "Lv3 Treasure Chest", "d_thorn_21", 882, 332, 1107, 90, "TREASUREBOX_LV_D_THORN_20865", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(700, 147392, "Lv1 Treasure Chest", "d_thorn_21", 3310, 332, 1009, 225, "TREASUREBOX_LV_D_THORN_20700", "", "");
	}
}
