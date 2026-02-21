//--- Melia Script ----------------------------------------------------------
// Test Zone
//--- Description -----------------------------------------------------------
// NPCs found in and around Test Zone.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class TestZoneNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Ausrine
		//-------------------------------------------------------------------------
		AddNpc(108, 40130, "Statue of Goddess Ausrine", "test_zone", 38, 0, 190, 90, "SKILLPOINTUP2", "", "");
	}
}
