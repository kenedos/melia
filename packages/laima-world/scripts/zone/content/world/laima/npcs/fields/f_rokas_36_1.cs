//--- Melia Script ----------------------------------------------------------
// Galeed Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Galeed Plateau.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas361NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_rokas_36_1", 228.39, 0.81, 533.28, 135, "TREASUREBOX_LV_F_ROKAS_36_11000", "", "");
	}
}
