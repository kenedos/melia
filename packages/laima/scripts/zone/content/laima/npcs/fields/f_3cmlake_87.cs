//--- Melia Script ----------------------------------------------------------
// Barynwell 87 Waters
//--- Description -----------------------------------------------------------
// NPCs found in and around Barynwell 87 Waters.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class F3Cmlake87NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(55, 40120, "Statue of Goddess Vakarine", "f_3cmlake_87", -48.28166, 168.3374, -407.464, 0, "WARP_F_3CMLAKE_87", "STOUP_CAMP", "STOUP_CAMP");
	}
}
