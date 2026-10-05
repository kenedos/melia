//--- Melia Script ----------------------------------------------------------
// Gliehel Memorial
//--- Description -----------------------------------------------------------
// NPCs found in and around Gliehel Memorial.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FDcapital106NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(22, 40120, "Statue of Goddess Vakarine", "f_dcapital_106", 1449.792, 89.99438, 642.953, 90, "WARP_DCAPITAL_106", "STOUP_CAMP", "STOUP_CAMP");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153230, "", "f_dcapital_106", 392.5825, 71.17463, 988.9101, 21, "f_dcapital_106_elt", 2, 1);

	}
}
