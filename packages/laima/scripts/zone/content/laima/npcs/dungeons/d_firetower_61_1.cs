//--- Melia Script ----------------------------------------------------------
// Roxona Reconstruction Agency West Building
//--- Description -----------------------------------------------------------
// NPCs found in and around Roxona Reconstruction Agency West Building.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower611NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(30, 40120, "Statue of Goddess Vakarine", "d_firetower_61_1", -185.7816, 310.271, 4.435118, 90, "WARP_D_FIRETOWER_61_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_firetower_61_1", 236.46, 146.86, -1513.29, 135, "TREASUREBOX_LV_D_FIRETOWER_61_11000", "", "");
	}
}
