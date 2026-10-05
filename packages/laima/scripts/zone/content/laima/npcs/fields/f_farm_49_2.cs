//--- Melia Script ----------------------------------------------------------
// Shaton Farm
//--- Description -----------------------------------------------------------
// NPCs found in and around Shaton Farm.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm492NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(51, 147392, "Lv1 Treasure Chest", "f_farm_49_2", 72, 53, -111, 270, "TREASUREBOX_LV_F_FARM_49_251", "", "");
	}
}
