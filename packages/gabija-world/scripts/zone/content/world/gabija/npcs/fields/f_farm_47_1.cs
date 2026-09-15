//--- Melia Script ----------------------------------------------------------
// Tenants' Farm
//--- Description -----------------------------------------------------------
// NPCs found in and around Tenants' Farm.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm471NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(4, "WARP_F_FARM_47_1", "f_farm_47_1", -1250.313, -41.2164, -270.3558, 90);
	}
}
