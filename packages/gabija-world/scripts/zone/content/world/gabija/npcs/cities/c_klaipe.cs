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
		// [Equipment Merchant]{nl}         Dunkel
		//-------------------------------------------------------------------------
		AddNpc(2, 20111, "[Equipment Merchant]{nl}         Dunkel", "c_Klaipe", 394, -1, -475, 90, "AKALABETH", "", "");
		
		// [Blacksmith]{nl}    Zaras
		//-------------------------------------------------------------------------
		AddNpc(13, 20105, "[Blacksmith]{nl}    Zaras", "c_Klaipe", 600, -1, -83, 90, "BLACKSMITH", "TUTO_REPAIR_NPC", "");
		
		// Statue of Goddess Ausrine
		//-------------------------------------------------------------------------
		AddWarpStatue(10017, "WARP_C_KLAIPE", "c_Klaipe", -206.574, 148.8251, 98.63973, 45, L("Statue of Goddess Ausrine"), 154039);

	}
}
