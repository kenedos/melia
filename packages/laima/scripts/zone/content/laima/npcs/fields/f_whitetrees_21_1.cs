//--- Melia Script ----------------------------------------------------------
// Yudejan Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Yudejan Forest.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees211NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_whitetrees_21_1", -178.82, 133.09, 762.39, 90, "TREASUREBOX_LV_F_WHITETREES_21_11000", "", "");
	}
}
