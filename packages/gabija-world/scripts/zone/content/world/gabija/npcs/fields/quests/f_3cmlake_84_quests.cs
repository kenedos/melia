//--- Melia Script ----------------------------------------------------------
// Absenta Reservoir Quest NPCs
//--- Description -----------------------------------------------------------
// Elder Eloizard, Hunter Modis and Lanaldas tracking down the source of
// the reservoir's corruption.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class F3Cmlake84QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Lake83Mq04 = new QuestId(90004);
	private readonly static QuestId Lake83Mq05 = new QuestId(90005);
	private readonly static QuestId Mq01 = new QuestId(90010);
	private readonly static QuestId Mq02 = new QuestId(90011);
	private readonly static QuestId Mq03 = new QuestId(90012);
	private readonly static QuestId Mq04 = new QuestId(90013);
	private readonly static QuestId Mq05 = new QuestId(90014);
	private readonly static QuestId Mq06 = new QuestId(90015);
	private readonly static QuestId Sq01 = new QuestId(90016);
	private readonly static QuestId Sq02 = new QuestId(90017);
	private readonly static QuestId Sq03 = new QuestId(90018);

	public const string LabsBurnedVar = "Gabija.Quests.Lake84Sq03.Burned";
	private const string LabVar = "Gabija.Quests.Lake84Sq03.Lab";
	private const string BaitVar = "Gabija.Quests.Lake84Mq06.Baited";
	private const string HerbVar = "Gabija.Quests.Lake84Mq05.Herb";
	private const string FlowerVar = "Gabija.Quests.Lake84Sq01.Flower";

	private const int HerbsNeeded = 5;
	private const int FlowersNeeded = 5;
	private const int OilPerLab = 5;

	private static readonly TimeSpan PlantRespawn = TimeSpan.FromSeconds(30);

	private static readonly double[,] BlueHerbs =
	{
		{ 335.41, 306.25 }, { 301.88, 672.92 }, { 440.84, 709.35 }, { 460.03, 317.80 }, { 231.15, 512.79 },
	};

	private static readonly double[,] MarkaziFlowers =
	{
		{ 377.88, 1766.27, 26 }, { 429.34, 1654.72, 339 }, { 345.16, 1568.48, 90 }, { 453.73, 1358.38, 47 }, { 302.01, 1449.77, 90 }, { 254.05, 1674.27, 35 },
	};

	protected override void Load()
	{
		// Elder Eloizard
		//-------------------------------------------------------------------------
		AddConditionalNpc(152002, L("Elder Eloizard"), "3CMLAKE_84_OLDMAN", "f_3cmlake_84", -171.06, 324.49, 42, c => c.Quests.HasCompleted(Lake83Mq04), this.Elder);

		// Hunter Modis
		//-------------------------------------------------------------------------
		AddConditionalNpc(47245, L("Hunter Modis"), "3CMLAKE_84_HUNTER", "f_3cmlake_84", -41.97, 257.24, 282, c => c.Quests.Has(Mq02) || c.Quests.HasCompleted(Mq02), this.Modis);

		// Lanaldas
		//-------------------------------------------------------------------------
		AddNpc(147481, L("Lanaldas"), "3CMLAKE_84_PEOPLE1", "f_3cmlake_84", -187.29, 188.31, 135, this.Lanaldas);

		// The villagers' camp
		//-------------------------------------------------------------------------
		AddNpc(155055, "UnvisibleName", "3CMLAKE_84_TENT1", "f_3cmlake_84", -222.97, 362.46, 50);
		AddNpc(155055, "UnvisibleName", "3CMLAKE_84_TENT2", "f_3cmlake_84", -22.66, 192.43, 95);

		AddConditionalNpc(151028, "UnvisibleName", "3CMLAKE_84_BUCKET1", "f_3cmlake_84", -157.09, 358.95, 90, c => !c.Quests.Has(Mq01) && !c.Quests.HasCompleted(Mq01));
		AddConditionalNpc(153136, "UnvisibleName", "3CMLAKE_84_BUCKET2", "f_3cmlake_84", -157.09, 358.95, 90, c => c.Quests.Has(Mq01) || c.Quests.HasCompleted(Mq01));

		// The laboratory in the Nesuga Small Corridor
		//-------------------------------------------------------------------------
		AddConditionalNpc(153132, "UnvisibleName", "3CMLAKE_84_WORKBENCH1", "f_3cmlake_84", -209.92, 1803.39, 44, c => !IsBurningLabs(c) && !c.Quests.HasCompleted(Sq03), this.Workbench);

		AddConditionalNpc(153132, "UnvisibleName", "3CMLAKE_84_WORKBENCH3", "f_3cmlake_84", -209.92, 1803.39, 44, c => IsBurningLabs(c) || c.Quests.HasCompleted(Sq03), async dialog =>
		{
			await this.BurnLab(dialog, 1);
		});

		AddConditionalNpc(153132, "UnvisibleName", "3CMLAKE_83_WORKBENCH2", "f_3cmlake_83", -46.96, 859.67, 294, c => IsBurningLabs(c) || c.Quests.HasCompleted(Sq03), async dialog =>
		{
			await this.BurnLab(dialog, 2);
		});

		AddNpc(57013, "UnvisibleName", "3CMLAKE_84_OBJ1", "f_3cmlake_84", -117.43, 1712.16, 298);
		AddNpc(153133, "UnvisibleName", "3CMLAKE_84_OBJ2", "f_3cmlake_84", -209.49, 1703.33, 1);
		AddNpc(153131, "UnvisibleName", "3CMLAKE_84_OBJ3", "f_3cmlake_84", -147.21, 1839.75, 4);
		AddNpc(153131, "UnvisibleName", "3CMLAKE_84_OBJ4", "f_3cmlake_84", -92.39, 1841.42, 4);

		// Jeneuam Corridor, where the Hydra shows itself
		//-------------------------------------------------------------------------
		AddQuestTrigger("3CMLAKE_84_ENTER1", "f_3cmlake_84", -1241.48, -452.14, 100, async args =>
		{
			if (args.Initiator is Character character && character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
				character.Quests.StartQuestTrack(Mq02);

			await Task.CompletedTask;
		});

		// Blue Herbs
		//-------------------------------------------------------------------------
		for (var i = 0; i < BlueHerbs.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "3CMLAKE_84_HERB1" : "3CMLAKE_84_HERB1_" + number;

			AddConditionalNpc(153054, L("Blue Herb"), uniqueName, "f_3cmlake_84", BlueHerbs[i, 0], BlueHerbs[i, 1], 90,
				character => character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05),
				async dialog =>
				{
					GatherPlant(dialog.Player, Mq05, HerbVar + number, ItemId.F_3CMLAKE_84_MQ_ITEM2, HerbsNeeded, L("The blue herb here has already been picked."));
					await Task.CompletedTask;
				});
		}

		// Markazi Flowers on Svaigulys Hill
		//-------------------------------------------------------------------------
		for (var i = 0; i < MarkaziFlowers.GetLength(0); ++i)
		{
			var number = i + 1;
			var uniqueName = number == 1 ? "3CMLAKE_84_HERB2" : "3CMLAKE_84_HERB2_" + number;

			AddConditionalNpc(153067, L("Markazi Flower"), uniqueName, "f_katyn_12", MarkaziFlowers[i, 0], MarkaziFlowers[i, 1], MarkaziFlowers[i, 2],
				character => character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01),
				async dialog =>
				{
					GatherPlant(dialog.Player, Sq01, FlowerVar + number, ItemId.F_3CMLAKE_84_SQ_ITEM1, FlowersNeeded, L("The Markazi Flower here has already been picked."));
					await Task.CompletedTask;
				});
		}

		// Modis' trap at the Heralve Vacant Lot
		//-------------------------------------------------------------------------
		AddConditionalNpc(57194, "UnvisibleName", "3CMLAKE_84_TRAP", "f_3cmlake_84", 656.14, -218.72, 138, c => c.Quests.IsActive(Mq06) && !c.Quests.IsCompletable(Mq06), async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
				return;

			if (!character.Variables.Perm.GetBool(BaitVar, false))
			{
				if (character.Inventory.CountItem(ItemId.F_3CMLAKE_83_MQ_ITEM5) == 0)
				{
					character.ServerMessage(L("You need Modis' Special Bait."));
					return;
				}

				character.Inventory.Remove(ItemId.F_3CMLAKE_83_MQ_ITEM5, 1, InventoryItemRemoveMsg.Given);
				character.Variables.Perm.Set(BaitVar, true);
			}

			character.Quests.StartQuestTrack(Mq06);
			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Elder Eloizard's dialog at the Absenta Reservoir.
	/// </summary>
	private async Task Elder(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Elder Eloizard"));

		if (character.Quests.IsCompletable(Lake83Mq05))
		{
			await dialog.Msg(L("You're back? Ah, my granddaughter, all she does is worry about me."));
			await dialog.Msg(L("Oh, of course. Can you tell me about what was in the diary?"));
			await dialog.CompleteQuest(Lake83Mq05);
			return;
		}

		if (character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("So, did you find anything? A picture? What is it?"));
			await dialog.Msg(L("Let's see... This looks like that red statue you found earlier."));
			await dialog.Msg(L("By the way, while you were gone I saw something strange. I was taking a closer look at that statue you brought and... Suddenly, the water on the bucket next to it turned red."));
			await dialog.Msg(L("Oh... the smell. If something like this exists but much bigger, then that must be what's tainting the water."));
			await dialog.CompleteQuest(Mq01);
			return;
		}

		if (character.Quests.IsCompletable(Mq02))
		{
			var talked = await character.TimeActions.StartAsync(L("Talking about the Hydra"), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
			if (talked != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("Did I... Did I see that right? It looked like it had some sort of gem on its body."));
			await dialog.Msg(L("Hydras aren't usually that big, and their scales aren't red either. This monster... It looks exactly like the one on that painting you found."));
			await dialog.Msg(L("People have called that monster a mystical creature since long ago, but this is different."));
			await dialog.CompleteQuest(Mq02);
			return;
		}

		if (character.Quests.IsCompletable(Mq06))
		{
			await dialog.Msg(L("You're back! How did it go? Did you destroy the gem?"));
			await dialog.Msg(L("Good, good! Very good. That red statue you brought earlier suddenly exploded, gave me quite the scare."));
			await dialog.Msg(L("I don't expect the water to turn back to normal right away. It's going to take a while for all this water to become clean again."));
			await dialog.Msg(L("But things are getting better, I'm sure those who went to Orsha will come back. You really were a savior to our village!"));
			await dialog.Msg(L("You're leaving already? Alright, just make sure you stay away from Delmore Castle."));
			await dialog.Msg(L("I heard from one of our youngsters who went there... Apparently it's completely deserted and swarming with demons."));
			await dialog.Msg(L("Seems like they followed Orsha's immigration order, huh? No use in going to a place that's just full of demons."));
			await dialog.Msg(L("Anyway, thank you for everything. May the goddesses bless every step you take..."));
			await dialog.CompleteQuest(Mq06);
			return;
		}

		if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
		{
			var told = await character.TimeActions.StartAsync(L("Telling him what the diary says"), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
			if (told != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("A vessel? That's odd. Someone was doing an experiment here... is that what you mean?"));
			await dialog.Msg(L("Hm... I never heard about any demons doing any sort of experiments... But it could be related to those black hoods that Modis saw."));

			var answer = await dialog.SelectQuestOffer(Mq01, L("I told you I saw them walking in and out of the Nesuga Small Corridor. Would you look into it?"),
				Option(L("I will investigate it"), "accept"),
				Option(L("It's not time for that yet"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq01);
				character.LookAround();

				await dialog.Msg(L("I hope we can find something this time, too. I don't think I can rest now that I know it was someone who did this."));
			}
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			await dialog.Msg(L("Let's see, then... Where can we find something like this but big... If only we got rid of that, I'm sure the water will be back to normal."));

			var answer = await dialog.SelectQuestOffer(Mq02, L("Also, the monster drawn on the bottom here got me thinking. I feel like I know this monster, but it looks different somehow."),
				Option(L("Joining Forces"), "accept"),
				Option(L("Let me know if you find a way"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq02);
				character.LookAround();

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("A villager comes running: the Hydra has appeared at the Jeneuam Corridor!"), 5);
				await dialog.Msg(L("A Hydra... Go now! I'll be with you soon!"));
			}
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			await dialog.Msg(L("So that beast has a gem stuck on it now, huh? I'd say you'd have to get the beast out and kill it to get rid of the gem..."));
			await dialog.Msg(L("Talking is easy. Do you know why they call it mystical? You can't just go and grab it."));

			var answer = await dialog.SelectQuestOffer(Mq03, L("Hm... That's if we count only the folk from our village. If you help us catch it I think we might just have a chance. What do you say?"),
				Option(L("I'll do it"), "accept"),
				Option(L("Tell him that you need some time to think"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq03);
				character.Quests.CompleteObjective(Mq03, "talkModis");

				await dialog.Msg(L("Right. Thank you. You want to talk to Modis, then."));
				await dialog.Msg(L("He's the best hunter in the village. I'm sure Modis can think of a way to lure to beast."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("I hope we can find something this time, too. I don't think I can rest now that I know it was someone who did this."));
			character.Quests.ClearQuestTrack(Mq01);
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			await dialog.Msg(L("A Hydra... Go now! I'll be with you soon!"));
			character.Quests.ClearQuestTrack(Mq02);
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("Go see Modis. He's an expert at setting straps, you know."));
			return;
		}

		if (character.Quests.HasCompleted(Mq06))
		{
			await dialog.Msg(L("I know the water is going to be fine now..."));
			await dialog.Msg(L("But it's a shame we still don't know who did this or why they did it."));
			return;
		}

		if (character.Quests.HasCompleted(Mq02))
		{
			await dialog.Msg(L("What do you know, there's a gem on the beast."));
			await dialog.Msg(L("Anyway, with you and Modis here... destroying it won't be impossible."));
			return;
		}

		await dialog.Msg(L("What on earth could this be?"));
		await dialog.Msg(L("It wouldn't be this red if you threw a whole bottle of ink in it."));
	}

	/// <summary>
	/// Hunter Modis' dialog.
	/// </summary>
	private async Task Modis(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Hunter Modis"));

		if (character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("So you're the one our village chief can't stop talking about."));
			await dialog.Msg(L("I'm really glad you decided to help us."));
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (character.Quests.IsCompletable(Mq05))
		{
			await dialog.Msg(L("Did you get everything? I was able to make a nice, sturdy trap."));
			await dialog.CompleteQuest(Mq05);
			return;
		}

		if (character.Quests.IsCompletable(Sq01))
		{
			await dialog.Msg(L("Oh, that's it. Let's hope our friend has learned their lesson now."));
			await dialog.CompleteQuest(Sq01);
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			await dialog.Msg(L("Hunting is all the same, no matter the size of the beast. We just lure it into a trap with some bait and then tie its feet."));

			var answer = await dialog.SelectQuestOffer(Mq04, L("Considering the size of this one, though, making the trap is going to take some time. Can you find some bait meanwhile?"),
				Option(L("I'll get it"), "accept"),
				Option(L("I'll wait until you're done with the trap"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);

				await dialog.Msg(L("For the bait you want to get some Black Sawpent meat. We need some herbs too, to disguise our scent."));
			}
			return;
		}

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			await dialog.Msg(L("I set up the trap at the Heralve Vacant Lot. With the bait on it we should be able to catch the Hydra."));

			var answer = await dialog.SelectQuestOffer(Mq06, L("Make sure you're ready for this, that beast is dangerous."),
				Option(L("I'll go and catch the Hydra"), "accept"),
				Option(L("I need some time to prepare"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.Set(BaitVar, false);
				character.Quests.Start(Mq06);

				if (character.Inventory.CountItem(ItemId.F_3CMLAKE_83_MQ_ITEM5) == 0)
					character.Inventory.Add(ItemId.F_3CMLAKE_83_MQ_ITEM5, 1, InventoryAddType.PickUp);

				character.LookAround();

				await dialog.Msg(L("I wish this would make the water go back to normal..."));
				await dialog.Msg(L("If it goes well, tell the village chief immediately. May the goddesses bless you."));
			}
			return;
		}

		if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
		{
			await dialog.Msg(L("We have a problem. One of the villagers went to see the chief and drank some water from the bucket."));
			await dialog.Msg(L("I told them to be careful, that the water was probably contaminated, but..."));

			var answer = await dialog.SelectQuestOffer(Sq01, L("I wish we had some sort of painkiller... Can you please get some herbs for us?"),
				Option(L("I'll get it."), "accept"),
				Option(L("I don't think I have time for that"), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= MarkaziFlowers.GetLength(0); ++i)
					character.Variables.Temp.SetLong(FlowerVar + i, 0);

				character.Quests.Start(Sq01);

				await dialog.Msg(L("The water all turned red, you can't get any around here. You need to go all the way to Letas Stream."));
				await dialog.Msg(L("If you go to Svaigulys Hill, on the right side you'll see some yellow flowers. They're Markazi Flowers, it's what we use in times of need."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("If it wasn't for you, we would have never been able to deal with the Hydra."));
			await dialog.Msg(L("As you know most of us work as farmers."));
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("For the bait you want to get some Black Sawpent meat. We need some herbs too, to disguise our scent."));
			return;
		}

		if (character.Quests.IsCompletable(Mq06))
		{
			await dialog.Msg(L("You mean you defeated the Hydra? Wow, your skills are impressive."));
			await dialog.Msg(L("Hurry and go see the village chief. This is the best news we've had since after Medzio Diena."));
			return;
		}

		if (character.Quests.IsActive(Mq06))
		{
			await dialog.Msg(L("I sure hope we're right after all. You're the only one we can trust now."));
			await dialog.Msg(L("I pray that the goddesses give us strength..."));
			character.Quests.ClearQuestTrack(Mq06);
			return;
		}

		if (character.Quests.IsActive(Sq01))
		{
			await dialog.Msg(L("This area is contaminated with the red water, you'll have to go to Svaigulys Hill in Letas Stream. Find some Markazi Flowers. They're yellow."));
			await dialog.Msg(L("I wish we knew how to cure this. Right now all we can do is try and relieve the pain."));
			return;
		}

		await dialog.Msg(L("If it wasn't for you, we would have never been able to deal with the Hydra."));
		await dialog.Msg(L("As you know most of us work as farmers."));
	}

	/// <summary>
	/// Lanaldas' dialog.
	/// </summary>
	private async Task Lanaldas(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Lanaldas"));

		if (character.Quests.IsCompletable(Sq02))
		{
			await dialog.Msg(L("Thank you. This should be enough to make the oil we need!"));
			await dialog.CompleteQuest(Sq02);
			return;
		}

		if (character.Quests.IsCompletable(Sq03))
		{
			await dialog.Msg(L("Thank you. That makes me feel better."));
			await dialog.Msg(L("How can someone think of such a cruel experiment? How can they even... Am I right?"));
			await dialog.CompleteQuest(Sq03);

			if (character.Quests.HasCompleted(Sq03))
				character.LookAround();
			return;
		}

		if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
		{
			await dialog.Msg(L("It's revolting to think that this red water is all because of some experiment. Because of it our lives have been threatened."));
			await dialog.Msg(L("Why did the people in the black hoods do such a thing? I just want to burn down the whole laboratory so they can't even think of doing it again."));

			var answer = await dialog.SelectQuestOffer(Sq02, L("Thing is... I can't fight the monsters and burn down the lab on my own. Our people want to leave as fast as possible."),
				Option(L("I'll help you so something like that doesn't happen again"), "accept"),
				Option(L("It's too dangerous; just hold on"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Sq02);

				await dialog.Msg(L("Really? I knew we were on the same page."));
				await dialog.Msg(L("The oil I have is not enough, though. First, I want to ask you to collect some Rajapearlite lard."));
			}
			return;
		}

		if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
		{
			await dialog.Msg(L("Now, pour the oil around the laboratories and set fire to them. We can't let this type of experiment happen ever again..."));

			var answer = await dialog.SelectQuestOffer(Sq03, L("So one of the laboratories is here at the Nesuga Small Corridor. There's another one in the Wandering Sanctuary at the Pelke Shrine Ruins."),
				Option(L("I'll go there"), "accept"),
				Option(L("Waiting it out is definitely the best"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.Set(LabVar + 1, false);
				character.Variables.Perm.Set(LabVar + 2, false);
				character.Variables.Perm.SetInt(LabsBurnedVar, 0);

				character.Quests.Start(Sq03);

				var oil = OilPerLab * 2 - character.Inventory.CountItem(ItemId.F_3CMLAKE_84_SQ_ITEM2);
				if (oil > 0)
					character.Inventory.Add(ItemId.F_3CMLAKE_84_SQ_ITEM2, oil, InventoryAddType.PickUp);

				character.LookAround();
			}
			return;
		}

		if (character.Quests.IsActive(Sq02))
		{
			await dialog.Msg(L("Why on earth would someone do an experiment like that? It's atrocious."));
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("Those filthy people... Next time I see the black hoods I need to have a word with them."));
			return;
		}

		if (character.Quests.HasCompleted(Sq03))
		{
			await dialog.Msg(L("I think we need to keep an eye on this place to keep things like this from happening again."));
			return;
		}

		await dialog.Msg(L("We need to find it and eliminate it as soon as possible."));
		await dialog.Msg(L("Otherwise our village is doomed."));
	}

	/// <summary>
	/// The worktable in the Nesuga Small Corridor, where the black hoods
	/// left a burnt painting behind.
	/// </summary>
	private async Task Workbench(Dialog dialog)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Mq01) || character.Quests.IsCompletable(Mq01))
			return;

		if (character.Inventory.CountItem(ItemId.F_3CMLAKE_84_MQ_ITEM1) == 0)
		{
			var searched = await character.TimeActions.StartAsync(L("Investigating"), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));
			if (searched != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.F_3CMLAKE_84_MQ_ITEM1, 1, InventoryAddType.PickUp);
			character.ServerMessage(L("You have found a burnt painting."));
		}

		character.Quests.StartQuestTrack(Mq01);
	}

	/// <summary>
	/// Pours Lanaldas' oil over one of the black hoods' laboratories and
	/// sets it on fire.
	/// </summary>
	private async Task BurnLab(Dialog dialog, int number)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Sq03) || character.Quests.IsCompletable(Sq03))
			return;

		if (character.Variables.Perm.GetBool(LabVar + number, false))
		{
			character.ServerMessage(L("This laboratory has already been burned down."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.F_3CMLAKE_84_SQ_ITEM2) < OilPerLab)
		{
			character.ServerMessage(L("You need more Gooey Grease."));
			return;
		}

		character.Inventory.Remove(ItemId.F_3CMLAKE_84_SQ_ITEM2, OilPerLab, InventoryItemRemoveMsg.Given);
		character.Variables.Perm.Set(LabVar + number, true);

		var burned = character.Variables.Perm.GetInt(LabsBurnedVar, 0) + 1;
		character.Variables.Perm.SetInt(LabsBurnedVar, burned);

		dialog.Npc.PlayEffect("F_burstup001_fire", 1.5f);
		character.ServerMessage(LF("Laboratories burned down: {0}/{1}", Math.Min(burned, 2), 2));

		await Task.CompletedTask;
	}

	/// <summary>
	/// Picks a plant for a quest, which grows back after a while.
	/// </summary>
	private static void GatherPlant(Character character, QuestId questId, string plantVar, int itemId, int needed, string pickedMessage)
	{
		if (!character.Quests.IsActive(questId) || character.Quests.IsCompletable(questId))
			return;

		if (character.Inventory.CountItem(itemId) >= needed)
			return;

		var pickedAt = character.Variables.Temp.GetLong(plantVar, 0);
		if (pickedAt != 0 && DateTime.Now - new DateTime(pickedAt) < PlantRespawn)
		{
			character.ServerMessage(pickedMessage);
			return;
		}

		character.Variables.Temp.SetLong(plantVar, DateTime.Now.Ticks);
		character.Inventory.Add(itemId, 1, InventoryAddType.PickUp);
	}

	/// <summary>
	/// Returns whether the character is out to burn down the laboratories.
	/// </summary>
	private static bool IsBurningLabs(Character character)
		=> character.Quests.IsActive(Sq03);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 90010: Clear the Corruption (1)
//-----------------------------------------------------------------------------
public class F3Cmlake84Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90010);
		SetName(L("Clear the Corruption (1)"));
		SetDescription(L("Elder Aloizard says Modis once saw people with black hoods doing something at the Nesuga Small Corridor. Go to the Nesuga Small Corridor and see if you can find any clues."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_OLDMAN", "f_3cmlake_84", L("Talk to Elder Aloizard"), L("Talk to Elder Aloizard about the contents of the journal."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_WORKBENCH1", "f_3cmlake_84", L("Investigate the Nesuga Small Corridor"), L("Elder Aloizard says Modis once saw people with black hoods doing something at the Nesuga Small Corridor. Go to the Nesuga Small Corridor and see if you can find any clues."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_OLDMAN", "f_3cmlake_84", L("Deliver to Elder Aloizard"), L("You have found a burnt painting. Elder Aloizard could possibly know something about the images in the painting."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "F_3CMLAKE_84_MQ_01_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(90005, QuestStatus.Completed));

		AddObjective("findPainting", L("Investigate the Nesuga Small Corridor"), new CollectItemObjective("F_3CMLAKE_84_MQ_ITEM1", 1));
		AddObjective("killSlimes", L("Defeat the attacking monsters"), new KillObjective(6, "slime_dark_blue") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 550));
		AddReward(new TakeItemReward("F_3CMLAKE_84_MQ_ITEM1", 1));
	}
}

// 90011: Clear the Corruption (2)
//-----------------------------------------------------------------------------
public class F3Cmlake84Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90011);
		SetName(L("Clear the Corruption (2)"));
		SetDescription(L("A resident of the village came running, saying the Hydra has appeared. Go and check the Jeneuam Corridor, where the Hydra is said to be."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_OLDMAN", "f_3cmlake_84", L("Talk to Elder Aloizard"), L("Elder Aloizard seems to have recognized the images in the painting. Ask Elder Aloizard about the painting,"));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_ENTER1", "f_3cmlake_84", L("Go to the Jeneuam Corridor and find the Hydra"), L("A resident of the village came running, saying the Hydra has appeared. Go and check the Jeneuam Corridor, where the Hydra is said to be."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_OLDMAN", "f_3cmlake_84", L("Talk to Elder Aloizard"), L("A colossal Hydra with a red gem on its body appeared but quickly fled. Return to Elder Aloizard."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "F_3CMLAKE_84_MQ_02_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(90010, QuestStatus.Completed));

		AddObjective("findHydra", L("Go to the Jeneuam Corridor and find the Hydra"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new ItemReward("Vis", 550));
	}
}

// 90012: Clear the Corruption (3)
//-----------------------------------------------------------------------------
public class F3Cmlake84Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90012);
		SetName(L("Clear the Corruption (3)"));
		SetDescription(L("Elder Aloizard believes that, with your help, they might have a chance to destroy the red gem on the Hydra's body. Go see Hunter Modis and find a way to lure the Hydra."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_OLDMAN", "f_3cmlake_84", L("Talk to Elder Aloizard"), L("The monster depicted in the painting is the Hydra. Ask Elder Aloizard about what to do next."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Talk to Hunter Modis"), L("Elder Aloizard believes that, with your help, they might have a chance to destroy the red gem on the Hydra's body. Go see Hunter Modis and find a way to lure the Hydra."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Talk to Hunter Modis"), L("Elder Aloizard believes that, with your help, they might have a chance to destroy the red gem on the Hydra's body. Go see Hunter Modis and find a way to lure the Hydra."));

		AddPrerequisite(new QuestStatusPrerequisite(90011, QuestStatus.Completed));

		AddObjective("talkModis", L("Talk to Hunter Modis"), new ManualObjective());
	}
}

// 90013: Clear the Corruption (4)
//-----------------------------------------------------------------------------
public class F3Cmlake84Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90013);
		SetName(L("Clear the Corruption (4)"));
		SetDescription(L("While Hunter Modis is making a trap to catch the Hydra, he wants you to collect Sawpent meat to use as bait."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Talk to Hunter Modis"), L("When it comes to bait and traps, no one knows better than Hunter Modis. Ask Hunter Modis about how to catch the Hydra."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Collect Sawpent Meat"), L("While Hunter Modis is making a trap to catch the Hydra, he wants you to collect Sawpent meat to use as bait."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Collect Sawpent Meat"), L("While Hunter Modis is making a trap to catch the Hydra, he wants you to collect Sawpent meat to use as bait."));

		AddPrerequisite(new QuestStatusPrerequisite(90012, QuestStatus.Completed));

		AddObjective("collectMeat", L("Defeat Sawpents and collect Bait Meat"), new CollectItemObjective("F_3CMLAKE_84_MQ_ITEM3", 8));
		AddPityDrop("F_3CMLAKE_84_MQ_ITEM3", 1.0f, 0, 1, "Sowpent");
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client ends this quest itself and chains straight into the herb gathering.
		character.Quests.Complete(this.QuestId);
		character.Quests.Start(new QuestId(90014));
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("You have collected enough bait. Now, collect blue herbs to disguise the scent of humans."), 5);
		character.LookAround();
	}
}

