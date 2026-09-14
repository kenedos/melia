//--- Melia Script ----------------------------------------------------------
// Saalus Convent
//--- Description -----------------------------------------------------------
// NPCs found in and around Saalus Convent.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class CNunneryNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(10, 40120, "Statue of Goddess Vakarine", "c_nunnery", 105, -75, 4, 405, "WARP_C_NUNNERY", "STOUP_CAMP", "STOUP_CAMP");
		
		// Merchant Running from the Petrifying Frost
		//-------------------------------------------------------------------------
		AddNpc(16, 20103, "Merchant Running from the Petrifying Frost", "c_nunnery", 506.4811, -28.62, -335.4216, 90, "HT_ESCAPE_MERCHANT", "", "");
	}
}
