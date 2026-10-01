//--- Melia Script ----------------------------------------------------------
// (Closed) Seir Rainforest Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_orchard_32_4'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard324TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(156037, "", "f_orchard_32_4", 1402.254, 135.6041, 557.9661, 0, "f_orchard_32_4_wheel", 2, 1);
		AddTrackNPC(156038, "", "f_orchard_32_4", -333.8946, 19.81506, 796.1085, 90, "f_orchard_32_4_elt", 2, 1);
	}
}
