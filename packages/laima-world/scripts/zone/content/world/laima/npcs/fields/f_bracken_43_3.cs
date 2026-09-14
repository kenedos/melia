//--- Melia Script ----------------------------------------------------------
// Ziburynas Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Ziburynas Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken433NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_bracken_43_3", 2234.82, 316.85, -577.87, 90, "TREASUREBOX_LV_F_BRACKEN_43_31000", "", "");
	}
}
