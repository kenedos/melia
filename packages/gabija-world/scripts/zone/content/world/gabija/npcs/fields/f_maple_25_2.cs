//--- Melia Script ----------------------------------------------------------
// Svalphinghas Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Svalphinghas Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FMaple252NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(22, "WARP_F_MAPLE_25_2", "f_maple_25_2", 1112.25, 641.79, 806.54, 90);

	}
}
