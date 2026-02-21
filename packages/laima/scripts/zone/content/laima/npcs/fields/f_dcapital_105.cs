//--- Melia Script ----------------------------------------------------------
// Baltinel Memorial
//--- Description -----------------------------------------------------------
// NPCs found in and around Baltinel Memorial.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital105NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(23, 40110, "Statue of Goddess Zemyna", "f_dcapital_105", -1997.51, 127.5562, 1472.45, 90, "DCAPITAL_105_EV_55_001", "DCAPITAL_105_EV_55_001", "DCAPITAL_105_EV_55_001");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153231, "", "f_dcapital_105", -264.1042, 111.0862, -237.836, 0, "f_dcapital_105_elt", 2, 1);

	}
}
