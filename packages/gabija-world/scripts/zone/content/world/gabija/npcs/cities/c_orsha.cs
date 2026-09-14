//--- Melia Script ----------------------------------------------------------
// Orsha
//--- Description -----------------------------------------------------------
// NPCs found in and around Orsha.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class COrshaNpcScript : GeneralScript
{
	protected override void Load()
	{
		// [Blacksmith]{nl}  Ilanai
		//-------------------------------------------------------------------------
		AddNpc(2, 20066, "[Blacksmith]{nl}  Ilanai", "c_orsha", -133.44, 175.98, -285.69, 73, "ORSHA_BLACKSMITH", "TUTO_REPAIR_NPC", "");
		
		// [Equipment Merchant]{nl}   Jura
		//-------------------------------------------------------------------------
		AddNpc(3, 20056, "[Equipment Merchant]{nl}   Jura", "c_orsha", 21, 176, 154, 123, "ORSHA_EQUIPMENT_DEALER", "ADDHELP_NPCSHOP", "");
		
		// [Item Merchant]{nl}    Alf
		//-------------------------------------------------------------------------
		AddNpc(9, 20055, "[Item Merchant]{nl}    Alf", "c_orsha", 231, 175, 166, 120, "ORSHA_TOOL_NPC", "", "");
		
		// [Accessory Merchant]{nl}    Jurus
		//-------------------------------------------------------------------------
		AddNpc(11, 20057, "[Accessory Merchant]{nl}    Jurus", "c_orsha", 462.1917, 175.9214, -29.93526, -11, "ORSHA_ACCESSARY_NPC", "ORSHA_HQ1_CONDITION", "");
		
		// Statue of Goddess Ausrine
		//-------------------------------------------------------------------------
		AddNpc(115, 154063, "Statue of Goddess Ausrine", "c_orsha", 103.14, 176.14, 322.95, -46, "WARP_C_ORSHA", "STOUP_CAMP", "STOUP_CAMP");
	}
}
