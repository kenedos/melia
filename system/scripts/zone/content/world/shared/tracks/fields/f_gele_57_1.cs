//--- Melia Script ----------------------------------------------------------
// Srautas Gorge Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_gele_57_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele571TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153006, "", "f_gele_57_1", -305.9306, 168.8233, -402.949, 0, "f_gele_57_1_elt", 2, 1);
	}
}
