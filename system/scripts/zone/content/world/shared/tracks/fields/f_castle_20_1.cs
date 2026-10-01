//--- Melia Script ----------------------------------------------------------
// Outer Wall District 9 Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_castle_20_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle201TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157027, "", "f_castle_20_1", 405.7464, 150.6457, -274.809, 0, "f_castle_20_1_elt", 2, 1);
		AddTrackNPC(157028, "", "f_castle_20_1", -576.8862, 237.7089, -320.7311, 90, "f_castle_20_1_elt2", 2, 1);
	}
}
