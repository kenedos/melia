//--- Melia Script ----------------------------------------------------------
// Jonael Commemorative Orb Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_dcapital_20_6'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital206TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157031, "", "f_dcapital_20_6", -755.0917, 283.1046, 1777.638, 0, "f_dcapital_20_6_elt", 2, 1);
	}
}
