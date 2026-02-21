//--- Melia Script ----------------------------------------------------------
// Istora Ruins
//--- Description -----------------------------------------------------------
// NPCs found in and around Istora Ruins.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains373NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(33, 40120, "Statue of Goddess Vakarine", "f_remains_37_3", 455.1151, 80.3744, -1175.304, 0, "WARP_F_REMAINS_37_3", "STOUP_CAMP", "STOUP_CAMP");
	}
}
