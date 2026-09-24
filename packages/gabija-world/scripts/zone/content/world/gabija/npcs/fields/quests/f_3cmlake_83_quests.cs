//--- Melia Script ----------------------------------------------------------
// Pelke Shrine Ruins Quest NPCs
//--- Description -----------------------------------------------------------
// Elder Eloizard, his granddaughter and the villagers searching for the
// cause of the red water.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class F3Cmlake83QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Katyn12Mq10 = new QuestId(30069);
	private readonly static QuestId Mq01 = new QuestId(90001);
	private readonly static QuestId Mq02 = new QuestId(90002);
	private readonly static QuestId Mq03 = new QuestId(90003);
	private readonly static QuestId Mq04 = new QuestId(90004);
	private readonly static QuestId Mq05 = new QuestId(90005);
	private readonly static QuestId Mq06 = new QuestId(90019);
	private readonly static QuestId Sq01 = new QuestId(90006);
	private readonly static QuestId Sq02 = new QuestId(90007);
	private readonly static QuestId Sq03 = new QuestId(90008);
	private readonly static QuestId Sq04 = new QuestId(90009);
	private readonly static QuestId Hq1 = new QuestId(50273);
	private readonly static QuestId Rp1 = new QuestId(60166);
	private readonly static QuestId Lake84Mq06 = new QuestId(90015);
	private readonly static QuestId Lake84Sq03 = new QuestId(90018);

	public const string OfferingCountVar = "Gabija.Quests.Lake83Sq02.Offerings";
	private const string OfferingVar = "Gabija.Quests.Lake83Sq02.Obelisk";
	private const string BadgeVar = "Gabija.Quests.Lake83Hq1.Badge";
	private const string DiscVar = "Gabija.Quests.Lake83Mq04.Disc";

	private const int FragmentsNeeded = 4;
	private const float DiscRange = 150;

	private static readonly string[] LakeMonsters = { "Rajatadpole", "Sec_merog_wogu", "Sec_merog_wizzard" };

	private static readonly double[,] Obelisks =
	{
		{ -1554.35, -1099.72 }, { -2450, -488.25 },
	};

	private static readonly double[,] Badges =
	{
		{ -1798.02, -566.93 }, { -1632.19, -709.13 }, { -1396.78, -591.64 }, { -1359.32, -353.82 }, { -1673.98, -356.67 },
	};

	protected override void Load()
	{
		// Elder Eloizard, at the edge of the shrine ruins
		//-------------------------------------------------------------------------
		AddConditionalNpc(152002, L("Elder Eloizard"), "3CMLAKE_83_OLDMAN1", "f_3cmlake_83", 815.88, -144.94, 217, c => !c.Quests.HasCompleted(Mq01), this.ElderAtRuins);

		// Elder Eloizard, at the Collapsed Hall Lot
		//-------------------------------------------------------------------------
		AddConditionalNpc(152002, L("Elder Eloizard"), "3CMLAKE_83_OLDMAN2", "f_3cmlake_83", -197.33, 143.66, 90, IsElderAtCamp, this.ElderAtCamp);

		// Elder Eloizard, near the Anga Hall
		//-------------------------------------------------------------------------
		AddConditionalNpc(152002, L("Elder Eloizard"), "3CMLAKE_83_OLDMAN3", "f_3cmlake_83", -1073.38, -776.44, 134, IsElderAtAngaHall, this.ElderAtAngaHall);

		AddConditionalNpc(147482, L("Town Youth"), "3CMLAKE_83_PEOPLE4", "f_3cmlake_83", -1109.84, -748.97, 90, IsElderAtAngaHall, async dialog =>
		{
			dialog.SetTitle(L("Town Youth"));
			await dialog.Msg(L("Thank you so much for saving me. I should have stopped our chief... It was my mistake."));
		});

		AddConditionalNpc(147482, L("Town Youth"), "3CMLAKE_83_PEOPLE6", "f_3cmlake_83", -37.29, 129.69, 346, c => c.Quests.Has(Mq04) || c.Quests.HasCompleted(Mq04), async dialog =>
		{
			dialog.SetTitle(L("Town Youth"));

			if (dialog.Player.Quests.HasCompleted(Lake84Mq06))
			{
				await dialog.Msg(L("The water should go back to normal now, right?"));
				await dialog.Msg(L("That red water, the first time I saw it I was terrified..."));
				return;
			}

			await dialog.Msg(L("I safely arrived thanks to you. I thank you again."));
		});

		// Elder's Granddaughter
		//-------------------------------------------------------------------------
		AddNpc(147473, L("Elder's Granddaughter"), "3CMLAKE_83_LADY", "f_3cmlake_83", -164.97, 169.77, 350, this.Granddaughter);

		// Samsonas
		//-------------------------------------------------------------------------
		AddConditionalNpc(147481, L("Samsonas"), "3CMLAKE_83_PEOPLE1", "f_3cmlake_83", -864.43, 1060.24, 275, c => !c.Quests.HasCompleted(Sq02), this.Samsonas);

		AddConditionalNpc(147481, L("Samsonas"), "3CMLAKE_83_PEOPLE8", "f_3cmlake_83", -248.97, -109.74, 142, c => c.Quests.HasCompleted(Sq02), async dialog =>
		{
			dialog.SetTitle(L("Samsonas"));

			if (dialog.Player.Quests.HasCompleted(Lake84Mq06))
			{
				await dialog.Msg(L("Oh... So it was the gem on the Hydra that turned our water red."));
				await dialog.Msg(L("What could this gem be, to cause so much trouble...?"));
				return;
			}

			await dialog.Msg(L("I'm telling you, I saw people praying."));
			await dialog.Msg(L("They wore black hoods... I really saw them..."));
		});

		// Nikodemas
		//-------------------------------------------------------------------------
		AddNpc(20117, L("Nikodemas"), "3CMLAKE_83_PEOPLE2", "f_3cmlake_83", -22.54, -106.83, 215, this.Nikodemas);

		// Scalvis
		//-------------------------------------------------------------------------
		AddNpc(20118, L("Scalvis"), "3CMLAKE_83_PEOPLE3", "f_3cmlake_83", -130.09, -111.93, 152, this.Scalvis);

		// Napalis, hiding at the Drava Chapel Lot
		//-------------------------------------------------------------------------
		AddConditionalNpc(147483, L("Napalis"), "3CMLAKE_83_PEOPLE5", "f_3cmlake_83", -1267.34, 1447.37, 346, c => c.Quests.Has(Sq03) && !c.Quests.HasCompleted(Sq03), this.NapalisHiding);

		AddConditionalNpc(147483, L("Napalis"), "3CMLAKE_83_PEOPLE7", "f_3cmlake_83", -309.70, -45.75, 100, c => c.Quests.HasCompleted(Sq03), async dialog =>
		{
			dialog.SetTitle(L("Napalis"));
			await dialog.Msg(L("Just a while ago here I was, thinking I'm done for, unable to escape..."));
			await dialog.Msg(L("Thinking about it, our chief must be a really brave man..."));
		});

		AddNpc(157000, "UnvisibleName", "3CMLAKE_83_RUINS", "f_3cmlake_83", -1283.54, 1411.73, 318);

		// Mysterious Obelisks
		//-------------------------------------------------------------------------
		AddNpc(147414, L("Mysterious Obelisk"), "3CMLAKE_83_OBELISK", "f_3cmlake_83", -892.18, 1045.04, 84);

		for (var i = 0; i < Obelisks.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(147414, L("Mysterious Obelisk"), "3CMLAKE_83_OBELISK" + number, "f_3cmlake_83", Obelisks[i, 0], Obelisks[i, 1], 90, async dialog =>
			{
				this.MakeOffering(dialog.Player, number, dialog.Npc);
				await Task.CompletedTask;
			});
		}

		// The laboratory in the Wandering Sanctuary
		//-------------------------------------------------------------------------
		AddConditionalNpc(153132, "UnvisibleName", "3CMLAKE_83_WORKBENCH1", "f_3cmlake_83", -46.96, 859.67, 294, c => !c.Quests.Has(Lake84Sq03) && !c.Quests.HasCompleted(Lake84Sq03), this.Workbench);

		AddNpc(57013, "UnvisibleName", "3CMLAKE_83_OBJ1", "f_3cmlake_83", -115.27, 787.02, 249);
		AddNpc(153133, "UnvisibleName", "3CMLAKE_83_OBJ2", "f_3cmlake_83", -56.76, 655.28, 317);
		AddNpc(153131, "UnvisibleName", "3CMLAKE_83_OBJ3", "f_3cmlake_83", -35.61, 795.18, 90);
		AddNpc(153131, "UnvisibleName", "3CMLAKE_83_OBJ4", "f_3cmlake_83", -37.70, 753.72, 277);

		// Anga Hall and the Drava Chapel Lot
		//-------------------------------------------------------------------------
		AddQuestTrigger("3CMLAKE_83_ENTER1", "f_3cmlake_83", -986.14, -647.25, 200, async args =>
		{
			if (args.Initiator is Character character && character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
				character.Quests.StartQuestTrack(Mq03);

			await Task.CompletedTask;
		});

		AddQuestTrigger("3CMLAKE_83_ENTER2", "f_3cmlake_83", -1509.98, 652.49, 150, async args =>
		{
			if (args.Initiator is Character character && character.Quests.IsActive(Sq03) && !character.Quests.IsCompletable(Sq03))
				character.Quests.StartQuestTrack(Sq03);

			await Task.CompletedTask;
		});

		// The lost badges
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Badge"), "F3CMLAKE83_HIDDEN_PRECHECK", "f_3cmlake_83", -1609.95, -239.16, 90, IsBadgeOnGround, async dialog =>
		{
			var character = dialog.Player;

			if (!IsBadgeOnGround(character))
				return;

			character.Inventory.Add(ItemId.F3CMLAKE83_HIDDENQ1_ITEM1, 1, InventoryAddType.PickUp);
			character.ServerMessage(L("Someone's badge. Go to the Pelke Shrine Ruins and find its owner."));
			character.LookAround();

			await Task.CompletedTask;
		});

		for (var i = 0; i < Badges.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(147469, L("Friendship Badge"), "F3CMLAKE83_HIDDEN_OBJ" + number, "f_3cmlake_83", Badges[i, 0], Badges[i, 1], 90,
				character => IsBadgeLost(character, number),
				async dialog =>
				{
					var character = dialog.Player;

					if (!IsBadgeLost(character, number))
						return;

					character.Variables.Perm.Set(BadgeVar + number, true);
					character.Inventory.Add(ItemId.F3CMLAKE83_HIDDENQ1_ITEM2, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}
	}

	/// <summary>
	/// Elder Eloizard's dialog at the edge of the shrine ruins.
	/// </summary>
	private async Task ElderAtRuins(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Elder Eloizard"));

		if (character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("Yes, yes. You're much better than the youngsters in our village."));
			await dialog.Msg(L("Seems like you're no nobody, hey? The young ones in our village can't kill monsters that easily."));
			await dialog.Msg(L("Hm? What's this trembling? Something's going on, for sure."));
			await dialog.Msg(L("The stream in our village, all our water is bright red. Drinking the water makes people sick as a dog..."));
			await dialog.Msg(L("Medzio Diena was hard enough on us, now this happens."));
			await dialog.Msg(L("You there. I could use a little help from you. The young ones in our village do nothing but give excuses to keep me out of their plans."));
			await dialog.Msg(L("They wouldn't even know about the reservoir if it wasn't for me! But, see, if someone talented like you can take me there... I can teach them not to disrespect their village chief."));
			await dialog.Msg(L("What do you say? I'll make it up to you, eh? Come to the Collapsed Hall Lot if you agree."));
			await dialog.CompleteQuest(Mq01);

			if (character.Quests.HasCompleted(Mq01))
				character.LookAround();
			return;
		}

		if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
		{
			await dialog.Msg(L("Hey, you. If you're going inside the shrine, can you give me a hand?"));
			await dialog.Msg(L("I need to go to where the villagers are but... I don't know, there's just so many monsters here. It was hard enough coming all the way up here."));
			await dialog.Msg(L("And those no-good youngsters won't even come out to help an old man."));
			await dialog.Msg(L("So you left me out, is it? You think you can sort out this red water problem all by yourselves? Who do you take the village chief for? You lousy youngsters."));

			var answer = await dialog.SelectQuestOffer(Mq01, L("Anyway, since it came to this... If your skills are okay, will you take down some monsters?"),
				Option(L("I'll help you"), "accept"),
				Option(L("I don't think I'm skilled enough for that"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Mq01);
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("Youngsters these days don't respect their elders anymore. What can they do by themselves?"));
			await dialog.Msg(L("Where's the respect for the village chief?"));
			return;
		}

		await dialog.Msg(L("The water's bright red, we can't drink it."));
		await dialog.Msg(L("And those darned youngsters don't even come out to help an old man."));
	}

	/// <summary>
	/// Elder Eloizard's dialog at the Collapsed Hall Lot.
	/// </summary>
	private async Task ElderAtCamp(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Elder Eloizard"));

		if (character.Quests.IsCompletable(Mq06))
		{
			await dialog.Msg(L("Go on, my granddaughter is waiting for you. I told her you're the one to count on."));
			await dialog.CompleteQuest(Mq06);
			return;
		}

		if (character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("So the disc was pointing to this red statue? Hm... I've never seen this before."));
			await dialog.Msg(L("Let me take a closer look at it, then. Who knows what else I'll be able to find."));
			await dialog.Msg(L("I should go to the Absenta Reservoir. I remember Modis once said that there was a group of people in black hoods headed there."));
			await dialog.Msg(L("Go see my granddaughter before you come along. She said something about a diary...?"));
			await dialog.CompleteQuest(Mq04);

			if (character.Quests.HasCompleted(Mq04))
				character.LookAround();
			return;
		}

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			await dialog.Msg(L("Ah, yes. You came. You're here to help me, yes?"));
			await dialog.Msg(L("I knew they'd be like this. The ground shakes a little, they start to panic. Asking why I even bothered to come here and such. Eh..."));
			await dialog.Msg(L("First, go see my granddaughter. I told her good things about you; I'm counting on you."));

			character.Quests.Start(Mq06);
			character.Quests.CompleteObjective(Mq06, "meetElder");
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("I don't know what they did here, or who did it, but we can't let this go."));
			await dialog.Msg(L("Sigh, it's tough..."));
			return;
		}

		await dialog.Msg(L("I... I don't trust what these youngsters are doing. No, sir."));
		await dialog.Msg(L("I know I can find a way if they would just let me, but they want to keep me out of it."));
	}

	/// <summary>
	/// Elder Eloizard's dialog near the Anga Hall.
	/// </summary>
	private async Task ElderAtAngaHall(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Elder Eloizard"));

		if (character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("Huh, what do you know. I owe you another one, eh? Thank you, thank you."));
			await dialog.Msg(L("But this I know for sure. Something big is happening at the Wandering Sanctuary, I tell you."));
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			await dialog.Msg(L("This is it, this disc. You know how the ground trembled earlier?"));
			await dialog.Msg(L("The disc suddenly started glowing red so I followed it all the way here. I was chased away by monsters, but I clearly saw that it was pointing to something."));
			await dialog.Msg(L("I'm going to give the disc to you; will you check where it's pointing to?"));

			var answer = await dialog.SelectQuestOffer(Mq04, L("My friend here got hurt so we're going to head back together."),
				Option(L("I will check it"), "accept"),
				Option(L("I can't see any more accidents"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);

				if (character.Inventory.CountItem(ItemId.F_3CMLAKE_83_MQ_ITEM1) == 0)
					character.Inventory.Add(ItemId.F_3CMLAKE_83_MQ_ITEM1, 1, InventoryAddType.PickUp);

				await dialog.Msg(L("My friend got hurt trying to protect me so we should stick together. Please do me this favor."));
				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("You saved my life. Thank you, really."));
			character.Quests.ClearQuestTrack(Mq03);
			return;
		}

		await dialog.Msg(L("You saved my life. Thank you, really."));
		await dialog.Msg(L("But didn't you find something useful because of me? We're even, then."));
	}

	/// <summary>
	/// The Elder's Granddaughter's dialog.
	/// </summary>
	private async Task Granddaughter(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Elder's Granddaughter"));

		if (character.Quests.IsCompletable(Hq1))
		{
			await dialog.Msg(L("You found them all. Thank you so much."));
			await dialog.CompleteQuest(Hq1);
			return;
		}

		if (character.Quests.IsCompletable(Rp1))
		{
			await dialog.Msg(L("Thank you! We should collect as much as we can until everyone has recovered."));
			await dialog.CompleteQuest(Rp1);
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			await dialog.Msg(L("Ugh, grandpa... How many times have I told you to stay at home? It's dangerous to go out on your own..."));
			await dialog.Msg(L("Thank you for looking after my grandfather. I heard you're very talented, is it? Anyway, if it's you... Hm..."));
			await dialog.Msg(L("We live in a village down from here. The water here runs all the way down to our village, you see."));
			await dialog.Msg(L("One day, our crops started to grow a lot bigger than usual. Of course the village thought it was a blessing from the goddesses but... that didn't last long."));
			await dialog.Msg(L("The crops were inedible. Every single one of them. The problem wasn't the taste, but everyone who ate them ended up bedridden, it was chaos."));
			await dialog.Msg(L("On top of that, all the water in the village turned red. And just like the crops... No one could drink it anymore."));
			await dialog.Msg(L("Eventually some people left for Orsha, thinking the village had been cursed... Those who stayed decided to follow the river and ended up here."));
			await dialog.Msg(L("But even among all of us, we have no idea what to do. People are starting to grow tired, too..."));
			await dialog.Msg(L("My grandfather praised you to the skies, he's really impressed by your talent. That's why I'm asking you. Please help our village."));

			var answer = await dialog.SelectQuestOffer(Mq02, L("I'm sure you'll be able to find a solution if you just use some of your power."),
				Option(L("I will help"), "accept"),
				Option(L("I don't think I'm skilled enough to solve a problem like that"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq02);
				character.Quests.CompleteObjective(Mq02, "investigate");

				await dialog.Msg(L("Thank you for being so helpful. The first thing I want to ask of you is... to investigate the Wandering Sanctuary."));
				await dialog.Msg(L("Not that long ago my grandfather went out there by himself. He ended up surrounded by monsters and... It was a close call."));
				await dialog.Msg(L("There were a few objects strewn around that I'd never seen before. I think my grandfather was there to investigate them."));
			}
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			await dialog.Msg(L("You're back. Did you see anything odd?"));
			await dialog.Msg(L("This... looks like a diary. I'll try and save whatever parts I can make out."));

			var answer = await dialog.SelectQuestOffer(Mq03, L("Meanwhile, could you look for my grandfather? The ground trembled all of a sudden and he went with some people from the village."),
				Option(L("I'll find the village chief"), "accept"),
				Option(L("Wait a minute."), "leave")
			);

			if (answer == "accept")
			{
				character.Inventory.Remove(ItemId.F_3CMLAKE_83_MQ_ITEM3, 1, InventoryItemRemoveMsg.Given);
				character.Quests.Start(Mq03);
				character.LookAround();

				await dialog.Msg(L("It's close to the Anga Hall. I'm glad you're not going out there on your own, but I have a bad feeling about this."));
			}
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			await dialog.Msg(L("You went out there without me again, did you? We could have gone together if you'd just waited a little while."));
			await dialog.Msg(L("I'm going to give you the diary, but first let me explain what it says."));

			var answer = await dialog.SelectQuestOffer(Mq05, L("Just take note of this and go find my grandfather right away."),
				Option(L("Read the diary to me"), "accept"),
				Option(L("Read it to me later"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq05);
				character.Quests.CompleteObjective(Mq05, "followElder");

				if (character.Inventory.CountItem(ItemId.F_3CMLAKE_83_MQ_05_BOOK) == 0)
					character.Inventory.Add(ItemId.F_3CMLAKE_83_MQ_05_BOOK, 1, InventoryAddType.PickUp);

				await dialog.Msg(L("...experiment succeeded... ... ... results... protecting vessel is alive... ...vina's power... Reservoir... spread... contaminate..."));
				await dialog.Msg(L("...Seir Rainforest... Novaha Mona... ... ... annihilate... ...proceed with the pla..."));
				await dialog.Msg(L("Sigh, it wasn't easy. This is all I was able to recover. Almost all the pages are completely burnt."));
				await dialog.Msg(L("All I understand is that... Some sort of power spread throughout the reservoir. I wonder if that's why the water turned red, but I can't be sure..."));
				await dialog.Msg(L("The vessel is alive... This part sounds strange, too. I have no idea about the rest. Any guesses?"));
				await dialog.Msg(L("The ground trembling... it seems odd. I wonder if it's related to the experiment..."));
				await dialog.Msg(L("Oh, right, my grandfather! I forgot about him. Please go back to the Absenta Reservoir, I don't want him to get in trouble again."));
			}
			return;
		}

		if (!character.Quests.Has(Hq1) && character.Quests.MeetsPrerequisites(Hq1))
		{
			await dialog.Msg(L("Those are the friendship badges I made with my friends!"));
			await dialog.Msg(L("We were playing at the Pelke Shrine Ruins when we saw a huge monster and ran away without the badges."));
			await dialog.Msg(L("I thought I had lost them forever, I'm so glad to have them back."));

			var answer = await dialog.SelectQuestOffer(Hq1, L("But I'm afraid some of the badges are still missing; would you please find them for me?"),
				Option(L("I'll try"), "accept"),
				Option(L("I'll find it myself"), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= Badges.GetLength(0); ++i)
					character.Variables.Perm.Set(BadgeVar + i, false);

				character.Quests.Start(Hq1);
				character.LookAround();
			}
			return;
		}

		if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
		{
			await dialog.Msg(L("The village priest asked me to collect some antidote from the Merog Stingers."));
			await dialog.Msg(L("I thought the villagers could help but, as you can see... it's hopeless..."));

			var answer = await dialog.SelectQuestOffer(Rp1, L("If you see any Merog Stingers, would you collect some antidote samples from them?"),
				Option(L("I'll help you"), "accept"),
				Option(L("I'm busy"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Rp1);

				await dialog.Msg(L("For some time now the people coming to the shrine ruins are being chased away by Merog Stingers."));
				await dialog.Msg(L("They even stung and almost killed a few villagers..."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			await dialog.Msg(L("I know I should take you there myself, but I feel like I'll just be a burden."));
			await dialog.Msg(L("That day, too, all we did was get grandpa and run away from there."));
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("You still can't find my grandfather? Sigh... If you do find him, make sure to bring him here."));
			character.Quests.ClearQuestTrack(Mq03);
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("Anyway, enough with the diary... You should go and follow my grandfather to the Absenta Reservoir."));
			return;
		}

		if (character.Quests.IsActive(Hq1))
		{
			await dialog.Msg(L("I don't know what monster it is. The only thing I can tell you is that it's huge."));
			return;
		}

		if (character.Quests.IsActive(Rp1))
		{
			await dialog.Msg(L("For some time now the people coming to the shrine ruins are being chased away by Merog Stingers."));
			return;
		}

		if (character.Quests.HasCompleted(Lake84Mq06))
		{
			await dialog.Msg(L("Wow, so the Hydra was the so-called vessel..."));
			await dialog.Msg(L("I wonder what kind of person would put a gem on the Hydra, and for what?"));
			return;
		}

		if (character.Quests.HasCompleted(Mq03))
		{
			await dialog.Msg(L("I'm glad everyone's safe, thanks to you."));
			await dialog.Msg(L("I have to confess this was quite reckless, even with you around."));
			return;
		}

		await dialog.Msg(L("Isn't it weird? All the red water around here."));
		await dialog.Msg(L("Don't drink it, though. It'll make you sick to your stomach."));
	}

	/// <summary>
	/// Samsonas' dialog next to the obelisk.
	/// </summary>
	private async Task Samsonas(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Samsonas"));

		if (character.Quests.IsCompletable(Sq01))
		{
			await dialog.Msg(L("You got it! Thank you so much for helping me."));
			await dialog.CompleteQuest(Sq01);
			return;
		}

		if (character.Quests.IsCompletable(Sq02))
		{
			await dialog.Msg(L("Have I made the offering, you ask? No, I only prayed so far... Why?"));
			await dialog.Msg(L("What? You mean the monsters suddenly ran to the offering? Huh?... That's impossible..."));
			await dialog.Msg(L("I'm sure I saw those people praying. They were wearing black hoods... I really saw them..."));
			await dialog.Msg(L("Hm... I'm confused. That's what I told everyone, what am I going to say now..."));
			await dialog.CompleteQuest(Sq02);

			if (character.Quests.HasCompleted(Sq02))
				character.LookAround();
			return;
		}

		if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
		{
			await dialog.Msg(L("I saw it. Before the water turned red, there were people praying here."));
			await dialog.Msg(L("Those fools in the village think it was because of something else... But I'm convinced there are goddesses we don't know about."));
			await dialog.Msg(L("The goddesses must have gotten angry because no one believes them. I'm sure offering them a tribute would appease them..."));
			await dialog.Msg(L("So I was thinking about offering them some monster meat."));

			var answer = await dialog.SelectQuestOffer(Sq01, L("Would you help me?"),
				Option(L("That's a bit weird but I'll help you"), "accept"),
				Option(L("There's no way the goddesses would do that"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Sq01);
			return;
		}

		if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
		{
			await dialog.Msg(L("I'm going to make an offering and pray to this obelisk. Oh, there's another obelisk somewhere."));
			await dialog.Msg(L("I'll tell you where it is so you can go and pray there, yes?"));

			var answer = await dialog.SelectQuestOffer(Sq02, L("If we solve this I'm sure you'll be generously rewarded by our people."),
				Option(L("That sounds suspicious, but I'll go"), "accept"),
				Option(L("I don't think this is it"), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= Obelisks.GetLength(0); ++i)
					character.Variables.Perm.Set(OfferingVar + i, false);
				character.Variables.Perm.SetInt(OfferingCountVar, 0);

				character.Quests.Start(Sq02);

				var sacks = Obelisks.GetLength(0) - character.Inventory.CountItem(ItemId.F_3CMLAKE_83_SQ_ITEM2);
				if (sacks > 0)
					character.Inventory.Add(ItemId.F_3CMLAKE_83_SQ_ITEM2, sacks, InventoryAddType.PickUp);
			}
			return;
		}

		if (character.Quests.IsActive(Sq01))
		{
			await dialog.Msg(L("The crops in our village were all contaminated by the red water so we can't eat them."));
			await dialog.Msg(L("I can't offer that to the goddesses, right?"));
			return;
		}

		if (character.Quests.IsActive(Sq02))
		{
			await dialog.Msg(L("The other obelisk is on the left side of the Vishikas Great Hall."));
			await dialog.Msg(L("I'll stay here and pray, then. You go now."));
			return;
		}

		await dialog.Msg(L("A food offering would probably appease the goddesses..."));
		await dialog.Msg(L("But all our crops were ruined by the red water, how can we offer that?"));
	}

	/// <summary>
	/// Nikodemas' dialog.
	/// </summary>
	private async Task Nikodemas(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Nikodemas"));

		if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
		{
			await dialog.Msg(L("You didn't see a young man called Napalis, did you? He went out a while ago and hasn't been back since. I'm worried about him now."));
			await dialog.Msg(L("He said he was headed to the Drava Chapel Lot."));

			var answer = await dialog.SelectQuestOffer(Sq03, L("If you're going that way, would you mind checking in on him?"),
				Option(L("I'll try to find them"), "accept"),
				Option(L("I have nothing to do there"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq03);
				character.LookAround();

				await dialog.Msg(L("Why did he have to go out at a time like this. I sure hope nothing happened..."));
			}
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("Nothing... Nothing terrible happened, I hope?"));
			character.Quests.ClearQuestTrack(Sq03);
			return;
		}

		if (character.Quests.HasCompleted(Sq03))
		{
			await dialog.Msg(L("Napalis said you saved them from a really close call."));
			await dialog.Msg(L("I'm glad they're back to safety, but I can't guarantee things like this won't happen again."));
			return;
		}

		await dialog.Msg(L("I'm also worried about the red water, and there's all these monsters around."));
		await dialog.Msg(L("Should I have just gone to Orsha...?"));
	}

	/// <summary>
	/// Napalis' dialog at the Drava Chapel Lot.
	/// </summary>
	private async Task NapalisHiding(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Napalis"));

		if (character.Quests.IsCompletable(Sq03))
		{
			await dialog.Msg(L("You saved my life. I was being chased by that huge monster and had to hide in here."));
			await dialog.Msg(L("I should go back to where the others are. Thank you so much for saving me!"));
			await dialog.CompleteQuest(Sq03);

			if (character.Quests.HasCompleted(Sq03))
				character.LookAround();
			return;
		}

		await dialog.Msg(L("We all live in a village down from here."));
		await dialog.Msg(L("We came all the way here to try and see what's making the water... like this."));
	}

	/// <summary>
	/// Scalvis' dialog.
	/// </summary>
	private async Task Scalvis(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Scalvis"));

		if (character.Quests.IsCompletable(Sq04))
		{
			await dialog.Msg(L("So it was right, what I saw. I knew it. Thank you!"));
			await dialog.Msg(L("I still feel a little uneasy, though..."));
			await dialog.CompleteQuest(Sq04);
			return;
		}

		if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
		{
			await dialog.Msg(L("Looking at that red water makes me feel terrible. It's not even good to drink... not to mention it ruined our crops."));
			await dialog.Msg(L("The waste was monumental. Our vegetables grew huge, but they were all left to rot as we couldn't eat them."));
			await dialog.Msg(L("Everyone in the village was ready to go out and find the reason why the water turned red, but... The only person who has actually fought any monsters is Modis, the hunter."));
			await dialog.Msg(L("I saw you fight those monsters earlier and you were really good."));

			var answer = await dialog.SelectQuestOffer(Sq04, L("If that's okay, will you clear out some monsters around here?"),
				Option(L("I can help you with that"), "accept"),
				Option(L("I have no time for that, sorry"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Sq04);
			return;
		}

		if (character.Quests.IsActive(Sq04))
		{
			await dialog.Msg(L("We should be fine if we just stick together..."));
			await dialog.Msg(L("I hope we don't run into trouble on our way to the village."));
			return;
		}

		await dialog.Msg(L("We came here together to try and figure out this red water, but..."));
		await dialog.Msg(L("It's too dangerous out here..."));
	}

	/// <summary>
	/// The worktable in the Wandering Sanctuary, where the burnt diary lies
	/// in the ashes.
	/// </summary>
	private async Task Workbench(Dialog dialog)
	{
		var character = dialog.Player;

		if (!character.Quests.IsCompletable(Mq02))
			return;

		var searched = await character.TimeActions.StartAsync(L("Investigating"), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));
		if (searched != TimeActionResult.Completed)
			return;

		AggroAround(character, 300);

		character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("Found something in the heap of ashes."), 2);
		await dialog.CompleteQuest(Mq02);

		if (character.Quests.HasCompleted(Mq02))
			character.LookAround();
	}

	/// <summary>
	/// Offers one of Samsonas' sacks at an obelisk, which draws the
	/// monsters out instead of appeasing anyone.
	/// </summary>
	private void MakeOffering(Character character, int number, IActor obelisk)
	{
		if (!character.Quests.IsActive(Sq02) || character.Quests.IsCompletable(Sq02))
			return;

		if (character.Variables.Perm.GetBool(OfferingVar + number, false))
		{
			character.ServerMessage(L("You have already made an offering at this obelisk."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.F_3CMLAKE_83_SQ_ITEM2) == 0)
		{
			character.ServerMessage(L("You need the Sack with Offerings."));
			return;
		}

		character.Inventory.Remove(ItemId.F_3CMLAKE_83_SQ_ITEM2, 1, InventoryItemRemoveMsg.Given);
		character.Variables.Perm.Set(OfferingVar + number, true);

		var offered = character.Variables.Perm.GetInt(OfferingCountVar, 0) + 1;
		character.Variables.Perm.SetInt(OfferingCountVar, offered);

		obelisk?.PlayEffect("F_spread_out004_dark", 1f);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The monsters rush towards the offering!"), 3);
		character.ServerMessage(LF("Offerings made: {0}/{1}", Math.Min(offered, Obelisks.GetLength(0)), Obelisks.GetLength(0)));

		SpawnAmbush(character, obelisk?.Position ?? character.Position, LakeMonsters);
	}

	/// <summary>
	/// Points the Old Disc at a nearby monster, which gives up the red
	/// fragment it was carrying.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_F_3CMLAKE_83_MQ_ITEM1(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_3cmlake_83" || !character.Quests.IsActive(Mq04) || character.Quests.IsCompletable(Mq04))
		{
			character.ServerMessage(L("The Old Disc does not react here."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Inventory.CountItem(ItemId.F_3CMLAKE_83_MQ_ITEM2) >= FragmentsNeeded)
			return ItemUseResult.OkayNotConsumed;

		var target = character.Map.GetAttackableEnemiesInPosition(character, character.Position, DiscRange)
			.OfType<Mob>()
			.FirstOrDefault(mob => LakeMonsters.Contains(mob.Data.ClassName) && !mob.Vars.GetBool(DiscVar + character.ObjectId, false));

		if (target == null)
		{
			character.ServerMessage(L("The Old Disc glows faintly, but there is nothing nearby for it to point at."));
			return ItemUseResult.OkayNotConsumed;
		}

		target.Vars.Set(DiscVar + character.ObjectId, true);
		target.PlayEffect("F_spread_out004_dark", 1f);
		target.InsertHate(character);

		character.Inventory.Add(ItemId.F_3CMLAKE_83_MQ_ITEM2, 1, InventoryAddType.PickUp);

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Returns whether Elder Eloizard is waiting at the Collapsed Hall Lot.
	/// </summary>
	private static bool IsElderAtCamp(Character character)
		=> (character.Quests.HasCompleted(Mq01) && !character.Quests.HasCompleted(Mq02))
		|| (character.Quests.Has(Mq04) && !character.Quests.HasCompleted(Mq04));

	/// <summary>
	/// Returns whether Elder Eloizard and the town youth are near the
	/// Anga Hall.
	/// </summary>
	private static bool IsElderAtAngaHall(Character character)
		=> character.Quests.Has(Mq03) && !character.Quests.Has(Mq04) && !character.Quests.HasCompleted(Mq04);

	/// <summary>
	/// Returns whether the elder's granddaughter's badge still lies on
	/// the ground for the character.
	/// </summary>
	private static bool IsBadgeOnGround(Character character)
		=> !character.Quests.Has(Hq1) && !character.Quests.HasCompleted(Hq1) && character.Inventory.CountItem(ItemId.F3CMLAKE83_HIDDENQ1_ITEM1) == 0;

	/// <summary>
	/// Returns whether the given friendship badge still has to be found.
	/// </summary>
	private static bool IsBadgeLost(Character character, int number)
		=> character.Quests.IsActive(Hq1) && !character.Quests.IsCompletable(Hq1) && !character.Variables.Perm.GetBool(BadgeVar + number, false);

	/// <summary>
	/// Turns the monsters around the character on them.
	/// </summary>
	public static void AggroAround(Character character, float radius)
	{
		foreach (var enemy in character.Map.GetAttackableEnemiesInPosition(character, character.Position, radius))
			enemy.InsertHate(character);
	}

	/// <summary>
	/// Spawns one of each given monster around the position, already
	/// set on the character.
	/// </summary>
	public static void SpawnAmbush(Character character, Position position, params string[] monsterClassNames)
	{
		var angle = 0.0;

		foreach (var className in monsterClassNames)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var monsterData))
				continue;

			var offsetX = (float)(Math.Cos(angle) * 60);
			var offsetZ = (float)(Math.Sin(angle) * 60);
			angle += Math.PI * 2 / monsterClassNames.Length;

			var monster = new Mob(monsterData.Id, RelationType.Enemy);
			monster.Position = new Position(position.X + offsetX, position.Y, position.Z + offsetZ);
			monster.SpawnPosition = monster.Position;

			monster.Components.Add(new LifeTimeComponent(monster, TimeSpan.FromMinutes(2)));
			monster.Components.Add(new MovementComponent(monster));
			monster.Components.Add(new AiComponent(monster, "BasicMonster"));

			character.Map.AddMonster(monster);
			monster.InsertHate(character);
		}
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 90001: The Corrupted Lake (1)
//-----------------------------------------------------------------------------
public class F3Cmlake83Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90001);
		SetName(L("The Corrupted Lake (1)"));
		SetDescription(L("Elder Aloizard wants to return to where the village residents are but is worried about the monsters. Clear out some monsters nearby to help Elder Aloizard return."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_OLDMAN1", "f_3cmlake_83", L("Talk to Elder Aloizard"), L("Elder Aloizard looks deeply concerned about something. Talk to him."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_OLDMAN1", "f_3cmlake_83", L("Defeat the monsters nearby"), L("Elder Aloizard wants to return to where the village residents are but is worried about the monsters. Clear out some monsters nearby to help Elder Aloizard return."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_OLDMAN1", "f_3cmlake_83", L("Talk to Elder Aloizard"), L("You have defeated a good number of monsters nearby. Go and tell Elder Aloizard."));

		AddPrerequisite(new QuestStatusPrerequisite(30069, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(55));

		AddObjective("killMonsters", L("Defeat the monsters nearby"), new KillObjective(8, "Sec_merog_wogu", "Sec_merog_wizzard", "Rajatadpole"));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 380));
		AddReward(new ItemReward("Drug_SP2_Q", 30));
	}
}

// 90019: The Corrupted Lake (2)
//-----------------------------------------------------------------------------
public class F3Cmlake83Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90019);
		SetName(L("The Corrupted Lake (2)"));
		SetDescription(L("If you're really set on helping Elder Aloizard, he will be waiting for you at the Collapsed Hall Lot. Go and find him there."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_OLDMAN2", "f_3cmlake_83", L("Talk to Elder Aloizard"), L("If you're really set on helping Elder Aloizard, he will be waiting for you at the Collapsed Hall Lot. Go and find him there."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_OLDMAN2", "f_3cmlake_83", L("Talk to Elder Aloizard"), L("If you're really set on helping Elder Aloizard, he will be waiting for you at the Collapsed Hall Lot. Go and find him there."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_OLDMAN2", "f_3cmlake_83", L("Talk to Elder Aloizard"), L("If you're really set on helping Elder Aloizard, he will be waiting for you at the Collapsed Hall Lot. Go and find him there."));

		AddPrerequisite(new QuestStatusPrerequisite(90001, QuestStatus.Completed));

		AddObjective("meetElder", L("Talk to Elder Aloizard"), new ManualObjective());
	}
}

// 90002: The Corrupted Lake (3)
//-----------------------------------------------------------------------------
public class F3Cmlake83Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90002);
		SetName(L("The Corrupted Lake (3)"));
		SetDescription(L("The elder's granddaughter says the village residents are suffering with the red water incident but have no way to solve it. Help them out by investigating the Wandering Sanctuary."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Talk to the Elder's Granddaughter"), L("The village chief's granddaughter is looking for someone to help her. Talk to her."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Investigate the Wandering Sanctuary"), L("The elder's granddaughter says the village residents are suffering with the red water incident but have no way to solve it. Help them out by investigating the Wandering Sanctuary."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_WORKBENCH1", "f_3cmlake_83", L("Investigate the Wandering Sanctuary"), L("The elder's granddaughter says the village residents are suffering with the red water incident but have no way to solve it. Help them out by investigating the Wandering Sanctuary."));

		AddPrerequisite(new QuestStatusPrerequisite(90019, QuestStatus.Completed));

		AddObjective("investigate", L("Investigate the Wandering Sanctuary"), new ManualObjective());

		AddReward(new ItemReward("F_3CMLAKE_83_MQ_ITEM3", 1));
	}
}

// 90003: The Corrupted Lake (4)
//-----------------------------------------------------------------------------
public class F3Cmlake83Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90003);
		SetName(L("The Corrupted Lake (4)"));
		SetDescription(L("The elder's granddaughter says he took off with a group of village youth and disappeared. Search for Elder Aloizard around the Anga Hall area."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Deliver the diary to the Elder's Granddaughter"), L("You have found a burnt journal at the Wandering Sanctuary. Bring it to the elder's granddaughter."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_ENTER1", "f_3cmlake_83", L("Search for Elder Aloizard"), L("The elder's granddaughter says he took off with a group of village youth and disappeared. Search for Elder Aloizard around the Anga Hall area."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_OLDMAN3", "f_3cmlake_83", L("Talk to Elder Aloizard"), L("The episode with the monsters was a close call. Ask Elder Aloizard and the village youth if they are all right."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "F_3CMLAKE_83_MQ_03_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(90002, QuestStatus.Completed));

		AddObjective("killRajatadpole", L("Rajatadpole"), new KillObjective(6, "Rajatadpole") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 560));
	}
}

// 90004: The Corrupted Lake (5)
//-----------------------------------------------------------------------------
public class F3Cmlake83Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90004);
		SetName(L("The Corrupted Lake (5)"));
		SetDescription(L("Elder Aloizard says that when the earth shook, he saw the old disc glow red and point towards somewhere. Find out where the old disc is pointing to."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_OLDMAN3", "f_3cmlake_83", L("Talk to Elder Aloizard"), L("Elder Aloizard believes there is something important happening at the Wandering Sanctuary. Talk to Elder Aloizard."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_OLDMAN2", "f_3cmlake_83", L("Look for the spot signaled by the Old Disc"), L("Elder Aloizard says that when the earth shook, he saw the old disc glow red and point towards somewhere. Find out where the old disc is pointing to."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_OLDMAN2", "f_3cmlake_83", L("Deliver to Elder Aloizard"), L("The object the old disc was pointing to was a red gem fragment. Bring it to Elder Aloizard."));

		AddPrerequisite(new QuestStatusPrerequisite(90003, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(55));

		AddObjective("haveDisc", L("Old Disc"), new CollectItemObjective("F_3CMLAKE_83_MQ_ITEM1", 1));
		AddObjective("collectFragments", L("Use the Old Disc on nearby monsters and acquire Red Fragment"), new CollectItemObjective("F_3CMLAKE_83_MQ_ITEM2", 4));

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 650));
		AddReward(new TakeItemReward("F_3CMLAKE_83_MQ_ITEM1", -1));
		AddReward(new TakeItemReward("F_3CMLAKE_83_MQ_ITEM2", -1));
	}
}

