//--- Melia Script ----------------------------------------------------------
// (Closed) Ashaq Underground Prison 2F Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_prison_62_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison622TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(154058, "", "d_prison_62_2", 5.702232, -707.8696, -1245.999, 0, "d_prison_62_2_elt", 2, 1);
	}
}
