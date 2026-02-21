//--- Melia Script ----------------------------------------------------------
// Aqueduct Bridge Area
//--- Description -----------------------------------------------------------
// NPCs found in and around Aqueduct Bridge Area.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm472NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(55, 147392, "Lv1 Treasure Chest", "f_farm_47_2", 1364.12, -116.04, -57.45, 90, "TREASUREBOX_LV_F_FARM_47_255", "", "");

		// Lv3 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(52, 147393, "Lv3 Treasure Chest", "f_farm_47_2", 100, -62, 1941, 0, "TREASUREBOX_LV_F_PILGRIMROAD_4652", "", "");
	}
}
