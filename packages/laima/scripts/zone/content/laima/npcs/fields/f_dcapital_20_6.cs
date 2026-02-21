//--- Melia Script ----------------------------------------------------------
// Jonael Commemorative Orb
//--- Description -----------------------------------------------------------
// NPCs found in and around Jonael Commemorative Orb.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital206NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_dcapital_20_6", -1107.86, 192.17, 1600.94, 90, "TREASUREBOX_LV_F_DCAPITAL_20_61000", "", "");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(157031, "", "f_dcapital_20_6", -755.0917, 283.1046, 1777.638, 0, "f_dcapital_20_6_elt", 2, 1);

	}
}
