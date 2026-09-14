//--- Melia Script ----------------------------------------------------------
// East Siauliai Woods
//--- Description -----------------------------------------------------------
// NPCs found in and around East Siauliai Woods.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai2NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(33, 40120, "Statue of Goddess Vakarine", "f_siauliai_2", 233, 157, 724, 0, "WARP_F_SIAULIAI_EST", "STOUP_CAMP", "STOUP_CAMP");
	}
}
