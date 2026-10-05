//--- Melia Script ----------------------------------------------------------
// Astral Tower 20F
//--- Description -----------------------------------------------------------
// NPCs found in and around Astral Tower 20F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DStartower91NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(46, 40120, "Statue of Goddess Vakarine", "d_startower_91", 2501, 1211, -1388, 45, "WARP_D_STARTOWER_91", "STOUP_CAMP", "STOUP_CAMP");
	}
}
