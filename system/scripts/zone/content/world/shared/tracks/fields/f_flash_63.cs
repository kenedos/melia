//--- Melia Script ----------------------------------------------------------
// Downtown Track NPCs
//--- Description -----------------------------------------------------------
// Elevators, cable cars and other track-driven NPCs on 'f_flash_63'.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash63TrackNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddTrackNPC(147427, "", "f_flash_63", 897.8192, 233.7511, 654.0224, 357.82477, "flash_elevator", 2, 1);
	}
}
