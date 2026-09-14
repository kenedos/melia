//--- Melia Script ----------------------------------------------------------
// Tenet Church B1
//--- Description -----------------------------------------------------------
// NPCs found in and around Tenet Church B1.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DChapel575NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(24, 40120, "Statue of Goddess Vakarine", "d_chapel_57_5", -1429.68, 0.55, 1033.58, 76, "WARP_D_CHAPEL_57_5", "STOUP_CAMP", "STOUP_CAMP");
	}
}
