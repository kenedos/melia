//--- Melia Script ----------------------------------------------------------
// Tenet Garden
//--- Description -----------------------------------------------------------
// NPCs found in and around Tenet Garden.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele574NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(50, 40120, "Statue of Goddess Vakarine", "f_gele_57_4", -755, -80, 491, 35, "WARP_F_GELE_57_4", "STOUP_CAMP", "STOUP_CAMP");
	}
}