// 90014: Clear the Corruption (5)
//-----------------------------------------------------------------------------
public class F3Cmlake84Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90014);
		SetName(L("Clear the Corruption (5)"));
		SetDescription(L("You have collected enough bait. Now, collect blue herbs to disguise the scent of humans."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Collect Blue Herbs"), L("You have collected enough bait. Now, collect blue herbs to disguise the scent of humans."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_HERB1", "f_3cmlake_84", L("Collect Blue Herbs"), L("You have collected enough bait. Now, collect blue herbs to disguise the scent of humans."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Deliver to Hunter Modis"), L("You have collected meat to use as bait and herbs to disguise your human scent. Bring the items to Hunter Modis."));

		AddPrerequisite(new QuestStatusPrerequisite(90013, QuestStatus.Completed));

		AddObjective("haveMeat", L("Bait Meat"), new CollectItemObjective("F_3CMLAKE_84_MQ_ITEM3", 8));
		AddObjective("collectHerbs", L("Collect Blue Herbs"), new CollectItemObjective("F_3CMLAKE_84_MQ_ITEM2", 5));

		AddReward(new ItemReward("expCard5", 4));
		AddReward(new ItemReward("Vis", 800));
		AddReward(new TakeItemReward("F_3CMLAKE_84_MQ_ITEM3", -1));
		AddReward(new TakeItemReward("F_3CMLAKE_84_MQ_ITEM2", -1));
	}
}

// 90015: Clear the Corruption (6)
//-----------------------------------------------------------------------------
public class F3Cmlake84Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90015);
		SetName(L("Clear the Corruption (6)"));
		SetDescription(L("Hunter Modis has set up a trap at the Heralve Vacant Lot. Place the bait on the trap to lure the Tyronas Hydra and defeat it."));
		SetType(QuestType.Main);
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Talk to Hunter Modis"), L("Hunter Modis seems to have finished all preparations to lure the Hydra. Talk to Hunter Modis."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_TRAP", "f_3cmlake_84", L("Lure Tyronas Hydra"), L("Hunter Modis has set up a trap at the Heralve Vacant Lot. Place the bait on the trap to lure the Tyronas Hydra and defeat it."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_OLDMAN", "f_3cmlake_84", L("Talk to Elder Aloizard"), L("After defeating the Hydra, the red gem on its body shattered to pieces. Go and tell Elder Aloizard about the Tyronas Hydra."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "F_3CMLAKE_84_MQ_06_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(90014, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(58));

		AddObjective("killHydra", L("Defeat Tyronas Hydra"), new KillObjective(1, "boss_hydra_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 5));
		AddReward(new ItemReward("Vis", 1100));
		AddReward(new ItemReward("TreasureboxKey2", 1));
		AddReward(new TakeItemReward("F_3CMLAKE_83_MQ_ITEM5", -1));
	}
}

// 90016: You Can't Drink That
//-----------------------------------------------------------------------------
public class F3Cmlake84Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90016);
		SetName(L("You Can't Drink That"));
		SetDescription(L("Someone from the village accidentally drank the contaminated red water. Go to the right side of Svaigulys Hill in Letas Stream and collect Markazi Flowers to ease their pain."));
		SetType(QuestType.Sub);
		SetLocation("f_3cmlake_84", "f_katyn_12");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Talk to Hunter Modis"), L("Hunter Modis seems to have a problem at hand. Talk to Hunter Modis."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_HERB2", "f_katyn_12", L("Collect Markazi Flowers"), L("Someone from the village accidentally drank the contaminated red water. Go to the right side of Svaigulys Hill in Letas Stream and collect Markazi Flowers to ease their pain."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_HUNTER", "f_3cmlake_84", L("Deliver to Hunter Modis"), L("You have collected enough Markazi Flowers now. Bring them to Hunter Modis."));

		AddPrerequisite(new QuestStatusPrerequisite(90015, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(58));

		AddObjective("collectFlowers", L("Collect Markazi Flowers"), new CollectItemObjective("F_3CMLAKE_84_SQ_ITEM1", 5));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 500));
		AddReward(new TakeItemReward("F_3CMLAKE_84_SQ_ITEM1", -1));
	}
}

