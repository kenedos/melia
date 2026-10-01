//--- Melia Script ----------------------------------------------------------
// Jeromel Park Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_dcapital_20_5'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital205TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157019, "", "f_dcapital_20_5", 929.8597, 294.7133, -722.7009, 0, "f_dcapital_20_5_elt", 2, 5);
	}
}
