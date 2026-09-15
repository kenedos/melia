//--- Melia Script ----------------------------------------------------------
// Gate Route
//--- Description -----------------------------------------------------------
// NPCs found in and around Gate Route.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn19NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(664, "WARP_D_THORN_19", "d_thorn_19", -206.46, 622.52, -3759.09, 35);
		
		// Track NPCs
		//---------------------------------------------------------------------------

	}
}
