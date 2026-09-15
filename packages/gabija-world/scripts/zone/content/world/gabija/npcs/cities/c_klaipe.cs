//--- Melia Script ----------------------------------------------------------
// Klaipeda
//--- Description -----------------------------------------------------------
// NPCs found in and around Klaipeda.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class CKlaipeNpcScript : GeneralScript
{
	protected override void Load()
	{
		// [Item Merchant]{nl}      Mirina
		//-------------------------------------------------------------------------
		AddNpc(1, 20115, "[Item Merchant]{nl}      Mirina", "c_Klaipe", 510.7029, -1.292879, -349.3194, 90, "EMILIA", "", "");
		
		// [Equipment Merchant]{nl}         Dunkel
		//-------------------------------------------------------------------------
		AddNpc(2, 20111, "[Equipment Merchant]{nl}         Dunkel", "c_Klaipe", 394, -1, -475, 90, "AKALABETH", "", "");
		
		// [Accessory Merchant]{nl}        Ronesa
		//-------------------------------------------------------------------------
		AddNpc(3, 20104, "[Accessory Merchant]{nl}        Ronesa", "c_Klaipe", 268.7077, -1.343773, -610.9401, 90, "ALFONSO", "ADDHELP_NPCSHOP", "");
		
		// [Blacksmith]{nl}    Zaras
		//-------------------------------------------------------------------------
		AddNpc(13, 20105, "[Blacksmith]{nl}    Zaras", "c_Klaipe", 600, -1, -83, 90, "BLACKSMITH", "TUTO_REPAIR_NPC", "");
		
		// Statue of Goddess Ausrine
		//-------------------------------------------------------------------------
		AddWarpStatue(10017, "WARP_C_KLAIPE", "c_Klaipe", -206.574, 148.8251, 98.63973, 45, L("Statue of Goddess Ausrine"), 154039);

	}
}
