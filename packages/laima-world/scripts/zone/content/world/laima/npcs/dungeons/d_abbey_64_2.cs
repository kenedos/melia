//--- Melia Script ----------------------------------------------------------
// Novaha Annex
//--- Description -----------------------------------------------------------
// NPCs found in and around Novaha Annex.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey642NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(660, 147392, "Lv1 Treasure Chest", "d_abbey_64_2", 97, 981, -1895, -45, "TREASUREBOX_LV_D_ABBEY_64_2660", "", "");
	}
}
