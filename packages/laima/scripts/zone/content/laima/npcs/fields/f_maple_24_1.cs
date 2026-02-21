//--- Melia Script ----------------------------------------------------------
// Central Parias Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Central Parias Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FMaple241NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(8, 40120, "Statue of Goddess Vakarine", "f_maple_24_1", 1440.543, 1.0349, 1481.02, 25, "WARP_F_MAPLE_24_1", "STOUP_CAMP", "STOUP_CAMP");
	}
}
