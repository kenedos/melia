//--- Melia Script ----------------------------------------------------------
// Coastal Fortress
//--- Description -----------------------------------------------------------
// NPCs found in and around Coastal Fortress.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash291NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(1000, 147392, "Lv1 Treasure Chest", "f_flash_29_1", -1129.15, 0.12, -835.57, 135, "TREASUREBOX_LV_F_FLASH_29_11000", "", "");

		// Map-Wide Petrification Curse
		//-------------------------------------------------------------------------
		var petrifyTrigger = AddAreaTrigger("f_flash_29_1", 0, 0, 99999, async (args) =>
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

		petrifyTrigger.SetWhileInsideTrigger("FLASH_PETRIFICATION_WHILE_INSIDE_29_1", petrifyTrigger.EnterFunc);
	}
}
