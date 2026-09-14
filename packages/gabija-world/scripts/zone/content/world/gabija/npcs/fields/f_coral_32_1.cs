//--- Melia Script ----------------------------------------------------------
// Cranto Coast
//--- Description -----------------------------------------------------------
// NPCs found in and around Cranto Coast.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral321NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Merchant Simonas
		//-------------------------------------------------------------------------
		AddNpc(19, 155035, "Merchant Simonas", "f_coral_32_1", -350.414, 236.2519, -1407.799, 78, "CORAL_32_1_MERCHANT1", "", "");
		
		// Merchant Felicia
		//-------------------------------------------------------------------------
		AddNpc(20, 152065, "Merchant Felicia", "f_coral_32_1", -328.0612, 236.2519, -1339.601, -18, "CORAL_32_1_MERCHANT2", "", "");
		
		// Merchant Alliance Worker
		//-------------------------------------------------------------------------
		AddNpc(47, 147483, "Merchant Alliance Worker", "f_coral_32_1", -463.0125, 236.2519, -1321.642, 182, "CORAL_32_1_WORKER1", "", "");
		
		// Merchant Alliance Worker
		//-------------------------------------------------------------------------
		AddNpc(48, 147485, "Merchant Alliance Worker", "f_coral_32_1", -464.16, 236.2519, -1251.43, 21, "CORAL_32_1_WORKER2", "", "");
	}
}
