//--- Melia Script ----------------------------------------------------------
// Sentry Bailey Quest NPCs
//--- Description -----------------------------------------------------------
// Amanda working her way in past the Royal Army guards, and the Resounding
// Bombs she builds to walk them off their posts.
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

public class DUnderfortress65QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq010 = new QuestId(50048);
	private readonly static QuestId Mq020 = new QuestId(50049);
	private readonly static QuestId Mq030 = new QuestId(50050);
	private readonly static QuestId Mq040 = new QuestId(50051);
	private readonly static QuestId Mq050 = new QuestId(50052);

	private const int HerbsNeeded = 8;
	private const int BombsNeeded = 3;

	// The Thunderous Herb of the base camp's lower floor.
	private readonly static double[,] Herbs =
	{
		{ 392.15, -1166.11 }, { 209.34, -1027.21 }, { -168.58, -1220.92 },
		{ -182.00, -742.49 }, { 139.58, -1239.39 }, { 11.47, -982.94 },
	};

	private readonly static double[] HerbFacings = { 90, 78, 90, 90, 90, 90 };

	// The Royal Army guards carrying more bombs than they can account for.
	private readonly static string[] GuardNames =
	{
		"KINGDOM_GUARDIAN01", "KINGDOM_GUARDIAN02", "KINGDOM_GUARDIAN03",
		"KINGDOM_GUARDIAN04", "KINGDOM_GUARDIAN05",
	};

	private readonly static double[,] GuardSpots =
	{
		{ 458.24, -709.41 }, { 965.44, -746.93 }, { -22.83, -446.51 },
		{ -649.73, -107.09 }, { 550.09, -1030.85 },
	};

	// The two spots Amanda wants the Resounding Bombs set at.
	private readonly static string[] BombSpotNames = { "SETUP_BOOM01", "SETUP_BOOM02" };

	private readonly static double[,] BombSpots =
	{
		{ -664.26, 94.06 }, { 1164.70, -702.01 },
	};

	private readonly static double[,] SetBombSpots =
	{
		{ -660.97, 94.89 }, { 1166.35, -703.43 },
	};

	protected override void Load()
	{
		// Amanda, at the fortress gate
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_65_1", "d_underfortress_65", 695.99, -1597.09, 197, this.IsAmandaAtTheGate, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq010) && character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("There are too many Royal Army guards to go further inside."));
				await dialog.CompleteQuest(Mq010);
				return;
			}

			if (character.Quests.IsActive(Mq020) && character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("Take those down to me. I have moved to the lower side of the Upper Story Hallway."));
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("Wait a minute."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("The Knights of Kaliss told us that the important thing is located deep inside the fortress."),
					Option(L("Look around with the Monocle"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);
					character.Quests.CompleteObjective(Mq010, "lookAround");
					await dialog.Msg(L("Hmm... there's nothing special."));
					await dialog.Msg(L("Instead, there are many Royal Army guards."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				var answer = await dialog.SelectQuestOffer(Mq020, L("We should lure the Royal Army guards in order to go deeper inside. Hmm..."),
					Option(L("Do you have any plans?"), "accept"),
					Option(L("The Resounding Bomb may be dangerous"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq020);
					await dialog.Msg(L("Resounding Bombs make really loud sounds although their explosiveness isn't that great."));
					await dialog.Msg(L("Get me the herbs from the lower floor at the base camp, and bring them to the Upper Story Hallway."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq020))
			{
				await dialog.Msg(L("The Royal Army guards are on patrol so you should be careful all the time."));
				return;
			}

			await dialog.Msg(L("A grave robber counting guards where she expected to count treasure."));
		});

		// Amanda, at the Upper Story Hallway
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_65_2", "d_underfortress_65", -537.05, -693.95, 30, this.IsAmandaAtTheHallway, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq020) && character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("You brought them well without getting detected by the Royal Army guards."));
				await dialog.Msg(L("I am sorry. When they get hot, they make loud sounds."));
				await dialog.CompleteQuest(Mq020);
				return;
			}

			if (character.Quests.IsActive(Mq030) && character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Good. It's not like they've been detected, right?"));
				await dialog.Msg(L("I will make the Resounding Bombs so wait a bit. If this works out, we can move deeper inside."));
				await dialog.CompleteQuest(Mq030);
				return;
			}

			if (character.Quests.IsActive(Mq040) && character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("Have you set all of them?"));
				await dialog.Msg(L("Let's wait until they explode."));
				await dialog.CompleteQuest(Mq040);
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				await dialog.Msg(L("Okay. Shall we start the work of the grave robbers?"));

				var answer = await dialog.SelectQuestOffer(Mq030, L("I saw the Royal Army guards have more bombs than they can handle. You know what that means, right?"),
					Option(L("I will go steal the bombs"), "accept"),
					Option(L("I'll do it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq030);
					await dialog.Msg(L("I will get ready to make Resounding Bombs."));
					await dialog.Msg(L("Just knock them out and bring the bombs. Okay?"));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				var answer = await dialog.SelectQuestOffer(Mq040, L("We've completed the Resounding Bombs. If these detonate well, the Royal Army guards will move to the locations where the sounds came from."),
					Option(L("I will come back after setting it"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq040);
					character.Inventory.Add(ItemId.UNDERFORTRESS65_MQ04_BOOM, 2, InventoryAddType.PickUp);
					await dialog.Msg(L("Here are the Resounding Bombs. I've made them quickly so I can't guarantee their stability."));
					await dialog.Msg(L("Please set them well at the hallway on the upper floor and the lower side of the gathering place."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				var answer = await dialog.SelectQuestOffer(Mq050, L("The Resounding Bombs should be exploding soon. Cover your ears since they make a loud boom when they explode."),
					Option(L("Wait with your ears covered"), "accept"),
					Option(L("I have something to do"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq050);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				await dialog.Msg(L("Even if you get caught by the Royal Army guards, don't tell them about me."));
				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				await dialog.Msg(L("You should be careful since I can't guarantee their stability..."));
				return;
			}

			if (character.Quests.IsActive(Mq050))
			{
				await dialog.Msg(L("We don't have time to waste. Let's hurry!"));
				character.Quests.ReplayQuestTrack(Mq050);
				return;
			}

			await dialog.Msg(L("A grave robber packing herbs into bomb casings in a corridor full of guards."));
		});

		// Amanda, past the guard line
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_65_3", "d_underfortress_65", 582.72, 170.61, -32, this.IsAmandaPastTheLine, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq050) && character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("Phew..."));
				await dialog.Msg(L("I guess the Royal Army guards won't be able to chase after us here."));
				await dialog.CompleteQuest(Mq050);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("A grave robber out of sight of the guard line and rather pleased about it."));
		});

		// Amanda, at the way down
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_65_4", "d_underfortress_65", -350.48, 760.00, 90, this.IsAmandaAtTheWayDown, async dialog =>
		{
			await dialog.Msg(L("The way down to the Drill Ground of Confliction is clear. Go when you are ready."));
		});

		// Royal Army Guards
		//-------------------------------------------------------------------------
		for (var i = 0; i < GuardSpots.GetLength(0); ++i)
		{
			AddNpc(103010, L("Royal Army Guard"), GuardNames[i], "d_underfortress_65",
				GuardSpots[i, 0], GuardSpots[i, 1], 90, this.RobTheGuard);
		}

		// Thunderous Herb
		//-------------------------------------------------------------------------
		for (var i = 0; i < Herbs.GetLength(0); ++i)
		{
			AddNpc(47201, L("Thunderous Herb"), i == 0 ? "GLASS_MATERIAL" : "GLASS_MATERIAL_" + (i + 1), "d_underfortress_65",
				Herbs[i, 0], Herbs[i, 1], HerbFacings[i], this.PickThunderousHerb);
		}

		// The spots the Resounding Bombs go
		//-------------------------------------------------------------------------
		for (var i = 0; i < BombSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(40095, L("Bomb Site"), BombSpotNames[i], "d_underfortress_65",
				BombSpots[i, 0], BombSpots[i, 1], 90, this.IsBombSiteEmpty, async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Bomb Site"));

				if (character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040))
				{
					var set = await character.TimeActions.StartAsync(L("Setting the Resounding Bomb..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

					if (set != TimeActionResult.Completed)
						return;

					character.Inventory.RemoveItem(ItemId.UNDERFORTRESS65_MQ04_BOOM, 1);
					character.Quests.CompleteObjective(Mq040, "setBomb" + number);
					character.LookAround();
					character.ServerMessage(L("The Resounding Bomb is set."));
					return;
				}

				await dialog.Msg(L("A corner out of the guards' sight, which is what Amanda asked for."));
			});
		}

		for (var i = 0; i < SetBombSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(147459, L("Resounding Bomb"), "UNDER_65_SOUND_BOMB0" + number, "d_underfortress_65",
				SetBombSpots[i, 0], SetBombSpots[i, 1], 90, character => this.IsBombSet(character, number), async dialog =>
			{
				await dialog.Msg(L("A Resounding Bomb, packed in a hurry and not to be trusted."));
			});
		}
	}

	/// <summary>
	/// Returns whether Amanda is still waiting at the fortress gate.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheGate(Character character)
		=> !character.Quests.HasCompleted(Mq020);

	/// <summary>
	/// Returns whether Amanda has moved up to the Upper Story Hallway.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheHallway(Character character)
		=> character.Quests.HasCompleted(Mq010) && !character.Quests.IsCompletable(Mq050) && !character.Quests.HasCompleted(Mq050);

	/// <summary>
	/// Returns whether Amanda has got past the guard line.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaPastTheLine(Character character)
		=> character.Quests.IsCompletable(Mq050) && !character.Quests.HasCompleted(Mq050);

	/// <summary>
	/// Returns whether Amanda has moved on to the way down.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheWayDown(Character character)
		=> character.Quests.HasCompleted(Mq050);

	/// <summary>
	/// Returns whether a bomb site is still waiting for its bomb.
	/// </summary>
	/// <param name="character"></param>
	private bool IsBombSiteEmpty(Character character)
		=> character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040);

	/// <summary>
	/// Returns whether the numbered Resounding Bomb has been set and has not
	/// gone off yet.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="number"></param>
	private bool IsBombSet(Character character, int number)
	{
		if (character.Quests.HasCompleted(Mq050))
			return false;

		if (!character.Quests.Has(Mq040))
			return false;

		return !character.Quests.IsActive(Mq040, "setBomb" + number);
	}

	/// <summary>
	/// Knocks a Royal Army guard out and takes what he is carrying.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task RobTheGuard(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Royal Army Guard"));

		if (!character.Quests.IsActive(Mq030))
		{
			await dialog.Msg(L("A Royal Army guard on a post he has stood on for too long to be careful about it."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.UNDERFORTRESS65_MQ03_DRUG) >= BombsNeeded)
		{
			await dialog.Msg(L("You have as many bombs as Amanda asked for."));
			return;
		}

		var robbed = await character.TimeActions.StartAsync(L("Taking the bombs off the guard..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

		if (robbed != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.UNDERFORTRESS65_MQ03_DRUG, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("He goes down quietly, and his pack comes away with a bomb in it."));
	}

	/// <summary>
	/// Picks the Thunderous Herb the Resounding Bombs are packed with.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task PickThunderousHerb(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Thunderous Herb"));

		if (!character.Quests.IsActive(Mq020))
		{
			await dialog.Msg(L("A stand of herb growing on the lower floor, where nothing else does."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.UNDERFORTRESS65_MQ02_MATERIAL) >= HerbsNeeded)
		{
			await dialog.Msg(L("You have as much of the herb as Amanda asked for."));
			return;
		}

		var picked = await character.TimeActions.StartAsync(L("Picking the herb..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

		if (picked != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.UNDERFORTRESS65_MQ02_MATERIAL, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("The stems crack when they are cut, loudly enough to make you look round."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50048: Sentry Bailey
//-----------------------------------------------------------------------------
public class Underfortress65Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50048);
		SetName(L("Sentry Bailey"));
		SetDescription(L("Amanda's Monocle finds nothing at the fortress gate but Royal Army guards."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_65");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_65_1", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("You've entered the Fortress of the Land safely. Talk to Amanda who is waiting for you."));
		SetPhase(QuestStatus.InProgress, "AMANDA_65_1", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("You've entered the Fortress of the Land safely. Talk to Amanda who is waiting for you."));
		SetPhase(QuestStatus.Success, "AMANDA_65_1", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("You've entered the Fortress of the Land safely. Talk to Amanda who is waiting for you."));

		AddPrerequisite(new QuestStatusPrerequisite(8857, QuestStatus.Completed));

		AddObjective("lookAround", L("Talk to Grave Robber Amanda"), new ManualObjective());
	}
}

// 50049: Drawing Attention (1)
//-----------------------------------------------------------------------------
public class Underfortress65Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50049);
		SetName(L("Drawing Attention (1)"));
		SetDescription(L("A Resounding Bomb starts with the herb that grows on the base camp's lower floor."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_65");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_65_1", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("You need to discuss with Amanda how to pass through the heavy security. Talk to Amanda."));
		SetPhase(QuestStatus.InProgress, "GLASS_MATERIAL", "d_underfortress_65", L("Collect the herbs"), L("Amanda asked you to make Resounding Bombs in order to change the attention of the guards. Obtain some herbs from the lower floor at the base camp."));
		SetPhase(QuestStatus.Success, "AMANDA_65_2", "d_underfortress_65", L("Hand them over to the Grave Robber Amanda who is at the lower side of Upper Story Hallway"), L("Acquired the herbs which are the materials to make Resounding Bombs. Hand them over to Amanda."));

		AddPrerequisite(new QuestStatusPrerequisite(50048, QuestStatus.Completed));

		AddObjective("collectHerbs", L("Collect the Thunderous Herb"), new CollectItemObjective("UNDERFORTRESS65_MQ02_MATERIAL", 8));

		AddReward(new ItemReward("expCard10", 2));
		AddReward(new TakeItemReward("UNDERFORTRESS65_MQ02_MATERIAL", 8));
	}
}

// 50050: Drawing Attention (2)
//-----------------------------------------------------------------------------
public class Underfortress65Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50050);
		SetName(L("Drawing Attention (2)"));
		SetDescription(L("The guards are carrying more bombs than anyone is counting."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_65");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_65_2", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("Amanda is trying to disturb the soldiers by making Resounding Bombs. Ask the next step of the Grave Robber Amanda."));
		SetPhase(QuestStatus.InProgress, "KINGDOM_GUARDIAN01", "d_underfortress_65", L("Steal the bombs from the guards"), L("The Royal Army guards possess bombs, which are the next materials to make Resounding Bombs. Knock the Royal Army guards out and steal the bombs."));
		SetPhase(QuestStatus.Success, "AMANDA_65_2", "d_underfortress_65", L("Hand them over to the Grave Robber Amanda"), L("You've stolen enough bombs. Hand them over to Amanda."));

		AddPrerequisite(new QuestStatusPrerequisite(50049, QuestStatus.Completed));

		AddObjective("stealBombs", L("Steal the bombs from the guards"), new CollectItemObjective("UNDERFORTRESS65_MQ03_DRUG", 3));

		AddReward(new ItemReward("expCard10", 3));
		AddReward(new TakeItemReward("UNDERFORTRESS65_MQ03_DRUG", 3));
	}
}

// 50051: Drawing Attention (3)
//-----------------------------------------------------------------------------
public class Underfortress65Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50051);
		SetName(L("Drawing Attention (3)"));
		SetDescription(L("The bombs go at the upper hallway and the lower side of the gathering place."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_65");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_65_2", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("Talk with Grave Robber Amanda."));
		SetPhase(QuestStatus.InProgress, "SETUP_BOOM01", "d_underfortress_65", L("Setting the bomb"), L("To disturb the guards, set the Resounding Bombs created by Amanda."));
		SetPhase(QuestStatus.Success, "AMANDA_65_2", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("You've set the Resounding Bomb. Talk to Amanda."));

		AddPrerequisite(new QuestStatusPrerequisite(50050, QuestStatus.Completed));

		AddObjective("setBomb1", L("Set the bomb at the Upper Story Hallway"), new ManualObjective());
		AddObjective("setBomb2", L("Set the bomb below the gathering place"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 2));
	}
}

// 50052: Drawing Attention (4)
//-----------------------------------------------------------------------------
public class Underfortress65Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50052);
		SetName(L("Drawing Attention (4)"));
		SetDescription(L("The bombs go off, the guard line walks off its posts, and the way in is open."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_65");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_65_2", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("You are ready to detonate the Resounding Bomb. Talk to Amanda."));
		SetPhase(QuestStatus.InProgress, "AMANDA_65_3", "d_underfortress_65", L("Move to Drill Ground of Confliction with Amanda"), L("The Resounding Bomb has exploded. Before the Royal Army guards come back, move to the next area."));
		SetPhase(QuestStatus.Success, "AMANDA_65_3", "d_underfortress_65", L("Talk to Grave Robber Amanda"), L("It seems that you avoided being detected by the guards. Talk with Amanda."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_65_MQ050_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50051, QuestStatus.Completed));

		AddObjective("slipPast", L("Move to Drill Ground of Confliction with Amanda"), new ManualObjective());

		AddReward(new ItemReward("expCard10", 3));
	}
}
