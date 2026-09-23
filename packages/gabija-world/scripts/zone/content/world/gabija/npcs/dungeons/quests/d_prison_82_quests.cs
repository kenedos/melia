//--- Melia Script ----------------------------------------------------------
// Kalejimas Investigation Room Quest NPCs
//--- Description -----------------------------------------------------------
// The last demon barrier, Zanas' sacrifice, Nebulas, and the Revelation of
// Kalejimas.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
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

public class DPrison82QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Prison81Mq10 = new QuestId(30183);
	private readonly static QuestId Mq1 = new QuestId(30184);
	private readonly static QuestId Mq2 = new QuestId(30185);
	private readonly static QuestId Mq3 = new QuestId(30186);
	private readonly static QuestId Mq4 = new QuestId(30187);
	private readonly static QuestId Mq5 = new QuestId(30188);
	private readonly static QuestId Mq6 = new QuestId(30189);
	private readonly static QuestId Mq7 = new QuestId(30190);
	private readonly static QuestId Mq8 = new QuestId(30191);
	private readonly static QuestId Mq9 = new QuestId(30192);
	private readonly static QuestId Mq10 = new QuestId(30193);
	private readonly static QuestId Mq11 = new QuestId(30194);
	private readonly static QuestId Sq1 = new QuestId(30203);
	private readonly static QuestId Sq2 = new QuestId(30204);

	private const string ZanasPortrait = "Dlg_port_zanas_prison";
	private const string EnergyVar = "Gabija.Prison82.Energy.";
	private const string KadumelCircleVar = "Gabija.Prison82.KadumelCircle";
	public const string SummoningCrystalVar = "Gabija.Prison82.SummoningCrystal";
	public const int SummoningCrystalCount = 10;
	private const string GravityMarksVar = "Gabija.Prison82.GravityMarks";
	private const string GravityKillsVar = "Gabija.Prison82.GravityKills";
	private const string LightsVar = "Gabija.Prison82.Lights";
	private const int GravityKillsNeeded = 5;
	private const int SummoningCrystalsNeeded = 5;
	private const int QuizQuestions = 3;

	// The Energy Crystal spots on the floor around the Incinerator.
	private readonly static double[,] EnergySpots =
	{
		{ 457.24, -2124.48 }, { 571.18, -2124.59 }, { 685.99, -2125.23 }, { 803.56, -2126.93 },
		{ 914.87, -2126.35 }, { 916.96, -2010.43 }, { 915.40, -1896.28 }, { 920.26, -1783.48 },
	};

	// The magic circles around the Tower of Discipline device, Kadumel's first.
	private readonly static double[,,] CircleSpots =
	{
		{ { 2248.51, 1028.68 }, { 2183.16, 784.97 }, { 1904.01, 998.38 } },
		{ { 2329.73, 1004.50 }, { 2048.11, 1225.55 }, { 2065.83, 845.85 } },
		{ { 2228.72, 1234.54 }, { 2157.96, 1010.86 }, { 1934.78, 830.00 } },
		{ { 2275.50, 852.97 }, { 1959.46, 1072.44 }, { 2317.60, 1197.45 } },
	};

	private readonly static double[,] SummoningCrystalSpots =
	{
		{ -255.95, 450.78 }, { -162.30, 753.99 }, { 64.09, 522.46 }, { -316.78, 304.31 }, { -124.84, -53.43 },
		{ -604.23, -42.72 }, { -93.57, -480.42 }, { -112.57, -860.00 }, { -343.32, -245.23 }, { -260.29, 927.08 },
	};

	private readonly static int[] KingsJewels =
	{
		ItemId.PRISON_79_MQ_8_ITEM, ItemId.PRISON_80_MQ_9_ITEM, ItemId.PRISON_81_MQ_7_ITEM, ItemId.PRISON_82_MQ_10_ITEM, ItemId.PRISON_79_MQ_3_ITEM,
	};

	protected override void Load()
	{
		// Zanas' Soul, at the Interrogation Room entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_82_NPC_1", "d_prison_82", -1152.48, -70.87, 0, this.IsZanasAtTheEntrance, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (!character.Quests.Has(Mq1) && character.Quests.MeetsPrerequisites(Mq1))
			{
				await dialog.Msg(L("So bizarre... There is a very powerful energy from that direction."));
				await dialog.Msg(L("I believe it is from Nebulas..."));

				var answer = await dialog.SelectQuestOffer(Mq1, L("Even so, it is far too strong."),
					Option(L("Ask if he presumes somthing"), "accept"),
					Option(L("Say that he is worrying too much"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1);
					character.Quests.CompleteObjective(Mq1, "senseThePower");
				}
				return;
			}

			if (character.Quests.IsActive(Mq1))
			{
				await dialog.Msg(L("Even so, it is far too strong."));
				character.Quests.ReplayQuestTrack(Mq1);
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("Something's wrong."));
			await dialog.Msg(L("I have this weird feeling."));
		});

		// Zanas' Soul, at the Incinerator
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_82_NPC_2", "d_prison_82", 530, -1735, 45, this.IsZanasAtTheIncinerator, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (character.Quests.IsActive(Mq3) && character.Quests.IsCompletable(Mq3))
			{
				await dialog.Msg(L("There is no time to lose. Don't waste your precious seconds worrying about me."));
				await dialog.Msg(L("We are racing against the clock."));
				await dialog.CompleteQuest(Mq3);
				return;
			}

			if (!character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2))
			{
				await dialog.Msg(L("This is wrong. Nebulas power should not be that high..."));
				await dialog.Msg(L("We must find out what happened."));
				await dialog.Msg(L("There is an observational detector nearby."));
				await dialog.Msg(L("But... when the demons invaded, the monsters broke it."));

				var answer = await dialog.SelectQuestOffer(Mq2, L("If we could retrieve some parts, we might be able to figure something out."),
					Option(L("Say that you will attempt to repair the Observational Detector"), "accept"),
					Option(L("Say that it is already useless"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2);
					await dialog.Msg(L("The detector is located up on the next floor."));
					await dialog.Msg(L("The nearby monsters might have the parts."));
				}
				return;
			}

			if (!character.Quests.Has(Mq3) && character.Quests.MeetsPrerequisites(Mq3))
			{
				var talked = await character.TimeActions.StartAsync(L("Talking about the Cell's Barrier"), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));

				if (talked != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("Wait, the demon barrier is that strong?"));
				await dialog.Msg(L("Inconceivable..."));
				await dialog.Msg(L("The power emitting from that one is stronger than all the previous four barriers put together."));
				await dialog.Msg(L("Perhaps, this is doing of a demon higher than Nebulas..."));
				await dialog.Msg(L("Before we disable the barrier, we have no chance against Nebulas."));
				await dialog.Msg(L("Now the Dominance Magic will not hold."));
				await dialog.Msg(L("Desperate times call for desperate measures."));
				await dialog.Msg(L("I will enhance the Dominance Magic until it's strong enough to disable that barrier."));
				await dialog.Msg(L("It means, I would have to pay my whole soul for it."));
				await dialog.Msg(L("For a Stronger Dominance Magic the price paid gets increased aswell."));
				await dialog.Msg(L("It must be done..."));

				var answer = await dialog.SelectQuestOffer(Mq3, L("If not, Nebulas can never be defeated."),
					Option(L("Ask if you must do so"), "accept"),
					Option(L("Say that you should think about it some more"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq3);
					character.Quests.CompleteObjective(Mq3, "hearHisResolve");
					await dialog.Msg(L("After all, this was caused by my own failure to protect the revelation."));
					await dialog.Msg(L("It is my responsibility."));
					await dialog.Msg(L("The cessation of my existence is a small price to pay for delivering the revelation to you, the revelator."));
					await dialog.Msg(L("Defeating Nebulas and getting the revelation..."));
					await dialog.Msg(L("Saving the world is the task, only you could do."));
					await dialog.Msg(L("Leave the demon barrier up to me."));
					await dialog.Msg(L("All you need to focus on is defeating Nebulas and obtaining the revelation."));
				}
				return;
			}

			if (!character.Quests.Has(Mq4) && character.Quests.MeetsPrerequisites(Mq4))
			{
				await dialog.Msg(L("Use the secret device near the incinerator to enhance the Dominance Magic."));
				await dialog.Msg(L("But it requires Energy Crystals first."));
				await dialog.Msg(L("Find Energy Crystals placed on the floor above the Incinerator."));

				var answer = await dialog.SelectQuestOffer(Mq4, L("Once enough crystals have been collected, you can activate the secret device."),
					Option(L("Alright"), "accept"),
					Option(L("Say that he should rethink his options"), "leave")
				);

				if (answer == "accept")
				{
					this.ClearTheEnergy(character, Mq4);
					character.Quests.Start(Mq4);
					await dialog.Msg(L("We must hurry before Nebulas finds us."));
				}
				return;
			}

			if (!character.Quests.Has(Mq5) && character.Quests.MeetsPrerequisites(Mq5))
			{
				await dialog.Msg(L("The secret device is operating perfectly."));
				await dialog.Msg(L("I will be here enhancing the Dominance Magic."));
				await dialog.Msg(L("As for you, activate all the secret devices in the Interrogation Room."));

				var answer = await dialog.SelectQuestOffer(Mq5, L("That would buy us some time."),
					Option(L("Say that you will not fail"), "accept"),
					Option(L("Say that it is impossible"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq5);
					await dialog.Msg(L("I have faith in you."));
					await dialog.Msg(L("You would not have much difficulty."));
					await dialog.Msg(L("Either activating secret devices to attack demons..."));
					await dialog.Msg(L("Or destroy demon devices or magic circles. Do whatever you seem fit."));
					await dialog.Msg(L("By the way,"));
					await dialog.Msg(L("I'd appreciate it if you could deal with some of the monsters on the way so they won't be able to get here."));
					await dialog.Msg(L("Give me the signal with the Teal Magic Stone as soon as you get to the Barrier."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq2))
			{
				await dialog.Msg(L("What on earth has happened to Nebulas?"));
				await dialog.Msg(L("He was a strong demon to begin with but this is ludicrous."));
				return;
			}

			if (character.Quests.IsActive(Mq4))
			{
				await dialog.Msg(L("I cannot even fathom what functions were intended for the secret device near the incinerator."));
				await dialog.Msg(L("I have never heard that King Kadumel was so knowledgeable in magic."));
				await dialog.Msg(L("What good could come out of enhancing such minute spell?"));
				await dialog.Msg(L("I am not complaining, it did give us the ample opportunity to combat Nebulas thanks to that secret device."));
				return;
			}

			if (character.Quests.IsActive(Mq5) || character.Quests.IsActive(Mq6) || character.Quests.IsActive(Mq7) || character.Quests.IsActive(Mq8) || character.Quests.IsActive(Mq9))
			{
				await dialog.Msg(L("It better not be too late."));
				await dialog.Msg(L("I really don't wanna fail twice."));
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("Nebulas is not one to be joked with."));
			await dialog.Msg(L("We should hurry."));
		});

		// The Observation Detector
		//-------------------------------------------------------------------------
		AddNpc(147504, L("Observation Detector"), "PRISON_82_OBJ_1", "d_prison_82", 448, -1402, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Observation Detector"));

			if (character.Quests.IsActive(Mq2) && character.Quests.IsCompletable(Mq2))
			{
				var repaired = await character.TimeActions.StartAsync(L("Repairing"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (repaired != TimeActionResult.Completed)
					return;

				character.Quests.ReplayQuestTrack(Mq2);
				return;
			}

			if (character.Quests.IsActive(Mq2))
			{
				await dialog.Msg(L("The detector is missing parts. The monsters nearby carried them off."));
				return;
			}

			await dialog.Msg(L("An Observation Detector, watching the demon barrier."));
		});

		// The Demon Barrier
		//-------------------------------------------------------------------------
		AddConditionalNpc(151003, L("Demon Barrier"), "PRISON_82_OBJ_2", "d_prison_82", -550, -1577, 90, this.IsTheBarrierStanding, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Demon Barrier"));

			if (!character.Quests.Has(Mq10) && character.Quests.MeetsPrerequisites(Mq10))
			{
				if (character.Inventory.CountItem(ItemId.PRISON_78_MQ_3_ITEM) == 0)
				{
					await dialog.Msg(L("Zanas answers only to the Teal Magic Stone."));
					return;
				}

				var called = await character.TimeActions.StartAsync(L("Using the Teal Magic Stone"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (called != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.PRISON_78_MQ_3_ITEM, character.Inventory.CountItem(ItemId.PRISON_78_MQ_3_ITEM));
				character.Quests.Start(Mq10);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq10) && !character.Quests.IsCompletable(Mq10))
			{
				await dialog.Msg(L("Nebulas' power became weaker after disabling the demon barrier. Defeat Nebulas!"));
				character.Quests.ReplayQuestTrack(Mq10);
				return;
			}

			await dialog.Msg(L("A demon barrier far stronger than the four before it."));
		});

		// The Incinerator's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151111, L("Secret Device"), "PRISON_82_OBJ_3", "d_prison_82", 643, -1840, 225, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq4) && character.Quests.IsCompletable(Mq4))
			{
				var used = await character.TimeActions.StartAsync(L("Using Energy Crystal"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (used != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The Energy Crystals pour into the device, and the Incinerator begins to hum with Dominance Magic."));
				await dialog.CompleteQuest(Mq4);
				return;
			}

			if (character.Quests.IsActive(Mq4))
			{
				await dialog.Msg(L("The device needs Energy Crystals. Charge them around the Incinerator first."));
				return;
			}

			await dialog.Msg(L("The Incinerator's secret device."));
		});

		// The Energy Crystal spots around the Incinerator
		//-------------------------------------------------------------------------
		for (var i = 0; i < EnergySpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddQuestTrigger("PRISON_82_OBJ_04_" + number, "d_prison_82", EnergySpots[i, 0], EnergySpots[i, 1], 35, args => this.ChargeTheEnergy(args, number));
		}

		// The Tower of Discipline Magical Device
		//-------------------------------------------------------------------------
		AddNpc(151110, L("Secret Device"), "PRISON_82_OBJ_5", "d_prison_82", 2306.59, 773.65, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq6) && character.Quests.IsCompletable(Mq6))
			{
				var activated = await character.TimeActions.StartAsync(L("Activating"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (activated != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("With Kadumel's circles burning, the device casts its illusions into the Interrogation Room."));
				await dialog.CompleteQuest(Mq6);
				character.LookAround();
				character.ServerMessage(L("Illusions have been summoned in the Interrogation Room after the Magic Device was activated"));
				return;
			}

			if (!character.Quests.Has(Mq6) && character.Quests.MeetsPrerequisites(Mq6))
			{
				var activated = await character.TimeActions.StartAsync(L("Activating"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (activated != TimeActionResult.Completed)
					return;

				for (var i = 0; i < CircleSpots.GetLength(1); ++i)
					character.Variables.Perm.Remove(KadumelCircleVar + i);

				character.Quests.Start(Mq6);
				character.LookAround();
				character.ServerMessage(L("Only activate Kadumel's Magic Circles. You must start over if you activate any of the others."));
				return;
			}

			if (character.Quests.IsActive(Mq6))
			{
				character.ServerMessage(L("Only activate Kadumel's Magic Circles. You must start over if you activate any of the others."));
				return;
			}

			await dialog.Msg(L("The Tower of Discipline's magical device."));
		});

		// The magic circles around the Tower of Discipline device
		//-------------------------------------------------------------------------
		var circleNames = new[] { L("Kadumel's Magic Circle"), L("Zachariel's Magic Circle"), L("Jonael's Magic Circle"), L("Hieskel's Magic Circle") };

		for (var owner = 0; owner < CircleSpots.GetLength(0); ++owner)
		{
			for (var i = 0; i < CircleSpots.GetLength(1); ++i)
			{
				var circleOwner = owner;
				var index = i;

				AddConditionalNpc(147469, circleNames[owner], "PRISON_82_OBJ_5_" + (owner + 1) + "_" + (i + 1), "d_prison_82",
					CircleSpots[owner, i, 0], CircleSpots[owner, i, 1], 90, this.AreTheCirclesUp, dialog => this.ActivateTheCircle(dialog, circleOwner, index));
			}
		}

		// The Execution Grounds' Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_82_OBJ_6", "d_prison_82", 769.61, 1196.15, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq7) && character.Quests.IsCompletable(Mq7))
			{
				var opened = await character.TimeActions.StartAsync(L("Opening the Device"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (opened != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("Inside the device lies a worn Gravity Stone, still humming with its old magic."));
				await dialog.CompleteQuest(Mq7);

				if (!character.Quests.Has(Mq8) && character.Quests.MeetsPrerequisites(Mq8))
				{
					this.ClearTheGravityMarks(character);
					character.Quests.Start(Mq8);
					character.ServerMessage(L("Use the Worn Gravity Stone on monsters"));
				}
				return;
			}

			if (!character.Quests.Has(Mq7) && character.Quests.MeetsPrerequisites(Mq7))
			{
				var activated = await character.TimeActions.StartAsync(L("Activating the Device"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (activated != TimeActionResult.Completed)
					return;

				character.Variables.Temp.Remove(LightsVar);
				character.Quests.Start(Mq7);
				character.ServerMessage(L("The device has been activated. Match the three lights to the same color."));
				return;
			}

			if (character.Quests.IsActive(Mq7))
			{
				if (!await this.MatchTheThreeLights(dialog))
					return;

				character.Quests.CompleteObjective(Mq7, "matchTheLights");
				character.ServerMessage(L("The secret device's seal has been disarmed. Retrieve the worn Gravity Stone from the secret device."));
				return;
			}

			await dialog.Msg(L("The Execution Grounds' secret device, with three lights set into it."));
		});

		// Demon Summoning Crystals
		//-------------------------------------------------------------------------
		for (var i = 0; i < SummoningCrystalSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(151113, L("Demon Summoning Crystal"), "PRISON_82_OBJ_7_" + number, "d_prison_82", SummoningCrystalSpots[i, 0], SummoningCrystalSpots[i, 1], 90,
				character => this.IsTheCrystalStanding(character, number), async dialog =>
			{
				dialog.SetTitle(L("Demon Summoning Crystal"));
				await dialog.Msg(L("A crystal the demons are summoned through. Force alone will not move it."));
			});
		}

		// The Confessional Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151109, L("Secret Device"), "PRISON_82_OBJ_8", "d_prison_82", -424, -345, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Mq10) && character.Quests.IsCompletable(Mq10))
			{
				await dialog.Msg(L("The Confessional's secret device, with four sockets waiting for the King's Jewels."));
				await dialog.CompleteQuest(Mq10);

				if (!character.Quests.Has(Mq11) && character.Quests.MeetsPrerequisites(Mq11))
					await this.OpenTheConfessional(character);

				return;
			}

			if (!character.Quests.Has(Mq11) && character.Quests.MeetsPrerequisites(Mq11))
			{
				await this.OpenTheConfessional(character);
				return;
			}

			if (character.Quests.IsActive(Mq11))
			{
				character.Quests.ReplayQuestTrack(Mq11);
				return;
			}

			await dialog.Msg(character.Quests.HasCompleted(Mq11) ? L("The Confessional's secret device stands open and empty.") : L("A secret device with four sockets, sealed tight."));
		});

		// The Room of Abyss' Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_82_SQ_OBJ_1", "d_prison_82", -733.44, 2210.27, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (character.Quests.IsActive(Sq1) && character.Quests.IsCompletable(Sq1))
			{
				await dialog.Msg(L("The seal is gone, and what the device kept is there for the taking."));
				await dialog.CompleteQuest(Sq1);
				return;
			}

			if (!character.Quests.Has(Sq1) && character.Quests.MeetsPrerequisites(Sq1))
			{
				var activated = await character.TimeActions.StartAsync(L("Activating Secret Device"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

				if (activated != TimeActionResult.Completed)
					return;

				character.Quests.Start(Sq1);
			}

			if (!character.Quests.IsActive(Sq1))
			{
				await dialog.Msg(L("A secret device in the Room of Abyss, reading out hints no one is there to answer."));
				return;
			}

			if (!await this.AnswerTheDevice(dialog))
				return;

			character.Quests.CompleteObjective(Sq1, "answerTheDevice");
			character.ServerMessage(L("The secret device has been disarmed"));
		});

		// Zanas' Echo
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Zanas' Echo"), "PRISON_82_SQ_OBJ_2", "d_prison_82", 530, -1735, 90, this.IsTheEchoLingering, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Echo"));

			if (character.Quests.IsActive(Sq2) && character.Quests.IsCompletable(Sq2))
			{
				var passed = await character.TimeActions.StartAsync(L("Passing energy to the Echo"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (passed != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("I can hear my own voice..."));
				await dialog.Msg(L("That must mean Nebulas was defeated and you have the revelation."));
				await dialog.Msg(L("That's great."));
				await dialog.Msg(L("I knew you could do it."));
				await dialog.Msg(L("I spent a lot of time in prison."));
				await dialog.Msg(L("Wondering... I'm just a man, what could I possibly do?"));
				await dialog.Msg(L("I spent years hiding the revelation..."));
				await dialog.Msg(L("And many more trying to protect it from the demons."));
				await dialog.Msg(L("I was chased by them and had no choice but to split my spirit with a magic device..."));
				await dialog.Msg(L("It was not fun."));
				await dialog.Msg(L("I became tired..."));
				await dialog.Msg(L("Waiting for a Revelator I wasn't even sure was going to come."));
				await dialog.Msg(L("And then you appeared."));
				await dialog.Msg(L("I could finally complete my mission."));
				await dialog.Msg(L("I couldn't stay for long but..."));
				await dialog.Msg(L("I'm glad you're the Revelator."));
				await dialog.Msg(L("There's no way I could defeat Nebulas without you."));
				await dialog.Msg(L("I dunno about you, but I made a really precious friend here."));
				await dialog.Msg(L("Sorry I couldn't be with you until the end."));
				await dialog.Msg(L("And thank you."));
				await dialog.Msg(L("Goodbye?"));
				await dialog.CompleteQuest(Sq2);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq2) && character.Quests.MeetsPrerequisites(Sq2))
			{
				var examined = await character.TimeActions.StartAsync(L("Examining Echo"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (examined != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The sound of Zanas' voice is heard."));
				this.ClearTheEnergy(character, Sq2);
				character.Quests.Start(Sq2);
				return;
			}

			await dialog.Msg(L("There seems to be a sound coming from somewhere."));
		});
	}

	/// <summary>
	/// Uses the Worn Gravity Stone on the demons or the Demon Summoning
	/// Crystals around the player.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_PRISON_82_MQ_7_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName == "d_prison_82" && character.Quests.IsActive(Mq9) && !character.Quests.IsCompletable(Mq9))
		{
			if (this.RemoveASummoningCrystal(character))
				return ItemUseResult.OkayNotConsumed;
		}

		if (character.Map.ClassName == "d_prison_82" && character.Quests.IsActive(Mq8) && !character.Quests.IsCompletable(Mq8))
		{
			var targets = character.Map.GetAttackableEnemiesInPosition(character, character.Position, 150).Take(5).ToList();

			if (targets.Count != 0)
			{
				if (character.Variables.Temp.Get(GravityMarksVar) is not HashSet<int> marks)
				{
					marks = new HashSet<int>();
					character.Variables.Temp.Set(GravityMarksVar, marks);
				}

				foreach (var target in targets)
				{
					marks.Add(target.Handle);
					target.PlayEffect("F_ground058_smoke", 1f);
				}

				character.ServerMessage(L("The Worn Gravity Stone drags the demons down. Defeat them."));
				return ItemUseResult.OkayNotConsumed;
			}
		}

		character.ServerMessage(L("There are no targets to use the Gravity Stone on nearby"));
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Counts the demons defeated after the Worn Gravity Stone struck them.
	/// </summary>
	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Attacker is not Character character || args.Target is not Mob mob)
			return;

		if (!character.Quests.IsActive(Mq8) || character.Quests.IsCompletable(Mq8))
			return;

		if (character.Variables.Temp.Get(GravityMarksVar) is not HashSet<int> marks || !marks.Remove(mob.Handle))
			return;

		var kills = character.Variables.Perm.GetInt(GravityKillsVar, 0) + 1;
		character.Variables.Perm.SetInt(GravityKillsVar, kills);

		if (kills < GravityKillsNeeded)
		{
			character.ServerMessage(LF("Demons defeated with the Gravity Stone: {0}/{1}", kills, GravityKillsNeeded));
			return;
		}

		this.ClearTheGravityMarks(character);
		character.Quests.CompleteObjective(Mq8, "testTheStone");
	}

	/// <summary>
	/// Removes the nearest Demon Summoning Crystal with the Worn Gravity
	/// Stone, returns false if none is in reach.
	/// </summary>
	/// <param name="character"></param>
	private bool RemoveASummoningCrystal(Character character)
	{
		for (var i = 0; i < SummoningCrystalSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			if (character.Variables.Perm.GetBool(SummoningCrystalVar + number, false))
				continue;

			var spot = new Position((float)SummoningCrystalSpots[i, 0], character.Position.Y, (float)SummoningCrystalSpots[i, 1]);
			if (character.Position.Get2DDistance(spot) > 150)
				continue;

			character.Variables.Perm.SetBool(SummoningCrystalVar + number, true);
			character.LookAround();

			var removed = 0;
			for (var j = 1; j <= SummoningCrystalSpots.GetLength(0); ++j)
			{
				if (character.Variables.Perm.GetBool(SummoningCrystalVar + j, false))
					removed++;
			}

			if (removed < SummoningCrystalsNeeded)
			{
				character.ServerMessage(LF("Demon Summoning Crystals removed: {0}/{1}", removed, SummoningCrystalsNeeded));
				return true;
			}

			character.Quests.CompleteObjective(Mq9, "removeTheCrystals");
			return true;
		}

		return false;
	}

	/// <summary>
	/// Charges one of the Energy Crystal spots around the Incinerator.
	/// </summary>
	/// <param name="args"></param>
	/// <param name="number"></param>
	private async Task ChargeTheEnergy(TriggerActorArgs args, int number)
	{
		if (args.Initiator is not Character character)
			return;

		var questId = Mq4;
		var objective = "chargeTheCrystals";

		if (!character.Quests.IsActive(Mq4) || character.Quests.IsCompletable(Mq4))
		{
			questId = Sq2;
			objective = "gatherTheEnergy";
		}

		if (!character.Quests.IsActive(questId) || character.Quests.IsCompletable(questId))
			return;

		var varName = EnergyVar + questId.Value + "." + number;
		if (character.Variables.Perm.GetBool(varName, false))
			return;

		character.Variables.Perm.SetBool(varName, true);

		var charged = 0;
		for (var i = 1; i <= EnergySpots.GetLength(0); ++i)
		{
			if (character.Variables.Perm.GetBool(EnergyVar + questId.Value + "." + i, false))
				charged++;
		}

		if (charged < EnergySpots.GetLength(0))
		{
			character.ServerMessage(LF("Energy Crystals: {0}/{1}", charged, EnergySpots.GetLength(0)));
			return;
		}

		character.Quests.CompleteObjective(questId, objective);

		await Task.CompletedTask;
	}

	/// <summary>
	/// Forgets the Energy Crystals charged for the given quest.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="questId"></param>
	private void ClearTheEnergy(Character character, QuestId questId)
	{
		for (var i = 1; i <= EnergySpots.GetLength(0); ++i)
			character.Variables.Perm.Remove(EnergyVar + questId.Value + "." + i);
	}

	/// <summary>
	/// Forgets which demons the Worn Gravity Stone has struck.
	/// </summary>
	/// <param name="character"></param>
	private void ClearTheGravityMarks(Character character)
	{
		character.Variables.Temp.Remove(GravityMarksVar);
		character.Variables.Perm.Remove(GravityKillsVar);
	}

	/// <summary>
	/// Activates one of the magic circles around the Tower of Discipline
	/// device, where anything but Kadumel's own starts it all over.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="owner"></param>
	/// <param name="index"></param>
	private async Task ActivateTheCircle(Dialog dialog, int owner, int index)
	{
		var character = dialog.Player;

		if (owner != 0)
		{
			for (var i = 0; i < CircleSpots.GetLength(1); ++i)
				character.Variables.Perm.Remove(KadumelCircleVar + i);

			character.ServerMessage(L("You must start over since you activated the wrong magic circle"));
			return;
		}

		character.Variables.Perm.SetBool(KadumelCircleVar + index, true);
		character.ServerMessage(L("You have activated Kadumel's Magic Circle"));

		for (var i = 0; i < CircleSpots.GetLength(1); ++i)
		{
			if (!character.Variables.Perm.GetBool(KadumelCircleVar + i, false))
				return;
		}

		character.Quests.CompleteObjective(Mq6, "activateKadumelsCircles");
		character.LookAround();
		character.ServerMessage(L("All of Kadumel's Magic Circles have been activated. Activate the Magical Device at the Tower of Discipline."));

		await Task.CompletedTask;
	}

	/// <summary>
	/// Runs the Execution Grounds device's three lights until they all
	/// show the same color, returns false if the player walks away.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task<bool> MatchTheThreeLights(Dialog dialog)
	{
		var character = dialog.Player;
		var colors = new[] { L("red"), L("blue"), L("yellow") };

		if (character.Variables.Temp.Get(LightsVar) is not int[] lights)
		{
			do
			{
				lights = new[] { System.Random.Shared.Next(3), System.Random.Shared.Next(3), System.Random.Shared.Next(3) };
			}
			while (lights[0] == lights[1] && lights[1] == lights[2]);

			character.Variables.Temp.Set(LightsVar, lights);
		}

		while (lights[0] != lights[1] || lights[1] != lights[2])
		{
			var answer = await dialog.Select(LF("The lights glow {0}, {1} and {2}. Magic poured into a light also reaches the one beside it.", colors[lights[0]], colors[lights[1]], colors[lights[2]]),
				Option(L("Pour magic into the first light"), "0"),
				Option(L("Pour magic into the second light"), "1"),
				Option(L("Pour magic into the third light"), "2"),
				Option(L("Step away"), "leave")
			);

			if (!int.TryParse(answer, out var light))
				return false;

			lights[light] = (lights[light] + 1) % colors.Length;
			lights[(light + 1) % lights.Length] = (lights[(light + 1) % lights.Length] + 1) % colors.Length;
		}

		character.Variables.Temp.Remove(LightsVar);
		return true;
	}

	/// <summary>
	/// Runs the Room of Abyss device's questions, returns false on the
	/// first wrong answer.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task<bool> AnswerTheDevice(Dialog dialog)
	{
		var questions = new (string Question, string Answer, string[] Wrong)[]
		{
			(L("Who is the creator of Chronomancy?"), L("Lydia Schaffen"), new[] { L("Agailla Flurry"), L("King Zachariel"), L("King Malkiel") }),
			(L("Who is the first lord of the Kingdom?"), L("King Taniel the First"), new[] { L("King Baltinel"), L("King Jeromel the First"), L("King Taniel the Second") }),
			(L("Who is the King that completed the Kingdom's walls?"), L("King Taniel the Second"), new[] { L("King Jeromel the Second"), L("King Malkiel"), L("King Zachariel") }),
		};

		for (var i = 0; i < QuizQuestions; ++i)
		{
			var (question, correct, wrong) = questions[i];
			var names = wrong.ToList();
			var answer = System.Random.Shared.Next(names.Count + 1);
			names.Insert(answer, correct);

			var picked = await dialog.Select(question, names.Select((name, n) => Option(name, n.ToString())));

			if (picked != answer.ToString())
			{
				await dialog.Msg(L("Wrong! Start from the beginning."));
				return false;
			}

			var left = QuizQuestions - i - 1;
			if (left > 0)
				await dialog.Msg(LF("Correct! The seal will be disarmed if you answer {0} more questions.", left));
		}

		return true;
	}

	/// <summary>
	/// Sets the four King's Jewels into the Confessional device and starts
	/// the Revelation of Kalejimas.
	/// </summary>
	/// <param name="character"></param>
	private async Task OpenTheConfessional(Character character)
	{
		var used = await character.TimeActions.StartAsync(L("Using the King's Jewel"), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(2));

		if (used != TimeActionResult.Completed)
			return;

		foreach (var jewel in KingsJewels)
		{
			var count = character.Inventory.CountItem(jewel);
			if (count > 0)
				character.Inventory.RemoveItem(jewel, count);
		}

		character.Quests.Start(Mq11);
		character.Quests.CompleteObjective(Mq11, "openTheConfessional");
	}

	/// <summary>
	/// Returns whether Zanas' Soul waits at the Interrogation Room entrance.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasAtTheEntrance(Character character)
		=> character.Quests.HasCompleted(Prison81Mq10) && !character.Quests.HasCompleted(Mq1);

	/// <summary>
	/// Returns whether Zanas' Soul waits at the Incinerator.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasAtTheIncinerator(Character character)
		=> character.Quests.HasCompleted(Mq1) && !character.Quests.Has(Mq10);

	/// <summary>
	/// Returns whether the last demon barrier still stands for the player.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTheBarrierStanding(Character character)
		=> character.Quests.HasCompleted(Prison81Mq10) && !character.Quests.HasCompleted(Mq10) && !character.Quests.IsCompletable(Mq10);

	/// <summary>
	/// Returns whether the Tower of Discipline's magic circles are out.
	/// </summary>
	/// <param name="character"></param>
	private bool AreTheCirclesUp(Character character)
		=> character.Quests.IsActive(Mq6) && !character.Quests.IsCompletable(Mq6);

	/// <summary>
	/// Returns whether the numbered Demon Summoning Crystal still stands.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="number"></param>
	private bool IsTheCrystalStanding(Character character, int number)
	{
		if (character.Quests.HasCompleted(Mq9))
			return false;

		return !character.Quests.IsActive(Mq9) || !character.Variables.Perm.GetBool(SummoningCrystalVar + number, false);
	}

	/// <summary>
	/// Returns whether Zanas' Echo lingers at the Incinerator.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTheEchoLingering(Character character)
		=> character.Quests.HasCompleted(Mq11) && !character.Quests.HasCompleted(Sq2);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 30184: Unexpected Situation(1)
//-----------------------------------------------------------------------------
public class Prison82Mq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30184);
		SetName(L("Unexpected Situation(1)"));
		SetDescription(L("A power far greater than Nebulas' own waits in the Interrogation Room."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_NPC_1", "d_prison_82", L("Talk to Zanas' Spirit in the Interrogation Room"), L("You must go to the Interrogation Room in order to gain the last King's Jewel. Go to the Interrogation Room and talk to Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_NPC_1", "d_prison_82", L("Talk to Zanas' Spirit in the Interrogation Room"), L("You must go to the Interrogation Room in order to gain the last King's Jewel. Go to the Interrogation Room and talk to Zanas' Spirit."));
		SetPhase(QuestStatus.Success, "PRISON_82_NPC_1", "d_prison_82", L("Talk to Zanas' Spirit in the Interrogation Room"), L("You must go to the Interrogation Room in order to gain the last King's Jewel. Go to the Interrogation Room and talk to Zanas' Spirit."));

		SetTrack(QuestStatus.Success, QuestStatus.Completed, "PRISON_82_MQ_1_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(30183, QuestStatus.Completed));

		AddObjective("senseThePower", L("Talk to Zanas' Spirit in the Interrogation Room"), new ManualObjective());
	}
}

// 30185: Unexpected Situation(2)
//-----------------------------------------------------------------------------
public class Prison82Mq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30185);
		SetName(L("Unexpected Situation(2)"));
		SetDescription(L("A repaired Observation Detector shows what has happened to the last barrier."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_NPC_2", "d_prison_82", L("Talk to Zanas' Soul"), L("You feel great power from Nebulas. Ask Zanas' Spirit what has happened."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_1", "d_prison_82", L("Defeat nearby monsters to retrieve the Observational Detector parts"), L("Zanas' Spirit wishes to see what is going on. Defeat nearby monsters and retrieve Observational Detector parts in order to repair and use it."));
		SetPhase(QuestStatus.Success, "PRISON_82_OBJ_1", "d_prison_82", L("Repair the Observational Detector"), L("You have gathered all of the parts required. Repair the Observational Detector and examine the Demon Barrier."));

		SetTrack(QuestStatus.Success, QuestStatus.Completed, "PRISON_82_MQ_2_TRACK", 4000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(30184, QuestStatus.Completed));

		AddObjective("collectParts", L("Defeat nearby monsters to retrieve Observational Detector Components"), new CollectItemObjective("PRISON_82_MQ_2_ITEM", 7));

		AddPityDrop("PRISON_82_MQ_2_ITEM", 1.0f, 0, 1, "Templeslave_sword_blue", "Templeslave_mage_blue", "Wendigo_bow_white");

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7529));
		AddReward(new TakeItemReward("PRISON_82_MQ_2_ITEM"));
	}
}

