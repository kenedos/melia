//--- Melia Script ----------------------------------------------------------
// Epherotao Coast Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_coral_44_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCoral443TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157050, "", "f_coral_44_3", 464.66, 92.97, 518.12, 335, "f_coral_44_3_elt", 2, 1);
	}
}
