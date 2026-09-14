//--- Melia Script ----------------------------------------------------------
// Tyla Monastery
//--- Description -----------------------------------------------------------
// NPCs found in and around Tyla Monastery.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey394NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_abbey_39_4", 594.13, 144.95, 1207.26, 225, "TREASUREBOX_LV_D_ABBEY_39_41000", "", "");
	}
}
