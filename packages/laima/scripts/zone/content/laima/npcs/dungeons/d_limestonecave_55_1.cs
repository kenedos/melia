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
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_limestonecave_55_1", 301.78, 1.4, 1501.42, 90, "TREASUREBOX_LV_D_LIMESTONECAVE_55_11000", "", "");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(154099, "", "d_limestonecave_55_1", 1227.525, 151.018, 529.8058, 16, "d_limestonecave_55_1_elt", 2, 1);
	}
}
