//--- Melia Script ----------------------------------------------------------
// Koru Jungle Quest NPCs
//--- Description -----------------------------------------------------------
// Rose and the traveling merchants scattered by the demons, and the
// herbalist wounded at Nevaginga Hillside.
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

public class FBracken631QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq010 = new QuestId(50090);
	private readonly static QuestId Mq020 = new QuestId(50091);
	private readonly static QuestId Mq030 = new QuestId(50092);
	private readonly static QuestId Mq040 = new QuestId(50093);
	private readonly static QuestId Sq010 = new QuestId(50094);
	private readonly static QuestId Sq020 = new QuestId(50095);
	private readonly static QuestId Sq030 = new QuestId(50096);
	private readonly static QuestId Sq040 = new QuestId(50097);
	private readonly static QuestId Sq050 = new QuestId(50098);
	private readonly static QuestId Rp1 = new QuestId(60155);

	public const string MerchantCountVar = "Gabija.Quests.Bracken631Mq030.Merchants";
	private const string MerchantVar = "Gabija.Quests.Bracken631Mq030.Merchant";
	private const string BagVar = "Gabija.Quests.Bracken631Sq010.Bag";
	private const string GrassVar = "Gabija.Quests.Bracken631Sq030.Grass";
	private const string SoilVar = "Gabija.Quests.Bracken631Rp1.Soil";

	private static readonly TimeSpan GrassRegrowth = TimeSpan.FromSeconds(20);

	private static readonly double[,] Bags =
	{
		{ -251.11, -1459.47, 90 }, { 201.99, -1478.46, 75 }, { 16.55, -1905.25, 94 },
	};

	private static readonly double[,] Grass =
	{
		{ -850.59, 1788.32, 53 }, { -328.78, 1522.76, 66 }, { -260.73, 1838.79, 88 }, { -574.25, 1970.70, 51 }, { -633.99, 1423.01, 70 },
	};

	private static readonly double[,] Soil =
	{
		{ 1070.73, -682.53 }, { 1828.71, -749.57 }, { 1691.01, -793.85 }, { 1540.43, -870.24 }, { 1226.85, -817.26 },
		{ 1782.53, -590.79 }, { 1596.29, -235.90 }, { 1418.29, -276.67 }, { 1235.12, -374.20 }, { 1116.68, -602.66 },
		{ 386.51, -956.63 }, { 271.26, -747.20 }, { 520.65, -665.25 }, { 630.93, -913.36 }, { 916.18, -852.06 },
		{ 786.54, -901.96 },
	};

	protected override void Load()
	{
		// Traveling Merchant Varas
		//-------------------------------------------------------------------------
		AddNpc(155038, L("Traveling Merchant Varas"), "BRACKEN631_TRADESMAN01", "f_bracken_63_1", 811.35, 267.40, 160, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Varas"));

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("Hey! Stop wandering around and run!"));
				await dialog.Msg(L("Oh no... There's no time to tell everyone in Orsha..."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("If I don't do anything the demons are going to get my colleagues... What do I do...?"),
					Option(L("Where?"), "accept"),
					Option(L("I'm running away; it's too dangerous"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);
					character.LookAround();

					await dialog.Msg(L("What? You mean the demons came?"));
					await dialog.Msg(L("I don't want to put someone I just met in such danger... But you're my only hope. Please, do this for me."));
					await dialog.Msg(L("They're hiding in the Herb Gatherers' Cabin. I don't know when the demons will find them."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq010) && !character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("Even if I want to ask for help in Orsha, but it's too far! Please, save my colleagues."));
				character.Quests.ClearQuestTrack(Mq010);
				return;
			}

			if (character.Quests.HasCompleted(Mq040))
			{
				await dialog.Msg(L("Thank you so much for saving my colleagues. You ran in even though you knew that there were demons..."));
				await dialog.Msg(L("Hmm... Rose is really late. Should we wait a little bit more?"));
				return;
			}

			if (character.Quests.Has(Mq010))
			{
				await dialog.Msg(L("I couldn't possibly rescue my people all by myself, I even thought of escaping to Orsha... You really are something, huh?"));
				return;
			}

			await dialog.Msg(L("Hey, you! Stop wandering around and run, quick! There are demons here!"));
		});

		AddQuestTrigger("BRACKEN631_HIDEENTRACK01", "f_bracken_63_1", 229.44, 30.19, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq010) && !character.Quests.IsCompletable(Mq010))
				character.Quests.StartQuestTrack(Mq010);

			await Task.CompletedTask;
		});

		// Traveling Merchant Rose
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "BRACKEN631_ROZE", "f_bracken_63_1", -92.42, -144.25, 40, c => c.Quests.Has(Mq010) && !c.Quests.HasCompleted(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("Thank you so much for saving me!"));
				await dialog.Msg(L("I came back to the village and all I saw were demons, I was wondering what happened..."));
				await dialog.CompleteQuest(Mq010);
				return;
			}

			if (character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("Oh, thank you. I'll give it to Laswi."));
				await dialog.Msg(L("I was worried about giving Laswi the pill, but I think it'll be fine. Fortunately, Laswi seems to feel better now."));
				await dialog.CompleteQuest(Mq020);
				return;
			}

			if (character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("My colleagues are safe! I'm so glad to see them again; it's all thanks to you."));
				await dialog.Msg(L("I wonder if I should go to Knidos Jungle by myself and look for my village's people now. I can't keep dragging my colleagues into something that concerns my hometown."));
				await dialog.Msg(L("Thank you for rescuing my colleagues. I should go now... Please pray for me so I can find the people of my hometown."));
				await dialog.CompleteQuest(Mq040);

				if (character.Quests.HasCompleted(Mq040))
				{
					character.LookAround();
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Rose will be in danger if she's all alone. Let's look for her in Knidos Jungle."), 8);
				}
				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				await dialog.Msg(L("Our hometown is in an inner area of Knidos Jungle. We're traveling merchants. We sell items all across the kingdom."));
				await dialog.Msg(L("I was in a village to the north of Orsha when I heard about the migration order. I was worried about my hometown so I came back."));
				await dialog.Msg(L("But the village is completely empty. And there's no word about the migration order in brother's letters either."));
				await dialog.Msg(L("I wondered if he had at least left a note out here... But when we were looking through the village the demons attacked and I ran all the way here."));
				await dialog.Msg(L("I'm worried for the village, but also for my colleagues who had to split up earlier."));

				var answer = await dialog.SelectQuestOffer(Mq020, L("We've never been in danger like this before..."),
					Option(L("I'll help you find your colleagues"), "accept"),
					Option(L("About the merchants and the letter"), "explain"),
					Option(L("I think it's best to get away from here"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("I started working as a merchant because I was curious about the world outside the village. My brother, of course, thought it was too dangerous and didn't want to let me."));
					await dialog.Msg(L("In the end I was more stubborn than him and he accepted. As long as I kept writing letters to him, that is."));
					await dialog.Msg(L("I always write down my next destination in my letters so my brother knows where to send his. He usually writes to me about the village and the workings of the world."));
					await dialog.Msg(L("But he didn't write anything about going to Orsha in his last letter... I wonder what happened."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq020);
					character.Quests.CompleteObjective(Mq020, "giveStaminaPill");

					await dialog.Msg(L("Will you really help me out? To be able to receive the help of someone as talented as you... It's clear the goddesses still haven't left us."));
					await dialog.Msg(L("Laswi can easily find out where my people are. But right now Laswi looks too tired."));
					await dialog.Msg(L("If only we had a Stamina pill or... You wouldn't happen to have one, would you?"));
				}
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				await dialog.Msg(L("There are four colleagues out there. Have Laswi smell their clothes and lead you to where they are hiding."));
				await dialog.Msg(L("If Laswi loses the scent, have her smell the clothes again."));

				var answer = await dialog.SelectQuestOffer(Mq030, L("Laswi is a smart dog, I'm sure you'll be able to find my colleagues."),
					Option(L("I will go find the merchants with Laswi"), "accept"),
					Option(L("I need some time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= 3; ++i)
						character.Variables.Perm.Set(MerchantVar + i, false);
					character.Variables.Perm.SetInt(MerchantCountVar, 0);

					character.Quests.Start(Mq030);
					character.Inventory.Add(ItemId.BRACKEN631_MQ3_ITEM01, 1, InventoryAddType.PickUp);
					character.Inventory.Add(ItemId.BRACKEN631_MQ3_ITEM02, 1, InventoryAddType.PickUp);
					character.Inventory.Add(ItemId.BRACKEN631_MQ3_ITEM03, 1, InventoryAddType.PickUp);
					character.LookAround();
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Have Laswi follow the scent of each merchant and find them"), 8);
				}
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				await dialog.Msg(L("I hope they're fine. Even small monsters are a worry, let alone demons..."));
				return;
			}

			if (character.Quests.HasCompleted(Mq030))
			{
				await dialog.Msg(L("Nothing happened to Cassius, right?"));
				return;
			}

			await dialog.Msg(L("Laswi is smart, finding my colleagues won't be a problem. I hope they're hiding somewhere safe..."));
		});

		// Laswi
		//-------------------------------------------------------------------------
		AddConditionalNpc(153128, L("Laswi"), "BRACKEN631_DOG", "f_bracken_63_1", -83.48, -127.24, 35, c => c.Quests.Has(Mq010) && !c.Quests.Has(Mq030));
		AddConditionalNpc(153128, L("Laswi"), "BRACKEN631_DOG_02", "f_bracken_63_1", -181.17, -1047.91, 199, c => c.Quests.HasCompleted(Mq030) && !IsCassiusBackAtTheCabin(c));
		AddConditionalNpc(153128, L("Laswi"), "BRACKEN631_DOG_02_1", "f_bracken_63_1", 80.41, 13.93, 102, IsCassiusBackAtTheCabin);

		// Traveling Merchant Andres
		//-------------------------------------------------------------------------
		AddConditionalNpc(155034, L("Traveling Merchant Andres"), "BRACKEN631_TRADESMAN02", "f_bracken_63_1", -95.34, -205.62, 105, c => c.Quests.Has(Mq010), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Andres"));

			if (character.Quests.IsCompletable(Sq010))
			{
				await dialog.Msg(L("Yes, this is it! Thank you! It seems like the missing goddesses are back after all!"));
				await dialog.CompleteQuest(Sq010);
				return;
			}

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				await dialog.Msg(L("How many times have I talked to her... I understand that Rose is worried about her people."));
				await dialog.Msg(L("But they probably just left for Orsha because of the migration order, isn't it?"));
				await dialog.Msg(L("Because of all of this we ran into the demons and I lost my luggage. And all my expensive items were there... They were my only assets."));

				var answer = await dialog.SelectQuestOffer(Sq010, L("But I can't ask our group to go out and get it... The demons will be all over us if we so much as try to move. It's a tough call..."),
					Option(L("I will find it for you"), "accept"),
					Option(L("I feel very sorry for what happened"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Bags.GetLength(0); ++i)
						character.Variables.Perm.Set(BagVar + i, false);

					character.Quests.Start(Sq010);
					character.LookAround();

					await dialog.Msg(L("Are you serious? Wow, that's... As if saving my life wasn't enough, now you're saving my entire assets, too."));
					await dialog.Msg(L("They're at the Doholle Rest Place. I was planning on going there to get them later."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq010))
			{
				await dialog.Msg(L("I never felt so disheartened, not even during Medzio Diena. But it's my assets that are at stake here."));
				return;
			}

			if (character.Quests.HasCompleted(Mq040))
			{
				await dialog.Msg(L("I do feel uncomfortable for the words I have used. Rose is still out and I am a bit worried."));
				return;
			}

			if (character.Quests.HasCompleted(Mq010))
			{
				await dialog.Msg(L("I understand that Rose is worried, but... If things go wrong we can lose our lives before we even see the villagers, let alone our possessions."));
				return;
			}

			await dialog.Msg(L("We came to the Croa Village for Rose; a little more and the demons would've caught us. You would think they'd all gone to Orsha because of that migration order... Isn't it?"));
		});

		// Andres' lost packages at the Doholle Rest Place
		//-------------------------------------------------------------------------
		for (var i = 0; i < Bags.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(47160, "UnvisibleName", "BRACKEN631_BAG0" + number, "f_bracken_63_1", Bags[i, 0], Bags[i, 1], Bags[i, 2],
				character => character.Quests.IsActive(Sq010) && !character.Quests.IsCompletable(Sq010) && !character.Variables.Perm.GetBool(BagVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq010) || character.Quests.IsCompletable(Sq010) || character.Variables.Perm.GetBool(BagVar + number, false))
						return;

					character.Variables.Perm.Set(BagVar + number, true);
					character.Inventory.Add(ItemId.BRACKEN631_SQ1_ITEM01, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Traveling Merchant Gomez
		//-------------------------------------------------------------------------
		AddConditionalNpc(155039, L("Traveling Merchant Gomez"), "BRACKEN631_TRADESMAN03", "f_bracken_63_1", -102.75, -87.24, 16, c => c.Quests.Has(Mq010), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Gomez"));

			if (character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("Oh? You really took care of it? What a relief to hear that. Thanks a lot."));
				await dialog.CompleteQuest(Sq020);
				return;
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				await dialog.Msg(L("Thank you for saving me earlier. It was the most terrorizing experience I'd had since Medzio Diena."));
				await dialog.Msg(L("Now Rose wants to go and look for the people of her hometown. I wish I could help her too, but I freeze every time I see demons, so I don't think I can unfortunately."));

				var answer = await dialog.SelectQuestOffer(Sq020, L("For now I think I'll stay here with my colleagues and wait for Rose. I'm still worried about the demons and the monsters, though."),
					Option(L("I'll defeat the monsters around"), "accept"),
					Option(L("It's best to escape to somewhere safe"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq020);

				return;
			}

			if (character.Quests.IsActive(Sq020))
			{
				await dialog.Msg(L("I really thought a demon had found me earlier, then I saw your face and, phew... I felt as if my prayers to the goddesses had been answered."));
				return;
			}

			if (character.Quests.HasCompleted(Sq020))
			{
				await dialog.Msg(L("The fear I felt being chased by the demons... It's not something I ever want to go through again. Not after Medzio Diena."));
				return;
			}

			await dialog.Msg(L("Where are all the people of Rose's village? She's been so worried..."));
		});

		// Traveling Merchant
		//-------------------------------------------------------------------------
		AddConditionalNpc(155035, L("Traveling Merchant"), "BRACKEN631_TRADESMAN06", "f_bracken_63_1", -93.96, -62.27, 95, c => c.Quests.Has(Mq010), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant"));

			if (character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("I think this will do to keep me safe for a few hours... Thank you!"));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				await dialog.Msg(L("I think I'm going to need to camp out here for a few days, but I'm worried about spending the night here."));

				var answer = await dialog.SelectQuestOffer(Rp1, L("Will you get me the blue coal I buried at Goram Shores and Saunu Brook? I buried it before Medzio Diena, but given my situation I really need to use it..."),
					Option(L("I'll try to find them"), "accept"),
					Option(L("I have no idea what you are talking about."), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Soil.GetLength(0); ++i)
						character.Variables.Perm.Set(SoilVar + i, false);

					character.Quests.Start(Rp1);
					character.LookAround();

					await dialog.Msg(L("I bought it from the mine in the northern snowfield thinking I could sell it in Orsha... However, after Medzio Diena, everyone was busy reconstructing the city and cutting down the tree around it."));
					await dialog.Msg(L("In the end, no one was buying my items. So I ended up burying it nearby in Koru Jungle."));
					await dialog.Msg(L("I didn't mean to leave it there for so long... Not sure if it'll still light up, because of the humidity."));
				}
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("I think it'll be hard to move anywhere until our people recover."));
				return;
			}

			await dialog.Msg(L("We should have hired some soldiers. This is getting more and more difficult."));
		});

		AddConditionalNpc(154075, "UnvisibleName", "BRACKEN631_RP_1_TORCH", "f_bracken_63_1", -56.40, -79.66, 90, c => c.Quests.Has(Mq010));

		// Piles of Soil at Goram Shores and Saunu Brook
		//-------------------------------------------------------------------------
		for (var i = 0; i < Soil.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(155026, L("Pile of Soil"), "BRACKEN631_RP_1_OBJ_" + number, "f_bracken_63_1", Soil[i, 0], Soil[i, 1], 90,
				character => character.Quests.IsActive(Rp1) && !character.Quests.IsCompletable(Rp1) && !character.Variables.Perm.GetBool(SoilVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Rp1) || character.Quests.IsCompletable(Rp1) || character.Variables.Perm.GetBool(SoilVar + number, false))
						return;

					character.Variables.Perm.Set(SoilVar + number, true);
					character.Inventory.Add(ItemId.BRACKEN631_RP_1_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// The scattered merchants
		//-------------------------------------------------------------------------
		AddConditionalNpc(152065, L("Traveling Merchant Ramille"), "BRACKEN631_MQ3TRADESMAN01", "f_bracken_63_1", 406.50, -1113.60, 90, c => IsMerchantHiding(c, 1), async dialog =>
		{
			dialog.SetTitle(L("Traveling Merchant Ramille"));
			await this.FindMerchant(dialog, 1, L("Laswi! Is Rose okay?"), L("I should go back now."));
		});

		AddConditionalNpc(155036, L("Traveling Merchant Ales"), "BRACKEN631_MQ3TRADESMAN02", "f_bracken_63_1", 179.86, -1933.28, 194, c => IsMerchantHiding(c, 2), async dialog =>
		{
			dialog.SetTitle(L("Traveling Merchant Ales"));
			await this.FindMerchant(dialog, 2, L("If Laswi is here... What about Rose?"), L("Rose must have escaped too, then. I'm going back to the Herb Gatherers Cabin!"));
		});

		AddConditionalNpc(155036, L("Traveling Merchant Krens"), "BRACKEN631_MQ3TRADESMAN03", "f_bracken_63_1", -345.49, -2030.07, 90, c => IsMerchantHiding(c, 3), async dialog =>
		{
			dialog.SetTitle(L("Traveling Merchant Krens"));
			await this.FindMerchant(dialog, 3, L("Hey, it's Laswi! Did you come find me?"), L("Thank you so much for finding me. I was so scared..."));
		});

		AddConditionalNpc(152065, L("Traveling Merchant Ramille"), "BRACKEN631_MQ3TRADESMAN01_1", "f_bracken_63_1", 9.93, -438.92, 189, c => IsMerchantBack(c, 1), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Ramille"));

			if (character.Quests.HasCompleted(Mq040))
				await dialog.Msg(L("I ended up coming here because of Rose, the damage done is unimaginable... But I hope the people of Rose's village will be all right."));
			else
				await dialog.Msg(L("With all the demons and monsters, if it weren't for you and Laswi... I don't even want to know."));
		});

		AddConditionalNpc(155036, L("Traveling Merchant Ales"), "BRACKEN631_MQ3TRADESMAN01_2", "f_bracken_63_1", 163.89, -404.69, 230, c => IsMerchantBack(c, 2), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Ales"));

			if (character.Quests.HasCompleted(Mq040))
				await dialog.Msg(L("I thought I was going to die when I was separated from my people. Thanks to you and Laswi I can be with them again."));
			else
				await dialog.Msg(L("I was always so mean to Laswi. To think that Laswi is the one who found me... I guess I'll have to be good to Laswi now."));
		});

		AddConditionalNpc(155036, L("Traveling Merchant Krens"), "BRACKEN631_MQ3TRADESMAN01_3", "f_bracken_63_1", -12.55, -2.04, 28, c => IsMerchantBack(c, 3), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Krens"));

			if (character.Quests.HasCompleted(Mq040))
				await dialog.Msg(L("All these monsters and demons... I hate dangerous things. This just isn't for me."));
			else
				await dialog.Msg(L("My waist hurts from being so nervous. You never know when a monster or a demon is going to attack."));
		});

		// Traveling Merchant Cassius
		//-------------------------------------------------------------------------
		AddConditionalNpc(155037, L("Traveling Merchant Cassius"), "BRACKEN631_TRADESMAN05", "f_bracken_63_1", -193.66, -1016.55, 28, c => !IsCassiusBackAtTheCabin(c), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Cassius"));

			if (character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Laswi! You're alive! Phew... I thought it was a demon."));
				await dialog.Msg(L("How come Laswi is with you, though? Did you happen to meet Laswi's owner?"));
				await dialog.CompleteQuest(Mq030);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				await dialog.Msg(L("Oh... Rose asked you to come find me... It seems she managed to stay safe, too."));
				await dialog.Msg(L("Laswi always protected me from the demons... And now she found me all the way here, she's really one of a kind."));
				await dialog.Msg(L("You said our people are at the Herb Gatherers' Cabin, right? I'm tired now. I'm going back to Orsha with them."));

				var answer = await dialog.SelectQuestOffer(Mq040, L("I'm worried about the people of Rose's village too, but what can we do? Migration order or not, they're probably heading to Orsha."),
					Option(L("Let's go back to the Herb Gatherers' Cabin"), "accept"),
					Option(L("Let's rest for a while."), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq040);

				return;
			}

			if (character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("We should go back with Rose now... But she's being so stubborn. Not even Goddess Gabija would make her change her mind."));
				character.Quests.ReplayQuestTrack(Mq040);
				return;
			}

			await dialog.Msg(L("We should go back with Rose now... But she's being so stubborn. Not even Goddess Gabija would make her change her mind."));
		});

		AddConditionalNpc(155037, L("Traveling Merchant Cassius"), "BRACKEN631_TRADESMAN05_1", "f_bracken_63_1", 80.57, 23.57, 37, IsCassiusBackAtTheCabin, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Cassius"));

			if (character.Quests.HasCompleted(Mq040))
			{
				await dialog.Msg(L("Rose is late... far too late. They say no news is good news, but this is not the time. What should we do?"));
				return;
			}

			await dialog.Msg(L("Everyone looks safe now. I was so scared earlier, you can say I almost returned to the goddess and back again."));
		});

		// Herbalist Tales
		//-------------------------------------------------------------------------
		AddNpc(155035, L("Herbalist Tales"), "BRACKEN631_PEAPLE01", "f_bracken_63_1", -856.63, 1306.80, 151, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Herbalist Tales"));

			if (character.Quests.IsCompletable(Sq030))
			{
				await dialog.Msg(L("You got it! Hand them over. They're probably a little bitter but I really need it."));
				await dialog.CompleteQuest(Sq030);
				return;
			}

			if (character.Quests.IsCompletable(Sq050))
			{
				var fed = await character.TimeActions.StartAsync(L("Giving the medicine to Herbalist Tales..."), L("Cancel"), "FEED", TimeSpan.FromSeconds(1));
				if (fed != TimeActionResult.Completed)
					return;

				dialog.Npc.PlayEffect("F_pc_drug_hpup", 2f);

				await dialog.Msg(L("The medicine you brought seems to be working. My temperature should start to lower, too."));
				await dialog.Msg(L("I thought I was about to return to the goddesses like this... You really saved my life."));
				await dialog.CompleteQuest(Sq050);
				return;
			}

			if (!character.Quests.Has(Sq030) && character.Quests.MeetsPrerequisites(Sq030))
			{
				await dialog.Msg(L("It was really unexpected... We barely saw any monsters before, and now all of a sudden we have to deal with demons."));
				await dialog.Msg(L("At least I survived, but the wounds feel strangely swollen and hot."));

				var answer = await dialog.SelectQuestOffer(Sq030, L("Will you bring me some herbs? I'm hoping they'll ease the pain a little bit."),
					Option(L("I'll go and find the herbs"), "accept"),
					Option(L("Go back to the village soon"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq030);
					character.LookAround();

					await dialog.Msg(L("It's called Ronjia Grass, it looks a little different from the grass around it. I think there's some on the Nevaginga Hillside."));
				}
				return;
			}

			if (!character.Quests.Has(Sq040) && character.Quests.MeetsPrerequisites(Sq040))
			{
				await dialog.Msg(L("This is... It looks serious."));

				var answer = await dialog.SelectQuestOffer(Sq040, L("I mean, it's a wound caused by a demon, so I don't think it'll ever really heal."),
					Option(L("Ask him if there's anything you can help"), "accept"),
					Option(L("Go back to the village fast"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq040);
					character.Quests.CompleteObjective(Sq040, "tellSymptoms");

					await dialog.Msg(L("I think the Cleric Submaster in Orsha might be able to help. I once helped her get some herbs."));
					await dialog.Msg(L("Tell her about my symptoms. My wounds are swollen, hot and now they're turning purple..."));
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Tell the Cleric Master in Orsha about Herbalist Tales' symptoms"), 5);
				}
				return;
			}

			if (character.Quests.IsActive(Sq030))
			{
				await dialog.Msg(L("It's so painful, even though I barely touched it. What kind of demon..."));
				return;
			}

			if (character.Quests.IsActive(Sq040))
			{
				await dialog.Msg(L("I think the Cleric Submaster in Orsha might be able to help. I once helped her get some herbs."));
				return;
			}

			if (character.Quests.HasCompleted(Sq030))
			{
				await dialog.Msg(L("The last time I have visited here, there was no sight of a demon.. I am worried something awful has happened. Have you heard anything about this situation?"));
				return;
			}

			await dialog.Msg(L("What happened to the people of the Croa Village? And why are there demons here?"));
		});

		// Ronjia Grass on Nevaginga Hillside
		//-------------------------------------------------------------------------
		for (var i = 0; i < Grass.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(47200, L("Ronjia Grass"), "BRACKEN631_MEDICAL_PLANT_" + number, "f_bracken_63_1", Grass[i, 0], Grass[i, 1], Grass[i, 2],
				character => character.Quests.IsActive(Sq030) && !character.Quests.IsCompletable(Sq030),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq030) || character.Quests.IsCompletable(Sq030))
						return;

					var pickedAt = character.Variables.Temp.GetLong(GrassVar + number, 0);
					if (pickedAt != 0 && DateTime.Now - new DateTime(pickedAt) < GrassRegrowth)
					{
						character.ServerMessage(L("There's no Ronjia Grass left to pick here yet."));
						return;
					}

					character.Variables.Temp.SetLong(GrassVar + number, DateTime.Now.Ticks);
					character.Inventory.Add(ItemId.BRACKEN631_SQ4_ITEM01, 1, InventoryAddType.PickUp);

					await Task.CompletedTask;
				});
		}
	}

	/// <summary>
	/// Returns whether the given scattered merchant still waits at their
	/// hiding spot for Laswi.
	/// </summary>
	private static bool IsMerchantHiding(Character character, int number)
		=> character.Quests.IsActive(Mq030) && !character.Variables.Perm.GetBool(MerchantVar + number, false);

	/// <summary>
	/// Returns whether the given scattered merchant made it back to the
	/// Herb Gatherers' Cabin.
	/// </summary>
	private static bool IsMerchantBack(Character character, int number)
		=> character.Quests.HasCompleted(Mq030) || (character.Quests.IsActive(Mq030) && character.Variables.Perm.GetBool(MerchantVar + number, false));

	/// <summary>
	/// Returns whether Cassius has made it back to the Herb Gatherers'
	/// Cabin after the demons' last attack.
	/// </summary>
	private static bool IsCassiusBackAtTheCabin(Character character)
		=> character.Quests.IsCompletable(Mq040) || character.Quests.HasCompleted(Mq040);

	/// <summary>
	/// Reunites one of the scattered merchants with Laswi, who sends
	/// them back to the Herb Gatherers' Cabin.
	/// </summary>
	private async Task FindMerchant(Dialog dialog, int number, string greeting, string farewell)
	{
		var character = dialog.Player;

		if (!IsMerchantHiding(character, number))
			return;

		await dialog.Msg(greeting);
		await dialog.Msg(farewell);

		character.Variables.Perm.Set(MerchantVar + number, true);
		var found = character.Variables.Perm.GetInt(MerchantCountVar, 0) + 1;
		character.Variables.Perm.SetInt(MerchantCountVar, found);

		character.ServerMessage(LF("Merchants found: {0}/{1}", Math.Min(found, 3), 3));
		character.LookAround();
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50090: Nervous Vendor
//-----------------------------------------------------------------------------
public class Bracken631Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50090);
		SetName(L("Nervous Vendor"));
		SetDescription(L("Varas says his colleagues are hidden somewhere around the Herb Gatherers' Cabin. Save Varas' friends before the demons get to them."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_TRADESMAN01", "f_bracken_63_1", L("Talk with Varas"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_HIDEENTRACK01", "f_bracken_63_1", L("Go to the Herb Gatherers' Cabin to save the merchants"));
		SetPhase(QuestStatus.Success, "BRACKEN631_ROZE", "f_bracken_63_1", L("Talk to Traveling Merchant Rose"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "BRACKEN_63_1_MQ010_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60145, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("killChasers", L("Defeat the demons threatening the merchants"), new KillObjective(7, "Sec_bubbe_chaser") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Vis", 160));
		AddReward(new SelectItemReward("HAND02_163", "HAND02_164", "HAND02_165"));
	}
}

