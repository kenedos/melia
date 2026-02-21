//--- Melia Script ----------------------------------------------------------
// Astral Tower 12F
//--- Description -----------------------------------------------------------
// NPCs found in and around Astral Tower 12F.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DStartower90NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Laima
		//-------------------------------------------------------------------------
		AddNpc(22, 40100, "Statue of Goddess Laima", "d_startower_90", -1261, 67, 158, 90, "D_STARTOWER_90_STATUE", "", "");
	}
}
