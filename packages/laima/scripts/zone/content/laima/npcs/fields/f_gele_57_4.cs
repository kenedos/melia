//--- Melia Script ----------------------------------------------------------
// Tenet Garden
//--- Description -----------------------------------------------------------
// NPCs found in and around Tenet Garden.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele574NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(50, 40120, "Statue of Goddess Vakarine", "f_gele_57_4", -755, -80, 491, 35, "WARP_F_GELE_57_4", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv2 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(51, 40030, "Lv2 Treasure Chest", "f_gele_57_4", -1823, 7.31, -728, 180, "TREASUREBOX_LV_F_GELE_57_451", "", "");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(60, 147392, "Lv1 Treasure Chest", "f_gele_57_4", -1854.78, -29.81, 158.67, 0, "TREASUREBOX_LV_F_GELE_57_460", "", "");

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_gele_57_4", 2314, 144, -607, 45, "blue");
		AddPlatformNpc("f_gele_57_4", 2274, 184, -647, 45, "red");
		AddPlatformNpc("f_gele_57_4", 2234, 224, -687, 45, "green");
		AddPlatformNpc("f_gele_57_4", 2194, 264, -727, 45, "yellow");
		AddMovingPlatformNpc("f_gele_57_4",
			new Position(2154, 304, -767),
			new Position(2283, 304, -896),
			TimeSpan.FromSeconds(2), direction: 45, color: "blue");
		AddMovingPlatformNpc("f_gele_57_4",
			new Position(2114, 344, -807),
			new Position(2243, 344, -936),
			TimeSpan.FromSeconds(1), direction: 45, color: "green");
		AddPlatformNpc("f_gele_57_4", 2302, 384, -992, 45, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_gele_57_4.Chest1", "f_gele_57_4", 2302, 384, -992, 45, ItemId.Unity_Emotion146, monsterId: 147393);
	}
}
