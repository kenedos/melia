//--- Melia Script ----------------------------------------------------------
// Veja Ravine Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_huevillage_58_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage581TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153008, "", "f_huevillage_58_1", 638.2, 129.74, 468.58, 0, "f_huevillage58_1_cablecar", 2, 5);
	}
}
