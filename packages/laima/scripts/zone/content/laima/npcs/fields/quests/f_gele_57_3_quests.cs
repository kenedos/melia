//--- Melia Script ----------------------------------------------------------
// Nefritas Cliff (Uzbaigi Hillside) Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Nefritas Cliff for cross-map quest interactions.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;
using Yggdrasil.Util;

public class FGele573QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1003: Message to the Hillside - Hillside Watcher (Destination)
		//---------------------------------------------------------------------
		AddNpc(20128, "[Hillside Watcher] Roderic", "f_gele_57_3", -16, -286, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_2", 1003);
			var letterId = 666037;

			dialog.SetTitle("Roderic");

			if (character.Quests.IsActive(questId) && character.Inventory.HasItem(letterId))
			{
				await dialog.Msg("{#666666}*The scout glances around nervously before acknowledging you*{/}");
				await dialog.Msg("Keep your voice down! A message from Gareth? Finally!");
				await dialog.Msg("{#666666}*He quickly breaks the seal, his eyes darting to the surroundings*{/}");
				await dialog.Msg("The creatures here... they're aggressive. I can barely rest without something trying to tear me apart.");
				await dialog.Msg("You made it through alive - that's more than I can say for the last courier. Take this reward and get out of here while you can.");

				character.Inventory.Remove(letterId, 1, InventoryItemRemoveMsg.Given);
				character.Quests.Complete(questId);
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg("{#666666}*He keeps his weapon ready*{/}");
				await dialog.Msg("Still here? Don't linger too long. The monsters in this area don't give warnings before they strike.");
			}
			else
			{
				await dialog.Msg("{#666666}*The scout speaks in hushed tones, constantly scanning the area*{/}");
				await dialog.Msg("You shouldn't be here. This place is crawling with aggressive creatures - Banshees, Puragi, things that hunt without mercy.");
				await dialog.Msg("If you don't have business with me, I suggest you leave. Quickly.");
			}
		});
	}
}