// 90005: The Corrupted Lake (6)
//-----------------------------------------------------------------------------
public class F3Cmlake83Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90005);
		SetName(L("The Corrupted Lake (6)"));
		SetDescription(L("The burnt journal seems to contain important information. Go and find Elder Aloizard right away."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_83", "f_3cmlake_84");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Talk to the Elder's Granddaughter"), L("Elder Aloizard took the red fragment and went to the Absenta Reservoir. The elder's granddaughter is looking to speak to you about the journal. Go and talk to her."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_OLDMAN", "f_3cmlake_84", L("Talk to Elder Aloizard at the Absenta Reservoir"), L("The burnt journal seems to contain important information. Go and find Elder Aloizard right away."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_OLDMAN", "f_3cmlake_84", L("Talk to Elder Aloizard at the Absenta Reservoir"), L("The burnt journal seems to contain important information. Go and find Elder Aloizard right away."));

		AddPrerequisite(new QuestStatusPrerequisite(90004, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(55));

		AddObjective("followElder", L("Talk to Elder Aloizard at the Absenta Reservoir"), new ManualObjective());
	}
}

// 90006: Offerings to the Goddess (1)
//-----------------------------------------------------------------------------
public class F3Cmlake83Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90006);
		SetName(L("Offerings to the Goddess (1)"));
		SetDescription(L("Samsonas believes the wrath of the goddesses is what caused the water to turn red. He proposes you collect some Rajatadpole meat to use as an offering."));
		SetType(QuestType.Sub);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_PEOPLE1", "f_3cmlake_83", L("Talk to Samsonas"), L("Samsonas is looking for someone to help him."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_PEOPLE1", "f_3cmlake_83", L("Collect Rajatadpole Meat"), L("Samsonas believes the wrath of the goddesses is what caused the water to turn red. He proposes you collect some Rajatadpole meat to use as an offering."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_PEOPLE1", "f_3cmlake_83", L("Deliver to Samsonas"), L("You have collected enough Rajatadpole meat to use as offering. Bring it to Samsonas."));

		AddPrerequisite(new LevelPrerequisite(55));

		AddObjective("collectMeat", L("Collect Rajatadpole Meat"), new CollectItemObjective("F_3CMLAKE_83_SQ_ITEM1", 8));
		AddPityDrop("F_3CMLAKE_83_SQ_ITEM1", 1.0f, 0, 1, "Rajatadpole");

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 460));
		AddReward(new TakeItemReward("F_3CMLAKE_83_SQ_ITEM1", -1));
	}
}

