//--- Melia Script ----------------------------------------------------------
// Alembique Cave Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'd_limestonecave_55_1'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DLimestonecave551TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(154099, "", "d_limestonecave_55_1", 1227.525, 151.018, 529.8058, 16, "d_limestonecave_55_1_elt", 2, 1);
	}
}
