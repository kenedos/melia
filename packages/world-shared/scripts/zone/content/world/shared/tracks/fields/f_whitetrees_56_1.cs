//--- Melia Script ----------------------------------------------------------
// Mishekan Forest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_whitetrees_56_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees561TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157038, "", "f_whitetrees_56_1", 1384.889, 68.06953, 243.3077, 10, "f_whitetrees_56_1_elt", 2, 1);
	}
}
