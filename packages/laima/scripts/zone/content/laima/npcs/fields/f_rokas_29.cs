//--- Melia Script ----------------------------------------------------------
// Rukas Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Rukas Plateau.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas29NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(654, 147392, "Lv1 Treasure Chest", "f_rokas_29", 1114.74, 657.97, -1202.02, 90, "TREASUREBOX_LV_F_ROKAS_29654", "", "");
	}
}
