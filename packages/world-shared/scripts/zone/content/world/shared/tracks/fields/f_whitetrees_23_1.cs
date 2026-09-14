//--- Melia Script ----------------------------------------------------------
// Emmet Forest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_whitetrees_23_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees231TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157041, "", "f_whitetrees_23_1", 527.8957, -140.5882, 190.1657, 43, "f_whitetrees_23_1_elt", 2, 1);
	}
}
