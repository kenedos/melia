//--- Melia Script ----------------------------------------------------------
// Fedimian Suburbs
//--- Description -----------------------------------------------------------
// NPCs found in and around Fedimian Suburbs.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains40NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(427, 147392, "Lv1 Treasure Chest", "f_remains_40", -1601.49, 315.44, -3954.92, 90, "TREASUREBOX_LV_F_REMAINS_40427", "", "");
	}
}
