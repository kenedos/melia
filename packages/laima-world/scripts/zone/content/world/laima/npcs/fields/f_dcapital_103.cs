//--- Melia Script ----------------------------------------------------------
// Taniel I Commemorative Orb
//--- Description -----------------------------------------------------------
// NPCs found in and around Taniel I Commemorative Orb.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital103NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_dcapital_103", -336.57, 220.98, -793.69, 90, "TREASUREBOX_LV_F_DCAPITAL_1031000", "", "");

	}
}
