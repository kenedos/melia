//--- Melia Script ----------------------------------------------------------
// Akmens Ridge Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_rokas_27'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas27TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(153009, "", "f_rokas_27", 393.83, 1297.32, -1456.52, 250, "f_rokas27_cablecar", 2, 5);
	}
}