// 50091: Rose's Friends (1)
//-----------------------------------------------------------------------------
public class Bracken631Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50091);
		SetName(L("Rose's Friends (1)"));
		SetDescription(L("Rose wants to go find her friends who are still scattered around the area, but Laswi is too tired to help. Give Laswi a Stamina pill."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_ROZE", "f_bracken_63_1", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_ROZE", "f_bracken_63_1", L("Give Laswi a Stamina Pill"));
		SetPhase(QuestStatus.Success, "BRACKEN631_ROZE", "f_bracken_63_1", L("Talk to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50090, QuestStatus.Completed));

		AddObjective("giveStaminaPill", L("Give Laswi a Stamina Pill"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Vis", 100));
	}
}

// 50092: Rose's Friends (2)
//-----------------------------------------------------------------------------
public class Bracken631Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50092);
		SetName(L("Rose's Friends (2)"));
		SetDescription(L("Have Laswi smell the merchants' clothes and follow their scent to find each one. When you find them, tell them of Rose's whereabouts."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_ROZE", "f_bracken_63_1", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_ROZE", "f_bracken_63_1", L("Find the merchants with Laswi"));
		SetPhase(QuestStatus.Success, "BRACKEN631_TRADESMAN05", "f_bracken_63_1", L("Find the last Traveling Merchant"));

		AddPrerequisite(new QuestStatusPrerequisite(50091, QuestStatus.Completed));

		AddObjective("findMerchants", L("Find the merchants with Laswi"), new VariableCheckObjective(FBracken631QuestNpcsScript.MerchantCountVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard2", 3));
		AddReward(new ItemReward("Vis", 210));
		AddReward(new TakeItemReward("BRACKEN631_MQ3_ITEM01", 1));
		AddReward(new TakeItemReward("BRACKEN631_MQ3_ITEM02", 1));
		AddReward(new TakeItemReward("BRACKEN631_MQ3_ITEM03", 1));
	}
}

// 50093: Rose's Friends (3)
//-----------------------------------------------------------------------------
public class Bracken631Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50093);
		SetName(L("Rose's Friends (3)"));
		SetDescription(L("Traveling Merchant Cassias was about to go back when the demons started attacking. Defeat all of the attacking demons."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_TRADESMAN05", "f_bracken_63_1", L("Talk to Traveling Merchant Cassias"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_TRADESMAN05", "f_bracken_63_1", L("Defeat the attacking demons"));
		SetPhase(QuestStatus.Success, "BRACKEN631_ROZE", "f_bracken_63_1", L("Report to Traveling Merchant Rose"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "BRACKEN_63_1_MQ040_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50092, QuestStatus.Completed));

		AddObjective("killChasers", L("Defeat the attacking demons"), new KillObjective(9, "Sec_bubbe_chaser") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Vis", 160));
		AddReward(new SelectItemReward("FOOT02_163", "FOOT02_164", "FOOT02_165"));
	}
}

// 50094: Vendor's Lost Baggage
//-----------------------------------------------------------------------------
public class Bracken631Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50094);
		SetName(L("Vendor's Lost Baggage"));
		SetDescription(L("Andres says he invested everything in his goods, but now they are lost. Look for Andres' goods at the Doholle Rest Place."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_TRADESMAN02", "f_bracken_63_1", L("Talk to Traveling Merchant Andres"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_TRADESMAN02", "f_bracken_63_1", L("Look for the Lost Package"));
		SetPhase(QuestStatus.Success, "BRACKEN631_TRADESMAN02", "f_bracken_63_1", L("Deliver to Traveling Merchant Andres"));

		AddPrerequisite(new QuestStatusPrerequisite(50090, QuestStatus.Completed));

		AddObjective("collectPackages", L("Look for the Lost Package"), new CollectItemObjective("BRACKEN631_SQ1_ITEM01", 3));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Vis", 300));
		AddReward(new ItemReward("Drug_SP1_Q", 15));
		AddReward(new TakeItemReward("BRACKEN631_SQ1_ITEM01", -1));
	}
}

// 50095: Nervous Vendor
//-----------------------------------------------------------------------------
public class Bracken631Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50095);
		SetName(L("Nervous Vendor"));
		SetDescription(L("Traveling Merchant Gomez is frightened by all the demons and monsters running wild. Defeat some monsters around the Herb Gatherers' Cabin to ease Gomez' anxiety."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_TRADESMAN03", "f_bracken_63_1", L("Talk to Traveling Merchant Gomez"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_TRADESMAN03", "f_bracken_63_1", L("Defeat the monsters nearby Herb Gatherers' Cabin"));
		SetPhase(QuestStatus.Success, "BRACKEN631_TRADESMAN03", "f_bracken_63_1", L("Report to Traveling Merchant Gomez"));

		AddPrerequisite(new QuestStatusPrerequisite(50090, QuestStatus.Completed));

		AddObjective("killMonsters", L("Defeat the monsters nearby Herb Gatherers' Cabin"), new KillObjective(10, "Ferrot", "Folibu", "Leafnut", "Sec_bubbe_chaser"));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new ItemReward("Vis", 130));
	}
}