// 90017: Recurrence Prevention (1)
//-----------------------------------------------------------------------------
public class F3Cmlake84Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90017);
		SetName(L("Recurrence Prevention (1)"));
		SetDescription(L("Lanaldas wants to burn down the laboratory as retaliation. Go and collect lard from Black Rajapearlites to make flammable oil."));
		SetType(QuestType.Sub);
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_PEOPLE1", "f_3cmlake_84", L("Talk to Lanaldas"), L("Lanaldas is furious at the fact that the red water of the reservoir was the fault of someone's experiment. Talk to Lanaldas."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_PEOPLE1", "f_3cmlake_84", L("Collect lard from Black Rajapearlites"), L("Lanaldas wants to burn down the laboratory as retaliation. Go and collect lard from Black Rajapearlites to make flammable oil."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_PEOPLE1", "f_3cmlake_84", L("Deliver to Lanaldas"), L("You have collected enough lard from Black Rajapearlites. Bring it to Lanaldas."));

		AddPrerequisite(new QuestStatusPrerequisite(90012, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(58));

		AddObjective("collectLard", L("Collect Rajapearlite Lard"), new CollectItemObjective("F_3CMLAKE_84_SQ_ITEM3", 8));
		AddPityDrop("F_3CMLAKE_84_SQ_ITEM3", 1.0f, 0, 1, "Rajapearlite_purple");

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("Vis", 550));
		AddReward(new TakeItemReward("F_3CMLAKE_84_SQ_ITEM3", -1));
	}
}

