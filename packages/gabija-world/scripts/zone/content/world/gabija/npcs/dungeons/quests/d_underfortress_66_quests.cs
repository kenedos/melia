//--- Melia Script ----------------------------------------------------------
// Drill Ground of Confliction Quest NPCs
//--- Description -----------------------------------------------------------
// Guard Delus and what is left of his detachment, the camp they take back,
// and the Ruklys officer who was waiting behind it.
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

public class DUnderfortress66QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq010 = new QuestId(50055);
	private readonly static QuestId Mq020 = new QuestId(50056);
	private readonly static QuestId Mq030 = new QuestId(50057);
	private readonly static QuestId Mq040 = new QuestId(50058);
	private readonly static QuestId Mq050 = new QuestId(50059);
	private readonly static QuestId Mq060 = new QuestId(50060);
	private readonly static QuestId Mq070 = new QuestId(50061);

	private const int WeaponsNeeded = 4;
	private const int ArmoursNeeded = 4;

	// The guards who fell behind on the way in.
	private readonly static string[] StragglerNames =
	{
		"UNDER66_KINGDOM_GUADIAN01", "UNDER66_KINGDOM_GUADIAN02",
		"UNDER66_KINGDOM_GUADIAN03", "UNDER66_KINGDOM_GUADIAN04",
	};

	private readonly static double[,] StragglerSpots =
	{
		{ 126.51, -738.95 }, { -704.62, -516.00 }, { -193.57, -529.53 }, { 155.62, -1265.67 },
	};

	private readonly static double[] StragglerFacings = { 234, 149, 90, 198 };

	// The bags the fallen guards left their equipment in.
	private readonly static string[] BagNames =
	{
		"UNDER66_DEAD_KINGDOM_GUADIAN01", "UNDER66_DEAD_KINGDOM_GUADIAN02",
		"UNDER66_DEAD_KINGDOM_GUADIAN03", "UNDER66_DEAD_KINGDOM_GUADIAN04",
	};

	private readonly static double[,] BagSpots =
	{
		{ -205.80, 393.59 }, { -32.37, 669.64 }, { 46.55, 952.60 }, { 35.15, 178.74 },
	};

	private readonly static double[] BagFacings = { 110, 50, -36, 2 };

	// The supply boxes the barricades come out of.
	private readonly static double[,] SupplyBoxes =
	{
		{ 1803.61, 262.44 }, { 1802.24, 499.58 }, { 1887.61, 441.22 },
	};

	private readonly static double[] SupplyBoxFacings = { -73, 185, 90 };

	// The spots the barricades go, and the barricades once they are up.
	private readonly static double[,] BarricadeSpots =
	{
		{ 1383.29, 411.32 }, { 1332.50, 323.54 }, { 1420.12, 342.57 }, { 1420.24, 325.01 },
	};

	private readonly static double[,] SetBarricades =
	{
		{ 1383.31, 411.55 }, { 1334.28, 323.98 }, { 1421.91, 342.60 }, { 1418.90, 325.23 },
	};

	private readonly static double[] SetBarricadeFacings = { 264, 269, 267, 263 };

	protected override void Load()
	{
		// Amanda, at the drill ground entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_66_1", "d_underfortress_66", 1250.00, -205.00, 56, this.IsAmandaAtTheEntrance, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("By the way, what is the thing that you are looking for?"));

				var answer = await dialog.SelectQuestOffer(Mq010, L("I guess it's important since you don't say anything about it."),
					Option(L("Let's move"), "accept"),
					Option(L("I have something to do"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq010))
			{
				await dialog.Msg(L("Something is coming up the drill ground, and it is not the guards."));
				character.Quests.ReplayQuestTrack(Mq010);
				return;
			}

			await dialog.Msg(L("A grave robber who will not say what she is looking for down here."));
		});

		// Amanda, at the reclaimed camp
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_66_2", "d_underfortress_66", 1932.61, 285.01, 230, this.IsAmandaAtTheCamp, async dialog =>
		{
			await dialog.Msg(L("Keep the guards talking. I want a look at the back of this camp."));
		});

		// Amanda, after the officer's spirit
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_66_3", "d_underfortress_66", 1610.56, 394.62, -68, this.IsAmandaAfterTheSpirit, async dialog =>
		{
			await dialog.Msg(L("There is a hidden place behind that camp and the guards never once looked at it."));
		});

		// Royal Army Guard Delus, at the drill ground
		//-------------------------------------------------------------------------
		AddConditionalNpc(20019, L("Royal Army Guard Delus"), "UNDER66_DELLOOS01", "d_underfortress_66", 1118.96, -5.82, 65, this.IsDelusAtTheDrillGround, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Army Guard Delus"));

			if (character.Quests.IsActive(Mq010) && character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("You are an outsider... but thanks for rescuing me."));
				await dialog.CompleteQuest(Mq010);
				return;
			}

			if (character.Quests.IsActive(Mq020) && character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("Thank you for rescuing our colleagues, but we haven't retrieved the camp yet."));
				await dialog.CompleteQuest(Mq020);
				return;
			}

			if (character.Quests.IsActive(Mq030) && character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Thank you for collecting the equipment."));
				await dialog.Msg(L("We are also ready to reclaim the camp."));
				await dialog.CompleteQuest(Mq030);
				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				await dialog.Msg(L("You guys should be punished according to kingdom orders in principle."));

				var answer = await dialog.SelectQuestOffer(Mq020, L("But since you've saved our lives, and many of our colleagues have fallen.."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("I can't help you"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq020);
					character.LookAround();
					await dialog.Msg(L("First, rescue our fallen colleagues."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				var answer = await dialog.SelectQuestOffer(Mq030, L("This place is being protected by the kingdom secretly. We here too are composed of the selected members from the kingdom."),
					Option(L("I will go collect the equipment"), "accept"),
					Option(L("I can't help you"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq030);
					await dialog.Msg(L("We also have our pride and fame."));
					await dialog.Msg(L("We are going to reclaim our camp so that the sacrifices of our colleagues won't be lost in vain."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				var answer = await dialog.SelectQuestOffer(Mq040, L("Okay. We will begin now. If you are scared, you can hide behind us. Let's go!"),
					Option(L("I'm ready"), "accept"),
					Option(L("I'm not yet ready"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq040);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq020))
			{
				await dialog.Msg(L("When our colleagues gather again, we are going to retrieve our camp."));
				await dialog.Msg(L("There were many monsters from the beginning, but I've never seen them going crazy like that."));
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				await dialog.Msg(L("We also have our pride and fame."));
				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				await dialog.Msg(L("We march on the camp on your word. Say when."));
				character.Quests.ReplayQuestTrack(Mq040);
				return;
			}

			await dialog.Msg(L("A guard with half a detachment and a camp he has been pushed out of."));
		});

		// Royal Army Guard Delus, mustered for the attack
		//-------------------------------------------------------------------------
		AddConditionalNpc(20019, L("Royal Army Guard Delus"), "UNDER66_DELLOOS", "d_underfortress_66", 1577.83, 369.74, 90, this.IsDelusMustered, async dialog =>
		{
			await dialog.Msg(L("Stay behind the line until we are through the gate."));
		});

		// Royal Army Guard Delus, in the reclaimed camp
		//-------------------------------------------------------------------------
		AddConditionalNpc(20019, L("Royal Army Guard Delus"), "UNDER66_DELLOOS03", "d_underfortress_66", 1944.44, 386.45, -56, this.IsDelusAtTheCamp, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Army Guard Delus"));

			if (character.Quests.IsActive(Mq040) && character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("I am sorry for not recognizing you."));
				await dialog.Msg(L("Your skills are so astonishing."));
				await dialog.CompleteQuest(Mq040);
				return;
			}

			if (character.Quests.IsActive(Mq050) && character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("Good."));
				await dialog.Msg(L("Since the hideout is safe now, you can leave the Fortress of the Land."));
				await dialog.CompleteQuest(Mq050);
				return;
			}

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				await dialog.Msg(L("We can't just relax even though we've reclaimed the camp."));

				var answer = await dialog.SelectQuestOffer(Mq050, L("We don't know when the monsters will attack again. It will be simple if you could place a barricade."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("I will leave now"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq050);
					character.LookAround();
					await dialog.Msg(L("It would help stop the monsters for a while."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq060) && character.Quests.MeetsPrerequisites(Mq060))
			{
				var answer = await dialog.SelectQuestOffer(Mq060, L("I've told you before, but you can't be at the Fortress of the Land. I am going to be quiet about the trespass since you've helped us, so please leave at once."),
					Option(L("Alright"), "accept"),
					Option(L("Think about it one more time"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq060);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq050))
			{
				await dialog.Msg(L("It will be simple if you could place a barricade."));
				return;
			}

			if (character.Quests.IsActive(Mq060))
			{
				await dialog.Msg(L("Leave while the way out is still quiet."));
				character.Quests.ReplayQuestTrack(Mq060);
				return;
			}

			await dialog.Msg(L("A guard standing in a camp he has back and no orders for what to do with it."));
		});

		// The Ruklys Army Officer's Spirit
		//-------------------------------------------------------------------------
		AddConditionalNpc(11282, L("Ruklys Army Officer's Spirit"), "UNDER66_MQ07_GHOST", "d_underfortress_66", 1806.75, 591.46, 46, this.IsSpiritPresent, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Ruklys Army Officer's Spirit"));

			if (character.Quests.IsActive(Mq060) && character.Quests.IsCompletable(Mq060))
			{
				await dialog.Msg(L("Finally... The worst curse has been undone!"));
				await dialog.CompleteQuest(Mq060);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq070) && character.Quests.MeetsPrerequisites(Mq070))
			{
				await dialog.Msg(L("Damn! We've lost the Fortress of the Land to those dirty demons!"));

				var answer = await dialog.SelectQuestOffer(Mq070, L("The magic circle... If we only have that magic circle..."),
					Option(L("I don't know what you are saying"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq070);
					await dialog.Msg(L("Yes... I can sense the same force like Ruklys within you.."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq070))
			{
				await dialog.Msg(L("Damn! We've lost the Fortress of the Land to those dirty demons!"));
				return;
			}

			await dialog.Msg(L("An officer of an army that lost this fortress long enough ago to be forgotten."));
		});

		// Hidden Area
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Hidden Area"), "UNDER66_TO_UNDER65_SECRET_ROOM", "d_underfortress_66", 1757.51, 602.33, 90, this.IsHiddenAreaOpen, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Hidden Area"));

			if (!character.Quests.IsActive(Mq070))
			{
				await dialog.Msg(L("A gap behind the camp that nothing draws the eye to."));
				return;
			}

			await dialog.Msg(L("The gap opens onto a room that was never on any plan of this fortress."));
			character.Warp("d_underfortress_65", -412.52, 303.23, 640);
		});

		// The guards who fell behind
		//-------------------------------------------------------------------------
		for (var i = 0; i < StragglerSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(10032, L("Left-behind Royal Army Guard"), StragglerNames[i], "d_underfortress_66",
				StragglerSpots[i, 0], StragglerSpots[i, 1], StragglerFacings[i], this.AreStragglersLeft, async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Left-behind Royal Army Guard"));

				if (character.Quests.IsActive(Mq020) && !character.Quests.IsCompletable(Mq020))
				{
					var carried = await character.TimeActions.StartAsync(L("Helping the guard up..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

					if (carried != TimeActionResult.Completed)
						return;

					character.Quests.CompleteObjective(Mq020, "rescueGuard" + number);
					character.ServerMessage(L("The guard makes his own way back to Delus from here."));
					return;
				}

				await dialog.Msg(L("A guard who did not get out with the rest and stopped where he fell."));
			});
		}

		// The bags the fallen guards left
		//-------------------------------------------------------------------------
		for (var i = 0; i < BagSpots.GetLength(0); ++i)
		{
			AddNpc(155012, L("Abandoned Kingdom Guard's Bag"), BagNames[i], "d_underfortress_66",
				BagSpots[i, 0], BagSpots[i, 1], BagFacings[i], this.SearchGuardBag);
		}

		// Supply Boxes
		//-------------------------------------------------------------------------
		for (var i = 0; i < SupplyBoxes.GetLength(0); ++i)
		{
			AddNpc(151029, L("Supply Box"), i == 0 ? "BOMB_BOX" : "BOMB_BOX_" + (i + 1), "d_underfortress_66",
				SupplyBoxes[i, 0], SupplyBoxes[i, 1], SupplyBoxFacings[i], this.TakeBarricade);
		}

		// The spots the barricades go
		//-------------------------------------------------------------------------
		for (var i = 0; i < BarricadeSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(40095, L("Barricade Site"), "UNDER66_BONB_SETUP0" + number, "d_underfortress_66",
				BarricadeSpots[i, 0], BarricadeSpots[i, 1], 90, character => this.IsBarricadeSiteEmpty(character, number), async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Barricade Site"));

				if (character.Quests.IsActive(Mq050) && !character.Quests.IsCompletable(Mq050))
				{
					var set = await character.TimeActions.StartAsync(L("Setting the barricade..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

					if (set != TimeActionResult.Completed)
						return;

					character.Quests.CompleteObjective(Mq050, "setBarricade" + number);
					character.LookAround();
					character.ServerMessage(L("The barricade is up."));
					return;
				}

				await dialog.Msg(L("A gap in the camp's line that Delus wants closed."));
			});
		}

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The box the hidden room was holding, over on the Sentry Bailey side.
		AddQuestTrigger("UNDER66_MQ7_TRIGGER", "d_underfortress_65", -336.02, 796.36, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq070) && !character.Quests.IsCompletable(Mq070))
			{
				character.Quests.CompleteObjective(Mq070, "searchTheRoom");
				character.Quests.StartQuestTrack(Mq070);
			}

			await Task.CompletedTask;
		});

		for (var i = 0; i < SetBarricades.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(57709, L("Barricade"), "UNDER66_BONB0" + number, "d_underfortress_66",
				SetBarricades[i, 0], SetBarricades[i, 1], SetBarricadeFacings[i], character => this.IsBarricadeSet(character, number), async dialog =>
			{
				await dialog.Msg(L("A stake barricade, set where the camp's line was open."));
			});
		}
	}

	/// <summary>
	/// Returns whether Amanda is still at the drill ground entrance.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheEntrance(Character character)
		=> !character.Quests.HasCompleted(Mq040);

	/// <summary>
	/// Returns whether Amanda has followed the guards into the camp.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheCamp(Character character)
		=> character.Quests.HasCompleted(Mq040) && !character.Quests.HasCompleted(Mq060);

	/// <summary>
	/// Returns whether Amanda has moved back from the officer's spirit.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAfterTheSpirit(Character character)
		=> character.Quests.HasCompleted(Mq060) && !character.Quests.HasCompleted(Mq070);

	/// <summary>
	/// Returns whether Delus is still holding the drill ground.
	/// </summary>
	/// <param name="character"></param>
	private bool IsDelusAtTheDrillGround(Character character)
		=> !character.Quests.Has(Mq040) || character.Quests.IsActive(Mq040);

	/// <summary>
	/// Returns whether Delus has mustered his line for the attack.
	/// </summary>
	/// <param name="character"></param>
	private bool IsDelusMustered(Character character)
		=> character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040);

	/// <summary>
	/// Returns whether Delus is standing in the reclaimed camp.
	/// </summary>
	/// <param name="character"></param>
	private bool IsDelusAtTheCamp(Character character)
		=> character.Quests.IsCompletable(Mq040) && !character.Quests.HasCompleted(Mq060);

	/// <summary>
	/// Returns whether the Ruklys officer's spirit has shown itself.
	/// </summary>
	/// <param name="character"></param>
	private bool IsSpiritPresent(Character character)
		=> character.Quests.IsCompletable(Mq060) && !character.Quests.HasCompleted(Mq070);

	/// <summary>
	/// Returns whether the hidden area behind the camp is open.
	/// </summary>
	/// <param name="character"></param>
	private bool IsHiddenAreaOpen(Character character)
		=> character.Quests.HasCompleted(Mq060);

	/// <summary>
	/// Returns whether any of the left-behind guards are still out there.
	/// </summary>
	/// <param name="character"></param>
	private bool AreStragglersLeft(Character character)
		=> character.Quests.IsActive(Mq020);

	/// <summary>
	/// Returns whether the numbered barricade site is still open.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="number"></param>
	private bool IsBarricadeSiteEmpty(Character character, int number)
		=> character.Quests.IsActive(Mq050, "setBarricade" + number);

	/// <summary>
	/// Returns whether the numbered barricade has been put up.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="number"></param>
	private bool IsBarricadeSet(Character character, int number)
	{
		if (!character.Quests.Has(Mq050))
			return false;

		return !character.Quests.IsActive(Mq050, "setBarricade" + number);
	}

	/// <summary>
	/// Takes the weapons and armour out of one of the fallen guards' bags.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task SearchGuardBag(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Abandoned Kingdom Guard's Bag"));

		if (!character.Quests.IsActive(Mq030))
		{
			await dialog.Msg(L("A guard's bag, left where its owner was."));
			return;
		}

		var weapons = character.Inventory.CountItem(ItemId.UNDER66_MQ3_ITEM01);
		var armours = character.Inventory.CountItem(ItemId.UNDER66_MQ3_ITEM02);

		if (weapons >= WeaponsNeeded && armours >= ArmoursNeeded)
		{
			await dialog.Msg(L("You have as much equipment as Delus asked for."));
			return;
		}

		var searched = await character.TimeActions.StartAsync(L("Searching the bag..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

		if (searched != TimeActionResult.Completed)
			return;

		if (weapons < WeaponsNeeded)
			character.Inventory.Add(ItemId.UNDER66_MQ3_ITEM01, 1, InventoryAddType.PickUp);

		if (armours < ArmoursNeeded)
			character.Inventory.Add(ItemId.UNDER66_MQ3_ITEM02, 1, InventoryAddType.PickUp);

		await dialog.Msg(L("What is in the bag is still serviceable, which is more than its owner is."));
	}

	/// <summary>
	/// Takes a barricade out of one of the camp's supply boxes.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task TakeBarricade(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Supply Box"));

		if (!character.Quests.IsActive(Mq050))
		{
			await dialog.Msg(L("A supply box of the camp, still packed and never opened."));
			return;
		}

		var taken = await character.TimeActions.StartAsync(L("Taking a barricade out..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(2));

		if (taken != TimeActionResult.Completed)
			return;

		await dialog.Msg(L("The barricades come out in one piece, stakes and all. Set them where the line is open."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50055: Surrounded by Enemies
//-----------------------------------------------------------------------------
public class Underfortress66Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50055);
		SetName(L("Surrounded by Enemies"));
		SetDescription(L("The Royal Army guards of the drill ground are being run down by the monsters."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_66");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_66_1", "d_underfortress_66", L("Move to Drill Ground of Confliction"), L("As Grave Robber Amanda has instructed, move to Drill Ground of Confliction."));
		SetPhase(QuestStatus.InProgress, "AMANDA_66_1", "d_underfortress_66", L("Defeat the monsters that are attacking the soldiers"), L("Monsters are rushing in! Get the guards out from under them."));
		SetPhase(QuestStatus.Success, "UNDER66_DELLOOS01", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("You've saved the Royal Army guards from the monsters. Talk with Guard Delus."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_66_MQ010_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50052, QuestStatus.Completed));

		AddObjective("saveTheGuards", L("Defeat the monsters that are attacking the soldiers"), new KillObjective(5, "ticen_blue") { LayerOnly = true });

		AddReward(new ItemReward("expCard10", 2));
	}
}

// 50056: Reclaim the Camp (1)
//-----------------------------------------------------------------------------
public class Underfortress66Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50056);
		SetName(L("Reclaim the Camp (1)"));
		SetDescription(L("Delus will forget the trespass if the guards who fell behind are brought in."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_66");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER66_DELLOOS01", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("Royal Army Guard Delus is confused after looking at you. Talk with Delus."));
		SetPhase(QuestStatus.InProgress, "UNDER66_KINGDOM_GUADIAN01", "d_underfortress_66", L("Bring the Royal Army Guards who fell behind"), L("Delus will let the trespass go if you help the soldiers. Bring the guards who've fallen behind."));
		SetPhase(QuestStatus.Success, "UNDER66_DELLOOS01", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("You've brought the guards safely. Talk with Delus."));

		AddPrerequisite(new QuestStatusPrerequisite(50055, QuestStatus.Completed));

		AddObjective("rescueGuard1", L("Bring in the first guard who fell behind"), new ManualObjective());
		AddObjective("rescueGuard2", L("Bring in the second guard who fell behind"), new ManualObjective());
		AddObjective("rescueGuard3", L("Bring in the third guard who fell behind"), new ManualObjective());
		AddObjective("rescueGuard4", L("Bring in the fourth guard who fell behind"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 2));
	}
}

// 50057: Reclaim the Camp (2)
//-----------------------------------------------------------------------------
public class Underfortress66Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50057);
		SetName(L("Reclaim the Camp (2)"));
		SetDescription(L("The detachment has no equipment left that is not lying where somebody dropped it."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_66");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER66_DELLOOS01", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("It seems that Delus still has many requests for you. Talk with Delus."));
		SetPhase(QuestStatus.InProgress, "UNDER66_DEAD_KINGDOM_GUADIAN01", "d_underfortress_66", L("Collect the equipment that can be used"), L("Delus told you that in order to reclaim the camp, you would need more equipment. Pick up the weapons and armors that were dropped on the ground."));
		SetPhase(QuestStatus.Success, "UNDER66_DELLOOS01", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("You've picked up enough weapons and armors. Talk with Delus."));

		AddPrerequisite(new QuestStatusPrerequisite(50056, QuestStatus.Completed));

		AddObjective("collectWeapons", L("Collect the abandoned weapons"), new CollectItemObjective("UNDER66_MQ3_ITEM01", 4));
		AddObjective("collectArmours", L("Collect the abandoned armour"), new CollectItemObjective("UNDER66_MQ3_ITEM02", 4));

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("UNDER66_MQ3_ITEM01"));
		AddReward(new TakeItemReward("UNDER66_MQ3_ITEM02"));
	}
}

