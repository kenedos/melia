//--- Melia Script ----------------------------------------------------------
// Lanko 26 Waters Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_3cmlake_26_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class F3cmlake261TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(155160, "", "f_3cmlake_26_1", 948.2599, -117.9686, 444.7431, 0, "f_3cmlake_26_1_elt", 2, 1);
	}
}
