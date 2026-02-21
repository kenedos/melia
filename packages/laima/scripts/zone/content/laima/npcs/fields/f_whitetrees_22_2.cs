//--- Melia Script ----------------------------------------------------------
// Tekel Shelter
//--- Description -----------------------------------------------------------
// NPCs found in and around Tekel Shelter.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees222NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(48, 40110, "Statue of Goddess Zemyna", "f_whitetrees_22_2", -889.7463, 266.4498, 990.5203, 90, "WHITETREES22_2_EV_55_001", "WHITETREES22_2_EV_55_001", "WHITETREES22_2_EV_55_001");
		
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153189, "", "f_whitetrees_22_2", 990.0172, 125.708, -499.8661, 351, "f_whitetrees_22_2_elt", 2, 1);

	}
}
