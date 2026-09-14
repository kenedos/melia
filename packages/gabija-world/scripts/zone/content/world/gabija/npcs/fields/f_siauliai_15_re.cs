//--- Melia Script ----------------------------------------------------------
// Woods of the Linked Bridges
//--- Description -----------------------------------------------------------
// NPCs found in and around Woods of the Linked Bridges.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai15ReNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(10, 40120, "Statue of Goddess Vakarine", "f_siauliai_15_re", 372.0667, 878.1719, 194.3833, -4, "WARP_F_SIAULIAI_15RE", "STOUP_CAMP", "STOUP_CAMP");
	}
}
