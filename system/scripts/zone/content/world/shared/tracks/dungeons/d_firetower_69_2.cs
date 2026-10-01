//--- Melia Script ----------------------------------------------------------
// Martuis Storage Room Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_firetower_69_2'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower692TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157026, "", "d_firetower_69_2", -757.8323, 85.4979, 728.7798, 45, "d_firetower_69_2_elt", 2, 1);
	}
}