// 30186: Zanas' Resolve(1)
//-----------------------------------------------------------------------------
public class Prison82Mq3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30186);
		SetName(L("Zanas' Resolve(1)"));
		SetDescription(L("Zanas means to pay his whole soul to break the last barrier."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_NPC_2", "d_prison_82", L("Talk to Zanas' Soul"), L("This is a much stronger Demon Barrier than the one you had disarmed. Tell this to Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_NPC_2", "d_prison_82", L("Talk to Zanas' Soul"), L("This is a much stronger Demon Barrier than the one you had disarmed. Tell this to Zanas' Spirit."));
		SetPhase(QuestStatus.Success, "PRISON_82_NPC_2", "d_prison_82", L("Talk to Zanas' Soul"), L("Zanas' Spirit is thinking of sacrificing itself to disarm the Barrier. Talk to Zanas' Spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(30185, QuestStatus.Completed));

		AddObjective("hearHisResolve", L("Talk to Zanas' Soul"), new ManualObjective());
	}
}

// 30187: Zanas' Resolve(2)
//-----------------------------------------------------------------------------
public class Prison82Mq4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30187);
		SetName(L("Zanas' Resolve(2)"));
		SetDescription(L("The Incinerator's device strengthens the Dominance Magic on Energy Crystals."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_NPC_2", "d_prison_82", L("Talk to Zanas' Soul"), L("Zanas' Spirit seems to have strengthened it's resolve. Talk about what needs to be done in order for his resolve to not be in vain."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_04_1", "d_prison_82", L("Charge Energy Crystals around the Incinerator"), L("It is said that the Incinerator's Secret Device must be used to enhance the Dominance Magic. Collect Energy Crystals from around the Incinerator in order to activate the Secret Device."));
		SetPhase(QuestStatus.Success, "PRISON_82_OBJ_3", "d_prison_82", L("Activate the Incinerator's Secret Device"), L("You have collected enough Energy Crystals. Activate the Incinerator's Secret Device."));

		AddPrerequisite(new QuestStatusPrerequisite(30186, QuestStatus.Completed));

		AddObjective("chargeTheCrystals", L("Charge Energy Crystals around the Incinerator"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7529));
	}
}

// 30188: What needs to be done(1)
//-----------------------------------------------------------------------------
public class Prison82Mq5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30188);
		SetName(L("What needs to be done(1)"));
		SetDescription(L("The way to the Incinerator has to be kept clear while Zanas works."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_NPC_2", "d_prison_82", L("Talk to Zanas' Soul"), L("The Incinerator's Secret Device has been activated. Talk to Zanas's Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_NPC_2", "d_prison_82", L("Defeat the monsters on the way to the Incinerator"), L("Zanas' Spirit says that it will utilize itself and the Incinerator in order to enhance the Dominance Magic. You must activate the Secret Devices and attack the Demons to draw their attention in order to protect Zanas and weaken the Demons. Defeat the monsters on the way to the Incinerator in order to protect Zanas' Spirit at the Incinerator."));
		SetPhase(QuestStatus.Success, "PRISON_82_NPC_2", "d_prison_82", L("Defeat the monsters on the way to the Incinerator"), L("Zanas' Spirit says that it will utilize itself and the Incinerator in order to enhance the Dominance Magic. You must activate the Secret Devices and attack the Demons to draw their attention in order to protect Zanas and weaken the Demons. Defeat the monsters on the way to the Incinerator in order to protect Zanas' Spirit at the Incinerator."));

		AddPrerequisite(new QuestStatusPrerequisite(30187, QuestStatus.Completed));

		AddObjective("clearTheWay", L("Defeat the monsters on the way to the Incinerator"), new KillObjective(13, "Templeslave_sword_blue", "Templeslave_mage_blue", "Wendigo_bow_white"));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7529));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("The way to the Incinerator is clear. Activate the Magical Device at the Tower of Discipline."));
	}
}

