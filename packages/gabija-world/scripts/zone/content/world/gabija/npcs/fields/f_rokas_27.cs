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
		AddNpc(702, 40120, "Statue of Goddess Vakarine", "f_rokas_27", -319.35, 1196.1, -2928.97, 0, "WARP_F_ROKAS_27", "STOUP_CAMP", "STOUP_CAMP");

	}
}
