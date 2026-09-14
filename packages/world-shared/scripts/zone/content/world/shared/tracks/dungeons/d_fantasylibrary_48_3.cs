//--- Melia Script ----------------------------------------------------------
// Valandis Room 2 Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_fantasylibrary_48_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFantasylibrary483TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157052, "", "d_fantasylibrary_48_3", -321.4122, 138.6892, 209.0464, 90, "d_fantasylibrary_48_3_elt", 2, 1);
	}
}
