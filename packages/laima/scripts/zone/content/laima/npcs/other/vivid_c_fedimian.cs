//--- Melia Script ----------------------------------------------------------
// Rainbow Fedimian
//--- Description -----------------------------------------------------------
// NPCs found in and around Rainbow Fedimian.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class VividCFedimianNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(10, 151167, "Treasure Chest", "VIVID_c_fedimian", 115.8499, 160.555, -506.0249, 90, "EVENT_VIVID_FEDIMIAN_BOX", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(29, 151167, "Treasure Chest", "VIVID_c_fedimian", -758.3153, 208.2247, -120.335, 90, "EVENT_VIVID_FEDIMIAN_BOX2", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(106, 151167, "Treasure Chest", "VIVID_c_fedimian", 798.2995, 448.5507, 371.3386, 90, "EVENT_VIVID_FEDIMIAN_BOX3", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(135, 151166, "Treasure Chest", "VIVID_c_fedimian", -433.4216, 796.6687, 476.4886, 90, "EVENT_VIVID_FEDIMIAN_BOX4", "", "");
	}
}
