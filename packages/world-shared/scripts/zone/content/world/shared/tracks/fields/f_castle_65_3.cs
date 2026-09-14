//--- Melia Script ----------------------------------------------------------
// (Closed) Delmore Outskirts Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_castle_65_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle653TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(155109, "", "f_castle_65_3", 123.7928, 96.6724, 200.1542, 0, "f_castle_65_3_elt", 2, 1);
	}
}
