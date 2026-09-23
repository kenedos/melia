//--- Melia Script ----------------------------------------------------------
// Kalejimas Solitary Cells Quest NPCs
//--- Description -----------------------------------------------------------
// Zanas' plan against Grinender, the King's Green Jewel, and the third
// demon barrier.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison80QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Prison79Mq10 = new QuestId(30163);
	private readonly static QuestId Mq1 = new QuestId(30164);
	private readonly static QuestId Mq2 = new QuestId(30165);
	private readonly static QuestId Mq3 = new QuestId(30166);
	private readonly static QuestId Mq4 = new QuestId(30167);
	private readonly static QuestId Mq5 = new QuestId(30168);
	private readonly static QuestId Mq6 = new QuestId(30169);
	private readonly static QuestId Mq7 = new QuestId(30170);
	private readonly static QuestId Mq8 = new QuestId(30171);
	private readonly static QuestId Mq9 = new QuestId(30172);
	private readonly static QuestId Mq10 = new QuestId(30173);
	private readonly static QuestId Sq1 = new QuestId(30199);

	private const string ZanasPortrait = "Dlg_port_zanas_prison";
	private const string HeardVar = "Gabija.Prison80.Heard";
	private const string CrystalVar = "Gabija.Prison80.Crystal";
	private const string CrystalTimeVar = "Gabija.Prison80.CrystalTime";

	private const int DivineNameRounds = 3;

	private readonly static TimeSpan CrystalWindow = TimeSpan.FromSeconds(40);
	private readonly static Position LongSentenceCell = new Position(1242, 148, 71);

	// The Red Socket Mages the illusion lets you walk among.
	private readonly static double[,] InformantSpots =
	{
		{ 118.18, -72.17 }, { 243.51, -383.35 }, { 117.95, -422.37 }, { 218.44, -114.00 }, { 352.49, -326.37 },
	};

	private readonly static double[,] CrystalSpots =
	{
		{ -1128.76, 329.77 }, { -1108.06, 234.47 }, { -1012.82, 204.89 },
		{ -935.73, 274.71 }, { -958.31, 371.40 }, { -1051.38, 400.62 },
	};

	protected override void Load()
	{
		// Zanas' Soul, at the Solitary Cells entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_80_NPC_1", "d_prison_80", 959.28, -906.89, 0, this.IsZanasAtTheEntrance, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (character.Quests.IsActive(Mq2) && character.Quests.IsCompletable(Mq2))
			{
				await dialog.Msg(L("Have you found out something?"));
				await dialog.CompleteQuest(Mq2);
				return;
			}

			if (character.Quests.IsActive(Mq3) && character.Quests.IsCompletable(Mq3))
			{
				await dialog.Msg(L("Did you get Grinender's Seal?"));
				await dialog.Msg(L("Good. I also had figured out a plan to get back at those demons."));
				await dialog.Msg(L("If it works, we might just easily get through the solitary confinement section."));
				await dialog.CompleteQuest(Mq3);
				return;
			}

			if (character.Quests.IsActive(Mq4) && character.Quests.IsCompletable(Mq4))
			{
				await dialog.Msg(L("Now that we got the Teleport Magic Scroll and jump ahead to the Long Sentence Prison Cell."));
				await dialog.CompleteQuest(Mq4);
				return;
			}

			if (!character.Quests.Has(Mq1) && character.Quests.MeetsPrerequisites(Mq1))
			{
				await dialog.Msg(L("We have disable two barriers now."));
				await dialog.Msg(L("I bet the demons have noticed something by now."));
				await dialog.Msg(L("We need to interrogate the demons."));
				await dialog.Msg(L("It is quite crucial to know how much they know."));
				await dialog.Msg(L("In the hanging room, there is a secret device that seems useful."));

				var answer = await dialog.SelectQuestOffer(Mq1, L("There is a magic device that gives out hallucination. Disguise yourself as a monster and let's get the info from them."),
					Option(L("Say that you think it is a good idea"), "accept"),
					Option(L("Say that they will never fall for that"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1);
					await dialog.Msg(L("The hallucination device in the hanging room uses spell or souls to create its shape."));
					await dialog.Msg(L("So that means to create a hallucination of a demon,"));
					await dialog.Msg(L("You have to defeat a demon that you want to make a hallucination out of near the hallucination device."));
				}
				return;
			}

			if (!character.Quests.Has(Mq3) && character.Quests.MeetsPrerequisites(Mq3))
			{
				await dialog.Msg(L("They don't suspect that the revelator is here."));
				await dialog.Msg(L("If anything, they think I am the intruder. Well, no one is stupid or brave enough to sneak in."));
				await dialog.Msg(L("But Grinender is here... now that is no good news."));
				await dialog.Msg(L("We ought to find Grinender's Seal that the demons are talking about."));

				var answer = await dialog.SelectQuestOffer(Mq3, L("It seems that that is what can disable the magic circle created by Grinender."),
					Option(L("Say that you will obtain Grinende's Seal"), "accept"),
					Option(L("Say that you will force them to hand it over"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq3);
					await dialog.Msg(L("While you get Grinender's Seal, I will be planning on how to hit Grinender where it hurts."));
					await dialog.Msg(L("There must be something."));
				}
				return;
			}

			if (!character.Quests.Has(Mq4) && character.Quests.MeetsPrerequisites(Mq4))
			{
				await dialog.Msg(L("Here's the plan, get the teleport scroll and get straight to the Long Sentence Prison Cells."));
				await dialog.Msg(L("And activate the defense device and exterminate them all."));
				await dialog.Msg(L("The demons in the mid hallway would not know what hit them."));
				await dialog.Msg(L("And we walk away like cool guys do."));
				await dialog.Msg(L("The first thing we should do is to get ahold of that scroll."));

				var answer = await dialog.SelectQuestOffer(Mq4, L("The Teleport Magic Scroll is inside the secret device located in the Prisoner Waiting Room."),
					Option(L("Say that you want to deal a blow to the Demons"), "accept"),
					Option(L("Say that the Demons can't be that stupid"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq4);
					await dialog.Msg(L("You and I make a great team."));
					await dialog.Msg(L("You need a password to open the secret device."));
					await dialog.Msg(L("I believe it was a name of a goddess."));
					await dialog.Msg(L("Gabija, Ausrine, Vakarine, Zemyna, Vaivora, Jurate... And you know a lot more, right?"));
				}
				return;
			}

			if (!character.Quests.Has(Mq5) && character.Quests.MeetsPrerequisites(Mq5))
			{
				await dialog.Msg(L("I will hold onto one of these Teleport Magic Scrolls, just in case."));
				await dialog.Msg(L("The other one you should use it to get to the Long Sentence Prison Cells."));
				await dialog.Msg(L("When you get to the Long Sentence Prison Cells, turn on the defense device right away."));

				var answer = await dialog.SelectQuestOffer(Mq5, L("The instruction is on the manual."),
					Option(L("Say that you will go to the Long Sentence Prison Cell"), "accept"),
					Option(L("Say that you have more to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq5);
					character.Inventory.Add(ItemId.PRISON_80_MQ_4_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("I cannot go because of that Grinender. Once it falls, I will join you as soon as possible."));
					await dialog.Msg(L("Godspeed."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq1) || character.Quests.IsActive(Mq2))
			{
				await dialog.Msg(L("I think the reason why King Kadumel made such device,"));
				await dialog.Msg(L("is to perhaps fool the enemies and escape himself."));
				await dialog.Msg(L("However, I still do have some questions about how to use the device."));
				await dialog.Msg(L("I feel like I am missing something here."));
				return;
			}

			if (character.Quests.IsActive(Mq3))
			{
				await dialog.Msg(L("Some planning is needed for secret devices in the solitary confinement section."));
				await dialog.Msg(L("If I am remembering correctly, there were things like teleport scrolls or a magic device..."));
				return;
			}

			if (character.Quests.IsActive(Mq4))
			{
				await dialog.Msg(L("Vaivora, Vakarine, Ausrine, Gabija, Jurate, Zemyna..."));
				await dialog.Msg(L("It might be enough."));
				return;
			}

			if (character.Quests.IsActive(Mq5) || character.Quests.IsActive(Mq6))
			{
				await dialog.Msg(L("The defense device in the long sentence prison cells is intricate with some powerful magic."));
				await dialog.Msg(L("I had some explaining to do to the wardens the first time I activate the machine and caused a havoc."));
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("We need to be really careful from now on."));
		});

		// The Hanging Room's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151111, L("Secret Device"), "PRISON_80_OBJ_1", "d_prison_80", -68.45, -889.71, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq1) && character.Quests.IsCompletable(Mq1))
			{
				await dialog.Msg(L("The device has taken in the Red Socket Mages' magic. It is ready to cast their shape over you."));
				await dialog.CompleteQuest(Mq1);

				if (!character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2))
					await this.PutOnTheIllusion(character);

				return;
			}

			if (!character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2))
			{
				await this.PutOnTheIllusion(character);
				return;
			}

			if (character.Quests.IsActive(Mq1))
			{
				await dialog.Msg(L("The device has nothing to make an illusion from. Defeat Red Socket Mages near it."));
				return;
			}

			await dialog.Msg(L("A secret device that casts illusions out of the magic it has taken in."));
		});

		// The Red Socket Mages in the Solitary Cell
		//-------------------------------------------------------------------------
		for (var i = 0; i < InformantSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(57930, L("Red Socket Mage"), "PRISON_80_MON_1_" + number, "d_prison_80", InformantSpots[i, 0], InformantSpots[i, 1], 90, this.IsTheIllusionOn, dialog => this.ListenToTheMage(dialog, number));
		}

		// The Prisoner Waiting Room's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_80_OBJ_2", "d_prison_80", 550.79, -719.63, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (!character.Quests.IsActive(Mq4) || character.Quests.IsCompletable(Mq4))
			{
				await dialog.Msg(L("A secret device waiting on a name."));
				return;
			}

			if (!await this.SayTheDivineNames(dialog))
				return;

			var opened = await character.TimeActions.StartAsync(L("Opening the secret device"), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(2));

			if (opened != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.PRISON_80_MQ_4_ITEM, 2, InventoryAddType.PickUp);
			character.ServerMessage(L("The secret device has been disarmed"));
		});

		// The Long Sentence Prison Cell's Defensive Device
		//-------------------------------------------------------------------------
		AddNpc(151111, L("Secret Device"), "PRISON_80_OBJ_3", "d_prison_80", 1220.48, 189.46, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (!character.Quests.Has(Mq6) && character.Quests.MeetsPrerequisites(Mq6))
			{
				character.Quests.Start(Mq6);
				character.ServerMessage(L("The defense devices have started to become active."));
				return;
			}

			if (character.Quests.IsActive(Mq6))
			{
				await dialog.Msg(L("The defense devices are already active."));
				return;
			}

			await dialog.Msg(L("The Long Sentence Prison Cell's defensive device, quiet for now."));
		});

		// Grinender's Magic Circle
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Grinender's Magic Circle"), "PRISON_80_OBJ_4", "d_prison_80", -125, 524, 90, this.IsTheCircleStanding, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grinender's Magic Circle"));

			if (!character.Quests.Has(Mq7) && character.Quests.MeetsPrerequisites(Mq7))
			{
				if (character.Inventory.CountItem(ItemId.PRISON_80_MQ_3_ITEM) == 0)
				{
					await dialog.Msg(L("Only those with Grinender's Seal can get past the magic circle."));
					return;
				}

				var used = await character.TimeActions.StartAsync(L("Using Grinender's Seal..."), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (used != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.PRISON_80_MQ_3_ITEM, character.Inventory.CountItem(ItemId.PRISON_80_MQ_3_ITEM));
				character.Quests.Start(Mq7);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("Grinender's magic circle, sealing off the Common Room."));
		});

		// Zanas' Soul, at the Common Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_80_NPC_2", "d_prison_80", -234.10, 438.98, 45, this.IsZanasAtTheCommonRoom, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (character.Quests.IsActive(Mq7) && character.Quests.IsCompletable(Mq7))
			{
				await dialog.Msg(L("I came here as fast as I could after seeing you defeating Grinender."));
				await dialog.Msg(L("It was no easy task, I imagine but well done."));
				await dialog.CompleteQuest(Mq7);
				return;
			}

			if (character.Quests.IsActive(Mq8) && character.Quests.IsCompletable(Mq8))
			{
				await dialog.Msg(L("Remarkable."));
				await dialog.Msg(L("Id I had your strength and skills, I would not have taken such a drastic choice.."));
				await dialog.Msg(L("There is no point crying over spilt milk..."));
				await dialog.Msg(L("The present is all that matters, right?"));
				await dialog.CompleteQuest(Mq8);
				return;
			}

			if (!character.Quests.Has(Mq8) && character.Quests.MeetsPrerequisites(Mq8))
			{
				var answer = await dialog.SelectQuestOffer(Mq8, L("With Grinender no more, the remnants don't stand a chance. Let's wipe them off so that they won't hinder our quest to find the King's gem."),
					Option(L("Say that the you are starting to deal with the stragglers"), "accept"),
					Option(L("State that you are tired and need to rest"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq8);
					await dialog.Msg(L("Don't let your guard down though."));
					await dialog.Msg(L("The demons can charge in at any time."));
				}
				return;
			}

			if (!character.Quests.Has(Mq9) && character.Quests.MeetsPrerequisites(Mq9))
			{
				await dialog.Msg(L("Now let's disable the demon barrier in the Regular Prison Cell."));
				await dialog.Msg(L("The King's Green Jewel is inside the the secret device inside the Regular Prison Cells."));

				var answer = await dialog.SelectQuestOffer(Mq9, L("Find it that and disable the demon barrier in the Reintegration Workshop."),
					Option(L("Say that you should leave at once"), "accept"),
					Option(L("Say that you need to rest now"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq9);
					await dialog.Msg(L("There are six crystals surrounding the secret device."));
					await dialog.Msg(L("Breathe spell into them and the device will unlock itself."));
					await dialog.Msg(L("However, the crystals lose their potency after a while."));
					await dialog.Msg(L("So, please be hasty."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq7))
			{
				await dialog.Msg(L("Upon disabling the demon magic circle, Grinender appeared. Defeat Grinender."));
				character.Quests.ReplayQuestTrack(Mq7);
				return;
			}

			if (character.Quests.IsActive(Mq8))
			{
				await dialog.Msg(L("I hope Nebulas also would go down this easily."));
				await dialog.Msg(L("But I do have a bad feeling about this."));
				await dialog.Msg(L("I hope this is just a feeling. Nothing more than that..."));
				return;
			}

			if (character.Quests.IsActive(Mq9))
			{
				await dialog.Msg(L("King Kadumel was never known to show any special talents in using magic."));
				await dialog.Msg(L("Probably, he had his mages activate such devices."));
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("The demons gotta be out of their minds by now."));
			await dialog.Msg(L("We should move."));
		});

		// The Regular Prison Cell's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_80_OBJ_5", "d_prison_80", -1034.47, 300.83, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq9) && character.Quests.IsCompletable(Mq9))
			{
				await dialog.Msg(L("With all six crystals shining, the device unlocks. The King's Green Jewel lies inside."));
				await dialog.CompleteQuest(Mq9);
				return;
			}

			if (character.Quests.IsActive(Mq9))
			{
				await dialog.Msg(L("Infuse magic into all six crystals around the device while they are still shining."));
				return;
			}

			await dialog.Msg(L("A secret device ringed with six crystals."));
		});

		// The crystals around the Regular Prison Cell's device
		//-------------------------------------------------------------------------
		for (var i = 0; i < CrystalSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(46215, L("Crystal"), "PRISON_80_OBJ_6_" + number, "d_prison_80", CrystalSpots[i, 0], CrystalSpots[i, 1], 90, dialog => this.InfuseTheCrystal(dialog, number));
		}

		// Writings on the Wall
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Writings on the Wall"), "PRISON_80_SQ_OBJ_1", "d_prison_80", 962.35, 517.72, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Writings on the Wall"));

			if (!character.Quests.Has(Sq1) && character.Quests.MeetsPrerequisites(Sq1))
			{
				var read = await character.TimeActions.StartAsync(L("Reading the Writing on the Walls"), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

				if (read != TimeActionResult.Completed)
					return;

				await this.ReadTheComplaint(dialog);
				character.Quests.Start(Sq1);
				return;
			}

			await this.ReadTheComplaint(dialog);
		});

		// The Confiscated Goods Box
		//-------------------------------------------------------------------------
		AddNpc(151030, L("Chest of Confiscated Goods"), "PRISON_80_SQ_OBJ_2", "d_prison_80", -666.39, 65.26, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chest of Confiscated Goods"));

			if (!character.Quests.IsActive(Sq1))
			{
				await dialog.Msg(L("A box of goods the guards took off the prisoners."));
				return;
			}

			var searched = await character.TimeActions.StartAsync(L("Examining the contraband box"), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

			if (searched != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("At the bottom of the box is a care package, the brother's name still written on it. The guards never did pass it on."));
			character.Quests.CompleteObjective(Sq1, "findTheGoods");
			await dialog.CompleteQuest(Sq1);
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("PRISON_80_MQ_10_TRIGGER", "d_prison_80", -863.26, -330.14, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq10) && character.Quests.MeetsPrerequisites(Mq10))
			{
				character.Quests.Start(Mq10);
				character.Quests.CompleteObjective(Mq10, "releaseTheBarrier");
			}
			else if (character.Quests.IsActive(Mq10) && character.Quests.IsCompletable(Mq10))
			{
				character.Quests.ReplayQuestTrack(Mq10);
			}

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Carries the player to the Long Sentence Prison Cell.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_PRISON_80_MQ_4_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "d_prison_80" || !character.Quests.IsActive(Mq5) || character.Quests.IsCompletable(Mq5))
		{
			character.ServerMessage(L("The scroll's magic does not answer here."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.Warp("d_prison_80", LongSentenceCell);
		character.Quests.CompleteObjective(Mq5, "teleport");

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Runs the Prisoner Waiting Room device's lock, which asks for a
	/// goddess' name among false ones several times over.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task<bool> SayTheDivineNames(Dialog dialog)
	{
		var goddesses = new[] { L("Vaivora"), L("Vakarine"), L("Ausrine"), L("Gabija"), L("Jurate"), L("Zemyna"), L("Laima") };
		var impostors = new[] { L("Inea"), L("Tilliana"), L("Aisa"), L("Natasha"), L("Irma"), L("Cordelia"), L("Gelija"), L("Onute"), L("Reina"), L("Ilanai") };

		for (var round = DivineNameRounds; round > 0; --round)
		{
			var names = impostors.OrderBy(_ => System.Random.Shared.Next()).Take(3).ToList();
			var answer = System.Random.Shared.Next(names.Count + 1);
			names.Insert(answer, goddesses[System.Random.Shared.Next(goddesses.Length)]);

			var options = names.Select((name, i) => Option(name, i.ToString())).ToList();
			var picked = await dialog.Select(LF("Say the name of the goddess. Say the name {0} times.", round), options);

			if (picked != answer.ToString())
			{
				await dialog.Msg(L("Those that do not know the divine name, are not worthy. You shall go back to the beginning."));
				return false;
			}
		}

		await dialog.Msg(L("You have chosen wisely."));
		return true;
	}

	/// <summary>
	/// Casts the Red Socket Mage illusion over the player and starts
	/// the eavesdropping.
	/// </summary>
	/// <param name="character"></param>
	private async Task PutOnTheIllusion(Character character)
	{
		var cast = await character.TimeActions.StartAsync(L("Activating Device"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

		if (cast != TimeActionResult.Completed)
			return;

		for (var i = 1; i <= InformantSpots.GetLength(0); ++i)
			character.Variables.Temp.Remove(HeardVar + i);

		character.Quests.Start(Mq2);
		character.LookAround();
		character.ServerMessage(L("Listen to the Red Socket Mage in the Solitary Punishment Room for information"));
	}

	/// <summary>
	/// Listens to one of the Red Socket Mages under the illusion.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="number"></param>
	private async Task ListenToTheMage(Dialog dialog, int number)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Red Socket Mage"));

		if (character.Variables.Temp.GetBool(HeardVar + number, false))
		{
			character.ServerMessage(L("You have already obtained information on this monster"));
			return;
		}

		switch (number)
		{
			case 1:
				await dialog.Msg(L("You heard about the intruder, right?"));
				await dialog.Msg(L("I'm thinking it's Zanas."));
				await dialog.Msg(L("This is someone who took his own life to become a spirit and escape."));
				await dialog.Msg(L("He's gonna be in trouble when we find him."));
				break;
			case 2:
				await dialog.Msg(L("Didn't Grinender catch Zanas' spirit?"));
				await dialog.Msg(L("They should be at the Common Room... Did you go an salute?"));
				break;
			case 3:
				await dialog.Msg(L("It's an order to capture the intruder."));
				await dialog.Msg(L("If it's human we kill them, if it's Zanas we take him."));
				break;
			case 4:
				await dialog.Msg(L("Grinender created a powerful magic circle."));
				await dialog.Msg(L("No intruder is gonna enter the Common Room."));
				break;
			default:
				await dialog.Msg(L("Only those with the Grinender Seal can get past the magic circle."));
				await dialog.Msg(L("You didn't lose yours, did you?"));
				break;
		}

		character.Variables.Temp.SetBool(HeardVar + number, true);

		var heard = 0;
		for (var i = 1; i <= InformantSpots.GetLength(0); ++i)
		{
			if (character.Variables.Temp.GetBool(HeardVar + i, false))
				heard++;
		}

		if (heard < InformantSpots.GetLength(0))
		{
			character.ServerMessage(LF("Red Socket Mages listened to: {0}/{1}", heard, InformantSpots.GetLength(0)));
			return;
		}

		character.Quests.CompleteObjective(Mq2, "listenToTheMages");
		character.LookAround();
		character.ServerMessage(L("You have heard enough from the demons. Return to Zanas' Soul."));
	}

	/// <summary>
	/// Infuses magic into one of the crystals around the Regular Prison
	/// Cell's device, which only counts while the others still shine.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="number"></param>
	private async Task InfuseTheCrystal(Dialog dialog, int number)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Crystal"));

		if (!character.Quests.IsActive(Mq9) || character.Quests.IsCompletable(Mq9))
		{
			await dialog.Msg(L("A crystal, dull without magic in it."));
			return;
		}

		var infused = await character.TimeActions.StartAsync(L("Infusing magic"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(1));

		if (infused != TimeActionResult.Completed)
			return;

		var now = DateTime.Now.Ticks;
		var first = character.Variables.Temp.GetLong(CrystalTimeVar, 0);

		if (first == 0 || now - first > CrystalWindow.Ticks)
		{
			for (var i = 1; i <= CrystalSpots.GetLength(0); ++i)
				character.Variables.Temp.Remove(CrystalVar + i);

			character.Variables.Temp.SetLong(CrystalTimeVar, now);

			if (first != 0)
				character.ServerMessage(L("Your time is up. Start from the beginning."));
		}

		character.Variables.Temp.SetBool(CrystalVar + number, true);

		var shining = 0;
		for (var i = 1; i <= CrystalSpots.GetLength(0); ++i)
		{
			if (character.Variables.Temp.GetBool(CrystalVar + i, false))
				shining++;
		}

		if (shining < CrystalSpots.GetLength(0))
		{
			character.ServerMessage(LF("Crystals shining: {0}/{1}", shining, CrystalSpots.GetLength(0)));
			return;
		}

		character.Variables.Temp.Remove(CrystalTimeVar);
		character.Quests.CompleteObjective(Mq9, "infuseTheCrystals");
		character.ServerMessage(L("All six crystals shine at once. The secret device unlocks."));
	}

	/// <summary>
	/// Reads the prisoner's complaint off the Long Sentence Prison Cell wall.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task ReadTheComplaint(Dialog dialog)
	{
		await dialog.Msg(L("I'm so mad."));
		await dialog.Msg(L("So what, I stole some bread. Why put me in this place and make me miserable..."));
		await dialog.Msg(L("Even the care package my brother sent me was \"confiscated\"."));
		await dialog.Msg(L("Right. I know you no-life guards took it all for yourselves."));
		await dialog.Msg(L("I'm just saying, the real thief here isn't me."));
		await dialog.Msg(L("Wish the guards would just burn this prison cell already."));
		await dialog.Msg(L("Please! Oh, Goddess Gabija!"));
	}

	/// <summary>
	/// Returns whether Zanas' Soul waits at the Solitary Cells entrance.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasAtTheEntrance(Character character)
		=> character.Quests.HasCompleted(Prison79Mq10) && !character.Quests.Has(Mq7);

	/// <summary>
	/// Returns whether Grinender's magic circle still seals the Common Room.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTheCircleStanding(Character character)
		=> character.Quests.HasCompleted(Prison79Mq10) && !character.Quests.Has(Mq7);

	/// <summary>
	/// Returns whether Zanas' Soul has made it to the Common Room.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasAtTheCommonRoom(Character character)
		=> character.Quests.Has(Mq7) && !character.Quests.HasCompleted(Mq10);

	/// <summary>
	/// Returns whether the illusion lets the player talk with the demons.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTheIllusionOn(Character character)
		=> character.Quests.IsActive(Mq2) && !character.Quests.IsCompletable(Mq2);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 30164: Who am I(1)
//-----------------------------------------------------------------------------
public class Prison80Mq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30164);
		SetName(L("Who am I(1)"));
		SetDescription(L("The Hanging Room's device makes its illusions out of the magic it takes in."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_NPC_1", "d_prison_80", L("Talk to Zanas' Spirit at the Solitary Cells"), L("Zanas' Spirit wishes to go to the Solitary Cells. Go and talk to him there."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_OBJ_1", "d_prison_80", L("Defeat the Demons near the Hanging Room's Secret Device"), L("In order to use the Illusion Magic with the Hanging Room's Secret Device, you must register the magic of the Demon to be used in the illusion. Defeat Red Socket Mages near the Device."));
		SetPhase(QuestStatus.Success, "PRISON_80_OBJ_1", "d_prison_80", L("Activate the Secret Device in the Hanging Room"), L("The Hanging Room's Secret Device has registered the Red Socket Mage's magic. Activate the Secret Device."));

		AddPrerequisite(new QuestStatusPrerequisite(30163, QuestStatus.Completed));

		AddObjective("killMages", L("Defeat Red Socket Mages near the Hanging Room's Secret Device"), new KillObjective(10, "Socket_mage_red"));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7440));
	}
}

