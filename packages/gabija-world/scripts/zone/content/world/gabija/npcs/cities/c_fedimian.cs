//--- Melia Script ----------------------------------------------------------
// Fedimian
//--- Description -----------------------------------------------------------
// NPCs found in and around Fedimian.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class CFedimianNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(10, "WARP_C_FEDIMIAN", "c_fedimian", -280, 162, -239, 7);
		
		// [Item Merchant]{nl}  Muras
		//-------------------------------------------------------------------------
		AddNpc(108, 151034, "[Item Merchant]{nl}  Muras", "c_fedimian", -631.32, 169.31, -174.9, 0, "FED_TOOL", "", "");
		
		// [Equipment Merchant]{nl}  Yorgis
		//-------------------------------------------------------------------------
		AddNpc(109, 151035, "[Equipment Merchant]{nl}  Yorgis", "c_fedimian", -219.15, 178.05, -558.35, 90, "FED_EQUIP", "FED_EQUIP_HQ_REINFORCE", "FED_EQUIP_HQ_REINFORCE");
		
		// [Blacksmith]{nl}    Anna
		//-------------------------------------------------------------------------
		AddNpc(126, 151036, "[Blacksmith]{nl}    Anna", "c_fedimian", 120, 160, -504, 75, "BLACKSMITH_FEDIMIAN", "", "");
		
		// [Accessory Merchant]{nl}  Joana
		//-------------------------------------------------------------------------
		AddNpc(130, 151038, "[Accessory Merchant]{nl}  Joana", "c_fedimian", -130.2, 177.28, -496.14, 0, "FED_ACCESSORY", "", "");

	}
}
