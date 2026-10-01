//--- Melia Script ----------------------------------------------------------
// Inner Wall District 9 Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_castle_20_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle202TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157029, "", "f_castle_20_2", -1006.002, 1.829224, -609.7711, 16, "f_castle_20_2_elt", 2, 1);
		AddTrackNPC(157029, "", "f_castle_20_2", -894.3446, -120.3677, 549.2271, 0, "f_castle_20_2_elt2", 2, 1);
	}
}
