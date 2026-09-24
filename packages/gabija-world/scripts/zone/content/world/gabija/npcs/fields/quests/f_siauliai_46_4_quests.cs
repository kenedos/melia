//--- Melia Script ----------------------------------------------------------
// Dina Bee Farm Quest NPCs
//--- Description -----------------------------------------------------------
// The beekeepers of Dina, the brewer who was going to burn the forest down,
// and the honey every monster in the woods has come for.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai464QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(16000);
	private readonly static QuestId Mq02 = new QuestId(16010);
	private readonly static QuestId Mq03 = new QuestId(16020);
	private readonly static QuestId Mq04 = new QuestId(16030);
	private readonly static QuestId Mq05 = new QuestId(16040);
	private readonly static QuestId Sq01 = new QuestId(16100);
	private readonly static QuestId Sq02 = new QuestId(16110);
	private readonly static QuestId Sq03 = new QuestId(16120);

	private const int OilBarrelsToDrain = 6;

	// The oil barrels Dorjen set out around Micolas Brewery.
	private readonly static double[,] OilBarrels =
	{
		{ 351.82, 32.09 }, { 108.03, 56.71 }, { 19.90, -153.51 },
		{ 588.34, -214.49 }, { 286.49, -414.26 }, { 991.85, -351.39 },
	};

	protected override void Load()
	{
		// Villager Darren
		//-------------------------------------------------------------------------
		AddNpc(147407, L("Villager Darren"), "SIAULIAI_46_4_MQ01_NPC", "f_siauliai_46_4", -227.36, -960.28, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Villager Darren"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("The bee farm is alright? What a relief."));
				await dialog.Msg(L("I got to think of a way to protect the bee farm now, quickly."));
				await dialog.CompleteQuest(Mq01);
				character.Quests.Start(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("You got one!"));
				await dialog.Msg(L("I hope this will be enough to fool them."));
				await dialog.CompleteQuest(Mq02);
				character.Quests.Start(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("The bees are a problem but the monsters that came in from the other forests are a bigger problem. I'm sure they are here for the honey."),
					Option(L("I'll take a look"), "accept"),
					Option(L("I've done enough so just go my way"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("The monsters that smell sweet must have empty beehives. Goddess Austeja, we will need to borrow some of her help here."));
				}

				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Are you the person they sent here?"));

				var answer = await dialog.SelectQuestOffer(Mq01, L("Even if it's not you, will you please help our village?"),
					Option(L("I'll check it out"), "accept"),
					Option(L("About the town"), "explain"),
					Option(L("I'm busy so I'll pass"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Our town lived on bee farming and mead."));
					await dialog.Msg(L("Goddess Austeja looks after us so we were better off than other villages."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("Really? The other Revelators just ignored us..."));
					await dialog.Msg(L("Ours is the Rododun Apiary just down the road. Thank you."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("By the way, I'm worried about the brewery of Mr. Dorjen. He must be fuming after seeing his life's work ruined by monsters."),
					Option(L("I'll help"), "accept"),
					Option(L("I'll get going then"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("He spread out oil barrels near Micolas Brewery. You must get rid of those first."));
					await dialog.Msg(L("And about Mr. Dorjen.. Please help him."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("I hope nothing happened."));
				await dialog.Msg(L("We put a lot of effort in the farm. We can't just lose it."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The monsters that smell sweet must have empty beehives."));
				await dialog.Msg(L("Goddess Austeja, we will need to borrow some of her help here."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("He spread out oil barrels near Micolas Brewery. You must get rid of those first."));
				return;
			}

			await dialog.Msg(L("The bees the village lived on have turned, and everything in the forest wants the honey."));
		});

		// Kirina
		//-------------------------------------------------------------------------
		AddNpc(147418, L("Kirina"), "SIAULIAI_46_4_SQ03_NPC", "f_siauliai_46_4", -260.58, -979.75, 60, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kirina"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thank you. Now how do I get back from here..."));
				await dialog.Msg(L("Isn't it ironic, that the very bees that are beholden to us have now become monsters?"));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("I followed Darren here because I was worried for him, but what can I do now? There are so many monsters, I'm worried if I could even make it back."),
					Option(L("I'll take care of the monsters around"), "accept"),
					Option(L("Be careful and don't let your guard down on your way back"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("The monsters here all seem like they're possessed by something, it's scary."));
					await dialog.Msg(L("Now that I think about it, they all seemed to have our honey stuck on them..."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("The monsters here all seem like they're possessed by something, it's scary."));
				return;
			}

			await dialog.Msg(L("A woman who followed her neighbour out to the farm and now cannot see a way back."));
		});

		// Brewer Dorjen
		//-------------------------------------------------------------------------
		AddNpc(147476, L("Brewer Dorjen"), "SIAULIAI_46_4_MQ04_NPC", "f_siauliai_46_4", 1074.29, 482.62, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Brewer Dorjen"));

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("What could the Revelator possibly want from me.."));
				await dialog.CompleteQuest(Mq03);
				character.Quests.Start(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("What in the world.. did you actually get that?"));
				await dialog.Msg(L("I thought you would give up because of the monsters... You are really remarkable."));
				await dialog.CompleteQuest(Mq04);
				character.Quests.Start(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("That is enough. Whoa, Joseph did find great talent,"));
				await dialog.Msg(L("even though we lost the apiary and brewery."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("Don't you dare think of stopping me. I am going to blow them up, even if it's going to cost me my brewery! ..Can't even trust Revelators anymore now."),
					Option(L("Calm down"), "accept"),
					Option(L("He's not going to calm down. Let's just go."), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("I bet you won't be able to get it. Just know that if you fail, I will burn down my workshop."));
				}

				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("Alright. You win. I won't burn down my workshop as I said. But I'm still pissed. All those years of work... because of those monsters.."),
					Option(L("I'll defeat the monsters around, so lighten up dude."), "accept"),
					Option(L("I've helped enough so I'm leaving"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					await dialog.Msg(L("I won't think about burning down the forest again. I'm sure a person with abilities like yours can save our village."));
				}

				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("I bet you won't be able to get it."));
				await dialog.Msg(L("Just know that if you fail, I will burn down my workshop."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("I won't think about burning down the forest again."));
				await dialog.Msg(L("I'm sure a person with abilities like yours can save our village."));
				return;
			}

			await dialog.Msg(L("A brewer standing in what is left of his brewery with a box of matches."));
		});

		// Cleopas
		//-------------------------------------------------------------------------
		AddNpc(147483, L("Cleopas"), "SIAULIAI_46_4_SQ04_NPC01", "f_siauliai_46_4", 1343.83, 388.26, 0, async dialog =>
		{
			dialog.SetTitle(L("Cleopas"));

			await dialog.Msg(L("Mikolas keeps saying the bees will come back on their own. I have stopped arguing with him about it."));
		});

		// Mikolas
		//-------------------------------------------------------------------------
		AddNpc(147485, L("Mikolas"), "SIAULIAI_46_4_SQ04_NPC02", "f_siauliai_46_4", 1093.22, 791.72, 91, async dialog =>
		{
			dialog.SetTitle(L("Mikolas"));

			await dialog.Msg(L("The brewery has my name on it and not a drop of mead left in it."));
		});

		// Honeycomb
		//-------------------------------------------------------------------------
		AddNpc(151025, L("Honeycomb"), "SIAULIAI_46_4_BEEHIVE01", "f_siauliai_46_4", 1370, -695, 30, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Honeycomb"));

			if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
			{
				var checked46 = await character.TimeActions.StartAsync(L("Checking the beehives..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (checked46 != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq01, "checkHives");
				character.ServerMessage(L("The beehives look whole. Go back and tell Darren."));
				return;
			}

			if (character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("The Sparnas is still sitting over the apiary."));
				character.Quests.ReplayQuestTrack(Sq01);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("The comb has been opened from the outside, and whatever did it is still close by."),
					Option(L("Look for what opened it"), "accept"),
					Option(L("Leave the apiary alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					character.ServerMessage(L("A Sparnas drops onto the apiary!"));
					return;
				}
				return;
			}

			await dialog.Msg(L("A comb of the Rododun Apiary, heavy and still full."));
		});

		// Mead Storage Box
		//-------------------------------------------------------------------------
		AddNpc(46212, L("Mead Storage Box"), "SIAULIAI_46_4_MEADBOX", "f_siauliai_46_4", 1172.81, 2575.27, 180, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Mead Storage Box"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				var found = await character.TimeActions.StartAsync(L("Checking the boxes..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (found != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.SIAULIAI_46_4_MQ_04_ITEM, 1, InventoryAddType.PickUp);
				character.ServerMessage(L("You found the mead Dorjen asked for."));
				return;
			}

			if (character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("The Biteregina is still working through the boxes."));
				character.Quests.ReplayQuestTrack(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("The boxes smell of honey from further off than they should, and something has been at them."),
					Option(L("Check the boxes"), "accept"),
					Option(L("Leave the warehouse"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					character.ServerMessage(L("A Biteregina came after the smell of the honey!"));
					return;
				}
				return;
			}

			await dialog.Msg(L("Boxes of the Honey Wine Warehouse, stacked to the roof and smelling of it."));
		});

		// Oil Barrels
		//-------------------------------------------------------------------------
		for (var i = 0; i < OilBarrels.GetLength(0); ++i)
		{
			AddNpc(147459, L("Oil Barrel"), i == 0 ? "SIAULIAI_46_4_MEADBARREL" : "SIAULIAI_46_4_MEADBARREL_" + (i + 1), "f_siauliai_46_4",
				OilBarrels[i, 0], OilBarrels[i, 1], 90, this.DrainOilBarrel);
		}
	}

	/// <summary>
	/// Draws the oil out of one of Dorjen's barrels.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task DrainOilBarrel(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Oil Barrel"));

		if (!character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("A barrel of lamp oil, set out where the wind would carry a fire into the trees."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.SIAULIAI_46_4_MQ_03_ITEM) >= OilBarrelsToDrain)
		{
			await dialog.Msg(L("Every barrel you could find is empty. Dorjen will have noticed by now."));
			return;
		}

		var drained = await character.TimeActions.StartAsync(L("Drawing out the oil..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

		if (drained != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.SIAULIAI_46_4_MQ_03_ITEM, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("The barrel empties into the ditch."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 16000: It's the Honey (1)
//-----------------------------------------------------------------------------
public class Siauliai464Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16000);
		SetName(L("It's the Honey (1)"));
		SetDescription(L("Darren does not know whether the Rododun Apiary survived the monsters."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_4_MQ01_NPC", "f_siauliai_46_4", L("Talk to Villager Darren"), L("Darren seems to be puzzled. Listen to him."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_4_BEEHIVE01", "f_siauliai_46_4", L("Check the beehives at Rododun Apiary"), L("Darren is worried about whether his beehives at Rododun Apiary are okay after they were attacked by monsters. Go to Rododun Apiary and check whether the beehives are okay."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_4_MQ01_NPC", "f_siauliai_46_4", L("Talk to Villager Darren"), L("Fortunately, the beehives are okay. Return to Villager Darren and let him know about this."));

		AddPrerequisite(new LevelPrerequisite(150));

		AddObjective("checkHives", L("Check the beehives at Rododun Apiary"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 16010: It's the Honey (2)
//-----------------------------------------------------------------------------
public class Siauliai464Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16010);
		SetName(L("It's the Honey (2)"));
		SetDescription(L("An empty beehive off a sweet-smelling monster would draw the rest of them off the apiary."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_4_MQ01_NPC", "f_siauliai_46_4", L("Talk to Villager Darren"), L("The beehives at Rododun Apiary are okay, but we should do something to them. Talk to the relieved Villager Darren."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_4_MQ01_NPC", "f_siauliai_46_4", L("Defeat the monsters with sweet fragrance."), L("Villager Darren told you that the monsters are going after the apiary's honey so he wants to use empty beehives to deceive the monsters. Defeat a monster with sweet fragrance and get an empty beehive."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_4_MQ01_NPC", "f_siauliai_46_4", L("Hand over the empty beehive to Villager Darren"), L("Acquired an empty beehive. Hand over the beehive to Villager Darren."));

		AddPrerequisite(new QuestStatusPrerequisite(16000, QuestStatus.Completed));

		AddObjective("findHive", L("Obtain an Empty Beehive from a monster with sweet fragrance"), new CollectItemObjective("SIAULIAI_46_4_MQ_02_ITEM", 1));

		AddPityDrop("SIAULIAI_46_4_MQ_02_ITEM", 0.3f, 7, 1, "Siaulamb", "Pendinmire", "lantern_mushroom_orange", "rabbee", "Honeybean");

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new TakeItemReward("SIAULIAI_46_4_MQ_02_ITEM"));
	}
}

// 16020: Ruined Brewery
//-----------------------------------------------------------------------------
public class Siauliai464Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16020);
		SetName(L("Ruined Brewery"));
		SetDescription(L("Dorjen has set oil barrels out around Micolas Brewery, and means to use them."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_4_MQ01_NPC", "f_siauliai_46_4", L("Talk to Villager Darren"), L("Villager Darren seems to have more to say. Listen to what he has to say."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_4_MEADBARREL", "f_siauliai_46_4", L("Draw out the oil from the oil barrels"), L("Villager Darren suspects Brewer Dorjen is going to burn down the forest in anger towards the monsters. Before you pacify Dorjen, draw out the oil from the barrels near Micolas Brewery."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_4_MQ04_NPC", "f_siauliai_46_4", L("Talk to Brewer Dorjen"), L("You've drawn out all the oil from the oil barrels you found. Talk to Brewer Dorjen."));

		AddPrerequisite(new QuestStatusPrerequisite(16010, QuestStatus.Completed));

		AddObjective("drainOil", L("Remove the Lamp Oil"), new CollectItemObjective("SIAULIAI_46_4_MQ_03_ITEM", 6));

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new TakeItemReward("SIAULIAI_46_4_MQ_03_ITEM"));
	}
}

// 16030: Brewer's Last Hope
//-----------------------------------------------------------------------------
public class Siauliai464Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16030);
		SetName(L("Brewer's Last Hope"));
		SetDescription(L("Dorjen will hear nothing until his signature mead is back out of the Honey Wine Warehouse."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_4_MQ04_NPC", "f_siauliai_46_4", L("Talk to Brewer Dorjen"), L("Brewer Dorjen is angry because he was stopped from burning the forest. Try to calm Dorjen down."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_4_MEADBOX", "f_siauliai_46_4", L("Bring back Dorjen's Signature Mead from the Honey Wine Warehouse"), L("Brewer Dorjen can't calm himself down and told you to bring back his signature mead from the Honey Wine Warehouse."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_4_MQ04_NPC", "f_siauliai_46_4", L("Talk to Brewer Dorjen"), L("You've brought the mead in good condition. Hand it over to Dorjen."));

		AddPrerequisite(new QuestStatusPrerequisite(16020, QuestStatus.Completed));

		AddObjective("findMead", L("Obtain Dorjen's Signature Mead"), new CollectItemObjective("SIAULIAI_46_4_MQ_04_ITEM", 1));

		AddReward(new ItemReward("expCard9", 1));
		AddReward(new TakeItemReward("SIAULIAI_46_4_MQ_04_ITEM"));
	}
}

// 16040: Sweet Revenge
//-----------------------------------------------------------------------------
public class Siauliai464Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16040);
		SetName(L("Sweet Revenge"));
		SetDescription(L("Dorjen will settle for the monsters that ruined the brewery being put down instead."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_4_MQ04_NPC", "f_siauliai_46_4", L("Talk to Brewer Dorjen"), L("Brewer Dorjen seems to trust you a lot now. Listen to what he has to say."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_4_MQ04_NPC", "f_siauliai_46_4", L("Defeat the monsters who ruined Dorjen's brewery"), L("Brewer Dorjen told you that if you could defeat the monsters that ruined his brewery, his anger may subside. Exact revenge on the monsters for Dorjen."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_4_MQ04_NPC", "f_siauliai_46_4", L("Talk to Brewer Dorjen"), L("You've defeated the monsters as requested by Dorjen. Let Dorjen know about it."));

		AddPrerequisite(new QuestStatusPrerequisite(16030, QuestStatus.Completed));

		AddObjective("killRaiders", L("Defeat the monsters who ruined the brewery business"), new KillObjective(15, "Siaulamb", "Pendinmire", "lantern_mushroom_orange"));

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 16100: Apiary-invader Sparnas
//-----------------------------------------------------------------------------
public class Siauliai464Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16100);
		SetName(L("Apiary-invader Sparnas"));
		SetDescription(L("A Sparnas has taken the Rododun Apiary for itself."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_4_BEEHIVE01", "f_siauliai_46_4", L("Check the beehives at Rododun Apiary"), L("Check the beehives at Rododun Apiary."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_4_BEEHIVE01", "f_siauliai_46_4", L("Defeat Sparnas"), L("Defeat the Sparnas that occupied the Rododun Apiary."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_4_BEEHIVE01", "f_siauliai_46_4", L("Defeat Sparnas"), L("Defeat the Sparnas that occupied the Rododun Apiary."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_4_SQ_01_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(150));

		AddObjective("killSparnas", L("Defeat Sparnas"), new KillObjective(1, "boss_sparnas_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Sparnas is down and the apiary is quiet."));
		character.Quests.Complete(this.QuestId);
	}
}

// 16110: Honey-eating Biteregina
//-----------------------------------------------------------------------------
public class Siauliai464Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16110);
		SetName(L("Honey-eating Biteregina"));
		SetDescription(L("A Biteregina has followed the smell of the Honey Wine Warehouse in."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_4_MEADBOX", "f_siauliai_46_4", L("Check the mead storage boxes"), L("You can smell some sweet fragrance from somewhere. Check the boxes with sweet fragrance."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_4_MEADBOX", "f_siauliai_46_4", L("Defeat Biteregina"), L("It seems that a Biteregina came after the smell of honey. Defeat Biteregina."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_4_MEADBOX", "f_siauliai_46_4", L("Defeat Biteregina"), L("It seems that a Biteregina came after the smell of honey. Defeat Biteregina."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_4_SQ_02_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(150));

		AddObjective("killBiteregina", L("Defeat Biteregina"), new KillObjective(1, "boss_BiteRegina_Q4") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Biteregina is down. What is left of the mead is safe."));
		character.Quests.Complete(this.QuestId);
	}
}

// 16120: Dislike for Danger
//-----------------------------------------------------------------------------
public class Siauliai464Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16120);
		SetName(L("Dislike for Danger"));
		SetDescription(L("Kirina followed Darren out to the farm and cannot see a way back through the monsters."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_4_SQ03_NPC", "f_siauliai_46_4", L("Talk to Kirina"), L("Kirina seems to be afraid of something. Ask her what happened."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_4_SQ03_NPC", "f_siauliai_46_4", L("Defeat the monsters nearby"), L("Kirina is worried about the way back to the village. To make her comfortable, defeat the monsters nearby."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_4_SQ03_NPC", "f_siauliai_46_4", L("Talk to Kirina"), L("You've defeated enough monsters nearby. Return to Kirina and soothe her."));

		AddPrerequisite(new LevelPrerequisite(150));

		AddObjective("clearTheRoad", L("Defeat the monsters nearby"), new KillObjective(20, "rabbee", "Honeybean"));

		AddReward(new ItemReward("expCard9", 1));
	}
}
