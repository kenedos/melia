//--- Melia Script ----------------------------------------------------------
// Nobreer Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Nobreer Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees212NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(8, "WARP_WHITETREES_21_2", "f_whitetrees_21_2", 793.81, -52.46, 118.36, 0);
	}
}