// 50058: Reclaim the Camp (3)
//-----------------------------------------------------------------------------
public class Underfortress66Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50058);
		SetName(L("Reclaim the Camp (3)"));
		SetDescription(L("The detachment walks the camp back with the Revelator behind its line."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_66");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER66_DELLOOS01", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("You are ready to reclaim the base camp. Talk with Delus."));
		SetPhase(QuestStatus.InProgress, "UNDER66_DELLOOS", "d_underfortress_66", L("Move to the base camp by following the guards"), L("Follow the guards and move to the gathering place."));
		SetPhase(QuestStatus.Success, "UNDER66_DELLOOS03", "d_underfortress_66", L("Move to the base camp by following the guards"), L("The camp is back in the detachment's hands."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_66_MQ040_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50057, QuestStatus.Completed));

		AddObjective("clearTheCamp", L("Defeat the monsters that are occupying the base camp"), new KillObjective(9, "ticen_mage_blue", "ticen_blue") { LayerOnly = true });

		AddReward(new ItemReward("expCard10", 3));
	}
}

// 50059: Reclaim the Camp (4)
//-----------------------------------------------------------------------------
public class Underfortress66Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50059);
		SetName(L("Reclaim the Camp (4)"));
		SetDescription(L("The camp's line has gaps in it and the supply boxes have barricades for them."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_66");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER66_DELLOOS03", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("You've reclaimed the base camp. Talk with Guard Delus."));
		SetPhase(QuestStatus.InProgress, "UNDER66_BONB_SETUP01", "d_underfortress_66", L("Set the barricade"), L("To improve the defense at the camp, obtain the barricade from the supply box and set it."));
		SetPhase(QuestStatus.Success, "UNDER66_DELLOOS03", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("You've set all the barricades. Return to Guard Delus."));

		AddPrerequisite(new QuestStatusPrerequisite(50058, QuestStatus.Completed));

		AddObjective("setBarricade1", L("Set the first barricade"), new ManualObjective());
		AddObjective("setBarricade2", L("Set the second barricade"), new ManualObjective());
		AddObjective("setBarricade3", L("Set the third barricade"), new ManualObjective());
		AddObjective("setBarricade4", L("Set the fourth barricade"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 1));
		AddReward(new ItemReward("UNDER66_MQ6_ITEM01", 1));
	}
}

