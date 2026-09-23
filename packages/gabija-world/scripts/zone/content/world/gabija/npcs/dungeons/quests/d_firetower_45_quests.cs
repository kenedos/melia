//--- Melia Script ----------------------------------------------------------
// Mage Tower 5F Quest NPCs
//--- Description -----------------------------------------------------------
// The four magic suppressors Helgasercle left on the top floor, the wizard
// still fighting for it, and the goddess waiting at the Great Hall.
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

/// <summary>
/// The two tracking quests the client runs over the fifth floor, which have
/// no NPC of their own and follow the suppressor and boss quests instead.
/// </summary>
public static class FiretowerTowerProgress
{
	public readonly static QuestId TowerProgress = new QuestId(17027);
	public readonly static QuestId HelgaserclePro = new QuestId(17028);

	private readonly static QuestId[] Suppressors =
	{
		new QuestId(8493), new QuestId(8494), new QuestId(8495), new QuestId(8496),
	};

	/// <summary>
	/// Ends the floor's tracking quest once all four suppressors are down.
	/// </summary>
	/// <param name="character"></param>
	public static void AdvanceSuppressors(Character character)
	{
		if (!character.Quests.IsActive(TowerProgress))
			return;

		foreach (var questId in Suppressors)
		{
			if (!character.Quests.HasCompleted(questId))
				return;
		}

		character.Quests.CompleteObjective(TowerProgress, "breakTheSuppressors");
		character.Quests.Complete(TowerProgress);
	}

	/// <summary>
	/// Ends the floor's second tracking quest once Helgasercle is down.
	/// </summary>
	/// <param name="character"></param>
	public static void AdvanceHelgasercle(Character character)
	{
		if (!character.Quests.IsActive(HelgaserclePro))
			return;

		character.Quests.CompleteObjective(HelgaserclePro, "defeatHelgasercle");
		character.Quests.Complete(HelgaserclePro);
	}
}

