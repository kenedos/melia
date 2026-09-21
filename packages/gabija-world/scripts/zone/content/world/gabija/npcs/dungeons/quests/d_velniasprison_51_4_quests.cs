//--- Melia Script ----------------------------------------------------------
// Demon Prison District 4 Quest NPCs
//--- Description -----------------------------------------------------------
// Vakarine herself, the three Kupoles around her, and the Evening Star Key
// that takes the Chain of Reversion back off Dionys.
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

public class DVelniasprison514QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(60012);
	private readonly static QuestId Mq02 = new QuestId(60013);
	private readonly static QuestId Mq03 = new QuestId(60014);
	private readonly static QuestId Mq04 = new QuestId(60015);
	private readonly static QuestId Mq05 = new QuestId(60016);
	private readonly static QuestId Mq06 = new QuestId(60017);
	private readonly static QuestId Sq01 = new QuestId(60033);
	private readonly static QuestId Sq02 = new QuestId(60034);
	private readonly static QuestId Sq03 = new QuestId(60035);

	private const int StarMarksNeeded = 8;

	// The three seals the Evening Star Key is used on.
	private readonly static string[] SealNames = { "VPRISON514_MQ_04_NPC_01", "VPRISON514_MQ_04_NPC_02", "VPRISON514_MQ_04_NPC_03" };

	private readonly static double[,] SealSpots =
	{
		{ -2676.94, 50.13 }, { -3504.32, 796.90 }, { -2604.22, 1699.41 },
	};

	// The three small dimensional cracks of the Scars of Fighting Spirits.
	private readonly static string[] CrackNames = { "VPRISON514_MQ_02_NPC_01", "VPRISON514_MQ_02_NPC_02", "VPRISON514_MQ_02_NPC_03" };

	private readonly static double[,] CrackSpots =
	{
		{ -1587.59, -235.17 }, { -724.28, -1010.96 }, { 146.52, -198.94 },
	};

	private readonly static double[] CrackFacings = { 90, 90, -42 };

	// The pieces of the Mark of Star, scattered around the Rada Seal.
	private readonly static double[,] StarMarkSpots =
	{
		{ -2620.16, 1562.84 }, { -2837.52, 1664.68 }, { -2692.55, 1818.58 }, { -2540.00, 1867.81 },
		{ -2437.01, 1818.18 }, { -2429.32, 1691.62 }, { -2569.53, 1719.48 }, { -2509.65, 1542.90 },
		{ -2455.62, 1445.76 }, { -2692.72, 1463.63 }, { -3002.03, 1565.64 }, { -2926.56, 1595.19 },
	};

	protected override void Load()
	{
		// Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(154010, L("Goddess Vakarine"), "VPRISON514_MQ_VAKARINE", "d_velniasprison_51_4", -1018.74, 1133.27, -13, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Goddess Vakarine"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("The dimensional crack and the Demon Lords... they were inevitable."));
				await dialog.Msg(L("I could feel my power becoming weaker after Medzio Diena and I had to find some way."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("Savior. The time has come for you to know the truth."));
				await dialog.Msg(L("The power which I gave to Dionys is called the Chain of Reversion."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The goddess has not opened her eyes yet. The Kupoles are still gathering what is left of her."));
				character.Quests.ReplayQuestTrack(Mq01);
				return;
			}

			await dialog.Msg(L("The Evening Star, sitting in a prison because there was nowhere safer to put her."));
		});

		// Kupole Zydrone
		//-------------------------------------------------------------------------
		AddNpc(154015, L("Kupole Zydrone"), "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", -633.83, 283.19, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Zydrone"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("We can start the ritual now."));
				await dialog.Msg(L("When the Evening Star Key is completed, we could probably retrieve his ego."));
				await dialog.CompleteQuest(Mq02);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Here. Please give this key to Kupole Aldona."));
				await dialog.Msg(L("She must be holding down Dionys now. Please hurry."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("The Oruarma Cathedral is where the goddess recovers her powers."));
				await dialog.Msg(L("That is why it should always be safe from the demons. Thank you very much."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("Savior! I have been waiting for you."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("Vakarine says the Evening Star Key has to be finished before anything can be taken back off Dionys."),
					Option(L("I will help you to make the keys"), "accept"),
					Option(L("I need to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					character.LookAround();
					await dialog.Msg(L("First, close the small dimensional cracks in the Scars of Fighting Spirits."));
					await dialog.Msg(L("It will be difficult to perform the ritual with the waves of demons coming out from there."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Now go to Oruarma Cathedral. Protect me from the demons while I focus the goddess' power into the Evening Star Key."),
					Option(L("I will make sure to protect it"), "accept"),
					Option(L("Tell her that we should prepare little more"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("Valtross is defeated but his servants are still left in Oruarma Cathedral. I want you to make those demons pay for their sins."),
					Option(L("I will defeat the demons"), "accept"),
					Option(L("I don't have time for that"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("We, Kupoles are different. Even if the goddesses are not around, we don't get violent."));
					return;
				}
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Dionys is the guardian whom Vakarine most adores."));
				await dialog.Msg(L("She reluctantly sealed the strong power to Dionys and fell into the deep sadness..."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The demons will come the moment I begin. Stand where they have to come past you."));
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("We, Kupoles are different."));
				await dialog.Msg(L("Even if the goddesses are not around, we don't get violent."));
				return;
			}

			await dialog.Msg(L("A Kupole who has been holding the Corridor of Monitor open for the goddess to rest behind."));
		});

		// Kupole Aldona
		//-------------------------------------------------------------------------
		AddConditionalNpc(154014, L("Kupole Aldona"), "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", -1823.76, 571.61, 166, this.IsAldonaHoldingDionys, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Aldona"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("The power of the Evening Star Key is accumulating in the magic suppressor."));
				await dialog.Msg(L("We are all ready and there's plenty of time."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Hauberk took the power that was sealed in Dionys and ran."));
				await dialog.Msg(L("Every part of that was planned, and none of it was ours."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("This will be of big help to the weakened Dionys."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thanks."));
				await dialog.Msg(L("If Vakarine recovers, then Dionys should also recover."));
				await dialog.CompleteQuest(Sq03);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("You are not too late!"));

				var answer = await dialog.SelectQuestOffer(Mq04, L("Let's release the power of the Evening Star Key. The three seals of Rearda, Kasa and Rada hold it in."),
					Option(L("I will come back after unleashing the seal"), "accept"),
					Option(L("I don't think I can handle it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("We are going to detach the power from Dionys now. If we fail, it would be as if tens of Demon Lords were let loose."),
					Option(L("I am ready"), "accept"),
					Option(L("I will prepare a little more"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.LookAround();
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				var answer = await dialog.SelectQuestOffer(Mq06, L("Hauberk fled towards District 4, which is under Daiva's watch. First, you should return to Vakarine. I will look for Dionys."),
					Option(L("I will report about it"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					character.Quests.CompleteObjective(Mq06, "tellVakarine");
					await dialog.Msg(L("How could Vakarine let Hauberk go on a rampage like that..."));
					await dialog.Msg(L("Well, I guess she must have a reason for that?"));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("Dionys is not recovering well. I think getting back the claws that have his powers might help him recover."),
					Option(L("I will retrieve the claws"), "accept"),
					Option(L("About Dionys"), "explain"),
					Option(L("Tell her that will not be enough to cheer him up"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Dionys was an ordinary creation of the human world."));
					await dialog.Msg(L("The goddess took and cared for him when the demons cruelly toyed with him."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("There are many demons that ran away from Dionys' attacks near the Rearda Seal."));
					await dialog.Msg(L("Let's just hope that Dionys' claws are still stuck in them."));
					return;
				}
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("When Dionys lost his ego, the Rada Seal risked breaking down. Fortunately, he regained his consciousness before that, but he could not save the Mark of Star."),
					Option(L("I will collect the Marks of Star"), "accept"),
					Option(L("I don't have time for that"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					character.LookAround();
					await dialog.Msg(L("Dionys' power is astonishing."));
					await dialog.Msg(L("Even the violent Baltrus were busy running away instead of touching him."));
					return;
				}
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Dionys is not someone we can face against."));
				await dialog.Msg(L("Even Vakarine in her old days would not be able to do anything."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Dionys is still standing. Get him down and I will take the chain off him."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("How could Vakarine let Hauberk go on a rampage like that..."));
				await dialog.Msg(L("Well, I guess she must have a reason for that?"));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Once Dionys gets back his strength, Vakarine should also recover."));
				await dialog.Msg(L("Aside from that, Dionys has other roles that are indispensable."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Dionys' power is astonishing."));
				await dialog.Msg(L("Even the violent Baltrus were busy running away instead of touching him."));
				return;
			}

			await dialog.Msg(L("A Kupole who has spent the whole siege standing between Dionys and everyone else."));
		});

		// Small Dimensional Cracks
		//-------------------------------------------------------------------------
		for (var i = 0; i < CrackSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(20025, L("Small Dimensional Crack"), CrackNames[i], "d_velniasprison_51_4",
				CrackSpots[i, 0], CrackSpots[i, 1], CrackFacings[i], this.AreCracksOpen, async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Small Dimensional Crack"));

				if (!character.Quests.IsActive(Mq02))
				{
					await dialog.Msg(L("A tear in the air, no wider than a hand, and something moving on the other side of it."));
					return;
				}

				var closed = await character.TimeActions.StartAsync(L("Closing the dimensional crack..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

				if (closed != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq02, "closeCrack" + number);
				character.ServerMessage(L("The crack closes on itself."));
			});
		}

		// The seals of Rearda, Kasa and Rada
		//-------------------------------------------------------------------------
		for (var i = 0; i < SealSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(154009, this.SealName(number), SealNames[i], "d_velniasprison_51_4",
				SealSpots[i, 0], SealSpots[i, 1], 90, async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(this.SealName(number));

				if (!character.Quests.IsActive(Mq04))
				{
					await dialog.Msg(L("A crystal of the goddess, holding shut what was put behind it."));
					return;
				}

				var released = await character.TimeActions.StartAsync(L("Turning the Evening Star Key..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

				if (released != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq04, "releaseSeal" + number);
				character.ServerMessage(L("The seal gives, and the key takes what it held."));
			});
		}

		// Pieces of the Mark of Star
		//-------------------------------------------------------------------------
		for (var i = 0; i < StarMarkSpots.GetLength(0); ++i)
		{
			AddConditionalNpc(20025, L("Mark of Star"), i == 0 ? "VPRISON514_SQ_03_NPC" : "VPRISON514_SQ_03_NPC_" + (i + 1), "d_velniasprison_51_4",
				StarMarkSpots[i, 0], StarMarkSpots[i, 1], 90, this.AreStarMarksScattered, this.PickStarMark);
		}

		// The magic suppressor that holds Dionys
		//-------------------------------------------------------------------------
		AddConditionalNpc(154001, L("Magic Suppressor"), "VPRISON514_MQ_05_NPC", "d_velniasprison_51_4", -2254.20, 811.22, 181, this.IsSuppressorStanding, async dialog =>
		{
			await dialog.Msg(L("A suppressor of the goddess' making, filling with what the Evening Star Key took out of the seals."));
		});
	}

	/// <summary>
	/// Returns the display name of the numbered Vakarine seal.
	/// </summary>
	/// <param name="number"></param>
	private string SealName(int number)
	{
		if (number == 1)
			return L("Rearda Seal");

		if (number == 2)
			return L("Kasa Seal");

		return L("Rada Seal");
	}

	/// <summary>
	/// Returns whether Aldona has come out to the district.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAldonaHoldingDionys(Character character)
		=> character.Quests.IsActive(Mq01) || character.Quests.HasCompleted(Mq01);

	/// <summary>
	/// Returns whether the small dimensional cracks are still open.
	/// </summary>
	/// <param name="character"></param>
	private bool AreCracksOpen(Character character)
		=> character.Quests.IsActive(Mq02);

	/// <summary>
	/// Returns whether the pieces of the Mark of Star are still lying around
	/// the Rada Seal.
	/// </summary>
	/// <param name="character"></param>
	private bool AreStarMarksScattered(Character character)
		=> character.Quests.IsActive(Sq03);

	/// <summary>
	/// Returns whether the magic suppressor is still holding Dionys.
	/// </summary>
	/// <param name="character"></param>
	private bool IsSuppressorStanding(Character character)
		=> !character.Quests.Has(Mq05);

	/// <summary>
	/// Hands out the pieces of the Mark of Star.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task PickStarMark(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Mark of Star"));

		if (!character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("A shard of the Mark of Star, bright enough to pick out of the dust."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.VPRISON514_SQ_03_ITEM) >= StarMarksNeeded)
		{
			await dialog.Msg(L("You have as many pieces as Aldona asked for."));
			return;
		}

		character.Inventory.Add(ItemId.VPRISON514_SQ_03_ITEM, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("You lift the shard out of the floor of the seal."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60012: The Night Star
//-----------------------------------------------------------------------------
public class Vprison514Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60012);
		SetName(L("The Night Star"));
		SetDescription(L("Vakarine is waiting behind the Corridor of Monitor, and she cannot come out to meet anyone."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_01_NPC", "d_velniasprison_51_4", L("Meet Goddess Vakarine"), L("Goddess Vakarine is waiting for your help at the Corridor of Monitor in the Demon's Prison."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_MQ_01_NPC", "d_velniasprison_51_4", L("Meet Goddess Vakarine"), L("Goddess Vakarine is waiting for your help at the Corridor of Monitor in the Demon's Prison."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_VAKARINE", "d_velniasprison_51_4", L("Talk to Goddess Vakarine"), L("Talk to Goddess Vakarine."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON514_MQ_01_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60011, QuestStatus.Completed));

		AddObjective("meetVakarine", L("Meet Goddess Vakarine"), new ManualObjective());
	}
}

// 60013: The Evening Star Key (1)
//-----------------------------------------------------------------------------
public class Vprison514Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60013);
		SetName(L("The Evening Star Key (1)"));
		SetDescription(L("The cracks in the Scars of Fighting Spirits have to be shut before the key can be charged."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", L("Talk to Kupole Zydrone"), L("Vakarine says that you need to complete the Evening Star Key to get rid of the power controlling Dionys. Go to Kupole Zydrone to complete the key."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_MQ_02_NPC_01", "d_velniasprison_51_4", L("Remove Small Dimensional Crack"), L("Kupole Zydrone asked you to remove the small dimensional cracks that interferes with charging the Evening Star Key."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", L("Talk to Kupole Zydrone"), L("Removed all the dimensional cracks. Tell Kupole Zydrone about it."));

		AddPrerequisite(new QuestStatusPrerequisite(60012, QuestStatus.Completed));

		AddObjective("closeCrack1", L("Remove the first Small Dimensional Crack"), new ManualObjective());
		AddObjective("closeCrack2", L("Remove the second Small Dimensional Crack"), new ManualObjective());
		AddObjective("closeCrack3", L("Remove the third Small Dimensional Crack"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 60014: The Evening Star Key (2)
//-----------------------------------------------------------------------------
public class Vprison514Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60014);
		SetName(L("The Evening Star Key (2)"));
		SetDescription(L("Zydrone pours the goddess' power into the key at Oruarma Cathedral, and cannot defend herself while she does."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", L("Talk to Kupole Zydrone"), L("Ready to charge the Evening Star Key. Talk to Kupole Zydrone."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", L("Protect Kupole Zydrone"), L("Kupole Zydrone asked you to protect her while she puts divine power into the Evening Star Key."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", L("Talk to Kupole Zydrone"), L("Protected Kupole Zydrone well until the Evening Star Key was charged. Talk to Zydrone."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON514_MQ_03_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60013, QuestStatus.Completed));

		AddObjective("guardZydrone", L("Protect Kupole Zydrone"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new ItemReward("VPRISON514_MQ_04_ITEM", 1));
	}
}

// 60015: The Evening Star Key (3)
//-----------------------------------------------------------------------------
public class Vprison514Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60015);
		SetName(L("The Evening Star Key (3)"));
		SetDescription(L("The key is turned on the seals of Rearda, Kasa and Rada, and what they held goes into the suppressor."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Talk to Kupole Aldona"), L("Kupole Zydrone asked you to deliver the Evening Star Key to Kupole Aldona, who is holding down Dionys."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_MQ_04_NPC_01", "d_velniasprison_51_4", L("Release the Seals"), L("Use the Evening Star Key to release the seal of Rearda, Kasa, and Rada to free the power of the goddess."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Talk to Kupole Aldona"), L("You've unleashed the seals and released the power of the key. Go back to Kupole Aldona and talk with her."));

		AddPrerequisite(new QuestStatusPrerequisite(60014, QuestStatus.Completed));

		AddObjective("releaseSeal1", L("Release the Rearda Seal"), new ManualObjective());
		AddObjective("releaseSeal2", L("Release the Kasa Seal"), new ManualObjective());
		AddObjective("releaseSeal3", L("Release the Rada Seal"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("VPRISON514_MQ_04_ITEM"));
	}
}

// 60016: The Evening Star at Night
//-----------------------------------------------------------------------------
public class Vprison514Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60016);
		SetName(L("The Evening Star at Night"));
		SetDescription(L("Dionys is put down so Aldona can take the Chain of Reversion off him, and Hauberk takes it instead."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Talk to Kupole Aldona"), L("Ready to remove the strong power from Dionys. Talk to Kupole Aldona."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Defeat Dionys"), L("Subdue Dionys for Kupole Aldona to remove the power from Dionys."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Talk to Kupole Aldona"), L("Hauberk took the strong power sealed in Dionys and ran away. Talk to Aldona."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON514_MQ_05_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60015, QuestStatus.Completed));

		AddObjective("subdueDionys", L("Defeat Dionys"), new KillObjective(1, "boss_Dionys_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard8", 3));
		AddReward(new TakeItemReward("VPRISON_HAUBERK_SEAL"));
	}
}

// 60017: The Planned Escape
//-----------------------------------------------------------------------------
public class Vprison514Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60017);
		SetName(L("The Planned Escape"));
		SetDescription(L("Vakarine names what Hauberk carried off: the Chain of Reversion."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Talk to Kupole Aldona"), L("Hauberk took the strong power sealed in Dionys and ran away. Ask Aldona what to do."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_MQ_VAKARINE", "d_velniasprison_51_4", L("Talk to Goddess Vakarine"), L("Tell Goddess Vakarine that Hauberk took the power in Dionys and ask what you should do."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_VAKARINE", "d_velniasprison_51_4", L("Talk to Goddess Vakarine"), L("Tell Goddess Vakarine that Hauberk took the power in Dionys and ask what you should do."));

		AddPrerequisite(new QuestStatusPrerequisite(60016, QuestStatus.Completed));

		AddObjective("tellVakarine", L("Talk to Goddess Vakarine"), new ManualObjective());
	}
}

// 60033: The Fury
//-----------------------------------------------------------------------------
public class Vprison514Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60033);
		SetName(L("The Fury"));
		SetDescription(L("Valtross is dead, and his servants are still loose in Oruarma Cathedral."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", L("Talk to Kupole Zydrone"), L("Seems like Kupole Zydrone needs your help. Talk to Kupole Zydrone."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", L("Defeat the remnants of Valtross"), L("Kupole Zydrone said that the remnants of Valtross may be making the rampaging. Defeat the remnant monsters on Oruarma Cathedral."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_ZYDRONE", "d_velniasprison_51_4", L("Talk to Kupole Zydrone"), L("The subordinate of Valtross has been defeated. Report to Kupole Zydrone."));

		AddPrerequisite(new LevelPrerequisite(147));
		AddPrerequisite(new QuestStatusPrerequisite(60017, QuestStatus.Completed));

		AddObjective("killRemnants", L("Defeat the remnants of Valtross"), new KillObjective(10, "Elma", "Nuo", "Socket", "mushroom_ent_green"));

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 60034: Dionys' Claws
//-----------------------------------------------------------------------------
public class Vprison514Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60034);
		SetName(L("Dionys' Claws"));
		SetDescription(L("The demons that fled Dionys near the Rearda Seal still have his claws in them."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Talk to Kupole Aldona"), L("Kupole Aldona feels pity seeing Dionys in pain. Talk to Kupole Aldona."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_MQ_04_NPC_01", "d_velniasprison_51_4", L("Collect Dionys' Claws"), L("Kupole Aldona says Dionys might recover his strength if you bring back its claws. Find and defeat the demons near Rearda Seal that was attacked by Dionys and get Dionys' claw back."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Give it to Kupole Aldona"), L("Acquired all of Dionys' claws. Give them to Kupole Aldona."));

		AddPrerequisite(new LevelPrerequisite(147));
		AddPrerequisite(new QuestStatusPrerequisite(60017, QuestStatus.Completed));

		AddObjective("collectClaws", L("Collect Dionys' Claws around the Rearda Seal"), new CollectItemObjective("VPRISON514_SQ_02_ITEM", 9));

		AddPityDrop("VPRISON514_SQ_02_ITEM", 1.0f, 0, 1, "Elma", "Nuo", "Socket", "mushroom_ent_green");

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("VPRISON514_SQ_02_ITEM"));
	}
}

// 60035: Lost Star
//-----------------------------------------------------------------------------
public class Vprison514Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60035);
		SetName(L("Lost Star"));
		SetDescription(L("The Mark of Star broke apart when Dionys lost himself, and its pieces are still around the Rada Seal."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Talk to Kupole Aldona"), L("Kupole Aldona needs your help. Talk to Kupole Aldona."));
		SetPhase(QuestStatus.InProgress, "VPRISON514_SQ_03_NPC", "d_velniasprison_51_4", L("Collect the destroyed Mark of Stars"), L("Kupole Aldona says Dionys was stopped but the Mark of Star was destroyed. Collect the scattered pieces of the Mark of Star near Rada Seal."));
		SetPhase(QuestStatus.Success, "VPRISON514_MQ_ALDONA", "d_velniasprison_51_4", L("Give it to Kupole Aldona"), L("Collected all the pieces of the Mark of Star. Give them to Kupole Aldona."));

		AddPrerequisite(new LevelPrerequisite(147));
		AddPrerequisite(new QuestStatusPrerequisite(60017, QuestStatus.Completed));

		AddObjective("collectMarks", L("Collect the destroyed Mark of Stars"), new CollectItemObjective("VPRISON514_SQ_03_ITEM", 8));

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("VPRISON514_SQ_03_ITEM"));
	}
}
