//--- Melia Script ----------------------------------------------------------
// Akmens Ridge
//--- Description -----------------------------------------------------------
// NPCs found in and around Akmens Ridge.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas27NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(702, "WARP_F_ROKAS_27", "f_rokas_27", -319.35, 1196.1, -2928.97, 0);

	}
}
