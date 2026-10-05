//--- Melia Script ----------------------------------------------------------
// Gateway of the Great King
//--- Description -----------------------------------------------------------
// NPCs found in and around Gateway of the Great King.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas24NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(3, 40120, "Statue of Goddess Vakarine", "f_rokas_24", 913, 123, -1882, 0, "WARP_F_ROKAS_24", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(727, 147392, "Lv1 Treasure Chest", "f_rokas_24", -677.85, 724.39, -2528, 90, "TREASUREBOX_LV_F_ROKAS_24727", "", "");

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_rokas_24", -1872, 935, -893, 0, "yellow");
		AddPlatformNpc("f_rokas_24", -1872, 975, -893, 0, "blue");
		AddPlatformNpc("f_rokas_24", -1872, 1005, -893, 0, "red");
		AddMovingPlatformNpc("f_rokas_24",
			new Position(-1832, 1045, -893),
			new Position(-1832, 1045, -1064),
			TimeSpan.FromSeconds(1), direction: 0, color: "green");
		AddMovingPlatformNpc("f_rokas_24",
			new Position(-1792, 1085, -893),
			new Position(-1792, 1085, -949),
			TimeSpan.FromSeconds(1), direction: 0, color: "yellow");
		AddPlatformNpc("f_rokas_24", -1709, 1115, -940, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_rokas_24.Chest1", "f_rokas_24", -1709, 1115, -940, 0, ItemId.Event_Gosu_Emoticon_Box_2, monsterId: 147393);
	}
}
