//--- Melia Script ----------------------------------------------------------
// Demon Prison District 4
//--- Description -----------------------------------------------------------
// NPCs found in and around Demon Prison District 4.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison513NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(17, 147392, "Lv1 Treasure Chest", "d_velniasprison_51_3", 1736.33, 128.36, -1242.48, 0, "TREASUREBOX_LV_D_VELNIASPRISON_51_317", "", "");
	}
}