// 30165: Who am I(2)
//-----------------------------------------------------------------------------
public class Prison80Mq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30165);
		SetName(L("Who am I(2)"));
		SetDescription(L("Wearing a Red Socket Mage's shape, you hear what the demons know."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_OBJ_1", "d_prison_80", L("Activate the Secret Device in the Hanging Room"), L("The Hanging Room's Secret Device has registered the Red Socket Mage's magic. Activate the Secret Device."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_MON_1_1", "d_prison_80", L("Listen to information from the Red Socket Mage in the Solitary Cell"), L("You will appear as a colleague to the other Demons thanks to the help of illusion magic. Listen to the Red Socket Mage for information in the Solitary Cell."));
		SetPhase(QuestStatus.Success, "PRISON_80_NPC_1", "d_prison_80", L("Talk to Zanas' Soul"), L("You have gained much information from the Demons. Return to Zanas' Spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(30164, QuestStatus.Completed));

		AddObjective("listenToTheMages", L("Listen to information from the Red Socket Mage in the Solitary Cell"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 7440));
	}
}

// 30166: Demon identification
//-----------------------------------------------------------------------------
public class Prison80Mq3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30166);
		SetName(L("Demon identification"));
		SetDescription(L("Only Grinender's Seal gets past the magic circle in the Common Room."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_NPC_1", "d_prison_80", L("Talk to Zanas' Soul"), L("Tell Zanas' Spirit about the information you have gained from the Demons."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_NPC_1", "d_prison_80", L("Obtain Grinende Seal's by defeating the Demons"), L("It seems as if you require Grinende Seal's to disarm the Demon Magic Circles. Defeat the Demons to obtain Grinende Seals."));
		SetPhase(QuestStatus.Success, "PRISON_80_NPC_1", "d_prison_80", L("Talk to Zanas' Soul"), L("You have obtained Grinende Seals. Return to Zanas' Spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(30165, QuestStatus.Completed));

		AddObjective("collectSeal", L("Obtain Grinender Seal by defeating Demons"), new CollectItemObjective("PRISON_80_MQ_3_ITEM", 1));

		AddPityDrop("PRISON_80_MQ_3_ITEM", 0.05f, 10, 1, "defender_spider_blue", "Socket_mage_red", "NightMaiden_bow_red");

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7440));
	}
}

