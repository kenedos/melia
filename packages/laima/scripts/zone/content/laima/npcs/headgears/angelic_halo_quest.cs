using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class AngelicHaloQuestScript : QuestScript
{
	private const int QuestNumber = 10036;
	private new readonly QuestId QuestId = new(QuestNamespace, QuestNumber);
	private const string QuestNamespace = "Laima.Quests";
	private const string QuestName = "Light of the Sacred Waters";
	private const string QuestDescription = "Help Sister Celestine craft a blessed halo using purified materials from the Goddess' Ancient Garden.";

	private const int ResinId = 645427;
	private const int ResinAmount = 75;
	private const int InfroburgShellId = 645190;
	private const int InfroburgShellAmount = 100;
	private const int AngelicHaloId = 628246;

	protected override void Load()
	{
		SetId(QuestNamespace, QuestNumber);
		SetName(QuestName);
		SetDescription(QuestDescription);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Manual);

		AddObjective("collect_resin", $"Collect {ResinAmount} Resin", new CollectItemObjective(ResinId, ResinAmount));
		AddObjective("collect_shells", $"Collect {InfroburgShellAmount} Infroburk Shell", new CollectItemObjective(InfroburgShellId, InfroburgShellAmount));

		AddReward(new ItemReward(AngelicHaloId, 1));

		AddNpc(147493, L("[Priestess] Celestine"), "f_remains_38", 236, -541, 90, CelestineDialog);
	}

	private async Task CelestineDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var hasActiveQuest = player.Quests.IsActive(QuestId);

		dialog.SetTitle(L("Celestine"));

		if (!hasActiveQuest)
		{
			await dialog.Msg(L("{#666666}*kneels in prayer beside the sacred waters*{/}"));
			await dialog.Msg(L("Blessings upon you, traveler. I am Sister Celestine, keeper of this sacred shrine. The goddess has guided your steps here for a purpose, I am certain."));
			await dialog.Msg(L("Long ago, the angels who served the goddesses wore halos of pure light. Though those celestial beings have long departed, I believe we can recreate their blessed crowns using the sacred materials found in this garden."));
		}
		else
		{
			var selection = await dialog.Select(L("Have you gathered the blessed materials?"),
				Option(L("Yes, I have them"), "complete",
					() => player.Inventory.HasItem(ResinId, ResinAmount) &&
						  player.Inventory.HasItem(InfroburgShellId, InfroburgShellAmount)),
				Option(L("Still searching"), "progress")
			);

			if (selection == "complete")
			{
				if (player.Inventory.HasItem(ResinId, ResinAmount) &&
					player.Inventory.HasItem(InfroburgShellId, InfroburgShellAmount))
				{
					player.Inventory.RemoveItem(ResinId, ResinAmount);
					player.Inventory.RemoveItem(InfroburgShellId, InfroburgShellAmount);
					player.Quests.Complete(QuestId);

					await dialog.Msg(L("{#666666}*accepts the materials with reverence*{/}"));
					await dialog.Msg(L("The resin carries the essence of the ancient trees that have witnessed centuries of prayer. And these shells... I can feel the warmth of divine fire still within them."));
					await dialog.Msg(L("{#666666}*dips the materials into the sacred waters and begins to chant*{/}"));
					await dialog.Msg(L("By the light of the goddesses, let this halo shine with celestial radiance. May it protect the wearer and remind them of the divine presence that watches over us all."));
					await dialog.Msg(L("{#666666}*a soft golden glow emanates from her hands*{/}"));
					await dialog.Msg(L("It is done. Take this Angelic Halo, blessed child. Wear it with humility, and may the goddesses smile upon your journey."));
				}
			}
			else
			{
				await dialog.Msg(L("The Long-Branched Trees in this garden produce a sacred resin that has absorbed centuries of prayers. The Infroburks, though fearsome, carry shells tempered by divine fire."));
				await dialog.Msg(L("Gather these materials with a pure heart, and the goddesses will guide your hands."));
			}
			return;
		}

		var startSelection = await dialog.Select(L("Would you help me gather the materials to craft a blessed halo?"),
			Option(L("I would be honored"), "accept"),
			Option(L("Perhaps another time"), "decline")
		);

		switch (startSelection)
		{
			case "accept":
				await player.Quests.Start(QuestId);
				await dialog.Msg(L("The goddesses smile upon your generosity. I shall need 75 portions of Resin from the Long-Branched Trees that grow in this sacred garden."));
				await dialog.Msg(L("I also require 100 Infroburk Shells. Though these creatures seem fearsome, their shells have been tempered by holy fire and carry a spark of the divine."));
				await dialog.Msg(L("Return to me when you have gathered these materials, and together we shall craft something truly miraculous."));
				break;
			case "decline":
				await dialog.Msg(L("I understand. The path of service is not for everyone. May the goddesses watch over you nonetheless."));
				break;
		}
	}
}
