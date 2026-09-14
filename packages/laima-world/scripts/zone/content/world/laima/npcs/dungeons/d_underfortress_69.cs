//--- Melia Script ----------------------------------------------------------
// Fortress Battlegrounds
//--- Description -----------------------------------------------------------
// NPCs found in and around Fortress Battlegrounds.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress69NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(42, "WARP_D_UNDERFORTRESS_69", "d_underfortress_69", 1734.104, 491.8457, 498.2167, 45);
	}
}