// 30167: Prison Movement(1)
//-----------------------------------------------------------------------------
public class Prison80Mq4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30167);
		SetName(L("Prison Movement(1)"));
		SetDescription(L("The Teleport Magic Scrolls are locked behind the name of a goddess."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_NPC_1", "d_prison_80", L("Talk to Zanas' Soul"), L("Talk to Zanas' Spirit about what to do next now that you have obtained the Grinende Seals."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_OBJ_2", "d_prison_80", L("Obtain Kalejimas Teleportation Scrolls from the Prisoner Waiting Room's Secret Device"), L("It is said the scrolls that allow you to teleport within the Kalejimas Prison are in the Prisoner Waiting Room's Secret Device. Disarm the Secret Device to obtain the Kalejimas Teleportation Scrolls."));
		SetPhase(QuestStatus.Success, "PRISON_80_NPC_1", "d_prison_80", L("Talk to Zanas' Soul"), L("You have disarmed the Secret Device and gained the Kalejimas Teleportation Scrolls. Return to Zanas' Spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(30166, QuestStatus.Completed));

		AddObjective("collectScrolls", L("Obtain Kalejimas Teleport Magic Scrolls from the Secret Device in the Prisoner Waiting Room"), new CollectItemObjective("PRISON_80_MQ_4_ITEM", 2));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7440));
		AddReward(new TakeItemReward("PRISON_80_MQ_4_ITEM"));
	}
}

