//--- Melia Script ----------------------------------------------------------
// Spell Tome Town Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_nicopolis_81_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FNicopolis813TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153233, "", "f_nicopolis_81_3", -372.14, -157.52, -888.32, 1, "f_nicopolis_81_3_elt", 4, 1);
	}
}
