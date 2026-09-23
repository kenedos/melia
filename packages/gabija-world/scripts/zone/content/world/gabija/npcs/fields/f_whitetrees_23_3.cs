//--- Melia Script ----------------------------------------------------------
// Syla Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Syla Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees233NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_whitetrees_23_3", 387.29, 219.3, 1259.58, 90, "TREASUREBOX_LV_F_WHITETREES_23_31000", "", "");
	}
}