// 90007: Offerings to the Goddess (2)
//-----------------------------------------------------------------------------
public class F3Cmlake83Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90007);
		SetName(L("Offerings to the Goddess (2)"));
		SetDescription(L("Samsonas is going to use this obelisk and has asked you to use one in another location. Go to the left side of the Vishikas Great Hall and make an offering to the obelisk there."));
		SetType(QuestType.Sub);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_PEOPLE1", "f_3cmlake_83", L("Talk to Samsonas"), L("It's time to make the offering. Talk to Samsonas again."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_PEOPLE1", "f_3cmlake_83", L("Make an offering to the Obelisk"), L("Samsonas is going to use this obelisk and has asked you to use one in another location. Go to the left side of the Vishikas Great Hall and make an offering to the obelisk there."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_PEOPLE1", "f_3cmlake_83", L("Report the results to Samsonas"), L("Oddly enough, the offering caused nearby monsters to attack. Go to Samsonas right away and warn him about it before he makes his offering."));

		AddPrerequisite(new QuestStatusPrerequisite(90006, QuestStatus.Completed));

		AddObjective("makeOfferings", L("Make an offering to the Obelisk"), new VariableCheckObjective(F3Cmlake83QuestNpcsScript.OfferingCountVar, 2, isPermanent: true));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 460));
	}
}

