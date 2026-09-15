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
		AddWarpStatue(7, "WARP_F_KATYN_45_1", "f_katyn_45_1", -2121.582, 128.0495, -254.6491, 0);
	}
}
