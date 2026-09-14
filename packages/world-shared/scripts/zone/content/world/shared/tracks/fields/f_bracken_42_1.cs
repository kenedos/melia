//--- Melia Script ----------------------------------------------------------
// Khonot Forest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_bracken_42_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken421TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157042, "", "f_bracken_42_1", 782.5125, 558.6927, -747.0397, 0, "f_bracken_42_1_elt", 2, 1);
		AddTrackNPC(157043, "", "f_bracken_42_1", 1067.31, 557.06, -425.56, 0, "f_bracken_42_1_elt2", 2, 1);
	}
}
