//--- Melia Script ----------------------------------------------------------
// Knidos Jungle Quest NPCs
//--- Description -----------------------------------------------------------
// Rose's search of the empty Croa Village, Tess who escaped the demons,
// and the herbalist who saw a demon picking Aconite.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken632QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Bracken631Mq040 = new QuestId(50093);
	private readonly static QuestId Mq010 = new QuestId(50099);
	private readonly static QuestId Mq020 = new QuestId(50100);
	private readonly static QuestId Mq030 = new QuestId(50101);
	private readonly static QuestId Mq040 = new QuestId(50102);
	private readonly static QuestId Mq050 = new QuestId(50103);
	private readonly static QuestId Mq060 = new QuestId(50145);
	private readonly static QuestId Sq010 = new QuestId(50104);
	private readonly static QuestId Sq020 = new QuestId(50105);
	private readonly static QuestId Sq030 = new QuestId(50106);
	private readonly static QuestId Sq040 = new QuestId(50107);
	private readonly static QuestId Rp1 = new QuestId(60162);
	private readonly static QuestId Abbey642Hq1 = new QuestId(50276);
	private readonly static QuestId Abbey642Hq2 = new QuestId(50277);
	private readonly static QuestId Abbay641Mq050 = new QuestId(50121);
	private readonly static QuestId Abbay643Mq050 = new QuestId(50144);
	private readonly static QuestId Abbay643Hq1 = new QuestId(50261);

	public const string SprayCountVar = "Gabija.Quests.Bracken632Sq040.Sprayed";
	private const string SprayVar = "Gabija.Quests.Bracken632Sq040.Spray";
	private const string HerbVar = "Gabija.Quests.Bracken632Sq020.Herb";

	private const int SpraysNeeded = 12;
	private const double SprayRange = 80;

	private static readonly TimeSpan AconiteRegrowth = TimeSpan.FromSeconds(20);

	private static readonly double[,] Aconite =
	{
		{ -1436.78, 337.67 }, { -1366.30, 0.30 }, { -1777.15, 215.41 }, { -1633.79, 385.29 }, { -1663.92, 24.53 },
		{ -1529.22, 136.73 }, { -1261.92, 217.98 },
	};

	private static readonly double[,] Beads =
	{
		{ 374.56, 1146.41 }, { 338.04, 984.22 }, { 360.22, 814.69 }, { 432.34, 699.77 }, { 426.08, 502.13 },
		{ 353.78, 289.02 },
	};

	protected override void Load()
	{
		// Traveling Merchant Rose in the Croa Village
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "BRACKEN632_ROZE01", "f_bracken_63_2", 394.42, 1213.34, 90, c => c.Quests.HasCompleted(Bracken631Mq040) && !c.Quests.Has(Mq020), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("It looks like... something bad has happened."));
				await dialog.Msg(L("The inside of our house is a complete chaos and there's demon footprints everywhere. I couldn't find any notes either."));
				await dialog.Msg(L("I wonder what could've happened. I've seen too many villages destroyed by demon attacks during my trips as a merchant, you know"));
				await dialog.Msg(L("What if... No. I'm worrying too much, aren't I? There would be at least some tombs left behind if that were the case, but there aren't any. Right?"));
				await dialog.CompleteQuest(Mq010);
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("Hm? I thought you'd left earlier... What are you doing here?"));
				await dialog.Msg(L("Ah... You were worried about me? Thank you. I don't think the demons have noticed me yet. I'll look somewhere else."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("I'm going back to the village and look through our house. If they're gone because of the migration order, my brother should've left a note."),
					Option(L("I'm worried about the demons; I'll check in on your people"), "accept"),
					Option(L("First, let's go to Orsha and ask around"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);

					await dialog.Msg(L("Really? I can be trouble sometimes, but thank you for worrying about me."));
					await dialog.Msg(L("Will you have a look around the village, then? I'm thinking there will be some clues left."));
				}
				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				await dialog.Msg(L("Those beads, though... I saw them before, too... I wonder if they're from Anne's necklace."));
				await dialog.Msg(L("That's odd. She would never drop her necklace... It's very precious to Anne, you see."));

				var answer = await dialog.SelectQuestOffer(Mq020, L("Still... The beads could be a clue to help us find where the villagers are. Anne is smart enough to have done something like that. She was always clever."),
					Option(L("Let's try and find more beads"), "accept"),
					Option(L("I'm sure it'll be nothing"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("I wouldn't think much of it otherwise, but all the mess and the demon footprints... It's just bothering me."));
					await dialog.Msg(L("The only strange thing we've found so far are the beads from Anne's necklace... Let's try and follow the trail of the beads. We need to try something, anything."));

					character.Quests.Start(Mq020);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("I was actually worried about coming alone, so thank you. Now if only we can find the people of my village."));
		});

		// The villagers' traces in the Croa Village
		//-------------------------------------------------------------------------
		AddQuestTrigger("BRACKEN632_TRACES01", "f_bracken_63_2", 487.47, 912.14, 60, async args => await this.CheckTrace(args, "checkCenter", L("(There's traces of something having been dragged here.)")));
		AddQuestTrigger("BRACKEN632_TRACES02", "f_bracken_63_2", 715.17, 873.24, 60, async args => await this.CheckTrace(args, "checkHouse", L("(There's only demon footprints left all over, nothing else.)")));
		AddQuestTrigger("BRACKEN632_TRACES03", "f_bracken_63_2", 368.82, 581.68, 60, async args => await this.CheckTrace(args, "checkWarehouse", L("(There doesn't seem to be any blood stains here.)")));

		// Anne's beads
		//-------------------------------------------------------------------------
		for (var i = 0; i < Beads.GetLength(0); ++i)
		{
			var name = i == 0 ? "BRACKEN632_TRACES_OBJECT" : "BRACKEN632_TRACES_OBJECT0" + i;
			AddConditionalNpc(153105, "UnvisibleName", name, "f_bracken_63_2", Beads[i, 0], Beads[i, 1], 90, c => c.Quests.IsActive(Mq020) && !c.Quests.IsCompletable(Mq020));
		}

		AddQuestTrigger("BRACKEN632_TRACES_OBJECT05_TRIGGER", "f_bracken_63_2", 353.78, 289.02, 160, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq020, "followBeads"))
				return;

			character.Quests.CompleteObjective(Mq020, "followBeads");
			character.LookAround();

			await Task.CompletedTask;
		});

		// Traveling Merchant Rose at the end of the beads
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "BRACKEN632_ROZE02", "f_bracken_63_2", 344.46, 349.77, 22, c => (c.Quests.IsCompletable(Mq020) || c.Quests.HasCompleted(Mq020)) && !c.Quests.Has(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("It looks like the beads end here. I don't see any more of them."));
				await dialog.CompleteQuest(Mq020);
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				await dialog.Msg(L("We can't give up. Maybe we're just not seeing the rest of the beads."));
				await dialog.Msg(L("They could've been kicked out of the way by monsters or..."));

				var answer = await dialog.SelectQuestOffer(Mq030, L("Let's take a look around. I'm sure we're missing something here."),
					Option(L("I'll try and have a look around"), "accept"),
					Option(L("It's best to go back to the village and find more clues"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq030);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				await dialog.Msg(L("I feel so desperate I want to cry. Please let us find something, please..."));
				return;
			}

			await dialog.Msg(L("Has Tess been chased by the demons just like other people? I have no idea what happened here."));
		});

		AddQuestTrigger("BRACKEN632_TRACES_SEARCH02", "f_bracken_63_2", 425.54, -564.63, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq030, "searchClues"))
				return;

			character.Quests.CompleteObjective(Mq030, "searchClues");
			character.LookAround();

			await Task.CompletedTask;
		});

		// Tess, hiding from the demons
		//-------------------------------------------------------------------------
		AddConditionalNpc(20064, L("Tess"), "BRACKEN632_TOWN_PEAPLE", "f_bracken_63_2", 423.53, -563.39, 90, c => c.Quests.Has(Mq030) && !c.Quests.Has(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Tess"));

			if (character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Don't hurt me!"));
				await dialog.CompleteQuest(Mq030);
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				var answer = await dialog.SelectQuestOffer(Mq040, L("A-are you... on the same side with the demons?"),
					Option(L("Don't worry, I'm not a demon"), "accept"),
					Option(L("Let's wait until things cool down a little"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq040);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("Don't hurt me!"));
		});

		// Tess and Rose after the demons were driven off
		//-------------------------------------------------------------------------
		AddConditionalNpc(20064, L("Tess"), "BRACKEN632_TOWN_PEAPLE_1", "f_bracken_63_2", 846.84, -338.78, 235, c => c.Quests.Has(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Tess"));

			if (character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("I'm saved! I'm finally saved..."));
				await dialog.Msg(L("Oh... It was horrible..."));
				await dialog.CompleteQuest(Mq040);
				return;
			}

			if (character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("Did you really get all of them? I can still feel them glaring at me from between the trees..."));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				await dialog.Msg(L("Everyone in the village was taken. The demons said they were going to use a type of bracken to feed us to the Divine Tree."));
				await dialog.Msg(L("Yes, the Divine Tree from four years ago! The one that spurted from under the ground and destroyed our cities on Medzio Diena..."));

				var answer = await dialog.SelectQuestOffer(Mq050, L("I managed to escape, but everyone else was taken to the Novaha Monastery."),
					Option(L("Exactly what is happening in the Croa Village?"), "accept"),
					Option(L("I'll ask later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq050);
					character.Quests.CompleteObjective(Mq050, "hearTess");

					await dialog.Msg(L("When the demons were taking us they suddenly stopped and a wizard appeared. I could tell for sure he was human... Oh, but that's not important."));
					await dialog.Msg(L("The wizard approached us one by one and then he did some sort of strange ritual. At first no one showed much of a reaction..."));
					await dialog.Msg(L("But when it was Edmundas' turn, there was a light. Yes, Rose, your brother Edmundas."));
					await dialog.Msg(L("The wizard had Edmundas taken somewhere separately. Then he started to threaten us, asking if he had any family..."));
					await dialog.Msg(L("I feel so sorry to Rose, but we were so afraid we had no choice. As soon as the wizard heard about Rose, he started saying some really scary things."));
					await dialog.Msg(L("Was it Kru... Kruvina? He said it was completed... He said the giant bracken was almost ready and soon he would spread the spores of death."));
					await dialog.Msg(L("He said all he needed was to find someone qualified to feed us to the Divine Tree."));
					await dialog.Msg(L("I thought I needed to tell Orsha about this so I took the chance and ran away. I've been hiding from the demons here ever since."));
				}
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				var answer = await dialog.SelectQuestOffer(Rp1, L("Please don't leave me alone here! If you go, the Lapasape Mages will come and take me! Please!"),
					Option(L("I'll make sure it's safe."), "accept"),
					Option(L("I think you should hide better."), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Rp1);

				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				await dialog.Msg(L("Every single one of the village residents was taken. What... What do I do now...?"));
				character.Quests.ReplayQuestTrack(Mq040);
				return;
			}

			if (character.Quests.IsActive(Mq050))
			{
				await dialog.Msg(L("Orsha is too far from here! Please, help us save our people..."));
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("I don't want them to take me. I miss my mother..."));
				return;
			}

			if (character.Quests.HasCompleted(Mq050))
			{
				await dialog.Msg(L("The other village residents were taken to the Novaha Monastery. I'm too scared... I was so afraid..."));
				return;
			}

			await dialog.Msg(L("Every single one of the village residents was taken. What... What do I do now...?"));
		});

		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "BRACKEN632_ROZE03", "f_bracken_63_2", 856.35, -314.26, -71, c => c.Quests.Has(Mq040) && !c.Quests.HasCompleted(Mq060), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq060))
			{
				await dialog.Msg(L("The Novaha Monastery is just on the other side of the Dadan Jungle."));
				await dialog.Msg(L("I'll go to the Dadan Jungle first. We meet each other there."));
				await dialog.CompleteQuest(Mq060);

				if (character.Quests.HasCompleted(Mq060))
					character.LookAround();
				return;
			}

			if (character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("What do I do? Edmundas, my brother..."));
				await dialog.Msg(L("My brother was taken by the demons."));
				await dialog.CompleteQuest(Mq050);

				if (!character.Quests.HasCompleted(Mq050) || character.Quests.Has(Mq060))
					return;
			}

			if (!character.Quests.Has(Mq060) && character.Quests.MeetsPrerequisites(Mq060))
			{
				await dialog.Msg(L("After losing my parents on Medzio Diena... The only family I have left is my brother, Edmundas."));

				var answer = await dialog.SelectQuestOffer(Mq060, L("Please. You're the only one I can count on. So please... Help save the townfolk and my brother..."),
					Option(L("Let's all help out"), "accept"),
					Option(L("Tell him that you need some time to think"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq060);
					character.Quests.CompleteObjective(Mq060, "comfortRose");

					await dialog.Msg(L("Thank you. I am a bit calmer thanks to your consolation. I need to keep my wits about me even more in times like this, right?"));
					await dialog.Msg(L("The Novaha Monastery is just on the other side of the Dadan Jungle. But we are facing the demons... We can't just rush in without any preparation."));
					await dialog.Msg(L("Spores and ferns... If there is anything I've learned as a traveling merchant, it's to take things slowly instead of rushing onwards."));
					await dialog.Msg(L("We don't even know what a 'Kruvina' is. I am sure that stopping the demons will lead to us saving the townfolk and my brother as well."));
					await dialog.Msg(L("I'll go to the Dadan Jungle first. We meet each other there."));
				}
				return;
			}

			if (character.Quests.HasCompleted(Mq040))
			{
				await dialog.Msg(L("I'm really glad Tess is safe, too. Does he know where the rest of the villagers are?"));
				return;
			}

			await dialog.Msg(L("Those demons just now... They were coming to take Tess, weren't they?"));
		});

		// Herbalist Ash
		//-------------------------------------------------------------------------
		AddNpc(147479, L("Herbalist Ash"), "BRACKEN632_PEAPLE01", "f_bracken_63_2", -1469.08, -814.29, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Herbalist Ash"));

			if (character.Quests.IsCompletable(Sq010))
			{
				await dialog.Msg(L("Thank you. I wonder, where could all the people of the Croa Village have gone?"));
				await dialog.CompleteQuest(Sq010);
				return;
			}

			if (character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("I didn't recognize it before but... These are Aconites."));
				await dialog.Msg(L("They're medicinal, but deadly when consumed in large doses. I wonder why they want this..."));
				await dialog.CompleteQuest(Sq020);
				return;
			}

			if (character.Quests.IsCompletable(Sq030))
			{
				await dialog.Msg(L("Thank you. This should be enough."));
				await dialog.CompleteQuest(Sq030);
				return;
			}

			if (character.Quests.IsCompletable(Sq040))
			{
				await dialog.Msg(L("This is all that wizard's fault. I saw him talking to those demons with my very own eyes."));
				await dialog.Msg(L("Thank you for helping me. Now that the Aconites are gone, I hope they can't carry out whatever plan they had."));
				await dialog.CompleteQuest(Sq040);
				return;
			}

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				await dialog.Msg(L("I saw something strange when I was picking herbs. There was a wizard... A human, no doubt. But he was giving orders to the demons."));
				await dialog.Msg(L("Suddenly he saw me and I had to run as fast as I could."));

				var answer = await dialog.SelectQuestOffer(Sq010, L("I'm scared and I want to go back to Orsha, but I can't bring myself to do it with all these demons around."),
					Option(L("I will tidy up the place"), "accept"),
					Option(L("There's too many demons; I can't do it"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq010);

				return;
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				await dialog.Msg(L("Have you ever heard about demons picking herbs? I've been picking herbs for a long time but that was a first, even for me."));
				await dialog.Msg(L("I'm tired of running from demons, too, but... As a herb gatherer myself I can't help but be curious."));

				var answer = await dialog.SelectQuestOffer(Sq020, L("If that's fine with you, will you pick some those herbs so I can see what they are?"),
					Option(L("I'll take a look"), "accept"),
					Option(L("First we need to escape to somewhere safe"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Aconite.GetLength(0); ++i)
						character.Variables.Perm.Set(HerbVar + i, false);

					character.Quests.Start(Sq020);
					character.LookAround();

					await dialog.Msg(L("I was hiding so I couldn't take a good look at them, but the leaves were small and thin. It's to right, at the Neneva Yard."));
				}
				return;
			}

			if (!character.Quests.Has(Sq030) && character.Quests.MeetsPrerequisites(Sq030))
			{
				await dialog.Msg(L("The demons may scare me, but as a herb gatherer I can't let this go. I think it's best to destroy all the Aconites."));

				var answer = await dialog.SelectQuestOffer(Sq030, L("Go and collect some Loktanun fluid. It's very acidic; it wil dry out the Aconites if you spray them with it."),
					Option(L("I'll get it quickly"), "accept"),
					Option(L("First we need to find somewhere safe"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq030);

				return;
			}

			if (!character.Quests.Has(Sq040) && character.Quests.MeetsPrerequisites(Sq040))
			{
				var answer = await dialog.SelectQuestOffer(Sq040, L("It's a shame to waste all the Aconites, but there's no way the demons will put it to good use. You can go back to the Neneva Yard and spray the demon fluid all around."),
					Option(L("I'll help you spray the Loktanun Fluid"), "accept"),
					Option(L("Can we stop and go back now?"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Aconite.GetLength(0); ++i)
						character.Variables.Temp.Remove(SprayVar + i);
					character.Variables.Perm.SetInt(SprayCountVar, 0);

					character.Quests.Start(Sq040);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq010))
			{
				await dialog.Msg(L("A few months ago, demons started to appear in Knidos Jungle. Many good herbs used to grow around here... Not so much anymore."));
				return;
			}

			if (character.Quests.IsActive(Sq020))
			{
				await dialog.Msg(L("I saw a human-looking wizard give orders to the demons. Humans associating with demons..."));
				return;
			}

			if (character.Quests.IsActive(Sq030))
			{
				await dialog.Msg(L("The demons may be scary... But I won't let them get away with this."));
				return;
			}

			if (character.Quests.IsActive(Sq040))
			{
				await dialog.Msg(L("This is the most I can do. You only need to spray a little to dry out the flowers."));
				return;
			}

			if (character.Quests.HasCompleted(Sq040))
			{
				await dialog.Msg(L("The world really is going to ruins. It's the same thing I felt four years ago on Medzio Diena..."));
				return;
			}

			await dialog.Msg(L("A few months back demons started to appear in Knidos Jungle. A lot of good herbs used to grow around here... Not so much anymore."));
		});

		// The Aconite at Neneva Yard
		//-------------------------------------------------------------------------
		for (var i = 0; i < Aconite.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(47200, "UnvisibleName", "BRACKEN632_MEDICAL_PLANT_" + number, "f_bracken_63_2", Aconite[i, 0], Aconite[i, 1], 90,
				character => IsHerbPickable(character, number) || (character.Quests.IsActive(Sq040) && !character.Quests.IsCompletable(Sq040)),
				async dialog =>
				{
					var character = dialog.Player;

					if (character.Quests.IsActive(Sq040) && !character.Quests.IsCompletable(Sq040))
					{
						character.ServerMessage(L("Spray the Loktanun Fluid on the Aconite."));
						return;
					}

					if (!IsHerbPickable(character, number))
						return;

					character.Variables.Perm.Set(HerbVar + number, true);
					character.Inventory.Add(ItemId.BRACKEN632_SQ2_ITEM02, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Croa villagers back from the Novaha Monastery
		//-------------------------------------------------------------------------
		AddConditionalNpc(20063, L("Kornas"), "BRACKEN632_TOWN_PEAPLE1", "f_bracken_63_2", 217.24, 882.25, 90, AreVillagersBack, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kornas"));

			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("Oh! You're the person who saved me and Rose. Where is she anyway?"));
			else
				await dialog.Msg(L("If it wasn't for you and Rose, we would have been subjected to horrific experiments. I want to thank Rose as well... She's coming soon I hope?"));
		});

		AddConditionalNpc(153111, L("Rona"), "BRACKEN632_TOWN_PEAPLE2", "f_bracken_63_2", 686.62, 967.04, -26, AreVillagersBack, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Rona"));

			if (character.Quests.IsCompletable(Abbey642Hq2))
			{
				await dialog.Msg(L("Thank you. We will always remember them and make sure no one ever has to go through the same fate."));
				await dialog.CompleteQuest(Abbey642Hq2);
				return;
			}

			if (!character.Quests.Has(Abbey642Hq1) && character.Quests.MeetsPrerequisites(Abbey642Hq1))
			{
				await dialog.Msg(L("We want to appease the spirits of the victims of Novaha. But the Monastery is so full of monsters we can't possibly erect a memorial there even if we want to."));

				var answer = await dialog.SelectQuestOffer(Abbey642Hq1, L("Would... Would you maybe place the memorial at the Novaha Assembly Hall for us?"),
					Option(L("I'll erect the tombstones for you."), "accept"),
					Option(L("You're better off asking someone else."), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Abbey642Hq1);

				return;
			}

			if (character.Quests.HasCompleted(Abbey642Hq2))
			{
				await dialog.Msg(L("There are still too many people that didn't make it back into town. I sincerely hope that... nothing has happened to them."));
				return;
			}

			await dialog.Msg(L("My lord... I still get goosebumps even thinking about it. How did they think of using people as nourishment for the Divine Tree?"));
		});

		AddConditionalNpc(20061, L("Anne"), "BRACKEN632_TOWN_PEAPLE3", "f_bracken_63_2", 221.36, 693.39, 90, AreVillagersBack, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Anne"));

			if (character.Quests.IsActive(Abbay643Hq1, "deliverLetter") && character.Inventory.CountItem(ItemId.ABBAY64_3_HIDDENQ1_ITEM1) > 0)
			{
				var delivered = await character.TimeActions.StartAsync(L("Delivering Rose's letter..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
				if (delivered != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("I'm glad to hear Rose and Edmundas are okay. When Edmundas was taken by the demons, I really..."));
				await dialog.Msg(L("I should write back to Rose and Edmundas. Just a moment."));
				character.ServerMessage(L("Anne is writing a letter to Rose and her brother."));
				await dialog.Msg(L("Please take this letter to Rose."));

				character.Inventory.RemoveItem(ItemId.ABBAY64_3_HIDDENQ1_ITEM1, 1);
				character.Inventory.Add(ItemId.ABBAY64_3_HIDDENQ1_ITEM2, 1, InventoryAddType.PickUp);
				return;
			}

			if (character.Quests.IsActive(Abbay643Hq1))
			{
				await dialog.Msg(L("And please don't read the letter on your way there."));
				return;
			}

			await dialog.Msg(L("I really treasured that necklace... But if it helped Rose find us all the way here, it was for a good cause."));
			await dialog.Msg(L("By the way, where is Rose? And Edmundas?"));

			if (character.Quests.HasCompleted(Abbay643Mq050))
				character.Variables.Perm.Set(Abbey643AnneMissesRosePrerequisite.VarName, true);
		});

		AddConditionalNpc(153110, L("Allonas"), "BRACKEN632_TOWN_PEAPLE4", "f_bracken_63_2", 279.36, 1157.42, 60, AreVillagersBack, async dialog =>
		{
			dialog.SetTitle(L("Allonas"));

			if (GameRandom.Get().NextDouble() >= 0.5)
				await dialog.Msg(L("The memories of being taken by the demons still hurt. Some of us haven't made it back yet, either..."));
			else
				await dialog.Msg(L("We're alive thanks to you and Rose... But I don't think the village will be the same again."));
		});
	}

	/// <summary>
	/// Returns whether the Croa villagers made it home from the Novaha
	/// Monastery.
	/// </summary>
	private static bool AreVillagersBack(Character character)
		=> character.Quests.HasCompleted(Abbay641Mq050);

	/// <summary>
	/// Returns whether the given Aconite can still be picked for Ash.
	/// </summary>
	private static bool IsHerbPickable(Character character, int number)
		=> character.Quests.IsActive(Sq020) && !character.Quests.IsCompletable(Sq020) && !character.Variables.Perm.GetBool(HerbVar + number, false);

	/// <summary>
	/// Checks one of the places in the Croa Village for traces of the
	/// villagers.
	/// </summary>
	private async Task CheckTrace(TriggerActorArgs args, string objectiveIdent, string observation)
	{
		if (args.Initiator is not Character character)
			return;

		if (!character.Quests.IsActive(Mq010, objectiveIdent))
			return;

		character.Quests.CompleteObjective(Mq010, objectiveIdent);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, observation, 5);

		await Task.CompletedTask;
	}

	/// <summary>
	/// Sprays Loktanun Fluid on the closest Aconite at Neneva Yard.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_BRACKEN632_SQ2_ITEM01(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_bracken_63_2" || !character.Quests.IsActive(Sq040) || character.Quests.IsCompletable(Sq040))
		{
			character.ServerMessage(L("There is no Aconite nearby to spray."));
			return ItemUseResult.Fail;
		}

		var closest = -1;
		var closestDistance = SprayRange;
		for (var i = 0; i < Aconite.GetLength(0); ++i)
		{
			var distance = character.Position.Get2DDistance(new Position((float)Aconite[i, 0], character.Position.Y, (float)Aconite[i, 1]));
			if (distance <= closestDistance)
			{
				closest = i + 1;
				closestDistance = distance;
			}
		}

		if (closest == -1)
		{
			character.ServerMessage(L("There is no Aconite nearby to spray."));
			return ItemUseResult.Fail;
		}

		var sprayedAt = character.Variables.Temp.GetLong(SprayVar + closest, 0);
		if (sprayedAt != 0 && DateTime.Now - new DateTime(sprayedAt) < AconiteRegrowth)
		{
			character.ServerMessage(L("This Aconite has already dried out."));
			return ItemUseResult.Fail;
		}

		character.Variables.Temp.SetLong(SprayVar + closest, DateTime.Now.Ticks);

		var sprayed = character.Variables.Perm.GetInt(SprayCountVar, 0) + 1;
		character.Variables.Perm.SetInt(SprayCountVar, sprayed);
		character.ServerMessage(LF("Aconite sprayed: {0}/{1}", Math.Min(sprayed, SpraysNeeded), SpraysNeeded));

		return ItemUseResult.Okay;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50099: Where Did Everybody Go? (1)
//-----------------------------------------------------------------------------
public class Bracken632Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50099);
		SetName(L("Where Did Everybody Go? (1)"));
		SetDescription(L("Like Rose said, the residents of the Croa Village are nowhere to be seen. Search the village and find out what happened."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_ROZE01", "f_bracken_63_2", L("Follow Traveling Merchant Rose to Knidos Jungle"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_ROZE01", "f_bracken_63_2", L("Search in Croa Village to find out what happened"));
		SetPhase(QuestStatus.Success, "BRACKEN632_ROZE01", "f_bracken_63_2", L("Talk to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50093, QuestStatus.Completed));

		AddObjective("checkCenter", L("Look for traces of villagers from the center of the Croa Village"), new ManualObjective());
		AddObjective("checkHouse", L("Look for traces of villagers from the bottom private house of the Croa Village"), new ManualObjective());
		AddObjective("checkWarehouse", L("Look for traces of villagers from the left warehouse of the Croa Village"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 200));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 50100: Where Did Everybody Go? (2)
//-----------------------------------------------------------------------------
public class Bracken632Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50100);
		SetName(L("Where Did Everybody Go? (2)"));
		SetDescription(L("Rose says the beads belong to someone in the village called Anne, and that she would never leave them behind. It could be a lead; follow the trail beads on the ground."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_ROZE01", "f_bracken_63_2", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_TRACES_OBJECT05_TRIGGER", "f_bracken_63_2", L("Follow the beads on the ground"));
		SetPhase(QuestStatus.Success, "BRACKEN632_ROZE02", "f_bracken_63_2", L("Talk to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50099, QuestStatus.Completed));

		AddObjective("followBeads", L("Follow the beads on the ground"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 200));
		AddReward(new SelectItemReward("LEG02_163", "LEG02_164", "LEG02_165"));
	}
}

// 50101: Where Did Everybody Go? (3)
//-----------------------------------------------------------------------------
public class Bracken632Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50101);
		SetName(L("Where Did Everybody Go? (3)"));
		SetDescription(L("Traveling Merchant Rose believes there might be other beads nearby. Look around and search for more beads or other clues."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_ROZE02", "f_bracken_63_2", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_TRACES_SEARCH02", "f_bracken_63_2", L("Look for more beads or other clues"));
		SetPhase(QuestStatus.Success, "BRACKEN632_TOWN_PEAPLE", "f_bracken_63_2", L("Talk to the person who is hiding"));

		AddPrerequisite(new QuestStatusPrerequisite(50100, QuestStatus.Completed));

		AddObjective("searchClues", L("Look for more beads or other clues"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 200));
	}
}

// 50102: Where Did Everybody Go? (4)
//-----------------------------------------------------------------------------
public class Bracken632Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50102);
		SetName(L("Where Did Everybody Go? (4)"));
		SetDescription(L("Tess was being chased by demons. Defeat the demons that chased her."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_TOWN_PEAPLE", "f_bracken_63_2", L("Talk to Tess"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_TOWN_PEAPLE_1", "f_bracken_63_2", L("Defeat the demons"));
		SetPhase(QuestStatus.Success, "BRACKEN632_TOWN_PEAPLE_1", "f_bracken_63_2", L("Return to to Tess and talk to her"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "BRACKEN_63_2_MQ040_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50101, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the demons chasing Tess"), new KillObjective(6, "Sec_bubbe_chaser", "lapasape_mage") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 4));
		AddReward(new ItemReward("Vis", 300));
		AddReward(new SelectItemReward("TOP02_163", "TOP02_164", "TOP02_165"));
	}
}

// 50103: Where Did Everybody Go? (5)
//-----------------------------------------------------------------------------
public class Bracken632Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50103);
		SetName(L("Where Did Everybody Go? (5)"));
		SetDescription(L("Tess seems to be a resident of the Croa Village. Talk to her to find out what happened back at the village."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_TOWN_PEAPLE_1", "f_bracken_63_2", L("Talk to Tess"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_ROZE03", "f_bracken_63_2", L("Talk to Tess"));
		SetPhase(QuestStatus.Success, "BRACKEN632_ROZE03", "f_bracken_63_2", L("Talk to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50102, QuestStatus.Completed));

		AddObjective("hearTess", L("Talk to Tess"), new ManualObjective());
	}
}

// 50145: Where Did Everybody Go? (6)
//-----------------------------------------------------------------------------
public class Bracken632Mq060Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50145);
		SetName(L("Where Did Everybody Go? (6)"));
		SetDescription(L("Rose is back to her senses after hearing your words of comfort. Talk to Rose about what to do next."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_ROZE03", "f_bracken_63_2", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_ROZE03", "f_bracken_63_2", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.Success, "BRACKEN632_ROZE03", "f_bracken_63_2", L("Talk to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50103, QuestStatus.Completed));

		AddObjective("comfortRose", L("Talk to Traveling Merchant Rose"), new ManualObjective());
	}
}

// 50104: Frightened Herbalist
//-----------------------------------------------------------------------------
public class Bracken632Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50104);
		SetName(L("Frightened Herbalist"));
		SetDescription(L("Herbalist Ash wants to go back to Orsha but is scared of all the demons around. Defeat some demons nearby for Herbalist Ash."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Talk to Herbalist Ash"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Defeat nearby demons"));
		SetPhase(QuestStatus.Success, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Talk to Herbalist Ash"));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("killMages", L("Defeat nearby demons"), new KillObjective(10, "lapasape_mage"));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 200));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 50105: Demon Herbalist (1)
//-----------------------------------------------------------------------------
public class Bracken632Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50105);
		SetName(L("Demon Herbalist (1)"));
		SetDescription(L("Herbalist Ash says they saw an unusual sight: a demon collecting herbs. They want to know what those herbs were. Go to Neneva Yard and collect a type of herb with small, thin leaves."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Talk to Herbalist Ash"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Collect the same herbs as the demon"));
		SetPhase(QuestStatus.Success, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Deliver to Herbalist Ash"));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("collectHerbs", L("Collect the same herbs as the demon"), new CollectItemObjective("BRACKEN632_SQ2_ITEM02", 6));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 200));
		AddReward(new TakeItemReward("BRACKEN632_SQ2_ITEM02", -1));
	}
}

