//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 4F
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum 4F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel35NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(3026, 147392, "Lv1 Treasure Chest", "d_zachariel_35", -297, -53.77, 1524, 270, "TREASUREBOX_LV_D_ZACHARIEL_353026", "", "");
	}
}
