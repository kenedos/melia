//--- Melia Script ----------------------------------------------------------
// Gele Plateau Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_gele_57_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele572TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153007, "", "f_gele_57_2", -85.66, 381.08, 841.97, 0, "f_gele57_2_cablecar", 2, 5);
	}
}
