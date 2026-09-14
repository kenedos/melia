//--- Melia Script ----------------------------------------------------------
// (Closed) Ashaq Underground Prison 1F Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_prison_62_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison621TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(154059, "", "d_prison_62_1", -241.3938, 200.532, 501.1691, 0, "d_prison_62_1_elt", 2, 1);
	}
}
