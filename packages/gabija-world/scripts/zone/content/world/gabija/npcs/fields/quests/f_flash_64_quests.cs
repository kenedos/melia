//--- Melia Script ----------------------------------------------------------
// Inner Enceinte District Quest NPCs
//--- Description -----------------------------------------------------------
// The Knights of Kaliss working the Petrified City, the grave robber who can
// get into the Fortress of the Land, and the four class masters camped here.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FFlash64QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Sq01 = new QuestId(8845);
	private readonly static QuestId Sq02 = new QuestId(8846);
	private readonly static QuestId Sq03 = new QuestId(8847);
	private readonly static QuestId Sq05 = new QuestId(8849);
	private readonly static QuestId Sq06 = new QuestId(8850);
	private readonly static QuestId Sq07 = new QuestId(8851);
	private readonly static QuestId Sq08 = new QuestId(8852);
	private readonly static QuestId Sq09 = new QuestId(8853);
	private readonly static QuestId Sq10 = new QuestId(8854);
	private readonly static QuestId Mq01 = new QuestId(8855);
	private readonly static QuestId Mq02 = new QuestId(8856);
	private readonly static QuestId Mq03 = new QuestId(8857);
	private readonly static QuestId Cannoneer7 = new QuestId(30118);
	private readonly static QuestId Musketeer7 = new QuestId(30119);
	private readonly static QuestId Under66Sq010 = new QuestId(50062);
	private readonly static QuestId Under67Hq1 = new QuestId(50259);
	private readonly static QuestId Flash64Hq1 = new QuestId(50267);
	private readonly static QuestId Lancer8 = new QuestId(90157);
	private readonly static QuestId Murmillo8 = new QuestId(90158);
	private readonly static QuestId Cannoneer8 = new QuestId(90161);
	private readonly static QuestId Musketeer8 = new QuestId(90162);

	private const int WillsNeeded = 5;
	private const int RecordsNeeded = 5;
	private const int BonfiresToLight = 3;

	// The Royal Army guards dying at the Stone Icicle Square.
	private readonly static int[] VictimModels = { 154023, 154023, 154023, 154023, 154026, 154026, 154026, 154029, 154029, 154029 };

	private readonly static double[,] VictimSpots =
	{
		{ 124.06, -517.75 }, { -229.97, -519.91 }, { -347.22, -440.55 }, { -187.26, -349.21 },
		{ -318.01, -656.54 }, { 12.98, -386.89 }, { -194.40, -220.86 },
		{ -131.23, -522.34 }, { -488.55, -603.27 }, { -90.79, -738.13 },
	};

	private readonly static double[] VictimFacings = { -69, 90, 34, 90, 147, 90, 186, 260, 71, 148 };

	// The petrified victims of Vienti Fortress the elixir is used on.
	private readonly static int[] PetrifiedModels =
	{
		154023, 154023, 154023, 154023, 154023, 154023, 154023, 154023,
		154024, 154024, 154024, 154024,
		154030, 154030, 154030, 154030, 154030,
		154031, 154031, 154031,
	};

	private readonly static double[,] PetrifiedSpots =
	{
		{ -1488.26, -134.86 }, { -1339.24, -230.76 }, { -1293.89, 92.75 }, { -918.67, -600.08 },
		{ -942.61, -295.99 }, { -986.59, 185.88 }, { -1222.98, 332.83 }, { -879.61, 721.28 },
		{ -1495.08, -19.45 }, { -1179.68, -516.00 }, { -1235.59, 538.94 }, { -909.50, 533.62 },
		{ -1137.30, -106.37 }, { -1283.52, 237.22 }, { -706.18, 760.47 }, { -1179.38, 642.01 },
		{ -1528.93, -517.26 },
		{ -1397.14, -292.65 }, { -1027.73, -157.00 }, { -871.26, 396.07 },
	};

	// The bonfires Edita scented for the Frosted souls.
	private readonly static double[,] Bonfires =
	{
		{ -1181.16, -354.16 }, { -1246.27, 238.41 }, { -1071.97, 657.20 }, { -685.68, 753.57 },
		{ -1437.44, -450.37 }, { -1371.18, 15.95 }, { -959.57, -224.94 }, { -1043.66, 97.78 },
		{ -973.70, 513.25 },
	};

	// The stone slabs of the Ruklys era, out on the gathering place.
	private readonly static double[,] RecordSlabs =
	{
		{ 4.75, 1444.94 }, { 132.46, 1595.13 }, { 125.24, 1734.52 }, { -13.23, 2047.85 },
		{ -244.22, 1972.20 }, { -53.40, 1892.05 }, { -297.58, 1718.13 }, { -369.87, 1782.01 },
		{ -207.40, 1419.50 }, { -90.39, 1404.17 }, { -36.56, 1648.13 },
	};

	private readonly static double[] RecordFacings = { 214, 208, 52, 90, 203, -70, 21, 90, -60, 24, 90 };

	// The Musketeer Master's practice poles.
	private readonly static double[,] PracticePoles =
	{
		{ -450.79, -1710.91 }, { -491.80, -1635.80 }, { -442.97, -1575.67 },
	};

	protected override void Load()
	{
		// Wilhelmina Carriot
		//-------------------------------------------------------------------------
		AddNpc(20106, L("[Knights of Kaliss]{nl}Wilhelmina Carriot"), "FLASH64_KARRIAT", "f_flash_64", -365.77, -1319.78, 17, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Wilhelmina Carriot"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("You're as great as the rumors say."));
				await dialog.Msg(L("The Knights of Kaliss need heroes like you."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("I won't blame you if you eat one secretly."));
				await dialog.Msg(L("But you don't seem to be that kind of a person."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Finally, the records which weren't in hands of the Royal Army guards are here."));
				character.Inventory.RemoveItem(ItemId.FLASH64_MQ_01_ITEM, RecordsNeeded);
				character.Inventory.Add(ItemId.FLASH64_MQ_03_ITEM, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Mq02, "tradeRecords");
				character.ServerMessage(L("Wilhelmina hands over a Silence Scroll. Take it back to Amanda."));
				return;
			}

			if (character.Quests.IsActive(Under66Sq010) && character.Quests.IsCompletable(Under66Sq010))
			{
				await dialog.Msg(L("This is enough."));
				await dialog.Msg(L("Thank you for helping our research."));
				await dialog.CompleteQuest(Under66Sq010);
				return;
			}

			if (character.Quests.IsActive(Under67Hq1) && character.Quests.IsCompletable(Under67Hq1))
			{
				await dialog.Msg(L("This should be enough."));
				await dialog.Msg(L("I'll select a few talented knights to enter the fortress."));
				await dialog.CompleteQuest(Under67Hq1);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("The number of monsters in Kovos Hall Site is increasing. We don't even know what is leading them here."),
					Option(L("Tell her that you would help get rid of monsters"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("I heard you have lots of battle experiences."));
					await dialog.Msg(L("I even want to suggest you to join the Knights of Kaliss."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("Stone debris and dust blow a lot in this area so many soldiers and knights cough severely. The headquarters send us refreshing candies mixed with medicinal herbs."),
					Option(L("I'll retrieve it"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("I would like to give you some if we have extras."));
					await dialog.Msg(L("Even the grave robbers are preying on the candies."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("Soldiers patrolling this area were swept by stone frost while passing by the Stone Icicle Square some time ago."),
					Option(L("I'll go there"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("I understand their loyalty for the kingdom, but when I see them make people do impossible things that lead to deaths..."));
					await dialog.Msg(L("I don't know who is the monster anymore."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Under66Sq010) && character.Quests.MeetsPrerequisites(Under66Sq010))
			{
				var answer = await dialog.SelectQuestOffer(Under66Sq010, L("That.. is that from the monsters of the Fortress of the Land? Ruklys' army used to have it in the past."),
					Option(L("I will collect them"), "accept"),
					Option(L("I won't go in again"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Under66Sq010);
					await dialog.Msg(L("I didn't expect the monsters at the Fortress of the Land have this.."));
					await dialog.Msg(L("This is very interesting."));
					return;
				}
			}

			if (!character.Quests.Has(Under67Hq1) && character.Quests.MeetsPrerequisites(Under67Hq1))
			{
				var answer = await dialog.SelectQuestOffer(Under67Hq1, L("We've decided to have a few Knights of Kaliss infiltrate the location undercover. Paying Amanda costs a fortune."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("You should pay a reasonable price"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Under67Hq1);
					character.Inventory.Add(ItemId.UNDER67_HIDDENQ1_ITEM1, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Please write down anything we should mind inside the Fortress of the Land on these notes."));
					await dialog.Msg(L("Feel free to be very specific."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("I heard you have lots of battle experiences."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Even the grave robbers are preying on the candies."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("I understand their loyalty for the kingdom, but when I see them make people do impossible things that lead to deaths..."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Bring me those records and I will find you something that works on a Gargoyle."));
				return;
			}

			if (character.Quests.IsActive(Under66Sq010))
			{
				await dialog.Msg(L("I didn't expect the monsters at the Fortress of the Land have this.."));
				return;
			}

			if (character.Quests.IsActive(Under67Hq1))
			{
				await dialog.Msg(L("I pondered deeply before deciding to have the Knights of Kaliss enter the fortress."));
				return;
			}

			await dialog.Msg(L("A Kaliss officer holding a dig that the Royal Army will not pay for."));
		});

		// Bokor Edita
		//-------------------------------------------------------------------------
		AddNpc(154019, L("[Knights of Kaliss]{nl}Bokor Edita"), "FLASH64_EDITA", "f_flash_64", -443.77, 357.74, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bokor Edita"));

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("That's enough ingredients to make the Liberation Elixir."));
				await dialog.Msg(L("Now, let's start."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (character.Quests.IsActive(Sq06) && character.Quests.IsCompletable(Sq06))
			{
				await dialog.Msg(L("There was nothing to find out about other than the fact that they were all trembling in panic and fear when they died."));
				await dialog.Msg(L("I guess I better look for even older victims."));
				await dialog.CompleteQuest(Sq06);
				return;
			}

			if (character.Quests.IsActive(Sq07) && character.Quests.IsCompletable(Sq07))
			{
				await dialog.Msg(L("The pain, anger... The betrayal and fear..."));
				await dialog.Msg(L("I can still feel the feelings from that day."));
				await dialog.CompleteQuest(Sq07);
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("In order to restore the city, the Knights of Kaliss hired many experts. But they just recruit people without any support afterwards."),
					Option(L("I'll help"), "accept"),
					Option(L("About the curse of the Petrifying Frost"), "explain"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Almost all curses will wear off when the one who cast it dies."));
					await dialog.Msg(L("That's why I think the curse of the petrification is similar to the one at Pilgrim's Way."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("Please collect the Dark Crystals from the monsters at Vienti Fortress."));
					await dialog.Msg(L("I will try to make an elixir that would pull out the controlled spirit."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq06) && character.Quests.MeetsPrerequisites(Sq06))
			{
				var answer = await dialog.SelectQuestOffer(Sq06, L("I will give you this Liberation Elixir. Use it on the petrified victims at Vienti Fortress. Their souls trapped in their body might tell you something about what they last witnessed."),
					Option(L("I'll go there"), "accept"),
					Option(L("Reject"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq06);
					character.Inventory.Add(ItemId.FLASH64_SQ_06_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("The angry souls might attack you."));
					await dialog.Msg(L("Please be careful."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq07) && character.Quests.MeetsPrerequisites(Sq07))
			{
				var answer = await dialog.SelectQuestOffer(Sq07, L("We can't even find the traces of the bodies of the victims who were sacrificed on the day of Ruklys' rebellion. Those angry souls are deeply permeated into the city."),
					Option(L("I'll come back after I light up the bonfire"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq07);
					await dialog.Msg(L("I've put the scent of butterflies that can be obtained from deep inside Kateen Forest into the bonfire."));
					await dialog.Msg(L("It has the power to comfort the souls."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("Besides the knowledge of the spells I possess, I am trying to approach it with various methods."));
				await dialog.Msg(L("I will lift this curse for sure."));
				return;
			}

			if (character.Quests.IsActive(Sq06))
			{
				await dialog.Msg(L("The angry souls might attack you. Please be careful."));
				return;
			}

			if (character.Quests.IsActive(Sq07))
			{
				await dialog.Msg(L("The scent in the bonfire has the power to comfort the souls."));
				return;
			}

			await dialog.Msg(L("A bokor working a curse that did not end when the one who cast it did."));
		});

		// Alchemist Saliamonas
		//-------------------------------------------------------------------------
		AddNpc(154022, L("[Knights of Kaliss]{nl}Alchemist Saliamonas"), "FLASH64_SALIAMONS", "f_flash_64", 29.38, 357.16, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Alchemist Saliamonas"));

			if (character.Quests.IsActive(Sq08) && character.Quests.IsCompletable(Sq08))
			{
				await dialog.Msg(L("You weren't able to save any of them?"));
				await dialog.Msg(L("I guess this is not a good way. I will take back what is left."));
				await dialog.CompleteQuest(Sq08);
				return;
			}

			if (character.Quests.IsActive(Sq09) && character.Quests.IsCompletable(Sq09))
			{
				await dialog.Msg(L("This should be enough for about five tests. Good work."));
				await dialog.CompleteQuest(Sq09);
				return;
			}

			if (character.Quests.IsActive(Sq10) && character.Quests.IsCompletable(Sq10))
			{
				await dialog.Msg(L("Did you defeat it? Great job!"));
				await dialog.Msg(L("Now I don't have to fear this place being torn down until another monster shows up."));
				await dialog.CompleteQuest(Sq10);
				return;
			}

			if (!character.Quests.Has(Sq08) && character.Quests.MeetsPrerequisites(Sq08))
			{
				var answer = await dialog.SelectQuestOffer(Sq08, L("Even trees or paper turn into stones besides living creatures... I am very interested in these non-senses."),
					Option(L("I'll help you"), "accept"),
					Option(L("About the curse of petrification"), "explain"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("I personally think the curse of the petrification is a chemical reaction."));
					await dialog.Msg(L("Which means you can't restore the things that are once petrified before."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq08);
					character.Inventory.Add(ItemId.FLASH64_SQ_08_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Thanks. First, would you use this melting solution on the monsters that are struggling due to the Petrifying Frost?"));
					await dialog.Msg(L("I mean Neiveikiama Castle. If it's useful, you would be able to use it on the humans as well."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq09) && character.Quests.MeetsPrerequisites(Sq09))
			{
				var answer = await dialog.SelectQuestOffer(Sq09, L("I think we should use different drugs and test as much as we can. But then we will need a lot of samples."),
					Option(L("Tell him that you would bring it"), "accept"),
					Option(L("I better get going"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq09);
					await dialog.Msg(L("I'm still thinking so can you come back later?"));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq10) && character.Quests.MeetsPrerequisites(Sq10))
			{
				var answer = await dialog.SelectQuestOffer(Sq10, L("May those victims rest in peace. However, like the river that attracts those who are thirsty, this place is astounding for my thirst for knowledge."),
					Option(L("I will defeat it"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq10);
					await dialog.Msg(L("If it has a brain, how can it go around ruining this?"));
					await dialog.Msg(L("Oh well, maybe that's why it's a monster."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq08))
			{
				await dialog.Msg(L("Even if it works on just one or two from ten of them, that would be great."));
				await dialog.Msg(L("I trust on my knowledge and skills."));
				return;
			}

			if (character.Quests.IsActive(Sq09))
			{
				await dialog.Msg(L("I'm still thinking so can you come back later?"));
				return;
			}

			if (character.Quests.IsActive(Sq10))
			{
				await dialog.Msg(L("If it has a brain, how can it go around ruining this?"));
				return;
			}

			await dialog.Msg(L("An alchemist who calls the Petrifying Frost a chemical reaction and is very pleased with the idea."));
		});

		// Grave Robber Amanda
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "FLASH64_AMANDA", "f_flash_64", -586.49, 1560.29, 90, this.IsAmandaOutside, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("You've seen the Gargoyle Sculpture at the gathering place, right?"));
				await dialog.Msg(L("That's the Gargoyle."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("A scroll? I thought she'd give us something like a ballista or a magical weapon..."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("The scroll Wilhelmina gave us is awesome!"));
				await dialog.Msg(L("I didn't hear a thing even from right beside."));
				await dialog.CompleteQuest(Mq03);
				character.ServerMessage(L("Follow Grave Robber Amanda into the Fortress of the Land."));
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("So you are the famous Revelator, right?"));

				var answer = await dialog.SelectQuestOffer(Mq01, L("Hmm... I have something to suggest to you. Why don't you work with me?"),
					Option(L("I am on the same side with the Knights of Kaliss"), "accept"),
					Option(L("I am on the same side with the Kingdom Soldiers"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("The Knights of Kaliss? Good."));
					await dialog.Msg(L("The Fortress of the Land can't be entered since there are too many Royal Army guards. But if you help me with the plan, I can take you with me."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("Have you met Wilhelmina at Kovos Hall Site before? She is in charge of the Knights of Kaliss, but she is full of hatred towards the Royal Army guards."),
					Option(L("Leave it to me"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("The Knights of Kaliss possesses many competent wizards."));
					await dialog.Msg(L("They will give us what we need."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Please get rid of Gargoyle. I need to pack my bag at the Fortress of the Land. It will not be hard for you."),
					Option(L("I will defeat Gargoyle"), "accept"),
					Option(L("They are too strong to face against"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					character.Inventory.Add(ItemId.FLASH64_MQ_03_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();
					await dialog.Msg(L("It's a Monocle that enables one to see the true nature of the special force."));
					await dialog.Msg(L("We will be able to find the room with the treasure easily with it."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Be careful not to get detected by kingdom soldiers."));
				await dialog.Msg(L("Even if you get caught, just laugh and pretend there's nothing."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The Knights of Kaliss possesses many competent wizards. They will give us what we need."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The Gargoyle is still sitting on the gathering place."));
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			await dialog.Msg(L("A grave robber with a way into the Fortress of the Land and a price for it."));
		});

		// Cannoneer Master
		//-------------------------------------------------------------------------
		AddNpc(151071, L("[Cannoneer Master]{nl}Eda Saker Bazaras"), "CANNONEER_MASTER", "f_flash_64", -399.15, -1350.01, 104, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Cannoneer Master"));

			if (character.Quests.IsActive(Cannoneer7) && character.Quests.IsCompletable(Cannoneer7))
			{
				await dialog.Msg(L("Good. Was this a good lesson? Am I a good teacher?"));
				await dialog.CompleteQuest(Cannoneer7);
				return;
			}

			if (character.Quests.IsActive(Cannoneer8) && character.Quests.IsCompletable(Cannoneer8))
			{
				await dialog.Msg(L("Like always, you did not betray my faith in you!"));
				await dialog.CompleteQuest(Cannoneer8);
				return;
			}

			if (!character.Quests.Has(Cannoneer7) && character.Quests.MeetsPrerequisites(Cannoneer7))
			{
				var answer = await dialog.SelectQuestOffer(Cannoneer7, L("You enrolled to become a Cannoneer? Good choice, hotshot."),
					Option(L("I will learn it"), "accept"),
					Option(L("I will return next time"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Cannoneer7);
					await dialog.Msg(L("Good! Alright, hotshot. I can only assume that you will remember what I will teach you?"));
					await dialog.Msg(L("The flying monsters are aerial-types and the ones that move on the ground are walking-types. Face against both types of monsters."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Cannoneer8) && character.Quests.MeetsPrerequisites(Cannoneer8))
			{
				await dialog.Msg(L("Ah, there you are. You are just in time. I have a favor to ask of you."));

				var answer = await dialog.SelectQuestOffer(Cannoneer8, L("Near the kingdom camp on Steel Heights, recently there has been sightings of strange objects."),
					Option(L("Don't worry. I will do it"), "accept"),
					Option(L("If it is something dangerous, I can't do it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Cannoneer8);
					await dialog.Msg(L("I have complete faith in you."));
					await dialog.Msg(L("But don't let the monsters surround you."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Cannoneer7))
			{
				await dialog.Msg(L("The flying monsters are aerial-types and the ones that move on the ground are walking-types."));
				await dialog.Msg(L("Face against both types of monsters. Understood?"));
				return;
			}

			if (character.Quests.IsActive(Cannoneer8))
			{
				await dialog.Msg(L("I have complete faith in you. But don't let the monsters surround you."));
				return;
			}

			await dialog.Msg(L("A gunner who would rather teach a cannon than carry one."));
		});

		// Musketeer Master
		//-------------------------------------------------------------------------
		AddNpc(151072, L("[Musketeer Master]{nl}Alloden Marzarine"), "MUSKETEER_MASTER", "f_flash_64", -314.86, -1315.04, -18, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Musketeer Master"));

			if (character.Quests.IsActive(Musketeer7) && character.Quests.IsCompletable(Musketeer7))
			{
				await dialog.Msg(L("Now that you know the basics of the Musketeer, it's time to put this in practice."));
				await dialog.Msg(L("Alright, let's teach you how to use a rifle!"));
				await dialog.CompleteQuest(Musketeer7);
				return;
			}

			if (character.Quests.IsActive(Musketeer8) && character.Quests.IsCompletable(Musketeer8))
			{
				await dialog.Msg(L("Your marksmanship is impressive."));
				await dialog.Msg(L("Now, let the lesson begin."));
				await dialog.CompleteQuest(Musketeer8);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Musketeer7) && character.Quests.MeetsPrerequisites(Musketeer7))
			{
				var answer = await dialog.SelectQuestOffer(Musketeer7, L("A Musketeer should know how to handle a rifle. Not many enemies can stand against simultaneous firing by well trained Musketeers."),
					Option(L("I'll do it, I understand"), "accept"),
					Option(L("It doesn't seem to fit for me"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Musketeer7);
					await dialog.Msg(L("One of the basic aspects of marksmanship is to procure range."));
					await dialog.Msg(L("Give it a shot. Attack an enemy at maximum range."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Musketeer8) && character.Quests.MeetsPrerequisites(Musketeer8))
			{
				var answer = await dialog.SelectQuestOffer(Musketeer8, L("You have already learnt how to fire from a distance. Now, it's time to learn precision and speed."),
					Option(L("I'm ready"), "accept"),
					Option(L("It doesn't seem to fit for me"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Musketeer8);
					character.LookAround();
					await dialog.Msg(L("When you go left from here, there is a pole to practice your marksmanship."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Musketeer7))
			{
				await dialog.Msg(L("It's not just extending the range, but being able to extend it up to the maximum distance where you are still able to shoot."));
				return;
			}

			if (character.Quests.IsActive(Musketeer8))
			{
				await dialog.Msg(L("Precision and speed. You cannot bring back the speeding bullet, after all."));
				return;
			}

			await dialog.Msg(L("A marksman who measures a recruit by how far back they are willing to stand."));
		});

		// Murmillo Master
		//-------------------------------------------------------------------------
		AddNpc(157020, L("[Murmillo Master]{nl}Phelixia"), "MURMILO_MASTER", "f_flash_64", -200.13, -1328.53, 7, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Murmillo Master"));

			if (character.Quests.IsActive(Murmillo8) && character.Quests.IsCompletable(Murmillo8))
			{
				await dialog.Msg(L("Hmm, you came earlier than I expected."));
				await dialog.Msg(L("I was a bit worried about you."));
				await dialog.CompleteQuest(Murmillo8);
				return;
			}

			if (character.Quests.IsActive(Flash64Hq1) && character.Quests.IsCompletable(Flash64Hq1))
			{
				await dialog.Msg(L("Wow... You really did it, huh."));
				await dialog.Msg(L("Impressive."));
				await dialog.CompleteQuest(Flash64Hq1);
				return;
			}

			if (!character.Quests.Has(Murmillo8) && character.Quests.MeetsPrerequisites(Murmillo8))
			{
				var answer = await dialog.SelectQuestOffer(Murmillo8, L("You came here to be a murmillo? A murmillo doesn't rely on flashy skills but pure strength."),
					Option(L("Tell him you would challenge for it"), "accept"),
					Option(L("I need some time to think about it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Murmillo8);
					await dialog.Msg(L("Your confidence is admirable."));
					await dialog.Msg(L("I will give you a simple task to prove yourself."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Flash64Hq1) && character.Quests.MeetsPrerequisites(Flash64Hq1))
			{
				var answer = await dialog.SelectQuestOffer(Flash64Hq1, L("Did you really do it? I mean, the monsters around here are kinda weak, so, you know."),
					Option(L("I will try"), "accept"),
					Option(L("I can't do it, sorry"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Flash64Hq1);
					await dialog.Msg(L("Don't push yourself too much, though, get some training first."));
					return;
				}
			}

			if (character.Quests.IsActive(Murmillo8))
			{
				await dialog.Msg(L("You are not scared are you?"));
				return;
			}

			if (character.Quests.IsActive(Flash64Hq1))
			{
				await dialog.Msg(L("Don't push yourself too much, though, get some training first."));
				return;
			}

			await dialog.Msg(L("A gladiator who counts armour pieces before she counts anything else."));
		});

		// Lancer Master
		//-------------------------------------------------------------------------
		AddNpc(157021, L("[Lancer Master]{nl}Noer Parecius"), "LANCER_MASTER", "f_flash_64", -60.33, -1343.24, 6, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Lancer Master"));

			if (character.Quests.IsActive(Lancer8) && character.Quests.IsCompletable(Lancer8))
			{
				await dialog.Msg(L("Hmm, the seal matches that of the Order."));
				await dialog.Msg(L("It is a bit smudged but it will serve as a big clue."));
				await dialog.CompleteQuest(Lancer8);
				return;
			}

			if (!character.Quests.Has(Lancer8) && character.Quests.MeetsPrerequisites(Lancer8))
			{
				await dialog.Msg(L("Monsters are the problem anywhere. What seems to be the problem?"));

				var answer = await dialog.SelectQuestOffer(Lancer8, L("I am currently examining the intel I have received about the Order of the Tree of Truth who attacked the prince. I have sent another lancer in my stead."),
					Option(L("I was born ready"), "accept"),
					Option(L("I'm not so sure about it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Lancer8);
					await dialog.Msg(L("The monsters and those who threaten the royal family must be eliminated. For the glory of the kingdom."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Lancer8))
			{
				await dialog.Msg(L("The monsters and those who threaten the royal family must be eliminated."));
				return;
			}

			await dialog.Msg(L("A lancer of the royal household, working through intelligence he does not trust."));
		});

		// The Royal Army guards of the Stone Icicle Square
		//-------------------------------------------------------------------------
		for (var i = 0; i < VictimSpots.GetLength(0); ++i)
		{
			AddNpc(VictimModels[i], L("Petrified Soldier"), i == 0 ? "FLASH64_SQ_03_NPC" : "FLASH64_SQ_03_NPC_" + (i + 1), "f_flash_64",
				VictimSpots[i, 0], VictimSpots[i, 1], VictimFacings[i], this.SootheTheVictim);
		}

		// The petrified victims of Vienti Fortress
		//-------------------------------------------------------------------------
		for (var i = 0; i < PetrifiedSpots.GetLength(0); ++i)
		{
			AddNpc(PetrifiedModels[i], L("Petrified Victim"), i == 0 ? "FLASH64_SQ_06_NPC" : "FLASH64_SQ_06_NPC_" + (i + 1), "f_flash_64",
				PetrifiedSpots[i, 0], PetrifiedSpots[i, 1], 90, this.UseLiberationElixir);
		}

		// Bonfires
		//-------------------------------------------------------------------------
		for (var i = 0; i < Bonfires.GetLength(0); ++i)
		{
			AddNpc(46011, L("Bonfire"), i == 0 ? "FLASH64_SQ_07_NPC" : "FLASH64_SQ_07_NPC_" + (i + 1), "f_flash_64",
				Bonfires[i, 0], Bonfires[i, 1], 90, this.LightTheBonfire);
		}

		// The records of the Ruklys era
		//-------------------------------------------------------------------------
		for (var i = 0; i < RecordSlabs.GetLength(0); ++i)
		{
			AddNpc(147464, L("Stone Slab"), i == 0 ? "FLASH64_MQ_01_NPC" : "FLASH64_MQ_01_NPC_" + (i + 1), "f_flash_64",
				RecordSlabs[i, 0], RecordSlabs[i, 1], RecordFacings[i], this.CopyRuklysRecord);
		}

		// Gargoyle Sculpture
		//-------------------------------------------------------------------------
		AddConditionalNpc(153094, L("Gargoyle Sculpture"), "FLASH64_MQ_03_NPC", "f_flash_64", -141.64, 2026.56, 20, this.IsGargoyleStanding, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Gargoyle Sculpture"));

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				if (character.Inventory.CountItem(ItemId.FLASH64_MQ_03_ITEM) == 0)
				{
					await dialog.Msg(L("Waking it without the Silence Scroll would bring every guard on the gathering place down on you."));
					return;
				}

				var used = await character.TimeActions.StartAsync(L("Using the Silence Scroll..."), L("Cancel"), "SCROLL", TimeSpan.FromSeconds(1.5));

				if (used != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.FLASH64_MQ_03_ITEM, 1);
				character.Quests.StartQuestTrack(Mq03);
				return;
			}

			await dialog.Msg(L("A sculpture of a Gargoyle that was not carved. It was one."));
		});

		// The Musketeer Master's practice poles
		//-------------------------------------------------------------------------
		for (var i = 0; i < PracticePoles.GetLength(0); ++i)
		{
			AddConditionalNpc(47004, L("Practice Pole"), i == 0 ? "JOB_MUSKETEER_8_1_WOOD_CARVING" : "JOB_MUSKETEER_8_1_WOOD_CARVING_" + (i + 1), "f_flash_64",
				PracticePoles[i, 0], PracticePoles[i, 1], 90, this.ArePolesUp, async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Practice Pole"));

				if (character.Quests.IsActive(Musketeer8) && !character.Quests.IsCompletable(Musketeer8))
				{
					character.Quests.StartQuestTrack(Musketeer8);
					return;
				}

				await dialog.Msg(L("A pell set out for shooting practice, with the paint worn off the middle of it."));
			});
		}

		// Hidden triggers
		//-------------------------------------------------------------------------
		// Neiveikiama Castle, where the melting solution is tried on the monsters.
		AddQuestTrigger("FLASH64_SQ_08_AREA", "f_flash_64", 997, 545, 450, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq08) && !character.Quests.IsCompletable(Sq08))
			{
				character.Quests.CompleteObjective(Sq08, "tryTheSolution");
				character.ServerMessage(L("The solution does nothing to the petrified monsters but make them angry. Report it to Saliamonas."));
			}

			await Task.CompletedTask;
		});

		// The five districts of the fortress the knights want written up.
		AddQuestTrigger("UNDER67_HIDDENQ1_AREA1", "d_underfortress_65", -133.23, -985.52, 160, args => this.NoteTheDistrict(args, 1));
		AddQuestTrigger("UNDER67_HIDDENQ1_AREA2", "d_underfortress_66", 1663.40, 395.43, 160, args => this.NoteTheDistrict(args, 2));
		AddQuestTrigger("UNDER67_HIDDENQ1_AREA3", "d_underfortress_67", 75.42, -768.88, 160, args => this.NoteTheDistrict(args, 3));
		AddQuestTrigger("UNDER67_HIDDENQ1_AREA4", "d_underfortress_68", -274.03, -868.76, 160, args => this.NoteTheDistrict(args, 4));
		AddQuestTrigger("UNDER67_HIDDENQ1_AREA5", "d_underfortress_69", 1744.16, 1004.69, 160, args => this.NoteTheDistrict(args, 5));
	}

	/// <summary>
	/// Writes one of the fortress districts up in Wilhelmina's notes.
	/// </summary>
	/// <param name="args"></param>
	/// <param name="number"></param>
	private async Task NoteTheDistrict(TriggerActorArgs args, int number)
	{
		if (args.Initiator is not Character character)
			return;

		if (character.Quests.IsActive(Under67Hq1, "noteArea" + number))
		{
			character.Quests.CompleteObjective(Under67Hq1, "noteArea" + number);
			character.ServerMessage(L("You write the district up in Wilhelmina's notes."));
		}

		await Task.CompletedTask;
	}

	/// <summary>
	/// Returns whether Amanda is still waiting outside the fortress.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaOutside(Character character)
		=> !character.Quests.HasCompleted(Mq03);

	/// <summary>
	/// Returns whether the Gargoyle is still sitting on the gathering place.
	/// </summary>
	/// <param name="character"></param>
	private bool IsGargoyleStanding(Character character)
		=> character.Quests.Has(Mq03) && !character.Quests.HasCompleted(Mq03);

	/// <summary>
	/// Returns whether the Musketeer Master's practice poles are set out.
	/// </summary>
	/// <param name="character"></param>
	private bool ArePolesUp(Character character)
		=> character.Quests.IsActive(Musketeer8);

	/// <summary>
	/// Takes the will and belongings off one of the dying guards.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task SootheTheVictim(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Petrified Soldier"));

		if (!character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("A Royal Army guard, stone from the knees down and still breathing."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.FLASH64_SQ_03_ITEM) >= WillsNeeded)
		{
			await dialog.Msg(L("You are carrying every will Wilhelmina asked for."));
			return;
		}

		var soothed = await character.TimeActions.StartAsync(L("Soothing the victim..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(3));

		if (soothed != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.FLASH64_SQ_03_ITEM, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("He gives you what he was carrying and a message for somebody in Kaliss."));
	}

	/// <summary>
	/// Uses Edita's Liberation Elixir on one of the petrified victims.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task UseLiberationElixir(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Petrified Victim"));

		if (!character.Quests.IsActive(Sq06) || character.Quests.IsCompletable(Sq06))
		{
			await dialog.Msg(L("Somebody who did not get out of the Vienti Fortress in time."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.FLASH64_SQ_06_ITEM) == 0)
		{
			await dialog.Msg(L("Without the Liberation Elixir there is nothing to be done here."));
			return;
		}

		var used = await character.TimeActions.StartAsync(L("Using the Liberation Elixir..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

		if (used != TimeActionResult.Completed)
			return;

		character.Quests.CompleteObjective(Sq06, "freeTheSoul");
		character.ServerMessage(L("The soul comes loose, says nothing of any use, and goes. Report it to Edita."));
	}

	/// <summary>
	/// Lights one of Edita's scented bonfires.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task LightTheBonfire(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Bonfire"));

		if (!character.Quests.IsActive(Sq07))
		{
			await dialog.Msg(L("A bonfire laid ready, with something sweet packed into the kindling."));
			return;
		}

		var lit = await character.TimeActions.StartAsync(L("Lighting the bonfire..."), L("Cancel"), "FIRE", TimeSpan.FromSeconds(2));

		if (lit != TimeActionResult.Completed)
			return;

		for (var i = 1; i <= BonfiresToLight; ++i)
		{
			if (character.Quests.IsActive(Sq07, "lightBonfire" + i))
			{
				character.Quests.CompleteObjective(Sq07, "lightBonfire" + i);
				character.ServerMessage(L("The bonfire catches, and the smoke of it smells of butterflies."));
				return;
			}
		}

		await dialog.Msg(L("Every bonfire Edita laid is burning. Go back and tell her."));
	}

	/// <summary>
	/// Copies one of the Ruklys era records off its stone slab.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task CopyRuklysRecord(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Stone Slab"));

		if (!character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("A slab of the Ruklys era, and the Royal Army has taken everything readable off the ones near the camp."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.FLASH64_MQ_01_ITEM) >= RecordsNeeded)
		{
			await dialog.Msg(L("You have as many records as Amanda asked for."));
			return;
		}

		var copied = await character.TimeActions.StartAsync(L("Copying the record..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

		if (copied != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.FLASH64_MQ_01_ITEM, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("The rubbing comes off clean enough to read."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8845: Intensified Rampage
//-----------------------------------------------------------------------------
public class Flash64Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8845);
		SetName(L("Intensified Rampage"));
		SetDescription(L("Something is drawing monsters onto the Kovos Hall Site and nobody knows what."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("Wilhelmina Carriot is seeking for someone's help at the inner district of the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_KARRIAT", "f_flash_64", L("Defeat the monsters at Kovos Hall Site"), L("Wilhelmina Carriot wants you to defeat the monsters at Kovos Hall Site."));
		SetPhase(QuestStatus.Success, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("You've defeated the monsters as Wilhelmina Carriot requested of you. Let's go back to Wilhelmina Carriot."));

		AddPrerequisite(new LevelPrerequisite(186));

		AddObjective("clearKovos", L("Defeat the monsters at Kovos Hall Site"), new KillObjective(8, "Repusbunny", "Lemuria", "Rubabos"));

		AddReward(new ItemReward("expCard10", 2));
	}
}

// 8846: Refreshing Work
//-----------------------------------------------------------------------------
public class Flash64Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8846);
		SetName(L("Refreshing Work"));
		SetDescription(L("The grave robbers have been taking the medicinal candy the headquarters sends out."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("Wilhelmina Carriot is seeking for someone's help at the inner district of the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_KARRIAT", "f_flash_64", L("Retrieve the Cough Medicine"), L("Wilhelmina Carriot asked you to retrieve the Cough Medicine that the monsters took away for the soldiers who were swept by the Petrifying Frost."));
		SetPhase(QuestStatus.Success, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("You've retrieved the Tasty Candy Balls. Let's take them back to Wilhelmina Carriot."));

		AddPrerequisite(new LevelPrerequisite(186));
		AddPrerequisite(new QuestStatusPrerequisite(8845, QuestStatus.Completed));

		AddObjective("retrieveCandy", L("Retrieve the Cough Medicine"), new CollectItemObjective("FLASH64_SQ_02_ITEM", 8));

		AddPityDrop("FLASH64_SQ_02_ITEM", 0.85f, 3, 1, "Repusbunny", "Lemuria", "Rubabos");

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("FLASH64_SQ_02_ITEM"));
	}
}

// 8847: Irrevocable Accident (1)
//-----------------------------------------------------------------------------
public class Flash64Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8847);
		SetName(L("Irrevocable Accident (1)"));
		SetDescription(L("The guards caught by the Frost at the Stone Icicle Square are still dying there."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("Wilhelmina Carriot is seeking for someone's help at the inner district of the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_SQ_03_NPC", "f_flash_64", L("Soothe the victims"), L("Wilhelmina Carriot asked you to soothe the Royal Army guards that are dying due to the Petrifying Frost at the Stone Icicle Square."));
		SetPhase(QuestStatus.Success, "FLASH64_SQ_03_NPC", "f_flash_64", L("Soothe the victims"), L("You have their wills and belongings."));

		AddPrerequisite(new LevelPrerequisite(186));

		AddObjective("sootheVictims", L("Soothe the victims"), new CollectItemObjective("FLASH64_SQ_03_ITEM", 5));

		AddReward(new ItemReward("expCard10", 1));
		AddReward(new TakeItemReward("FLASH64_SQ_03_ITEM"));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client names no turn-in NPC; the wills are carried off the square.
		character.ServerMessage(L("Every guard who could still speak has been heard. Their wills go to Kaliss."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8849: Magical Opinion (1)
//-----------------------------------------------------------------------------
public class Flash64Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8849);
		SetName(L("Magical Opinion (1)"));
		SetDescription(L("Edita's Liberation Elixir needs Dark Crystals out of the Vienti Fortress monsters."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_EDITA", "f_flash_64", L("Talk to Bokor Edita"), L("Bokor Edita at the Petrified City is seeking for someone's help."));
		SetPhase(QuestStatus.InProgress, "FLASH64_EDITA", "f_flash_64", L("Collect Dark Crystals"), L("Bokor Edita asked you to defeat the monsters at Vienti Fortress and collect the dark crystals."));
		SetPhase(QuestStatus.Success, "FLASH64_EDITA", "f_flash_64", L("Hand them over to the Bokor Master"), L("You have collected enough dark crystals as Bokor Edita requested to you. Return to Edita."));

		AddPrerequisite(new LevelPrerequisite(186));

		AddObjective("collectCrystals", L("Collect Dark Crystals from monsters in Vienti Fortress"), new CollectItemObjective("FLASH64_SQ_05_ITEM", 8));

		AddPityDrop("FLASH64_SQ_05_ITEM", 0.75f, 3, 1, "Repusbunny", "Lemuria", "Rubabos");

		AddReward(new ItemReward("expCard10", 1));
		AddReward(new TakeItemReward("FLASH64_SQ_05_ITEM"));
	}
}

// 8850: Magical Opinion (2)
//-----------------------------------------------------------------------------
public class Flash64Sq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8850);
		SetName(L("Magical Opinion (2)"));
		SetDescription(L("The elixir pulls a soul out of a petrified victim, and it may remember what it saw."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_EDITA", "f_flash_64", L("Talk to Bokor Edita"), L("Bokor Edita at the Petrified City is seeking for someone's help."));
		SetPhase(QuestStatus.InProgress, "FLASH64_SQ_06_NPC", "f_flash_64", L("Release the petrified soldier"), L("Bokor Edita asked you to use the Liberation Elixir on the petrified victims to release their evil thoughts."));
		SetPhase(QuestStatus.Success, "FLASH64_EDITA", "f_flash_64", L("Talk to Bokor Edita"), L("You've released the evil thoughts of the victims by using the Liberation Elixir on them. Return to Edita."));

		AddPrerequisite(new LevelPrerequisite(186));
		AddPrerequisite(new QuestStatusPrerequisite(8849, QuestStatus.Completed));

		AddObjective("freeTheSoul", L("Release the petrified soldier"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("FLASH64_SQ_06_ITEM"));
	}
}

// 8851: Kaliss Wants the Truth
//-----------------------------------------------------------------------------
public class Flash64Sq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8851);
		SetName(L("Kaliss Wants the Truth"));
		SetDescription(L("The bonfires of the Vienti Fortress are laid with a scent that quiets the Frosted souls."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_EDITA", "f_flash_64", L("Talk to Bokor Edita"), L("Bokor Edita at the Petrified City is seeking for someone's help."));
		SetPhase(QuestStatus.InProgress, "FLASH64_SQ_07_NPC", "f_flash_64", L("Light a bonfire in Vienti Fortress"), L("Bokor Edita asked you to light the bonfires in Vienti Fortress to console the Frosted souls in this city."));
		SetPhase(QuestStatus.Success, "FLASH64_EDITA", "f_flash_64", L("Talk to Bokor Edita"), L("You lighted the bonfire as Edita asked. Return to Edita."));

		AddPrerequisite(new LevelPrerequisite(186));

		AddObjective("lightBonfire1", L("Light the first bonfire in Vienti Fortress"), new ManualObjective());
		AddObjective("lightBonfire2", L("Light the second bonfire in Vienti Fortress"), new ManualObjective());
		AddObjective("lightBonfire3", L("Light the third bonfire in Vienti Fortress"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 1));
	}
}

// 8852: Value of the Alchemist
//-----------------------------------------------------------------------------
public class Flash64Sq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8852);
		SetName(L("Value of the Alchemist"));
		SetDescription(L("Saliamonas' Petrification Thawing Liquid has not been tried on anything yet."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_SALIAMONS", "f_flash_64", L("Talk with Alchemist Saliamonas"), L("Alchemist Saliamonas is waiting for someone's help at the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_SQ_08_AREA", "f_flash_64", L("Use the melting solution on the monsters at Neiveikiama Castle"), L("Alchemist Saliamonas told you to use the Petrification Thawing Liquid on the monsters."));
		SetPhase(QuestStatus.Success, "FLASH64_SALIAMONS", "f_flash_64", L("Report to the Alchemist Saliamonas"), L("You used the melting solution on the monsters as Saliamonas told you to, but you made them angry. Let's go back to Saliamonas."));

		AddPrerequisite(new LevelPrerequisite(186));

		AddObjective("tryTheSolution", L("Use the melting solution on the monsters at Neiveikiama Castle"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("FLASH64_SQ_08_ITEM"));
	}
}

// 8853: Interesting Copy
//-----------------------------------------------------------------------------
public class Flash64Sq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8853);
		SetName(L("Interesting Copy"));
		SetDescription(L("Saliamonas wants enough petrified samples to run every drug he can think of."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_SALIAMONS", "f_flash_64", L("Talk with Alchemist Saliamonas"), L("Alchemist Saliamonas is waiting for someone's help at the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_SALIAMONS", "f_flash_64", L("Collect the Petrified Samples from the monsters in Neiveikiama Castle"), L("Saliamonas asked you to obtain the Petrified Samples by defeating the monsters at Neiveikiama Castle for the experiment of the new medicine."));
		SetPhase(QuestStatus.Success, "FLASH64_SALIAMONS", "f_flash_64", L("Talk with Alchemist Saliamonas"), L("You've collected enough Petrified Samples as requested by Saliamonas. Let's go back to Saliamonas."));

		AddPrerequisite(new LevelPrerequisite(186));

		AddObjective("collectSamples", L("Obtain Petrification Samples by defeating monsters"), new CollectItemObjective("FLASH64_SQ_09_ITEM", 7));

		AddPityDrop("FLASH64_SQ_09_ITEM", 0.8f, 3, 1, "Repusbunny", "Lemuria", "Rubabos");

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("FLASH64_SQ_09_ITEM"));
	}
}

// 8854: Lab Destroyer
//-----------------------------------------------------------------------------
public class Flash64Sq10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8854);
		SetName(L("Lab Destroyer"));
		SetDescription(L("The Rubabos of the Nebekia Fortress are pulling down the ruins Saliamonas works in."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_SALIAMONS", "f_flash_64", L("Talk with Alchemist Saliamonas"), L("Alchemist Saliamonas is waiting for someone's help at the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_SALIAMONS", "f_flash_64", L("Defeat Rubabos"), L("Alchemist Saliamonas asked you to defeat the Rubabos that are destroying the ruins in Nebekia Fortress."));
		SetPhase(QuestStatus.Success, "FLASH64_SALIAMONS", "f_flash_64", L("Talk with Alchemist Saliamonas"), L("You've defeated Rubabos as requested by Saliamonas. Return to Saliamonas."));

		AddPrerequisite(new LevelPrerequisite(186));

		AddObjective("killRubabos", L("Defeat Rubabos"), new KillObjective(3, "Rubabos"));

		AddReward(new ItemReward("expCard10", 2));
	}
}

// 8855: Secret Trade (1)
//-----------------------------------------------------------------------------
public class Flash64Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8855);
		SetName(L("Secret Trade (1)"));
		SetDescription(L("Amanda will take the Revelator into the Fortress of the Land for the right records."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_AMANDA", "f_flash_64", L("Talk to Grave Robber Amanda"), L("Grave Robber Amanda is waiting for someone's help in the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_MQ_01_NPC", "f_flash_64", L("Collect the Record from the Ruklys Era"), L("Grave Robber Amanda told you that in order to go into the Fortress of the Land, you would need her help. Please collect the records from the Ruklys era."));
		SetPhase(QuestStatus.Success, "FLASH64_AMANDA", "f_flash_64", L("Talk to Grave Robber Amanda"), L("You've collected the records of the era of Ruklys as requested by Amanda. Let's go back to Amanda."));

		AddPrerequisite(new LevelPrerequisite(190));

		AddObjective("collectRecords", L("Collect the Record from the Ruklys Era"), new CollectItemObjective("FLASH64_MQ_01_ITEM", 5));

		AddReward(new ItemReward("expCard10", 1));
	}
}

// 8856: Secret Trade (2)
//-----------------------------------------------------------------------------
public class Flash64Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8856);
		SetName(L("Secret Trade (2)"));
		SetDescription(L("The records go to Wilhelmina, and what comes back is a Silence Scroll."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_AMANDA", "f_flash_64", L("Talk to Grave Robber Amanda"), L("Grave Robber Amanda is waiting for someone's help in the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("Amanda asked you to hand over the records from the Ruklys era to Wilhelmina Carriot at Kovos Hall Site and receive the weapons that can be used to defeat Gargoyle."));
		SetPhase(QuestStatus.Success, "FLASH64_AMANDA", "f_flash_64", L("Talk to Grave Robber Amanda"), L("You've received the silence scroll from Wilhelmina Carriot. Let's hand it over to the Grave Robber Amanda."));

		AddPrerequisite(new LevelPrerequisite(190));
		AddPrerequisite(new QuestStatusPrerequisite(8855, QuestStatus.Completed));

		AddObjective("tradeRecords", L("Talk to Wilhelmina Carriot"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 1));
		AddReward(new TakeItemReward("FLASH64_MQ_03_ITEM"));
	}
}

// 8857: Secret Trade (3)
//-----------------------------------------------------------------------------
public class Flash64Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8857);
		SetName(L("Secret Trade (3)"));
		SetDescription(L("The Gargoyle over the gathering place comes down quietly, with the scroll doing the quiet part."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_AMANDA", "f_flash_64", L("Talk to Grave Robber Amanda"), L("Grave Robber Amanda is waiting for someone's help in the Petrified City."));
		SetPhase(QuestStatus.InProgress, "FLASH64_MQ_03_NPC", "f_flash_64", L("Defeat Gargoyle"), L("Use the silence scroll which you've received from Wilhelmina Carriot so the sound doesn't carry, and defeat the Gargoyle."));
		SetPhase(QuestStatus.Success, "FLASH64_AMANDA", "f_flash_64", L("Talk to Grave Robber Amanda"), L("You've defeated Gargoyle as Grave Robber Amanda requested to you. Return to her and talk about what to do next."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FLASH64_MQ_03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(190));
		AddPrerequisite(new QuestStatusPrerequisite(8856, QuestStatus.Completed));

		AddObjective("killGargoyle", L("Defeat Gargoyle"), new KillObjective(1, "boss_Gargoyle") { LayerOnly = true });

		AddReward(new ItemReward("expCard10", 1));
	}
}

// 30118: From Ground to Air, From Ground to Ground [Cannoneer Advancement]
//-----------------------------------------------------------------------------
public class JobCannoneer71Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30118);
		SetName(L("From Ground to Air, From Ground to Ground"));
		SetDescription(L("The Cannoneer Master wants both a flying and a walking target studied."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CANNONEER_MASTER", "f_flash_64", L("Talk with the Cannoneer Master"), L("Talk with the Cannoneer Master at the Inner Enceinte District."));
		SetPhase(QuestStatus.InProgress, "CANNONEER_MASTER", "f_flash_64", L("Defeat Flying-type and Walking-type monsters"), L("In order to use both aerial and ground attacks efficiently, defeat both a flying-type monster and a walking-type monster."));
		SetPhase(QuestStatus.Success, "CANNONEER_MASTER", "f_flash_64", L("Report to the Cannoneer Master"), L("You've studied enough on Flying-type monsters and Walking-type monsters. Return to the Cannoneer Master."));

		AddPrerequisite(new LevelPrerequisite(235));

		AddObjective("studyFlying", L("Defeat a Flying-type monster"), new KillObjective(1, "Lemuria"));
		AddObjective("studyWalking", L("Defeat a Walking-type monster"), new KillObjective(1, "Repusbunny", "Rubabos"));

		AddReward(new ItemReward("CAN01_103", 1));
	}
}

// 30119: Procuring Distance [Musketeer Advancement]
//-----------------------------------------------------------------------------
public class JobMusketeer71Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30119);
		SetName(L("Procuring Distance"));
		SetDescription(L("A rifle is only worth carrying at the range it was made for."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MUSKETEER_MASTER", "f_flash_64", L("Talk with the Musketeer Master"), L("Talk with the Musketeer Master at the Inner Enceinte District."));
		SetPhase(QuestStatus.InProgress, "MUSKETEER_MASTER", "f_flash_64", L("Attack the monsters from the maximum distance"), L("Since you won't be able to attack enemies close ranged properly, attack the enemies from a long distance."));
		SetPhase(QuestStatus.Success, "MUSKETEER_MASTER", "f_flash_64", L("Report to the Musketeer Master"), L("It seems that you've completed the assignment well. Return to the Musketeer Master."));

		AddPrerequisite(new LevelPrerequisite(235));

		AddObjective("shootAtRange", L("Attack the monsters from the maximum distance"), new KillObjective(5, "Repusbunny", "Lemuria", "Rubabos"));

		AddReward(new ItemReward("MUS01_103", 1));
	}
}

// 50062: The seal which the monster possesses
//-----------------------------------------------------------------------------
public class Underfortress66Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50062);
		SetName(L("The seal which the monster possesses"));
		SetDescription(L("Ruklys' army seals turn up on the monsters of the Fortress of the Land."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64", "d_underfortress_66");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("Wilhelmina Carriot is looking at what you have closely. Talk with Wilhelmina Carriot."));
		SetPhase(QuestStatus.InProgress, "FLASH64_KARRIAT", "d_underfortress_66", L("Obtain Ruklys' Army Seals and Parchments"), L("Wilhelmina Carriot wants more documents. Obtain some things that can be used as information from the Fortress of the Land."));
		SetPhase(QuestStatus.Success, "FLASH64_KARRIAT", "f_flash_64", L("Hand them over to Wilhelmina Carriot"), L("Wilhelmina Carriot will be satisfied with this. Hand them over to Wilhelmina Carriot."));

		AddPrerequisite(new LevelPrerequisite(194));
		AddPrerequisite(new ItemPrerequisite("UNDERFORTRESS66_SQ_ITEM01"));
		AddPrerequisite(new ItemPrerequisite("UNDERFORTRESS66_SQ_ITEM02"));

		AddObjective("collectSeals", L("Get Ruklys' Army Seals"), new CollectItemObjective("UNDERFORTRESS66_SQ_ITEM01", 6));
		AddObjective("collectParchments", L("Get Ruklys' Army Parchments"), new CollectItemObjective("UNDERFORTRESS66_SQ_ITEM02", 6));

		AddPityDrop("UNDERFORTRESS66_SQ_ITEM01", 0.8f, 3, 1, "Chafperor_mage_purple");
		AddPityDrop("UNDERFORTRESS66_SQ_ITEM02", 0.8f, 3, 1, "ticen_mage_blue");

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("UNDERFORTRESS66_SQ_ITEM01"));
		AddReward(new TakeItemReward("UNDERFORTRESS66_SQ_ITEM02"));
	}
}

// 50259: To the Fortress of the Land
//-----------------------------------------------------------------------------
public class Underfortress67Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50259);
		SetName(L("To the Fortress of the Land"));
		SetDescription(L("The Knights of Kaliss want the inside of the fortress written up before they walk into it."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64", "d_underfortress_65", "d_underfortress_66", "d_underfortress_67", "d_underfortress_68", "d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("Wilhelmina Carriot seems to have made a decision. Talk to her."));
		SetPhase(QuestStatus.InProgress, "UNDER67_HIDDENQ1_AREA1", "d_underfortress_65", L("Report on the Environment Inside the Fortress of the Land"), L("Use the writing tools provided by Wilhelmina Carriot to note down any aspects the knights will need to be careful of inside the fortress."));
		SetPhase(QuestStatus.Success, "FLASH64_KARRIAT", "f_flash_64", L("Talk to Wilhelmina Carriot"), L("You have completed your report on the environment inside the fortress. Return to Wilhelmina Carriot and talk to her."));

		AddPrerequisite(new QuestStatusPrerequisite(50084, QuestStatus.Completed));

		AddObjective("noteArea1", L("Note down the Sentry Bailey"), new ManualObjective());
		AddObjective("noteArea2", L("Note down the Drill Ground of Confliction"), new ManualObjective());
		AddObjective("noteArea3", L("Note down the Resident Quarter"), new ManualObjective());
		AddObjective("noteArea4", L("Note down the Storage Quarter"), new ManualObjective());
		AddObjective("noteArea5", L("Note down the Fortress Battlegrounds"), new ManualObjective());

		AddReward(new ItemReward("COLLECT_308", 1));
		AddReward(new ItemReward("misc_scrollskulp", 1));
		AddReward(new TakeItemReward("UNDER67_HIDDENQ1_ITEM1"));
	}
}

// 50267: Competitive to the Extreme
//-----------------------------------------------------------------------------
public class Flash64Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50267);
		SetName(L("Competitive to the Extreme"));
		SetDescription(L("The Murmillo Master will be impressed by a strong monster put down in very little armour."));
		SetType(QuestType.Sub);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MURMILO_MASTER", "f_flash_64", L("Talk with the Murmillo Master"), L("Talk to the Murmillo Master."));
		SetPhase(QuestStatus.InProgress, "MURMILO_MASTER", "f_flash_64", L("Defeat a Strong Monster While Equipping 3 or Less Armor Items"), L("To impress the Murmillo Master, defeat a strong monster while wearing three armour pieces or fewer."));
		SetPhase(QuestStatus.Success, "MURMILO_MASTER", "f_flash_64", L("Talk with the Murmillo Master"), L("You have defeated a correct target monster while equipping 3 or less armor items. Talk to the Murmillo Master."));

		AddPrerequisite(new LevelPrerequisite(186));

		AddObjective("proveStrength", L("Defeat a Strong Monster While Equipping 3 or Less Armor Items"), new KillObjective(1, "Rubabos"));

		AddReward(new ItemReward("misc_scrollskulp", 1));
	}
}

// 90157: Make It Doubly Sure [Lancer Advancement]
//-----------------------------------------------------------------------------
public class JobLancer81Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90157);
		SetName(L("Make It Doubly Sure"));
		SetDescription(L("The Order of the Tree of Truth left traces in Nheto Forest, and the other lancer did not come back."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64", "f_maple_25_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "LANCER_MASTER", "f_flash_64", L("Talk with the Lancer Master"), L("Talk with the Lancer Master at the Inner Enceinte District."));
		SetPhase(QuestStatus.InProgress, "JOB_LANCER_8_1", "f_maple_25_1", L("Find the traces of the Order of the Tree of Truth"), L("Go to Nheto Forest and find any traces related to the Order of the Tree of Truth."));
		SetPhase(QuestStatus.Success, "LANCER_MASTER", "f_flash_64", L("Report to the Lancer Master"), L("You've found the document related to the Order of the Tree of Truth. Return to the Lancer Master at Inner Enceinte District."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "JOB_LANCER_8_1_TRACK", 2000, autoStart: false);

		AddPrerequisite(new LevelPrerequisite(285));

		AddObjective("killGuards", L("Defeat the Rhodenag"), new KillObjective(4, "rodenag", "rodenarcorng") { LayerOnly = true });
		AddObjective("findTraces", L("Find the traces of the Order of the Tree of Truth"), new CollectItemObjective("JOB_LANCER_8_1_ITEM", 1));

		AddReward(new ItemReward("COLLECT_308", 1));
	}
}

// 90158: The Glory of a Gladiator [Murmillo Advancement]
//-----------------------------------------------------------------------------
public class JobMurmillo81Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90158);
		SetName(L("The Glory of a Gladiator"));
		SetDescription(L("The Silva Griffin of the Grynas Trail is the Murmillo Master's idea of a simple task."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64", "f_katyn_45_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MURMILO_MASTER", "f_flash_64", L("Talk with the Murmillo Master"), L("Talk to the Murmillo Master at Inner Enceinte District."));
		SetPhase(QuestStatus.InProgress, "JOB_MURMILLO_8_1", "f_katyn_45_1", L("Defeat Silva Griffin"), L("Go to Grynas Trail and defeat Silva Griffin."));
		SetPhase(QuestStatus.Success, "MURMILO_MASTER", "f_flash_64", L("Report to the Murmillo Master"), L("Silva Griffin has fallen. Return to the Murmillo Master at Inner Enceinte District."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "JOB_MURMILLO_8_1_TRACK", 2000, autoStart: false);

		AddPrerequisite(new LevelPrerequisite(285));

		AddObjective("killGriffin", L("Defeat Silva Griffin"), new KillObjective(1, "boss_Silva_griffin_J1") { LayerOnly = true });

		AddReward(new ItemReward("COLLECT_308", 1));
	}
}

// 90161: De-Construction [Cannoneer Advancement]
//-----------------------------------------------------------------------------
public class JobCannoneer81Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90161);
		SetName(L("De-Construction"));
		SetDescription(L("Somebody has been leaving observation orbs around the kingdom camp on Steel Heights."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64", "f_tableland_74");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CANNONEER_MASTER", "f_flash_64", L("Talk with the Cannoneer Master"), L("Talk with the Cannoneer Master at the Inner Enceinte District."));
		SetPhase(QuestStatus.InProgress, "JOB_CANNONEER_8_1", "f_tableland_74", L("Destroy Observation Orb"), L("The orbs can be found at Ghresmei Passage on Steel Heights."));
		SetPhase(QuestStatus.Success, "CANNONEER_MASTER", "f_flash_64", L("Report to the Cannoneer Master"), L("You've destroyed the surveillance sphere. Return to the Cannoneer Master."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "JOB_CANNONEER_8_1_TRACK", 2000, autoStart: false);

		AddPrerequisite(new LevelPrerequisite(285));

		AddObjective("breakTheOrb", L("Destroy Observation Orb"), new KillObjective(1, "Mon_npc_figurine_device") { LayerOnly = true });

		AddReward(new ItemReward("COLLECT_308", 1));
	}
}

// 90162: Fast and Precise [Musketeer Advancement]
//-----------------------------------------------------------------------------
public class JobMusketeer81Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90162);
		SetName(L("Fast and Precise"));
		SetDescription(L("The practice poles west of the camp light up when they are worth shooting."));
		SetType(QuestType.Main);
		SetLocation("f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "MUSKETEER_MASTER", "f_flash_64", L("Talk with the Musketeer Master"), L("Talk with the Musketeer Master at the Inner Enceinte District."));
		SetPhase(QuestStatus.InProgress, "JOB_MUSKETEER_8_1_WOOD_CARVING", "f_flash_64", L("Hit the practice pole of Musketeer at right times"), L("Move to the practice pole and attack when the pole shines. If you make three mistakes, you start anew."));
		SetPhase(QuestStatus.Success, "MUSKETEER_MASTER", "f_flash_64", L("Report to the Musketeer Master"), L("It seems that you've completed the assignment well. Return to the Musketeer Master."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "JOB_MUSKETEER_8_1_TRACK", 2000, autoStart: false);

		AddPrerequisite(new LevelPrerequisite(285));

		AddObjective("hitThePoles", L("Hit the practice pole of Musketeer at right times"), new KillObjective(3, "Monster_wood_carving") { LayerOnly = true });

		AddReward(new ItemReward("COLLECT_308", 1));
	}
}
