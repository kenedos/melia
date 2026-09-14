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
		AddNpc(7, 40120, "Statue of Goddess Vakarine", "f_pilgrimroad_55", 1055.57, 242.4188, -424.0734, 0, "WARP_F_PILGRIMROAD_55", "STOUP_CAMP", "STOUP_CAMP");
	}
}
