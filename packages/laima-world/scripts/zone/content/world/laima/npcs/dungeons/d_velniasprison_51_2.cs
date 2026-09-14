//--- Melia Script ----------------------------------------------------------
// Demon Prison District 2
//--- Description -----------------------------------------------------------
// NPCs found in and around Demon Prison District 2.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison512NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(22, "WARP_D_VPRISON_51_2", "d_velniasprison_51_2", 1041.867, 296.5582, 1709.221, 60);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(25, 147392, "Lv1 Treasure Chest", "d_velniasprison_51_2", -972.24, 395.65, -460.24, 0, "TREASUREBOX_LV_D_VELNIASPRISON_51_225", "", "");
	}
}
