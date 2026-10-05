//--- Melia Script ----------------------------------------------------------
// Issaugoti Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Issaugoti Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class Ep13FSiauliai4NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(12, 40120, "Statue of Goddess Vakarine", "ep13_f_siauliai_4", 22.51642, 146.2609, -946.3156, 90, "WARP_EP13_F_SIAULIAI_4", "STOUP_CAMP", "STOUP_CAMP");
		
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(13, 40110, "Statue of Goddess Zemyna", "ep13_f_siauliai_4", 1142.392, 79.90748, 609.9449, 17, "EP13_F_SIAULIAI_4_ZEMINA", "EP13_F_SIAULIAI_4_ZEMINA", "EP13_F_SIAULIAI_4_ZEMINA");
	}
}
