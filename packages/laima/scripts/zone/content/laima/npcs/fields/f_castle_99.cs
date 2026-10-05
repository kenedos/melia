//--- Melia Script ----------------------------------------------------------
// Tvirtumas Fortress
//--- Description -----------------------------------------------------------
// NPCs found in and around Tvirtumas Fortress.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FCastle99NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(8, 40120, "Statue of Goddess Vakarine", "f_castle_99", 1782.298, -43.84729, -1170.19, 90, "WARP_F_CASTLE_99", "STOUP_CAMP", "STOUP_CAMP");
	}
}