// 30168: Prison Movement(2)
//-----------------------------------------------------------------------------
public class Prison80Mq5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30168);
		SetName(L("Prison Movement(2)"));
		SetDescription(L("One Teleport Magic Scroll for the Long Sentence Prison Cell, one kept by Zanas."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_NPC_1", "d_prison_80", L("Talk to Zanas' Soul"), L("Things seem to be going well. Talk to Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_OBJ_3", "d_prison_80", L("Use the Kalejimas Teleportation Scroll"), L("Move to the Long Sentence Prison Cell by using the Kalejimas Teleportation Scrolls."));
		SetPhase(QuestStatus.Success, "PRISON_80_OBJ_3", "d_prison_80", L("Use the Kalejimas Teleportation Scroll"), L("Move to the Long Sentence Prison Cell by using the Kalejimas Teleportation Scrolls."));

		AddPrerequisite(new QuestStatusPrerequisite(30167, QuestStatus.Completed));

		AddObjective("teleport", L("Use the Kalejimas Teleportation Scroll"), new ManualObjective());

		AddReward(new ItemReward("Vis", 7440));
		AddReward(new TakeItemReward("PRISON_80_MQ_4_ITEM"));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("You have been teleported to the Long-term Confinement Area"));
	}
}

// 30169: The perfect massacre
//-----------------------------------------------------------------------------
public class Prison80Mq6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30169);
		SetName(L("The perfect massacre"));
		SetDescription(L("The Long Sentence Prison Cell's defensive device cuts down whatever comes close."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_OBJ_3", "d_prison_80", L("Activate the Secret Device in the Long Sentence Prison Cell"), L("You have safely arrived at the Long Sentence Prison Cell by using Kalejimas Teleportation Scrolls. Activate the Long Sentence Prison Cell's Secret Device."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_OBJ_3", "d_prison_80", L("Lure the monsters near the Secret Device"), L("Long Sentence Prison Cell Defensive Device : It attacks approaching enemies with a strong magical attack once activated. It will stop after a time but it can be reused without any special conditions."));
		SetPhase(QuestStatus.Success, "PRISON_80_OBJ_3", "d_prison_80", L("Lure the monsters near the Secret Device"), L("Long Sentence Prison Cell Defensive Device : It attacks approaching enemies with a strong magical attack once activated. It will stop after a time but it can be reused without any special conditions."));

		AddPrerequisite(new QuestStatusPrerequisite(30168, QuestStatus.Completed));

		AddObjective("massacre", L("Lure the monsters near the Secret Device and defeat them"), new KillObjective(10, "defender_spider_blue", "Socket_mage_red", "NightMaiden_bow_red"));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7440));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("The demons of the Solitary Cells are in disarray. Disarm Grinender's magic circle in the Common Room."));
	}
}

