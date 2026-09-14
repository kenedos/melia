//--- Melia Script ----------------------------------------------------------
// Elgos Abbey Main Building
//--- Description -----------------------------------------------------------
// NPCs found in and around Elgos Abbey Main Building.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey354NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(20, 40120, "Statue of Goddess Vakarine", "d_abbey_35_4", 1284.374, 0.3437, -1020.557, 77, "WARP_D_ABBEY_35_4", "STOUP_CAMP", "STOUP_CAMP");
	}
}
