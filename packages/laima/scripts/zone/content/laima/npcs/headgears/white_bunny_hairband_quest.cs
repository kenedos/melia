using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors.Characters;

public class WhiteBunnyHairbandQuestScript : QuestScript
{
	private const int QuestNumber = 10054;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Soft as Snow";
	private const string QuestDescription = "Gather Lepusbunny Hair and White Chains from the creatures roaming the Tableland for a traveling seamstress.";

	private const int LepusbunnyHairId = 645392;
	private const int LepusbunnyHairAmount = 60;
	private const int WhiteChainId = 645649;
	private const int WhiteChainAmount = 8;
	private const int WhiteBunnyHairbandId = 628141;

	protected override void Load()
	{
		this.SetId(QuestNamespace, QuestNumber);
		this.SetName(QuestName);
		SetType(QuestType.Repeat);
		this.SetDescription(QuestDescription);
		this.SetUnlock(QuestUnlockType.AllAtOnce);
		this.SetCancelable(true);
		this.SetReceive(QuestReceiveType.Manual);

		this.AddObjective("collect_hair", "Collect Lepusbunny Hair",
			new CollectItemObjective(LepusbunnyHairId, LepusbunnyHairAmount));
		this.AddObjective("collect_chains", "Collect White Chains",
			new CollectItemObjective(WhiteChainId, WhiteChainAmount));

		this.AddReward(new ItemReward(WhiteBunnyHairbandId, 1));

		AddNpc(155033, L("[Seamstress] Mirela"), "f_tableland_28_1", -881, 1549, 90, this.MirelaDialog);
	}

	private async Task MirelaDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Mirela"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*runs her fingers through a bundle of soft white fur*{/}"));
			await dialog.Msg(L("Oh, hello there! I've been watching the bunnies that hop around these highlands. Their fur is incredibly soft, perfect for sewing into accessories."));
			await dialog.Msg(L("I'm working on a special hairband design, white as fresh snow. But I'll need their fur and some White Chains from those strange magicians nearby to hold it all together."));
		}
		else
		{
			var selection = await dialog.Select(L("Were you able to gather enough Lepusbunny Hair?"),
				Option(L("Yes, here they are"), "complete",
					() => player.Inventory.HasItem(LepusbunnyHairId, LepusbunnyHairAmount) &&
					  player.Inventory.HasItem(WhiteChainId, WhiteChainAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(LepusbunnyHairId, LepusbunnyHairAmount) &&
					player.Inventory.HasItem(WhiteChainId, WhiteChainAmount))
				{
					player.Quests.Complete(QuestId);
					await dialog.Msg(L("{#666666}*deftly weaves the fur into a delicate hairband*{/}"));
					await dialog.Msg(L("Just wonderful! Feel how soft it is. Here, take it, you've more than earned it."));
				}
			}
			else
			{
				await dialog.Msg(L("The green ones shed the most, keep an eye out for them!"));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather some Lepusbunny Hair?"),
			Option(L("I'll help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("Wonderful! Bring me 40 tufts of Lepusbunny Hair and 8 White Chains. The bunnies and those red magicians around here should have what I need."));
				break;
			case "decline":
				await dialog.Msg(L("That's alright, dear. I'll be here if you change your mind."));
				break;
		}
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.RemoveItem(LepusbunnyHairId, LepusbunnyHairAmount);
		character.Inventory.RemoveItem(WhiteChainId, WhiteChainAmount);
	}
}
