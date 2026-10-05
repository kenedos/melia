//--- Melia Script ----------------------------------------------------------
// Ruklys Street
//--- Description -----------------------------------------------------------
// NPCs found in and around Ruklys Street.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash61NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(13, 40120, "Statue of Goddess Vakarine", "f_flash_61", -99.65971, 435.358, 1297.89, 0, "WARP_F_FLASH_61", "STOUP_CAMP", "STOUP_CAMP");

		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_61", -735.61, 511.26, 189.44, 0, "TREASUREBOX_LV_F_FLASH_611000", "", "");

		// Map-Wide Petrification Curse
		//-------------------------------------------------------------------------
		var petrifyTrigger = AddAreaTrigger("f_flash_61", 0, 0, 99999, async (args) =>
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

			if (RandomProvider.Get().Next(100) >= 5)
				return;

			character.StartBuff(BuffId.Petrification, 1, 0, TimeSpan.FromSeconds(3), character);
		});

		petrifyTrigger.SetWhileInsideTrigger("FLASH_PETRIFICATION_WHILE_INSIDE_61", petrifyTrigger.EnterFunc);
	}
}
