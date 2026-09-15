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
		AddWarpStatue(50, "WARP_F_GELE_57_4", "f_gele_57_4", -755, -80, 491, 35);
	}
}
