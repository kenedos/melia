//--- Melia Script ----------------------------------------------------------
// Novaha Annex Quest NPCs
//--- Description -----------------------------------------------------------
// The search for Edmundas in the Apega State Chamber, the experiment
// victim Hilbeth and Monk Abels' last mission.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DAbbey642QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Abbay641Mq050 = new QuestId(50121);
	private readonly static QuestId Mq010 = new QuestId(50125);
	private readonly static QuestId Mq020 = new QuestId(50126);
	private readonly static QuestId Mq030 = new QuestId(50127);
	private readonly static QuestId Mq040 = new QuestId(50128);
	private readonly static QuestId Sq010 = new QuestId(50129);
	private readonly static QuestId Sq020 = new QuestId(50130);
	private readonly static QuestId Sq030 = new QuestId(50131);
	private readonly static QuestId Sq040 = new QuestId(50132);
	private readonly static QuestId Sq050 = new QuestId(50133);

	public const string StoneCountVar = "Gabija.Quests.Abbay642Mq020.Stones";
	private const string StoneVar = "Gabija.Quests.Abbay642Mq020.Stone";
	public const string BelongingCountVar = "Gabija.Quests.Abbay642Sq020.Belongings";
	private const string BelongingVar = "Gabija.Quests.Abbay642Sq020.Belonging";
	private const string FragmentVar = "Gabija.Quests.Abbay642Sq040.Fragment";
	public const string RelicCountVar = "Gabija.Quests.Abbay642Sq050.Relics";
	private const string RelicVar = "Gabija.Quests.Abbay642Sq050.Relic";

	private static readonly double[,] Stones =
	{
		{ -626.12, -1187.33 }, { -604.76, -1390.77 }, { -414.76, -1400.41 }, { -423.19, -1193.47 },
	};

	private static readonly double[,] Belongings =
	{
		{ -493.83, 2017.71, 6 }, { -584.59, 2325.93, -30 }, { -811.94, 2223.25, -9 }, { -859.06, 1961.80, -29 },
	};

	private static readonly double[,] Fragments =
	{
		{ 901.02, 1273.66 }, { 899.23, 1029.13 }, { 815.78, 1564.17 }, { 1115.54, 1679.46 }, { 1116.20, 1442.01 },
		{ 604.71, 1644.89 }, { 535.81, 1384.64 },
	};

	private static readonly double[,] RelicSpots =
	{
		{ 44.73, 984.71, 45 }, { -932.14, 2110.18, 42 }, { -586.41, 248.99, 29 },
	};

	protected override void Load()
	{
		// Traveling Merchant Rose at the Annex entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "ABBEY642_ROZE01", "d_abbey_64_2", 920.06, -114.04, 189, c => c.Quests.HasCompleted(Abbay641Mq050) && !c.Quests.HasCompleted(Mq010), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("You found my brother in the Apega State Chamber? Then... why isn't he with you?"));
				await dialog.Msg(L("So he's strapped to some weird device? Let's go, then! We have to save him!"));
				await dialog.CompleteQuest(Mq010);

				if (character.Quests.HasCompleted(Mq010))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("We're here now, but I think the Annex is too big."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("It'll be faster if we split up and look for my brother."),
					Option(L("It's dangerous, stay here"), "accept"),
					Option(L("Let's rest for a while first"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);
					character.LookAround();

					await dialog.Msg(L("I'm not sure how I can cover such a wide area... Alright."));
					await dialog.Msg(L("It's me who the demons want... They could easily catch me if I'm not careful and then I'll be just another burden on you."));
					await dialog.Msg(L("I'll wait here, where it's safe. Please tell me as soon as you find my brother."));
				}
				return;
			}

			await dialog.Msg(L("The thing about finding someone qualified is bothering me. He'll be all right... won't he?"));
		});

		// Edmundas in the Apega State Chamber
		//-------------------------------------------------------------------------
		AddConditionalNpc(153110, L("Edmundas"), "ABBEY642_EDMONDAS", "d_abbey_64_2", -11.70, -1335.25, 166, c => c.Quests.Has(Mq010) && !c.Quests.HasCompleted(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Edmundas"));

			if (character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Rose... she's...! That's why I told her to run... Why didn't she listen to me..."));
				await dialog.Msg(L("So I was nothing but bait to get Rose to come here. And now this... What do we do...?"));
				await dialog.CompleteQuest(Mq030);
				return;
			}

			if (character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("Thank you for releasing me. I'm so exhausted... But I have to save Rose."));
				await dialog.Msg(L("Please. Help me save Rose. I know we've only just met and it's not right of me to ask so much of you right away, but please."));
				await dialog.Msg(L("Rose... She's my little sister, my only one. I can't bear to lose her."));
				await dialog.Msg(L("I know where they took Rose. The main monastery building... If you can lend me some of your strength, please come with me."));
				await dialog.CompleteQuest(Mq040);

				if (character.Quests.HasCompleted(Mq040))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				await dialog.Msg(L("That wizard... He's going to do to Rose the same experiments he did on me... He's going to use her to spread the giant bracken spores."));

				var answer = await dialog.SelectQuestOffer(Mq040, L("Release me first. Then help me bring Rose back, I beg you..."),
					Option(L("I'll have a look in the vestry"), "accept"),
					Option(L("Let's find another solution"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq040);

				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				character.Quests.ReplayQuestTrack(Mq030);
				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				await dialog.Msg(L("There should be some device to shut this down around here somewhere. Please... before anything bad happens to Rose."));
				return;
			}
		});

		AddQuestTrigger("ABBEY642_EDMONDA_CHECK", "d_abbey_64_2", -11.70, -1335.25, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq010, "findEdmundas"))
				return;

			character.Quests.CompleteObjective(Mq010, "findEdmundas");

			await Task.CompletedTask;
		});

		// Traveling Merchant Rose beside her brother
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "ABBEY642_ROZE02", "d_abbey_64_2", 11, -1272, -4, c => c.Quests.HasCompleted(Mq010) && !c.Quests.Has(Mq030), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("My brother... he's fine now. I was so worried about what might happen to him..."));
				await dialog.CompleteQuest(Mq020);
				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				await dialog.Msg(L("What do we do... My brother is suffering. The wizard must have put him in this device so he couldn't escape..."));
				await dialog.Msg(L("He says the pain started when the wizard came inside the Apega State Chamber."));

				var answer = await dialog.SelectQuestOffer(Mq020, L("I'll stay here and keep an eye on him. You go and find a way to stop this. Quick!"),
					Option(L("I'll go right away"), "accept"),
					Option(L("Let's find another solution"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Stones.GetLength(0); ++i)
						character.Variables.Perm.Set(StoneVar + i, false);
					character.Variables.Perm.SetInt(StoneCountVar, 0);

					character.Quests.Start(Mq020);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				await dialog.Msg(L("We need to disable this barrier so my brother and I can escape... But no matter how hard I try, it won't open."));

				var answer = await dialog.SelectQuestOffer(Mq030, L("There has to be a way to free my brother around here. Will you look for it?"),
					Option(L("I'll take a look around the vestry"), "accept"),
					Option(L("I'm going to rest for a while"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq030);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("My brother is looking worse. I can't even go near him because of this barrier..."));
		});

		// Magic Generating Stones in the inner State Chamber
		//-------------------------------------------------------------------------
		for (var i = 0; i < Stones.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(151057, "UnvisibleName", "ABBEY642_DEVICE0" + number, "d_abbey_64_2", Stones[i, 0], Stones[i, 1], 90,
				character => !character.Quests.HasCompleted(Mq020) && !character.Variables.Perm.GetBool(StoneVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Mq020) || character.Quests.IsCompletable(Mq020) || character.Variables.Perm.GetBool(StoneVar + number, false))
						return;

					character.Variables.Perm.Set(StoneVar + number, true);
					var destroyed = character.Variables.Perm.GetInt(StoneCountVar, 0) + 1;
					character.Variables.Perm.SetInt(StoneCountVar, destroyed);

					dialog.Npc.PlayEffect("F_explosion014", 1f);
					character.ServerMessage(LF("Crystal pillars destroyed: {0}/{1}", Math.Min(destroyed, 4), 4));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// The device holding Edmundas' shackles
		//-------------------------------------------------------------------------
		AddNpc(147307, "UnvisibleName", "ABBEY642_DEVICE05", "d_abbey_64_2", 583.92, -1283.52, 90, async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq040, "releaseShackles"))
				return;

			dialog.Npc.PlayEffect("F_light018_yellow", 1f);
			character.Quests.CompleteObjective(Mq040, "releaseShackles");

			await Task.CompletedTask;
		});

		// Experiment Victim Hilbeth
		//-------------------------------------------------------------------------
		AddNpc(20063, L("Experiment Victim Hilbeth"), "ABBEY642_PEAPLE01", "d_abbey_64_2", 28.79, 1562.41, 55, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Experiment Victim Hilbeth"));

			if (character.Quests.IsCompletable(Sq010))
			{
				await dialog.Msg(L("Thank you. Now if I can just gather enough strength to walk..."));
				await dialog.CompleteQuest(Sq010);
				return;
			}

			if (character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("That... That thing you're holding... That's Anne's bracelet... Her parents..."));
				await dialog.CompleteQuest(Sq020);
				return;
			}

			if (character.Quests.IsCompletable(Sq030))
			{
				await dialog.Msg(L("Thank you. How... How am I going to live now..."));
				await dialog.CompleteQuest(Sq030);
				return;
			}

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				await dialog.Msg(L("You... You don't seem like you're with that wizard. Please, don't ignore me... Help me."));
				await dialog.Msg(L("I barely just escaped from the demons. But the experiments were hard on me... My body is too exhausted right now..."));

				var answer = await dialog.SelectQuestOffer(Sq010, L("You'll find my bag of herbs in the Tebeti Small Corridor. Please... bring it to me."),
					Option(L("I'll find the backpack for you"), "accept"),
					Option(L("I'm sorry"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq010);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				var answer = await dialog.SelectQuestOffer(Sq020, L("I'm still injured, but I think I can move now. I should get up and go rescue my family."),
					Option(L("You stay put; I'll do it for you"), "accept"),
					Option(L("Getting some rest first will be better"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Belongings.GetLength(0); ++i)
						character.Variables.Perm.Set(BelongingVar + i, false);
					character.Variables.Perm.SetInt(BelongingCountVar, 0);

					character.Quests.Start(Sq020);
					character.LookAround();

					await dialog.Msg(L("You say you're going to help me find my family? Really? Wow, I don't know what to say..."));
					await dialog.Msg(L("My family was taken to the Collapsed Grand Corridor. I hope nothing happened to them..."));
				}
				return;
			}

			if (!character.Quests.Has(Sq030) && character.Quests.MeetsPrerequisites(Sq030))
			{
				await dialog.Msg(L("I have one last favor to ask. I can't let these horrible experiments keep happening..."));
				await dialog.Msg(L("Please destroy the demon experiment equipment in the Errzze Oratorium."));

				var answer = await dialog.SelectQuestOffer(Sq030, L("Please... I don't want anyone else to fall victim to this."),
					Option(L("Destroy the testing facilities"), "accept"),
					Option(L("Go back to the village"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq030);

				return;
			}

			if (character.Quests.IsActive(Sq010))
			{
				await dialog.Msg(L("My family is waiting for me. I should go back..."));
				return;
			}

			if (character.Quests.IsActive(Sq020))
			{
				await dialog.Msg(L("I promised them I would be back. They'll be all right... I'm sure they will."));
				return;
			}

			if (character.Quests.IsActive(Sq030))
			{
				await dialog.Msg(L("We have to stop any more experiments like this from happening... I don't want anyone else to suffer..."));
				return;
			}

			if (character.Quests.HasCompleted(Sq030))
			{
				await dialog.Msg(L("I wish we'd escaped sooner. How can I live now without my family...?"));
				return;
			}

			if (character.Quests.HasCompleted(Sq020))
			{
				await dialog.Msg(L("How am I going to live now...? I have no will to be alive anymore..."));
				return;
			}

			if (character.Quests.HasCompleted(Sq010))
			{
				await dialog.Msg(L("Please... I'll do anything to see my family again..."));
				return;
			}

			await dialog.Msg(L("My family... I should go back to them..."));
		});

		// Hilbeth's backpack in the Tebeti Small Corridor
		//-------------------------------------------------------------------------
		AddConditionalNpc(47161, "UnvisibleName", "ABBEY642_LOSTBAG01", "d_abbey_64_2", 1442.58, 610.74, 90, c => c.Quests.IsActive(Sq010) && !c.Quests.IsCompletable(Sq010), async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Sq010) || character.Quests.IsCompletable(Sq010))
				return;

			character.Inventory.Add(ItemId.ABBAY642_SQ1_ITEM, 1, InventoryAddType.PickUp);
			character.LookAround();

			await Task.CompletedTask;
		});

		AddNpc(47160, "UnvisibleName", "d_abbey_64_2", 1413.19, 307.71, 19);
		AddNpc(47160, "UnvisibleName", "d_abbey_64_2", 1403.50, 794.31, 59);
		AddNpc(47160, "UnvisibleName", "d_abbey_64_2", 1765.47, 697.79, 76);
		AddNpc(47160, "UnvisibleName", "d_abbey_64_2", 1760.17, 289.30, 23);

		// The piles of bracken at the Collapsed Grand Corridor
		//-------------------------------------------------------------------------
		for (var i = 0; i < Belongings.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(153116, "UnvisibleName", "ABBEY642_BRACKEN0" + number, "d_abbey_64_2", Belongings[i, 0], Belongings[i, 1], Belongings[i, 2],
				character => character.Quests.IsActive(Sq020) && !character.Quests.IsCompletable(Sq020) && !character.Variables.Perm.GetBool(BelongingVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq020) || character.Quests.IsCompletable(Sq020) || character.Variables.Perm.GetBool(BelongingVar + number, false))
						return;

					character.Variables.Perm.Set(BelongingVar + number, true);
					character.Variables.Perm.SetInt(BelongingCountVar, character.Variables.Perm.GetInt(BelongingCountVar, 0) + 1);
					character.Inventory.Add(BelongingItemId(number), 1, InventoryAddType.PickUp);
					character.LookAround();

					await dialog.Msg(BelongingText(number));
				});
		}

		AddNpc(153116, "UnvisibleName", "d_abbey_64_2", -603.64, 2184.68, -9);
		AddNpc(153116, "UnvisibleName", "d_abbey_64_2", -577.34, 1952.41, 1);
		AddNpc(153116, "UnvisibleName", "d_abbey_64_2", -725.12, 2054.56, -12);

		// Monk Abels
		//-------------------------------------------------------------------------
		AddNpc(155044, L("Monk Abels"), "ABBEY642_MONK01", "d_abbey_64_2", 610.39, 884.19, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Monk Abels"));

			if (character.Quests.IsCompletable(Sq040))
			{
				await dialog.Msg(L("Are those the fragments...? My vision is becoming blurry but I can feel their energy."));
				await dialog.CompleteQuest(Sq040);
				return;
			}

			if (character.Quests.IsCompletable(Sq050))
			{
				await dialog.Msg(L("How I wish to see the monastery fully purified again... I'm afraid I won't have the time."));
				await dialog.Msg(L("I should rest..."));
				await dialog.CompleteQuest(Sq050);
				return;
			}

			if (!character.Quests.Has(Sq040) && character.Quests.MeetsPrerequisites(Sq040))
			{
				await dialog.Msg(L("It's... too late for me now. Please, traveler, will you spare me some of your time? I don't have much left myself..."));
				await dialog.Msg(L("All my life I have dedicated to my service here at the Novaha Monastery... I can't bear to see it tainted by demons as it is."));

				var answer = await dialog.SelectQuestOffer(Sq040, L("Please help me purify the monastery. Gather the holy relic fragments in the Gaile Chapel..."),
					Option(L("I'll collect some right away"), "accept"),
					Option(L("I'm not so sure I can help you"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Fragments.GetLength(0); ++i)
						character.Variables.Perm.Set(FragmentVar + i, false);

					character.Quests.Start(Sq040);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Sq050) && character.Quests.MeetsPrerequisites(Sq050))
			{
				await dialog.Msg(L("There's... not much left now. I gathered all the strength I have left to restore the holy relic."));

				var answer = await dialog.SelectQuestOffer(Sq050, L("Use this around the monastery..."),
					Option(L("I will come back soon"), "accept"),
					Option(L("Just give me some time"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= RelicSpots.GetLength(0); ++i)
						character.Variables.Perm.Set(RelicVar + i, false);
					character.Variables.Perm.SetInt(RelicCountVar, 0);

					character.Quests.Start(Sq050);
					character.Inventory.Add(ItemId.ABBAY642_SQ5_ITEM01, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (character.Quests.IsActive(Sq040))
			{
				await dialog.Msg(L("Please help me complete this final mission. I can't return to the goddess with the monastery tainted by these demons..."));
				return;
			}

			if (character.Quests.IsActive(Sq050))
			{
				await dialog.Msg(L("...I'm terribly sorry to the people of the Croa Village. I failed to protect them..."));
				return;
			}

			if (character.Quests.HasCompleted(Sq040))
			{
				await dialog.Msg(L("I am slowly losing consciousness... Why are the goddesses still not answering to me..."));
				return;
			}

			await dialog.Msg(L("I am so bitter and resentful about myself... How I could only watch as the demons defiled the monastery..."));
		});

		// Holy relic fragments at Gaile Chapel
		//-------------------------------------------------------------------------
		for (var i = 0; i < Fragments.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(151022, L("Holy Relic Fragment"), "ABBEY642_ORB_" + number, "d_abbey_64_2", Fragments[i, 0], Fragments[i, 1], 90,
				character => character.Quests.IsActive(Sq040) && !character.Quests.IsCompletable(Sq040) && !character.Variables.Perm.GetBool(FragmentVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq040) || character.Quests.IsCompletable(Sq040) || character.Variables.Perm.GetBool(FragmentVar + number, false))
						return;

					character.Variables.Perm.Set(FragmentVar + number, true);
					character.Inventory.Add(ItemId.ABBAY642_SQ4_ITEM01, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// The Novaha Relics placed around the monastery
		//-------------------------------------------------------------------------
		for (var i = 0; i < RelicSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(153026, L("Novaha Relic"), "ABBEY642_ORB_SET0" + number, "d_abbey_64_2", RelicSpots[i, 0], RelicSpots[i, 1], RelicSpots[i, 2],
				character => character.Quests.HasCompleted(Sq050) || (character.Quests.IsActive(Sq050) && character.Variables.Perm.GetBool(RelicVar + number, false)));

			AddQuestTrigger("ABBEY642_ORB_SETUP0" + number, "d_abbey_64_2", RelicSpots[i, 0], RelicSpots[i, 1], 60, async args =>
			{
				if (args.Initiator is not Character character)
					return;

				if (!character.Quests.IsActive(Sq050) || character.Quests.IsCompletable(Sq050) || character.Variables.Perm.GetBool(RelicVar + number, false))
					return;

				if (character.Inventory.CountItem(ItemId.ABBAY642_SQ5_ITEM01) == 0)
					return;

				character.Variables.Perm.Set(RelicVar + number, true);
				var placed = character.Variables.Perm.GetInt(RelicCountVar, 0) + 1;
				character.Variables.Perm.SetInt(RelicCountVar, placed);

				character.ServerMessage(LF("Holy relics placed: {0}/{1}", Math.Min(placed, 3), 3));
				character.LookAround();

				await Task.CompletedTask;
			});
		}
	}

	/// <summary>
	/// Returns the item id of the given belonging of Hilbeth's family.
	/// </summary>
	private static int BelongingItemId(int number)
	{
		switch (number)
		{
			case 1: return ItemId.ABBAY642_SQ41_ITEM01;
			case 2: return ItemId.ABBAY642_SQ41_ITEM02;
			case 3: return ItemId.ABBAY642_SQ41_ITEM03;
			default: return ItemId.ABBAY642_SQ41_ITEM04;
		}
	}

	/// <summary>
	/// Returns what the character notices about the given belonging.
	/// </summary>
	private static string BelongingText(int number)
	{
		switch (number)
		{
			case 1: return L("(I found a chunky ring that seems to belong to a man.)");
			case 2: return L("(A thin bracelet... It seems to belong to a woman.)");
			case 3: return L("(A jewel necklace. 'My beloved mother' is engraved on the back.)");
			default: return L("(Found a handkerchief with detailed embroideries.)");
		}
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50125: Rescue Edmundas (1)
//-----------------------------------------------------------------------------
public class Abbay642Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50125);
		SetName(L("Rescue Edmundas (1)"));
		SetDescription(L("Traveling Merchant Rose went into hiding as the demons are after her. Search for the place where her brother Edmundas is being held captive in the Novaha Annex."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_ROZE01", "d_abbey_64_2", L("Follow Traveling Merchant Rose to the Novaha Annex"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_EDMONDA_CHECK", "d_abbey_64_2", L("Search for Rose's brother, Edmundas"));
		SetPhase(QuestStatus.Success, "ABBEY642_ROZE01", "d_abbey_64_2", L("Report to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50121, QuestStatus.Completed));

		AddObjective("findEdmundas", L("Look for Rose's brother, Edmundas"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 50126: Rescue Edmundas (2)
//-----------------------------------------------------------------------------
public class Abbay642Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50126);
		SetName(L("Rescue Edmundas (2)"));
		SetDescription(L("Edmundas says a wizard came into the room, causing him to feel pain all of a sudden. Go to the inner section of the Apega State Chamber and destroy the thing that's hurting Edmundas."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_ROZE02", "d_abbey_64_2", L("Follow Rose into the Apega State Chamber"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_ROZE02", "d_abbey_64_2", L("Eliminate the cause of Edmundas' pain"));
		SetPhase(QuestStatus.Success, "ABBEY642_ROZE02", "d_abbey_64_2", L("Talk to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50125, QuestStatus.Completed));

		AddObjective("destroyStones", L("Destroy the reason that is causing Edmundas' pain"), new VariableCheckObjective(DAbbey642QuestNpcsScript.StoneCountVar, 4, isPermanent: true));

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 50127: Rescue Edmundas (3)
//-----------------------------------------------------------------------------
public class Abbay642Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50127);
		SetName(L("Rescue Edmundas (3)"));
		SetDescription(L("As you were about to destroy the protective barrier around Edmundas, a mysterious wizard appeared and kidnapped Rose! Hurry and destroy the wizard's Magic Stone of Pain!"));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_ROZE02", "d_abbey_64_2", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_EDMONDAS", "d_abbey_64_2", L("Destroy the Mysterious Wizard's Magic Stone of Pain"));
		SetPhase(QuestStatus.Success, "ABBEY642_EDMONDAS", "d_abbey_64_2", L("Talk to Edmundas"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ABBAY_64_2_MQ030_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50126, QuestStatus.Completed));

		AddObjective("destroyStones", L("Destroy the Mysterious Wizard's Magic Stone of Pain"), new KillObjective(4, "Link_stone_small") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 4));
	}
}

// 50128: Rescue Edmundas (4)
//-----------------------------------------------------------------------------
public class Abbay642Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50128);
		SetName(L("Rescue Edmundas (4)"));
		SetDescription(L("Find a way to release Edmundas from the shackles in the State Chamber."));
		SetType(QuestType.Main);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_EDMONDAS", "d_abbey_64_2", L("Talk to Edmundas"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_DEVICE05", "d_abbey_64_2", L("Find a way to release Edmundas from the shackles"));
		SetPhase(QuestStatus.Success, "ABBEY642_EDMONDAS", "d_abbey_64_2", L("Talk to Edmundas"));

		AddPrerequisite(new QuestStatusPrerequisite(50127, QuestStatus.Completed));

		AddObjective("releaseShackles", L("Find a way to release Edmundas from the shackles"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 50129: The Experiment (1)
//-----------------------------------------------------------------------------
public class Abbay642Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50129);
		SetName(L("The Experiment (1)"));
		SetDescription(L("Experiment Victim Hilbeth wants you to find his backpack of herbs. Go to the Tebeti Small Corridor and look for Hilbeth's backpack."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_PEAPLE01", "d_abbey_64_2", L("Talk to Experiment Victim Hilbeth"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_LOSTBAG01", "d_abbey_64_2", L("Search for Hilbeth's Backpack"));
		SetPhase(QuestStatus.Success, "ABBEY642_PEAPLE01", "d_abbey_64_2", L("Deliver to Experiment Victim Hilbeth"));

		AddPrerequisite(new LevelPrerequisite(39));

		AddObjective("findBackpack", L("Look for the Hilbeth's backpack in Tebeti Small Corridor"), new CollectItemObjective("ABBAY642_SQ1_ITEM", 1));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("ABBAY642_SQ1_ITEM", -1));
	}
}

// 50130: The Experiment (2)
//-----------------------------------------------------------------------------
public class Abbay642Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50130);
		SetName(L("The Experiment (2)"));
		SetDescription(L("Hilbeth's family are being victims of demon experiments at the Collapsed Grand Corridor and need to be rescued. Go there and investigate."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_PEAPLE01", "d_abbey_64_2", L("Talk to Experiment Victim Hilbeth"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_PEAPLE01", "d_abbey_64_2", L("Search the Collapsed Grand Corridor"));
		SetPhase(QuestStatus.Success, "ABBEY642_PEAPLE01", "d_abbey_64_2", L("Talk to Experiment Victim Hilbeth"));

		AddPrerequisite(new QuestStatusPrerequisite(50129, QuestStatus.Completed));

		AddObjective("searchCorridor", L("Search the Collapsed Grand Corridor"), new VariableCheckObjective(DAbbey642QuestNpcsScript.BelongingCountVar, 4, isPermanent: true));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("ABBAY642_SQ41_ITEM01", -1));
		AddReward(new TakeItemReward("ABBAY642_SQ41_ITEM02", -1));
		AddReward(new TakeItemReward("ABBAY642_SQ41_ITEM03", -1));
		AddReward(new TakeItemReward("ABBAY642_SQ41_ITEM04", -1));
	}
}

// 50131: The Experiment (3)
//-----------------------------------------------------------------------------
public class Abbay642Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50131);
		SetName(L("The Experiment (3)"));
		SetDescription(L("Experiment Victim Hilbeth wants to stop any kind of experiment from happening again. As requested, destroy the remaining demon experiment facilities at the Errzze Oratorium."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_PEAPLE01", "d_abbey_64_2", L("Talk to Experiment Victim Hilbeth"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_PEAPLE01", "d_abbey_64_2", L("Destroy the remaining experiment facilities at the Errzze Oratorium"));
		SetPhase(QuestStatus.Success, "ABBEY642_PEAPLE01", "d_abbey_64_2", L("Talk to Experiment Victim Hilbeth"));

		AddPrerequisite(new QuestStatusPrerequisite(50130, QuestStatus.Completed));

		AddObjective("destroyFacilities", L("Destroy the remaining experiment facilities at the Errzze Oratorium"), new KillObjective(6, "firetower_device_01_Q", "firetower_valve_Q"));

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 50132: A Monk's Last Mission (1)
//-----------------------------------------------------------------------------
public class Abbay642Sq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50132);
		SetName(L("A Monk's Last Mission (1)"));
		SetDescription(L("Monk Abels cannot turn a blind eye to the damage the demons have done. Go to Gaile Chapel and retrieve the holy relic fragments needed to purify the monastery."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_MONK01", "d_abbey_64_2", L("Talk to Monk Abels"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_MONK01", "d_abbey_64_2", L("Retrieve Holy Relic Fragments at Gaile Chapel"));
		SetPhase(QuestStatus.Success, "ABBEY642_MONK01", "d_abbey_64_2", L("Deliver to Monk Abels"));

		AddPrerequisite(new LevelPrerequisite(39));

		AddObjective("collectFragments", L("Retrieve Holy Relic Fragments at Gaile Chapel"), new CollectItemObjective("ABBAY642_SQ4_ITEM01", 7));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("ABBAY642_SQ4_ITEM01", -1));
	}
}

// 50133: A Monk's Last Mission (2)
//-----------------------------------------------------------------------------
public class Abbay642Sq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50133);
		SetName(L("A Monk's Last Mission (2)"));
		SetDescription(L("Monk Abels put his last efforts into restoring the holy relic. Use the relic to purify different locations in the monastery."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ABBEY642_MONK01", "d_abbey_64_2", L("Talk to Monk Abels"));
		SetPhase(QuestStatus.InProgress, "ABBEY642_MONK01", "d_abbey_64_2", L("Prepare the relic for purifying the monastery"));
		SetPhase(QuestStatus.Success, "ABBEY642_MONK01", "d_abbey_64_2", L("Report to Monk Abels"));

		AddPrerequisite(new QuestStatusPrerequisite(50132, QuestStatus.Completed));

		AddObjective("placeRelics", L("Place the Holy Relics to purify the monastery"), new VariableCheckObjective(DAbbey642QuestNpcsScript.RelicCountVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new TakeItemReward("ABBAY642_SQ5_ITEM01", 1));
	}
}
