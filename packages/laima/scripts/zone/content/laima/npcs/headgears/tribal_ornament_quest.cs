using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class TribalOrnamentQuestScript : QuestScript
{
	private const int QuestNumber = 10010;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Tribal Treasures";
	private const string QuestDescription = "Help the tribal craftsman create a traditional hair ornament using materials from the local creatures.";

	private const int UltanunFeather = 645153;
	private const int LoxodonIvory = 645393;
	private const int TribalOrnamentId = 628074;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_feathers", "Collect 120 Ultanun Feathers", new CollectItemObjective(UltanunFeather, 120));
		AddObjective("collect_ivory", "Collect 1 Loxodon Ivory", new CollectItemObjective(LoxodonIvory, 1));
		AddReward(new ItemReward(TribalOrnamentId, 1));

		AddNpc(155120, L("[Tribal Craftsman] Kamu"), "f_huevillage_58_2", -452, -56, 0, KamuDialog);
	}

	private async Task KamuDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Kamu"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*carefully arranging feathers* Each piece tells a story, young one. The feathers speak of freedom, the ivory of strength... *looks up thoughtfully* Our traditions live on through these crafts."));
			await dialog.Msg(L("Would you like to learn our ways? Help me gather materials, and I will craft you a traditional hair ornament that carries the spirit of our tribe."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you brought the materials of our land?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(UltanunFeather, 120) && player.Inventory.HasItem(LoxodonIvory, 1)),
				Option(L("Still gathering"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(UltanunFeather, 120) &&
					player.Inventory.HasItem(LoxodonIvory, 1))
				{
					player.Inventory.RemoveItem(UltanunFeather, 120);
					player.Inventory.RemoveItem(LoxodonIvory, 1);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*works with practiced hands* Yes... the spirits of the Ultanun and Loxodon flow through these materials. Wear this ornament with pride - it carries the essence of our traditions."));

					await dialog.Msg(L("*arranges his crafting tools* The spirits are always willing to share their gifts. Return if you wish to create another."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*nods sagely* Take your time. The materials must be gathered with respect and purpose."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Would you like to learn more of our tribal crafts?"),
			Option(L("I seek to learn"), "accept"),
			Option(L("Another time"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("A wise choice! Bring me 120 Ultanun Feathers - they must be gathered with care. Also, 1 piece of Loxodon Ivory - respect the strength of these great beasts as you collect them."));
		}
		else
		{
			await dialog.Msg(L("*returns to his crafting* The spirits will wait. Return when you feel their call."));
		}
	}
}
