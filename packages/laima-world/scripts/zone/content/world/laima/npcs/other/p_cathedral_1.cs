//--- Melia Script ----------------------------------------------------------
// Passage of the Recluse
//--- Description -----------------------------------------------------------
// NPCs found in and around Passage of the Recluse.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class PCathedral1NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(3, "WARP_P_CATHEDRAL_1", "p_cathedral_1", 429.5168, 0.3999373, -446.9579, 270);
	}
}
