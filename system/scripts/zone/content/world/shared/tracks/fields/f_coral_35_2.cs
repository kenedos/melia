//--- Melia Script ----------------------------------------------------------
// Vera Coast Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_coral_35_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral352TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(156034, "", "f_coral_35_2", 401.4194, 224.4277, 717.4001, 0, "f_coral_35_2_elt", 2, 1);
	}
}