// 50060: Reclaim the Camp (5)
//-----------------------------------------------------------------------------
public class Underfortress66Mq060Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50060);
		SetName(L("Reclaim the Camp (5)"));
		SetDescription(L("A Specter Monarch takes the whole detachment, and what is left of it speaks."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_66");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER66_DELLOOS03", "d_underfortress_66", L("Talk to Royal Army Guard Delus"), L("You've listened to the request of Delus. Talk to Delus again."));
		SetPhase(QuestStatus.InProgress, "UNDER66_DELLOOS03", "d_underfortress_66", L("Defeat the monsters"), L("The guards are all under attack! Defeat the monsters that are rushing in."));
		SetPhase(QuestStatus.Success, "UNDER66_MQ07_GHOST", "d_underfortress_66", L("Talk to the Ruklys Army Officer's Spirit"), L("After you defeated the monsters, some spirit appeared. Talk with the Ruklys Army Officer's Spirit."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_66_MQ060_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50059, QuestStatus.Completed));

		AddObjective("killMonarch", L("Defeat Specter Monarch"), new KillObjective(1, "boss_Spector_m_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard10", 3));
		AddReward(new TakeItemReward("UNDER66_MQ6_ITEM01"));
	}
}

// 50061: Hidden Area
//-----------------------------------------------------------------------------
public class Underfortress66Mq070Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50061);
		SetName(L("Hidden Area"));
		SetDescription(L("The officer's spirit points at a room behind the camp that nobody has looked into."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_66", "d_underfortress_65");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER66_MQ07_GHOST", "d_underfortress_66", L("Talk to the Ruklys Army Officer's Spirit"), L("A strange spirit appeared. Talk with the Ruklys Army Officer's Spirit."));
		SetPhase(QuestStatus.InProgress, "UNDER66_MQ7_TRIGGER", "d_underfortress_65", L("Search for the hidden place"), L("Use the portal to move to the secret location and investigate the place."));
		SetPhase(QuestStatus.Success, "AMANDA_65_4", "d_underfortress_65", L("Search for the hidden place"), L("Amanda has what the room was holding."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_66_MQ070_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(50060, QuestStatus.Completed));

		AddObjective("searchTheRoom", L("Search for the hidden place"), new ManualObjective());
	}
}
