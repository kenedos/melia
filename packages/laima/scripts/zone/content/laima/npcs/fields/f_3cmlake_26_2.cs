//--- Melia Script ----------------------------------------------------------
// Lanko 22 Waters
//--- Description -----------------------------------------------------------
// NPCs found in and around Lanko 22 Waters.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class F3Cmlake262NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(77, 40110, "Statue of Goddess Zemyna", "f_3cmlake_26_2", 1497.572, 2.64352, -99.23495, 0, "F_3CMLAKE_26_2_ZEMINA", "F_3CMLAKE_26_2_ZEMINA", "F_3CMLAKE_26_2_ZEMINA");
	}
}