// 90008: One-Way Street
//-----------------------------------------------------------------------------
public class F3Cmlake83Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90008);
		SetName(L("One-Way Street"));
		SetDescription(L("Nikodemas says someone from the village wandered off and hasn't been back since. It seems they were headed towards the Drava Chapel Lot; go and look for them there."));
		SetType(QuestType.Sub);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_PEOPLE2", "f_3cmlake_83", L("Talk to Nikodemas"), L("Nikodemas is looking around apprehensively. Talk to Nikodemas."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_ENTER2", "f_3cmlake_83", L("Find the Village Resident at the Drava Chapel Lot"), L("Nikodemas says someone from the village wandered off and hasn't been back since. It seems they were headed towards the Drava Chapel Lot; go and look for them there."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_PEOPLE5", "f_3cmlake_83", L("Talk to Napalis"), L("You successfully defeated the monster after it suddenly attacking from beneath the water. Ask Napalis if they are all right."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "F_3CMLAKE_83_SQ_03_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(55));

		AddObjective("killRocksodon", L("Defeat Rocksodon"), new KillObjective(1, "boss_Rocksodon") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 560));
	}
}

// 90009: Accident Prevention
//-----------------------------------------------------------------------------
public class F3Cmlake83Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90009);
		SetName(L("Accident Prevention"));
		SetDescription(L("Scalvis is worried about the large number of monsters around. Defeat nearby monsters to make the area safer for the village residents."));
		SetType(QuestType.Repeat);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_PEOPLE3", "f_3cmlake_83", L("Talk to Scalvis"), L("Scalvis is worried about the monsters roaming around. Talk to Scalvis."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_PEOPLE3", "f_3cmlake_83", L("Defeat the monsters nearby"), L("Scalvis is worried about the large number of monsters around. Defeat nearby monsters to make the area safer for the village residents."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_PEOPLE3", "f_3cmlake_83", L("Talk to Scalvis"), L("The area seems safer now. Report back to Scalvis."));

		AddPrerequisite(new LevelPrerequisite(55));

		AddObjective("killMonsters", L("Defeat the monsters nearby"), new KillObjective(10, "Sec_merog_wogu", "Sec_merog_wizzard", "Rajatadpole"));

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new ItemReward("Vis", 235));
	}
}

