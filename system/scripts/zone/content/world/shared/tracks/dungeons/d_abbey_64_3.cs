//--- Melia Script ----------------------------------------------------------
// (Closed) Novaha Institute Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_abbey_64_3'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey643TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153114, "", "d_abbey_64_3", -1155.956, 411.8994, -1264.598, 90, "d_abbey_64_3_elt", 2, 1);
	}
}
