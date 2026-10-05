//--- Melia Script ----------------------------------------------------------
// Tevhrin Stalactite Cave Section 2
//--- Description -----------------------------------------------------------
// NPCs found in and around Tevhrin Stalactite Cave Section 2.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DLimestonecave522NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "d_limestonecave_52_2", -1444.84, -961.75, -1527.41, 90, "TREASUREBOX_LV_D_LIMESTONECAVE_52_21000", "", "");
	}
}
