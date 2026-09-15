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
		AddWarpStatue(33, "WARP_F_HUEVILLAGE_58_1", "f_huevillage_58_1", 217.9083, 371.3148, -916.1648, 79);

	}
}
