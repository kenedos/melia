//--- Melia Script ----------------------------------------------------------
// Mollogheo Forest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_bracken_43_4'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken434TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153181, "", "f_bracken_43_4", 665.4066, 59.12576, 327.8538, 0, "f_bracken_43_4_elt", 2, 1);
	}
}
