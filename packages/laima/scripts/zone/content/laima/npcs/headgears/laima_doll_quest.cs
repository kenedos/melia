using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class LaimaDollQuestScript : QuestScript
{
	private const int QuestNumber = 10001;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Events";
	private const string QuestName = "Laima's Sacred Doll";
	private const string QuestDescription = "Help reconstruct the Laima Sanctuary by crafting a sacred doll.";

	// Required items
	private const int HohenLeatherId = 645582;
	private const int BlackDrakeHeartId = 645206;
	private const int TallaBattleBossHammerId = 645948;
	private const int LaimaDollId = 900008;

	// Stat/Skill Reset Items and Variables
	private const int StatResetItemId = ItemId.Premium_StatReset;
	private const int SkillResetItemId = ItemId.Premium_SkillReset;
	private const int VisItemId = ItemId.Vis; // Silver (currency)
	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		// Add objectives
		AddObjective("collect_leather", $"Collect 50 Hohen Leather", new CollectItemObjective(HohenLeatherId, 50));
		AddObjective("collect_heart", $"Collect 25 Black Drake Heart", new CollectItemObjective(BlackDrakeHeartId, 25));
		AddObjective("collect_hammer", $"Collect 2 Tala Battle Boss' Hammer", new CollectItemObjective(TallaBattleBossHammerId, 2));

		// Add rewards
		AddReward(new ItemReward(LaimaDollId, 1));

		// Add NPC to Klaipeda
		AddNpc(57582, "[Angel] Seraphina", "c_Klaipe", 295, 83, 90, SerafinaDialog);
		AddNpc(57582, "[Angel] Seraphina", "c_fedimian", -410, -262, 45, SerafinaDialog);
		AddNpc(57582, "[Angel] Seraphina", "c_orsha", -65, 232, 90, SerafinaDialog);
	}

	private async Task SerafinaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.Has(QuestId);
		var hasCompletedQuest = player.Quests.HasCompleted(QuestId);

		dialog.SetTitle("Seraphina");

		// --- Main Dialog Options ---
		var mainDialogText = hasCompletedQuest
			? "Thank you again for your help with the sanctuary reconstruction. Is there anything else I can help you with?"
			: "Oh! It brings me such joy to see you again. Do you remember our first meeting at the sanctuary?";

		var options = new[]
		{
			//Option(L("Laima Doll Quest"), "laima_doll"),
			Option(L("Stat Reset"), "stat_reset"),
			Option(L("Skill Reset"), "skill_reset"),
			Option(L("Nothing, thank you."), "cancel")
		};

		var selection = await dialog.Select(mainDialogText, options);

		switch (selection)
		{
			case "laima_doll":
				await HandleLaimaDollQuest(dialog, player, hasActiveQuest, hasCompletedQuest);
				break;
			case "stat_reset":
				await HandleReset(dialog, player, StatResetItemId, "StatResetCount");
				break;
			case "skill_reset":
				await HandleReset(dialog, player, SkillResetItemId, "SkillResetCount");
				break;
			case "cancel":
				await dialog.Msg("May the blessings of the goddess Laima be with you.");
				break;
		}
	}

	// --- Laima Doll Quest Logic ---
	private async Task HandleLaimaDollQuest(Dialog dialog, Character player, bool hasActiveQuest, bool hasCompletedQuest)
	{
		if (hasCompletedQuest)
		{
			await dialog.Msg("Thank you again for your help with the sanctuary reconstruction. The Laima doll you crafted brings comfort to many pilgrims.");
			return;
		}

		if (hasActiveQuest)
		{
			var select = await dialog.Select("How goes the collection of materials for the sacred doll?",
				Option("Turn in materials", "turnin"),
				Option("I'm still gathering", "cancel")
			);

			if (select == "turnin")
			{
				// Check if player has all required materials
				if (player.Inventory.HasItem(BlackDrakeHeartId, 25) &&
					player.Inventory.HasItem(TallaBattleBossHammerId, 2) &&
					player.Inventory.HasItem(HohenLeatherId, 50))
				{
					// Remove materials
					player.Inventory.RemoveItem(BlackDrakeHeartId, 25);
					player.Inventory.RemoveItem(TallaBattleBossHammerId, 2);
					player.Inventory.RemoveItem(HohenLeatherId, 50);

					// Complete quest
					player.Quests.Complete(QuestId);

					await dialog.Msg("The materials are perfect! I'll craft this into a beautiful sacred doll for the sanctuary. Here, take this one I prepared earlier as thanks for your help.");
				}
				else
				{
					await dialog.Msg("You don't seem to have all the required materials yet. Remember, we need 50 Hohen Leather, 25 Black Drake Heart and 2 Talla Battle Boss' Hammer to craft the sacred doll.");
				}
			}
			return;
		}

		// Initial dialog for new players
		await dialog.Msg("We're working on reconstructing parts of Laima's sanctuary, and I could use your help crafting sacred dolls for the pilgrims.");

		var selection = await dialog.Select("Would you help me gather materials to craft a Laima doll?",
			Option("I'll help", "accept"),
			Option("Maybe later", "decline")
		);

		if (selection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg("Thank you! We'll need 50 Hohen Leather for the doll's body, 2 Tala Battle Boss' Hammer for the framework, and 25 Black Drake Heart for it's essence. Please bring them to me when you've gathered everything.");
		}
		else
		{
			await dialog.Msg("I understand. Please come back if you change your mind - the sanctuary could really use your help.");
		}
	}

	// --- Stat/Skill Reset Logic ---
	private async Task HandleReset(Dialog dialog, Character player, int resetItemId, string resetCountVariable)
	{
		var isStatReset = resetItemId == StatResetItemId;
		var resetTypeName = isStatReset ? "stat" : "skill";

		// Get the number of times the player has reset (stats or skills)
		var resetCount = player.Variables.Perm.GetInt(resetCountVariable);
		var basePrice = 10000000;
		if (ZoneServer.Instance.Conf.World.IsPBE)
			basePrice = 100000;
		var price = (int)(basePrice * Math.Pow(2, resetCount)); // Double the price each time

		// Check for Maximum Price (2 Billion)
		if (price > 2000000000)
		{
			price = 2000000000; // Cap the price at 2 billion
		}

		await dialog.Msg(LF("A {0} reset will cost you {1} Silver. You have reset {2} times.", resetTypeName, price, resetCount));

		// Check if the price is at the maximum
		if (price == 2000000000)
		{
			await dialog.Msg(L("Further resets will not increase the price."));
		}

		var confirm = await dialog.Select(L("Are you sure you want to proceed?"),
			Option(L("Yes, reset"), "confirm", () => player.Inventory.HasItem(VisItemId, price)),
			Option(L("No, I can't afford that"), "cancel")
		);

		if (confirm == "confirm")
		{
			if (player.HasSilver(price))
			{
				// Deduct the Vis (silver)
				player.RemoveItem(VisItemId, price);

				// Directly reset stats or skills
				if (isStatReset)
				{
					player.ResetStats();
				}
				else
				{
					player.ResetSkills();
					player.AddonMessage(AddonMessage.RESET_SKL_UP);
				}

				// Increment the reset count
				player.Variables.Perm.Set(resetCountVariable, resetCount + 1);

				await dialog.Msg(LF("Your {0}s have been reset. May the goddess guide your new path.", resetTypeName));
			}
			else
			{
				await dialog.Msg(L("You don't have enough Vis. Come back when you're richer."));
			}
		}
		else
		{
			await dialog.Msg(L("Perhaps it's best to keep things as they are... for now."));
		}
	}
}
