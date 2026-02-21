//--- Melia Script ----------------------------------------------------------
// Tevhrin Stalactite Cave Section 4
//--- Description -----------------------------------------------------------
// NPCs found in and around Tevhrin Stalactite Cave Section 4.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DLimestonecave524NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_limestonecave_52_4", 1572.35, -102.42, 127.14, 90, "TREASUREBOX_LV_D_LIMESTONECAVE_52_41000", "", "");
	}
}
