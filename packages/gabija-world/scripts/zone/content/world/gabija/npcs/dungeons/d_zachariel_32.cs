//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel32NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(3015, 40120, "Statue of Goddess Vakarine", "d_zachariel_32", -68.32312, 261.6406, -2170.473, 57, "WARP_D_ZACHARIEL_32", "STOUP_CAMP", "STOUP_CAMP");
	}
}