// 90018: Recurrence Prevention (2)
//-----------------------------------------------------------------------------
public class F3Cmlake84Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90018);
		SetName(L("Recurrence Prevention (2)"));
		SetDescription(L("Lanaldas wants you to burn down the laboratory to prevent future experiments. One is located in the Nesuga Small Corridor of the Absenta Reservoir, the other in the Wandering Sanctuary of the Pelke Shrine Ruins."));
		SetType(QuestType.Sub);
		SetLocation("f_3cmlake_84", "f_3cmlake_83");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "3CMLAKE_84_PEOPLE1", "f_3cmlake_84", L("Talk to Lanaldas"), L("The oil is ready now. Talk to Lanaldas before burning down the laboratory."));
		SetPhase(QuestStatus.InProgress, "3CMLAKE_84_PEOPLE1", "f_3cmlake_84", L("Set fire to the laboratory"), L("Lanaldas wants you to burn down the laboratory to prevent future experiments. One is located in the Nesuga Small Corridor of the Absenta Reservoir, the other in the Wandering Sanctuary of the Pelke Shrine Ruins."));
		SetPhase(QuestStatus.Success, "3CMLAKE_84_PEOPLE1", "f_3cmlake_84", L("Talk to Lanaldas"), L("You have burned down all the laboratories. Go and tell Lanaldas."));

		AddPrerequisite(new QuestStatusPrerequisite(90017, QuestStatus.Completed));

		AddObjective("burnLabs", L("Set fire to the laboratory"), new VariableCheckObjective(F3Cmlake84QuestNpcsScript.LabsBurnedVar, 2, isPermanent: true));

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Vis", 600));
	}
}