// 30189: What needs to be done(2)
//-----------------------------------------------------------------------------
public class Prison82Mq6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30189);
		SetName(L("What needs to be done(2)"));
		SetDescription(L("Only Kadumel's own magic circles wake the Tower of Discipline's illusions."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_OBJ_5", "d_prison_82", L("Activate the Magical Device at the Tower of Discipline"), L("You must distract the Demons in order to make it harder for them to find Zanas. Activate the Magical Device on the Tower of Discipline."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_5", "d_prison_82", L("Activate all of Kadumel's Magic Circles"), L("Tower of Discipline Magical Device : You must find and activate only Kadumel's Magic Circles after activating the Device. You must start over if you activate other Magic Circle as Kadumel's Magic Circles will lose their strength. If you activate the Device after activating all of Kadumel's Magic Circles, illusions will appear in the Interrogation Room and confuse enemies."));
		SetPhase(QuestStatus.Success, "PRISON_82_OBJ_5", "d_prison_82", L("Activate the Magical Device at the Tower of Discipline"), L("All of Kadumel's Magic Circles have been activated. Move to the Execution Grounds to activate the next Device."));

		AddPrerequisite(new QuestStatusPrerequisite(30188, QuestStatus.Completed));

		AddObjective("activateKadumelsCircles", L("Activate all of Kadumel's Magic Circles"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 7529));
	}
}

