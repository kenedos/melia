//--- Melia Script ----------------------------------------------------------
// Apsimesti Crossroads
//--- Description -----------------------------------------------------------
// NPCs found in and around Apsimesti Crossroads.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad52NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Great Merchant Gilliam
		//-------------------------------------------------------------------------
		AddNpc(5, 20154, "Great Merchant Gilliam", "f_pilgrimroad_52", 205, 214, 553, 115, "REQ_SEMPLE_06", "", "");
	}
}
