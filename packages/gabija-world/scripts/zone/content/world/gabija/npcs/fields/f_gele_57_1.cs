//--- Melia Script ----------------------------------------------------------
// Srautas Gorge
//--- Description -----------------------------------------------------------
// NPCs found in and around Srautas Gorge.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele571NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(133, 40120, "Statue of Goddess Vakarine", "f_gele_57_1", -132.3, 168.82, -571.54, -9, "WARP_F_GELE_57_1", "STOUP_CAMP", "STOUP_CAMP");

	}
}
