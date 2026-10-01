//--- Melia Script ----------------------------------------------------------
// (Closed) Zeraha Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_orchard_34_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard342TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(155059, "", "f_orchard_34_2", -1761.933, -357.2179, 384.1256, 90, "f_orchard_34_2_elevator", 3);
	}
}
