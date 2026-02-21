//--- Melia Script ----------------------------------------------------------
// Lanko 26 Waters
//--- Description -----------------------------------------------------------
// NPCs found in and around Lanko 26 Waters.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class F3Cmlake261NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(63, 40120, "Statue of Goddess Vakarine", "f_3cmlake_26_1", -291.0521, -70.41682, 1488.81, 90, "WARP_F_3CMLAKE_26_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(155160, "", "f_3cmlake_26_1", 948.2599, -117.9686, 444.7431, 0, "f_3cmlake_26_1_elt", 2, 1);

	}
}
