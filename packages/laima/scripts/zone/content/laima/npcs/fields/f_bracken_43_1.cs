//--- Melia Script ----------------------------------------------------------
// Arcus Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Arcus Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken431NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_bracken_43_1", -401.92, 188.43, -501.73, 90, "TREASUREBOX_LV_F_BRACKEN_43_11000", "", "");
	}
}
