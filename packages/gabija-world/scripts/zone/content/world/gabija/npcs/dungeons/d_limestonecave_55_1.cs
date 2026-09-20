//--- Melia Script ----------------------------------------------------------
// Alembique Cave
//--- Description -----------------------------------------------------------
// NPCs found in and around Alembique Cave.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DLimestonecave551NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Kedoran Alliance Merchant Alta
		//-------------------------------------------------------------------------
		AddNpc(7, 20158, "Kedoran Alliance Merchant Alta", "d_limestonecave_55_1", -2261.05, 136.62, 205.7, 90, "LSCAVE551_ALTAR_NPC", "LSCAVE551_ALTAR_NPC_1", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_limestonecave_55_1", 301.78, 1.4, 1501.42, 90, "TREASUREBOX_LV_D_LIMESTONECAVE_55_11000", "", "");
	}
}
