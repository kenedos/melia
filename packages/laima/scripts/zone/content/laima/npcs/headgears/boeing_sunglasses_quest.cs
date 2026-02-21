using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class BoeingSunglassesQuestScript : QuestScript
{
	private const int QuestNumber = 10030;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Hogma Investigation";
	private const string QuestDescription = "Help the Klaipeda agent investigate the Hogmas around Fedimian by collecting evidence.";

	private const int LauzininteSkinId = 645176;
	private const int HogmaTuskId = 645177;
	private const int BoeingSunglassesId = 629023;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_skin", "Collect 50 Lauzinute Skins", new CollectItemObjective(LauzininteSkinId, 50));
		AddObjective("collect_tusk", "Collect 30 Hogma Tusks", new CollectItemObjective(HogmaTuskId, 30));
		AddReward(new ItemReward(BoeingSunglassesId, 1));

		AddNpc(20143, L("[Klaipeda Agent] Markas"), "f_rokas_28", 225, -289, 0, AgentDialog);
	}

	private async Task AgentDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Markas"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*adjusts his collar and looks around nervously* I've been sent here from Klaipeda to investigate these... Hogmas. Humanoid pigs, they call them."));
			await dialog.Msg(L("The reports say they're becoming more aggressive around Fedimian. I need proof of their activities, but I'd rather not get too close to those creatures myself."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the evidence I need?"),
				Option(L("Yes, here's the evidence"), "complete", () => player.Inventory.HasItem(LauzininteSkinId, 50) && player.Inventory.HasItem(HogmaTuskId, 30)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(LauzininteSkinId, 50) &&
					player.Inventory.HasItem(HogmaTuskId, 30))
				{
					player.Inventory.RemoveItem(LauzininteSkinId, 50);
					player.Inventory.RemoveItem(HogmaTuskId, 30);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*examines the materials carefully* Excellent! The Lauzinute skins and Hogma tusks provide clear evidence of the creatures' presence and behavior."));
					await dialog.Msg(L("Here, take these sunglasses. They were part of my field equipment, but I think you've earned them more than I have. Stay safe out there."));
					await StartNewQuest(dialog, player);
				}
				else
				{
					await dialog.Msg(L("*frowns* Hmm, I don't see enough evidence here. I need 50 Lauzinute skins and 30 Hogma tusks to compile a proper report."));
				}
				return;
			}

			await dialog.Msg(L("*scribbles in a notebook* Take your time, but be careful. These creatures aren't to be taken lightly."));
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you be willing to help me gather evidence from the creatures in this area?"),
			Option(L("I'll help with the investigation"), "accept"),
			Option(L("Not right now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*breathes a sigh of relief* Thank you! I need 50 Lauzinute skins from the scorpion-like creatures and 30 Hogma tusks from those pig-faced archers. This evidence will help me compile a thorough report on the regional threats."));
		}
		else
		{
			await dialog.Msg(L("*nods understandingly* I understand. If you change your mind, I'll be here... keeping a safe distance from those Hogmas."));
		}
	}
}
