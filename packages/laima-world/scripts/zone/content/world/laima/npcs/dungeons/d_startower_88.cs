//--- Melia Script ----------------------------------------------------------
// Astral Tower 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Astral Tower 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DStartower88NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(34, "WARP_D_STARTOWER_88", "d_startower_88", 2352.313, 212.6299, 1488.612, 45);
	}
}
