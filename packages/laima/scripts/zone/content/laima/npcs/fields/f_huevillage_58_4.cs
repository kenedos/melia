//--- Melia Script ----------------------------------------------------------
// Septyni Glen
//--- Description -----------------------------------------------------------
// NPCs found in and around Septyni Glen.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage584NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(43, 40120, "Statue of Goddess Vakarine", "f_huevillage_58_4", 20.74365, -8.675209, -837.3439, 90, "WARP_F_HUEVILLAGE_58_4", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(45, 147392, "Lv1 Treasure Chest", "f_huevillage_58_4", 240, -8.58, -735, 270, "TREASUREBOX_LV_F_HUEVILLAGE_58_445", "", "");

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("f_huevillage_58_4", 210, 56, 966, 0, "blue");
		AddPlatformNpc("f_huevillage_58_4", 210, 96, 916, 0, "green");
		AddPlatformNpc("f_huevillage_58_4", 210, 136, 866, 0, "red");
		AddPlatformNpc("f_huevillage_58_4", 142, 176, 826, 0, "yellow");
		AddPlatformNpc("f_huevillage_58_4", 142, 216, 746, 0, "blue");
		AddPlatformNpc("f_huevillage_58_4", 85, 256, 696, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.f_huevillage_58_4.Chest1", "f_huevillage_58_4", 85, 256, 696, 0, ItemId.Pajauta_Emoticon_152_154, monsterId: 147393);
	}
}
