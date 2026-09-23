//--- Melia Script ----------------------------------------------------------
// Storage Quarter Quest NPCs
//--- Description -----------------------------------------------------------
// The Restraint Token the Old Manager builds out of the quarter, the Ruklys
// spirits it holds down, and what the keeper turns out to be.
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

public class DUnderfortress68QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq010 = new QuestId(50072);
	private readonly static QuestId Mq020 = new QuestId(50073);
	private readonly static QuestId Mq030 = new QuestId(50074);
	private readonly static QuestId Mq040 = new QuestId(50075);
	private readonly static QuestId Mq050 = new QuestId(50076);
	private readonly static QuestId Mq060 = new QuestId(50088);
	private readonly static QuestId Mq070 = new QuestId(50089);

	private const int EggsNeeded = 5;
	private const int SpiritsToBring = 3;

	// The Green Infroholder Eggs of the quarter's lower halls.
	private readonly static double[,] Eggs =
	{
		{ -1228.44, -932.39 }, { -1299.04, -1348.83 }, { -1313.99, -1855.14 }, { -1305.63, -1533.18 },
		{ -1545.59, -965.50 }, { -1635.71, -1221.10 }, { -1530.75, -1487.95 },
	};

	private readonly static double[] EggFacings = { 90, -4, 90, 146, 90, 90, 90 };

	// The Ruklys squad members still walking the quarter.
	private readonly static double[,] Spirits =
	{
		{ 35.24, -642.28 }, { 194.60, -722.53 }, { 742.56, -678.13 },
		{ -642.83, -939.66 }, { -987.38, -1044.30 }, { 411.34, -612.55 },
	};

	protected override void Load()
	{
		// The Old Manager
		//-------------------------------------------------------------------------
		AddConditionalNpc(153139, L("Old Manager"), "EMINENT_68_1", "d_underfortress_68", -151.65, -1001.35, 90, this.IsManagerAtTheQuarter, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Manager"));
			dialog.SetPortrait("Dlg_port_Premier_Eminent");

			if (character.Quests.IsActive(Mq010) && character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("The eggs of this green infroholder represent the power to bind the souls."));
				await dialog.Msg(L("Now, we need to substantiate it."));
				await dialog.CompleteQuest(Mq010);
				return;
			}

			if (character.Quests.IsActive(Mq020) && character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("Good, good. I will make an orb."));
				await dialog.Msg(L("One final step remains before the Restraint Token."));
				await dialog.CompleteQuest(Mq020);
				return;
			}

			if (character.Quests.IsActive(Mq030) && character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Good. That's the Restraint Token."));
				await dialog.CompleteQuest(Mq030);
				return;
			}

			if (character.Quests.IsActive(Mq040) && character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("They say they destroyed something on Ruklys' command."));
				await dialog.Msg(L("I suspect that something might be related to the revelation of the goddess."));
				await dialog.CompleteQuest(Mq040);
				return;
			}

			if (character.Quests.IsActive(Mq050) && character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("Finally..."));
				await dialog.Msg(L("I've been looking for it for 600 years and finally... I found the revelation."));
				await dialog.CompleteQuest(Mq050);
				return;
			}

			if (character.Quests.IsActive(Mq060) && character.Quests.IsCompletable(Mq060))
			{
				await dialog.Msg(L("It's up to you to believe this story, but look at me in front of you."));
				await dialog.Msg(L("I've been living with the blessing from the goddesses for the last 600 years."));
				await dialog.CompleteQuest(Mq060);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("For all my life, I have been guarding this place but my struggle bore no fruit on where the revelation lies."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("However, if we could consult with the spirit who has lived the era when Ruklys walked, we might get some answer."),
					Option(L("How do you make a symbol of restraint?"), "accept"),
					Option(L("We need to find another way"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);
					await dialog.Msg(L("It is very complex to make the Restraint Token."));
					await dialog.Msg(L("We've failed before, but since we have you this time, it will be much easier."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				var answer = await dialog.SelectQuestOffer(Mq020, L("Nothing works better than to use Demon Bone. Get me the bone and I will extract the power and inject it into the bone."),
					Option(L("I will go get some demon bones"), "accept"),
					Option(L("Give me a second"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq020);
					await dialog.Msg(L("Even I failed to pull out the memories of the spirits."));
					await dialog.Msg(L("I feel more confident since you are with me."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				var answer = await dialog.SelectQuestOffer(Mq030, L("Fill this orb with the life force of the monsters. That will complete the whole process of restraining and actualizing the energy."),
					Option(L("I'll collect it"), "accept"),
					Option(L("I'll do it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq030);
					character.Inventory.Add(ItemId.UNDER68_MQ3_ITEM01, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Don't think of it as unpleasant."));
					await dialog.Msg(L("This is the best I can do for now."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				var answer = await dialog.SelectQuestOffer(Mq040, L("As I told you before, I've tried to look at the memories of the spirits. The resistance from the spirits was too aggressive then, so I failed."),
					Option(L("I will bring the soul"), "accept"),
					Option(L("I won't do it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq040);
					character.Inventory.Add(ItemId.UNDER68_MQ4_ITEM01, 1, InventoryAddType.PickUp);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				var answer = await dialog.SelectQuestOffer(Mq050, L("We must find out what the device was that Ruklys destroyed. Thanks to you, I can now delve deeper into their memories."),
					Option(L("I will bring the spirits"), "accept"),
					Option(L("Stop! They are in pain!"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq050);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq060) && character.Quests.MeetsPrerequisites(Mq060))
			{
				var answer = await dialog.SelectQuestOffer(Mq060, L("No need to keep it secret anymore. Let me introduce myself properly. I am Premier Eminent, who served King Kadumel 600 years ago."),
					Option(L("What happened?"), "accept"),
					Option(L("Skip the explanation"), "leave")
				);

				if (answer != "accept")
					return;

				var asked = await character.TimeActions.StartAsync(L("Asking what he means..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));

				if (asked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq060);
				character.Quests.CompleteObjective(Mq060, "hearTheTruth");
				await dialog.Msg(L("I am sure that you know this place is the last battlefield where Ruklys resisted until his death."));
				await dialog.Msg(L("And Ruklys was also the guardian of the revelation like his master, Maven."));
				return;
			}

			if (character.Quests.IsActive(Mq010))
			{
				await dialog.Msg(L("The spirits are left with nothing but rage."));
				await dialog.Msg(L("Guilt is nowhere to be felt."));
				return;
			}

			if (character.Quests.IsActive(Mq020))
			{
				await dialog.Msg(L("Even I failed to pull out the memories of the spirits."));
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				await dialog.Msg(L("Knowing how to operate it is different from being able to actually operate it."));
				await dialog.Msg(L("Those spirits took the secret to their graves and beyond."));
				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				await dialog.Msg(L("Please bring the spirits fast."));
				await dialog.Msg(L("I am also curious."));
				return;
			}

			if (character.Quests.IsActive(Mq050))
			{
				await dialog.Msg(L("I have no choice even if the spirits suffer."));
				await dialog.Msg(L("That's the best way I can help you."));
				return;
			}

			await dialog.Msg(L("A keeper who has been in this quarter for longer than anyone alive can account for."));
		});

		// Amanda, at the quarter
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_68_1", "d_underfortress_68", -494.31, -946.35, 139, this.IsAmandaAtTheQuarter, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (!character.Quests.Has(Mq070) && character.Quests.MeetsPrerequisites(Mq070))
			{
				await dialog.Msg(L("Thank heavens, you are still alive."));

				var answer = await dialog.SelectQuestOffer(Mq070, L("Remember the time when I was looking around through the monocle?"),
					Option(L("But that's absurd!"), "accept"),
					Option(L("About other suspicious things"), "explain"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("The whole shenanigan of making the certificate."));
					await dialog.Msg(L("What did he say? \"Overwhelm my withered body\", my behind!"));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq070);
					character.LookAround();
					await dialog.Msg(L("I pray I am wrong."));
					await dialog.Msg(L("It is best if you return to the man called Eminent. It's no good if he suspects you."));
					return;
				}
				return;
			}

			await dialog.Msg(L("A grave robber who has stopped believing a word the keeper says."));
		});

		// Amanda, at the battlefield approach
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_68_2", "d_underfortress_68", 2197.11, -448.80, 90, this.IsAmandaAtTheBattlefield, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq070) && character.Quests.IsCompletable(Mq070))
			{
				await dialog.Msg(L("Are you telling me that he was with the demons?"));
				await dialog.Msg(L("And they were not attacking him at all?"));
				await dialog.CompleteQuest(Mq070);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq070))
			{
				await dialog.Msg(L("Go and see for yourself. I will wait here."));
				character.Quests.ReplayQuestTrack(Mq070);
				return;
			}

			await dialog.Msg(L("A grave robber waiting short of the battlefield with her Monocle out."));
		});

		// Platform of Confessions
		//-------------------------------------------------------------------------
		AddNpc(147503, L("Platform of Confessions"), "UNDER68_DEVICE01", "d_underfortress_68", -107.12, -978.67, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Platform of Confessions"));

			if (character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("The platform is ready. Bring the spirits to it with the Restraint Token."));
				return;
			}

			if (character.Quests.IsActive(Mq050) && !character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("The platform will hold a spirit for a second reading. Bring one back."));
				return;
			}

			await dialog.Msg(L("A gear of the fortress' own spellwork, set up to hold a soul still."));
		});

		// Green Infroholder Eggs
		//-------------------------------------------------------------------------
		for (var i = 0; i < Eggs.GetLength(0); ++i)
		{
			AddNpc(41327, L("Green Infroholder Egg"), i == 0 ? "VELNIAS_PLANT" : "VELNIAS_PLANT_" + (i + 1), "d_underfortress_68",
				Eggs[i, 0], Eggs[i, 1], EggFacings[i], this.TakeInfroholderEgg);
		}

		// Ruklys' Squad Member Spirits
		//-------------------------------------------------------------------------
		for (var i = 0; i < Spirits.GetLength(0); ++i)
		{
			AddNpc(103015, L("Ruklys' Squad Member Spirit"), i == 0 ? "UNDER68_GHOST" : "UNDER68_GHOST_" + (i + 1), "d_underfortress_68",
				Spirits[i, 0], Spirits[i, 1], 90, this.RestrainSpirit);
		}

		// Owl Sculpture
		//-------------------------------------------------------------------------
		AddNpc(48004, L("Owl Sculpture"), "UNDER68_SQ2_OWL", "d_underfortress_68", 715.72, -470.96, 15, async dialog =>
		{
			dialog.SetTitle(L("Owl Sculpture"));

			await dialog.Msg(L("An owl cut out of the quarter's own stone, facing a wall rather than a door."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The battlefield approach, where the keeper is seen giving orders.
		AddQuestTrigger("UNDER68_MQ7_TRACK", "d_underfortress_68", 2521.92, -104.35, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq070) && !character.Quests.IsCompletable(Mq070))
				character.Quests.StartQuestTrack(Mq070);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether the Old Manager is still keeping his name to himself.
	/// </summary>
	/// <param name="character"></param>
	private bool IsManagerAtTheQuarter(Character character)
		=> !character.Quests.HasCompleted(Mq060);

	/// <summary>
	/// Returns whether Amanda has caught up in the quarter.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheQuarter(Character character)
		=> character.Quests.HasCompleted(Mq060) && !character.Quests.Has(Mq070);

	/// <summary>
	/// Returns whether Amanda has moved up to the battlefield approach.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheBattlefield(Character character)
		=> character.Quests.Has(Mq070);

	/// <summary>
	/// Takes one of the Green Infroholder Eggs the token is bound with.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task TakeInfroholderEgg(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Green Infroholder Egg"));

		if (!character.Quests.IsActive(Mq010))
		{
			await dialog.Msg(L("An Infroholder egg, and whatever laid it is not far off."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.UNDER68_MQ1_ITEM01) >= EggsNeeded)
		{
			await dialog.Msg(L("You have as many eggs as the keeper asked for."));
			return;
		}

		var taken = await character.TimeActions.StartAsync(L("Taking the egg..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

		if (taken != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.UNDER68_MQ1_ITEM01, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("The shell holds, and the weight of it pulls against your hand."));
	}

	/// <summary>
	/// Binds one of the Ruklys spirits with the Restraint Token or fills the
	/// Absorption Orb on it.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task RestrainSpirit(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Ruklys' Squad Member Spirit"));

		if (character.Quests.IsActive(Mq030) && !character.Quests.IsCompletable(Mq030))
		{
			var drained = await character.TimeActions.StartAsync(L("Setting the Absorption Orb down..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

			if (drained != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(Mq030, "fillTheOrb");
			character.ServerMessage(L("The orb takes what it can hold. Bring it back to the keeper."));
			return;
		}

		if (character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040))
		{
			await this.BindSpirit(dialog, character, Mq040);
			return;
		}

		if (character.Quests.IsActive(Mq050) && !character.Quests.IsCompletable(Mq050))
		{
			await this.BindSpirit(dialog, character, Mq050);
			return;
		}

		await dialog.Msg(L("A soldier of Ruklys' squad, with nothing left of him but the rage."));
	}

	/// <summary>
	/// Binds one spirit for whichever reading the keeper is working on.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="character"></param>
	/// <param name="questId"></param>
	private async Task BindSpirit(Dialog dialog, Character character, QuestId questId)
	{
		var bound = await character.TimeActions.StartAsync(L("Binding the spirit..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

		if (bound != TimeActionResult.Completed)
			return;

		for (var i = 1; i <= SpiritsToBring; ++i)
		{
			if (character.Quests.IsActive(questId, "bindSpirit" + i))
			{
				character.Quests.CompleteObjective(questId, "bindSpirit" + i);
				character.ServerMessage(L("The Restraint Token holds the spirit. It will follow you to the platform."));
				return;
			}
		}

		await dialog.Msg(L("You are carrying as many spirits as the token will hold."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50072: The Past of the Spirits (1)
//-----------------------------------------------------------------------------
public class Underfortress68Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50072);
		SetName(L("The Past of the Spirits (1)"));
		SetDescription(L("A Restraint Token starts with the eggs of the quarter's Infroholders."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_68");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("You've met with the Old Manager. Ask the Old Manager what to do to get the revelation."));
		SetPhase(QuestStatus.InProgress, "VELNIAS_PLANT", "d_underfortress_68", L("Collect Green Infroholder Eggs"), L("To speak to the unruly Ruklys era spirits, collect Green Infroholder Eggs to make a token of restraint."));
		SetPhase(QuestStatus.Success, "EMINENT_68_1", "d_underfortress_68", L("Deliver to the Old Manager"), L("You've collected the Green Infroholder Eggs. Bring them to the Old Manager."));

		AddPrerequisite(new QuestStatusPrerequisite(50068, QuestStatus.Completed));

		AddObjective("collectEggs", L("Collect Green Infroholder Eggs"), new CollectItemObjective("UNDER68_MQ1_ITEM01", 5));

		AddReward(new ItemReward("expCard11", 2));
		AddReward(new TakeItemReward("UNDER68_MQ1_ITEM01", 5));
	}
}

// 50073: The Past of the Spirits (2)
//-----------------------------------------------------------------------------
public class Underfortress68Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50073);
		SetName(L("The Past of the Spirits (2)"));
		SetDescription(L("The token needs demon bone to carry what the eggs hold."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_68");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("The next ingredient is needed. Speak with the Old Manager again."));
		SetPhase(QuestStatus.InProgress, "EMINENT_68_1", "d_underfortress_68", L("Collect Demon Bones"), L("Defeat the demons and get their bones."));
		SetPhase(QuestStatus.Success, "EMINENT_68_1", "d_underfortress_68", L("Deliver to the Old Manager"), L("Acquired the demon bones. Take them to the Old Manager."));

		AddPrerequisite(new QuestStatusPrerequisite(50072, QuestStatus.Completed));

		AddObjective("collectBones", L("Collect Demon Bones"), new CollectItemObjective("UNDER68_MQ2_ITEM01", 10));

		AddPityDrop("UNDER68_MQ2_ITEM01", 0.8f, 3, 1, "Deadbornscab_red");

		AddReward(new ItemReward("expCard11", 2));
		AddReward(new TakeItemReward("UNDER68_MQ2_ITEM01"));
	}
}

// 50074: The Past of the Spirits (3)
//-----------------------------------------------------------------------------
public class Underfortress68Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50074);
		SetName(L("The Past of the Spirits (3)"));
		SetDescription(L("The Absorption Orb has to be filled off the quarter's own demons."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_68");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("You've completed the first step of making the Restraint Token. Ask the Old Manager about the second step."));
		SetPhase(QuestStatus.InProgress, "UNDER68_GHOST", "d_underfortress_68", L("Absorb the vitalities of demons with the Absorption Orb"), L("Place the Absorption Orb near the demons to suck them dry."));
		SetPhase(QuestStatus.Success, "EMINENT_68_1", "d_underfortress_68", L("Deliver to the Old Manager"), L("Filled the orb with the life force of demons. Take it back to the Old Manager."));

		AddPrerequisite(new QuestStatusPrerequisite(50073, QuestStatus.Completed));

		AddObjective("fillTheOrb", L("Absorb the vitalities of demons with the Absorption Orb"), new ManualObjective());

		AddReward(new ItemReward("expCard11", 2));
		AddReward(new TakeItemReward("UNDER68_MQ3_ITEM01", 1));
	}
}

// 50075: The Past of the Spirits (4)
//-----------------------------------------------------------------------------
public class Underfortress68Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50075);
		SetName(L("The Past of the Spirits (4)"));
		SetDescription(L("The token holds a Ruklys spirit still long enough for the keeper to read it."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_68");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("The Absorption Orb is complete. Talk to the Old Manager to read the memories of the spirits."));
		SetPhase(QuestStatus.InProgress, "UNDER68_GHOST", "d_underfortress_68", L("Bring the spirits after restraining them"), L("Restrain the spirits with the Restraint Token and take them to the Old Manager."));
		SetPhase(QuestStatus.Success, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("The Old Manager has read their memories. Speak with him."));

		AddPrerequisite(new QuestStatusPrerequisite(50074, QuestStatus.Completed));

		AddObjective("bindSpirit1", L("Bring the first restrained spirit"), new ManualObjective());
		AddObjective("bindSpirit2", L("Bring the second restrained spirit"), new ManualObjective());
		AddObjective("bindSpirit3", L("Bring the third restrained spirit"), new ManualObjective());

		AddReward(new ItemReward("expCard11", 3));
	}
}

// 50076: The Past of the Spirits (5)
//-----------------------------------------------------------------------------
public class Underfortress68Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50076);
		SetName(L("The Past of the Spirits (5)"));
		SetDescription(L("A second reading goes deeper, and the spirits pay for it."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_68");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("You are slowly putting pieces of the puzzle together. Speak with the Old Manager again."));
		SetPhase(QuestStatus.InProgress, "UNDER68_GHOST", "d_underfortress_68", L("Bring the restrained spirit with the Restraint Token"), L("Bring the spirit back again by using the Restraint Token."));
		SetPhase(QuestStatus.Success, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("The Old Manager has read their memories. Speak with him."));

		AddPrerequisite(new QuestStatusPrerequisite(50075, QuestStatus.Completed));

		AddObjective("bindSpirit1", L("Bring the first restrained spirit back"), new ManualObjective());
		AddObjective("bindSpirit2", L("Bring the second restrained spirit back"), new ManualObjective());
		AddObjective("bindSpirit3", L("Bring the third restrained spirit back"), new ManualObjective());

		AddReward(new ItemReward("expCard11", 3));
		AddReward(new TakeItemReward("UNDER68_MQ4_ITEM01", 1));
	}
}

// 50088: The Old Manager's Identity
//-----------------------------------------------------------------------------
public class Underfortress68Mq060Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50088);
		SetName(L("The Old Manager's Identity"));
		SetDescription(L("The keeper gives his name at last: Premier Eminent, who served King Kadumel."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_68");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("What you heard from the Old Manager is suspicious. Talk to the Old Manager."));
		SetPhase(QuestStatus.InProgress, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("What you heard from the Old Manager is suspicious. Talk to the Old Manager."));
		SetPhase(QuestStatus.Success, "EMINENT_68_1", "d_underfortress_68", L("Talk to the Old Manager"), L("What you heard from the Old Manager is suspicious. Talk to the Old Manager."));

		AddPrerequisite(new QuestStatusPrerequisite(50076, QuestStatus.Completed));

		AddObjective("hearTheTruth", L("Talk to the Old Manager"), new ManualObjective());
	}
}

// 50089: Doubt
//-----------------------------------------------------------------------------
public class Underfortress68Mq070Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50089);
		SetName(L("Doubt"));
		SetDescription(L("Premier Eminent is seen on the battlefield giving the demons their orders."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_68");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_68_1", "d_underfortress_68", L("Talk to Grave Robber Amanda"), L("The identity of the Old Manager was Premier Eminent, who lived 600 years ago. Return to Amanda."));
		SetPhase(QuestStatus.InProgress, "UNDER68_MQ7_TRACK", "d_underfortress_68", L("Move to the battlefield of the Fortress of the Land"), L("Amanda is having doubts about Premier Eminent. But first, meet up with Premier Eminent in the final battleground."));
		SetPhase(QuestStatus.Success, "AMANDA_68_2", "d_underfortress_68", L("Talk to Grave Robber Amanda"), L("You saw Premier Eminent talking so freely with the demons! Hurry, meet up with Amanda again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_68_MQ070_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50088, QuestStatus.Completed));

		AddObjective("seeTheOrders", L("Move to the battlefield of the Fortress of the Land"), new ManualObjective());
	}
}
