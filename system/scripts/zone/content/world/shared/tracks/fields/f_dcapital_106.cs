//--- Melia Script ----------------------------------------------------------
// Gliehel Memorial Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_dcapital_106'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital106TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153230, "", "f_dcapital_106", 392.5825, 71.17463, 988.9101, 21, "f_dcapital_106_elt", 2, 1);
	}
}
