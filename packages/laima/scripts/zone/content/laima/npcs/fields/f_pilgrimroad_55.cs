//--- Melia Script ----------------------------------------------------------
// Penitence Route
//--- Description -----------------------------------------------------------
// NPCs found in and around Penitence Route.
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
		
		// Lv4 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(39, 147394, "Lv4 Treasure Chest", "f_pilgrimroad_55", -105.74, 242.52, 77.43, 90, "TREASUREBOX_LV_F_PILGRIMROAD_5539", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(900, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_55", -1007.17, 331.96, 339.83, 0, "TREASUREBOX_LV_F_PILGRIMROAD_55900", "", "");
	}
}
