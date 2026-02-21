//--- Melia Script ----------------------------------------------------------
// Pradzia Temple
//--- Description -----------------------------------------------------------
// NPCs found in and around Pradzia Temple.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DDcapital108NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(78, 40110, "Statue of Goddess Zemyna", "d_dcapital_108", -1606.255, 27.08374, -2953.66, -20, "D_DCAPITAL_108_ZEMINA", "D_DCAPITAL_108_ZEMINA", "D_DCAPITAL_108_ZEMINA");
	}
}
