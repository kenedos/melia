//--- Melia Script ----------------------------------------------------------
// Topes Fortress 2F
//--- Description -----------------------------------------------------------
// NPCs found in and around Topes Fortress 2F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCastle672NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(219, 147392, "Lv1 Treasure Chest", "d_castle_67_2", 1854.48, 165.25, -1071.52, 180, "TREASUREBOX_LV_D_CASTLE_67_2219", "", "");
	}
}
