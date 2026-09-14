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
		AddNpc(664, 40120, "Statue of Goddess Vakarine", "d_thorn_19", -206.46, 622.52, -3759.09, 35, "WARP_D_THORN_19", "STOUP_CAMP", "STOUP_CAMP");
		
		// Track NPCs
		//---------------------------------------------------------------------------

	}
}
