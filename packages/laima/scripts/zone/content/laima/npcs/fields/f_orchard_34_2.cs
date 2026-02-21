//--- Melia Script ----------------------------------------------------------
// Zeraha
//--- Description -----------------------------------------------------------
// NPCs found in and around Zeraha.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard342NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(155059, "", "f_orchard_34_2", -1761.933, -357.2179, 384.1256, 90, "f_orchard_34_2_elevator", 3);

		// Track Starting NPCs
		//---------------------------------------------------------------------------
		AddTrackStartingNPC(147501, "", "f_orchard_34_2", -1702.2494, 67.79986, 134, "f_orchard_34_2_slider", -1682.90f, 346.20f, 56.95f);
		AddTrackStartingNPC(147501, "", "f_orchard_34_2", 1906.933, -451.2874, 0, "f_orchard_34_2_Flume", 1933.90f, 399.69f, -455.72f);


		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(314, 147392, "Lv1 Treasure Chest", "f_orchard_34_2", -2120, 1429, -103, 0, "TREASUREBOX_LV_F_ORCHARD_34_2314", "", "");
	}
}
