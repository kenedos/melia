//--- Melia Script ----------------------------------------------------------
// Phamer Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Phamer Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken432NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(29, 40120, "Statue of Goddess Vakarine", "f_bracken_43_2", -745.9572, 83.88464, -153.7229, 74, "WARP_F_BRACKEN_43_2", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_bracken_43_2", -1720.48, 175.32, -48.3, 90, "TREASUREBOX_LV_F_BRACKEN_43_21000", "", "");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153179, "", "f_bracken_43_2", 1369.781, 116.1511, 470.741, 0, "f_bracken_43_2_elt", 2, 1);
		AddTrackNPC(153180, "", "f_bracken_43_2", 552.7917, 35.09259, -574.3962, 354, "f_bracken_43_2_elt2", 2, 1);
	}
}
