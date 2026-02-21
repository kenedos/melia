using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class GhostHeadbandQuestScript : QuestScript
{
	private const int QuestNumber = 10007;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Spectral Vengeance";
	private const string QuestDescription = "Help the spirit of Marcus gather essence from the Banshees that ended his life.";

	private const int BansheeSpriteMatterId = 645348;
	private const int BansheeHoodId = 645412;
	private const int GhostHeadbandId = 628081;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_matter", "Collect 75 Banshee Spirit Matter", new CollectItemObjective(BansheeSpriteMatterId, 75));
		AddObjective("collect_hoods", "Collect 3 Banshee Hoods", new CollectItemObjective(BansheeHoodId, 3));
		AddReward(new ItemReward(GhostHeadbandId, 1));

		AddNpc(155025, L("[Spirit] Marcus"), "f_gele_57_3", -685, -679, 45, MarcusDialog);
	}

	private async Task MarcusDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Marcus"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("*a small agitated spirit hovers around the gravestone* Those wretched Banshees... their screams still echo in my ethereal form. I need your help to exact my revenge."));
			await dialog.Msg(L("Bring me their essence, their very being... Only then can I find peace. Or perhaps... *voice fades* just a measure of satisfaction."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the Banshee essences?"),
				Option(L("Yes, here they are"), "complete", () => player.Inventory.HasItem(BansheeSpriteMatterId, 75) && player.Inventory.HasItem(BansheeHoodId, 3)),
				Option(L("Still hunting"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(BansheeSpriteMatterId, 75) &&
					player.Inventory.HasItem(BansheeHoodId, 3))
				{
					player.Inventory.RemoveItem(BansheeSpriteMatterId, 75);
					player.Inventory.RemoveItem(BansheeHoodId, 3);

					player.Quests.Complete(QuestId);
					await dialog.Msg(L("*the spirits swirl around you* Yes... YES! I can feel their essence! Take this headband, forged from my spiritual energy. May it serve you well... as you have served my vengeance."));
				}
			}
			else
			{
				await dialog.Msg(L("*ghost flickers* I've waited this long for revenge... I can wait longer. But do hurry, won't you?"));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Will you help me gather these essences of vengeance?"),
			Option(L("I will help"), "accept"),
			Option(L("Not now"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("*ghost brightens* Excellent! Bring me 75 measures of their Spirit Matter and 3 of their Hoods. Every essence you gather weakens them... and strengthens my revenge!"));
				break;
			case "decline":
				await dialog.Msg(L("*ghost fades slightly* Then I shall continue to wait... as I have for so long already..."));
				break;
			default:
				break;
		}
	}
}
