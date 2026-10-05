//--- Melia Script ----------------------------------------------------------
// Fasika Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Fasika Plateau.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FPilgrimroad362NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_pilgrimroad_36_2", -297.14, 225.48, -545.6, 135, "TREASUREBOX_LV_F_PILGRIMROAD_36_21000", "", "");
	}
}
