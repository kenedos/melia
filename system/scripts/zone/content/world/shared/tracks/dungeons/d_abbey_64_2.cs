//--- Melia Script ----------------------------------------------------------
// (Closed) Novaha Annex Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_abbey_64_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey642TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153113, "", "d_abbey_64_2", 275.6379, 348.6844, -439.101, 90, "d_abbey_64_2_elt", 2, 1);
	}
}