// 50096: The Injured Herbalist (1)
//-----------------------------------------------------------------------------
public class Bracken631Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50096);
		SetName(L("The Injured Herbalist (1)"));
		SetDescription(L("Herbalist Tales requested you get some Ronjia Grass to use as a painkiller. Go to Nevaginga Hillside and collect Ronjia Grass."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_PEAPLE01", "f_bracken_63_1", L("Talk to Herbalist Tales"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_PEAPLE01", "f_bracken_63_1", L("Collect Ronjia Grass at Nevaginga Hillside"));
		SetPhase(QuestStatus.Success, "BRACKEN631_PEAPLE01", "f_bracken_63_1", L("Deliver to Herbalist Tales"));

		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("collectGrass", L("Collect Ronjia Grass at Nevaginga Hillside"), new CollectItemObjective("BRACKEN631_SQ4_ITEM01", 6));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 70));
		AddReward(new TakeItemReward("BRACKEN631_SQ4_ITEM01", -1));
	}
}

// 50097: The Injured Herbalist (2)
//-----------------------------------------------------------------------------
public class Bracken631Sq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50097);
		SetName(L("The Injured Herbalist (2)"));
		SetDescription(L("Herbalist Tales thinks the Cleric Submaster might know of a way to heal their injuries. Go see the Cleric Submaster in Orsha and tell them about Herbalist Tales' symptoms."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_1", "c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_PEAPLE01", "f_bracken_63_1", L("Talk to Herbalist Tales"));
		SetPhase(QuestStatus.InProgress, "JOB_2_CLERIC_NPC", "c_orsha", L("Tell the Cleric Master in Orsha about Herbalist Tales' symptoms"));
		SetPhase(QuestStatus.Success, "JOB_2_CLERIC_NPC", "c_orsha", L("Tell the Cleric Master in Orsha about Herbalist Tales' symptoms"));

		AddPrerequisite(new QuestStatusPrerequisite(50096, QuestStatus.Completed));

		AddObjective("tellSymptoms", L("Tell the Cleric Master in Orsha about Herbalist Tales' symptoms"), new ManualObjective());
	}
}

// 60155: Camping Preparations
//-----------------------------------------------------------------------------
public class Bracken631Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60155);
		SetName(L("Camping Preparations"));
		SetDescription(L("The merchant wants to retrieve some blue coal they buried in Koru Jungle to help them make it through until the group has recovered. Go to Goram Shores and Saunu Brook and find the pieces of blue coal in the piles of dirt."));
		SetType(QuestType.Repeat);
		SetLocation("f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN631_TRADESMAN06", "f_bracken_63_1", L("Talk with the merchant"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_TRADESMAN06", "f_bracken_63_1", L("Collect Blue Coal"));
		SetPhase(QuestStatus.Success, "BRACKEN631_TRADESMAN06", "f_bracken_63_1", L("Deliver the Blue Coal to the Merchant"));

		AddPrerequisite(new QuestStatusPrerequisite(50090, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("collectCoal", L("Collect Blue Coal"), new CollectItemObjective("BRACKEN631_RP_1_ITEM", 4));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("BRACKEN631_RP_1_ITEM", -1));
	}
}
