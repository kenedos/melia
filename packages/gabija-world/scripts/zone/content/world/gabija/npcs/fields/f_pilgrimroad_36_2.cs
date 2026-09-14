//--- Melia Script ----------------------------------------------------------
// Fasika Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Fasika Plateau.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad362NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Merchant Dolonas
		//-------------------------------------------------------------------------
		AddNpc(1001, 152060, "Merchant Dolonas", "f_pilgrimroad_36_2", -660.7841, 153.1389, 825.1987, 9, "PILGRIM362_RP_1_NPC", "", "");
	}
}
