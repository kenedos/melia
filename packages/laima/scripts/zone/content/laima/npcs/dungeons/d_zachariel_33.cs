//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel33NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(3016, 40120, "Statue of Goddess Vakarine", "d_zachariel_33", 1503.584, 671.4375, -592.7075, 270, "WARP_D_ZACHARIEL_33", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(3024, 147392, "Lv1 Treasure Chest", "d_zachariel_33", -1956.07, 721.25, 242.44, 0, "TREASUREBOX_LV_D_ZACHARIEL_333024", "", "");
	}
}
