//--- Melia Script ----------------------------------------------------------
// Nefritas Cliff Quest NPCs
//--- Description -----------------------------------------------------------
// The Watchers, the Followers and the barriers the cliff's quests run on.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele573QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8538);
	private readonly static QuestId Mq02 = new QuestId(8539);
	private readonly static QuestId Mq03 = new QuestId(8540);
	private readonly static QuestId Mq04 = new QuestId(8541);
	private readonly static QuestId Mq05 = new QuestId(8542);
	private readonly static QuestId Mq06 = new QuestId(8543);
	private readonly static QuestId Mq07 = new QuestId(8544);
	private readonly static QuestId Mq08 = new QuestId(8545);
	private readonly static QuestId Mq09 = new QuestId(8546);
	private readonly static QuestId Hq01 = new QuestId(9102);
	private readonly static QuestId Hq02 = new QuestId(9104);
	private readonly static QuestId Reveal2 = new QuestId(30031);

	protected override void Load()
	{
		// Watcher Allen
		//-------------------------------------------------------------------------
		AddNpc(147422, L("Watcher Allen"), "GELE573_ALLEN", "f_gele_57_3", -770, -1083, 92, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Allen"));

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("The barrier that used to stop the demons at Mazas Rest Place is broken."));
				var answer = await dialog.SelectQuestOffer(Mq01, L("I want to fix it, could you lend me your help?"),
					Option(L("I'll help if it's simple"), "accept"),
					Option(L("About the barriers in Nefritas Cliff"), "explain"),
					Option(L("I'm busy on my way"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("This is one of the devices the first Paladin set in preparation for the demon invasion."));
					await dialog.Msg(L("Nefritas Cliff is covered with several of these devices."));
					return;
				}

				if (answer == "accept")
				{
					await dialog.Msg(L("The pieces of the destroyed barrier are scattered at Mazas Rest Place."));
					await dialog.Msg(L("Collect them and give them to Kayetonas at Flower Greeting Hill."));
					character.Quests.Start(Mq01);
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("Activating the Tree Guard Post Barrier is taking longer than I thought."));
				var answer = await dialog.SelectQuestOffer(Mq02, L("We have no time, let's charge it with demon souls instead."),
					Option(L("I'll help"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Defeat the demons around the Tree Guard Post Barrier. Their souls should fill its divine power a little."));
					character.Quests.Start(Mq02);
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Collect the barrier pieces at Mazas Rest Place. Kayetonas is at Flower Greeting Hill."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Charge the barrier at the Tree Guard Post. Kayetonas wants to hear of it."));
				return;
			}

			await dialog.Msg(L("The barriers stand, most of them. That has to be enough."));
		});

		// Watcher Kenneth
		//-------------------------------------------------------------------------
		AddNpc(147423, L("Watcher Kenneth"), "GELE573_KENNETH", "f_gele_57_3", 799, -183, 168, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Kenneth"));

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("I didn't know the demons would use summoning circles to come through."));
				var answer = await dialog.SelectQuestOffer(Mq03, L("I suggest removing them all before they besiege us."),
					Option(L("I'll destroy the Demon Summoning Circles"), "accept"),
					Option(L("It will be fine"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq03);

				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("It might be a bit extreme, but I'm thinking about eradicating all the demon's souls."),
					Option(L("Alright"), "accept"),
					Option(L("It's too difficult"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Weaken them below half and use the holy powers of the barrier to sever their souls."));
					character.Quests.Start(Mq04);
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("I'm exhausted and would like to rest for a bit."));
				var answer = await dialog.SelectQuestOffer(Mq05, L("I just need some time, so do you mind taking care of the demons around here?"),
					Option(L("I'll defeat the demons while resting"), "accept"),
					Option(L("Cheer up and hold on"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq05);

				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The summoning circles are at Mairunas Knoll. Remove them before they can be used again."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The barrier at Pumpura Hill will sever the souls. Use it."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Just a little longer. I can hold on."));
				return;
			}

			await dialog.Msg(L("The demons keep coming. It never seems to end."));
		});

		// Follower Kayetonas
		//-------------------------------------------------------------------------
		AddNpc(147400, L("Follower Kayetonas"), "GELE573_KAROLINA", "f_gele_57_3", 266, 546, 85, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Kayetonas"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("These are pieces of the barrier at Mazas Rest Place."));
				await dialog.Msg(L("Do not worry. There are ways to recover the barrier, so I'm sure it will be alright."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("So the Tree Guard Post Barrier has been charged with demon souls, huh."));
				await dialog.Msg(L("It still needs some final touches, but I'll take care of it from here. Great job."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("No wonder. I was bewildered when the demons suddenly appeared out of nowhere."));
				await dialog.Msg(L("I better let my brothers know about this. Thank you."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("It's amazing you can use the barrier like that."));
				await dialog.Msg(L("Even the Paladins did not know about that."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Even though he's still young, Kenneth is amazing."));
				await dialog.Msg(L("I think we can trust him a little more."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("The demons are very strong."));
				await dialog.Msg(L("I will protect this place to block those demons."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("I have received an urgent message from the Watchers."));
				var answer = await dialog.SelectQuestOffer(Mq06, L("A gigantic demon monster is coming this way. Quickly, follow me."),
					Option(L("I'll go with you"), "accept"),
					Option(L("About the Followers"), "explain"),
					Option(L("I need to prepare myself"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("We are the people following the Paladin Master."));
					await dialog.Msg(L("We will defeat those who disobey the goddess."));
					return;
				}

				if (answer == "accept")
					character.Quests.Start(Mq06);

				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The Minotaur is coming. Stay close to me."));
				character.Quests.ReplayQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("The Paladin Master is above, at Uzbaiga Hillside."));
		});

		// Paladin Master
		//-------------------------------------------------------------------------
		AddNpc(57223, L("[Paladin Master]{nl}Valentinas Naimon"), "GELE573_MASTER", "f_gele_57_3", 62, -135, 89, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Paladin Master"));
			dialog.SetPortrait("Dlg_port_Vlaentinas_Naimon");

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("I asked my precious friend Algis to go to the Tenet Church."));
				await dialog.Msg(L("I won't lose to Gesti, but it is important to have some backup plan just in case."));
				await dialog.CompleteQuest(Mq07);
				return;
			}

			if (character.Quests.IsActive(Mq09) && character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("Sorry. My abilities weren't good enough."));
				await dialog.Msg(L("Gesti has now realized that the Holy Relic is not the revelation, and is probably heading straight to the church."));
				await dialog.CompleteQuest(Mq09);
				return;
			}

			if (character.Quests.IsActive(Mq08) && character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("So Gesti just ran away."));
				await dialog.Msg(L("Fine. Since you got the revelation, we've completed the mission."));
				await dialog.CompleteQuest(Mq08);
				return;
			}

			if (character.Quests.IsActive(Hq02) && character.Quests.IsCompletable(Hq02))
			{
				await dialog.Msg(L("The Chapparition will not bring back the lives of those who died."));
				await dialog.Msg(L("It's a sad thing... But we can't just leave it like that."));
				await dialog.CompleteQuest(Hq02);
				return;
			}

			if (!character.Quests.Has(Reveal2) && character.Quests.MeetsPrerequisites(Reveal2))
			{
				await dialog.Msg(L("So Gesti just ran away."));
				await dialog.Msg(L("Fine. Since you got the revelation, we've completed the mission here."));
				character.Quests.Start(Reveal2);
				character.Quests.CompleteObjective(Reveal2, "tellStory");
				await dialog.CompleteQuest(Reveal2);
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("Welcome. I'm glad to see that you've had a safe journey here."));
				var answer = await dialog.SelectQuestOffer(Mq07, L("Before anything else, there are a few things about this place that the Revelator should know."),
					Option(L("I'll ask"), "accept"),
					Option(L("I'm not ready to hear it yet"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("There is enough time, so go meet with Follower Algis first."));
					character.Quests.Start(Mq07);
				}
				return;
			}

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("Starting from now, I will focus on the Divine Sphere."));
				var answer = await dialog.SelectQuestOffer(Mq09, L("In the meantime, please use your skills so that nothing can disturb me."),
					Option(L("Yes, I'll help you concentrate"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq09);

				return;
			}

			if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
			{
				var answer = await dialog.SelectQuestOffer(Mq08, L("The Paladin Master is looking for you."),
					Option(L("What is happening?"), "accept"),
					Option(L("Wait a moment"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq08);

				return;
			}

			if (!character.Quests.Has(Hq02) && character.Quests.MeetsPrerequisites(Hq02))
			{
				await dialog.Msg(L("Without the blessing from the goddess, you would probably be with the goddess now too."));
				await dialog.Msg(L("Nonetheless, a lot of people are already like that without such blessing."));
				var answer = await dialog.SelectQuestOffer(Hq02, L("There is a spooky Chapparition slaying people in the Tenet Church. Find and defeat it."),
					Option(L("I will find and defeat Chapparition"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Hq02);

				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Follower Algis is beside me. Hear what he has to say."));
				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("Hold the line while I focus on the Divine Sphere."));
				character.Quests.ReplayQuestTrack(Mq09);
				return;
			}

			if (character.Quests.IsActive(Mq08))
			{
				await dialog.Msg(L("Gesti is near. We end this here."));
				return;
			}

			await dialog.Msg(L("The demons press on Nefritas Cliff. The Followers hold the line."));
		});

		// Follower Algis
		//-------------------------------------------------------------------------
		AddNpc(11281, L("Follower Algis"), "GELE573_MQ_07_F", "f_gele_57_3", 86, -110, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Algis"));
			dialog.SetPortrait("Dlg_port_algis");

			if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("I am about to head down this path to the church."));
				await dialog.Msg(L("Our Paladin friend likes the word 'if' a lot."));
				await dialog.Msg(L("Keep the Tenet Church in your mind. If the Master falls short, that is where the answer will be."));
				character.Quests.CompleteObjective(Mq07, "hearAlgis");
				return;
			}

			await dialog.Msg(L("The church is below, past the cliff road. Hold it in your mind."));
		});

		// Watcher James
		//-------------------------------------------------------------------------
		AddNpc(147406, L("Watcher James"), "GELE_57_3_HQ01_NPC01", "f_gele_57_3", -638, -1276, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher James"));

			if (character.Quests.IsActive(Hq01) && character.Quests.IsCompletable(Hq01))
			{
				await dialog.Msg(L("Haha, I lost."));
				await dialog.Msg(L("You are great as I heard so. Here, it's your reward as promised."));
				await dialog.CompleteQuest(Hq01);
				return;
			}

			if (!character.Quests.Has(Hq01) && character.Quests.MeetsPrerequisites(Hq01))
			{
				var answer = await dialog.SelectQuestOffer(Hq01, L("Vubbe Tokens, Panto Horns, Merog Hearts, and Hogma Teeth. That is a lot of stuff!"),
					Option(L("I will accept that bet"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Try and perform a killing spree of the monsters in the Owl Burial Ground."));
					await dialog.Msg(L("Around 40 of them? I bet you can't do it."));
					character.Quests.Start(Hq01);
				}
				return;
			}

			if (character.Quests.IsActive(Hq01))
			{
				await dialog.Msg(L("The monsters in the Owl Burial Ground should be a fair bet for you. Right?"));
				return;
			}

			await dialog.Msg(L("A wager is a wager. The Owl Burial Ground is waiting."));
		});

		// Barrier Piece at Mazas Rest Place
		//-------------------------------------------------------------------------
		AddNpc(147380, L("Barrier Piece"), "GELE573_MQ_01", "f_gele_57_3", -233, -651, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Barrier Piece"));

			if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("You gather the shattered pieces of the barrier from the ground."));
				character.Inventory.Add(650704, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Mq01, "collectPieces");
				return;
			}

			await dialog.Msg(L("Broken pieces of an old barrier lie scattered at Mazas Rest Place."));
		});

		// Tree Guard Post Barrier
		//-------------------------------------------------------------------------
		AddNpc(147413, L("Tree Guard Post Barrier"), "GELE573_BASIC_1", "f_gele_57_3", 249, -733, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Tree Guard Post Barrier"));

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("You feed the barrier the demon souls you gathered. It hums and steadies."));
				character.Quests.CompleteObjective(Mq02, "chargeBarrier");
				return;
			}

			await dialog.Msg(L("The Tree Guard Post Barrier flickers, not yet fully charged."));
		});

		// Demon Summoning Circle
		//-------------------------------------------------------------------------
		AddNpc(147372, L("Demon Summoning Circle"), "GELE573_MQ_03_AI_KILL", "f_gele_57_3", 943, -625, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Demon Summoning Circle"));

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("You scuff out the glowing sigils. The circle dies with a hiss."));
				character.Quests.CompleteObjective(Mq03, "removeCircles");
				return;
			}

			await dialog.Msg(L("A demon summoning circle glows faintly on the ground."));
		});

		// Pumpura Hill Barrier
		//-------------------------------------------------------------------------
		AddNpc(147413, L("Pumpura Hill Barrier"), "GELE573_MQ_04", "f_gele_57_3", 823, 90, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Pumpura Hill Barrier"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You turn the barrier's holy power on the severed souls. They scatter and fade."));
				character.Quests.CompleteObjective(Mq04, "severSouls");
				return;
			}

			await dialog.Msg(L("The barrier at Pumpura Hill pulses with holy power."));
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8538: Destroyed Barrier
//-----------------------------------------------------------------------------
public class Gele573Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8538);
		SetName(L("Destroyed Barrier"));
		SetDescription(L("Collect the pieces of the barrier the demons destroyed at Mazas Rest Place."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_ALLEN", "f_gele_57_3", L("Talk to Watcher Allen"), L("Watcher Allen is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MQ_01", "f_gele_57_3", L("Collect Destroyed Barrier Piece"), L("Collect the pieces of the barrier at Mazas Rest Place."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Give the pieces to Follower Kayetonas."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("collectPieces", L("Collect Destroyed Barrier Piece"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new SelectItemReward("LEG02_160", "LEG02_161", "LEG02_162"));
		AddReward(new TakeItemReward("GELE573_MQ_01_ITEM"));
	}
}

// 8539: Out of Time...
//-----------------------------------------------------------------------------
public class Gele573Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8539);
		SetName(L("Out of Time..."));
		SetDescription(L("Charge the Tree Guard Post Barrier with demon souls."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_ALLEN", "f_gele_57_3", L("Talk to Watcher Allen"), L("Watcher Allen is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_BASIC_1", "f_gele_57_3", L("Charge the Tree Guard Post Barrier"), L("Defeat the demons around the Tree Guard Post Barrier and charge it."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Inform Follower Kayetonas the barrier has been charged."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("chargeBarrier", L("Charge the Tree Guard Post Barrier"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 8540: Demon Summoning Circle
//-----------------------------------------------------------------------------
public class Gele573Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8540);
		SetName(L("Demon Summoning Circle"));
		SetDescription(L("Remove the demon summoning circles at Mairunas Knoll."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_KENNETH", "f_gele_57_3", L("Talk to Watcher Kenneth"), L("Watcher Kenneth is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MQ_03_AI_KILL", "f_gele_57_3", L("Remove the Demon Summoning Circles"), L("Remove the summoning circles at Mairunas Knoll."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Tell Follower Kayetonas there were Demon Summoning Circles."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("removeCircles", L("Remove the Demon Summoning Circles"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8541: To the Goddess at Once
//-----------------------------------------------------------------------------
public class Gele573Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8541);
		SetName(L("To the Goddess at Once"));
		SetDescription(L("Use the barrier's holy power to sever the demons' souls at Pumpura Hill."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_KENNETH", "f_gele_57_3", L("Talk to Watcher Kenneth"), L("Watcher Kenneth is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MQ_04", "f_gele_57_3", L("Defeat severed demons' souls"), L("Weaken the demons and use the barrier to sever their souls."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Report to Follower Kayetonas."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("severSouls", L("Defeat severed demons' souls"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 8542: Kenneth's Protector
//-----------------------------------------------------------------------------
public class Gele573Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8542);
		SetName(L("Kenneth's Protector"));
		SetDescription(L("Defeat the demons around Kenneth while he rests."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_KENNETH", "f_gele_57_3", L("Talk to Watcher Kenneth"), L("Watcher Kenneth is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_KENNETH", "f_gele_57_3", L("Defeat the demon monsters nearby Kenneth"), L("Defeat the nearby monsters while Kenneth rests."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Tell Follower Kayetonas about it."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("killDemons", L("Defeat demons"), new KillObjective(9, "puragi_green", "banshee", "zigri_brown"));

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8543: Bull Hunting
//-----------------------------------------------------------------------------
public class Gele573Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8543);
		SetName(L("Bull Hunting"));
		SetDescription(L("A gigantic demon monster is coming. Follow Kayetonas and defeat the Minotaur."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Follower Kayetonas is looking for someone who can fight with him."));
		SetPhase(QuestStatus.InProgress, "GELE573_KAROLINA", "f_gele_57_3", L("Defeat Minotaur"), L("Defeat the Minotaur before it reaches the Paladin Master."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Talk to Follower Kayetonas."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE573_MQ_06_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("killMinotaur", L("Defeat Minotaur"), new KillObjective(1, "boss_Minotaurs") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8544: Foreseen Crisis (1)
//-----------------------------------------------------------------------------
public class Gele573Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8544);
		SetName(L("Foreseen Crisis (1)"));
		SetDescription(L("The Paladin Master asks you to listen to Follower Algis about the Tenet Church."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("The Paladin Master is waiting for you at Uzbaiga Hillside."));
		SetPhase(QuestStatus.InProgress, "GELE573_MQ_07_F", "f_gele_57_3", L("Listen to Follower Algis' explanations"), L("Listen to Follower Algis before he leaves."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("Report to the Paladin Master."));

		AddPrerequisite(new QuestStatusPrerequisite(17200, QuestStatus.Completed));

		AddObjective("hearAlgis", L("Listen to Follower Algis' explanations"), new ManualObjective());
	}
}

// 8545: Foreseen Crisis (3)
//-----------------------------------------------------------------------------
public class Gele573Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8545);
		SetName(L("Foreseen Crisis (3)"));
		SetDescription(L("The Paladin Master is looking for you. Gesti has appeared."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("The Paladin Master is looking for you."));
		SetPhase(QuestStatus.InProgress, "GELE573_MASTER", "f_gele_57_3", L("Help the Paladin Master"), L("Gesti appeared. Help the Paladin Master."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Report to the Paladin Master"), L("Talk to the Paladin Master."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE573_MQ_09_AFTER", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(8546, QuestStatus.Completed));

		AddObjective("helpMaster", L("Help the Paladin Master"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new SelectItemReward("TOP02_160", "TOP02_161", "TOP02_162"));
	}
}

// 8546: Foreseen Crisis (2)
//-----------------------------------------------------------------------------
public class Gele573Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8546);
		SetName(L("Foreseen Crisis (2)"));
		SetDescription(L("Clear the area while the Paladin Master focuses on the Divine Sphere."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("The Paladin Master is waiting for your help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MASTER", "f_gele_57_3", L("Defeat Throneweaver"), L("Defeat the Throneweaver coming to attack."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("Report to the Paladin Master."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE573_MQ_09_TRACK", 10000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8544, QuestStatus.Completed));

		AddObjective("killThroneweaver", L("Defeat Throneweaver"), new KillObjective(1, "boss_Throneweaver_Q1") { LayerOnly = true });

		AddReward(new ItemReward("R_TOP02_118", 1));
	}
}

// 9102: Proving skills
//-----------------------------------------------------------------------------
public class Gele573Hq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9102);
		SetName(L("Proving skills"));
		SetDescription(L("Watcher James bets you cannot cut down forty monsters in the Owl Burial Ground."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3", "f_katyn_7_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE_57_3_HQ01_NPC01", "f_gele_57_3", L("Talk to Watcher James"), L("James is looking at your bag. Talk to him."));
		SetPhase(QuestStatus.InProgress, "GELE_57_3_HQ01_NPC01", "f_katyn_7_2", L("Defeat the monsters roaming around the Owl Burial Ground"), L("Defeat forty monsters in the Owl Burial Ground."));
		SetPhase(QuestStatus.Success, "GELE_57_3_HQ01_NPC01", "f_gele_57_3", L("Talk to James"), L("You've completed the bet with James. Talk to him."));

		// The gate was a script with no recoverable logic; the level band stands in.
		AddPrerequisite(new LevelPrerequisite(27));

		AddObjective("killOwlGround", L("Defeat the monsters in Owl Burial Ground"), new KillObjective(40, "ellomago", "Ridimed", "jellyfish_red", "Sakmoli"));
	}
}

// 9104: The One Who Experienced Death
//-----------------------------------------------------------------------------
public class Gele573Hq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9104);
		SetName(L("The One Who Experienced Death"));
		SetDescription(L("A monster is slaying people in the Tenet Church. Hunt the Chapparition."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("The Paladin Master has something to say. Talk to him."));
		SetPhase(QuestStatus.InProgress, "GELE573_MASTER", "f_gele_57_3", L("Defeat Field Boss Chapparition"), L("Find and defeat the spooky Chapparition in the Tenet Church."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Report to the Paladin Master"), L("Report to the Paladin Master."));

		// The gate was a script with no recoverable logic; the level band stands in.
		AddPrerequisite(new LevelPrerequisite(98));

		AddObjective("killChapparition", L("Defeat spooky Chapparition"), new KillObjective(1, "F_boss_Chapparition"));
	}
}

// 30031: The Hidden Sanctum's Revelation (2)
//-----------------------------------------------------------------------------
public class Chaple577Mq10AfterQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30031);
		SetName(L("The Hidden Sanctum's Revelation (2)"));
		SetDescription(L("Tell the Paladin Master the story of the Tenet Church."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Tell the Paladin Master about the story so far"), L("Go to the Paladin Master in Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MASTER", "f_gele_57_3", L("Tell the Paladin Master about the story so far"), L("Tell the Paladin Master about the story so far."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Tell the Paladin Master about the story so far"), L("Tell the Paladin Master about the story so far."));

		AddPrerequisite(new QuestStatusPrerequisite(8537, QuestStatus.Completed));

		AddObjective("tellStory", L("Tell the Paladin Master about the story so far"), new ManualObjective());
	}
}
