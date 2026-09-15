//--- Melia Script ----------------------------------------------------------
// Penitence Route of Great Cathedral
//--- Description -----------------------------------------------------------
// NPCs found in and around Penitence Route of Great Cathedral.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad55NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(7, "WARP_F_PILGRIMROAD_55", "f_pilgrimroad_55", 1055.57, 242.4188, -424.0734, 0);
	}
}
