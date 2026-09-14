//--- Melia Script ----------------------------------------------------------
// (Closed) Novaha Assembly Hall Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_abbey_64_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey641TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153102, "", "d_abbey_64_1", -1017.423, 366.9117, -465.3019, 0, "d_abbey_64_1_elt2", 3, 2);
	}
}
