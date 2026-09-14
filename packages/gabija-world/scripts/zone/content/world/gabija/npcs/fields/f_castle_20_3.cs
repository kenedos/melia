//--- Melia Script ----------------------------------------------------------
// Inner Wall District 8
//--- Description -----------------------------------------------------------
// NPCs found in and around Inner Wall District 8.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle203NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(31, 40120, "Statue of Goddess Vakarine", "f_castle_20_3", 228.12, 143.62, -694.9, 90, "WARP_CASTLE_20_3", "STOUP_CAMP", "STOUP_CAMP");

	}
}
