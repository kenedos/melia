//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum Constructors' Chapel
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum Constructors' Chapel.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress592NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue(28, "UNDERF592_ZEMINA_STATUE", "d_underfortress_59_2", -951.0473, 0.377, 696.8472, 0);
		
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(37, "WARP_D_UNDERFORTRESS_59_2", "d_underfortress_59_2", 449.585, 0.377, -208.2484, -30);
	}
}