// 50106: Demon Herbalist (2)
//-----------------------------------------------------------------------------
public class Bracken632Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50106);
		SetName(L("Demon Herbalist (2)"));
		SetDescription(L("Herbalist Ash wants to dry out all the Aconite to prevent the demons from using it. First, catch some Loktanuns and collect their acidic fluid."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Talk to Herbalist Ash"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Collect Loktanun Fluid"));
		SetPhase(QuestStatus.Success, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Talk to Herbalist Ash"));

		AddPrerequisite(new QuestStatusPrerequisite(50105, QuestStatus.Completed));

		AddObjective("collectFluid", L("Collect Loktanun Fluid"), new CollectItemObjective("BRACKEN632_SQ2_ITEM01", 15));
		AddPityDrop("BRACKEN632_SQ2_ITEM01", 1.0f, 0, 1, "Loktanun");

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 200));
	}
}

// 50107: Demon Herbalist (3)
//-----------------------------------------------------------------------------
public class Bracken632Sq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50107);
		SetName(L("Demon Herbalist (3)"));
		SetDescription(L("Spraying the Loktanun Fluid on the Aconite should dry it out. Go to Neneva Yard and spray the Aconite with the collected Loktanun fluid."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Talk to Herbalist Ash"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Spray the Loktanun Fluid on the Aconite"));
		SetPhase(QuestStatus.Success, "BRACKEN632_PEAPLE01", "f_bracken_63_2", L("Talk to Herbalist Ash"));

		AddPrerequisite(new QuestStatusPrerequisite(50106, QuestStatus.Completed));

		AddObjective("sprayAconite", L("Spray the Loktanun Fluid on the Aconite"), new VariableCheckObjective(FBracken632QuestNpcsScript.SprayCountVar, 12, isPermanent: true));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 200));
		AddReward(new TakeItemReward("BRACKEN632_SQ2_ITEM01", -1));
	}
}

