//--- Melia Script ----------------------------------------------------------
// Igti Coast Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_coral_32_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral322TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157003, "", "f_coral_32_2", -641.0241, -36.33413, 1405.18, 21, "f_coral_32_2_elt", 3, 2);
	}
}
