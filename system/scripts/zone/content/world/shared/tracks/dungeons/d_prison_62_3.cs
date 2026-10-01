//--- Melia Script ----------------------------------------------------------
// (Closed) Ashaq Underground Prison 3F Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_prison_62_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison623TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(154058, "", "d_prison_62_3", -324.5829, 1013.862, 329.7611, 0, "d_prison_62_3_elt", 2, 1);
	}
}
