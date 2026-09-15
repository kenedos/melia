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
		AddWarpStatue(10, "WARP_C_NUNNERY", "c_nunnery", 105, -75, 4, 405);
		
		// Merchant Running from the Petrifying Frost
		//-------------------------------------------------------------------------
		AddNpc(16, 20103, "Merchant Running from the Petrifying Frost", "c_nunnery", 506.4811, -28.62, -335.4216, 90, "HT_ESCAPE_MERCHANT", "", "");
	}
}
