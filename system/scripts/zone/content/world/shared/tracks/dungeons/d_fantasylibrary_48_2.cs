//--- Melia Script ----------------------------------------------------------
// Sausis Room 10 Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_fantasylibrary_48_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFantasylibrary482TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157051, "", "d_fantasylibrary_48_2", 60.01674, 69.90582, -182.7349, 90, "d_fantasylibrary_48_2_elt", 2, 1);
	}
}
