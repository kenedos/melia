//--- Melia Script ----------------------------------------------------------
// Grynas Trails
//--- Description -----------------------------------------------------------
// NPCs found in and around Grynas Trails.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn451NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(7, 40120, "Statue of Goddess Vakarine", "f_katyn_45_1", -2121.582, 128.0495, -254.6491, 0, "WARP_F_KATYN_45_1", "STOUP_CAMP", "STOUP_CAMP");
	}
}
