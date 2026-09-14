//--- Melia Script ----------------------------------------------------------
// Kalejimas Visiting Room
//--- Description -----------------------------------------------------------
// NPCs found in and around Kalejimas Visiting Room.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison78NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(6, "WARP_D_PRISON_78", "d_prison_78", 1242.155, 742.4698, -770.2209, 0);
	}
}
