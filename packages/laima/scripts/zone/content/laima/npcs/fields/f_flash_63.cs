//--- Melia Script ----------------------------------------------------------
// Downtown
//--- Description -----------------------------------------------------------
// NPCs found in and around Downtown.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash63NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_63", 1208.05, 424.94, 1317.05, 45, "TREASUREBOX_LV_F_FLASH_631000", "", "");

		// Track NPCs
		//---------------------------------------------------------------------------
		AddTrackNPC(147427, "", "f_flash_63", 897.8192, 233.7511, 654.0224, 357.82477, "flash_elevator", 2, 1);

		// Map-Wide Petrification Curse
		//-------------------------------------------------------------------------
		var petrifyTrigger = AddAreaTrigger("f_flash_63", 0, 0, 99999, async (args) =>
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

		petrifyTrigger.SetWhileInsideTrigger("FLASH_PETRIFICATION_WHILE_INSIDE_63", petrifyTrigger.EnterFunc);
	}
}
