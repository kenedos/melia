//--- Melia Script ----------------------------------------------------------
// Gele Plateau
//--- Description -----------------------------------------------------------
// NPCs found in and around Gele Plateau.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele572NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv3 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(150, 147393, "Lv3 Treasure Chest", "f_gele_57_2", -659, 418.99, -72.39, 90, "TREASUREBOX_LV_F_GELE_57_2150", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(153, 147392, "Lv1 Treasure Chest", "f_gele_57_2", -1049.22, 418.99, -641.81, 90, "TREASUREBOX_LV_F_GELE_57_2153", "", "");

		// Lv1 Treasure Chest (Carnivore Necklace)
		//-------------------------------------------------------------------------
		AddNpc(9001, 147392, "Lv1 Treasure Chest", "f_gele_57_2", -813, 419, -548, 225, "TREASUREBOX_LV_F_GELE_57_29001", "", "");
	}
}
