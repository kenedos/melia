//--- Melia Script ----------------------------------------------------------
// Woods of the Linked Bridges
//--- Description -----------------------------------------------------------
// NPCs found in and around Woods of the Linked Bridges.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep13FSiauliai2NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(33, 40120, "Statue of Goddess Vakarine", "ep13_f_siauliai_2", 2428.743, 139.0712, -912.3688, 45, "WARP_EP13_F_SIAULIAI_2", "STOUP_CAMP", "STOUP_CAMP");
	}
}