// 30190: What needs to be done(3)
//-----------------------------------------------------------------------------
public class Prison82Mq7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30190);
		SetName(L("What needs to be done(3)"));
		SetDescription(L("The Execution Grounds' device keeps a worn Gravity Stone behind three lights."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_OBJ_6", "d_prison_82", L("Disarm the Secret Device at the Execution Grounds"), L("You must distract the Demons to make it harder for them to find Zanas. Activate the Secret Device at the Execution Grounds."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_6", "d_prison_82", L("Disarm the Secret Device at the Execution Grounds"), L("Execution Grounds Secret Device : You must match the lights to the same color to disarm the Device. The lights will change color at certain intervals or by reacting to Magic."));
		SetPhase(QuestStatus.Success, "PRISON_82_OBJ_6", "d_prison_82", L("Obtained a Worn Gravity Stone from the Secret Device"), L("The Secret Device has been disarmed. Take the Worn Gravity Stone from the Secret Device."));

		AddPrerequisite(new QuestStatusPrerequisite(30189, QuestStatus.Completed));

		AddObjective("matchTheLights", L("Disarm the Secret Device at the Execution Grounds"), new ManualObjective());

		AddReward(new ItemReward("PRISON_82_MQ_7_ITEM", 1));
		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 7529));
	}
}

