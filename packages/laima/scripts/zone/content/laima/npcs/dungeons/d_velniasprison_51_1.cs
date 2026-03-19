//--- Melia Script ----------------------------------------------------------
// Demon Prison District 1
//--- Description -----------------------------------------------------------
// NPCs found in and around Demon Prison District 1.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison511NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(33, 147392, "Lv1 Treasure Chest", "d_velniasprison_51_1", 659.86, 353.03, 1632.73, 0, "TREASUREBOX_LV_D_VELNIASPRISON_51_133", "", "");
	}
}