// 50273: The Lost Object
//-----------------------------------------------------------------------------
public class F3Cmlake83Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50273);
		SetName(L("The Lost Object"));
		SetDescription(L("The elder's granddaughter made friendship badges with her friends but lost them during an accident. Find the badges for her."));
		SetType(QuestType.Sub);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Talk to the Elder's Granddaughter"), L("Talk to the elder's granddaughter."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Find the Friendship Badges"), L("The elder's granddaughter made friendship badges with her friends but lost them during an accident. Find the badges for her."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Talk to the Elder's Granddaughter"), L("Talk to the elder's granddaughter."));

		AddPrerequisite(new ItemPrerequisite("F3CMLAKE83_HIDDENQ1_ITEM1", 1));

		AddObjective("findBadges", L("Find the Elder's Granddaughter's Friendship Badge"), new CollectItemObjective("F3CMLAKE83_HIDDENQ1_ITEM2", 5));

		AddReward(new ItemReward("misc_scrollskulp", 1));
		AddReward(new TakeItemReward("F3CMLAKE83_HIDDENQ1_ITEM1", -1));
		AddReward(new TakeItemReward("F3CMLAKE83_HIDDENQ1_ITEM2", -1));
	}
}

// 60166: Collect Antidote Sample
//-----------------------------------------------------------------------------
public class F3Cmlake83Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60166);
		SetName(L("Collect Antidote Sample"));
		SetDescription(L("The Mayor's granddaughter has asked you to collect antidote samples after defeating Merog Stingers."));
		SetType(QuestType.Repeat);
		SetLocation("f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Talk to the Elder's Granddaughter"), L("The Mayor's granddaughter of Pelke Shrine Ruins is waiting for help."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Collect Antidote Sample"), L("The Mayor's granddaughter has asked you to collect antidote samples after defeating Merog Stingers."));
		SetPhase(QuestStatus.Success, "3CMLAKE_83_LADY", "f_3cmlake_83", L("Report Back to the Mayors' Granddaughter"), L("You have collected enough antidote samples. Return to the Mayor's granddaughter."));

		AddPrerequisite(new LevelPrerequisite(55));

		AddObjective("collectSamples", L("Collect Antidote Sample"), new CollectItemObjective("CM3LAKE83_RP_1_ITEM", 7));
		AddPityDrop("CM3LAKE83_RP_1_ITEM", 0.6f, 3, 1, "Sec_merog_wogu");

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("CM3LAKE83_RP_1_ITEM", -1));
	}
}