// 30170: Through the front door
//-----------------------------------------------------------------------------
public class Prison80Mq7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30170);
		SetName(L("Through the front door"));
		SetDescription(L("Grinender's Seal opens his own magic circle, and Grinender comes through it."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_OBJ_4", "d_prison_80", L("Disarm Grinende's Magic Circle"), L("Disarm Grinende's Magic Circle in the Common Room by using Grinende Seals."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_NPC_2", "d_prison_80", L("Defeat Grinende"), L("Grinende appears as soon as you disarm his Magic Circle. Defeat Grinende."));
		SetPhase(QuestStatus.Success, "PRISON_80_NPC_2", "d_prison_80", L("Talk to Zanas' Soul"), L("Go talk to Zanas' Spirit since you have defeated Grinende."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON_80_MQ_7_TRACK", "m_boss_b", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(30169, QuestStatus.Completed));

		AddObjective("killGrinender", L("Defeat Grinender"), new KillObjective(1, "boss_Grinender_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 7440));
	}
}

// 30171: Deal with the stragglers in the Solitary Cells
//-----------------------------------------------------------------------------
public class Prison80Mq8Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30171);
		SetName(L("Deal with the stragglers in the Solitary Cells"));
		SetDescription(L("With Grinender gone, only his stragglers are left in the Solitary Cells."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_NPC_2", "d_prison_80", L("Talk to Zanas' Soul"), L("Zanas' Spirit seems to be able to move more easily now that Grinende has been defeated. Talk to Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_NPC_2", "d_prison_80", L("Defeat the Demon stragglers in the Solitary Cells"), L("All that is left now are stragglers since you've defeated Grinende. Defeat the Demons in the Solitary Cells so they won't interrupt you while trying to obtain the King's Jewels."));
		SetPhase(QuestStatus.Success, "PRISON_80_NPC_2", "d_prison_80", L("Talk to Zanas' Soul"), L("Now there aren't too many stragglers. Return to Zanas."));

		AddPrerequisite(new QuestStatusPrerequisite(30170, QuestStatus.Completed));

		AddObjective("killStragglers", L("Defeat the Demon stragglers in the Solitary Cells"), new KillObjective(20, "defender_spider_blue", "Socket_mage_red", "NightMaiden_bow_red"));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7440));
	}
}

