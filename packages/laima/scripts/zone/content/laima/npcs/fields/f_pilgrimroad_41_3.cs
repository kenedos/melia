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
		AddNpc(5, 40120, "Statue of Goddess Vakarine", "f_pilgrimroad_41_3", -899.8269, 62.01554, 515.5572, 45, "WARP_PILGRIMROAD_41_3", "STOUP_CAMP", "STOUP_CAMP");
		
	}
}
