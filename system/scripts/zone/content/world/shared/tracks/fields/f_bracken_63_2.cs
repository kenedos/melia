//--- Melia Script ----------------------------------------------------------
// (Closed) Knidos Jungle Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_bracken_63_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken632TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153112, "", "f_bracken_63_2", -47.54993, 253.4295, -541.7952, 0, "f_bracken_63_2_elt", 2, 1);
	}
}
