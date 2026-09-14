//--- Melia Script ----------------------------------------------------------
// Viltis Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Viltis Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn391NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(33, 40120, "Statue of Goddess Vakarine", "d_thorn_39_1", 84.45702, 1216.375, 19.4914, 90, "WARP_D_THORN_39_1", "STOUP_CAMP", "STOUP_CAMP");
	}
}
