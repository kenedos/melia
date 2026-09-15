//--- Melia Script ----------------------------------------------------------
// Pystis Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Pystis Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FMaple232NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(7, "WARP_C_MAPLE_23_2", "f_maple_23_2", 1185.827, 0, 56.20589, 5);
	}
}
