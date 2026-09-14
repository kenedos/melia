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
		AddNpc(8, 40120, "Statue of Goddess Vakarine", "f_whitetrees_21_2", 793.81, -52.46, 118.36, 0, "WARP_WHITETREES_21_2", "STOUP_CAMP", "STOUP_CAMP");
	}
}
