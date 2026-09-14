//--- Melia Script ----------------------------------------------------------
// Rasvoy Lake
//--- Description -----------------------------------------------------------
// NPCs found in and around Rasvoy Lake.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad413NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(5, "WARP_PILGRIMROAD_41_3", "f_pilgrimroad_41_3", -899.8269, 62.01554, 515.5572, 45);
		
	}
}
