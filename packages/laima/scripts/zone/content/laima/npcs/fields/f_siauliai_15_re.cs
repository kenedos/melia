//--- Melia Script ----------------------------------------------------------
// Woods of the Linked Bridges
//--- Description -----------------------------------------------------------
// NPCs found in and around Woods of the Linked Bridges.
// Includes environmental interactive NPCs.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai15ReNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(10, 40120, "Statue of Goddess Vakarine", "f_siauliai_15_re", 372.0667, 878.1719, 194.3833, -4, "WARP_F_SIAULIAI_15RE", "STOUP_CAMP", "STOUP_CAMP");

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_siauliai_15_re", -903, 962, 1335, 0, "red");
		AddMovingPlatformNpc("f_siauliai_15_re",
			new Position(-903, 1002, 1277),
			new Position(-903, 1002, 1145),
			TimeSpan.FromSeconds(3), color: "blue");
		AddPlatformNpc("f_siauliai_15_re", -973, 1032, 1145, 0, "yellow");
		AddMovingPlatformNpc("f_siauliai_15_re",
			new Position(-973, 1062, 1195),
			new Position(-1133, 1062, 1195),
			TimeSpan.FromSeconds(3), color: "green");
		AddPlatformNpc("f_siauliai_15_re", -1187, 1097, 1193, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_siauliai_15_re.Chest1", "f_siauliai_15_re", -1187, 1097, 1193, 90, ItemId.EmoticonItem_77_79, monsterId: 147393);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(643, 147392, "Lv1 Treasure Chest", "f_siauliai_15_re", 1575, 878, 435, 0, "TREASUREBOX_LV_F_SIAULIAI_15_RE643", "", "");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(644, 147392, "Lv1 Treasure Chest", "f_siauliai_15_re", 463.58, 922.86, 1401.50, 45, "TREASUREBOX_LV_F_SIAULIAI_15_RE644", "", "");
	}
}
