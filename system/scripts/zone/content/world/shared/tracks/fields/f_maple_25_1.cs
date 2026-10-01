//--- Melia Script ----------------------------------------------------------
// Nheto Forest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_maple_25_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FMaple251TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157034, "", "f_maple_25_1", -953.6499, -38.68602, 177.3624, 342, "f_maple_25_1_elt", 2, 1);
	}
}
