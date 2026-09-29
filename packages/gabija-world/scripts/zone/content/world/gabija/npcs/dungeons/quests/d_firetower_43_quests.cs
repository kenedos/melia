//--- Melia Script ----------------------------------------------------------
// Mage Tower 3F Quest NPCs
//--- Description -----------------------------------------------------------
// The magician the tower kept locked up, the two valves that hold its magic,
// and the sealed stones on this floor.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower43QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8483);
	private readonly static QuestId Mq02 = new QuestId(8484);
	private readonly static QuestId Mq03 = new QuestId(8485);
	private readonly static QuestId Mq05 = new QuestId(8487);
	private readonly static QuestId Mq06 = new QuestId(8499);
	private readonly static QuestId Mq07 = new QuestId(8516);
	private readonly static QuestId Sq01 = new QuestId(17012);
	private readonly static QuestId Sq02 = new QuestId(17013);
	private readonly static QuestId Sq03 = new QuestId(17014);
	private readonly static QuestId Sq04 = new QuestId(17015);
	private readonly static QuestId Sq05 = new QuestId(17016);
	private readonly static QuestId Hq02 = new QuestId(19062);

	private readonly static double[,] ResearchBookSpots =
	{
		{ -1668, 537, 866 }, { -1035, 359, -147 }, { -1762, 537, 696 }, { -1524, 537, 895 },
		{ -1166.43, 358.90, -73.99 }, { -947, 359, 53 }, { -1079.01, 450.69, 449.85 },
		{ -1178.22, 393.90, 176.63 }, { -1526, 536, 398 }, { -1188.98, 379.21, -249.92 },
	};

	private readonly static double[] ResearchBookDirections = { 120, 130, 180, 210, 240, 270, 75, 55, 15, 300 };

	protected override void Load()
	{
		// Grita, at the third floor landing
		//-------------------------------------------------------------------------
		AddConditionalNpc(147449, L("Grita"), "FTOWER43_GRITA_01", "d_firetower_43", -2555, -190, 30, c => !c.Quests.HasCompleted(Mq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Oh my goodness, why now..."));
				await dialog.CompleteQuest(Mq01);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("We have to be careful here. This is the place where Antares, a dangerous magician, was imprisoned."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("We should hurry up to the next floor as soon as possible."),
					Option(L("Quick, follow me"), "accept"),
					Option(L("Let's rest for a while"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("He was a great magician before. But his ideology was too dangerous."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Behind you! He sent them the moment we set foot on the floor."));
				character.Quests.ReplayQuestTrack(Mq01);
				return;
			}

			await dialog.Msg(L("Antares was locked on this floor, and the lock is clearly no longer holding."));
		});

		// Grita, once she is walking the floor with you
		//-------------------------------------------------------------------------
		// The client has her follow the player; the port stands her in the
		// middle of the floor instead.
		AddConditionalNpc(147449, L("Grita"), "FTOWER43_G_AI", "d_firetower_43", -609.71, 48.13, 352, c => c.Quests.HasCompleted(Mq01) && !c.Quests.HasCompleted(Mq06), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("That explosion was too dangerous. Let's find a safer way to destroy these valves."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("This is it. I think I know how to break those valves now."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("Alright. We will be able to use the magic once without problems."));
				await dialog.CompleteQuest(Mq07);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("A suitable end for a crazy magician. It could have been dangerous without the defensive magic."));
				await dialog.Msg(L("He probably operated the wrong line, without knowing the first valve was destroyed..."));
				await dialog.CompleteQuest(Mq06);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("Goddess Gabija is important, but the tower is in more trouble now. Antares may already be up to something."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("There are two valves here that control the Magic Power Device. The first valve is at the Laboratory."),
					Option(L("Let's stop Antares"), "accept"),
					Option(L("Just ignore it and go up"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("Let's go and destroy it now."));
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Antares came here to do extensive research on the flow of magic. If his research materials are still present, they may help us with destroying those valves."),
					Option(L("I'll find the data"), "accept"),
					Option(L("Let's just get up there quickly"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("They should be here somewhere..."));
				}
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("Oh, I thought of something. Just a hunch I have, but let's also prepare some defensive magic."));

				var answer = await dialog.SelectQuestOffer(Mq07, L("There is a spell we can use to protect ourselves against explosions from any of Antares' experiments."),
					Option(L("What are you going to do if you can't use the spell?"), "accept"),
					Option(L("Why the spell can't be used"), "explain"),
					Option(L("I need some time to think"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("I am sorry, but I can't explain the details. All I can say is if we use magic, we will be discovered by the demons."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq07);
					await dialog.Msg(L("I think I can find a way if I have some Red Infrorocktor cores. I should be able to do something with those even without using magic."));
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				var answer = await dialog.SelectQuestOffer(Mq06, L("Now it's time for the second valve. It is in the Central Control Room, to the east."),
					Option(L("Go to the Central Control Room"), "accept"),
					Option(L("Not yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					await dialog.Msg(L("He was always curious about the secret of the tower. The reason why he was imprisoned was because he was too obsessed with that secret."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The first valve is at the Laboratory, to the west."));
				character.Quests.ClearQuestTrack(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The research materials should be here somewhere..."));
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Ten cores. The Red Infrorocktors on this floor carry them."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The Central Control Room is east of here. Be careful of what he has already touched."));
				character.Quests.ClearQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("Two valves hold the magic of this floor, and Antares wants both of them open."));
		});

		// The Magic Control Valve in the Laboratory
		//-------------------------------------------------------------------------
		AddConditionalNpc(147504, L("Magic Control Valve"), "FTOWER43_MQ_02_VALVE", "d_firetower_43", -1598, 708, 45, c => !c.Quests.IsCompletable(Mq02) && !c.Quests.HasCompleted(Mq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Magic Control Valve"));

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				var looked = await character.TimeActions.StartAsync(L("Looking the valve over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (looked != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The valve is live. It will not come apart quietly."));
				character.Quests.StartQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("One of the two valves that hold the flow of magic on this floor."));
		});

		// Antares' research, scattered through the Laboratory
		//-------------------------------------------------------------------------
		for (var i = 0; i < ResearchBookSpots.GetLength(0); i++)
		{
			var uniqueName = "FTOWER43_MQ_03_BOOK" + (i + 1);
			AddNpc(147311, L("Scattered Book"), uniqueName, "d_firetower_43", ResearchBookSpots[i, 0], ResearchBookSpots[i, 2], ResearchBookDirections[i], this.ReadResearchBook);
		}

		// The Immobile Mineloader
		//-------------------------------------------------------------------------
		AddConditionalNpc(147472, L("Immobile Mineloader"), "FTOWER43_MQ_05_MINENPC", "d_firetower_43", 669, -730, 266, c => !c.Quests.IsCompletable(Mq05) && !c.Quests.HasCompleted(Mq05), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Immobile Mineloader"));

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var looked = await character.TimeActions.StartAsync(L("Looking the Mineloader over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (looked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq05);
				character.ServerMessage(L("Something turns over inside the machine and it starts up."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("The Mineloader is up and moving somewhere in this hall."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("A Mineloader set here as a guard, and left standing long enough to gather dust."));
		});

		// The sealed stone in the 1st Library
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER43_SQ_01", "d_firetower_43", -460, -831, 90, c => !c.Quests.HasCompleted(Sq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("This is way better without the monitors."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("The pieces from my other self are able to call you now. Now, I can be released."));
				await dialog.CompleteQuest(Sq02);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("I was sealed when my spirit was ripped away by a high class demon. Please defeat the Red Infrorocktors for me. I promise you that I will not be a burden to you."),
					Option(L("Defeat the monitors"), "accept"),
					Option(L("Ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("The Monitors' Eyes are on the Red Infrorocktors."));
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("Those pieces of another me lost the voices to call you. Please find my voice again from Arma."),
					Option(L("Regain him his voice"), "accept"),
					Option(L("Ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("His creations should have voices to ask with, and ears to listen."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq01) || character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("The Monitors' Eyes are still on me."));
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// The sealed stone in the Central Control Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER43_SQ_03", "d_firetower_43", 1306, -910, 90, c => !c.Quests.HasCompleted(Sq04), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("You dealt with them well. I shall entrust you with the task of releasing this seal."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("Now, I can be released."));
				await dialog.CompleteQuest(Sq04);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("My body is restrained. The Armas are mocking me. Please teach them some manners."),
					Option(L("Seems suspicious, but grant his wish"), "accept"),
					Option(L("Just ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("You should give them a lesson so that they'll behave properly."));
				}
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("Please release this seal. When you collect the Crystals of Restriction from Flask Mages and break them, the seal will be broken."),
					Option(L("Release the seal"), "accept"),
					Option(L("You're busy so just ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("You will know when the time comes."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq03) || character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("The Armas and the Flask Mages. Neither of them will let go of me on their own."));
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// The suspicious table in the office
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Suspicious Table"), "FTOWER43_SQ_05", "d_firetower_43", 1370, -225, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Suspicious Table"));

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("Thanks. I will stay here until it becomes safe, and then run away."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("There's a Golem near here. I am too scared to go out. Please help me."),
					Option(L("Seems strange, but let's help"), "accept"),
					Option(L("Pretend you didn't hear it"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq05);

				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("It is still out there. I can hear it through the floor."));
				character.Quests.ReplayQuestTrack(Sq05);
				return;
			}

			await dialog.Msg(L("A table that talks, which is a great deal less strange than the rest of this tower."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The Central Control Room, where the second valve stands.
		AddQuestTrigger("FTOWER43_MQ_06_TRIGGER", "d_firetower_43", 1274.03, -793.38, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
			{
				character.Inventory.Remove(ItemId.FTOWER_FIRE_ESSENCE_2, 1, InventoryItemRemoveMsg.Given);
				character.Inventory.Add(ItemId.FTOWER_FIRE_ESSENCE_3, 1, InventoryAddType.PickUp);
				character.Quests.StartQuestTrack(Mq06);
			}

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Reads one of the pages of Antares' research left around the Laboratory.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task ReadResearchBook(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Scattered Book"));

		if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
		{
			var read = await character.TimeActions.StartAsync(L("Reading the research..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

			if (read != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(Mq03, "findTheResearch");

			await dialog.Msg(L("Pages on the flow of magic through the tower, and on where the valves are weakest."));
			return;
		}

		if (character.Quests.IsActive(Hq02) && !character.Quests.IsCompletable(Hq02))
		{
			var gathered = await character.TimeActions.StartAsync(L("Gathering the pages..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

			if (gathered != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(Hq02, "findTheBookPage");

			await dialog.Msg(L("The scattered pages of the book Simon Shaw asked after, gathered up at last."));
			return;
		}

		await dialog.Msg(L("A book from the Laboratory, its pages spread across the floor."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8483: Lunatic Wizard (1)
//-----------------------------------------------------------------------------
public class Ftower43Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8483);
		SetName(L("Lunatic Wizard (1)"));
		SetDescription(L("Antares is loose on the third floor, and he throws his Red Infrorocktors at the landing."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_GRITA_01", "d_firetower_43", L("Talk to Grita at Mage Tower 3F"), L("Grita is waiting at the third floor of Mage Tower. Go to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_GRITA_01", "d_firetower_43", L("Defeat Red Infrorocktors"), L("It seems that the magician Antares is loose. Defeat the Red Infrorocktor that Antares has summoned."));
		SetPhase(QuestStatus.Success, "FTOWER43_GRITA_01", "d_firetower_43", L("Talk to Grita"), L("You defeated the Red Infrorocktors which Antares summoned. Talk to Grita."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER43_MQ_01_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8482, QuestStatus.Completed));

		AddObjective("killRocktors", L("Defeat Red Infrorocktors"), new KillObjective(6, "InfroRocktor_red") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8484: Lunatic Wizard (2)
//-----------------------------------------------------------------------------
public class Ftower43Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8484);
		SetName(L("Lunatic Wizard (2)"));
		SetDescription(L("The first of the two Magic Control Valves stands in the Laboratory."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_G_AI", "d_firetower_43", L("Talk to Grita"), L("It was the mad magician, Antares who summoned the Krumadoks. Talk to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_MQ_02_VALVE", "d_firetower_43", L("Destroy the first Magic Control Valve"), L("Grita told you to break the Magic Control Valve to stop Antares. Destroy the valve at the Laboratory."));
		SetPhase(QuestStatus.Success, "FTOWER43_G_AI", "d_firetower_43", L("Talk to Grita"), L("As you destroyed the valve, it exploded. While it cleared away the monsters, it was really dangerous. Talk to Grita again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER43_MQ_02_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8483, QuestStatus.Completed));

		AddObjective("breakFirstValve", L("Destroy the Magic Control Valve"), new KillObjective(1, "firetower_valve_01") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8485: Lunatic Wizard (3)
//-----------------------------------------------------------------------------
public class Ftower43Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8485);
		SetName(L("Lunatic Wizard (3)"));
		SetDescription(L("Antares studied the flow of magic here, and what he wrote down is still on the Laboratory floor."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_G_AI", "d_firetower_43", L("Talk to Grita"), L("The Magic Control Valve was way more powerful than you expected. You better discuss it with Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_MQ_03_BOOK1", "d_firetower_43", L("Look for info on the Magic Control Valve"), L("Grita told you that since Antares did research on the flow of magic, the research information must be left nearby. Search Antares' books and find information on the valves."));
		SetPhase(QuestStatus.Success, "FTOWER43_G_AI", "d_firetower_43", L("Talk to Grita"), L("It seems that you found information on the flow of magic. Talk to Grita."));

		AddPrerequisite(new QuestStatusPrerequisite(8484, QuestStatus.Completed));

		AddObjective("findTheResearch", L("Look for info on the Magic Control Valve"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8487: Immobile Security Device
//-----------------------------------------------------------------------------
public class Ftower43Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8487);
		SetName(L("Immobile Security Device"));
		SetDescription(L("A Mineloader was set here as a guard, and it has not moved in years."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_MQ_05_MINENPC", "d_firetower_43", L("Check the Immobile Mineloader"), L("There is a Mineloader at the Mage Tower that acts as a guard. Check the immobile Mineloader."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_MQ_05_MINENPC", "d_firetower_43", L("Defeat the hostile Mineloader"), L("Defeat the suddenly mobile Mineloader."));
		SetPhase(QuestStatus.Success, "FTOWER43_MQ_05_MINENPC", "d_firetower_43", L("Defeat the hostile Mineloader"), L("Defeat the suddenly mobile Mineloader."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER43_MQ_05_TRACK", "m_boss_c", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(109));

		AddObjective("killMineloader", L("Defeat the hostile Mineloader"), new KillObjective(1, "boss_mineloader") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Mineloader is down and the hall is quiet again."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8516: Lunatic Wizard (4)
//-----------------------------------------------------------------------------
public class Ftower43Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8516);
		SetName(L("Lunatic Wizard (4)"));
		SetDescription(L("The defensive magic against the next explosion wants Red Infrorocktor cores."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_G_AI", "d_firetower_43", L("Talk to Grita"), L("Grita seems to have remembered something. Talk to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_G_AI", "d_firetower_43", L("Collect the cores of Red Infrorocktors"), L("Grita wants to prepare defensive magic that protects against explosions that will come from destroying the valves. Bring the cores of Red Infrorocktor that is used for this defensive magic."));
		SetPhase(QuestStatus.Success, "FTOWER43_G_AI", "d_firetower_43", L("Talk to Grita"), L("Collected the cores of Red Infrorocktor. Talk to Grita again."));

		AddPrerequisite(new QuestStatusPrerequisite(8485, QuestStatus.Completed));

		AddPityDrop("FTOWER43_MQ_07_ITEM", 1.0f, 0, 1, "InfroRocktor_red");

		AddObjective("collectCores", L("Collect the cores of Red Infrorocktors"), new CollectItemObjective("FTOWER43_MQ_07_ITEM", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER43_MQ_07_ITEM"));
	}
}

// 8499: Lunatic Wizard (5)
//-----------------------------------------------------------------------------
public class Ftower43Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8499);
		SetName(L("Lunatic Wizard (5)"));
		SetDescription(L("Antares reaches the second valve first, and he does not know the first one is already gone."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "FTOWER43_G_AI", "d_firetower_43", L("Talk to Grita"), L("Now it's time to destroy the 2nd Valve. Talk to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_MQ_06_TRIGGER", "d_firetower_43", L("Find the 2nd Valve in the Central Control Room"), L("Grita told you that the 2nd Valve is in the Central Control Room. Go to the Central Control Room."));
		SetPhase(QuestStatus.Success, "FTOWER43_G_AI", "d_firetower_43", L("Talk to Grita"), L("Antares touched the wrong part of the valve, so the valve exploded. Talk to Grita."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER43_MQ_06_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8516, QuestStatus.Completed));

		AddObjective("seeTheSecondValve", L("Find the 2nd Valve in the Central Control Room"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new SelectItemReward("R_FOOT02_178", "R_FOOT02_179", "R_FOOT02_180"));
	}
}

// 17012: What Kind of Sin (1)
//-----------------------------------------------------------------------------
public class Ftower43Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17012);
		SetName(L("What Kind of Sin (1)"));
		SetDescription(L("A stone in the 1st Library wants the Red Infrorocktors that watch it taken off."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_SQ_01", "d_firetower_43", L("Talk to the Sealed Stone"), L("There is a Sealed Stone in the 1st Library. Talk to it."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_SQ_01", "d_firetower_43", L("Defeat Red Infrorocktors"), L("The Sealed Stone told you to defeat the monitors for it and that it won't bother you afterwards. Defeat Red Infrorocktors for the Sealed Stone."));
		SetPhase(QuestStatus.Success, "FTOWER43_SQ_01", "d_firetower_43", L("Report to the Sealed Stone"), L("You defeated the Red Infrorocktors as the Stone wanted. Return to the Stone and talk to it."));

		AddPrerequisite(new LevelPrerequisite(109));

		AddObjective("killRocktors", L("Defeat Red Infrorocktors"), new KillObjective(10, "InfroRocktor_red"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17013: What Kind of Sin (2)
//-----------------------------------------------------------------------------
public class Ftower43Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17013);
		SetName(L("What Kind of Sin (2)"));
		SetDescription(L("The other pieces of the same soul have lost the voices they called with."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_SQ_01", "d_firetower_43", L("Talk to the Sealed Stone"), L("The Sealed Stone has more favors to ask. Talk to the Sealed Stone."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_SQ_01", "d_firetower_43", L("Obtain the Sealed Stone's Voices by defeating Armas"), L("The Sealed Stone told you that its other spirit lost its voice and wants you to retrieve the voice. Collect the Sealed Stone's Voices from Armas."));
		SetPhase(QuestStatus.Success, "FTOWER43_SQ_01", "d_firetower_43", L("Return and talk to the Sealed Stone"), L("Gathered all the voices. Return and give it to the Sealed Stone."));

		AddPrerequisite(new QuestStatusPrerequisite(17012, QuestStatus.Completed));

		AddPityDrop("FTOWER43_SQ_01_01", 0.5f, 4, 1, "arma");

		AddObjective("collectVoices", L("Obtain the Sealed Stone's Voices by defeating Armas"), new CollectItemObjective("FTOWER43_SQ_01_01", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER43_SQ_01_01"));
	}
}

// 17014: Too Many Seals (1)
//-----------------------------------------------------------------------------
public class Ftower43Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17014);
		SetName(L("Too Many Seals (1)"));
		SetDescription(L("The stone in the Central Control Room has had enough of the Armas around it."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_SQ_03", "d_firetower_43", L("Talk to the Sealed Stone"), L("There is a Sealed Stone in the Central Control Room. Talk to it."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_SQ_03", "d_firetower_43", L("Defeat Arma that mocked the Sealed Stone"), L("The Sealed Stone asked you to defeat the Arma that mocked it. Defeat Arma for the Stone."));
		SetPhase(QuestStatus.Success, "FTOWER43_SQ_03", "d_firetower_43", L("Report to the Sealed Stone"), L("Defeated Arma that mocked the Stone. Report to the Stone."));

		AddPrerequisite(new LevelPrerequisite(109));

		AddObjective("killArmas", L("Defeat Arma"), new KillObjective(10, "arma"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17015: Too Many Seals (2)
//-----------------------------------------------------------------------------
public class Ftower43Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17015);
		SetName(L("Too Many Seals (2)"));
		SetDescription(L("The seal breaks when the Crystals of Restriction the Flask Mages carry are broken."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_SQ_03", "d_firetower_43", L("Talk to the Sealed Stone"), L("The spirit of the Sealed Stone wants to be freed. Talk to it."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_SQ_03", "d_firetower_43", L("Defeat the Flask Mage and get the Crystal of Restriction"), L("The spirit of the Sealed Stone wants to be freed from the seal. Gather the Crystal of Restriction from the Flask Mage and break them."));
		SetPhase(QuestStatus.Success, "FTOWER43_SQ_03", "d_firetower_43", L("Report to the Sealed Stone"), L("Collected all the crystals. Return and report to the Sealed Stone."));

		AddPrerequisite(new QuestStatusPrerequisite(17014, QuestStatus.Completed));

		AddPityDrop("FTOWER43_SQ_02_01", 0.5f, 4, 1, "Flask_mage");

		AddObjective("collectCrystals", L("Defeat the Flask Mage and get the Crystal of Restriction"), new CollectItemObjective("FTOWER43_SQ_02_01", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new SelectItemReward("R_HAND02_178", "R_HAND02_179", "R_HAND02_180"));
		AddReward(new TakeItemReward("FTOWER43_SQ_02_01"));
	}
}

// 17016: Scream in the Silence
//-----------------------------------------------------------------------------
public class Ftower43Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17016);
		SetName(L("Scream in the Silence"));
		SetDescription(L("A table in the office would rather not be found by the Gray Golem outside."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER43_SQ_05", "d_firetower_43", L("Find the suspicious table"), L("There is a suspicious table in the office. Can you talk to it?"));
		SetPhase(QuestStatus.InProgress, "FTOWER43_SQ_05", "d_firetower_43", L("Defeat Gray Golem"), L("As a suspicious desk has told you, the Gray Golem is chasing after you. It will be better to defeat the Gray Golem first."));
		SetPhase(QuestStatus.Success, "FTOWER43_SQ_05", "d_firetower_43", L("Talk to the suspicious table"), L("It seems like you got out of danger. Talk to the weird and polite table."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER43_SQ_05_TRACK", "m_boss_d", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(109));

		AddObjective("killGrayGolem", L("Defeat Gray Golem"), new KillObjective(1, "boss_golem_Gray_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
	}
}
