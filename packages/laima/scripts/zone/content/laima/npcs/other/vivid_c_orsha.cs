//--- Melia Script ----------------------------------------------------------
// Rainbow Orsha
//--- Description -----------------------------------------------------------
// NPCs found in and around Rainbow Orsha.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class VividCOrshaNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(64, 151167, "Treasure Chest", "VIVID_c_orsha", -437.0525, 447.2, 130.4015, 2, "EVENT_VIVID_ORSHA_BOX", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(138, 151167, "Treasure Chest", "VIVID_c_orsha", 270.5591, 345.4451, 765.3452, -8, "EVENT_VIVID_ORSHA_BOX2", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(180, 151166, "Treasure Chest", "VIVID_c_orsha", 1430.157, 21.81651, 371.0398, -4, "EVENT_VIVID_ORSHA_BOX4", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(181, 151167, "Treasure Chest", "VIVID_c_orsha", -196.2404, 317.9385, 305.8789, 90, "EVENT_VIVID_ORSHA_BOX3", "", "");
	}
}
