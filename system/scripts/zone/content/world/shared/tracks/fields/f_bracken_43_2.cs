//--- Melia Script ----------------------------------------------------------
// Phamer Forest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_bracken_43_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken432TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153179, "", "f_bracken_43_2", 1369.781, 116.1511, 470.741, 0, "f_bracken_43_2_elt", 2, 1);
		AddTrackNPC(153180, "", "f_bracken_43_2", 552.7917, 35.09259, -574.3962, 354, "f_bracken_43_2_elt2", 2, 1);
	}
}
