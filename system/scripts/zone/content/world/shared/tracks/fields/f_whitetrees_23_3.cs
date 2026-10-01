//--- Melia Script ----------------------------------------------------------
// Syla Forest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_whitetrees_23_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees233TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157037, "", "f_whitetrees_23_3", -997.1872, 160.0814, 367.3382, 38, "f_whitetrees_23_3_elt", 2, 1);
	}
}
