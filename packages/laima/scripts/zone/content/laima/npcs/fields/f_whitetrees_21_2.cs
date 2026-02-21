//--- Melia Script ----------------------------------------------------------
// Nobreer Forest
//--- Description -----------------------------------------------------------
// NPCs found in and around Nobreer Forest.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees212NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(8, 40120, "Statue of Goddess Vakarine", "f_whitetrees_21_2", 793.81, -52.46, 118.36, 0, "WARP_WHITETREES_21_2", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_whitetrees_21_2", 498.81, 114.39, 1433.28, 90, "TREASUREBOX_LV_F_WHITETREES_21_21000", "", "");

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_whitetrees_21_2", 1398, -33, 549, 0, "green");
		AddPlatformNpc("f_whitetrees_21_2", 1459, -3, 549, 0, "yellow");
		AddPlatformNpc("f_whitetrees_21_2", 1537, 27, 549, 0, "red");
		AddMovingPlatformNpc("f_whitetrees_21_2",
			new Position(1615, 62, 704),
			new Position(1615, 62, 403),
			TimeSpan.FromSeconds(1), direction: 0, color: "blue");
		AddMovingPlatformNpc("f_whitetrees_21_2",
			new Position(1676, 62, 403),
			new Position(1779, 62, 403),
			TimeSpan.FromSeconds(1), direction: 0, color: "red");
		AddMovingPlatformNpc("f_whitetrees_21_2",
			new Position(1676, 62, 704),
			new Position(1779, 62, 704),
			TimeSpan.FromSeconds(1), direction: 0, color: "red");
		AddPlatformNpc("f_whitetrees_21_2", 1779, 92, 473, 0, "yellow");
		AddPlatformNpc("f_whitetrees_21_2", 1779, 92, 634, 0, "yellow");
		AddPlatformNpc("f_whitetrees_21_2", 1779, 122, 554, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_whitetrees_21_2.Chest1", "f_whitetrees_21_2", 1779, 122, 554, 0, ItemId.EmoticonItem_111_112, monsterId: 147393);

	}
}
