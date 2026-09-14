//--- Melia Script ----------------------------------------------------------
// Paupys Crossing
//--- Description -----------------------------------------------------------
// NPCs found in and around Paupys Crossing.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai11ReNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(4, 40120, "Statue of Goddess Vakarine", "f_siauliai_11_re", 558.7085, 209.7152, 707.2426, 69, "WARP_F_SIAULIAI_11RE", "STOUP_CAMP", "STOUP_CAMP");
	}
}
