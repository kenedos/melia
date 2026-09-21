//--- Melia Script ----------------------------------------------------------
// Demon Prison District 2 Quest NPCs
//--- Description -----------------------------------------------------------
// Kupole Arune on the Shirtis Monitor Pass, and Aldona who opens the way into
// Nuaele's own territory.
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

public class DVelniasprison512QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(60007);
	private readonly static QuestId Mq02 = new QuestId(60008);
	private readonly static QuestId Mq03 = new QuestId(60009);
	private readonly static QuestId Mq04 = new QuestId(60010);
	private readonly static QuestId Mq05 = new QuestId(60011);
	private readonly static QuestId Sq01 = new QuestId(60031);
	private readonly static QuestId Sq02 = new QuestId(60032);

	protected override void Load()
	{
		// Kupole Arune
		//-------------------------------------------------------------------------
		AddNpc(154016, L("Kupole Arune"), "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", -93.15, 553.98, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Arune"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("With her forces diminished, Nuaele's power should be weakened."));
				await dialog.Msg(L("Before anything happens to Aldona, I will tell you what to do next."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("I am sure the Demon Lord Nuaele is aware of this now."));
				await dialog.Msg(L("That the power of the others is not hers to keep."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("It seems she's planned to make this prison her world from the beginning."));
				await dialog.Msg(L("If her plans succeed, even if Goddess Vakarine powers were restored, it would not be enough."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("The Harugals used to be Hauberk's elite servants."));
				await dialog.Msg(L("But it was also them who turned over Hauberk's seal fragment to Nuaele."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Goddess Vakarine is waiting for you at the District 3."));
				await dialog.Msg(L("But, I feel somewhat uneasy since she is so stubborn."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Many demons came here through the dimensional crack to support the Demon Lord here."));
				await dialog.Msg(L("It will be the same in other places as well. Thanks to you we now have peace."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Thank you. I will destroy these fragments myself."));
				await dialog.Msg(L("May the blessings of the goddess be with you."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				var answer = await dialog.SelectQuestOffer(Mq01, L("So you are the Revelator Vakarine talked about. It is great that you aren't late."),
					Option(L("Tell me how I can help Aldona"), "accept"),
					Option(L("I need some time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("Please stop the demons that are coming here through the dimensional crack."));
					await dialog.Msg(L("If you could stop them before they join Nuaele, it will be great help to the safety of Aldona."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("Nuaele is difficult to face head-on with only us. But Nuaele wasn't always that strong."),
					Option(L("What should I do?"), "accept"),
					Option(L("That sounds dangerous"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("Even now, Hauberk's servants think Nuaele is Hauberk."));
					await dialog.Msg(L("But if Hauberk gets back his marks, then they will realize who the real master is."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Hmm... there's something that's been bugging me. When Nuaele collected power, she could've tried escaping immediately."),
					Option(L("I'll help you"), "accept"),
					Option(L("Everything will be alright"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("Please interrogate the servants of Nuaele."));
					await dialog.Msg(L("Since Hauberk is also a demon, it will be easy to read their minds. It becomes even easier when they are in their spirit form."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("Since we've found out Nuaele's intention, we should be moving fast."),
					Option(L("I'll take care of it"), "accept"),
					Option(L("About Hauberk and Nuaele's relationship"), "explain"),
					Option(L("I need more preparation"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Hauberk was once the strongest Demon Lord who led the largest army."));
					await dialog.Msg(L("After he was sealed, I thought no one as strong as him would appear again."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("The Harugals of the Fourth Isolation Area are the last of her loyal servants. Put them down."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("Nuaele is currently performing a demon summoning ritual in her territory. But both of her arms and legs are cut off, so it will be easy to defeat her."),
					Option(L("I am ready"), "accept"),
					Option(L("I need to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.LookAround();
					await dialog.Msg(L("Without Aldona's help, you will never enter the area of where Nuaele resides."));
					await dialog.Msg(L("You should seek for guidance from Aldona."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("Nuaele may be gone but her remnants are gathering in the 1st Isolation Area. They intend to continue her will. Please get rid of the demons to dissuade them."),
					Option(L("I will come back after defeating it"), "accept"),
					Option(L("I will do it next time"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("You have to be more careful."));
					return;
				}
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("Nuaele is gone now, but there are still demons that continue her orders. They will repeat the past unless we retrieve her fragments."),
					Option(L("I will retrieve the fragments"), "accept"),
					Option(L("I will meet the Vakarine first"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("They are the demons of the Fourth Isolation Area."));
					await dialog.Msg(L("Let this be a lesson for them, so that it won't happen again."));
					return;
				}
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Goddess. I hope nothing has happened to Aldona..."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Even now, Hauberk's servants think Nuaele is Hauberk."));
				await dialog.Msg(L("But if Hauberk gets back his marks, then they will realize who the real master is."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("When the power of the goddesses get weak, the mighty power of us, Kupoles also get weak."));
				await dialog.Msg(L("Furthermore, trying to disturb the minds of the demons, it's impossible."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("You now see the end of someone who tried to use another's power instead of using his own."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Without Aldona's help, you will never enter the area of where Nuaele resides."));
				await dialog.Msg(L("You should seek for guidance from Aldona."));
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("You have to be more careful."));
				await dialog.Msg(L("Hauberk and Nuaele became symbolic figures to the demons."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("It will take a long time to re-establish order in the prison."));
				await dialog.Msg(L("It will take a long time, just like Nuaele did preparing for now.."));
				return;
			}

			await dialog.Msg(L("The Shirtis Monitor Pass holds, but only because what is on the other side has not pushed yet."));
		});

		// Kupole Aldona
		//-------------------------------------------------------------------------
		AddConditionalNpc(154014, L("Kupole Aldona"), "VPRISON512_MQ_ALDONA", "d_velniasprison_51_2", 989.95, -57.54, 90, this.IsAldonaWaiting, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Aldona"));

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
			{
				var spoke = await character.TimeActions.StartAsync(L("Talking..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(1.5));

				if (spoke != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("A Kupole who has held the gate of Nuaele's territory alone for longer than she should have had to."));
		});

		// Tesla's Dedication
		//-------------------------------------------------------------------------
		AddNpc(147464, L("Tesla's Dedication"), "VPRISON512_STONE_01", "d_velniasprison_51_2", 1046.44, 1676.10, 90, async dialog =>
		{
			dialog.SetTitle(L("Tesla's Dedication"));

			await dialog.Msg(L("A slab set into the floor beside the statue, worn smooth where hands have rested on it."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The 3rd Isolation Area, where Hauberk reads the demons that are left.
		AddQuestTrigger("VPRISON512_MQ_03_AREA", "d_velniasprison_51_2", -1477, -44, 450, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				character.Quests.CompleteObjective(Mq03, "interrogate");
				character.ServerMessage(L("Hauberk's seal reads the demons of the Third Isolation Area. Take what they gave up to Arune."));
			}

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether Aldona is at the gate of Nuaele's territory.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAldonaWaiting(Character character)
		=> character.Quests.IsActive(Mq05);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60007: Ridding the Traitor (1)
//-----------------------------------------------------------------------------
public class Vprison512Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60007);
		SetName(L("Ridding the Traitor (1)"));
		SetDescription(L("Nuaele's reinforcements are still coming through the dimensional crack into the First Isolation Area."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("The situation of Kupole placed on Shirtis Monitor Pass does not look good. Talk to Kupole Arune."));
		SetPhase(QuestStatus.InProgress, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Defeat the demons that arrived from the dimensional crack"), L("Kupole Arune says you need to stem the power of Demon Lord Nuaele to stabilize the goddess' barrier. Defeat Nuaele's subordinates."));
		SetPhase(QuestStatus.Success, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Nuaele's subordinates are defeated. Go back and report to Kupole Arune."));

		AddPrerequisite(new QuestStatusPrerequisite(60006, QuestStatus.Completed));

		AddObjective("killArrivals", L("Defeat the demons of the 1st Isolation Area"), new KillObjective(10, "defender_spider", "Nuka", "Elet"));

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 60008: Ridding the Traitor (2)
//-----------------------------------------------------------------------------
public class Vprison512Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60008);
		SetName(L("Ridding the Traitor (2)"));
		SetDescription(L("Nuaele's demons still wear the marks Hauberk gave them, and taking them back tells them who their master is."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Kupole Arune has the better idea. Talk to Kupole Arune."));
		SetPhase(QuestStatus.InProgress, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Obtain Hauberk's Marks"), L("Some of Nuaele's demons used to be Hauberk's demons who've received empowered Marks from Hauberk before. Defeat Nuaele's demons in the 2nd Isolation Area and get Hauberk's Marks back."));
		SetPhase(QuestStatus.Success, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Regained all symbols of Hauberk. Take them to Kupole Arune."));

		AddPrerequisite(new QuestStatusPrerequisite(60007, QuestStatus.Completed));

		AddObjective("collectMarks", L("Collect Hauberk's Mark from the demons"), new CollectItemObjective("VPRISON512_MQ_02_ITEM", 7));

		AddPityDrop("VPRISON512_MQ_02_ITEM", 1.0f, 0, 1, "defender_spider", "Nuka", "Elet");

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("VPRISON512_MQ_02_ITEM"));
	}
}

// 60009: Ridding the Traitor (3)
//-----------------------------------------------------------------------------
public class Vprison512Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60009);
		SetName(L("Ridding the Traitor (3)"));
		SetDescription(L("Hauberk's seal reads the minds of the demons in the Third Isolation Area, and Nuaele's plan with them."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Kupole Arune seems to know something. Talk to Kupole Arune."));
		SetPhase(QuestStatus.InProgress, "VPRISON512_MQ_03_AREA", "d_velniasprison_51_2", L("Interrogation of Nuaele's plan"), L("Use Hauberk's seal fragment to suppress the demons in the 3rd Isolation Area and find out Nuaele's plans."));
		SetPhase(QuestStatus.Success, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Found out Nuaele's plans. Tell Kupole Arune about Nuaele's plans."));

		AddPrerequisite(new QuestStatusPrerequisite(60008, QuestStatus.Completed));

		AddObjective("interrogate", L("Interrogation of Nuaele's plan"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 60010: Ridding the Traitor (4)
//-----------------------------------------------------------------------------
public class Vprison512Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60010);
		SetName(L("Ridding the Traitor (4)"));
		SetDescription(L("The Harugals of the Fourth Isolation Area are the last servants Nuaele can rely on."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Figured out the plans of Nuaele. Talk to Kupole Arune on what to do next."));
		SetPhase(QuestStatus.InProgress, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Defeat the Harugals"), L("Kupole Arune suggested defeating the strongest and most loyal servants of Nuaele. Defeat the Harugals in the 4th Isolation Area."));
		SetPhase(QuestStatus.Success, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Defeated the Harugals. Talk to Kupole Arune again."));

		AddPrerequisite(new QuestStatusPrerequisite(60009, QuestStatus.Completed));

		AddObjective("killHarugals", L("Defeat Harugals in the 4th Isolation Area"), new KillObjective(2, "Harugal"));

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 60011: Lodged Stone
//-----------------------------------------------------------------------------
public class Vprison512Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60011);
		SetName(L("Lodged Stone"));
		SetDescription(L("Aldona opens the way into Nuaele's territory, and Nuaele is put down in the middle of her own ritual."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("You're prepared to defeat the Demon Lord Nuaele. Talk to Kupole Arune."));
		SetPhase(QuestStatus.InProgress, "VPRISON512_MQ_ALDONA", "d_velniasprison_51_2", L("Defeat Demon Lord Nuaele"), L("Meet Kupole Aldona and head to Nuaele's territory to defeat Nuaele."));
		SetPhase(QuestStatus.Success, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Defeated Nuaele. Talk to Kupole Arune."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON512_MQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60010, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(60007, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(60008, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(60009, QuestStatus.Completed));

		AddObjective("killNuaele", L("Defeat Demon Lord Nuaele"), new KillObjective(1, "boss_Nuaelle") { LayerOnly = true });

		AddReward(new ItemReward("expCard8", 3));
	}
}

// 60031: Unreasonable Defeat
//-----------------------------------------------------------------------------
public class Vprison512Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60031);
		SetName(L("Unreasonable Defeat"));
		SetDescription(L("What is left of Nuaele's following is gathering in the First Isolation Area to carry on her will."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Though Nuaele has disappeared, there are still remnants left. Talk to Kupole Arune."));
		SetPhase(QuestStatus.InProgress, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Defeat the remnants of Nuaele"), L("Nuaele is gone from Demon Prison and to stabilize it, Kupole Arune asked you defeat the remaining demons in 1st Isolation Area."));
		SetPhase(QuestStatus.Success, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("All of Nuaele's remnants have been defeated. Talk to Kupole Arune."));

		AddPrerequisite(new LevelPrerequisite(144));
		AddPrerequisite(new QuestStatusPrerequisite(60011, QuestStatus.Completed));

		AddObjective("killRemnants", L("Defeat demons"), new KillObjective(8, "defender_spider", "Nuka", "Elet"));

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 60032: Recovering Prestige
//-----------------------------------------------------------------------------
public class Vprison512Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60032);
		SetName(L("Recovering Prestige"));
		SetDescription(L("Nuaele's fragments still hold the demons of the Fourth Isolation Area to her orders."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Talk to Kupole Arune"), L("Kupole Arune is waiting for something. Talk to Kupole Arune."));
		SetPhase(QuestStatus.InProgress, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Collect the fragments of Nuaele"), L("There are demon monsters left in 4th Isolation Area that are still following Nuaele's orders because of her fragments. Kupole Arune asked you to defeat those monsters and retrieve the fragments."));
		SetPhase(QuestStatus.Success, "VPRISON512_MQ_NORGAILE", "d_velniasprison_51_2", L("Take it to Kupole Arune"), L("All of Nuaele's Fragments have been collected. Take them to Kupole Arune."));

		AddPrerequisite(new LevelPrerequisite(144));
		AddPrerequisite(new QuestStatusPrerequisite(60011, QuestStatus.Completed));

		AddObjective("collectFragments", L("Retrieve the fragments of Nuaele"), new CollectItemObjective("VPRISON512_SQ_02_ITEM", 7));

		AddPityDrop("VPRISON512_SQ_02_ITEM", 1.0f, 0, 1, "defender_spider", "Nuka", "Elet");

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("VPRISON512_SQ_02_ITEM"));
	}
}
