//--- Melia Script ----------------------------------------------------------
// (Closed) Sicarius 2F Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_underfortress_68_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress682TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153135, "", "d_underfortress_68_2", 600.5497, 205.7321, 37.91431, 0, "d_underfortress_68_2_elt", 3, 2);
		AddTrackNPC(153135, "", "d_underfortress_68_2", 600.5497, 205.7321, 37.91431, 0, "d_underfortress_68_2_elt", 3, 2);
		AddTrackNPC(153135, "", "d_underfortress_68_2", 830.5077, 195.7104, -1507.612, 0, "d_underfortress_68_2_elt2", 3, 2);
	}
}