// 30172: Six Crystals
//-----------------------------------------------------------------------------
public class Prison80Mq9Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30172);
		SetName(L("Six Crystals"));
		SetDescription(L("The King's Green Jewel is locked behind six crystals that must shine at once."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_NPC_2", "d_prison_80", L("Talk to Zanas' Soul"), L("It is now time to obtain the second King's Jewel. Go and ask Zanas about the King's Jewel."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_OBJ_5", "d_prison_80", L("Disarm the Regular Prison Cell's Secret Device"), L("Regular Prison Cell's Secret Device : Enfuse magic into all six Crystals. You must enfuse magic into all of the Crystals while they are still shining. This will disarm the Device."));
		SetPhase(QuestStatus.Success, "PRISON_80_OBJ_5", "d_prison_80", L("Obtain the King's Green Jewel from the Secret Device"), L("You have disarmed the Regular Prison Cell's Secret Device. Take the King's Green Jewel."));

		AddPrerequisite(new QuestStatusPrerequisite(30171, QuestStatus.Completed));

		AddObjective("infuseTheCrystals", L("Disarm the Regular Prison Cell's Secret Device"), new ManualObjective());

		AddReward(new ItemReward("PRISON_80_MQ_9_ITEM", 1));
		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 7440));
	}
}

// 30173: Solitary Cell Barrier
//-----------------------------------------------------------------------------
public class Prison80Mq10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30173);
		SetName(L("Solitary Cell Barrier"));
		SetDescription(L("The third demon barrier, and another piece of Zanas' soul for it."));
		SetType(QuestType.Main);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_MQ_10_TRIGGER", "d_prison_80", L("Release the Demon Barrier"), L("You have obtained the King's Green Jewel. Now go to the Demon Barrier at the Reintegration Workshop."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_MQ_10_TRIGGER", "d_prison_80", L("Release the Demon Barrier"), L("You have obtained the King's Green Jewel. Now go to the Demon Barrier at the Reintegration Workshop."));
		SetPhase(QuestStatus.Success, "PRISON_80_MQ_10_TRIGGER", "d_prison_80", L("Release the Demon Barrier"), L("You have obtained the King's Green Jewel. Now go to the Demon Barrier at the Reintegration Workshop."));

		SetTrack(QuestStatus.Success, QuestStatus.Completed, "PRISON_80_MQ_10_TRACK", 1000);

		AddPrerequisite(new QuestStatusPrerequisite(30172, QuestStatus.Completed));

		AddObjective("releaseTheBarrier", L("Release the Demon Barrier"), new ManualObjective());
	}
}

// 30199: Confiscated Prisoner Belongings
//-----------------------------------------------------------------------------
public class Prison80Sq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30199);
		SetName(L("Confiscated Prisoner Belongings"));
		SetDescription(L("A prisoner's care package never made it past the guards."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_80");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_80_SQ_OBJ_1", "d_prison_80", L("Check the walls of the Long Sentence Prison Cell"), L("There is something written on the walls of the Long Sentence Prison Cell. Read it to see what it has to say."));
		SetPhase(QuestStatus.InProgress, "PRISON_80_SQ_OBJ_2", "d_prison_80", L("Search Confiscated Goods"), L("The wall is filled with the angry words of a prisoner who had his belongings taken by a guard. Look for the Confiscated Goods by reading what the prisoner has written."));
		SetPhase(QuestStatus.Success, "PRISON_80_SQ_OBJ_2", "d_prison_80", L("Search Confiscated Goods"), L("The wall is filled with the angry words of a prisoner who had his belongings taken by a guard. Look for the Confiscated Goods by reading what the prisoner has written."));

		AddPrerequisite(new LevelPrerequisite(255));

		AddObjective("findTheGoods", L("Search Confiscated Goods"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7440));
		AddReward(new ItemReward("Drug_Premium_HP1", 20));
	}
}
