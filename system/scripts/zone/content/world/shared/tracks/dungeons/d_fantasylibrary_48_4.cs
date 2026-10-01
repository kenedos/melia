//--- Melia Script ----------------------------------------------------------
// Valandis Room 3 Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_fantasylibrary_48_4'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFantasylibrary484TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157053, "", "d_fantasylibrary_48_4", -1004.409, 6.635758, -709.1039, 90, "d_fantasylibrary_48_4_elt", 2, 1);
	}
}
