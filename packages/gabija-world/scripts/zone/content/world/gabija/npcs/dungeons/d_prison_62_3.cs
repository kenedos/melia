//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 3F
//--- Description -----------------------------------------------------------
// NPCs found in and around Ashaq Underground Prison 3F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison623NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(22, 40120, "Statue of Goddess Vakarine", "d_prison_62_3", 877.5243, 997.5414, 20.58967, 7, "WARP_D_PRISON_62_3", "STOUP_CAMP", "STOUP_CAMP");

	}
}
