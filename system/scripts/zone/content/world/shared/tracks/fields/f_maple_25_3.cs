//--- Melia Script ----------------------------------------------------------
// Lhadar Forest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_maple_25_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FMaple253TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157036, "", "f_maple_25_3", -917.34, 36.80652, -444.9715, 0, "f_maple_25_3_elt", 2, 1);
	}
}
