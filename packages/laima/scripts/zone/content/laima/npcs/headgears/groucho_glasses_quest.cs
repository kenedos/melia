using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class GrouchoGlassesQuestScript : QuestScript
{
	private const int QuestNumber = 10038;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "A Desperate Disguise";
	private const string QuestDescription = "Help a terrified villager create a disguise to fool the spirits haunting Escanciu Village.";

	private const int HallowventerHandBonesId = 645481;
	private const int HallowventerHandBonesAmount = 75;
	private const int GravegolemCoreId = 645192;
	private const int GravegolemCoreAmount = 50;
	private const int GrouchoGlassesId = 628352;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_bones", $"Collect {HallowventerHandBonesAmount} Hallowventer Hand Bones", new CollectItemObjective(HallowventerHandBonesId, HallowventerHandBonesAmount));
		AddObjective("collect_cores", $"Collect {GravegolemCoreAmount} Gravegolem Core", new CollectItemObjective(GravegolemCoreId, GravegolemCoreAmount));

		AddReward(new ItemReward(GrouchoGlassesId, 1));

		AddNpc(20117, L("[Survivor] Berthold"), "f_remains_39", 442, 562, 0, BertholdDialog);
	}

	private async Task BertholdDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Berthold"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*crouches behind a crumbling wall, eyes darting nervously*{/}"));
			await dialog.Msg(L("Shh! Keep your voice down! They might hear us... The spirits, they're everywhere now. Ever since they invaded the village, I've been hiding here, barely surviving."));
			await dialog.Msg(L("I've watched them for weeks. They don't seem to recognize living faces they haven't seen before. If I could just... disguise myself somehow, change my face, I might be able to slip past them and escape!"));
			await dialog.Msg(L("I know it sounds mad, but I've been working on something. A sort of mask, with a fake nose and spectacles. Ridiculous looking, yes, but if it fools the spirits, I don't care how silly I appear!"));
		}
		else
		{
			var selection = await dialog.Select(L("Did you gather the materials I need?"),
				Option(L("Yes, I have everything"), "complete",
					() => player.Inventory.HasItem(HallowventerHandBonesId, HallowventerHandBonesAmount) &&
						  player.Inventory.HasItem(GravegolemCoreId, GravegolemCoreAmount)),
				Option(L("Still collecting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(HallowventerHandBonesId, HallowventerHandBonesAmount) &&
					player.Inventory.HasItem(GravegolemCoreId, GravegolemCoreAmount))
				{
					player.Inventory.RemoveItem(HallowventerHandBonesId, HallowventerHandBonesAmount);
					player.Inventory.RemoveItem(GravegolemCoreId, GravegolemCoreAmount);
					player.Quests.Complete(QuestId);

					await dialog.Msg(L("{#666666}*grabs the materials with trembling hands*{/}"));
					await dialog.Msg(L("Yes! Yes, this is perfect! The bone fragments will form the frame, and the cores... I can grind them into a paste that hardens like stone."));
					await dialog.Msg(L("{#666666}*works frantically, assembling the peculiar disguise*{/}"));
					await dialog.Msg(L("There! It's done! A bit... unusual looking, I admit. But I made two - one for me, and one for you. You've earned it, friend."));
					await dialog.Msg(L("The spirits seem to avoid anything that looks even slightly different from what they remember. This ridiculous thing might just save both our lives. Thank you, truly."));
				}
			}
			else
			{
				await dialog.Msg(L("{#666666}*peeks nervously around the corner*{/}"));
				await dialog.Msg(L("Please hurry! The Hallowventers that float through the village streets... their bony hands hold the key. And the Gravegolems that patrol the ruins, their cores are what I need to bind it all together."));
				await dialog.Msg(L("Be careful out there. The spirits are restless, especially at night."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Will you help me gather what I need for this disguise?"),
			Option(L("I'll help you"), "accept"),
			Option(L("That sounds too dangerous"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("{#666666}*relief washes over his face*{/}"));
				await dialog.Msg(L("Thank the goddesses! I need 75 Hand Bones from those horrible Hallowventers. Their skeletal fingers are surprisingly sturdy, perfect for the frame of the disguise."));
				await dialog.Msg(L("I also need 50 Gravegolem Cores. The golems roam throughout these ruins - their cores can be ground into a bonding paste."));
				await dialog.Msg(L("I know it seems like a lot, but I want to make sure this disguise is convincing. My life depends on it... and perhaps yours will too, someday."));
				break;
			case "decline":
				await dialog.Msg(L("{#666666}*his face falls*{/}"));
				await dialog.Msg(L("I... I understand. It is dangerous out there. Maybe I'll find another way... or maybe I'll just stay hidden here forever."));
				break;
		}
	}
}
