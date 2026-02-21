//--- Melia Script ----------------------------------------------------------
// Barynwell 86 Waters
//--- Description -----------------------------------------------------------
// NPCs found in and around Barynwell 86 Waters.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class F3Cmlake86NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(74, 40110, "Statue of Goddess Zemyna", "f_3cmlake_86", -403.3236, 39.31152, 1682.504, 90, "F_3CMLAKE_86_EV_55_001", "F_3CMLAKE_86_EV_55_001", "F_3CMLAKE_86_EV_55_001");
	}
}
