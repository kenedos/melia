using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class ShamanMaskQuestScript : QuestScript
{
	private const int QuestNumber = 10023;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Spirit Caller";
	private const string QuestDescription = "Help the old mystic gather materials to craft a powerful shaman mask.";

	private const int WendigoFangId = 645256;
	private const int WendigoToothId = 645174;
	private const int ShamanMaskId = 628071;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_teeth", "Collect 45 Wendigo Teeth", new CollectItemObjective(WendigoToothId, 45));
		AddObjective("collect_fangs", "Collect 8 Wendigo Fangs", new CollectItemObjective(WendigoFangId, 8));
		AddReward(new ItemReward(ShamanMaskId, 1));

		AddNpc(154093, L("[Mystic Doll] Yagra"), "f_rokas_26", -54, -1215, 45, MysticDialog);
	}

	private async Task MysticDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Yagra"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*the mystic doll seems impatient yet friendly* Ahh... the spirits speak of your arrival. The wendigos possess powerful spiritual essence in their fangs and teeth..."));
			await dialog.Msg(L("With these, I can craft a mask to help you commune with the spirit world. Will you help gather what I need?"));
		}
		else
		{
			var selection = await dialog.Select(L("*waves hand through the smoke* Have you gathered the spiritual essence I need?"),
				Option(L("Here are the materials"), "complete", () => player.Inventory.HasItem(WendigoFangId, 8) && player.Inventory.HasItem(WendigoToothId, 45)),
				Option(L("Still hunting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(WendigoFangId, 8) &&
					player.Inventory.HasItem(WendigoToothId, 45))
				{
					player.Inventory.RemoveItem(WendigoFangId, 8);
					player.Inventory.RemoveItem(WendigoToothId, 45);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*chants in ancient tongue while crafting* The spirits are pleased with these offerings. Take this mask - may it help you see beyond the veil."));

					await dialog.Msg(L("*smoke swirls* The spirits are ever-hungry. Return if you wish to craft another mask..."));
					await StartNewQuest(dialog, player);
				}
			}
			else
			{
				await dialog.Msg(L("*peers through the smoke* Continue your hunt. The spirits will guide you to what you seek."));
			}
			return;
		}

		await StartNewQuest(dialog, player);
	}

	private async Task StartNewQuest(Dialog dialog, Character player)
	{
		var startSelection = await dialog.Select(L("Will you help gather materials to craft a shaman mask?"),
			Option(L("I will help"), "accept"),
			Option(L("Maybe later"), "decline")
		);

		if (startSelection == "accept")
		{
			await player.Quests.Start(QuestId);
			await dialog.Msg(L("*nods solemnly* The wendigos' spiritual essence is strong. Their fangs and teeth hold power... bring them to me, and I will shape them into a mask of communion."));
		}
		else
		{
			await dialog.Msg(L("*returns to meditation* The spirits will wait... as will I."));
		}
	}
}
