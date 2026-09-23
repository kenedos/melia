//--- Melia Script ----------------------------------------------------------
// Koru Jungle
//--- Description -----------------------------------------------------------
// NPCs found in and around Koru Jungle.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken631NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(503, 147392, "Lv1 Treasure Chest", "f_bracken_63_1", 1658, 0, -866, 0, "TREASUREBOX_LV_F_BRACKEN_63_1503", "", "");
	}
}