// 30191: What needs to be done(4)
//-----------------------------------------------------------------------------
public class Prison82Mq8Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30191);
		SetName(L("What needs to be done(4)"));
		SetDescription(L("The worn Gravity Stone has to prove it still works on the demons."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "PRISON_82_OBJ_6", "d_prison_82", L("Defeat the Demons by using the Worn Gravity Stone"), L("You should check to see if the Gravity Stone is still in working condition. Use the Worn Gravity Stone on Demons."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_6", "d_prison_82", L("Defeat the Demons by using the Worn Gravity Stone"), L("You should check to see if the Gravity Stone is still in working condition. Use the Worn Gravity Stone on Demons."));
		SetPhase(QuestStatus.Success, "PRISON_82_OBJ_6", "d_prison_82", L("Defeat the Demons by using the Worn Gravity Stone"), L("You should check to see if the Gravity Stone is still in working condition. Use the Worn Gravity Stone on Demons."));

		AddPrerequisite(new QuestStatusPrerequisite(30190, QuestStatus.Completed));

		AddObjective("testTheStone", L("Defeat the Demons by using the Worn Gravity Stone"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7529));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);

		var next = new QuestId(30192);
		if (character.Quests.Has(next) || !character.Quests.MeetsPrerequisites(next))
			return;

		for (var i = 1; i <= DPrison82QuestNpcsScript.SummoningCrystalCount; ++i)
			character.Variables.Perm.Remove(DPrison82QuestNpcsScript.SummoningCrystalVar + i);

		character.Quests.Start(next);
		character.LookAround();
		character.ServerMessage(L("Use the Worn Gravity Stone to remove the Demon Summoning Crystal"));
	}
}

