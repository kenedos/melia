//--- Melia Script ----------------------------------------------------------
// Greene Manor
//--- Description -----------------------------------------------------------
// NPCs found in and around Greene Manor.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm491NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(40, "WARP_F_FARM_49_1", "f_farm_49_1", -1180, 0, 1031, 405);
	}
}
