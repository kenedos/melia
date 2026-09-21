//--- Melia Script ----------------------------------------------------------
// Demon Prison District 3 Quest NPCs
//--- Description -----------------------------------------------------------
// Kupole Daiva working her way along the maze after Hauberk, and Sigita
// waiting at the only way out of it.
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

public class DVelniasprison513QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(60018);
	private readonly static QuestId Mq02 = new QuestId(60019);
	private readonly static QuestId Mq03 = new QuestId(60020);
	private readonly static QuestId Mq04 = new QuestId(60021);
	private readonly static QuestId Mq05 = new QuestId(60022);
	private readonly static QuestId Sq01 = new QuestId(60036);
	private readonly static QuestId Sq02 = new QuestId(60037);
	private readonly static QuestId Sq03 = new QuestId(60038);

	protected override void Load()
	{
		// Kupole Daiva, at the district gate
		//-------------------------------------------------------------------------
		AddConditionalNpc(154013, L("Kupole Daiva"), "VPRISON513_MQ_DAIVA_01", "d_velniasprison_51_3", -3158.28, -653.90, 90, this.IsDaivaAtTheGate, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Daiva"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Anything Hauberk tries won't be effective."));
				await dialog.Msg(L("Let's keep pressuring him, and chase him to the very end."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("So you've come, Savior."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("Fortunately, Hauberk seems to be not focused on leaving this place through the dimensional crack. That means he is not complete yet enough to go out through the dimensional crack."),
					Option(L("I will chase after Hauberk"), "accept"),
					Option(L("I will prepare little more"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					character.LookAround();
					await dialog.Msg(L("Now, Hauberk is risking everything he's got."));
					await dialog.Msg(L("Now is the only chance that he can restore everything about him."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("It seems that Hauberk's servants are coming again. I think he's trying to deceive us by giving the fragments of his soul to his servants, but that's also what we want."),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("It doesn't sound like a good idea"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("I will chase after Hauberk while I am collecting the fragments of his soul."));
					await dialog.Msg(L("Let's meet in front of Idinga Solitary Confinement."));
					character.LookAround();
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Hauberk is at the Nevirau Collapsed Area, and his servants are between you and him."));
				character.Quests.ReplayQuestTrack(Mq01);
				return;
			}

			await dialog.Msg(L("A Kupole holding the gate of a maze she has walked more times than she can count."));
		});

		// Kupole Daiva, at Idinga Solitary Confinement
		//-------------------------------------------------------------------------
		AddConditionalNpc(154013, L("Kupole Daiva"), "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", -1382.82, 940.34, 90, this.IsDaivaAtIdinga, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Daiva"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Well done."));
				await dialog.Msg(L("They might have succeeded in slowing us down, but they did not buy themselves much time at all."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Fortunately, he did not come to my side and he moved as I thought."));
				await dialog.Msg(L("With the Savior's power, we can surely catch him."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("I will stay here and pursue the remaining demons."));
				await dialog.CompleteQuest(Sq01);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Now he's trying to escape using the body of his servant. Let's look for him by getting rid of the demons that contain Hauberk. Until he gives up."),
					Option(L("I will chase after Hauberk"), "accept"),
					Option(L("I'm not sure if I can take down all those demons"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					character.Inventory.Add(ItemId.VPRISON513_MQ_03_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("With this Night Star Spectral Orb, we can easily find Hauberk."));
					await dialog.Msg(L("If Hauberk gives up and tries to run away, I will do my best to block it."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("There is only one way out from here. It will be our great chance to catch Hauberk."),
					Option(L("I will get him for sure"), "accept"),
					Option(L("Ask her to wait a bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("We got Hauberk but now his servants are the problem. We don't know what they will do."),
					Option(L("I will defeat it"), "accept"),
					Option(L("I will take care of it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("The dimensional crack is the problem."));
					await dialog.Msg(L("There weren't this many low-class demons before."));
					return;
				}
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Even if Hauberk does something, it can only be inside the prison."));
				await dialog.Msg(L("Hauberk could never run away."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("If Hauberk gives up and tries to run away, I will do my best to stop him."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Drive him along the Zinuma Passage. Sigita is waiting at the far end of it."));
				character.Quests.ReplayQuestTrack(Mq04);
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("The dimensional crack is the problem."));
				await dialog.Msg(L("There weren't this many low-class demons before."));
				return;
			}

			await dialog.Msg(L("Idinga Solitary Confinement, and a Kupole standing in its doorway with an orb in her hand."));
		});

		// Kupole Daiva, at Galutin Solitary Confinement
		//-------------------------------------------------------------------------
		AddConditionalNpc(154013, L("Kupole Daiva"), "VPRISON513_MQ_DAIVA_03", "d_velniasprison_51_3", 1154.90, 969.07, 90, this.IsDaivaAtGalutin, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Daiva"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Good work."));
				await dialog.Msg(L("I will track down every last demon so that they can't resist against the goddess."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("Please keep this a secret from Vakarine."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("Some of Hauberk's servants ran away to Ishidevi Hideout. They are probably hoping for a revival, like Nuaele's."),
					Option(L("I will defeat it"), "accept"),
					Option(L("Tell her that there is a more emergent issue"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("Leaving Nuaele alone just because it was trapped was a source of trouble."));
					await dialog.Msg(L("We paid a big price for that lesson."));
					return;
				}
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("I need proof to show to Vakarine we've been fighting here. Can you collect demon teeth in Huradeti Crossroads?"),
					Option(L("I'll bring it"), "accept"),
					Option(L("I might be scolded by the goddess"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("The demons need to learn their lessons."));
					await dialog.Msg(L("It's rather gentle compared to what they did."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Leaving Nuaele alone just because it was trapped was a source of trouble."));
				await dialog.Msg(L("We paid a big price for that lesson."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("The demons need to learn their lessons."));
				await dialog.Msg(L("It's rather gentle compared to what they did."));
				return;
			}

			await dialog.Msg(L("A Kupole who has followed the chase all the way to its last door."));
		});

		// Kupole Sigita
		//-------------------------------------------------------------------------
		AddNpc(154012, L("Kupole Sigita"), "VPRISON513_MQ_SIGITA", "d_velniasprison_51_3", 1307.43, 916.96, 269, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Sigita"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("It's a success!"));
				await dialog.Msg(L("If Hauberk has any thought of doing so, he wouldn't have wasted any time in harming me."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Well done."));
				await dialog.Msg(L("We will collect Hauberk's sealed strong power now and take it to Vakarine."));
				await dialog.CompleteQuest(Mq05);
				character.ServerMessage(L("Vakarine has left the Corridor of Monitor. She will be waiting in District 5."));
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("Well done. Hauberk didn't know what to do when he saw me and ran away to the Galutin Solitary Confinement."),
					Option(L("I am ready"), "accept"),
					Option(L("I will prepare a lot"), "leave")
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
				await dialog.Msg(L("He is still in there, and he will not go quietly."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("A Kupole standing in the only doorway out of the maze, and quite content to."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// Idinga Solitary Confinement, where the Night Star Spectral Orb picks
		// Hauberk out of his servants.
		AddQuestTrigger("VPRISON513_MQ_03_AREA", "d_velniasprison_51_3", -1520, 333, 550, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				character.Quests.CompleteObjective(Mq03, "chaseHauberk");
				character.ServerMessage(L("The orb burns out. Hauberk gave up the body he was wearing and ran as a soul."));
			}

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether Daiva is still at the district gate.
	/// </summary>
	/// <param name="character"></param>
	private bool IsDaivaAtTheGate(Character character)
		=> !character.Quests.Has(Mq02);

	/// <summary>
	/// Returns whether Daiva has moved up to Idinga Solitary Confinement.
	/// </summary>
	/// <param name="character"></param>
	private bool IsDaivaAtIdinga(Character character)
		=> character.Quests.Has(Mq02) && !character.Quests.HasCompleted(Sq01);

	/// <summary>
	/// Returns whether Daiva has followed the chase to Galutin Solitary
	/// Confinement.
	/// </summary>
	/// <param name="character"></param>
	private bool IsDaivaAtGalutin(Character character)
		=> character.Quests.HasCompleted(Sq01);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60018: Hauberk in the Maze (1)
//-----------------------------------------------------------------------------
public class Vprison513Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60018);
		SetName(L("Hauberk in the Maze (1)"));
		SetDescription(L("Hauberk is looking for the thinnest part of the prison wall, and his servants are in the way."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON513_MQ_DAIVA_01", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("You need to find Hauberk who ran away with the Chain of Reversion. Go to Kupole Daiva in District 3 of Demon Prison."));
		SetPhase(QuestStatus.InProgress, "VPRISON513_MQ_DAIVA_01", "d_velniasprison_51_3", L("Pursue Demon Lord Hauberk"), L("Daiva says Hauberk must be after the weakened space of the prison. Defeat Hauberk who is looking for the space of the weakened prison at Nevirau Collapsed Area."));
		SetPhase(QuestStatus.Success, "VPRISON513_MQ_DAIVA_01", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("You lost Hauberk. Talk to Kupole Daiva."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON513_MQ_01_TRACK", partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60017, QuestStatus.Completed));

		AddObjective("killServants", L("Defeat the servants of Hauberk"), new KillObjective(8, "Hohen_ritter", "Hohen_orben") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 60019: Hauberk in the Maze (2)
//-----------------------------------------------------------------------------
public class Vprison513Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60019);
		SetName(L("Hauberk in the Maze (2)"));
		SetDescription(L("Hauberk has given pieces of his own soul to his servants, which is exactly what Daiva wanted."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON513_MQ_DAIVA_01", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("You lost Hauberk. Talk to Kupole Daiva."));
		SetPhase(QuestStatus.InProgress, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Collect Hauberk's Soul Fragments"), L("Kupole Daiva said to collect the Soul Fragments of Hauberk to defeat the servants of Hauberk throughout District 3."));
		SetPhase(QuestStatus.Success, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Give them to Kupole Daiva"), L("Collected all of Hauberk's Soul Fragments. Talk to Kupole Daiva in Idinga Solitary Confinement."));

		AddPrerequisite(new QuestStatusPrerequisite(60018, QuestStatus.Completed));

		AddObjective("collectFragments", L("Retrieve the pieces of the spirit of Hauberk"), new CollectItemObjective("VPRISON513_MQ_02_ITEM", 10));

		AddPityDrop("VPRISON513_MQ_02_ITEM", 0.4f, 3, 1, "Hohen_ritter", "hohen_barkle", "Hohen_orben", "Hohen_mane");

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new TakeItemReward("VPRISON513_MQ_02_ITEM"));
	}
}

// 60020: Hauberk in the Maze (3)
//-----------------------------------------------------------------------------
public class Vprison513Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60020);
		SetName(L("Hauberk in the Maze (3)"));
		SetDescription(L("The Night Star Spectral Orb shows which of the demons at Idinga is carrying Hauberk."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("Kupole Daiva seems to have found where Hauberk could be hiding. Talk to Kupole Daiva in Idinga Solitary Confinement."));
		SetPhase(QuestStatus.InProgress, "VPRISON513_MQ_03_AREA", "d_velniasprison_51_3", L("Pursue Hauberk"), L("Use the Night Star Spectral Orb at Idinga Solitary Confinement to find demons containing Hauberk and defeat them."));
		SetPhase(QuestStatus.Success, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("Hauberk gave up possession and ran away as a soul. Talk to Kupole Daiva in Idinga Solitary Confinement."));

		AddPrerequisite(new QuestStatusPrerequisite(60019, QuestStatus.Completed));

		AddObjective("chaseHauberk", L("Pursue Hauberk"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new TakeItemReward("VPRISON513_MQ_03_ITEM"));
	}
}

// 60021: Hauberk in the Maze (4)
//-----------------------------------------------------------------------------
public class Vprison513Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60021);
		SetName(L("Hauberk in the Maze (4)"));
		SetDescription(L("Hauberk is driven up the Zinuma Passage and into Galutin Solitary Confinement, where Sigita is standing."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("Daiva says everything is going as planned. Ask Daiva what you should do next."));
		SetPhase(QuestStatus.InProgress, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Chase after Hauberk"), L("Push Hauberk who is running away through the Zinuma Passage in to the Galutin Solitary Confinement!"));
		SetPhase(QuestStatus.Success, "VPRISON513_MQ_SIGITA", "d_velniasprison_51_3", L("Talk to Kupole Sigita"), L("Kupole Sigita drove Hauberk to Galutin Solitary Confinement. Talk to Sigita."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON513_MQ_04_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60020, QuestStatus.Completed));

		AddObjective("driveHauberk", L("Chase after Hauberk"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 60022: Hauberk in the Maze (5)
//-----------------------------------------------------------------------------
public class Vprison513Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60022);
		SetName(L("Hauberk in the Maze (5)"));
		SetDescription(L("Hauberk is held down inside Galutin Solitary Confinement and sealed there with all four Kupoles on him."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON513_MQ_SIGITA", "d_velniasprison_51_3", L("Talk to Kupole Sigita"), L("Talk to Kupole Sigita at the entrance of Galutin Solitary Confinement at District 3 of the Demon's Prison."));
		SetPhase(QuestStatus.InProgress, "VPRISON513_MQ_SIGITA", "d_velniasprison_51_3", L("Suppress Demon Lord Hauberk"), L("Suppress Hauberk who is resisting inside the Galutin Solitary Confinement with Kupole Sigita and seal him."));
		SetPhase(QuestStatus.Success, "VPRISON513_MQ_SIGITA", "d_velniasprison_51_3", L("Talk to Kupole Sigita"), L("Talk to Kupole Sigita at the entrance of Galutin Solitary Confinement at District 3 of the Demon's Prison."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON513_MQ_05_TRACK", 100, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60021, QuestStatus.Completed));

		AddObjective("sealHauberk", L("Suppress Demon Lord Hauberk"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 3));
	}
}

// 60036: Eyes of the Goddess
//-----------------------------------------------------------------------------
public class Vprison513Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60036);
		SetName(L("Eyes of the Goddess"));
		SetDescription(L("Hauberk is sealed and his servants have fallen back to the Aklaga Isolation Area."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("Successfully caught Hauberk but his demons are still left. Talk to Kupole Daiva."));
		SetPhase(QuestStatus.InProgress, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Defeat Hauberk's remnant forces"), L("Kupole Daiva asked you to defeat Hauberk's demons that fled to Aklaga Isolation Area."));
		SetPhase(QuestStatus.Success, "VPRISON513_MQ_DAIVA_02", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("Defeated all Hauberk's remnant forces. Tell Kupole Daiva about it."));

		AddPrerequisite(new LevelPrerequisite(150));
		AddPrerequisite(new QuestStatusPrerequisite(60022, QuestStatus.Completed));

		AddObjective("killRemnants", L("Defeat Hauberk's remnant forces"), new KillObjective(10, "Hohen_ritter", "hohen_barkle", "Hohen_orben", "Hohen_mane"));

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 60037: The Hidden Betrayer
//-----------------------------------------------------------------------------
public class Vprison513Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60037);
		SetName(L("The Hidden Betrayer"));
		SetDescription(L("The servants that sold Hauberk out are waiting at the Ishidevi Hideout for a revival of their own."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON513_MQ_DAIVA_03", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("The behavior of Hauberk's remnants is somewhat strange. Talk to Kupole Daiva."));
		SetPhase(QuestStatus.InProgress, "VPRISON513_MQ_DAIVA_03", "d_velniasprison_51_3", L("Defeat Hauberk's remnant forces"), L("Kupole Daiva asked you to defeat the demons that ran away after betraying Hauberk."));
		SetPhase(QuestStatus.Success, "VPRISON513_MQ_DAIVA_03", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("You've defeated Hauberk's remnants at Ishidevi Hideout. Talk to Kupole Daiva."));

		AddPrerequisite(new LevelPrerequisite(150));
		AddPrerequisite(new QuestStatusPrerequisite(60036, QuestStatus.Completed));

		AddObjective("killBetrayers", L("Defeat Hauberk's remnant forces"), new KillObjective(10, "Hohen_ritter", "hohen_barkle", "Hohen_orben", "Hohen_mane"));

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 60038: Demons at the Crossroads
//-----------------------------------------------------------------------------
public class Vprison513Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60038);
		SetName(L("Demons at the Crossroads"));
		SetDescription(L("Daiva wants the teeth of the Huradeti Crossroads demons as proof the district was held."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON513_MQ_DAIVA_03", "d_velniasprison_51_3", L("Talk to Kupole Daiva"), L("It seems that Kupole Daiva made an important decision. Talk to Kupole Daiva."));
		SetPhase(QuestStatus.InProgress, "VPRISON513_MQ_DAIVA_03", "d_velniasprison_51_3", L("Collect Demon's Teeth"), L("Kupole Daiva told you that now is the time to show your real strength. Defeat the demons that hid at Huradeti Crossroads and collect the teeth of the demons that are their symbol."));
		SetPhase(QuestStatus.Success, "VPRISON513_MQ_DAIVA_03", "d_velniasprison_51_3", L("Give them to Kupole Daiva"), L("You've collected Demon's Teeth. Take them to Kupole Daiva."));

		AddPrerequisite(new LevelPrerequisite(150));
		AddPrerequisite(new QuestStatusPrerequisite(60036, QuestStatus.Completed));

		AddObjective("collectTeeth", L("Obtain the teeth of the demons"), new CollectItemObjective("VPRISON513_SQ_03_ITEM", 8));

		AddPityDrop("VPRISON513_SQ_03_ITEM", 1.0f, 0, 1, "Hohen_ritter", "hohen_barkle", "Hohen_orben", "Hohen_mane");

		AddReward(new ItemReward("expCard9", 1));
		AddReward(new TakeItemReward("VPRISON513_SQ_03_ITEM"));
	}
}