// 30192: What needs to be done(5)
//-----------------------------------------------------------------------------
public class Prison82Mq9Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30192);
		SetName(L("What needs to be done(5)"));
		SetDescription(L("The Demon Summoning Crystals on the way to the Public Punishment Room give way to the Gravity Stone."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "PRISON_82_OBJ_7_1", "d_prison_82", L("Remove the Demon Summoning Crystal by using the Worn Gravity Stone"), L("The Worn Gravity Stone still seems to be in working condition after testing it on monsters. Use the Worn Gravity Stone to remove the Demon Summoning Crystal on the way to the Public Punishment Room."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_7_1", "d_prison_82", L("Remove the Demon Summoning Crystal by using the Worn Gravity Stone"), L("The Worn Gravity Stone still seems to be in working condition after testing it on monsters. Use the Worn Gravity Stone to remove the Demon Summoning Crystal on the way to the Public Punishment Room."));
		SetPhase(QuestStatus.Success, "PRISON_82_OBJ_7_1", "d_prison_82", L("Remove the Demon Summoning Crystal by using the Worn Gravity Stone"), L("The Worn Gravity Stone still seems to be in working condition after testing it on monsters. Use the Worn Gravity Stone to remove the Demon Summoning Crystal on the way to the Public Punishment Room."));

		AddPrerequisite(new QuestStatusPrerequisite(30191, QuestStatus.Completed));

		AddObjective("removeTheCrystals", L("Remove the Demon Summoning Crystal by using the Worn Gravity Stone"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7529));
		AddReward(new TakeItemReward("PRISON_82_MQ_7_ITEM"));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		character.Quests.Complete(this.QuestId);
		character.LookAround();
		character.ServerMessage(L("You have done all that you can. Go to the Demon's Barrier and call Zanas' Spirit with the Teal Magic Stone."));
	}
}

// 30193: His name is Zanas
//-----------------------------------------------------------------------------
public class Prison82Mq10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30193);
		SetName(L("His name is Zanas"));
		SetDescription(L("Zanas gives the last of himself to the Dominance Magic, and Nebulas is left to face."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_OBJ_2", "d_prison_82", L("Call Zanas' Spirit from the Demon's Barrier"), L("You have done all that you can. Go to the Demon's Barrier and use the Teal Magic Stone to call Zanas' Spirit."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_2", "d_prison_82", L("Obtain the King's Blue Jewel after defeating Nebulas"), L("Zanas' Spirit sacrificed itself to destroy the Demon's Barrier. Obtain the King's Jewel after defeating Nebulas in order for the sacrifice to not be in vain."));
		SetPhase(QuestStatus.Success, "PRISON_82_OBJ_8", "d_prison_82", L("Obtain the Revelation from the Confessional Secret Device"), L("You have defeated Nebulas and obtained the King's Blue Jewel. Obtain the Revelation from the Confessional Secret Device."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON_82_MQ_10_TRACK", "m_boss_scenario2", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(30192, QuestStatus.Completed));

		AddObjective("takeTheJewel", L("Obtain the King's Blue Jewel by defeating Nebulas"), new CollectItemObjective("PRISON_82_MQ_10_ITEM", 1));

		AddPityDrop("PRISON_82_MQ_10_ITEM", 1.0f, 0, 1, "boss_ChiefGuard");

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7529));
	}
}

