//--- Melia Script ----------------------------------------------------------
// Mochia Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Mochia Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad313NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(21, "WARP_F_PILGRIMROAD_31_3", "f_pilgrimroad_31_3", -1138.227, 125.4098, -1534.654, 0);
	}
}
