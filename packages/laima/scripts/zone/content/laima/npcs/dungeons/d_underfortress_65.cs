//--- Melia Script ----------------------------------------------------------
// Sentry Bailey
//--- Description -----------------------------------------------------------
// NPCs found in and around Sentry Bailey.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress65NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(35, 40120, "Statue of Goddess Vakarine", "d_underfortress_65", 537.7255, 602.314, -1996.485, 45, "WARP_D_UNDERFORTRESS_65", "STOUP_CAMP", "STOUP_CAMP");
	}
}
