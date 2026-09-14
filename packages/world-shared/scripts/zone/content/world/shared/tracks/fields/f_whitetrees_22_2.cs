//--- Melia Script ----------------------------------------------------------
// Tekel Shelter Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_whitetrees_22_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees222TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153189, "", "f_whitetrees_22_2", 990.0172, 125.708, -499.8661, 351, "f_whitetrees_22_2_elt", 2, 1);
	}
}
