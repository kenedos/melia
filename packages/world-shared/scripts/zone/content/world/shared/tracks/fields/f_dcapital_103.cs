//--- Melia Script ----------------------------------------------------------
// Taniel I Commemorative Orb Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_dcapital_103'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital103TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(157032, "", "f_dcapital_103", 1260.842, 214.2436, -324.7393, 8, "f_dcapital_103_elt", 2, 1);
		AddTrackNPC(157033, "", "f_dcapital_103", -401.9065, 174.6531, -97.95161, 0, "f_dcapital_103_elt2", 2, 1);
	}
}
