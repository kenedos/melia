//--- Melia Script ----------------------------------------------------------
// Mage Tower 5F
//--- Description -----------------------------------------------------------
// NPCs found in and around Mage Tower 5F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower45NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(210, "WARP_D_FIRETOWER_45", "d_firetower_45", -1689.079, 420.4852, -643.7197, 90);
	}
}
