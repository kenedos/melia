//--- Melia Script ----------------------------------------------------------
// Stele Road
//--- Description -----------------------------------------------------------
// NPCs found in and around Stele Road.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRemains37NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(53, 40120, "Statue of Goddess Vakarine", "f_remains_37", 433.36, 1011.26, -1704, 84, "WARP_F_REMAINS_37", "STOUP_CAMP", "STOUP_CAMP");
	}
}
