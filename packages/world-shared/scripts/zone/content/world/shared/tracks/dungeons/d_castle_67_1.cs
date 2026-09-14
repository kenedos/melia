//--- Melia Script ----------------------------------------------------------
// (Closed) Topes Fortress 1F Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_castle_67_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCastle671TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153129, "", "d_castle_67_1", 183.9423, 277.7617, 330.8154, 21, "d_castle_67_1_elt", 3, 2);
	}
}
