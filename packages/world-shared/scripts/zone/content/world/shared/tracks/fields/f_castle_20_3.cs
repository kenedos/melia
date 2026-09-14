//--- Melia Script ----------------------------------------------------------
// Inner Wall District 8 Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_castle_20_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle203TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157030, "", "f_castle_20_3", 467.1729, 139.2464, -586.9828, 0, "f_castle_20_3_elt", 2, 1);
	}
}
