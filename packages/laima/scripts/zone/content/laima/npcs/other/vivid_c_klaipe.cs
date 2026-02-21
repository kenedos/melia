//--- Melia Script ----------------------------------------------------------
// Rainbow Klaipeda
//--- Description -----------------------------------------------------------
// NPCs found in and around Rainbow Klaipeda.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class VividCKlaipeNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(43, 151167, "Treasure Chest", "VIVID_c_Klaipe", -217.5021, -1.156545, -1091.075, 90, "EVENT_VIVID_KLAIPE_BOX", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(47, 151167, "Treasure Chest", "VIVID_c_Klaipe", 254.3475, -1.156548, -1026.819, 90, "EVENT_VIVID_KLAIPE_BOX2", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(56, 151167, "Treasure Chest", "VIVID_c_Klaipe", 905.7919, -1.156548, -545.6175, 90, "EVENT_VIVID_KLAIPE_BOX3", "", "");
		
		// Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(159, 151166, "Treasure Chest", "VIVID_c_Klaipe", 241.4297, 149.1097, 903.0467, 90, "EVENT_VIVID_KLAIPE_BOX4", "", "");
	}
}
