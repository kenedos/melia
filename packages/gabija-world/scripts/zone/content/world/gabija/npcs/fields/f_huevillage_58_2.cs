//--- Melia Script ----------------------------------------------------------
// Vieta Gorge
//--- Description -----------------------------------------------------------
// NPCs found in and around Vieta Gorge.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage582NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(34, "WARP_F_HUEVILLAGE_58_2", "f_huevillage_58_2", -515.8, 271.89, -1541.66, 125);
	}
}
