//--- Melia Script ----------------------------------------------------------
// Tenants' Farm
//--- Description -----------------------------------------------------------
// NPCs found in and around Tenants' Farm.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm471NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(4, 40120, "Statue of Goddess Vakarine", "f_farm_47_1", -1250.313, -41.2164, -270.3558, 90, "WARP_F_FARM_47_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(64, 147392, "Lv1 Treasure Chest", "f_farm_47_1", -1173.09, 40.77, -1323.89, 90, "TREASUREBOX_LV_F_FARM_47_164", "", "");
	}
}
