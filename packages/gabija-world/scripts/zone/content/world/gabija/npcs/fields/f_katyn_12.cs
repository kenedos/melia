//--- Melia Script ----------------------------------------------------------
// Letas Stream
//--- Description -----------------------------------------------------------
// NPCs found in and around Letas Stream.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FKatyn12NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(24, "WARP_F_KATYN_12", "f_katyn_12", 29.33316, 249.4619, -758.8959, 45);
	}
}
