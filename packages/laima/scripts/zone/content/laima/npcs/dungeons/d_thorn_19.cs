//--- Melia Script ----------------------------------------------------------
// Gate Route
//--- Description -----------------------------------------------------------
// NPCs found in and around Gate Route.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn19NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(664, 40120, "Statue of Goddess Vakarine", "d_thorn_19", -208.2775, 622.5202, -3814.656, 35, "WARP_D_THORN_19", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(682, 147392, "Lv1 Treasure Chest", "d_thorn_19", 1761.78, 458.62, 1590.11, 180, "TREASUREBOX_LV_D_THORN_19682", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(687, 147392, "Lv1 Treasure Chest", "d_thorn_19", 627, 600, 1910, 0, "TREASUREBOX_LV_D_THORN_19687", "", "");


	}
}
