//--- Melia Script ----------------------------------------------------------
// Delmore Hamlet
//--- Description -----------------------------------------------------------
// NPCs found in and around Delmore Hamlet.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle651NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(29, 40120, "Statue of Goddess Vakarine", "f_castle_65_1", 1083.615, -8.393933, -1060.086, 90, "WARP_CASTLE_65_1", "STOUP_CAMP", "STOUP_CAMP");

	}
}
