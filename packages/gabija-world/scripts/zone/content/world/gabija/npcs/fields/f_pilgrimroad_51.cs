//--- Melia Script ----------------------------------------------------------
// Forest of Prayer
//--- Description -----------------------------------------------------------
// NPCs found in and around Forest of Prayer.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad51NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(7, 40120, "Statue of Goddess Vakarine", "f_pilgrimroad_51", 746.57, 571.14, 1650.72, 99, "WARP_F_PILGRIMROAD_51", "STOUP_CAMP", "STOUP_CAMP");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(21, 40120, "Statue of Goddess Vakarine", "f_pilgrimroad_51", -974.02, 548.29, -937.99, 3, "PILGRIM51_FGOD01", "", "");
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(22, 40120, "Statue of Goddess Vakarine", "f_pilgrimroad_51", 1544.68, 143.8, -1701.02, 96, "PILGRIM51_FGOD02", "", "");
	}
}
