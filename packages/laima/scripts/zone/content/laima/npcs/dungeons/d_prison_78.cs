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
		AddNpc(6, 40120, "Statue of Goddess Vakarine", "d_prison_78", 1242.155, 742.4698, -770.2209, 0, "WARP_D_PRISON_78", "STOUP_CAMP", "STOUP_CAMP");
	}
}
