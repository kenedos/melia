//--- Melia Script ----------------------------------------------------------
// Seir Rainforest
//--- Description -----------------------------------------------------------
// NPCs found in and around Seir Rainforest.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard324NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(319, 147392, "Lv1 Treasure Chest", "f_orchard_32_4", -1802, 679, 534, 45, "TREASUREBOX_LV_F_ORCHARD_32_4319", "", "");
	}
}
