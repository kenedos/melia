//--- Melia Script ----------------------------------------------------------
// Tenet Church 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Tenet Church 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DChapel577NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(60, 147392, "Lv1 Treasure Chest", "d_chapel_57_7", -738.51, 36.02, 125.63, 90, "TREASUREBOX_LV_D_CHAPEL_57_760", "", "");
	}
}