// 60162: Fallen Days
//-----------------------------------------------------------------------------
public class Bracken632Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60162);
		SetName(L("Fallen Days"));
		SetDescription(L("Collect Lapasape wand fragments to show that you have dealed with Lapasape Mages in order to calm Tess."));
		SetType(QuestType.Repeat);
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_TOWN_PEAPLE_1", "f_bracken_63_2", L("Talk to Tess"));
		SetPhase(QuestStatus.InProgress, "BRACKEN632_TOWN_PEAPLE_1", "f_bracken_63_2", L("Collect Lapasape Wand Fragments"));
		SetPhase(QuestStatus.Success, "BRACKEN632_TOWN_PEAPLE_1", "f_bracken_63_2", L("Take the Fragments to Tess"));

		AddPrerequisite(new QuestStatusPrerequisite(50145, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("collectFragments", L("Obtain Lapasape Wand Fragment"), new CollectItemObjective("BRACKEN632_RP_1_ITEM", 7));
		AddPityDrop("BRACKEN632_RP_1_ITEM", 0.9f, 3, 1, "lapasape_mage");

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("BRACKEN632_RP_1_ITEM", -1));
	}
}

// 50276: Remembering the Victims (1)
//-----------------------------------------------------------------------------
public class Abbey642Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50276);
		SetName(L("Remembering the Victims (1)"));
		SetDescription(L("Rona wants to erect a tombstone in the Novaha Monastery as a memorial to the victims of the demons' experiments. Unfortunately, she can't go there because of the monsters. Go to the Novaha Monastery and set up the memorial for her."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_2", "d_abbey_64_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN632_TOWN_PEAPLE2", "f_bracken_63_2", L("Talk to Rona"));
		SetPhase(QuestStatus.InProgress, "ABBEY64_2_HIDDENQ2_OBJ2", "d_abbey_64_1", L("Set a Tombstone at the Novaha Assembly Hall"));
		SetPhase(QuestStatus.Success, "ABBEY64_2_HIDDENQ2_OBJ2", "d_abbey_64_1", L("Set a Tombstone at the Novaha Assembly Hall"));

		AddPrerequisite(new QuestStatusPrerequisite(50123, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(50124, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(50131, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(50133, QuestStatus.Completed));

		AddObjective("findStone", L("Find a Stone to Use as Memorial"), new CollectItemObjective("ABBAY642_HIDDENQ1_ITEM", 1));

		AddReward(new TakeItemReward("ABBAY642_HIDDENQ1_ITEM", 1));
	}
}
