//--- Melia Script ----------------------------------------------------------
// Seir Rainforest
//--- Description -----------------------------------------------------------
// NPCs found in and around Seir Rainforest.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FOrchard324NpcScript : GeneralScript
{
	protected override void Load()
	{

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(156037, "", "f_orchard_32_4", 1402.254, 135.6041, 557.9661, 0, "f_orchard_32_4_wheel", 2, 1);
		AddTrackNPC(156038, "", "f_orchard_32_4", -333.8946, 19.81506, 796.1085, 90, "f_orchard_32_4_elt", 2, 1);

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_orchard_32_4", 1053, 556, 802, 0, "yellow");
		AddPlatformNpc("f_orchard_32_4", 1053, 596, 802, 0, "blue");
		AddPlatformNpc("f_orchard_32_4", 1053, 636, 802, 0, "red");
		AddPlatformNpc("f_orchard_32_4", 1053, 676, 802, 0, "green");
		AddPlatformNpc("f_orchard_32_4", 1053, 716, 802, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_orchard_32_4.Chest1", "f_orchard_32_4", 1053, 716, 802, 0, ItemId.EmoticonItem_2404_Popo, monsterId: 147393);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(319, 147392, "Lv1 Treasure Chest", "f_orchard_32_4", -1802, 679, 534, 45, "TREASUREBOX_LV_F_ORCHARD_32_4319", "", "");
	}
}
