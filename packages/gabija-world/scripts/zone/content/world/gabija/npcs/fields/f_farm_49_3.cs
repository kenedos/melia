//--- Melia Script ----------------------------------------------------------
// Shaton Reservoir
//--- Description -----------------------------------------------------------
// NPCs found in and around Shaton Reservoir.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm493NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(7, "WARP_F_FARM_49_3", "f_farm_49_3", 941.8351, 293.2046, 12.10072, 0);
	}
}