// 30194: The Revelation of Kalejimas
//-----------------------------------------------------------------------------
public class Prison82Mq11Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30194);
		SetName(L("The Revelation of Kalejimas"));
		SetDescription(L("The four King's Jewels open the Confessional, and Goddess Laima speaks from it."));
		SetType(QuestType.Main);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_OBJ_8", "d_prison_82", L("Obtain the Revelation from the Confessional Secret Device"), L("You have defeated Nebulas and obtained the King's Blue Jewel. Obtain the Revelation from the Confessional Secret Device."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_8", "d_prison_82", L("Obtain the Revelation from the Confessional Secret Device"), L("You have defeated Nebulas and obtained the King's Blue Jewel. Obtain the Revelation from the Confessional Secret Device."));
		SetPhase(QuestStatus.Success, "PRISON_82_OBJ_8", "d_prison_82", L("Obtain the Revelation from the Confessional Secret Device"), L("You have defeated Nebulas and obtained the King's Blue Jewel. Obtain the Revelation from the Confessional Secret Device."));

		SetTrack(QuestStatus.Success, QuestStatus.Completed, "PRISON_82_MQ_11_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(30193, QuestStatus.Completed));

		AddObjective("openTheConfessional", L("Obtain the Revelation from the Confessional Secret Device"), new ManualObjective());

		AddReward(new StatPointReward(3));
		AddReward(new ItemReward("stonetablet08", 1));
	}
}

