//--- Melia Script ----------------------------------------------------------
// Nazarene Tower
//--- Description -----------------------------------------------------------
// NPCs found in and around Nazarene Tower.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class DStartower762NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue(32, "FD_STARTOWER762_EV_55_001", "d_startower_76_2", -852.9421, 0.6973495, 949.3286, 121);
	}
}
