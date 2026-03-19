//--- Melia Script ----------------------------------------------------------
// Demon Prison District 5
//--- Description -----------------------------------------------------------
// NPCs found in and around Demon Prison District 5.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison515NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(23, 147392, "Lv1 Treasure Chest", "d_velniasprison_51_5", -1598.03, 156.67, -116.49, 0, "TREASUREBOX_LV_D_VELNIASPRISON_51_523", "", "");
	}
}
