//--- Melia Script ----------------------------------------------------------
// Tenet Church 2F Quest NPCs
//--- Description -----------------------------------------------------------
// Follower Algis, the altars of the central hall and the sanctuary the
// revelation is hidden in.
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

public class DChapel577QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8528);
	private readonly static QuestId Mq02 = new QuestId(8529);
	private readonly static QuestId Mq03 = new QuestId(8530);
	private readonly static QuestId Mq04 = new QuestId(8531);
	private readonly static QuestId Mq05 = new QuestId(8532);
	private readonly static QuestId Mq06 = new QuestId(8533);
	private readonly static QuestId Mq07 = new QuestId(8534);
	private readonly static QuestId Mq09 = new QuestId(8536);
	private readonly static QuestId Mq10 = new QuestId(8537);

	protected override void Load()
	{
		// Follower Algis at the cathedral door
		//-------------------------------------------------------------------------
		AddNpc(147390, L("Follower Algis"), "CHAPLE577_ARUNE_01", "d_chapel_57_7", -634, -934, 81, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Algis"));
			dialog.SetPortrait("Dlg_port_algis");

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Gesti has made the first move."));
				await dialog.Msg(L("It will be difficult to approach her secretly."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("You need the Seal of Space in order to enter the hidden sanctuary."));
				var answer = await dialog.SelectQuestOffer(Mq01, L("It's hidden in the Sventove Central Altar, but I just hope it's not too late."),
					Option(L("Let's go and find"), "accept"),
					Option(L("Hide first"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq01);

				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("We must change our plan."));
				var answer = await dialog.SelectQuestOffer(Mq02, L("First, we'll need to go observe Gesti's actions."),
					Option(L("Begin immediately"), "accept"),
					Option(L("Gesti might still be around"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq02);

				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("We need the Seal of Space before we move."));
				character.Quests.ReplayQuestTrack(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The Bell Tower is the best place to watch her from."));
				character.Quests.ReplayQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("The second floor is Gesti's. We move carefully here."));
		});

		// Follower Algis at the bell tower
		//-------------------------------------------------------------------------
		AddNpc(147390, L("Follower Algis"), "CHAPLE577_ARUNE_02", "d_chapel_57_7", 110, -579, 180, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Algis"));
			dialog.SetPortrait("Dlg_port_algis");

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("From here, we can see what Gesti is up to."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("The Seal of Space can only be used by the Revelator, so she won't be able to find the revelation right away."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Seems like Gesti has not noticed yet."));
				await dialog.Msg(L("We better prepare the Divine Sphere."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("So you activated the Malda Altar?"));
				await dialog.Msg(L("Now it will be safe to stay here for the time being. Good work."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("It looks like even the Auka Altar lost its powers."));
				await dialog.Msg(L("There were stories about its powers wiping out dozens of demons in the past."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("Thank you for your hard work."));
				await dialog.Msg(L("When this is over, I plan to gather all the brothers and drive them away at once."));
				await dialog.CompleteQuest(Mq07);
				return;
			}

			if (character.Quests.IsActive(Mq09) && character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("Gesti fled, wounded. We have the church back."));
				await dialog.Msg(L("But there are still more things left to do here."));
				await dialog.CompleteQuest(Mq09);
				return;
			}

			if (character.Quests.IsActive(Mq10) && character.Quests.IsCompletable(Mq10))
			{
				await dialog.Msg(L("It's the revelation."));
				await dialog.Msg(L("Cherish it and do not show it to anyone else."));
				await dialog.CompleteQuest(Mq10);
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("Alright, bring back the Seal of Space from the Sventove Central Altar."));
				var answer = await dialog.SelectQuestOffer(Mq03, L("Can you do this?"),
					Option(L("I will bring it back secretly"), "accept"),
					Option(L("About the Seal of Space"), "explain"),
					Option(L("I'll observe the situation a little more"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("You must realize that the revelation is hidden in the sanctuary."));
					await dialog.Msg(L("The Seal of Space is the single key that will get you to the sanctuary."));
					return;
				}

				if (answer == "accept")
					character.Quests.Start(Mq03);

				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("We can't do anything about the destroyed altar, but the fragments still have power."));
				var answer = await dialog.SelectQuestOffer(Mq04, L("Gather the pieces and insert them into the 8 pillars of the Sventove Central Hall."),
					Option(L("I'll be cautious on my way"), "accept"),
					Option(L("Check Gesti's action and go"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("I'll ring a bell to let you know when Gesti gets close, so please listen for it."));
					character.Quests.Start(Mq04);
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("It is too much for me to handle alone without any help from the other brothers."));
				var answer = await dialog.SelectQuestOffer(Mq05, L("I'd better use the Malda Altar."),
					Option(L("I'll activate the altar"), "accept"),
					Option(L("It will be okay"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq05);

				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("Demons are going around the second floor searching every corner."));
				var answer = await dialog.SelectQuestOffer(Mq06, L("Activating the Auka Altar will divert their attention."),
					Option(L("No problem"), "accept"),
					Option(L("I'll think about it for a while"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq06);

				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("It's hard to watch the Egnomes running around the Atgaila Chapel."));
				var answer = await dialog.SelectQuestOffer(Mq07, L("Please clean up the Egnomes."),
					Option(L("Sure, I'll defeat it"), "accept"),
					Option(L("I don't have time for that"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq07);

				return;
			}

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("All preparations have been made."));
				var answer = await dialog.SelectQuestOffer(Mq09, L("The barriers of the church will soon weaken Gesti."),
					Option(L("I trust you"), "accept"),
					Option(L("About the Divine Sphere"), "explain"),
					Option(L("I'm not yet ready"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("This Divine Sphere is a holy weapon that will be used to fight against Gesti in Nefritas Cliff."));
					await dialog.Msg(L("It has been handed down since the times of the first Paladin."));
					return;
				}

				if (answer == "accept")
					character.Quests.Start(Mq09);

				return;
			}

			if (!character.Quests.Has(Mq10) && character.Quests.MeetsPrerequisites(Mq10))
			{
				await dialog.Msg(L("So you found the Seal of Space."));
				var answer = await dialog.SelectQuestOffer(Mq10, L("Honestly, I did not believe it when my friend, the Paladin Master, said a Savior would come."),
					Option(L("I'll go there"), "accept"),
					Option(L("There is still more to do"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("When you find the revelation, tell the Paladin Master of the story you've been through so far."));
					await dialog.Msg(L("He must be the one most anxious about it."));
					character.Quests.Start(Mq10);
				}
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The Seal of Space is at the Sventove Central Altar. Move quietly."));
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Insert the altar fragments into the eight pillars of the Sventove Central Hall."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Activate the Malda Altar and lure the demons to it."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("Charge the Auka Altar so the demons chase it instead of us."));
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Clean the Egnomes out of the Atgaila Chapel."));
				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("Gesti is trapped. Hold her while the Divine Sphere charges."));
				character.Quests.ReplayQuestTrack(Mq09);
				return;
			}

			if (character.Quests.IsActive(Mq10))
			{
				await dialog.Msg(L("The sanctuary is behind the seal. Find the revelation."));
				character.Quests.ClearQuestTrack(Mq10);
				return;
			}

			await dialog.Msg(L("The Divine Sphere is ready. We end this at Nefritas Cliff."));
		});

		// Malda Altar
		//-------------------------------------------------------------------------
		AddNpc(147357, L("Malda Altar"), "CHAPLE577_HOLY_2", "d_chapel_57_7", 1516, -104, 45, async dialog =>
		{
			await dialog.Msg(L("The Malda Altar stands quiet, waiting to be woken."));
		});

		// Auka Altar
		//-------------------------------------------------------------------------
		AddNpc(147357, L("Auka Altar"), "CHAPLE577_HOLY_3", "d_chapel_57_7", -942, -106, 45, async dialog =>
		{
			await dialog.Msg(L("The Auka Altar has lost its old power."));
		});

		// Sventove Central Altar
		//-------------------------------------------------------------------------
		AddNpc(147358, L("Sventove Central Altar"), "CHAPLE577_HOLY_1", "d_chapel_57_7", -27, -137, 45, async dialog =>
		{
			await dialog.Msg(L("The Sventove Central Altar pulses with a power that is not its own."));
		});

		// Altar Fragment
		//-------------------------------------------------------------------------
		AddNpc(147372, L("Altar Fragment"), "CHAPLE577_MQ_03", "d_chapel_57_7", -145, 122, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Altar Fragment"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You gather the fragments of the destroyed altar and fit them into the pillar. It answers with a low chime."));
				character.Quests.CompleteObjective(Mq04, "insertPillars");
				return;
			}

			await dialog.Msg(L("A jagged fragment of the destroyed altar."));
		});

		// Central Pillars
		//-------------------------------------------------------------------------
		AddNpc(147457, L("Central Pillar"), "CHAPLE577_MQ_04_1", "d_chapel_57_7", -229, -309, 90, async dialog => await InsertPillar(dialog));
		AddNpc(147457, L("Central Pillar"), "CHAPLE577_MQ_04_2", "d_chapel_57_7", -34, -298, 90, async dialog => await InsertPillar(dialog));
		AddNpc(147457, L("Central Pillar"), "CHAPLE577_MQ_04_3", "d_chapel_57_7", 168, -298, 90, async dialog => await InsertPillar(dialog));
		AddNpc(147457, L("Central Pillar"), "CHAPLE577_MQ_04_4", "d_chapel_57_7", 165, -133, 90, async dialog => await InsertPillar(dialog));
		AddNpc(147457, L("Central Pillar"), "CHAPLE577_MQ_04_5", "d_chapel_57_7", 162, 41, 90, async dialog => await InsertPillar(dialog));
		AddNpc(147457, L("Central Pillar"), "CHAPLE577_MQ_04_6", "d_chapel_57_7", -34, 39, 90, async dialog => await InsertPillar(dialog));
		AddNpc(147457, L("Central Pillar"), "CHAPLE577_MQ_04_7", "d_chapel_57_7", -237, 37, 90, async dialog => await InsertPillar(dialog));
		AddNpc(147457, L("Central Pillar"), "CHAPLE577_MQ_04_8", "d_chapel_57_7", -237, -134, 90, async dialog => await InsertPillar(dialog));

		// Sanctuary Mural
		//-------------------------------------------------------------------------
		AddNpc(147372, L("Sanctuary Mural"), "CHAPLE577_MQ_10", "d_chapel_57_7", 801, -1250, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sanctuary Mural"));

			if (character.Quests.IsActive(Mq10) && !character.Quests.IsCompletable(Mq10))
			{
				await dialog.Msg(L("The Seal of Space turns in your hand. The wall folds away, revealing the sanctuary."));
				var readMural = await character.TimeActions.StartAsync(L("Reading the mural..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

				if (readMural != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq10);
				return;
			}

			await dialog.Msg(L("A mural older than the church, hiding a door that is not a door."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("CHAPLE577_MQ_06_TRIGGER", "d_chapel_57_7", -942, -106, 400, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
				character.Quests.CompleteObjective(Mq06, "chargeCrystal");

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Fits an altar fragment into one of the Sventove pillars.
	/// </summary>
	private static async Task InsertPillar(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Central Pillar"));

		if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
		{
			await dialog.Msg(L("You wedge the altar fragment into the pillar's socket. It hums, and the trap tightens another notch."));
			character.Quests.CompleteObjective(Mq04, "insertPillars");
			return;
		}

		await dialog.Msg(L("A pillar of the Sventove Central Hall, cut with old sigils."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8528: Gesti's Plan
//-----------------------------------------------------------------------------
public class Chaple577Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8528);
		SetName(L("Gesti's Plan"));
		SetDescription(L("Watch Gesti from the cathedral door and learn what she is after."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_01", "d_chapel_57_7", L("Talk to Follower Algis"), L("Follower Algis was waiting for you on the 2nd Floor."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_ARUNE_01", "d_chapel_57_7", L("Go to the Bell Tower"), L("Go with Algis and watch Gesti."));
		SetPhase(QuestStatus.Success, "CHAPLE577_ARUNE_01", "d_chapel_57_7", L("Talk to Follower Algis"), L("Ask Follower Algis about what to do next."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE577_MQ_01_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(8730, QuestStatus.Completed));

		AddObjective("observeGesti", L("Go to the Bell Tower"), new ManualObjective());
	}
}

// 8529: Recapture the Bell Tower
//-----------------------------------------------------------------------------
public class Chaple577Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8529);
		SetName(L("Recapture the Bell Tower"));
		SetDescription(L("Occupy the Bell Tower, a good observation point over Gesti."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_01", "d_chapel_57_7", L("Talk to Follower Algis"), L("Gesti disappeared to the Central Hall. Talk to Follower Algis."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_ARUNE_01", "d_chapel_57_7", L("Recapture the Bell Tower"), L("Occupy the Bell Tower."));
		SetPhase(QuestStatus.Success, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Talk to Follower Algis."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE577_MQ_02_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8528, QuestStatus.Completed));

		AddObjective("killNecroventer", L("Defeat Necroventer"), new KillObjective(1, "boss_necrovanter") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 8530: Stolen Seal of Space
//-----------------------------------------------------------------------------
public class Chaple577Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8530);
		SetName(L("Stolen Seal of Space"));
		SetDescription(L("Gesti destroys the Sventove Central Altar and takes the Seal of Space."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Captured the Bell Tower. Ask Follower Algis what to do next."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_HOLY_1", "d_chapel_57_7", L("Stolen Seal of Space"), L("Reach the Sventove Central Altar."));
		SetPhase(QuestStatus.Success, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Talk to Follower Algis."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE577_MQ_03_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8529, QuestStatus.Completed));

		AddObjective("returnSeal", L("Stolen Seal of Space"), new ManualObjective());
	}
}

// 8531: Cat and Mouse
//-----------------------------------------------------------------------------
public class Chaple577Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8531);
		SetName(L("Cat and Mouse"));
		SetDescription(L("Insert the altar fragments into the eight pillars of Sventove Central Hall."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Work with Follower Algis on a way to trap Gesti."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_MQ_03", "d_chapel_57_7", L("Make a trap at Sventove Central Hall"), L("Gather the altar pieces and insert them into the 8 pillars."));
		SetPhase(QuestStatus.Success, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Return to Follower Algis."));

		AddPrerequisite(new QuestStatusPrerequisite(8530, QuestStatus.Completed));

		AddObjective("insertPillars", L("Make a trap at Sventove Central Hall"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8532: Activate the Malda Altar
//-----------------------------------------------------------------------------
public class Chaple577Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8532);
		SetName(L("Activate the Malda Altar"));
		SetDescription(L("Activate the Malda Altar to lure the demons to their demise."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Follower Algis is waiting for the Revelator in the Tenet Church 2F."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_HOLY_2", "d_chapel_57_7", L("Activate the Malda Altar"), L("Activate the Malda Altar and clear the demons it draws."));
		SetPhase(QuestStatus.Success, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Return to Follower Algis."));

		AddPrerequisite(new QuestStatusPrerequisite(8530, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat demons"), new KillObjective(10, "Egnome", "Spector_Gh", "colitile", "Infroholder_bow"));

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8533: Activate the Auka Altar
//-----------------------------------------------------------------------------
public class Chaple577Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8533);
		SetName(L("Activate the Auka Altar"));
		SetDescription(L("Charge the Auka Altar and shift the demons' attention to it."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Follower Algis is waiting for the Revelator in the Tenet Church 2F."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_HOLY_3", "d_chapel_57_7", L("Charge the low level spirit crystal"), L("Activate the Auka Altar."));
		SetPhase(QuestStatus.Success, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Return to Follower Algis."));

		AddPrerequisite(new QuestStatusPrerequisite(8530, QuestStatus.Completed));

		AddObjective("chargeCrystal", L("Charge the low level spirit crystal"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8534: Cleaning the Church
//-----------------------------------------------------------------------------
public class Chaple577Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8534);
		SetName(L("Cleaning the Church"));
		SetDescription(L("Defeat the Egnomes wandering inside the church."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Follower Algis is waiting for the Revelator in the Tenet Church 2F."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Defeat Egnome"), L("Defeat the Egnomes wandering inside the church."));
		SetPhase(QuestStatus.Success, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Return to Follower Algis."));

		AddPrerequisite(new QuestStatusPrerequisite(8530, QuestStatus.Completed));

		AddObjective("killEgnome", L("Defeat Egnome"), new KillObjective(2, "Egnome"));

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8536: Trapped Gesti
//-----------------------------------------------------------------------------
public class Chaple577Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8536);
		SetName(L("Trapped Gesti"));
		SetDescription(L("Gesti is weakened by the central altar. Hold her while the Divine Sphere charges."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Proceed to the next plan with Follower Algis."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Fight with Gesti"), L("Disable Gesti while Follower Algis prepares the Divine Sphere."));
		SetPhase(QuestStatus.Success, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Talk to Follower Algis."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE577_MQ_09_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8531, QuestStatus.Completed));

		AddObjective("fightGesti", L("Fight with Gesti"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 4));
		AddReward(new ItemReward("KEY_OF_LEGEND_01", 1));
	}
}

// 8537: The Hidden Sanctum's Revelation (1)
//-----------------------------------------------------------------------------
public class Chaple577Mq10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8537);
		SetName(L("The Hidden Sanctum's Revelation (1)"));
		SetDescription(L("Use the Seal of Space on the pillar and take the revelation from the sanctuary."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_7");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPLE577_ARUNE_02", "d_chapel_57_7", L("Talk to Follower Algis"), L("Talk to Follower Algis again."));
		SetPhase(QuestStatus.InProgress, "CHAPLE577_MQ_10", "d_chapel_57_7", L("Find the revelation in the hidden sanctuary"), L("Use the Seal of Space on the pillar and enter the sanctuary."));
		SetPhase(QuestStatus.Success, "CHAPLE577_MQ_10", "d_chapel_57_7", L("Find the revelation in the hidden sanctuary"), L("Find the revelation in the hidden sanctuary."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE577_MQ_10_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(8536, QuestStatus.Completed));

		AddObjective("findRevelation", L("Find the revelation in the hidden sanctuary"), new ManualObjective());

		AddReward(new ItemReward("stonetablet02", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The revelation is the quest; there is no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}
