//--- Melia Script ----------------------------------------------------------
// Srautas Gorge Quest NPCs
//--- Description -----------------------------------------------------------
// The Watchers, the cable car parts and the Pantos the map's quests run on.
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

public class FGele571QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(17100);
	private readonly static QuestId Mq02 = new QuestId(17110);
	private readonly static QuestId Mq03 = new QuestId(17120);
	private readonly static QuestId Mq04 = new QuestId(17130);
	private readonly static QuestId Mq05 = new QuestId(17140);
	private readonly static QuestId Mq06 = new QuestId(17150);
	private readonly static QuestId Mq07 = new QuestId(17160);
	private readonly static QuestId Rp1 = new QuestId(60151);
	private readonly static QuestId ToGele = new QuestId(50006);

	protected override void Load()
	{
		// Watcher Gilbert
		//-------------------------------------------------------------------------
		AddNpc(147406, L("Watcher Gilbert"), "GELE571_NPC_GILBERT", "f_gele_57_1", -352, -564, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Gilbert"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("As I'm the only one who can fix the cable car, I'll take a look."));
				await dialog.Msg(L("Oh, of course, thanks for your help."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Well done."));
				await dialog.Msg(L("Now attach this and grease it, then it should work correctly."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Good work. These are the parts the Pantos stole."));
				await dialog.Msg(L("I never imagined I'd be seeing them again like this."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Look at this busted cable car."));
				var answer = await dialog.SelectQuestOffer(Mq01, L("It's still working, but the damage is pretty serious."),
					Option(L("I'll teach them a lesson"), "accept"),
					Option(L("About the Watchers"), "explain"),
					Option(L("Leave if for him to do it himself"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("This place has a very sacred meaning to us."));
					await dialog.Msg(L("And we have a calling as Watchers, to protect this place."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("The Grummers and Zignuts have been biting at the ropes again. Thin them out."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("It's the Pantos again."));
				await dialog.Msg(L("They even stole the working parts."));
				var answer = await dialog.SelectQuestOffer(Mq02, L("Well then, we've got to take them back, right?"),
					Option(L("I'll get it to you"), "accept"),
					Option(L("About repairing the cable car"), "explain"),
					Option(L("I don't want to"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("In the past, this cable car was just a rope with a basket."));
					await dialog.Msg(L("You had to put your life on the line whenever you crossed it."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("The Pantos hid some parts in the grasslands of Mieguista Slope."));
				var answer = await dialog.SelectQuestOffer(Mq03, L("Try to get me those for the time being since it's urgent."),
					Option(L("I'll bring it"), "accept"),
					Option(L("I don't have time"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq03);

				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The Grummers and Zignuts are still out there. The cable car can't wait on them."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The Pantos won't hand the gears back on their own. Go and take them."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Search the grass at Mieguista Slope. The latches are small and easy to miss."));
				return;
			}

			await dialog.Msg(L("The cable car holds, barely. It will not hold forever."));
		});

		// Watcher Matthew
		//-------------------------------------------------------------------------
		AddNpc(147421, L("Watcher Matthew"), "GELE571_NPC_MATTHEW", "f_gele_57_1", -401, -642, 74, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Matthew"));

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Alright. Thank you very much!"));
				await dialog.Msg(L("Do you see that damaged cable car over there? I used to ride that when I was a kid."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("Thank you! You've saved me and my friends from spending more than half of our lives just protecting the cable car."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("I am looking for cable car parts, but I can't seem to find any."));
				var answer = await dialog.SelectQuestOffer(Mq05, L("Maybe the Zignuts at Nepavy Grassland swallowed them."),
					Option(L("I'll find it for you"), "accept"),
					Option(L("About the Holy Land"), "explain"),
					Option(L("That's too bad (leave)"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("This is a sacred land which we have served for generations."));
					await dialog.Msg(L("We used to hold prayer and important meetings here whenever there was a disaster."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("I worked very hard to fix the cable car, but I'm still worried about the Poata."));
				var answer = await dialog.SelectQuestOffer(Mq06, L("A monster of that size could destroy the cable car."),
					Option(L("I'll go and hunt the Poata"), "accept"),
					Option(L("About the cable car"), "explain"),
					Option(L("Don't worry. That will never happen"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("You need to use the cable car to go from Srautas Gorge to Gele Plateau."));
					await dialog.Msg(L("I heard people used to hang baskets on ropes in the past, and use those to traverse the gorge."));
					return;
				}

				if (answer == "accept")
				{
					await dialog.Msg(L("Soil the Poata's nest at Margas Hill, and it will show up."));
					character.Quests.Start(Mq06);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("The Zignuts swallow everything. Search the grassland and you'll find the parts."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The Poata's nest is at Margas Hill. Soil it and it will come out."));
				character.Quests.ClearQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("The cable car has to cross the gorge again. One way or another."));
		});

		// Watcher Molly
		//-------------------------------------------------------------------------
		AddNpc(147423, L("Watcher Molly"), "GELE571_NPC_MARLEY", "f_gele_57_1", -258, 284, 123, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Molly"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Look at these gentle Pantos."));
				await dialog.Msg(L("It's insane that we have to make the Pantos our enemies when the demons are charging in."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("It got angry as soon as it saw the Baby Pantos? So the plan really did not work."));
				await dialog.CompleteQuest(Mq07);
				return;
			}

			if (character.Quests.IsActive(Rp1) && character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("Did you destroy the roots, too?"));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("The Pantos were not always such violent monsters."));
				var answer = await dialog.SelectQuestOffer(Mq04, L("There must be a way to change them back."),
					Option(L("I will try"), "accept"),
					Option(L("About the Pantos"), "explain"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("The Pantos changed and became wild after Medzio Diena, four years ago."));
					await dialog.Msg(L("Before that, we didn't really mind each other and even joked around together."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					character.Inventory.Add(650584, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Take these sugar beets and lure the Baby Pantos with them."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				var answer = await dialog.SelectQuestOffer(Mq07, L("If you can persuade Capria, the Pantos should be tamed. Capria is the leader of the Pantos."),
					Option(L("I'm not sure but I'll give it a shot"), "accept"),
					Option(L("About the Capri"), "explain"),
					Option(L("Seems like a dangerous plan"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Capria was the first to change after the incident four years ago."));
					await dialog.Msg(L("And then the Pantos... now only the baby monsters are left."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq07);
					await dialog.Msg(L("Take the Baby Pantos to Capria. Maybe it will listen."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				var answer = await dialog.SelectQuestOffer(Rp1, L("The Pantos must have eaten something wrong to be acting like that."),
					Option(L("I'll help you"), "accept"),
					Option(L("That is not needed"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Rp1);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The Baby Pantos are gentle. Lure one and see for yourself."));
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Capria is out past the junction. Be careful - it is not the Panto you knew."));
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("The Sugar Beet stems grow thick on the plateau. Clear some of them."));
				return;
			}

			await dialog.Msg(L("The Pantos watch the cable car as closely as we do."));
		});

		// Pile of Grass
		//-------------------------------------------------------------------------
		AddNpc(47204, L("Pile of Grass"), "GELE571_MQ_03", "f_gele_57_1", -1502, 400, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Pile of Grass"));

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("You dig through the grass and find a lever handle latch the Pantos left behind."));
				character.Inventory.Add(650582, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Mq03, "findLatches");
				return;
			}

			await dialog.Msg(L("A heap of trampled grass. Nothing else is hidden here."));
		});

		// Poata's Nest
		//-------------------------------------------------------------------------
		AddNpc(47203, L("Poata's Nest"), "GELE571_MQ_05", "f_gele_57_1", 793, -1362, 126, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Poata's Nest"));

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("You soil the nest. The ground trembles - the Poata is coming."));
				var fouledIt = await character.TimeActions.StartAsync(L("Fouling the water..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

				if (fouledIt != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("A great nest of trampled reeds. Whatever sleeps here is far too large for the gorge."));
		});

		// Plateau Sugar Beet Stems
		//-------------------------------------------------------------------------
		AddNpc(47201, L("Plateau Sugar Beet Stems"), "GELE571_RP_1_OBJ", "f_gele_57_1", 1247, 626, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Plateau Sugar Beet Stems"));

			if (character.Quests.IsActive(Rp1) && !character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("You pull the sugar beet stems up by the roots and leave them to wither."));
				character.Quests.CompleteObjective(Rp1, "removeStems");
				return;
			}

			await dialog.Msg(L("Thick sugar beet stems, sweet enough to draw every Panto on the plateau."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The Baby Panto grounds, where the sugar beets are scattered.
		AddQuestTrigger("GELE571_MQ_04_LURE", "f_gele_57_1", 602, 408, 250, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
				character.Quests.CompleteObjective(Mq04, "lurePantos");

			await Task.CompletedTask;
		});

		// The clearing where Capria is lured.
		AddQuestTrigger("GELE571_MQ_07", "f_gele_57_1", 980, 961, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
			{
				var petted = await character.TimeActions.StartAsync(L("Petting the Baby Pantos..."), L("Cancel"), "PET", TimeSpan.FromSeconds(2));

				if (petted != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq07);
			}

			await Task.CompletedTask;
		});

		// The cable car that carries the player up to Gele Plateau.
		AddQuestTrigger("SOUT_Q_41_ARRIVE", "f_gele_57_1", 640, 1489, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(ToGele) && !character.Quests.IsCompletable(ToGele))
				character.Quests.CompleteObjective(ToGele, "travelToGele");

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 17100: The Fearless Ones
//-----------------------------------------------------------------------------
public class Gele571Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17100);
		SetName(L("The Fearless Ones"));
		SetDescription(L("Gilbert wants the Grummers and Zignuts that damaged the cable car taught a lesson."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Talk to Watcher Gilbert"), L("Guards in Strautas are waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Defeat Grummers and Zignuts"), L("Gilbert wants you to defeat the monsters that damaged the cable car."));
		SetPhase(QuestStatus.Success, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Talk to Watcher Gilbert"), L("Tell Gilbert you taught the monsters a lesson."));

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killGrummer", L("Defeat Grummer"), new KillObjective(4, "Grummer"));
		AddObjective("killZignuts", L("Defeat Zignuts"), new KillObjective(5, "Zignuts"));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 17110: The Parts Thief
//-----------------------------------------------------------------------------
public class Gele571Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17110);
		SetName(L("The Parts Thief"));
		SetDescription(L("The Pantos at Mieguista Hill stole the cable car's gears. Take them back."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Talk to Watcher Gilbert"), L("Guards in Strautas are waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Defeat the Pantos and retrieve the gears"), L("Defeat the Pantos to get the cable car's gears back."));
		SetPhase(QuestStatus.Success, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Talk to Watcher Gilbert"), L("Return the gears to Gilbert."));

		AddPityDrop("GELE571_MQ_02_ITEM", 0.8f, 3, 1, "Npanto_baby");

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("collectGears", L("Defeat Pantos and get gears"), new CollectItemObjective("GELE571_MQ_02_ITEM", 8));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("GELE571_MQ_02_ITEM"));
	}
}

// 17120: Finding the Lever Handle Latches
//-----------------------------------------------------------------------------
public class Gele571Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17120);
		SetName(L("Finding the Lever Handle Latches"));
		SetDescription(L("The Pantos hid cable car parts in the grass at Mieguista Slope."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Talk to Watcher Gilbert"), L("Guards in Strautas are waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Find the lever handle latches"), L("Find the lever handle latches in the grass at Mieguista Slope."));
		SetPhase(QuestStatus.Success, "GELE571_NPC_GILBERT", "f_gele_57_1", L("Talk to Watcher Gilbert"), L("Return the lever handle latches to Gilbert."));

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("findLatches", L("Find the lever handle latches"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("GELE571_MQ_01_ITEM"));
	}
}

// 17130: Lure the Baby Pantos
//-----------------------------------------------------------------------------
public class Gele571Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17130);
		SetName(L("Lure the Baby Pantos"));
		SetDescription(L("Molly believes the Pantos can be calmed, and asks you to lure a Baby Panto with sugar beets."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE571_NPC_MARLEY", "f_gele_57_1", L("Talk to Watcher Molly"), L("Guards in Strautas are waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE571_NPC_MARLEY", "f_gele_57_1", L("Give sugar beets to Baby Pantos"), L("Approach the Baby Pantos and use the sugar beets."));
		SetPhase(QuestStatus.Success, "GELE571_NPC_MARLEY", "f_gele_57_1", L("Talk to Watcher Molly"), L("Listen to Molly's next plans."));

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("lurePantos", L("Give sugar beets to Baby Pantos"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("GELE571_MQ_04_ITEM"));
	}
}

// 17140: Collecting Cable Car Parts
//-----------------------------------------------------------------------------
public class Gele571Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17140);
		SetName(L("Collecting Cable Car Parts"));
		SetDescription(L("The Zignuts at Nepavy Grassland swallowed the cable car parts Matthew needs."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE571_NPC_MATTHEW", "f_gele_57_1", L("Talk to Watcher Matthew"), L("Guards in Strautas are waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE571_NPC_MATTHEW", "f_gele_57_1", L("Retrieve Cable Car Parts"), L("Defeat the Zignuts and retrieve the parts they swallowed."));
		SetPhase(QuestStatus.Success, "GELE571_NPC_MATTHEW", "f_gele_57_1", L("Talk to Watcher Matthew"), L("Hand the cable car parts to Matthew."));

		AddPityDrop("GELE571_MQ_05_ITEM", 0.65f, 4, 1, "Zignuts");

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("collectParts", L("Retrieve the cable car parts from Zignuts"), new CollectItemObjective("GELE571_MQ_05_ITEM", 7));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("GELE571_MQ_05_ITEM"));
	}
}

// 17150: Tyrant of the Srautas Gorge
//-----------------------------------------------------------------------------
public class Gele571Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17150);
		SetName(L("Tyrant of the Srautas Gorge"));
		SetDescription(L("Soil the Poata's nest at Margas Hill and defeat the beast before it wrecks the cable car."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE571_NPC_MATTHEW", "f_gele_57_1", L("Talk to Watcher Matthew"), L("Guards in Strautas are waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE571_MQ_05", "f_gele_57_1", L("Defeat Poata who is a threat to the cable car"), L("Soil the Poata's nest at Margas Hill to lure it out."));
		SetPhase(QuestStatus.Success, "GELE571_NPC_MATTHEW", "f_gele_57_1", L("Talk to Watcher Matthew"), L("Tell Matthew the Poata is dead."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE571_MQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killPoata", L("Defeat Poata"), new KillObjective(1, "boss_poata_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
	}
}

// 17160: Stubborness
//-----------------------------------------------------------------------------
public class Gele571Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17160);
		SetName(L("Stubborness"));
		SetDescription(L("Molly wants to try persuading Capria with the Baby Pantos. It may not work."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE571_NPC_MARLEY", "f_gele_57_1", L("Talk to Watcher Molly"), L("Watcher Molly is looking for someone brave to help her great plan."));
		SetPhase(QuestStatus.InProgress, "GELE571_MQ_07", "f_gele_57_1", L("Take the Baby Pantos and find Capria"), L("Bring the Baby Pantos to Capria and try to persuade it."));
		SetPhase(QuestStatus.Success, "GELE571_NPC_MARLEY", "f_gele_57_1", L("Talk to Watcher Molly"), L("Tell Molly that peace in Srautas does not seem possible."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE571_MQ_07_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(17130, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killCapria", L("Defeat Capria"), new KillObjective(1, "boss_capria") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 3));
		AddReward(new ItemReward("HAND02_117", 1));
	}
}

// 60151: Justified Suspicion
//-----------------------------------------------------------------------------
public class Gele571Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60151);
		SetName(L("Justified Suspicion"));
		SetDescription(L("Molly blames the Plateau Sugar Beets for the Pantos' strange behaviour."));
		SetType(QuestType.Repeat);
		SetLocation("f_gele_57_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE571_NPC_MARLEY", "f_gele_57_1", L("Talk to Watcher Molly"), L("Watcher Molly at Srautas Gorge is waiting for your help."));
		SetPhase(QuestStatus.InProgress, "GELE571_RP_1_OBJ", "f_gele_57_1", L("Remove the Plateau Sugar Beet Stems"), L("Get rid of the sugar beet stems Molly suspects are corrupting the Pantos."));
		SetPhase(QuestStatus.Success, "GELE571_NPC_MARLEY", "f_gele_57_1", L("Talk to Watcher Molly"), L("Report back to Watcher Molly."));

		AddPrerequisite(new QuestStatusPrerequisite(17150, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("removeStems", L("Remove the Plateau Sugar Beet Stems"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 2));
	}
}