public class DFiretower45QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8493);
	private readonly static QuestId Mq02 = new QuestId(8494);
	private readonly static QuestId Mq03 = new QuestId(8495);
	private readonly static QuestId Mq04 = new QuestId(8496);
	private readonly static QuestId Mq05 = new QuestId(8497);
	private readonly static QuestId Mq06 = new QuestId(8498);
	private readonly static QuestId Sq01 = new QuestId(17022);
	private readonly static QuestId Sq02 = new QuestId(17023);
	private readonly static QuestId Sq03 = new QuestId(17024);
	private readonly static QuestId Sq04 = new QuestId(17025);
	private readonly static QuestId Sq05 = new QuestId(17026);
	private readonly static QuestId Hq01 = new QuestId(19061);
	private readonly static QuestId Hq02 = new QuestId(19062);

	protected override void Load()
	{
		// The four Magic Suppressors
		//-------------------------------------------------------------------------
		AddConditionalNpc(151003, L("Magic Suppressor"), "FTOWER45_MQ_01_D", "d_firetower_45", -576, -1092, 0, c => !c.Quests.HasCompleted(Mq01), dialog => this.CheckSuppressor(dialog, Mq01, L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 1st Suppressor located at the Hall of Fire.")));
		AddConditionalNpc(151003, L("Magic Suppressor"), "FTOWER45_MQ_02_D", "d_firetower_45", -501, -745, 45, c => !c.Quests.HasCompleted(Mq02), dialog => this.CheckSuppressor(dialog, Mq02, L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 2nd Suppressor located at the Reception Room.")));
		AddConditionalNpc(151003, L("Magic Suppressor"), "FTOWER45_MQ_03_D", "d_firetower_45", -12, 26, 91, c => !c.Quests.HasCompleted(Mq03), dialog => this.CheckSuppressor(dialog, Mq03, L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 3rd Suppressor located at the Small Hall.")));
		AddConditionalNpc(151003, L("Magic Suppressor"), "FTOWER45_MQ_04_D", "d_firetower_45", -223, 1036, 72, c => !c.Quests.HasCompleted(Mq04), dialog => this.CheckSuppressor(dialog, Mq04, L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 4th Suppressor at the Reading Room.")));

		// The place the Jewel of Prominence is set down
		//-------------------------------------------------------------------------
		AddConditionalNpc(151018, L("Keturidu Great Hall"), "FTOWER45_MQ_05_D", "d_firetower_45", 839.33, 2290.11, 90, c => !c.Quests.HasCompleted(Mq06), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Keturidu Great Hall"));

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				var answer = await dialog.SelectQuestOffer(Mq06, L("The center of the hall is marked for something, and the Jewel of Prominence is the shape of the mark."),
					Option(L("Set the Jewel of Prominence down"), "accept"),
					Option(L("Not yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					character.ServerMessage(L("The jewel settles into the mark and the hall begins to take light."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The mark at the center of the hall is still waiting."));
				character.Quests.ReplayQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("The center of the Keturidu Great Hall, marked for something the tower has been without."));
		});

		// Goddess Gabija
		//-------------------------------------------------------------------------
		AddConditionalNpc(147452, L("Goddess Gabija"), "FTOWER45_MQ_06_D", "d_firetower_45", 837.07, 2330.39, 0, c => c.Quests.Has(Mq06), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Goddess Gabija"));
			dialog.SetPortrait("Dlg_port_gabija");

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("I've been able to regain some of my power thanks to you, but this is only the beginning."));
				await dialog.Msg(L("I'll help protect you now by luring the demons with the fake revelation in this tower."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			await dialog.Msg(L("Go on to the Great Cathedral. Pilgrim's Way is the road to it, and it leaves from Fedimian."));
		});

		// Grita, once the tower is hers to keep
		//-------------------------------------------------------------------------
		AddConditionalNpc(151053, L("Grita"), "FTOWER45_GRITA_PHOENIX", "d_firetower_45", 860.45, 2355.37, 0, c => c.Quests.HasCompleted(Mq06), async dialog =>
		{
			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			await dialog.Msg(L("The tower holds. Whatever comes for it next will find it awake."));
		});

		// Simon Shaw, at the Hall of Fire
		//-------------------------------------------------------------------------
		AddConditionalNpc(20139, L("Simon Shaw"), "FTOWER45_SQ_01", "d_firetower_45", -1361, -1314, 90, c => !c.Quests.Has(Sq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Simon Shaw"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("You are so great. Now we will be able to face against it."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("We have to stop Bearkaras, but there are so many monsters. Can you help us?"),
					Option(L("I'll help you"), "accept"),
					Option(L("I'm sorry, but I don't think I can"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("I'm glad to hear that you will help. I want you to get back the Flame Charm I set inside Black Drakes."));
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("I will go to the Small Hall to chase after Bearkaras, so please take care of the surrounding monsters. I am counting on you!"),
					Option(L("Don't worry"), "accept"),
					Option(L("I can only help so much"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("Now it's time to get revenge!"));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Ten charms. They are inside the Black Drakes, and they will not come out politely."));
				return;
			}

			await dialog.Msg(L("A wizard who leaves the tower to the demons is no wizard at all."));
		});

		// Simon Shaw, at the Small Hall
		//-------------------------------------------------------------------------
		AddConditionalNpc(20139, L("Simon Shaw"), "FTOWER45_SQ_03", "d_firetower_45", -1239, 24, 90, c => c.Quests.Has(Sq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Simon Shaw"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("There's no way I can defeat Bearkaras myself... Sorry you have to see me like this..."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("It was a good decision to trust you. I am more motivated because of you."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (character.Quests.IsActive(Hq01) && character.Quests.IsCompletable(Hq01))
			{
				await dialog.Msg(L("Wow, you came up here! You are incredible."));
				await dialog.Msg(L("Well that's it for questions, should we do something else?"));
				await dialog.CompleteQuest(Hq01);
				return;
			}

			if (character.Quests.IsActive(Hq02) && character.Quests.IsCompletable(Hq02))
			{
				await dialog.Msg(L("Yes. That's correct! This is it."));
				await dialog.CompleteQuest(Hq02);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("Bearkaras is still near here. You are our last hope. We are counting on you."),
					Option(L("I'll defeat Bearkaras for you"), "accept"),
					Option(L("Better run away quickly"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("Don't ever think of blocking its axe head-on. You will be crushed into a pulp."));
				}
				return;
			}

			if (!character.Quests.Has(Hq01) && character.Quests.MeetsPrerequisites(Hq01))
			{
				await dialog.Msg(L("You sure know a lot of people. Every wizard on the way up this tower knows your name by now."));

				var answer = await dialog.SelectQuestOffer(Hq01, L("Each of them sets a question for anyone who comes past. You have answered all of theirs, so mine is the last one left."),
					Option(L("I'll take the bet"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq01);
					character.Quests.CompleteObjective(Hq01, "reachSimonShaw");
				}
				return;
			}

			if (!character.Quests.Has(Hq02) && character.Quests.MeetsPrerequisites(Hq02))
			{
				var answer = await dialog.SelectQuestOffer(Hq02, L("It's nothing special. If you can completely collect the scattered pages of the book at the Laboratory on the 3rd floor, I will approve that as you answered the question."),
					Option(L("I will do it"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq02);
					await dialog.Msg(L("That book is very important to me so I can't just leave this place."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Hold the ones coming up behind us. I will keep after Bearkaras."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("It is still out there, past the hall."));
				character.Quests.ClearQuestTrack(Sq03);
				return;
			}

			if (character.Quests.IsActive(Hq02))
			{
				await dialog.Msg(L("The Laboratory on the third floor. The pages are all over its floor."));
				return;
			}

			await dialog.Msg(L("A wizard who leaves the tower to the demons is no wizard at all."));
		});

		// The sealed stone on the way to the Great Hall
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER45_SQ_04", "d_firetower_45", 599, 1146, 90, c => !c.Quests.HasCompleted(Sq05), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("Now only the last piece is left."));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("I am Hauberk, a Demon Lord. Helgasercle tore up my soul."));
				await dialog.Msg(L("A part of me is within the barrier of this tower, and the rest of me were scattered throughout the world."));
				await dialog.CompleteQuest(Sq05);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("The contract needs to continue. Please release me. As those observers are defeated, my spirit becomes free."),
					Option(L("Defeat the observing monsters"), "accept"),
					Option(L("About the possessed soul"), "explain"),
					Option(L("I'm busy now"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("I am here and all over the world. My soul was torn into pieces long ago."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("It wasn't a long time ago, but it was frightening for me."));
				}
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("The last spirit is in the monitor Stone Whale. My spirit will be free when he gets defeated."),
					Option(L("I will release the soul by defeating the Stone Whale"), "accept"),
					Option(L("I'm not interested"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("My body was ripped apart, humiliated... I can't forgive that."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("Fifteen of them. They watch this road from both sides."));
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("Quickly... We should continue the contract."));
				character.Quests.ClearQuestTrack(Sq05);
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The entrance to the Great Hall, where Helgasercle waits.
		AddQuestTrigger("FTOWER45_MQ_05_E", "d_firetower_45", 826.44, 2333.98, 300, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			FiretowerTowerProgress.AdvanceSuppressors(character);

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				character.Quests.Start(Mq05);
				character.ServerMessage(L("Helgasercle is standing at the head of the Great Hall."));
			}

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Quests.StartQuestTrack(Mq05);

			await Task.CompletedTask;
		});

		// Where Simon Shaw chased Bearkaras to.
		AddQuestTrigger("FTOWER45_SQ_T", "d_firetower_45", -1430, -193, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq03) && !character.Quests.IsCompletable(Sq03))
				character.Quests.StartQuestTrack(Sq03);

			await Task.CompletedTask;
		});

		// Where the Stone Whale lay.
		AddQuestTrigger("FTOWER45_SQ_05", "d_firetower_45", 197, 1441, 250, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq05) && !character.Quests.IsCompletable(Sq05))
				character.Quests.StartQuestTrack(Sq05);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Looks one of the floor's Magic Suppressors over, which starts the
	/// quest that breaks it.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="questId"></param>
	/// <param name="story"></param>
	private async Task CheckSuppressor(Dialog dialog, QuestId questId, string story)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Magic Suppressor"));

		FiretowerTowerProgress.AdvanceSuppressors(character);

		if (!character.Quests.Has(questId) && character.Quests.MeetsPrerequisites(questId))
		{
			await dialog.Msg(story);

			var looked = await character.TimeActions.StartAsync(L("Looking the suppressor over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

			if (looked != TimeActionResult.Completed)
				return;

			character.Quests.Start(questId);
			character.ServerMessage(L("This device is holding the tower's magic down. It would be better to destroy it quickly."));
			return;
		}

		if (character.Quests.IsActive(questId))
		{
			await dialog.Msg(L("The suppressor is still standing, and it is not standing alone."));
			character.Quests.ReplayQuestTrack(questId);
			return;
		}

		await dialog.Msg(L("One of the devices Helgasercle set up to hold the tower's magic down."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 17027: Goddess' Tower
//-----------------------------------------------------------------------------
public class Ftower45MqProQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17027);
		SetName(L("Goddess' Tower"));
		SetDescription(L("Helgasercle set four Magic Suppressors on the top floor, and every one of them has to come down."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "FTOWER45_MQ_01_D", "d_firetower_45", L("Find the goddess in Mage Tower 5F"), L("Look for the goddess in Mage Tower 5F."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_MQ_01_D", "d_firetower_45", L("Destroy the Magic Suppressors prepared by Helgasercle"), L("Destroy all the Magic Suppressors that Helgasercle prepared on the 5th floor of the Mage Tower."));
		SetPhase(QuestStatus.Success, "FTOWER45_MQ_05_E", "d_firetower_45", L("Destroy the Magic Suppressors prepared by Helgasercle"), L("Destroy all the Magic Suppressors that Helgasercle prepared on the 5th floor of the Mage Tower."));

		AddPrerequisite(new QuestStatusPrerequisite(8492, QuestStatus.Completed));

		AddObjective("breakTheSuppressors", L("Destroy the Magic Suppressors prepared by Helgasercle"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17028: Demon Lord Helgasercle
//-----------------------------------------------------------------------------
public class Ftower45SqProQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17028);
		SetName(L("Demon Lord Helgasercle"));
		SetDescription(L("With the suppressors down, what is left is the demon lord who set them."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "FTOWER45_MQ_05_E", "d_firetower_45", L("Defeat Helgasercle"), L("Find and defeat Helgasercle who invaded the Mage Tower."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_MQ_05_E", "d_firetower_45", L("Defeat Helgasercle"), L("Find and defeat Helgasercle who invaded the Mage Tower."));
		SetPhase(QuestStatus.Success, "FTOWER45_MQ_05_D", "d_firetower_45", L("Use the Jewel of Prominence"), L("Helgasercle is down. Take the Jewel of Prominence to the Keturidu Great Hall."));

		AddPrerequisite(new QuestStatusPrerequisite(17027, QuestStatus.Completed));

		AddObjective("defeatHelgasercle", L("Defeat Helgasercle"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8493: Destroy the 1st Magic Suppressor
//-----------------------------------------------------------------------------
public class Ftower45Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8493);
		SetName(L("Destroy the 1st Magic Suppressor"));
		SetDescription(L("The first of Helgasercle's suppressors stands in the Hall of Fire."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_MQ_01_D", "d_firetower_45", L("Destroy Helgasercle's 1st Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 1st Suppressor located at the Hall of Fire."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_MQ_01_D", "d_firetower_45", L("Destroy Helgasercle's 1st Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 1st Suppressor located at the Hall of Fire."));
		SetPhase(QuestStatus.Success, "FTOWER45_MQ_01_D", "d_firetower_45", L("Destroy Helgasercle's 1st Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 1st Suppressor located at the Hall of Fire."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER45_MQ_01_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(103));

		AddObjective("breakSuppressor", L("Destroy the Magic Suppressor"), new KillObjective(1, "spell_suppressors") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		base.OnComplete(character, quest);

		FiretowerTowerProgress.AdvanceSuppressors(character);
	}
}

// 8494: Destroy the 2nd Magic Suppressor
//-----------------------------------------------------------------------------
public class Ftower45Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8494);
		SetName(L("Destroy the 2nd Magic Suppressor"));
		SetDescription(L("The second of Helgasercle's suppressors stands in the Reception Room."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_MQ_02_D", "d_firetower_45", L("Destroy Helgasercle's 2nd Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 2nd Suppressor located at the Reception Room."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_MQ_02_D", "d_firetower_45", L("Destroy Helgasercle's 2nd Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 2nd Suppressor located at the Reception Room."));
		SetPhase(QuestStatus.Success, "FTOWER45_MQ_02_D", "d_firetower_45", L("Destroy Helgasercle's 2nd Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 2nd Suppressor located at the Reception Room."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER45_MQ_02_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(103));

		AddObjective("breakSuppressor", L("Destroy the Magic Suppressor"), new KillObjective(1, "spell_suppressors") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		base.OnComplete(character, quest);

		FiretowerTowerProgress.AdvanceSuppressors(character);
	}
}

// 8495: Destroy the 3rd Magic Suppressor
//-----------------------------------------------------------------------------
public class Ftower45Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8495);
		SetName(L("Destroy the 3rd Magic Suppressor"));
		SetDescription(L("The third of Helgasercle's suppressors stands in the Small Hall."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_MQ_03_D", "d_firetower_45", L("Destroy Helgasercle's 3rd Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 3rd Suppressor located at the Small Hall."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_MQ_03_D", "d_firetower_45", L("Destroy Helgasercle's 3rd Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 3rd Suppressor located at the Small Hall."));
		SetPhase(QuestStatus.Success, "FTOWER45_MQ_03_D", "d_firetower_45", L("Destroy Helgasercle's 3rd Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 3rd Suppressor located at the Small Hall."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER45_MQ_03_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(103));

		AddObjective("breakSuppressor", L("Destroy the Magic Suppressor"), new KillObjective(1, "spell_suppressors") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		base.OnComplete(character, quest);

		FiretowerTowerProgress.AdvanceSuppressors(character);
	}
}

// 8496: Destroy the 4th Magic Suppressor
//-----------------------------------------------------------------------------
public class Ftower45Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8496);
		SetName(L("Destroy the 4th Magic Suppressor"));
		SetDescription(L("The last of Helgasercle's suppressors stands in the Reading Room."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_MQ_04_D", "d_firetower_45", L("Destroy Helgasercle's 4th Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 4th Suppressor at the Reading Room."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_MQ_04_D", "d_firetower_45", L("Destroy Helgasercle's 4th Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 4th Suppressor at the Reading Room."));
		SetPhase(QuestStatus.Success, "FTOWER45_MQ_04_D", "d_firetower_45", L("Destroy Helgasercle's 4th Magic Suppressor"), L("Helgasercle created Magic Suppressors on the 5th floor to weaken the Mage Tower's power. Destroy the 4th Suppressor at the Reading Room."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER45_MQ_04_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(103));

		AddObjective("breakSuppressor", L("Destroy the Magic Suppressor"), new KillObjective(1, "spell_suppressors") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		base.OnComplete(character, quest);

		FiretowerTowerProgress.AdvanceSuppressors(character);
	}
}

// 8497: Helgasercle Invading the Tower
//-----------------------------------------------------------------------------
public class Ftower45Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8497);
		SetName(L("Helgasercle Invading the Tower"));
		SetDescription(L("With all four suppressors down, the demon lord who set them is the last thing in the way."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_MQ_05_E", "d_firetower_45", L("Find Helgasercle"), L("You destroyed all four Magic Suppressors that Helgasercle prepared. It is now time to liberate the Mage Tower from the demon's powers. Find Helgasercle at the Great Hall."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_MQ_05_E", "d_firetower_45", L("Defeat Helgasercle"), L("You destroyed all four Magic Suppressors that Helgasercle prepared. It is now time to liberate the Mage Tower from the demon's powers."));
		SetPhase(QuestStatus.Success, "FTOWER45_MQ_05_E", "d_firetower_45", L("Defeat Helgasercle"), L("You destroyed all four Magic Suppressors that Helgasercle prepared. It is now time to liberate the Mage Tower from the demon's powers."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER45_MQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8493, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(8494, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(8495, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(8496, QuestStatus.Completed));

		AddObjective("killHelgasercle", L("Defeat Helgasercle"), new KillObjective(1, "boss_helgasercle") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
		AddReward(new ItemReward("Drug_AddStat", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Helgasercle is down and the Mage Tower is free of her."));
		character.Quests.Complete(this.QuestId);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		base.OnComplete(character, quest);

		FiretowerTowerProgress.AdvanceHelgasercle(character);
	}
}

// 8498: Goddess Gabija
//-----------------------------------------------------------------------------
public class Ftower45Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8498);
		SetName(L("Goddess Gabija"));
		SetDescription(L("The Jewel of Prominence goes into the mark at the center of the Keturidu Great Hall."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "FTOWER45_MQ_05_D", "d_firetower_45", L("Use the Jewel of Prominence"), L("You should help Gabija now with the Jewel of Prominence. Put the jewel in the center of Keturidu Great Hall."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_MQ_05_D", "d_firetower_45", L("Use the Jewel of Prominence"), L("You should help Gabija now with the Jewel of Prominence. Put the jewel in the center of Keturidu Great Hall."));
		SetPhase(QuestStatus.Success, "FTOWER45_MQ_06_D", "d_firetower_45", L("Talk to Goddess Gabija"), L("You received the revelation from the goddess. Ask the goddess what to do next."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER45_MQ_05_GABIA_END_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(17028, QuestStatus.Completed));

		AddObjective("giveTheJewel", L("Use the Jewel of Prominence"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new ItemReward("stonetablet05", 1));
		AddReward(new StatPointReward(3));
		AddReward(new TakeItemReward("FTOWER_FIRE_ESSENCE_2"));
	}
}

// 17022: Hot-blooded Simon Shaw (1)
//-----------------------------------------------------------------------------
public class Ftower45Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17022);
		SetName(L("Hot-blooded Simon Shaw (1)"));
		SetDescription(L("A well-dressed wizard is still holding the Hall of Fire, and his charms are inside the Black Drakes."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_SQ_01", "d_firetower_45", L("Talk to Simon Shaw"), L("There is a well-dressed, gentlemanly wizard resisting the monsters. Talk to Simon Shaw."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_SQ_01", "d_firetower_45", L("Defeat Black Drake and retrieve the Flame Charms"), L("Simon Shaw is doing his best to resist the monsters. Help him retrieve the Flame Charms that he planted on monsters."));
		SetPhase(QuestStatus.Success, "FTOWER45_SQ_01", "d_firetower_45", L("Give Flame Charms to Simon Shaw"), L("Acquired all Flame Charms. Give them to Simon Shaw."));

		AddPrerequisite(new LevelPrerequisite(116));

		AddPityDrop("FTOWER45_SQ_01_01", 0.5f, 4, 1, "Fire_Dragon_purple");

		AddObjective("collectCharms", L("Defeat Black Drake and retrieve the Flame Charms"), new CollectItemObjective("FTOWER45_SQ_01_01", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER45_SQ_01_01"));
	}
}

// 17023: Hot-blooded Simon Shaw (2)
//-----------------------------------------------------------------------------
public class Ftower45Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17023);
		SetName(L("Hot-blooded Simon Shaw (2)"));
		SetDescription(L("Simon Shaw goes after Bearkaras and leaves the hall behind him to be held."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_SQ_01", "d_firetower_45", L("Talk to Simon Shaw"), L("Simon Shaw saw something. Talk to him."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_SQ_01", "d_firetower_45", L("Defeat the monsters nearby"), L("Simon Shaw says he saw Bearkaras and will chase him. Stop the monsters marching in while Simon chases after Bearkaras."));
		SetPhase(QuestStatus.Success, "FTOWER45_SQ_03", "d_firetower_45", L("Find Simon Shaw"), L("Simon Shaw is not back. Go to the Small Hall where he went chasing Bearkaras."));

		AddPrerequisite(new QuestStatusPrerequisite(17022, QuestStatus.Completed));

		AddObjective("killDimmers", L("Defeat Dimmer"), new KillObjective(5, "dimmer"));
		AddObjective("killPuppets", L("Defeat Black Shaman Doll"), new KillObjective(5, "tower_of_firepuppet_black"));
		AddObjective("killDrakes", L("Defeat Black Drake"), new KillObjective(5, "Fire_Dragon_purple"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17024: Hot-blooded Simon Shaw (3)
//-----------------------------------------------------------------------------
public class Ftower45Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17024);
		SetName(L("Hot-blooded Simon Shaw (3)"));
		SetDescription(L("Bearkaras is more than Simon Shaw can take on his own."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_SQ_03", "d_firetower_45", L("Talk to Simon Shaw"), L("Simon looks very tired. Talk to him."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_SQ_T", "d_firetower_45", L("Defeat Bearkaras"), L("Simon says he can't defeat Bearkaras on his own. Defeat the Bearkaras for him."));
		SetPhase(QuestStatus.Success, "FTOWER45_SQ_03", "d_firetower_45", L("Talk to Simon Shaw"), L("Defeated Bearkaras. Tell Simon Shaw about it so that he can be relieved."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER45_SQ_03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(17023, QuestStatus.Completed));

		AddObjective("killBearkaras", L("Defeat Bearkaras"), new KillObjective(1, "boss_bearkaras_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new SelectItemReward("R_TOP02_178", "R_TOP02_179", "R_TOP02_180"));
	}
}

// 17025: Truth of the Suspicious Seal Stone (1)
//-----------------------------------------------------------------------------
public class Ftower45Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17025);
		SetName(L("Truth of the Suspicious Seal Stone (1)"));
		SetDescription(L("The last of the stones stands on the road to the Great Hall, and it knows the others."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_SQ_04", "d_firetower_45", L("Find the Sealed Stone"), L("If you had saved all of the sealed souls in the sealed stones past the Mage Tower, you will be able to receive the last mission from the sealed stone on the way to the Great Hall of Keturidu."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_SQ_04", "d_firetower_45", L("Defeat the monsters nearby"), L("If you do the seal stone's favors, then you will be able to find out the identity of those trapped inside the seal stones."));
		SetPhase(QuestStatus.Success, "FTOWER45_SQ_04", "d_firetower_45", L("Report to the Sealed Stone"), L("Defeated all the monsters around. Return to the seal stone."));

		// The client's only other gate is a script the server cannot recover.
		AddPrerequisite(new LevelPrerequisite(116));
		AddPrerequisite(new QuestStatusPrerequisite(17013, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(17015, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(17020, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(17021, QuestStatus.Completed));

		AddObjective("killWatchers", L("Defeat the nearby monsters"), new KillObjective(15, "dimmer", "tower_of_firepuppet_black", "Fire_Dragon_purple"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17026: Truth of the Suspicious Seal Stone (2)
//-----------------------------------------------------------------------------
public class Ftower45Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17026);
		SetName(L("Truth of the Suspicious Seal Stone (2)"));
		SetDescription(L("The last piece of the soul is held by the Stone Whale that was set over it."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_SQ_04", "d_firetower_45", L("Talk to the Sealed Stone"), L("Talk to the Seal Stone again."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_SQ_05", "d_firetower_45", L("Defeat Stone Whale"), L("The Stone Whale that was protecting Hauberk woke up. Defeat the Stone Whale that is holding the soul."));
		SetPhase(QuestStatus.Success, "FTOWER45_SQ_04", "d_firetower_45", L("Release the Seal Stone"), L("Return to Hauberk and release his last soul and wish for the future."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER45_SQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(17025, QuestStatus.Completed));

		AddObjective("killStoneWhale", L("Defeat Stone Whale"), new KillObjective(1, "boss_stone_whale_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 19061: The Wizard and the Mage Tower (1)
//-----------------------------------------------------------------------------
public class Firetower45Hq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19061);
		SetName(L("The Wizard and the Mage Tower (1)"));
		SetDescription(L("Every wizard on the way up the tower sets a question, and Simon Shaw's is the last one left."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45");
		SetAutoTracked(true);
		SetCancelable(true);

		// The client's giver is a class master the 2016 data places outside
		// this chain's maps, so Simon Shaw hands it out as well as taking it.
		SetPhase(QuestStatus.Possible, "FTOWER45_SQ_03", "d_firetower_45", L("Talk to Simon Shaw"), L("Talk to Simon Shaw for the last quiz question."));
		SetPhase(QuestStatus.InProgress, "FTOWER45_SQ_03", "d_firetower_45", L("Solve the quiz of Simon Shaw in Mage Tower 5th Floor"), L("Solved quizzes of three wizards. Talk to Simon Shaw in Mage Tower 5th Floor for the last quiz question."));
		SetPhase(QuestStatus.Success, "FTOWER45_SQ_03", "d_firetower_45", L("Solve the quiz of Simon Shaw in Mage Tower 5th Floor"), L("Solved quizzes of three wizards. Talk to Simon Shaw in Mage Tower 5th Floor for the last quiz question."));

		// The client's only other gate is a script the server cannot recover.
		AddPrerequisite(new LevelPrerequisite(113));
		AddPrerequisite(new QuestStatusPrerequisite(17024, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(17003, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(17006, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(17019, QuestStatus.Completed));

		AddObjective("reachSimonShaw", L("Solve the quiz of Simon Shaw in Mage Tower 5th Floor"), new ManualObjective());
	}
}

// 19062: The Wizard and the Mage Tower (2)
//-----------------------------------------------------------------------------
public class Firetower45Hq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19062);
		SetName(L("The Wizard and the Mage Tower (2)"));
		SetDescription(L("Simon Shaw would rather have his book back than hear an answer."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_45", "d_firetower_43");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER45_SQ_03", "d_firetower_45", L("Talk to Simon Shaw"), L("Talk to Simon Shaw for the last quiz question."));
		SetPhase(QuestStatus.InProgress, "FTOWER43_MQ_03_BOOK1", "d_firetower_43", L("Find the page of the book in the Mage Tower 3rd Floor Laboratory"), L("Simon Shaw offered bringing him a page of book instead of the quiz. Find the book in the Mage Tower 3rd Floor Laboratory."));
		SetPhase(QuestStatus.Success, "FTOWER45_SQ_03", "d_firetower_45", L("Give it to Simon Shaw"), L("Found all the pages. Talk to Simon Shaw."));

		AddPrerequisite(new QuestStatusPrerequisite(19061, QuestStatus.Completed));

		AddObjective("findTheBookPage", L("Find the page of the book in the Mage Tower 3rd Floor Laboratory"), new ManualObjective());
	}
}
