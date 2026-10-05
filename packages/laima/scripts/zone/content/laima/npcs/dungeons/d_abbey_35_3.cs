//--- Melia Script ----------------------------------------------------------
// Elgos Monastery Annex
//--- Description -----------------------------------------------------------
// NPCs found in and around Elgos Monastery Annex.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey353NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_abbey_35_3", 565.98, 0.57, -317.33, 180, "TREASUREBOX_LV_D_ABBEY_35_31000", "", "");
	}
}
