//--- Melia Script ----------------------------------------------------------
// Tiltas Valley
//--- Description -----------------------------------------------------------
// NPCs found in and around Tiltas Valley.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas28NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1040, 147392, "Lv1 Treasure Chest", "f_rokas_28", 630.55, 1156.98, -159.62, 270, "TREASUREBOX_LV_F_ROKAS_281040", "", "");

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_rokas_28", 315, 1389, 765, 0, "yellow");
		AddPlatformNpc("f_rokas_28", 365, 1429, 765, 0, "green");
		AddPlatformNpc("f_rokas_28", 415, 1469, 765, 0, "blue");
		AddMovingPlatformNpc("f_rokas_28",
			new Position(465, 1519, 765),
			new Position(465, 1519, 608),
			TimeSpan.FromSeconds(1), direction: 0, color: "red");
		AddPlatformNpc("f_rokas_28", 415, 1559, 608, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_rokas_28.Chest1", "f_rokas_28", 415, 1559, 608, 0, ItemId.EmoticonItem_109_110, monsterId: 147393);
	}
}
