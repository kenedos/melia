//--- Melia Script ----------------------------------------------------------
// Veja Ravine
//--- Description -----------------------------------------------------------
// NPCs found in and around Veja Ravine.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage581NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(33, 40120, "Statue of Goddess Vakarine", "f_huevillage_58_1", 217.9083, 371.3148, -916.1648, 79, "WARP_F_HUEVILLAGE_58_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153008, "", "f_huevillage_58_1", 638.2, 129.74, 468.58, 0, "f_huevillage58_1_cablecar", 2, 5);

	}
}
