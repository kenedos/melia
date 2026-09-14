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
	}
}
