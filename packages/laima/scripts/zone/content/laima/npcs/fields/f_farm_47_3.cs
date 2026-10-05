//--- Melia Script ----------------------------------------------------------
// Myrkiti Farm
//--- Description -----------------------------------------------------------
// NPCs found in and around Myrkiti Farm.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm473NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv3 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(39, 147393, "Lv3 Treasure Chest", "f_farm_47_3", -1311.72, -9.32, 647.56, 90, "TREASUREBOX_LV_F_FARM_47_339", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(700, 147392, "Lv1 Treasure Chest", "f_farm_47_3", -110.67, 90.19, -508.38, 45, "TREASUREBOX_LV_F_FARM_47_3700", "", "");
	}
}
