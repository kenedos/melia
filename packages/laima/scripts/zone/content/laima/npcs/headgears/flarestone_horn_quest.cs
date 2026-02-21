using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FlarestoneHornQuestScript : QuestScript
{
	private const int QuestNumber = 10041;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "The Cursed Woods";
	private const string QuestDescription = "Help a desperate villager find a way to ward off the devil statues terrorizing Spring Light Woods.";

	private const int SiaulambMaskId = 645596;
	private const int SiaulambMaskAmount = 80;
	private const int FlarestoneHornId = 628180;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_masks", "Collect 80 Siaulamb Masks", new CollectItemObjective(SiaulambMaskId, SiaulambMaskAmount));
		AddReward(new ItemReward(FlarestoneHornId, 1));

		AddNpc(20117, L("[Frightened Villager] Valdas"), "f_siauliai_46_1", -184, -990, 90, VillagersDialog);
	}

	private async Task VillagersDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Valdas"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*glances nervously at the surrounding trees*{/}"));
			await dialog.Msg(L("Traveler! Please, you must help us! The devil statues... they've come alive! They attack anyone who enters these woods now."));
			await dialog.Msg(L("The Siaulavs that roam here, they wear cursed masks that seem connected to this dark magic. If we can gather enough of their masks, perhaps we can break this curse... or at least protect ourselves from it."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you collected the Siaulamb Masks?"),
				Option(L("Yes, I have them"), "complete", () => player.Inventory.HasItem(SiaulambMaskId, SiaulambMaskAmount)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(SiaulambMaskId, SiaulambMaskAmount))
				{
					player.Inventory.RemoveItem(SiaulambMaskId, SiaulambMaskAmount);
					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*takes the masks with trembling hands*{/}"));
					await dialog.Msg(L("These masks... I can feel the dark energy within them. But look what I found hidden among the Siaulavs' possessions - a horn carved from flarestone! It pulses with fiery power."));
					await dialog.Msg(L("Take it. Perhaps it will ward off the evil that plagues these woods. And if you ever need another... well, the curse shows no signs of lifting."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("The Siaulavs lurk deeper in the woods to the east. Be careful of the Shardstatues - they're the awakened devil statues I warned you about!"));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Will you help us gather the cursed masks?"),
			Option(L("I'll help you"), "accept"),
			Option(L("Not right now"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("Thank the goddess! Hunt the Siaulavs in the eastern parts of these woods. Bring me 80 of their masks, and I'll reward you well for your bravery."));
		}
		else
		{
			await dialog.Msg(L("{#666666}*sighs heavily*{/} I understand. These are dangerous times. Please reconsider if you change your mind."));
		}
	}
}
