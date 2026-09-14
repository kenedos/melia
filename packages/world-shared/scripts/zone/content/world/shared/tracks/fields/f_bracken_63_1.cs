//--- Melia Script ----------------------------------------------------------
// (Closed) Koru Jungle Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_bracken_63_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken631TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153107, "", "f_bracken_63_1", -268.1401, 544.2999, 995.2676, 21, "f_bracken_63_1_elt", 3);
	}
}
