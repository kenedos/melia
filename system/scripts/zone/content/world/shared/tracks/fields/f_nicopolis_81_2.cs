//--- Melia Script ----------------------------------------------------------
// Feline Post Town Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_nicopolis_81_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FNicopolis812TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153232, "", "f_nicopolis_81_2", 60.4, 2.22, -1645, 0, "f_nicopolis_81_2_elt", 2, 1);
	}
}
