//--- Melia Script ----------------------------------------------------------
// (Closed) Delmore Hamlet Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_castle_65_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle651TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(155112, "", "f_castle_65_1", -728.6979, -8.345245, 768.4816, 0, "f_castle_65_1_elt", 2, 1);
	}
}
