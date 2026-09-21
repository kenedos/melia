//--- Melia Script ----------------------------------------------------------
// Demon Prison District 1 Quest NPCs
//--- Description -----------------------------------------------------------
// The two Kupoles holding the first district, the altar Hauberk drains, and
// the barrier Blut is kept behind.
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

public class DVelniasprison511QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Pre02 = new QuestId(60001);
	private readonly static QuestId Mq01 = new QuestId(60002);
	private readonly static QuestId Mq02 = new QuestId(60003);
	private readonly static QuestId Mq03 = new QuestId(60004);
	private readonly static QuestId Mq04 = new QuestId(60005);
	private readonly static QuestId Mq05 = new QuestId(60006);
	private readonly static QuestId Sq01 = new QuestId(60029);
	private readonly static QuestId Sq02 = new QuestId(60030);
	private readonly static QuestId Baiga60 = new QuestId(90175);
	private readonly static QuestId Baiga70 = new QuestId(90176);

	protected override void Load()
	{
		// Demon Lord Hauberk
		//-------------------------------------------------------------------------
		AddConditionalNpc(57840, L("Demon Lord Hauberk"), "VPRISON511_MQ_HAUBERK", "d_velniasprison_51_1", -11, -4, -65, this.IsHauberkStillWalking, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Demon Lord Hauberk"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Well then, I will be inside your little pocket now."));
				await dialog.Msg(L("Don't forget. Trust no one."));
				await dialog.CompleteQuest(Mq01);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The Kupole is watching us both. Let her say what she came to say."));
				character.Quests.ReplayQuestTrack(Mq01);
				return;
			}

			await dialog.Msg(L("A demon lord standing in a prison he was never sentenced to."));
		});

		// Kupole Audra
		//-------------------------------------------------------------------------
		AddConditionalNpc(154011, L("Kupole Audra"), "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", -162.73, -69.57, 109, this.IsAudraOnPost, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Audra"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Well done. You have prevented the worst case scenario."));
				await dialog.Msg(L("Now it is time to stem Demon Lord Blut's power."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("This will be a great help to Zydrone."));
				await dialog.Msg(L("I will eliminate these. Blut should feel like he's lost his senses right now."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Good work."));
				await dialog.Msg(L("It's a relief that those demons did not notice the weakening of the crack to the outside world."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("I hope there are no more sacrifices like this..."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("Revelator."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("After Medzio Diena, Vakarine's power has become weak, so we have to protect her."),
					Option(L("I will lend you my power"), "accept"),
					Option(L("I am not competent enough yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("Please first stop the demons that are trying to release the Demon Lord Blut that is locked here."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Right now, Kupole Zydrone is holding down Blut. But since Vakarine's power has waned after Medzio Diena, we are not like what we were before."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("About Demon Lord Blut"), "explain"),
					Option(L("I need more preparation"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("He was the head of the demons that are locked in this district."));
					await dialog.Msg(L("They were trying to control this prison somehow and looking for chances to escape from here."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("Take Blut's marks from the demons of the Bjaurer Hideout."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("To defeat the Demon Lord Blut completely, our power is not enough. I didn't want to do this before, but let's try getting some help from Hauberk."),
					Option(L("I will try"), "accept"),
					Option(L("About the Kupoles"), "explain"),
					Option(L("Tell her to wait a bit"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("We are the attendants who are serving the goddesses."));
					await dialog.Msg(L("Our work differs depending on which goddess we serve."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("After Hauberk absorbs that power, please go help Zydrone right away."));
					await dialog.Msg(L("To Nuzikalti Hall. Hurry. We don't have much time."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("The demons that lost Blut are causing a rampage. If they escape to the outside world, it will be a great disaster."),
					Option(L("I will defeat those demons"), "accept"),
					Option(L("I will be fine"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("The barrier is gradually collapsing."));
					return;
				}
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("Human exploration teams came in from the outside world in the past. Unfortunately, they all died before we could do anything."),
					Option(L("I will look for keepsakes"), "accept"),
					Option(L("Tell her that there is a more emergent issue"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("Humans should not ever come here."));
					return;
				}
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Demon Lord Blut's servants are trying to help him escape here."));
				await dialog.Msg(L("Hauberk's help will be useful this time."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Vakarine and the other Kupoles are also having hard time."));
				await dialog.Msg(L("Let's finish our task here fast and go to a different district to help."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The power of the demons reject the power of the goddesses."));
				await dialog.Msg(L("This role completely depends on Hauberk."));
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("The barrier is gradually collapsing."));
				await dialog.Msg(L("I don't know what would happen even if we defeated the demons..."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Humans should not ever come here."));
				return;
			}

			await dialog.Msg(L("Vakarine's power has waned, and this district is held by less than it was."));
		});

		// Kupole Zydrone
		//-------------------------------------------------------------------------
		AddConditionalNpc(154015, L("Kupole Zydrone"), "VPRISON511_MQ_ZYDRONE", "d_velniasprison_51_1", -1809.14, -485.56, -76, this.IsZydroneOnPost, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Zydrone"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("So you are the one who Vakarine talked about."));
				await dialog.Msg(L("Well done. Everything will be done with goddess' will."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("We were able to defeat Blut because of you."));
				await dialog.Msg(L("You brought us hope."));
				await dialog.CompleteQuest(Mq05);
				character.ServerMessage(L("Move to the 2nd District of the Demon Prison and meet Kupole Arune!"));
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("We've locked Blut across this barrier."),
					Option(L("All prepared"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Blut is across the barrier still. Hold the line with us."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Hauberk has to take the altar's power first. Audra will have told you that much."));
				return;
			}

			await dialog.Msg(L("A Kupole standing watch on Nuzikalti Hall, and tired of it."));
		});

		// Blut's Altar
		//-------------------------------------------------------------------------
		AddNpc(41327, L("Blut's Altar"), "VPRISON511_MQ_04_NPC", "d_velniasprison_51_1", -1831.05, 510.96, -19, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Blut's Altar"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				var examined = await character.TimeActions.StartAsync(L("Examining the altar..."), L("Cancel"), "LOOK_SIT", TimeSpan.FromSeconds(2));

				if (examined != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq04);
				return;
			}

			await dialog.Msg(L("An altar built to gather what the prison was meant to hold in."));
		});

		// The barrier Blut is kept behind
		//-------------------------------------------------------------------------
		AddConditionalNpc(154001, L("Sealing Barrier"), "VPRISON511_MQ_05_1", "d_velniasprison_51_1", -1914.78, -475.05, 165, this.IsBarrierStanding, async dialog =>
		{
			await dialog.Msg(L("A barrier of the goddess' own making, and it is thinner than it was."));
		});

		AddConditionalNpc(154003, L("Demon Lord Blut"), "VPRISON511_MQ_05_BLUT", "d_velniasprison_51_1", -2009.62, -461.11, 90, this.IsBarrierStanding, async dialog =>
		{
			await dialog.Msg(L("Something the size of a house, pacing the far side of the barrier."));
		});

		// Abandoned Journal
		//-------------------------------------------------------------------------
		AddNpc(147311, L("Abandoned Journal"), "VPRISON_PAPER01", "d_velniasprison_51_1", -300, 1623, 90, async dialog =>
		{
			dialog.SetTitle(L("Abandoned Journal"));

			await dialog.Msg(L("...the third day in the hideout. The demons here do not sleep, and neither do we."));
			await dialog.Msg(L("...whoever finds this, do not go further in. Nothing in this prison was put here by accident."));
		});

		// Order to defeat the Demons
		//-------------------------------------------------------------------------
		AddNpc(147312, L("Order to defeat the Demons"), "VPRISON_PAPER03", "d_velniasprison_51_1", -491, -159, 90, async dialog =>
		{
			dialog.SetTitle(L("Order to defeat the Demons"));

			await dialog.Msg(L("An order signed by a house that does not exist any more, sending men in here to put the demons down."));
		});

		// Link Tracking Device
		//-------------------------------------------------------------------------
		AddNpc(154064, L("Link Tracking Device"), "LOWLV_EYEOFBAIGA_SQ_20", "d_velniasprison_51_1", 531.98, 1426.38, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Link Tracking Device"));

			if (!character.Quests.Has(Baiga70) && character.Quests.MeetsPrerequisites(Baiga70))
			{
				await dialog.Msg(L("The tracking device will now temporarily link you with the owner of the Golems."));

				var answer = await dialog.SelectQuestOffer(Baiga70, L("The needle has stopped swinging, and something on the other end of the link is waiting."),
					Option(L("Listen to the link"), "accept"),
					Option(L("Break the link off"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Baiga70);
					character.Quests.CompleteObjective(Baiga70, "hearVaiga");
					await dialog.Msg(L("Wondering why you still can't see anything?"));
					await dialog.Msg(L("There's nothing you can do about it, so keep quiet and listen."));
					character.ServerMessage(L("Report what Vaiga said to the Linker Master."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Baiga70))
			{
				await dialog.Msg(L("The link is spent. The Linker Master will want to hear it from you."));
				return;
			}

			await dialog.Msg(L("A tracking device left standing in the hideout, its needle swinging."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The arrival that plays the meeting with Audra.
		AddQuestTrigger("VPRISON511_MQ_01_NPC", "d_velniasprison_51_1", -13.20, -3.11, 500, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Pre02))
			{
				character.Quests.CompleteObjective(Pre02, "enterPrison");
				character.Quests.Complete(Pre02);
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
				character.Quests.Start(Mq01);

			if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
				character.Quests.StartQuestTrack(Mq01);

			await Task.CompletedTask;
		});

		// The spot the tracking device points at, where the Gazing Golem comes through.
		AddQuestTrigger("LOWLV_EYEOFBAIGA_SQ_60", "d_velniasprison_51_1", 307.36, 1417.83, 120, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Baiga60) && character.Quests.MeetsPrerequisites(Baiga60))
			{
				character.Quests.Start(Baiga60);
				character.ServerMessage(L("You installed the device! Seeing how it's reacting, it looks like a portal is about to open!"));
			}

			if (character.Quests.IsActive(Baiga60) && !character.Quests.IsCompletable(Baiga60))
				character.Quests.StartQuestTrack(Baiga60);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether Hauberk is still walking beside the player rather than
	/// riding in their pack.
	/// </summary>
	/// <param name="character"></param>
	private bool IsHauberkStillWalking(Character character)
		=> character.Quests.Has(Mq01) && !character.Quests.HasCompleted(Mq01);

	/// <summary>
	/// Returns whether Audra has shown herself to the player.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAudraOnPost(Character character)
		=> character.Quests.IsActive(Mq01) || character.Quests.HasCompleted(Mq01);

	/// <summary>
	/// Returns whether Zydrone is still holding Nuzikalti Hall.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZydroneOnPost(Character character)
		=> !character.Quests.HasCompleted(Mq05);

	/// <summary>
	/// Returns whether Blut is still behind the sealing barrier.
	/// </summary>
	/// <param name="character"></param>
	private bool IsBarrierStanding(Character character)
		=> !character.Quests.Has(Mq05);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60002: A Place Unreachable (3)
//-----------------------------------------------------------------------------
public class Vprison511Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60002);
		SetName(L("A Place Unreachable (3)"));
		SetDescription(L("Kupole Audra meets the Revelator at the prison gate, and she has seen what is riding with them."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "VPRISON511_MQ_01_NPC", "d_velniasprison_51_1", L("Enter the 1st District of the Demon's Prison"), L("You've completed the contract with Hauberk. He is going to travel with you until you find Vakarine. Now, go to the 1st District of the Demon's Prison with Hauberk!"));
		SetPhase(QuestStatus.InProgress, "VPRISON511_MQ_01_NPC", "d_velniasprison_51_1", L("Enter the 1st District of the Demon's Prison"), L("You've completed the contract with Hauberk. He is going to travel with you until you find Vakarine. Now, go to the 1st District of the Demon's Prison with Hauberk!"));
		SetPhase(QuestStatus.Success, "VPRISON511_MQ_HAUBERK", "d_velniasprison_51_1", L("Talk to Hauberk's Spirit"), L("Talk to Hauberk's Spirit."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON511_MQ_01_TRACK", 1000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60001, QuestStatus.Completed));

		AddObjective("meetAudra", L("Talk to Hauberk's Spirit"), new ManualObjective());
	}
}

// 60003: The Teeth of Revenge (1)
//-----------------------------------------------------------------------------
public class Vprison511Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60003);
		SetName(L("The Teeth of Revenge (1)"));
		SetDescription(L("The demons of the Rituala Assembly Area are working Blut's escape loose."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Talk to Kupole Audra"), L("Talk to Kupole Audra at the Demon's Prison."));
		SetPhase(QuestStatus.InProgress, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Defeat the demons performing the ritual"), L("Kupole Audra asked for your help to defeat the demons that are performing rituals at the Rituala Assembly Area to help Demon Lord Blut escape."));
		SetPhase(QuestStatus.Success, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Talk to Kupole Audra"), L("You've interrupted the ritual for Blut's escape. Go back and report to Kupole Audra at the Corridor of Monitor."));

		AddPrerequisite(new QuestStatusPrerequisite(60002, QuestStatus.Completed));

		AddObjective("killRitualists", L("Defeat the demons performing the ritual"), new KillObjective(8, "yognome_yellow", "Egnome_yellow", "Gazing_Golem_yellow", "Moya_yellow"));

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 60004: The Teeth of Revenge (2)
//-----------------------------------------------------------------------------
public class Vprison511Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60004);
		SetName(L("The Teeth of Revenge (2)"));
		SetDescription(L("Blut's marks are what carries his hold over the district, and the demons of the Bjaurer Hideout wear them."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Talk to Kupole Audra"), L("Talk to Kupole Audra at the Demon's Prison."));
		SetPhase(QuestStatus.InProgress, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Obtain Blut's Marks"), L("Kupole Audra told you that you need to weaken the influence of Demon Lord Blut. Take the Blut's symbol from demons in Bjaurer Hideout."));
		SetPhase(QuestStatus.Success, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Give them to Kupole Audra"), L("You have stolen all of Blut's marks from the demons. Take them to Kupole Audra at Demon Prison."));

		AddPrerequisite(new QuestStatusPrerequisite(60003, QuestStatus.Completed));

		AddObjective("collectMarks", L("Collect Blut's Mark by defeating the demons"), new CollectItemObjective("VPRISON511_MQ_03_ITEM", 7));

		AddPityDrop("VPRISON511_MQ_03_ITEM", 0.7f, 3, 1, "yognome_yellow", "Egnome_yellow", "Gazing_Golem_yellow", "Moya_yellow");

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("VPRISON511_MQ_03_ITEM"));
	}
}

// 60005: The Teeth of Revenge (3)
//-----------------------------------------------------------------------------
public class Vprison511Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60005);
		SetName(L("The Teeth of Revenge (3)"));
		SetDescription(L("Only a demon can take what Blut gathered, so Hauberk drains the altar while the player holds the room."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Talk to Kupole Audra"), L("There is still work left to do in order to face Blut. Talk to Kupole Audra."));
		SetPhase(QuestStatus.InProgress, "VPRISON511_MQ_04_NPC", "d_velniasprison_51_1", L("Help Hauberk absorb Blut's powers"), L("Kupole Audra says the power gathered by Blut can only be controlled by Hauberk. Help Hauberk absorb the power of Blut from the Blut Altar in Concentrated Management Area."));
		SetPhase(QuestStatus.Success, "VPRISON511_MQ_ZYDRONE", "d_velniasprison_51_1", L("Talk to Kupole Zydrone"), L("Hauberk successfully absorbed Blut's powers. Go and help Kupole Zydrone in Nuzikalti Hall."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON511_MQ_04_TRACK", 6000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60004, QuestStatus.Completed));

		AddObjective("drainAltar", L("Help Hauberk absorb Blut's powers"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 3));
	}
}

// 60006: Old Pride
//-----------------------------------------------------------------------------
public class Vprison511Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60006);
		SetName(L("Old Pride"));
		SetDescription(L("The barrier comes down, and Blut is put down with Zydrone and Hauberk beside you."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON511_MQ_ZYDRONE", "d_velniasprison_51_1", L("Talk to Kupole Zydrone"), L("Talk to Kupole Zydrone."));
		SetPhase(QuestStatus.InProgress, "VPRISON511_MQ_ZYDRONE", "d_velniasprison_51_1", L("Punishment of the Demon Lord Blut"), L("Prepared to defeat Demon Lord Blut. Help Kupole Zydrone and defeat Blut."));
		SetPhase(QuestStatus.Success, "VPRISON511_MQ_ZYDRONE", "d_velniasprison_51_1", L("Talk to Kupole Zydrone"), L("Defeated the Demon Lord Blut. Talk to Kupole Zydrone."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON511_MQ_05_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60005, QuestStatus.Completed));

		AddObjective("killBlut", L("Defeat Blut"), new KillObjective(1, "boss_Blud") { LayerOnly = true });

		AddReward(new ItemReward("expCard8", 4));
	}
}

// 60029: Shutting the Door
//-----------------------------------------------------------------------------
public class Vprison511Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60029);
		SetName(L("Shutting the Door"));
		SetDescription(L("With Blut gone the demons of the district are making for the crack to the outside world."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Kupole Audra"), L("Kupole Audra is planning to take care of the rest of the demons. Talk to Kupole Audra."));
		SetPhase(QuestStatus.InProgress, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Defeat the monsters that are trying to escape"), L("With their commander lost, the demons are trying to escape the Demon Prison. Defeat the demons that are trying to escape."));
		SetPhase(QuestStatus.Success, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Talk to Kupole Audra"), L("Defeated the demons trying to escape. Report to Kupole Audra."));

		AddPrerequisite(new LevelPrerequisite(141));
		AddPrerequisite(new QuestStatusPrerequisite(60006, QuestStatus.Completed));

		AddObjective("killEscapers", L("Defeat demons"), new KillObjective(10, "yognome_yellow", "Egnome_yellow", "Gazing_Golem_yellow", "Moya_yellow"));

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 60030: According To One's Duty
//-----------------------------------------------------------------------------
public class Vprison511Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60030);
		SetName(L("According To One's Duty"));
		SetDescription(L("A human exploration team died in the hideout, and the demons still carry what they left."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Talk to Kupole Audra"), L("Kupole Audra still has some favors to ask of you. Talk to Kupole Audra."));
		SetPhase(QuestStatus.InProgress, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Collect the keepsakes of the investigation team"), L("Kupole Audra requested you to defeat the demons at Bjaurer Hideout and collect the belongings of the investigation team."));
		SetPhase(QuestStatus.Success, "VPRISON511_MQ_AUDRA", "d_velniasprison_51_1", L("Give them to Kupole Audra"), L("Gathered all the keepsakes. Give it to Kupole Audra."));

		AddPrerequisite(new LevelPrerequisite(141));
		AddPrerequisite(new QuestStatusPrerequisite(60006, QuestStatus.Completed));

		AddObjective("collectKeepsakes", L("Obtain the belongings of the investigation team"), new CollectItemObjective("VPRISON511_SQ_02_ITEM", 8));

		AddPityDrop("VPRISON511_SQ_02_ITEM", 1.0f, 0, 1, "yognome_yellow", "Egnome_yellow", "Gazing_Golem_yellow", "Moya_yellow");

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("VPRISON511_SQ_02_ITEM"));
	}
}

// 90175: The Eye of Demon Lord (1)
//-----------------------------------------------------------------------------
public class LowlvEyeofbaigaSq60Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90175);
		SetName(L("The Eye of Demon Lord (1)"));
		SetDescription(L("The Linker Master's tracker has found a portal opening in the Bjaurer Hideout."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "LOWLV_EYEOFBAIGA_SQ_60", "d_velniasprison_51_1", L("Contact with Linker Master"), L("The Linker Master seems to have something to say to you. Camouflage Crystal sent you a message."));
		SetPhase(QuestStatus.InProgress, "LOWLV_EYEOFBAIGA_SQ_60", "d_velniasprison_51_1", L("Go to the Link Tracking Device"), L("The Linker Tracker has detected something. Go to it immediately."));
		SetPhase(QuestStatus.Success, "LOWLV_EYEOFBAIGA_SQ_60", "d_velniasprison_51_1", L("Go to the Link Tracking Device"), L("The Linker Tracker has detected something. Go to it immediately."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "LOWLV_EYEOFBAIGA_SQ_60_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(90174, QuestStatus.Completed));

		AddObjective("killGolem", L("Defeat Gazing Golem"), new KillObjective(1, "boss_GazingGolem_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard13", 2));
		AddReward(new ItemReward("misc_ore17", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Gazing Golem is down, and the portal it came through is closing."));
		character.Quests.Complete(this.QuestId);
	}
}

// 90176: The Eye of Demon Lord (2)
//-----------------------------------------------------------------------------
public class LowlvEyeofbaigaSq70Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(90176);
		SetName(L("The Eye of Demon Lord (2)"));
		SetDescription(L("The tracker links the Revelator to whoever sends the Golems, and the Linker Master wants the account."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_1", "c_orsha");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "LOWLV_EYEOFBAIGA_SQ_60", "d_velniasprison_51_1", L("Check the Demon Sent by Gazing Golem"), L("You must find out which demon was sent by Gazing Golem."));
		SetPhase(QuestStatus.Success, "JOB_2_LINKER_MASTER", "c_orsha", L("Report to the Linker Master"), L("Talk to the Linker Master about who is behind Gazing Golem."));

		AddPrerequisite(new QuestStatusPrerequisite(90175, QuestStatus.Completed));

		AddObjective("hearVaiga", L("Check the Demon Sent by Gazing Golem"), new ManualObjective());

		AddReward(new TakeItemReward("LOWLV_EYEOFBAIGA_SQ_50_ITEM2"));
	}
}
