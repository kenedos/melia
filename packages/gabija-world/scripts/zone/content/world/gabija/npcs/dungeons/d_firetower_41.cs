//--- Melia Script ----------------------------------------------------------
// Mage Tower 1F
//--- Description -----------------------------------------------------------
// NPCs found in and around Mage Tower 1F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower41NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(116, 40120, "Statue of Goddess Vakarine", "d_firetower_41", 2005.266, 1446.488, -1369.808, 30, "WARP_D_FIRETOWER_41", "STOUP_CAMP", "STOUP_CAMP");
		
		// Track NPCs
		//---------------------------------------------------------------------------

	}
}
