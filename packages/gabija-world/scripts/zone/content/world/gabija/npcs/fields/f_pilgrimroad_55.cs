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

		// Lv3 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(39, 147393, "Lv3 Treasure Chest", "f_pilgrimroad_55", -105.74, 242.52, 77.43, 90, "TREASUREBOX_LV_F_PILGRIMROAD_5539", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(900, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_55", -1008.1, 331.96, 336.24, 0, "TREASUREBOX_LV_F_PILGRIMROAD_55900", "", "");
	}
}
