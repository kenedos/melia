//--- Melia Script ----------------------------------------------------------
// Sekta Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Sekta Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad414NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Lv3 Treasure Chest (Lepusbunny Headband)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147393, "Lv3 Treasure Chest", "f_pilgrimroad_41_4", 112, -105, -864, 315, "TREASUREBOX_LV_F_PILGRIMROAD_41_49001", "", "");
	}
}
