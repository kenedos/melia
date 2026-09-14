//--- Melia Script ----------------------------------------------------------
// Baltinel Memorial Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_dcapital_105'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital105TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153231, "", "f_dcapital_105", -264.1042, 111.0862, -237.836, 0, "f_dcapital_105_elt", 2, 1);
	}
}
