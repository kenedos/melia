//--- Melia Script ----------------------------------------------------------
// Veja Ravine Quest NPCs
//--- Description -----------------------------------------------------------
// The Old Man of Andale Village, who points the Revelator toward the portal
// to Goddess Saule.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage581QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(18100);

	protected override void Load()
	{
		// Old Man of Andale Village
		//-------------------------------------------------------------------------
		AddNpc(147396, L("Old Man of Andale Village"), "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", 200, -1300, 102, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Man of Andale Village"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("You seem to live up to your reputation."));
				await dialog.Msg(L("The people in our village just cowardly hope that the goddess will take care of everything."));
				character.Quests.Complete(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Aren't you the Revelator?"));
				await dialog.Msg(L("So, what brings you to our village?"));

				var answer = await dialog.Select(L("I'll purify the pond"),
					Option(L("I'll purify the pond"), "accept"),
					Option(L("About Goddess Saule"), "explain"),
					Option(L("Think of another way"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Just like there are many demons, the same goes for the goddesses."));
					await dialog.Msg(L("The five goddesses are the only ones who became well known."));
					return;
				}

				if (answer == "accept")
				{
					await dialog.Msg(L("The Tanus in Zvelgian Vacant Lot carry Purifying Stones. They will be able to purify the Pond."));
					character.ServerMessage(L("Use the cable car to move across to Zvelgian Vacant Lot."));
					character.Quests.Start(Mq01);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The Tanus of Zvelgian Vacant Lot carry the Purifying Stones. Bring me twelve of them."));
				return;
			}

			await dialog.Msg(L("The portal to Goddess Saule has not worked in years. The pond must be cleansed first."));
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 18100: Searching for Goddess Saule
//-----------------------------------------------------------------------------
public class Huevillage581Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18100);
		SetName(L("Searching for Goddess Saule"));
		SetDescription(L("The portal to Goddess Saule is dead. Collect Purifying Stones from the Tanu to cleanse the Holy Pond."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", L("Search the portal in Veja Ravine"), L("As written on the revelation, go through Naslaite Cliff Path and arrive at Veja Ravine."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", L("Collect Purifying Stones from Tanu"), L("The Old Man said you need Purifying Stones of Tanu to activate the portal."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", L("Talk to the Old Man"), L("Bring the Purifying Stones to the Old Man."));

		AddPrerequisite(new QuestStatusPrerequisite(30031, QuestStatus.Completed));

		AddDrop(650654, 1f, 47472);
		AddObjective("collectStones", L("Collect Tanu's Purifying Stone"), new CollectItemObjective("HUEVILLAGE_58_1_MQ01_ITEM1", 12));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("HUEVILLAGE_58_1_MQ01_ITEM1"));
	}
}