// 30203: Interrogation Room's Secret Device
//-----------------------------------------------------------------------------
public class Prison82Sq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30203);
		SetName(L("Interrogation Room's Secret Device"));
		SetDescription(L("The device in the Room of Abyss opens only for the right answers."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_SQ_OBJ_1", "d_prison_82", L("Disarm the Secret Device in the Room of Abyss"), L("Disarm the Secret Device in the Room of Abyss. You must choose the proper password by reading the hints from the Device."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_SQ_OBJ_1", "d_prison_82", L("Disarm the Secret Device in the Room of Abyss"), L("Disarm the Secret Device in the Room of Abyss. You must choose the proper password by reading the hints from the Device."));
		SetPhase(QuestStatus.Success, "PRISON_82_SQ_OBJ_1", "d_prison_82", L("Obtain the Reward from the Secret Device"), L("The Secret Device has been disarmed. Take the object from the Secret Device."));

		AddPrerequisite(new LevelPrerequisite(262));

		AddObjective("answerTheDevice", L("Disarm the Secret Device in the Room of Abyss"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7529));
		AddReward(new ItemReward("Drug_Haste1_event", 5));
	}
}

// 30204: Will Not Forget
//-----------------------------------------------------------------------------
public class Prison82Sq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30204);
		SetName(L("Will Not Forget"));
		SetDescription(L("Something of Zanas still shines in the Incinerator."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_82");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_82_SQ_OBJ_2", "d_prison_82", L("Check Zanas' Echo"), L("There is something shining in the Incinerator where Zanas' Spirit was. Check it to see what it is."));
		SetPhase(QuestStatus.InProgress, "PRISON_82_OBJ_04_1", "d_prison_82", L("Gather Energy Crystals from near the Incinerator"), L("You think you hear something from the Echo but can't quite make out what it is saying. Collect Energy Crystals from near the Incinerator and give the Echo more energy."));
		SetPhase(QuestStatus.Success, "PRISON_82_SQ_OBJ_2", "d_prison_82", L("Passing energy to Zanas' Echo"), L("You have gathered enough energy. Pass the energy to Zanas' Echo."));

		AddPrerequisite(new QuestStatusPrerequisite(30194, QuestStatus.Completed));

		AddObjective("gatherTheEnergy", L("Gather Energy Crystals from near the Incinerator"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 7535));
	}
}
