//--- Melia Script ----------------------------------------------------------
// Resident Quarter Quest NPCs
//--- Description -----------------------------------------------------------
// The Old Manager of the fortress, the certification ticket he makes out of
// mushrooms, and the soldier's spirit still holding his mother's ring.
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

public class DUnderfortress67QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq010 = new QuestId(50063);
	private readonly static QuestId Mq020 = new QuestId(50064);
	private readonly static QuestId Mq030 = new QuestId(50065);
	private readonly static QuestId Mq040 = new QuestId(50066);
	private readonly static QuestId Mq050 = new QuestId(50067);
	private readonly static QuestId Mq060 = new QuestId(50068);
	private readonly static QuestId Sq010 = new QuestId(50069);
	private readonly static QuestId Sq020 = new QuestId(50070);
	private readonly static QuestId Sq030 = new QuestId(50071);

	private const int MushroomsNeeded = 6;

	// The Tempas Mushrooms the certification ticket is made from.
	private readonly static double[,] Mushrooms =
	{
		{ 1673.59, -344.96 }, { 1466.19, -903.17 }, { 1709.32, -558.18 },
		{ 1845.79, -1411.20 }, { 1155.38, -852.99 }, { 1282.82, -327.51 },
	};

	private readonly static double[] MushroomFacings = { 37, 82, 49, 90, 99, 90 };

	protected override void Load()
	{
		// Amanda, at the quarter gate
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_67_1", "d_underfortress_67", 95.14, -1444.12, 90, this.IsAmandaAtTheGate, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq010) && character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("Did you find something special from the demons?"));
				await dialog.CompleteQuest(Mq010);
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				var answer = await dialog.SelectQuestOffer(Mq010, L("Okay. Let's look around again with the Monocle. If you can see some special force, then there will be something like the treasure of Ruklys."),
					Option(L("Could you search the area just one more time?"), "accept"),
					Option(L("Wait a moment"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				await dialog.Msg(L("We have no choice."));

				var answer = await dialog.SelectQuestOffer(Mq020, L("It's far too wide and there are so many things here that look like a monocle. We should split up."),
					Option(L("Alright"), "accept"),
					Option(L("Tell her to look for it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq020);
					character.LookAround();
					await dialog.Msg(L("I will search the area up here."));
					await dialog.Msg(L("Let's meet later."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq010))
			{
				await dialog.Msg(L("That box over there is what the Monocle picked out. Get whatever is in it off the demons."));
				character.Quests.ReplayQuestTrack(Mq010);
				return;
			}

			await dialog.Msg(L("A grave robber sweeping a whole quarter with one Monocle."));
		});

		// Amanda, in the Bingcoolers Path
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_67_2", "d_underfortress_67", -821.05, -1075.80, 171, this.IsAmandaOnThePath, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq020) && character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("How about you?"));
				await dialog.Msg(L("Have you obtained anything?"));
				await dialog.CompleteQuest(Mq020);
				return;
			}

			if (character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("An old keeper with a certification ticket? That is worth more than the box was."));
				character.Quests.CompleteObjective(Mq040, "tellAmanda");
				character.ServerMessage(L("Amanda knows about the Old Manager now. Collect the mushrooms he asked for."));
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				await dialog.Msg(L("Is that so? My side also had demons only, nothing else."));

				var answer = await dialog.SelectQuestOffer(Mq030, L("That's strange. There must be something here."),
					Option(L("Try using the monocle"), "accept"),
					Option(L("There's probably nothing of importance"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq030);
					character.LookAround();
					await dialog.Msg(L("Wait! I can see the great energy coming from Leima Small Square."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq020))
			{
				await dialog.Msg(L("It really angers me."));
				await dialog.Msg(L("Demons do not deserve any mercy."));
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				await dialog.Msg(L("You can't miss it, really. The sheer size of it stands out."));
				await dialog.Msg(L("Promise me that you won't keep it to yourself when you find it."));
				return;
			}

			await dialog.Msg(L("A grave robber working the Bingcoolers Path over for the second time."));
		});

		// The Old Manager
		//-------------------------------------------------------------------------
		AddConditionalNpc(153139, L("Old Manager"), "EMINENT_67_1", "d_underfortress_67", 438.07, -509.96, 90, this.IsManagerAtLeima, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Manager"));

			if (character.Quests.IsActive(Mq030) && !character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Huh? Outsiders.. Who are you?"));
				await dialog.Msg(L("How did you get past all the guards?"));

				var explained = await character.TimeActions.StartAsync(L("Explaining the revelation..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(1));

				if (explained != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq030, "findTheSquare");
				return;
			}

			if (character.Quests.IsActive(Mq030) && character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Yes! A revelation..."));
				await dialog.Msg(L("Only the Revelators know the existence of the revelation."));
				await dialog.CompleteQuest(Mq030);
				return;
			}

			if (character.Quests.IsActive(Mq040) && character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("They are enough to make the certificate."));
				await dialog.Msg(L("Please wait a bit."));
				await dialog.CompleteQuest(Mq040);
				return;
			}

			if (character.Quests.IsActive(Mq050) && character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("A certificate is just a fancy word. The principle behind it is quite simple."));
				await dialog.Msg(L("What matters the most is ingredients and how you mix them."));
				await dialog.CompleteQuest(Mq050);
				character.ServerMessage(L("The Old Manager hands over a certification ticket. His devices will not attack you now."));
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				await dialog.Msg(L("You can't say no nor ask for reasons to the kingdom orders."));

				var answer = await dialog.SelectQuestOffer(Mq040, L("But as I have lived here my whole life, I have many things to think about."),
					Option(L("Follow the keeper, but tell Amanda discreetly"), "accept"),
					Option(L("I will find a way on my own"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq040);
					await dialog.Msg(L("Oh my, how silly of me."));
					await dialog.Msg(L("I have forgotten about the devices I have laid around here to keep intruders away. The ticket that gets you past them starts with mushrooms."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				var answer = await dialog.SelectQuestOffer(Mq050, L("Let's inscribe the magic of the fortress into the mushrooms you have brought. My old body cannot hold the spell, so Earth Crystals will have to do it for me."),
					Option(L("I will acquire the Earth Crystal"), "accept"),
					Option(L("I will obtain them later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq050);
					await dialog.Msg(L("It's not like the olden days."));
					await dialog.Msg(L("Reckless usage of magic... will take its toll on my old body."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq060) && character.Quests.MeetsPrerequisites(Mq060))
			{
				var answer = await dialog.SelectQuestOffer(Mq060, L("Now, the Storage Quarter will be accessible. Clues to find the revelation must be there."),
					Option(L("Alright"), "accept"),
					Option(L("Tell him to go there later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq060);
					character.LookAround();
					await dialog.Msg(L("Don't come late."));
					character.ServerMessage(L("Leave a note with the way to make a certification ticket for Amanda."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				await dialog.Msg(L("I will help anything that is related to the goddesses."));
				await dialog.Msg(L("Finally, an opportunity has come in my boring life."));
				return;
			}

			if (character.Quests.IsActive(Mq050))
			{
				await dialog.Msg(L("Reckless usage of magic... will take its toll on my old body."));
				return;
			}

			await dialog.Msg(L("A keeper who has run this quarter longer than the kingdom has held the fortress."));
		});

		// The Old Manager, on his way out
		//-------------------------------------------------------------------------
		AddConditionalNpc(153139, L("Old Manager"), "EMINENT_67_2", "d_underfortress_67", 62.62, -277.83, 90, this.IsManagerLeaving, async dialog =>
		{
			await dialog.Msg(L("I will be in the Storage Quarter. Do not be late."));
		});

		// The Old Manager's devices
		//-------------------------------------------------------------------------
		AddNpc(147306, L("Keeper's Device"), "UNDER67_MQ060_DEVICE", "d_underfortress_67", 459.13, -509.82, 52, async dialog =>
		{
			await dialog.Msg(L("One of the keeper's devices, and it does not appear to care who it is pointed at."));
		});

		// The note left for Amanda
		//-------------------------------------------------------------------------
		AddConditionalNpc(40095, L("Note Spot"), "UNDER67_MQ060", "d_underfortress_67", 453.14, -456.71, 90, this.IsNoteSpotOpen, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Note Spot"));

			if (character.Quests.IsActive(Mq060) && !character.Quests.IsCompletable(Mq060))
			{
				var left = await character.TimeActions.StartAsync(L("Leaving the note..."), L("Cancel"), "BURY", TimeSpan.FromSeconds(3));

				if (left != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq060, "leaveTheNote");
				character.LookAround();
				character.ServerMessage(L("The note is where Amanda will find it. Go on to the Storage Quarter."));
				return;
			}

			await dialog.Msg(L("A gap in the flagstones where a folded note would not be noticed."));
		});

		AddConditionalNpc(147312, L("Note"), "UNDER67_MQ6_TO_MEMO", "d_underfortress_67", 451.65, -458.20, 44, this.IsNoteLeft, async dialog =>
		{
			await dialog.Msg(L("A folded note, put where a grave robber would look and a keeper would not."));
		});

		// Tempas Mushrooms
		//-------------------------------------------------------------------------
		for (var i = 0; i < Mushrooms.GetLength(0); ++i)
		{
			AddNpc(155023, L("Mushroom"), i == 0 ? "UNDER67_GRASS" : "UNDER67_GRASS_" + (i + 1), "d_underfortress_67",
				Mushrooms[i, 0], Mushrooms[i, 1], MushroomFacings[i], this.PickMushroom);
		}

		// The books of the Ruklys era
		//-------------------------------------------------------------------------
		AddConditionalNpc(147311, L("Old Book"), "UNDER67_BOOK1", "d_underfortress_67", -1426.49, -953.43, -13, this.IsFirstBookLying, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Book"));

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				var read = await character.TimeActions.StartAsync(L("Looking through the book..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(2));

				if (read != TimeActionResult.Completed)
					return;

				character.Quests.Start(Sq010);
				character.Inventory.Add(ItemId.UNDER67_SQ1_ITEM01, 1, InventoryAddType.PickUp);
				character.LookAround();
				character.ServerMessage(L("The book has an account of the Ruklys army in it. There should be another."));
				return;
			}

			await dialog.Msg(L("A book of the Ruklys era, and the damp has taken most of it."));
		});

		AddConditionalNpc(153014, L("Old Book"), "UNDER67_BOOK2", "d_underfortress_67", 1622.99, -1651.72, 103, this.IsSecondBookLying, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Book"));

			if (character.Quests.IsActive(Sq010) && !character.Quests.IsCompletable(Sq010))
			{
				var read = await character.TimeActions.StartAsync(L("Looking through the book..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(2));

				if (read != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.UNDER67_SQ1_ITEM02, 1, InventoryAddType.PickUp);
				character.LookAround();
				character.ServerMessage(L("The second account is readable too. Wilhelmina Carriot will want both."));
				return;
			}

			await dialog.Msg(L("A second book of the same set, in rather better condition."));
		});

		// Soldier's Grave
		//-------------------------------------------------------------------------
		AddNpc(47170, L("Soldier's Grave"), "UNDER67_SQ020", "d_underfortress_67", 1769.68, 778.66, 17, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Soldier's Grave"));

			if (character.Quests.IsActive(Sq020) && !character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("The grave is open, and what came out of it is not the only thing that did."));
				character.Quests.ReplayQuestTrack(Sq020);
				return;
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				var examined = await character.TimeActions.StartAsync(L("Examining the soldier's grave..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(1));

				if (examined != TimeActionResult.Completed)
					return;

				character.Quests.Start(Sq020);
				character.Inventory.Add(ItemId.UNDER_67_SQ020_ITEM01, 1, InventoryAddType.PickUp);
				character.ServerMessage(L("There is a ring in the grave, and something comes for it at once!"));
				return;
			}

			await dialog.Msg(L("A soldier's grave, old enough that nobody has come to it in a long time."));
		});

		// The Resentful Soldier's Spirit
		//-------------------------------------------------------------------------
		AddConditionalNpc(11283, L("Resentful Spirit"), "UNDER67_SQ030", "d_underfortress_67", 1776.00, 802.00, 0, this.IsSpiritCalm, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Resentful Spirit"));

			if (character.Quests.IsActive(Sq020) && character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("No! Please don't take it away from me.."));
				await dialog.CompleteQuest(Sq020);
				return;
			}

			if (!character.Quests.Has(Sq030) && character.Quests.MeetsPrerequisites(Sq030))
			{
				await dialog.Msg(L("That ring was given to me by my mom."));

				var answer = await dialog.SelectQuestOffer(Sq030, L("She told me to help Ruklys and win."),
					Option(L("Let's help"), "accept"),
					Option(L("Ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq030);
					await dialog.Msg(L("Our house is in the inner castle district..."));
					await dialog.Msg(L("Please... I want to go back home with the ring..."));
					character.LookAround();
					return;
				}
				return;
			}

			await dialog.Msg(L("A soldier's spirit that has been holding one ring since Ruklys fell."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The soldier's house inside the castle walls, where the ring goes back.
		AddQuestTrigger("UNDER_67_SQ030_NPC", "f_flash_64", -400.56, 710.55, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Sq030) || character.Quests.IsCompletable(Sq030))
				return;

			character.Quests.CompleteObjective(Sq030, "takeTheRingHome");
			character.ServerMessage(L("The ring goes back where it came from, and the spirit does not follow it in."));

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether Amanda is still at the quarter gate.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheGate(Character character)
		=> !character.Quests.Has(Mq020);

	/// <summary>
	/// Returns whether Amanda has moved on to the Bingcoolers Path.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaOnThePath(Character character)
		=> character.Quests.Has(Mq020) && !character.Quests.HasCompleted(Mq040);

	/// <summary>
	/// Returns whether the Old Manager is still at the Leima Small Square.
	/// </summary>
	/// <param name="character"></param>
	private bool IsManagerAtLeima(Character character)
		=> character.Quests.Has(Mq030) && !character.Quests.IsCompletable(Mq060) && !character.Quests.HasCompleted(Mq060);

	/// <summary>
	/// Returns whether the Old Manager is on his way to the Storage Quarter.
	/// </summary>
	/// <param name="character"></param>
	private bool IsManagerLeaving(Character character)
		=> character.Quests.IsActive(Mq060);

	/// <summary>
	/// Returns whether the note spot is still bare.
	/// </summary>
	/// <param name="character"></param>
	private bool IsNoteSpotOpen(Character character)
		=> character.Quests.IsActive(Mq060) && !character.Quests.IsCompletable(Mq060);

	/// <summary>
	/// Returns whether the note for Amanda has been left.
	/// </summary>
	/// <param name="character"></param>
	private bool IsNoteLeft(Character character)
		=> character.Quests.IsCompletable(Mq060) || character.Quests.HasCompleted(Mq060);

	/// <summary>
	/// Returns whether the first Ruklys era book is still where it fell.
	/// </summary>
	/// <param name="character"></param>
	private bool IsFirstBookLying(Character character)
		=> !character.Quests.Has(Sq010);

	/// <summary>
	/// Returns whether the second Ruklys era book is still where it fell.
	/// </summary>
	/// <param name="character"></param>
	private bool IsSecondBookLying(Character character)
		=> character.Quests.IsActive(Sq010) && !character.Quests.IsCompletable(Sq010);

	/// <summary>
	/// Returns whether the soldier's spirit has stopped fighting.
	/// </summary>
	/// <param name="character"></param>
	private bool IsSpiritCalm(Character character)
		=> character.Quests.IsCompletable(Sq020) && !character.Quests.Has(Sq030);

	/// <summary>
	/// Picks one of the Tempas Mushrooms the ticket is made from.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task PickMushroom(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Mushroom"));

		if (!character.Quests.IsActive(Mq040))
		{
			await dialog.Msg(L("A mushroom of the fortress, grown fat on whatever is in the walls."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.UNDER67_MQ4_ITEM01) >= MushroomsNeeded)
		{
			await dialog.Msg(L("You have as many mushrooms as the keeper asked for."));
			return;
		}

		var picked = await character.TimeActions.StartAsync(L("Picking the mushroom..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

		if (picked != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.UNDER67_MQ4_ITEM01, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("The cap comes away whole, which is what the keeper wanted."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50063: Special Powers Discovered by the Monocle (1)
//-----------------------------------------------------------------------------
public class Underfortress67Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50063);
		SetName(L("Special Powers Discovered by the Monocle (1)"));
		SetDescription(L("The box the Monocle picked out of the quarter has demons in it rather than treasure."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_67");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_67_1", "d_underfortress_67", L("Speak with Grave Robber Amanda in the habitants' area"), L("What you found in the secret area is a strange scroll. Move back to the habitants' area and talk with Amanda again."));
		SetPhase(QuestStatus.InProgress, "AMANDA_67_1", "d_underfortress_67", L("Defeat the demons"), L("The chest Amanda saw with a special power hidden inside was actually containing demons. Defeat the incoming demons first."));
		SetPhase(QuestStatus.Success, "AMANDA_67_1", "d_underfortress_67", L("Talk to Grave Robber Amanda"), L("The oncoming wave of demons have fallen. Talk with Amanda again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_67_MQ010_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50061, QuestStatus.Completed));

		AddObjective("killTheBox", L("Defeat demon monsters"), new KillObjective(4, "Rambear_brown", "Rambear_bow_brown", "Rambear_mage_brown") { LayerOnly = true });

		AddReward(new ItemReward("expCard10", 1));
	}
}

// 50064: Special Powers Discovered by the Monocle (2)
//-----------------------------------------------------------------------------
public class Underfortress67Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50064);
		SetName(L("Special Powers Discovered by the Monocle (2)"));
		SetDescription(L("The quarter is too wide for one Monocle, so the search splits up."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_67");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_67_1", "d_underfortress_67", L("Talk to Grave Robber Amanda"), L("Amanda becomes increasingly nervous after her monocle produces no useful result. Speak with Amanda."));
		SetPhase(QuestStatus.InProgress, "AMANDA_67_2", "d_underfortress_67", L("Find the special power that exists in demons"), L("Defeat the demons in the Bingcoolers Path area and look for the special power."));
		SetPhase(QuestStatus.Success, "AMANDA_67_2", "d_underfortress_67", L("Talk to Grave Robber Amanda"), L("Nothing unusual here. Return to Amanda."));

		AddPrerequisite(new QuestStatusPrerequisite(50063, QuestStatus.Completed));

		AddObjective("searchTheDemons", L("Defeat the demons"), new KillObjective(6, "Rambear_brown", "Rambear_bow_brown", "Rambear_mage_brown"));

		AddReward(new ItemReward("expCard10", 2));
	}
}

// 50065: Special Powers Discovered by the Monocle (3)
//-----------------------------------------------------------------------------
public class Underfortress67Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50065);
		SetName(L("Special Powers Discovered by the Monocle (3)"));
		SetDescription(L("The great energy at the Leima Small Square turns out to be one old keeper."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_67");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_67_2", "d_underfortress_67", L("Talk to Grave Robber Amanda"), L("All work and no fruit. Speak with Amanda again."));
		SetPhase(QuestStatus.InProgress, "EMINENT_67_1", "d_underfortress_67", L("Search the Leima Small Square"), L("Amanda says she saw something great in the Leima Small Square. Find out what she saw."));
		SetPhase(QuestStatus.Success, "EMINENT_67_1", "d_underfortress_67", L("Talk to the Old Manager"), L("Only an old manager was in the Leima Small Square. Ask the Old Manager about what is happening."));

		AddPrerequisite(new QuestStatusPrerequisite(50064, QuestStatus.Completed));

		AddObjective("findTheSquare", L("Search the Leima Small Square"), new ManualObjective());
	}
}

// 50066: Fortress of the Land Manager
//-----------------------------------------------------------------------------
public class Underfortress67Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50066);
		SetName(L("Fortress of the Land Manager"));
		SetDescription(L("The keeper's traps take a certification ticket, and a ticket takes mushrooms."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_67");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_67_1", "d_underfortress_67", L("Talk to the Old Manager"), L("The Old Manager is happy to help you on the quest to find the revelation. Speak with the Old Manager again."));
		SetPhase(QuestStatus.InProgress, "UNDER67_GRASS", "d_underfortress_67", L("Collect Mushrooms"), L("The ticket's main ingredient is mushrooms. On your way there, speak with Amanda and report the situation to her."));
		SetPhase(QuestStatus.Success, "EMINENT_67_1", "d_underfortress_67", L("Deliver to the Old Manager"), L("You've told Amanda about what happened and gathered enough mushrooms. Give the mushrooms to the Old Manager."));

		AddPrerequisite(new QuestStatusPrerequisite(50065, QuestStatus.Completed));

		AddObjective("tellAmanda", L("Tell Amanda about the Old Manager"), new ManualObjective());
		AddObjective("collectMushrooms", L("Collect Mushrooms"), new CollectItemObjective("UNDER67_MQ4_ITEM01", 6));

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("UNDER67_MQ4_ITEM01"));
	}
}

// 50067: To the Storage Quarter (1)
//-----------------------------------------------------------------------------
public class Underfortress67Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50067);
		SetName(L("To the Storage Quarter (1)"));
		SetDescription(L("The keeper is too old to put the spell in himself, so Earth Crystals do it."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_67");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_67_1", "d_underfortress_67", L("Talk to the Old Manager"), L("You've collected enough mushrooms to make a certification ticket. Ask the Old Manager what the other ingredients are."));
		SetPhase(QuestStatus.InProgress, "EMINENT_67_1", "d_underfortress_67", L("Collect Earth Crystals"), L("Defeat the monsters and gather Earth Crystals, the perfect replacement for the keeper's own magic."));
		SetPhase(QuestStatus.Success, "EMINENT_67_1", "d_underfortress_67", L("Deliver to the Old Manager"), L("You've gathered enough Earth Crystals. Give them to the Old Manager."));

		AddPrerequisite(new QuestStatusPrerequisite(50066, QuestStatus.Completed));

		AddObjective("collectCrystals", L("Collect Earth Crystals from the monsters"), new CollectItemObjective("UNDER67_MQ5_ITEM01", 10));

		AddPityDrop("UNDER67_MQ5_ITEM01", 0.7f, 3, 1, "dandel_white");

		AddReward(new ItemReward("UNDER67_MQ5_ITEM02", 1));
		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("UNDER67_MQ5_ITEM01"));
	}
}

// 50068: To the Storage Quarter (2)
//-----------------------------------------------------------------------------
public class Underfortress67Mq060Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50068);
		SetName(L("To the Storage Quarter (2)"));
		SetDescription(L("The keeper goes on ahead, which leaves a moment to write Amanda her own way in."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_67", "d_underfortress_68");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_67_1", "d_underfortress_67", L("Talk to the Old Manager"), L("You finally have the certification ticket that ensures safe access to the storage quarter. Speak to the Old Manager."));
		SetPhase(QuestStatus.InProgress, "UNDER67_MQ060", "d_underfortress_67", L("Leave the note"), L("This is the chance to leave a note for Amanda so she can make her own certification ticket."));
		SetPhase(QuestStatus.Success, "EMINENT_68_1", "d_underfortress_68", L("Speak with the Old Manager in the Storage Quarter"), L("Left the note safely. Go to the Old Manager in the Storage Quarter."));

		AddPrerequisite(new QuestStatusPrerequisite(50067, QuestStatus.Completed));

		AddObjective("leaveTheNote", L("Leave the note"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 1));
		AddReward(new TakeItemReward("UNDER67_MQ5_ITEM02", 1));
	}
}

// 50069: The Soldiers' Story
//-----------------------------------------------------------------------------
public class Underfortress67Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50069);
		SetName(L("The Soldiers' Story"));
		SetDescription(L("Two of the Ruklys era books in the quarter are still readable, and Kaliss pays for readable."));
		SetType(QuestType.Sub);
		SetLocation("d_underfortress_67", "f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER67_BOOK1", "d_underfortress_67", L("Look for the hidden book"), L("The books from the Ruklys Era are scattered around. Look for the books that are readable."));
		SetPhase(QuestStatus.InProgress, "UNDER67_BOOK2", "d_underfortress_67", L("Look for the hidden book"), L("The books from the Ruklys Era are scattered around. Look for the books that are readable."));
		SetPhase(QuestStatus.Success, "FLASH64_KARRIAT", "f_flash_64", L("Hand them over to Wilhelmina Carriot"), L("If you bring these books to Wilhelmina Carriot, you would be able to receive some rewards."));

		AddPrerequisite(new LevelPrerequisite(197));

		AddObjective("findFirstBook", L("Look for the first readable book"), new CollectItemObjective("UNDER67_SQ1_ITEM01", 1));
		AddObjective("findSecondBook", L("Look for the second readable book"), new CollectItemObjective("UNDER67_SQ1_ITEM02", 1));

		AddReward(new ItemReward("UNDER67_SQ1_COPY_BOOK1", 1));
		AddReward(new ItemReward("UNDER67_SQ1_COPY_BOOK2", 1));
		AddReward(new TakeItemReward("UNDER67_SQ1_ITEM01", 1));
		AddReward(new TakeItemReward("UNDER67_SQ1_ITEM02", 1));
	}
}

// 50070: The Resentful Soldier's Spirit (1)
//-----------------------------------------------------------------------------
public class Underfortress67Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50070);
		SetName(L("The Resentful Soldier's Spirit (1)"));
		SetDescription(L("Opening the soldier's grave brings something else out with the spirit."));
		SetType(QuestType.Sub);
		SetLocation("d_underfortress_67");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER67_SQ020", "d_underfortress_67", L("Examine the Soldier's Grave"), L("There is a grave of a soldier that looks very old. Take a look at it."));
		SetPhase(QuestStatus.InProgress, "UNDER67_SQ020", "d_underfortress_67", L("Defeat the resentment that rushed in"), L("Suddenly a monster came to attack the soldier's spirit. Destroy it."));
		SetPhase(QuestStatus.Success, "UNDER67_SQ030", "d_underfortress_67", L("Talk to the Resentful Soldier's Spirit"), L("The Resentful Soldier's Spirit seems to be somewhat calm. Talk with the Resentful Soldier's Spirit."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_67_SQ020_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(197));

		AddObjective("killNecroventer", L("Defeat the resentment that rushed in"), new KillObjective(1, "boss_necrovanter_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard10", 3));
	}
}

// 50071: The Resentful Soldier's Spirit (2)
//-----------------------------------------------------------------------------
public class Underfortress67Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50071);
		SetName(L("The Resentful Soldier's Spirit (2)"));
		SetDescription(L("The ring belongs at a house inside the castle walls, and the spirit wants it there."));
		SetType(QuestType.Sub);
		SetLocation("d_underfortress_67", "f_flash_64");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER67_SQ030", "d_underfortress_67", L("Talk to the Resentful Soldier's Spirit"), L("The spirit seems to have some story. Talk with the Resentful Soldier's Spirit."));
		SetPhase(QuestStatus.InProgress, "UNDER_67_SQ030_NPC", "f_flash_64", L("Go to the place where the Resentful Soldier's Spirit told you to go"), L("Go to the soldier's house, which is located inside the castle walls, with the ring."));
		SetPhase(QuestStatus.Success, "UNDER_67_SQ030_NPC", "f_flash_64", L("Go to the place where the Resentful Soldier's Spirit told you to go"), L("The ring is back where it came from."));

		AddPrerequisite(new QuestStatusPrerequisite(50070, QuestStatus.Completed));

		AddObjective("takeTheRingHome", L("Take the ring to the soldier's house"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("UNDER_67_SQ020_ITEM01", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client names no turn-in NPC; leaving the ring is the whole of it.
		character.Quests.Complete(this.QuestId);
	}
}
