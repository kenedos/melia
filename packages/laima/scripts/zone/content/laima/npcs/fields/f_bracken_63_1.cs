//--- Melia Script ----------------------------------------------------------
// Koru Jungle
//--- Description -----------------------------------------------------------
// NPCs found in and around Koru Jungle.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken631NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(153107, "", "f_bracken_63_1", -268.1401, 544.2999, 995.2676, 21, "f_bracken_63_1_elt", 3);

		// Track Starting NPCs
		//---------------------------------------------------------------------------
		AddTrackStartingNPC(153108, "", "f_bracken_63_1", -254.3194, 2126.975, 0, "f_bracken_63_1_flume", -256.82f, 978.94f, 2094.44f);

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_bracken_63_1", -597, 955, 1080, 45, "blue");
		AddMovingPlatformNpc("f_bracken_63_1",
			new Position(-678, 995, 1050),
			new Position(-564, 995, 1162),
			TimeSpan.FromSeconds(1), direction: 45, color: "green");
		AddMovingPlatformNpc("f_bracken_63_1",
			new Position(-614, 1035, 1212),
			new Position(-728, 1035, 1100),
			TimeSpan.FromSeconds(1), direction: 45, color: "yellow");
		AddPlatformNpc("f_bracken_63_1", -687, 1075, 1170, 45, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_bracken_63_1.Chest1", "f_bracken_63_1", -687, 1075, 1170, 45, ItemId.EmoticonItem_64_69, monsterId: 147393);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(503, 147392, "Lv1 Treasure Chest", "f_bracken_63_1", 1658, 0, -866, 0, "TREASUREBOX_LV_F_BRACKEN_63_1503", "", "");
	}
}
