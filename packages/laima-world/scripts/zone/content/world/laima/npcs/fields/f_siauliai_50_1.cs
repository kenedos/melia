//--- Melia Script ----------------------------------------------------------
// Gytis Settlement Area
//--- Description -----------------------------------------------------------
// NPCs found in and around Gytis Settlement Area.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai501NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(90, 147392, "Lv1 Treasure Chest", "f_siauliai_50_1", -222.81, 0.31, -652.48, 90, "TREASUREBOX_LV_F_SIAULIAI_50_190", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(94, 147392, "Lv1 Treasure Chest", "f_siauliai_50_1", -1142.29, 0.31, -1366.83, 45, "TREASUREBOX_LV_F_SIAULIAI_50_194", "", "");
	}
}
