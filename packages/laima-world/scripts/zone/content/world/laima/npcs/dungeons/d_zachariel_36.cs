//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 5F
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum 5F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel36NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(3008, "WARP_D_ZACHARIEL_36", "d_zachariel_36", -2509.775, 329.4753, -5506.343, 25);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(3025, 147392, "Lv1 Treasure Chest", "d_zachariel_36", -2772.17, 324.06, -4747.02, 0, "TREASUREBOX_LV_D_ZACHARIEL_363025", "", "");
	}
}
