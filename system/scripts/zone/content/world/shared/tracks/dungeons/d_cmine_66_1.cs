//--- Melia Script ----------------------------------------------------------
// (Closed) Nevellet Quarry 1F Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_cmine_66_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCmine661TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(155110, "", "d_cmine_66_1", 77.5127, 413.2733, -77.08086, 0, "d_cmine_66_1_elt", 2, 1);
	}
}
