//--- Melia Script ----------------------------------------------------------
// Roxona Reconstruction Agency West Building
//--- Description -----------------------------------------------------------
// NPCs found in and around Roxona Reconstruction Agency West Building.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower611NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(30, 40120, "Statue of Goddess Vakarine", "d_firetower_61_1", -185.7816, 310.271, 4.435118, 90, "WARP_D_FIRETOWER_61_1", "STOUP_CAMP", "STOUP_CAMP");
	}
}
