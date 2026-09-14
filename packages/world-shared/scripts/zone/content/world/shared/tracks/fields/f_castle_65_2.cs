//--- Melia Script ----------------------------------------------------------
// (Closed) Delmore Manor Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_castle_65_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle652TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(155108, "", "f_castle_65_2", -849.7505, 220.4114, 516.4036, 0, "f_castle_65_2_elt", 2, 1);
	}
}
