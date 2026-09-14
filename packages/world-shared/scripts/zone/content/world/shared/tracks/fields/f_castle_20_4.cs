//--- Melia Script ----------------------------------------------------------
// City Wall District 8 Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_castle_20_4'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle204TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157025, "", "f_castle_20_4", 1157.179, 126.4701, 271.2875, 0, "d_castle_20_4_elt", 2, 1);
	}
}
