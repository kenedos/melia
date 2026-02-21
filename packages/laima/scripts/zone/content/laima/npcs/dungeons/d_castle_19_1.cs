//--- Melia Script ----------------------------------------------------------
// Sajunga Road
//--- Description -----------------------------------------------------------
// NPCs found in and around Sajunga Road.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DCastle191NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(13, 40120, "Statue of Goddess Vakarine", "d_castle_19_1", 197.3949, 142.2863, 909.6616, 7, "WARP_D_CASTLE_19_1", "STOUP_CAMP", "STOUP_CAMP");
	}
}
