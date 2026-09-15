//--- Melia Script ----------------------------------------------------------
// Martuis Storage Room
//--- Description -----------------------------------------------------------
// NPCs found in and around Martuis Storage Room.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower692NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(2, "WARP_D_FIRETOWER_69_2", "d_firetower_69_2", 931.0221, 119.6176, -2366.862, 90);

	}
}
