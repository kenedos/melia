//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 5F
//--- Description -----------------------------------------------------------
// NPCs found in and around Royal Mausoleum 5F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DZachariel36NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(3008, 40120, "Statue of Goddess Vakarine", "d_zachariel_36", -2509.775, 329.4753, -5506.343, 25, "WARP_D_ZACHARIEL_36", "STOUP_CAMP", "STOUP_CAMP");
	}
}
