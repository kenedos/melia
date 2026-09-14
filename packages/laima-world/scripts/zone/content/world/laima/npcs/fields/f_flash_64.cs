//--- Melia Script ----------------------------------------------------------
// Inner Enceinte District
//--- Description -----------------------------------------------------------
// NPCs found in and around Inner Enceinte District.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash64NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddStatPointStatue(7, "F_FLASH_64_EV_55_001", "f_flash_64", -570.1172, 885.3875, 1373.78, 105);

		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddWarpStatue(27, "WARP_F_FLASH_64", "f_flash_64", -142.5398, 745.6932, -1353.881, 0);

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_64", -560.66, 673.74, 526.11, 135, "TREASUREBOX_LV_F_FLASH_641000", "", "");

		// Map-Wide Petrification Curse
		//-------------------------------------------------------------------------
		var petrifyTrigger = AddAreaTrigger("f_flash_64", 0, 0, 99999, async (args) =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.IsDead)
				return;

			var lastCheckKey = $"FlashCurse.LastCheck.{character.Handle}";
			var lastCheck = args.Npc.Vars.Get<DateTime>(lastCheckKey, DateTime.MinValue);
			var now = DateTime.Now;

			if ((now - lastCheck).TotalSeconds < 10)
				return;

			args.Npc.Vars.Set(lastCheckKey, now);

			if (GameRandom.Get().Next(100) >= 5)
				return;

			character.StartBuff(BuffId.Petrification, 1, 0, TimeSpan.FromSeconds(3), character);
		});

		petrifyTrigger.SetWhileInsideTrigger("FLASH_PETRIFICATION_WHILE_INSIDE_64", petrifyTrigger.EnterFunc);
	}
}
